using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using Newtonsoft.Json;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using tms_mka_v2.Security;
using tms_mka_v2.Infrastructure;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace tms_mka_v2.Controllers
{
    public class DataTruckController : BaseController
    {
        private IDataTruckRepo RepoDataTruck;
        private IMasterPoolRepo MasterPool;
        private IJenisTruckRepo JenisTruck;
        private IDataBoxRepo RepoBox;
        private IDataGPSRepo RepoGps;
        private IDataPendinginRepo RepoPendingin;
        private IPenetapanaDriverRepo RepoPenetapan;
        private ISalesOrderRepo RepoSalesOrder;
        private IHistoryJalanTruckRepo RepoHistoryJalanTruck;

        public DataTruckController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDataTruckRepo repoDataTruck, IMasterPoolRepo masterPool, IJenisTruckRepo jenisTruck,
            IDataPendinginRepo repoPendingin, IDataGPSRepo repoGps, IDataBoxRepo repoBox, IPenetapanaDriverRepo repoPenetapan, ISalesOrderRepo repoSalesOrder, IHistoryJalanTruckRepo repoHistoryJalanTruck)
            : base(repoBase, repoLookup)
        {
            RepoDataTruck = repoDataTruck;
            MasterPool = masterPool;
            JenisTruck = jenisTruck;
            RepoPendingin = repoPendingin;
            RepoGps = repoGps;
            RepoBox = repoBox;
            RepoPenetapan = repoPenetapan;
            RepoSalesOrder = repoSalesOrder;
            RepoHistoryJalanTruck = repoHistoryJalanTruck;
        }

        public string CheckArea(int id)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindAll().Where(d => 
                (d.Status == "save" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                    (d.SalesOrderOncallId.HasValue && d.SalesOrderOncall.IdDataTruck == id) || (d.SalesOrderPickupId.HasValue && d.SalesOrderPickup.IdDataTruck == id) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue && d.SalesOrderProsesKonsolidasi.IdDataTruck == id) ||
                    (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.DataTruckId == id))
                )
            ).FirstOrDefault();
            if (dbso.SalesOrderOncallId.HasValue)
                return new JavaScriptSerializer().Serialize(new { area = RepoSalesOrder.FindArea(dbso.SalesOrderOncall.IdDaftarHargaItem.Value)});
            return null;
        }

        [MyAuthorize(Menu = "Data Truk", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DataTruck").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DataTruck> items = RepoDataTruck.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            
            List<DataTruck> ListModel = new List<DataTruck>();
            foreach (Context.DataTruck item in items)
            {
                ListModel.Add(new DataTruck(item));
            }

            int total = RepoDataTruck.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingDetail(int IdTruck, int idSo = 0)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            if (param.Filters.Filters == null)
            {
                param.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
                param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo{Field = "IdJenisTruck", Operator = "eq", Value = IdTruck.ToString()});
            }
            List<DataTruckDetail> ListModel = new List<DataTruckDetail>();

            List<Context.DataTruck> items = RepoDataTruck.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            foreach (Context.DataTruck item in items)
            {
                ListModel.Add(new DataTruckDetail(item));
            }

            GridRequestParameters param2 = GridRequestParameters.Current;
            if (param2.Filters.Filters == null)
            {
                param2.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
                param2.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo{Field = "IdJenisTruck", Operator = "neq", Value = IdTruck.ToString()});
            }

            items = RepoDataTruck.FindAll(param2.Skip, param2.Take, (param2.Sortings != null ? param2.Sortings.ToList() : null), param2.Filters);
            foreach (Context.DataTruck item in items)
            {
                ListModel.Add(new DataTruckDetail(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
        public string BindingDetailAll()
        {
            List<DataTruckDetail> ListModel = new List<DataTruckDetail>();

            List<Context.DataTruck> items = RepoDataTruck.FindAll();
            foreach (Context.DataTruck item in items)
            {
                ListModel.Add(new DataTruckDetail(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
        public string GetHistoryJalan(int id)
        {
            List<Context.HistoryJalanTruck> dbitem = RepoHistoryJalanTruck.FindByTruck(id);

            return new JavaScriptSerializer().Serialize(dbitem.Select(d => new { 
                idDriver1 = d.IdDriver1,
                idDriver2 = d.IdDriver2,
                driver1 = d.Driver1.NamaDriver,
                driver2 = d.IdDriver2.HasValue ? d.Driver2.NamaDriver : "",
                customer = d.IdCustomer.HasValue ? d.Customer.CustomerNama : "",
                rute = d.Rute,
                tglMuat = d.TanggalMuat,
            }));
        }
        [MyAuthorize(Menu = "Data Truk", Action="create")]
        public ActionResult Add()
        {
            DataTruck model = new DataTruck();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DataTruck model)
        {
            if (ModelState.IsValid)
            {
                if (RepoDataTruck.IsExist(model.VehicleNo))
                {
                    ModelState.AddModelError("VehicleNo", "Vehicle No sudah terdaftar.");
                    return View("Form", model);
                }
                Context.DataTruck dbitem = new Context.DataTruck();
                model.SetDb(dbitem);
                //generate code
                dbitem.urutan = RepoDataTruck.getUrutan() + 1;
                dbitem.NoTruck = RepoDataTruck.generateCode(dbitem.urutan);

                RepoDataTruck.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Data Truk", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.DataTruck dbitem = RepoDataTruck.FindByPK(id);
            DataTruck model = new DataTruck(dbitem);
            ViewBag.name = model.NoTruck;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(DataTruck model)
        {
            if (ModelState.IsValid)
            {
                if (RepoDataTruck.IsExist(model.VehicleNo, model.Id))
                {
                    ModelState.AddModelError("VehicleNo", "Vehicle No sudah terdaftar.");
                    return View("Form", model);
                }
                Context.DataTruck dbitem = RepoDataTruck.FindByPK(model.Id);
                model.SetDb(dbitem);
                RepoDataTruck.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            if (RepoDataTruck.IsValidDelete(id))
            {
                Context.DataTruck dbItem = RepoDataTruck.FindByPK(id);

                RepoDataTruck.delete(dbItem, UserPrincipal.id);
            }
            else
            {
                response.SetFail("Data truck terpasang Box,Gps atau Pendingin, data tidak dapat dihapus.");
            }

            return Json(response);
        }
        #region options
        public string getTruck(string type, int id = 0)
        {
            List<Context.DataTruck> ListExist = new List<Context.DataTruck>();
            if (type == "AC")
            {
                ListExist = RepoPendingin.FindAll().Select(d => d.DataTruk).ToList();
            }
            else if(type == "GPS")
            {
                ListExist = RepoGps.FindAll().Select(d => d.DataTruck).ToList();
            }
            else if(type == "BOX")
            {
                ListExist = RepoBox.FindAll().Select(d => d.DataTruck).ToList();            
            }

            List<DataTruck> listmodel = new List<DataTruck>();
            foreach (Context.DataTruck item in RepoDataTruck.FindAll().Where(d => !ListExist.Select(c => c.Id).Contains(d.Id) || d.Id == id))
            {
                listmodel.Add(new DataTruck(item));
            }
            return new JavaScriptSerializer().Serialize(listmodel);

        }
        public string GetTruckById(int id)
        {
            return new JavaScriptSerializer().Serialize(new DataTruck(RepoDataTruck.FindByPK(id)));
        }
        public string GetAlert(int id)
        {
            DataTruck model = new DataTruck(RepoDataTruck.FindByPK(id));

            List<DataTruckAlert> alert = new List<DataTruckAlert>();

            if (model.STNK.HasValue)
            {
                if (model.STNK <= DateTime.Now)
                {
                    alert.Add(new DataTruckAlert() { Keterangan = "STNK Expired", Value = model.STNK.Value.ToShortDateString() });
                }
            }
            if (model.KIR.HasValue)
            {
                if (model.KIR <= DateTime.Now)
                {
                    alert.Add(new DataTruckAlert() { Keterangan = "KIR Expired", Value = model.KIR.Value.ToShortDateString() });
                }
            }
            if (model.KIU.HasValue)
            {
                if (model.KIU <= DateTime.Now)
                {
                    alert.Add(new DataTruckAlert() { Keterangan = "KIU Expired", Value = model.KIU.Value.ToShortDateString() });
                }
            }
            if (model.IBM.HasValue)
            {
                if (model.IBM <= DateTime.Now)
                {
                    alert.Add(new DataTruckAlert() { Keterangan = "IBM Expired", Value = model.IBM.Value.ToShortDateString() });
                }
            }
            if (model.Asuransi.HasValue)
            {
                if (model.Asuransi <= DateTime.Now)
                {
                    alert.Add(new DataTruckAlert() { Keterangan = "Asuransi Expired", Value = model.Asuransi.Value.ToShortDateString() });
                }
            }
            if (model.Reklame.HasValue)
            {
                if (model.Reklame <= DateTime.Now)
                {
                    alert.Add(new DataTruckAlert() { Keterangan = "Reklame Expired", Value = model.Reklame.Value.ToShortDateString() });
                }
            }
            
            return new JavaScriptSerializer().Serialize(alert);
        }
        #endregion

        #region import export
        public string UploadDataTruck(IEnumerable<HttpPostedFileBase> filesDataTruck)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesDataTruck != null)
            {
                foreach (var file in filesDataTruck)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && RepoLookup.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()) != null)
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 38].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 38].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    Context.DataTruck dbitem = new Context.DataTruck();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoDataTruck.FindByPK(id);
                                            if (RepoDataTruck.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString(), id))
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (RepoDataTruck.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString()))
                                            {
                                                continue;
                                            }
                                            dbitem.urutan = RepoDataTruck.getUrutan() + 1;
                                            dbitem.NoTruck = RepoDataTruck.generateCode(dbitem.urutan);
                                        }

                                        dbitem.VehicleNo = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        if (workSheet.Cells[rowIterator, 2].Value != null)
                                            dbitem.IdMerk = RepoLookup.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 3].Value != null)
                                            dbitem.IdJenisTruck = JenisTruck.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 4].Value != null)
                                            dbitem.TahunBuat = int.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                        if (workSheet.Cells[rowIterator, 5].Value != null)
                                            dbitem.TahunBeli = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        if (workSheet.Cells[rowIterator, 6].Value != null)
                                            dbitem.IdPool = MasterPool.FindByNamePool(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 7].Value != null && RepoLookup.FindByName(workSheet.Cells[rowIterator, 7].Value.ToString()) != null)
                                            dbitem.IdUnit = RepoLookup.FindByName(workSheet.Cells[rowIterator, 7].Value.ToString()).Id;
                                        dbitem.Keterangan = workSheet.Cells[rowIterator, 8].Value != null ? workSheet.Cells[rowIterator, 8].Value.ToString() : "";
                                        dbitem.Kondisi = workSheet.Cells[rowIterator, 9].Value != null ? workSheet.Cells[rowIterator, 9].Value.ToString() : "";
                                        dbitem.SpecModel = workSheet.Cells[rowIterator, 10].Value != null ? workSheet.Cells[rowIterator, 10].Value.ToString() : "";
                                        dbitem.KmLimit = workSheet.Cells[rowIterator, 11].Value != null ? int.Parse(workSheet.Cells[rowIterator, 11].Value.ToString()) : 0;
                                        dbitem.NoMesin = workSheet.Cells[rowIterator, 12].Value != null ? workSheet.Cells[rowIterator, 12].Value.ToString() : "";
                                        dbitem.NoRangka = workSheet.Cells[rowIterator, 13].Value != null ? workSheet.Cells[rowIterator, 13].Value.ToString() : "";
                                        if (workSheet.Cells[rowIterator, 14].Value != null)
                                            dbitem.GaransiStr = DateTime.Parse(workSheet.Cells[rowIterator, 14].Value.ToString());
                                        if (workSheet.Cells[rowIterator, 15].Value != null)
                                            dbitem.GaransiEnd = DateTime.Parse(workSheet.Cells[rowIterator, 15].Value.ToString());
                                        dbitem.SpecKeterangan = workSheet.Cells[rowIterator, 16].Value != null ? workSheet.Cells[rowIterator, 16].Value.ToString() : "";
                                        dbitem.AtasNama = workSheet.Cells[rowIterator, 17].Value != null ? workSheet.Cells[rowIterator, 17].Value.ToString() : "";
                                        dbitem.BPKB = workSheet.Cells[rowIterator, 18].Value != null ? workSheet.Cells[rowIterator, 18].Value.ToString() : "";
                                        dbitem.keteranganBPKB = workSheet.Cells[rowIterator, 19].Value != null ? workSheet.Cells[rowIterator, 19].Value.ToString() : "";
                                        if (workSheet.Cells[rowIterator, 20].Value != null)
                                            dbitem.STNK = DateTime.Parse(workSheet.Cells[rowIterator, 20].Value.ToString());
                                        dbitem.keteranganSTNK = workSheet.Cells[rowIterator, 21].Value != null ? workSheet.Cells[rowIterator, 21].Value.ToString() : "";
                                        if (workSheet.Cells[rowIterator, 22].Value != null)
                                        dbitem.KIR = DateTime.Parse(workSheet.Cells[rowIterator, 22].Value.ToString());
                                        dbitem.keteranganKIR = workSheet.Cells[rowIterator, 23].Value != null ? workSheet.Cells[rowIterator, 23].Value.ToString() : "";
                                        if (workSheet.Cells[rowIterator, 24].Value != null)
                                            dbitem.KIU = DateTime.Parse(workSheet.Cells[rowIterator, 24].Value.ToString());
                                        dbitem.keteranganKIU = workSheet.Cells[rowIterator, 25].Value != null ? workSheet.Cells[rowIterator, 25].Value.ToString() : "";
                                        if (workSheet.Cells[rowIterator, 26].Value != null)
                                            dbitem.IBM = DateTime.Parse(workSheet.Cells[rowIterator, 26].Value.ToString());
                                        dbitem.keteranganIBM = workSheet.Cells[rowIterator, 27].Value != null ? workSheet.Cells[rowIterator, 27].Value.ToString() : "";
                                        if (workSheet.Cells[rowIterator, 28].Value != null)
                                            dbitem.Asuransi = DateTime.Parse(workSheet.Cells[rowIterator, 28].Value.ToString());
                                        dbitem.keteranganAsuransi = workSheet.Cells[rowIterator, 29].Value != null ? workSheet.Cells[rowIterator, 29].Value.ToString() : null;
                                        if(workSheet.Cells[rowIterator, 30].Value != null){
                                            dbitem.Reklame =  DateTime.Parse(workSheet.Cells[rowIterator, 30].Value.ToString());
                                        }else{
                                            dbitem.Reklame = null;
                                        }
                                        dbitem.keteranganReklame = workSheet.Cells[rowIterator, 31].Value != null ? workSheet.Cells[rowIterator, 31].Value.ToString() : "";
                                        dbitem.NoPolis = workSheet.Cells[rowIterator, 32].Value != null ? workSheet.Cells[rowIterator, 32].Value.ToString() : null;
                                        dbitem.keteranganNoPolis = workSheet.Cells[rowIterator, 33].Value != null ? workSheet.Cells[rowIterator, 33].Value.ToString() : "";
                                        dbitem.Peminjam = workSheet.Cells[rowIterator, 34].Value != null ? workSheet.Cells[rowIterator, 34].Value.ToString() : null;
                                        dbitem.keteranganPeminjam = workSheet.Cells[rowIterator, 35].Value != null ? workSheet.Cells[rowIterator, 35].Value.ToString() : "";
                                        dbitem.Leasing = workSheet.Cells[rowIterator, 36].Value != null ? workSheet.Cells[rowIterator, 36].Value.ToString() : null;
                                        dbitem.keteranganLeasing = workSheet.Cells[rowIterator, 37].Value != null ? workSheet.Cells[rowIterator, 37].Value.ToString() : "";

                                        RepoDataTruck.save(dbitem, UserPrincipal.id);
                                    }
                                    catch (Exception)
                                    {
                                        response.Message = response.Message + Environment.NewLine + " " + rowIterator;
                                    }
                                }
                            }
                        }
                        response.Success = true;
                    }
                    catch (Exception e)
                    {
                        response.Success = false;
                        response.Message = e.Message.ToString();
                    }

                }
            }

            return new JavaScriptSerializer().Serialize(new { Response = response });
        }

        [MyAuthorize(Menu = "Data Truk", Action="read")]
        public FileContentResult ExportDataTruck()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.DataTruck> dbitems = RepoDataTruck.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Vehicle No";
            ws.Cells[1, 2].Value = "Merk Truk";
            ws.Cells[1, 3].Value = "Jenis Truk";
            ws.Cells[1, 4].Value = "Tahun Pembuatan";
            ws.Cells[1, 5].Value = "Tahun Pembelian";
            ws.Cells[1, 6].Value = "Alokasi Pool";
            ws.Cells[1, 7].Value = "Alokasi Unit";
            ws.Cells[1, 8].Value = "Keterangan";
            ws.Cells[1, 9].Value = "Kondisi Khusus";
            ws.Cells[1, 10].Value = "Model";
            ws.Cells[1, 11].Value = "KM Limit";
            ws.Cells[1, 12].Value = "No Mesin";
            ws.Cells[1, 13].Value = "No Rangka";
            ws.Cells[1, 14].Value = "Tanggal Mulai Garansi";
            ws.Cells[1, 15].Value = "Tanggal Akhir Garansi";
            ws.Cells[1, 16].Value = "Keterangan Spec";
            ws.Cells[1, 17].Value = "Atas Nama";
            ws.Cells[1, 18].Value = "BPKB";
            ws.Cells[1, 19].Value = "Keterangan BPKB";
            ws.Cells[1, 20].Value = "Masa Berlaku STNK";
            ws.Cells[1, 21].Value = "Keterangan STNK";
            ws.Cells[1, 22].Value = "Masa Berlaku KIR";
            ws.Cells[1, 23].Value = "Keterangan KIR";
            ws.Cells[1, 24].Value = "Masa Berlaku KIU/SIPA";
            ws.Cells[1, 25].Value = "Keterangan KIU/SIPA";
            ws.Cells[1, 26].Value = "Masa Berlaku IBM";
            ws.Cells[1, 27].Value = "Keterangan IBM";
            ws.Cells[1, 28].Value = "Masa Berlaku Asuransi";
            ws.Cells[1, 29].Value = "Keterangan Asuransi";
            ws.Cells[1, 30].Value = "Masa Berlaku Pajak Reklame";
            ws.Cells[1, 31].Value = "Keterangan Pajak Reklame";
            ws.Cells[1, 32].Value = "No Polis";
            ws.Cells[1, 33].Value = "Keterangan No Polis";
            ws.Cells[1, 34].Value = "Penjamin";
            ws.Cells[1, 35].Value = "Keterangan Penjamin";
            ws.Cells[1, 36].Value = "Leasing";
            ws.Cells[1, 37].Value = "Keterangan Leasing";
            ws.Cells[1, 38].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].VehicleNo;
                ws.Cells[i + 2, 2].Value = dbitems[i].LookupCodeMerk == null ? "" : dbitems[i].LookupCodeMerk.Nama;
                ws.Cells[i + 2, 3].Value = dbitems[i].JenisTrucks == null ? "" : dbitems[i].JenisTrucks.StrJenisTruck;
                ws.Cells[i + 2, 4].Value = dbitems[i].TahunBuat;
                ws.Cells[i + 2, 5].Value = dbitems[i].TahunBeli;
                ws.Cells[i + 2, 6].Value = dbitems[i].MasterPool == null ? "" : dbitems[i].MasterPool.NamePool;
                ws.Cells[i + 2, 7].Value = dbitems[i].LookupCodeUnit == null ? "" : dbitems[i].LookupCodeUnit.Nama;
                ws.Cells[i + 2, 8].Value = dbitems[i].Keterangan;
                ws.Cells[i + 2, 9].Value = dbitems[i].Kondisi;
                ws.Cells[i + 2, 10].Value = dbitems[i].SpecModel;
                ws.Cells[i + 2, 11].Value = dbitems[i].KmLimit;
                ws.Cells[i + 2, 12].Value = dbitems[i].NoMesin;
                ws.Cells[i + 2, 13].Value = dbitems[i].NoRangka;
                ws.Cells[i + 2, 14].Value = Convert.ToDateTime(dbitems[i].GaransiStr).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 15].Value = Convert.ToDateTime(dbitems[i].GaransiEnd).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 16].Value = dbitems[i].SpecKeterangan;
                ws.Cells[i + 2, 17].Value = dbitems[i].AtasNama;
                ws.Cells[i + 2, 18].Value = dbitems[i].BPKB;
                ws.Cells[i + 2, 19].Value = dbitems[i].keteranganBPKB;
                ws.Cells[i + 2, 20].Value = Convert.ToDateTime(dbitems[i].STNK).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 21].Value = dbitems[i].keteranganSTNK;
                ws.Cells[i + 2, 22].Value = Convert.ToDateTime(dbitems[i].KIR).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 23].Value = dbitems[i].keteranganKIR;
                ws.Cells[i + 2, 24].Value = Convert.ToDateTime(dbitems[i].KIU).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 25].Value = dbitems[i].keteranganKIU;
                ws.Cells[i + 2, 26].Value = Convert.ToDateTime(dbitems[i].IBM).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 27].Value = dbitems[i].keteranganIBM;
                ws.Cells[i + 2, 28].Value = Convert.ToDateTime(dbitems[i].Asuransi).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 29].Value = dbitems[i].keteranganAsuransi;
                ws.Cells[i + 2, 30].Value = Convert.ToDateTime(dbitems[i].Reklame).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 31].Value = dbitems[i].keteranganReklame;
                ws.Cells[i + 2, 32].Value = dbitems[i].NoPolis;
                ws.Cells[i + 2, 33].Value = dbitems[i].keteranganNoPolis;
                ws.Cells[i + 2, 34].Value = dbitems[i].Peminjam;
                ws.Cells[i + 2, 35].Value = dbitems[i].keteranganPeminjam;
                ws.Cells[i + 2, 36].Value = dbitems[i].Leasing;
                ws.Cells[i + 2, 37].Value = dbitems[i].keteranganLeasing;
                ws.Cells[i + 2, 38].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Data_Truck.xls";

            return fsr;
        }
        #endregion
    }
}