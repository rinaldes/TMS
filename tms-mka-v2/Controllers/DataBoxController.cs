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
    public class DataBoxController : BaseController
    {
        private IDataBoxRepo RepoDataBox;
        private IDataTruckRepo RepoDataTruck;

        public DataBoxController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDataBoxRepo repoDataBox, IDataTruckRepo repoDataTruck)
            : base(repoBase, repoLookup)
        {
            RepoDataBox = repoDataBox;
            RepoDataTruck = repoDataTruck;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DataBox").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DataBox> items = RepoDataBox.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<DataBox> ListModel = new List<DataBox>();
            foreach (Context.DataBox item in items)
            {
                ListModel.Add(new DataBox(item));
            }

            int total = RepoDataBox.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingHistory(int id)
        {
            Context.DataBox db = RepoDataBox.FindByPK(id);

            List<DataBox> listmodel = new List<DataBox>();

            foreach (Context.DataBoxHistory item in db.DataBoxHistory.OrderByDescending(d => d.Id))
            {
                listmodel.Add(new DataBox(item));
            }
            return new JavaScriptSerializer().Serialize(listmodel);
        }
        public void GenerateLantaiDinding(DataBox model)
        {
            model.ListLantai = new List<LantaiModel>();
            foreach (Context.LookupCode item in GetListLantaiBox())
            {
                model.ListLantai.Add(new LantaiModel(item));
            }
            model.ListDinding = new List<DindingModel>();
            foreach (Context.LookupCode item in GetListDindingBox())
            {
                model.ListDinding.Add(new DindingModel(item));
            }
        }
        public void SetLantaiDinding(DataBox model, Context.DataBox dbitem)
        {
            foreach (Context.DataBoxLantai item in dbitem.DataBoxLantai)
            {
                model.ListLantai.Where(d => d.IdLantai == item.IdLantaiCode).First().IsSelect = true;
            }
            foreach (Context.DataBoxDinding item in dbitem.DataBoxDinding)
            {
                model.ListDinding.Where(d => d.IdDinding == item.IdDindingCode).First().IsSelect = true;
            }
        }
        public ActionResult Add()
        {
            DataBox model = new DataBox();
            GenerateLantaiDinding(model);
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DataBox model)
        {
            if (ModelState.IsValid)
            {
                if (RepoDataBox.IsBoxExist(model.IdDataTruck.Value))
                {
                    ModelState.AddModelError("IdDataTruck", "Truck sudah tepasang box, harap ganti dengan truck yang lain");
                    return View("Form", model);
                }
                Context.DataBox dbitem = new Context.DataBox();
                model.SetDb(dbitem);
                //generate code
                dbitem.Urutan = RepoDataBox.getUrutan() + 1;
                dbitem.NoBox = RepoDataBox.generateCode(dbitem.Urutan);
                
                Context.DataBoxHistory dbitemHistory = new Context.DataBoxHistory();
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                dbitemHistory.Vehicle = RepoDataTruck.FindByPK(model.IdDataTruck.Value).VehicleNo;
                dbitemHistory.NoBox = dbitem.NoBox;
                dbitemHistory.strKategori = model.IdKategori.HasValue ? RepoLookup.FindByPK(model.IdKategori.Value).Nama : "";
                dbitemHistory.strType = model.IdType.HasValue ? RepoLookup.FindByPK(model.IdType.Value).Nama : "";
                dbitem.DataBoxHistory.Add(dbitemHistory);
                Context.DataTruckBoxHistory dbtruckHistory = new Context.DataTruckBoxHistory();
                model.SetDbTruckHistory(dbtruckHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                RepoDataBox.save(dbitem, UserPrincipal.id, dbitemHistory, dbtruckHistory);

                Context.DataTruck dbtruck = RepoDataTruck.FindByPK(model.IdDataTruck.Value);
                dbtruckHistory.Vehicle = dbtruck.VehicleNo;
                dbtruckHistory.NoBox = dbitem.NoBox;
                dbtruckHistory.strKategori = model.IdKategori.HasValue ? RepoLookup.FindByPK(model.IdKategori.Value).Nama : "";
                dbtruckHistory.strType = model.IdType.HasValue ? RepoLookup.FindByPK(model.IdType.Value).Nama : "";
                dbtruck.DataTruckBoxHistory.Add(dbtruckHistory);
                RepoDataTruck.save(dbtruck, UserPrincipal.id);
                

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.DataBox dbitem = RepoDataBox.FindByPK(id);
            DataBox model = new DataBox(dbitem);
            GenerateLantaiDinding(model);
            SetLantaiDinding(model, dbitem);
            ViewBag.name = model.NoBox;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(DataBox model)
        {
            if (ModelState.IsValid)
            {
                if (RepoDataBox.IsBoxExist(model.IdDataTruck.Value, model.Id))
                {
                    ModelState.AddModelError("IdDataTruck", "Truck sudah tepasang box, harap ganti dengan truck yang lain");
                    return View("Form", model);
                }
                Context.DataBox dbitem = RepoDataBox.FindByPK(model.Id);
                model.SetDb(dbitem);
                Context.DataBoxHistory dbitemHistory = new Context.DataBoxHistory();
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                dbitem.DataBoxHistory.Add(dbitemHistory);
                Context.DataTruck dbtruck = RepoDataTruck.FindByPK(model.IdDataTruck.Value);
                Context.DataTruckBoxHistory dbtruckHistory = new Context.DataTruckBoxHistory();
                RepoDataBox.save(dbitem, UserPrincipal.id, dbitemHistory, dbtruckHistory);

                model.SetDbTruckHistory(dbtruckHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                dbtruck.DataTruckBoxHistory.Add(dbtruckHistory);
                RepoDataTruck.save(dbtruck, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.DataBox dbItem = RepoDataBox.FindByPK(id);

            RepoDataBox.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        #region options

        #endregion

        #region export import
        public string UploadDataBox(IEnumerable<HttpPostedFileBase> filesDataBox)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesDataBox != null)
            {
                foreach (var file in filesDataBox)
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
                                if (workSheet.Cells[rowIterator, 1].Value != null /*&& workSheet.Cells[rowIterator, 2].Value != null &&
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null &&
                                    workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null &&
                                    workSheet.Cells[rowIterator, 9].Value != null && workSheet.Cells[rowIterator, 10].Value != null &&
                                    workSheet.Cells[rowIterator, 11].Value != null && workSheet.Cells[rowIterator, 12].Value != null &&
                                    workSheet.Cells[rowIterator, 13].Value != null && workSheet.Cells[rowIterator, 14].Value != null &&
                                    workSheet.Cells[rowIterator, 15].Value != null && workSheet.Cells[rowIterator, 16].Value != null*/)
                                {
                                    int id = 0;
                                    int resId;
                                    int temprow = rowIterator;
                                    if (workSheet.Cells[rowIterator, 17].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 17].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    Context.DataBox dbitem = new Context.DataBox();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            if (RepoDataBox.IsBoxExist(RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id, id))
                                            {
                                                continue;
                                            }
                                            dbitem = RepoDataBox.FindByPK(id);
                                            dbitem.DataBoxLantai.Clear();
                                            dbitem.DataBoxDinding.Clear();
                                        }
                                        else
                                        {
                                            if (RepoDataBox.IsBoxExist(RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id))
                                            {
                                                continue;
                                            }
                                            dbitem.Urutan = RepoDataBox.getUrutan() + 1;
                                            dbitem.NoBox = RepoDataBox.generateCode(dbitem.Urutan);
                                        }
                                        dbitem.IdDataTruck = RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        dbitem.Karoseri = workSheet.Cells[rowIterator, 2].Value == null ? null : workSheet.Cells[rowIterator, 2].Value.ToString();
                                        dbitem.Tahun = workSheet.Cells[rowIterator, 3].Value == null ? (int?)null : int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                        dbitem.IdType = workSheet.Cells[rowIterator, 4].Value == null ? (int?)null : RepoLookup.FindByName(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                                        dbitem.IdKategori = workSheet.Cells[rowIterator, 5].Value == null ? (int?)null : RepoLookup.FindByName(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                                        dbitem.tglPasang = workSheet.Cells[rowIterator, 6].Value == null ? (DateTime?)null : DateTime.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                                        dbitem.Lantai = workSheet.Cells[rowIterator, 7].Value == null ? null : workSheet.Cells[rowIterator, 7].Value.ToString();
                                        dbitem.Dinding = workSheet.Cells[rowIterator, 9].Value == null ? null : workSheet.Cells[rowIterator, 9].Value.ToString();
                                        dbitem.PintuSamping = workSheet.Cells[rowIterator, 11].Value == null ? (bool?)null : bool.Parse(workSheet.Cells[rowIterator, 11].Value.ToString());
                                        dbitem.Sekat = workSheet.Cells[rowIterator, 12].Value == null ? (bool?)null : bool.Parse(workSheet.Cells[rowIterator, 12].Value.ToString());
                                        dbitem.garansiStr = workSheet.Cells[rowIterator, 13].Value == null ? (DateTime?)null : DateTime.Parse(workSheet.Cells[rowIterator, 13].Value.ToString());
                                        dbitem.garansiEnd = workSheet.Cells[rowIterator, 14].Value == null ? (DateTime?)null : DateTime.Parse(workSheet.Cells[rowIterator, 14].Value.ToString());
                                        dbitem.asuransiStr = workSheet.Cells[rowIterator, 15].Value == null ? (DateTime?)null : DateTime.Parse(workSheet.Cells[rowIterator, 15].Value.ToString());
                                        dbitem.asuransiEnd = workSheet.Cells[rowIterator, 16].Value == null ? (DateTime?)null : DateTime.Parse(workSheet.Cells[rowIterator, 16].Value.ToString());
                                        //lantai
                                        int idx1 = 0;
                                        for (idx1 = rowIterator; idx1 <= noOfRow; idx1++)
                                        {
                                            if (workSheet.Cells[idx1, 1].Value == null || idx1 == rowIterator)
                                            {
                                                if (workSheet.Cells[idx1, 8].Value != null)
                                                {
                                                    Context.DataBoxLantai item = new Context.DataBoxLantai();
                                                    item.IdLantaiCode = RepoLookup.FindByName(workSheet.Cells[idx1, 8].Value.ToString()).Id;
                                                    dbitem.DataBoxLantai.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        //dinding
                                        int idx2 = 0;
                                        for (idx2 = rowIterator; idx2 <= noOfRow; idx2++)
                                        {
                                            if (workSheet.Cells[idx2, 1].Value == null || idx2 == rowIterator)
                                            {
                                                if (workSheet.Cells[idx2, 10].Value != null)
                                                {
                                                    Context.DataBoxDinding item = new Context.DataBoxDinding();
                                                    item.IdDindingCode = RepoLookup.FindByName(workSheet.Cells[idx2, 10].Value.ToString()).Id;
                                                    dbitem.DataBoxDinding.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        if (idx1 > idx2)
                                            rowIterator = idx1 - 1;
                                        else
                                            rowIterator = idx2 - 1;

                                        //history
                                        Context.DataBoxHistory dbhistory = new Context.DataBoxHistory();
                                        dbhistory.NoBox = dbitem.NoBox;
                                        dbhistory.Vehicle = workSheet.Cells[rowIterator, 1].Value == null ? null : workSheet.Cells[temprow, 1].Value.ToString();
                                        dbhistory.Karoseri = dbitem.Karoseri;
                                        dbhistory.Tahun = dbitem.Tahun;
                                        dbhistory.strType = workSheet.Cells[rowIterator, 4].Value == null ? null : workSheet.Cells[temprow, 4].Value.ToString();
                                        dbhistory.strKategori = workSheet.Cells[rowIterator, 5].Value == null ? null : workSheet.Cells[temprow, 5].Value.ToString();
                                        dbhistory.Lantai = dbitem.Lantai;
                                        dbhistory.Dinding = dbitem.Dinding;
                                        dbhistory.PintuSamping = dbitem.PintuSamping;
                                        dbhistory.Sekat = dbitem.Sekat;
                                        dbhistory.garansiStr = dbitem.garansiStr;
                                        dbhistory.garansiEnd = dbitem.garansiEnd;
                                        dbhistory.asuransiStr = dbitem.asuransiStr;
                                        dbhistory.asuransiEnd = dbitem.asuransiEnd;
                                        dbhistory.tglPasang = dbitem.tglPasang;
                                        dbhistory.Tanggal = DateTime.Now;
                                        dbhistory.username = UserPrincipal.firstname + " " + UserPrincipal.lastname;

                                        //foreach (LantaiModel item in )
                                        //{
                                        //    if (item.IsSelect)
                                        //    {
                                        //        dbitem.Lantai = dbitem.Lantai + ", " + item.StrLantai;
                                        //    }
                                        //}
                                        //foreach (DindingModel item in ListDinding)
                                        //{
                                        //    if (item.IsSelect)
                                        //    {
                                        //        dbitem.Dinding = dbitem.Dinding + ", " + item.StrDinding;
                                        //    }
                                        //}

                                        dbitem.DataBoxHistory.Add(dbhistory);

                                        RepoDataBox.save(dbitem, UserPrincipal.id, dbhistory);
                                    }
                                    catch (Exception)
                                    {

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


        public FileContentResult ExportDataBox()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.DataBox> dbitems = RepoDataBox.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");
            //ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Sheet 2");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Vehicle No";
            ws.Cells[1, 2].Value = "Karoseri";
            ws.Cells[1, 3].Value = "Tahun";
            ws.Cells[1, 4].Value = "Type";
            ws.Cells[1, 5].Value = "Kategori";
            ws.Cells[1, 6].Value = "Tanggal Pasang";
            ws.Cells[1, 7].Value = "Type Lantai";
            ws.Cells[1, 8].Value = "Lantai";
            ws.Cells[1, 9].Value = "Type Dinding";
            ws.Cells[1, 10].Value = "Dinding";
            ws.Cells[1, 11].Value = "Pintu Samping";
            ws.Cells[1, 12].Value = "Sekat";
            ws.Cells[1, 13].Value = "Garansi Start";
            ws.Cells[1, 14].Value = "Garansi End";
            ws.Cells[1, 15].Value = "Asuransi Start";
            ws.Cells[1, 16].Value = "Asuransi End";
            ws.Cells[1, 17].Value = "Id Database";
            
            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[idx, 1].Value = dbitems[i].DataTruck == null ? "" : dbitems[i].DataTruck.VehicleNo;
                ws.Cells[idx, 2].Value = dbitems[i].Karoseri;
                ws.Cells[idx, 3].Value = dbitems[i].Tahun;
                ws.Cells[idx, 4].Value = dbitems[i].LookupCodeType == null ? "" : dbitems[i].LookupCodeType.Nama;
                ws.Cells[idx, 5].Value = dbitems[i].LookupCodeKategori == null ? "" : dbitems[i].LookupCodeKategori.Nama;
                ws.Cells[idx, 6].Value = Convert.ToDateTime(dbitems[i].tglPasang).ToString("dd/MM/yyyy");
                ws.Cells[idx, 7].Value = dbitems[i].Lantai;
                ws.Cells[idx, 9].Value = dbitems[i].Dinding;
                ws.Cells[idx, 11].Value = dbitems[i].PintuSamping;
                ws.Cells[idx, 12].Value = dbitems[i].Sekat;
                ws.Cells[idx, 13].Value = Convert.ToDateTime(dbitems[i].garansiStr).ToString("dd/MM/yyyy");
                ws.Cells[idx, 14].Value = Convert.ToDateTime(dbitems[i].garansiEnd).ToString("dd/MM/yyyy");
                ws.Cells[idx, 15].Value = Convert.ToDateTime(dbitems[i].asuransiStr).ToString("dd/MM/yyyy");
                ws.Cells[idx, 16].Value = Convert.ToDateTime(dbitems[i].asuransiEnd).ToString("dd/MM/yyyy");
                ws.Cells[idx, 17].Value = dbitems[i].Id;
                int i1 = idx;
                foreach (Context.DataBoxLantai item in dbitems[i].DataBoxLantai)
                {
                    ws.Cells[i1, 8].Value = item.LookupCodeLantaiCode.Nama;
                    i1++;
                }
                int i2 = idx;
                foreach (Context.DataBoxDinding item in dbitems[i].DataBoxDinding)
                {
                    ws.Cells[i2, 10].Value = item.LookupCodeDinding.Nama;
                    i2++;
                }
                if (i1 > i2)
                    idx = i1;
                else
                    idx = i2;

                idx++;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Data_Box.xls";

            return fsr;
        }

        #endregion
    }
}