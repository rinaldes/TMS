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
    public class FaktorBoronganController : BaseController
    {
        private IFaktorBoronganRepo RepoFaktorBorongan;
        private IMasterPoolRepo RepoMasterPool;
        private IJenisTruckRepo RepoJenisTruck;
        public FaktorBoronganController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IFaktorBoronganRepo repoFaktorBorongan, IMasterPoolRepo repoMasterPool, IJenisTruckRepo repoJenisTruck)
            : base(repoBase, repoLookup)
        {
            RepoFaktorBorongan = repoFaktorBorongan;
            RepoJenisTruck = repoJenisTruck;
            RepoMasterPool = repoMasterPool;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "FaktorBorongan").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.FaktorBorongan> items = RepoFaktorBorongan.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<FaktorBorongan> ListModel = new List<FaktorBorongan>();
            foreach (Context.FaktorBorongan item in items)
            {
                ListModel.Add(new FaktorBorongan(item));
            }

            int total = RepoFaktorBorongan.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingHistory(int id)
        {
            Context.FaktorBorongan db = RepoFaktorBorongan.FindByPK(id);

            List<FaktorBoronganHistory> ListModel = new List<FaktorBoronganHistory>();

            foreach (Context.FaktorBoronganHistory item in db.FaktorBoronganHistory.ToList())
            {
                ListModel.Add(new FaktorBoronganHistory(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public ActionResult Add()
        {
            FaktorBorongan model = new FaktorBorongan();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(FaktorBorongan model)
        {
            if (ModelState.IsValid)
            {
                if (RepoFaktorBorongan.isExist(model.IdMasterPool.Value, model.IdJenisTruck.Value))
                {
                    ModelState.AddModelError("IdMasterPool", "Alokasi Pool dan Jenis Truk sudah terdaftar.");
                    ModelState.AddModelError("IdJenisTruck", "Alokasi Pool dan Jenis Truk sudah terdaftar.");
                    return View("Form", model);
                }
                Context.FaktorBorongan dbitem = new Context.FaktorBorongan();
                model.SetDb(dbitem);
                Context.FaktorBoronganHistory dbitemHistory = new Context.FaktorBoronganHistory();
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + ' ' + UserPrincipal.lastname);
                dbitem.FaktorBoronganHistory.Add(dbitemHistory);
                //generate code

                RepoFaktorBorongan.save(dbitem, UserPrincipal.id, dbitemHistory);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.FaktorBorongan dbitem = RepoFaktorBorongan.FindByPK(id);
            FaktorBorongan model = new FaktorBorongan(dbitem);
            ViewBag.name = model.IdJenisTruck;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(FaktorBorongan model)
        {
            if (ModelState.IsValid)
            {
                if (RepoFaktorBorongan.isExist(model.IdMasterPool.Value, model.IdJenisTruck.Value, model.Id))
                {
                    ModelState.AddModelError("IdMasterPool", "Alokasi Pool dan Jenis Truk sudah terdaftar.");
                    ModelState.AddModelError("IdJenisTruck", "Alokasi Pool dan Jenis Truk sudah terdaftar.");
                    return View("Form", model);
                }
                Context.FaktorBorongan dbitem = RepoFaktorBorongan.FindByPK(model.Id);
                model.SetDb(dbitem);
                Context.FaktorBoronganHistory dbitemHistory = new Context.FaktorBoronganHistory();
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + ' ' + UserPrincipal.lastname);
                dbitem.FaktorBoronganHistory.Add(dbitemHistory);
                RepoFaktorBorongan.save(dbitem, UserPrincipal.id, dbitemHistory);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.FaktorBorongan dbItem = RepoFaktorBorongan.FindByPK(id);

            RepoFaktorBorongan.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        #region option
        public string GetSolarRasio(int IdPool, int IdJenisTruck, string typeRasion)
        {
            ResponeModel response = new ResponeModel();
            Decimal rasio = 0 ;
            Context.FaktorBorongan dbitem = RepoFaktorBorongan.FindAll().Where(d=>d.IdMasterPool == IdPool && d.IdJenisTruck == IdJenisTruck).FirstOrDefault();
            
            if (dbitem != null)
            {
                //ambil data berdasarkan rasio
                if (typeRasion == "KOTA1")
                    rasio = dbitem.RasioDlmKota;
                else if (typeRasion == "KOTA2")
                    rasio = dbitem.RasioDlmKota2;
                else if (typeRasion == "JAWABALI")
                    rasio = dbitem.RasioJawaBali;
                else if (typeRasion == "SUMATRA")
                    rasio = dbitem.RasioSumatra;
                else if (typeRasion == "KOSONG")
                    rasio = dbitem.RasioKosong;
                response.Success = true;
                response.Data = new JavaScriptSerializer().Serialize(new { Rasio = rasio });
            }
            else
            {
                response.Success = false;
            }

            return new JavaScriptSerializer().Serialize(response);            
        }
        public string GetUangMakan(string AreaMakan, int idpool, int idjenistruck)
        {
            ResponeModel response = new ResponeModel();
            Decimal UangMakan = 0;
            Context.FaktorBorongan dbitem = RepoFaktorBorongan.FindAll().Where(d => d.IdMasterPool == idpool && d.IdJenisTruck == idjenistruck).FirstOrDefault();

            if (dbitem != null)
            {
                //ambil data berdasarkan rasio
                if (AreaMakan == "JAWABALI")
                    UangMakan = dbitem.UangMakanJawaBali;
                else if (AreaMakan == "SUMATRA")
                    UangMakan = dbitem.UangMakanSumatra;
                response.Success = true;
                response.Data = new JavaScriptSerializer().Serialize(new { UangMakan = UangMakan });
            }
            else
            {
                response.Success = false;
            }

            return new JavaScriptSerializer().Serialize(response);
        }
        public string GetBiayaKapal(string kapal, int idpool, int idjenistruck)
        {
            ResponeModel response = new ResponeModel();
            Decimal BiayaKapal = 0;
            Context.FaktorBorongan dbitem = RepoFaktorBorongan.FindAll().Where(d => d.IdMasterPool == idpool && d.IdJenisTruck == idjenistruck).FirstOrDefault();

            if (dbitem != null)
            {
                //ambil data berdasarkan rasio
                if (kapal == "BALI")
                    BiayaKapal = dbitem.BiayaKapalBali;
                else if (kapal == "BALI_NTB")
                    BiayaKapal = dbitem.BiayaKapalBaliNTB;
                else if (kapal == "SUMATRA")
                    BiayaKapal = dbitem.BiayaKapalSumatra;
                else if (kapal == "KALIMANTAN")
                    BiayaKapal = dbitem.BiayaKapalKalimantan;
                else if (kapal == "SULAWESI")
                    BiayaKapal = dbitem.BiayaKapalSulawesi;

                response.Success = true;
                response.Data = new JavaScriptSerializer().Serialize(new { BiayaKapal = BiayaKapal });
            }
            else
            {
                response.Success = false;
            }

            return new JavaScriptSerializer().Serialize(response);
        }
        public string GetFaktorPengali(int idpool, int idjenistruck)
        {
            ResponeModel response = new ResponeModel();
            Context.FaktorBorongan dbitem = RepoFaktorBorongan.FindAll().Where(d => d.IdMasterPool == idpool && d.IdJenisTruck == idjenistruck).FirstOrDefault();

            if (dbitem != null)
            {
                response.Success = true;
                response.Data = new JavaScriptSerializer().Serialize(new {
                    FaktorPengaliTips = dbitem.FaktorPengaliTips,
                    FaktorPengaliGaji = dbitem.FaktorPengaliGaji,
                });
            }
            else
            {
                response.Success = false;
            }

            return new JavaScriptSerializer().Serialize(response);
        }
        public string GetPool()
        {
            List<MasterPool> ListModel = new List<MasterPool>();
            foreach (Context.MasterPool item in RepoFaktorBorongan.FindAll().Select(d => d.MasterPool).Distinct())
            {
                ListModel.Add(new MasterPool(item));
            }
            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string GetJnsTruck()
        {
            List<JenisTruck> ListModel = new List<JenisTruck>();
            foreach (Context.JenisTrucks item in RepoFaktorBorongan.FindAll().Select(d => d.JenisTrucks).Distinct())
            {
                ListModel.Add(new JenisTruck(item));
            }
            return new JavaScriptSerializer().Serialize(ListModel);
        }
        #endregion
        #region upload
        public string UploadFaktorBorongan(IEnumerable<HttpPostedFileBase> filesFaktorBorongan)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesFaktorBorongan != null)
            {
                foreach (var file in filesFaktorBorongan)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && 
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null &&
                                    workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null && 
                                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null &&
                                    workSheet.Cells[rowIterator, 9].Value != null && workSheet.Cells[rowIterator, 10].Value != null && 
                                    workSheet.Cells[rowIterator, 11].Value != null )
                                {
                                    int id = 0;
                                    int resId;
                                    if (workSheet.Cells[rowIterator, 19].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 19].Value.ToString(), out resId))
                                            id = resId;
                                    }
                                    Context.FaktorBorongan dbitem = new Context.FaktorBorongan();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            if (RepoFaktorBorongan.isExist(RepoMasterPool.FindByNamePool(workSheet.Cells[rowIterator, 1].Value.ToString()).Id,
                                                RepoJenisTruck.FindByStrJenisTruck(workSheet.Cells[rowIterator, 2].Value.ToString()).Id, id))
                                            {
                                                continue;
                                            }
                                            dbitem = RepoFaktorBorongan.FindByPK(id);
                                        }
                                        else
                                        {
                                            if (RepoFaktorBorongan.isExist(RepoMasterPool.FindByNamePool(workSheet.Cells[rowIterator, 1].Value.ToString()).Id,
                                                RepoJenisTruck.FindByStrJenisTruck(workSheet.Cells[rowIterator, 2].Value.ToString()).Id))
                                            {
                                                continue;
                                            }
                                        }
                                        dbitem.IdMasterPool = RepoMasterPool.FindByNamePool(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        dbitem.IdJenisTruck = RepoJenisTruck.FindByStrJenisTruck(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        dbitem.RasioDlmKota = Decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                        dbitem.RasioDlmKota2 = Decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                        dbitem.RasioJawaBali = Decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        dbitem.RasioSumatra = Decimal.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                                        dbitem.RasioKosong = Decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString());
                                        dbitem.UangMakanJawaBali = Decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                                        dbitem.UangMakanSumatra = Decimal.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                        dbitem.FaktorPengaliGaji = Decimal.Parse(workSheet.Cells[rowIterator, 10].Value.ToString());
                                        dbitem.FaktorPengaliTips = Decimal.Parse(workSheet.Cells[rowIterator, 11].Value.ToString());
                                        dbitem.PotonganDriver1 = workSheet.Cells[rowIterator, 12].Value == null ? Convert.ToDecimal(null) : Decimal.Parse(workSheet.Cells[rowIterator, 12].Value.ToString());
                                        dbitem.PotonganDriver2 = workSheet.Cells[rowIterator, 13].Value == null ? Convert.ToDecimal(null) : Decimal.Parse(workSheet.Cells[rowIterator, 13].Value.ToString());
                                        dbitem.BiayaKapalBali = workSheet.Cells[rowIterator, 14].Value == null ? Convert.ToDecimal(null) : Decimal.Parse(workSheet.Cells[rowIterator, 14].Value.ToString());
                                        dbitem.BiayaKapalBaliNTB = workSheet.Cells[rowIterator, 15].Value == null ? Convert.ToDecimal(null) : Decimal.Parse(workSheet.Cells[rowIterator, 15].Value.ToString());
                                        dbitem.BiayaKapalSumatra = workSheet.Cells[rowIterator, 16].Value == null ? Convert.ToDecimal(null) : Decimal.Parse(workSheet.Cells[rowIterator, 16].Value.ToString());
                                        dbitem.BiayaKapalKalimantan = workSheet.Cells[rowIterator, 17].Value == null ? Convert.ToDecimal(null) : Decimal.Parse(workSheet.Cells[rowIterator, 17].Value.ToString());
                                        dbitem.BiayaKapalSulawesi = workSheet.Cells[rowIterator, 18].Value == null ? Convert.ToDecimal(null) : Decimal.Parse(workSheet.Cells[rowIterator, 18].Value.ToString());
                                        RepoFaktorBorongan.save(dbitem, UserPrincipal.id, new Context.FaktorBoronganHistory());
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
        public FileContentResult ExportFaktorBorongan()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.FaktorBorongan> dbitems = RepoFaktorBorongan.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Alokasi Pool";
            ws.Cells[1, 2].Value = "Jenis Truck";
            ws.Cells[1, 3].Value = "Rasio Dalam Kota 1";
            ws.Cells[1, 4].Value = "Rasio Dalam Kota 2";
            ws.Cells[1, 5].Value = "Rasio Jawa Bali";
            ws.Cells[1, 6].Value = "Rasio Sumatra";
            ws.Cells[1, 7].Value = "Rasio Kosong";
            ws.Cells[1, 8].Value = "Uang Makan Jawa Bali";
            ws.Cells[1, 9].Value = "Uang Makan Sumatra";
            ws.Cells[1, 10].Value = "Faktor Pengali Gaji";
            ws.Cells[1, 11].Value = "Faktor Pengali Tips Parkir";
            ws.Cells[1, 12].Value = "Potongan Driver 1";
            ws.Cells[1, 13].Value = "Potongan Driver 2";
            ws.Cells[1, 14].Value = "Biaya Kapal Bali";
            ws.Cells[1, 15].Value = "Biaya Kapal Bali-NTB";
            ws.Cells[1, 16].Value = "Biaya Kapal Sumatra";
            ws.Cells[1, 17].Value = "Biaya Kapal Kalimantan";
            ws.Cells[1, 18].Value = "Biaya Kapal Sulawesi";
            ws.Cells[1, 19].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].MasterPool.NamePool;
                ws.Cells[i + 2, 2].Value = dbitems[i].JenisTrucks.StrJenisTruck;
                ws.Cells[i + 2, 3].Value = dbitems[i].RasioDlmKota;
                ws.Cells[i + 2, 4].Value = dbitems[i].RasioDlmKota2;
                ws.Cells[i + 2, 5].Value = dbitems[i].RasioJawaBali;
                ws.Cells[i + 2, 6].Value = dbitems[i].RasioSumatra;
                ws.Cells[i + 2, 7].Value = dbitems[i].RasioKosong;
                ws.Cells[i + 2, 8].Value = dbitems[i].UangMakanJawaBali;
                ws.Cells[i + 2, 9].Value = dbitems[i].UangMakanSumatra;
                ws.Cells[i + 2, 10].Value = dbitems[i].FaktorPengaliGaji;
                ws.Cells[i + 2, 11].Value = dbitems[i].FaktorPengaliTips;
                ws.Cells[i + 2, 12].Value = dbitems[i].PotonganDriver1;
                ws.Cells[i + 2, 13].Value = dbitems[i].PotonganDriver2;
                ws.Cells[i + 2, 14].Value = dbitems[i].BiayaKapalBali;
                ws.Cells[i + 2, 15].Value = dbitems[i].BiayaKapalBaliNTB;
                ws.Cells[i + 2, 16].Value = dbitems[i].BiayaKapalSumatra;
                ws.Cells[i + 2, 17].Value = dbitems[i].BiayaKapalKalimantan;
                ws.Cells[i + 2, 18].Value = dbitems[i].BiayaKapalSulawesi;
                ws.Cells[i + 2, 19].Value = dbitems[i].Id;
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "FaktorBorongan.xls";

            return fsr;
        }
        #endregion
    }
}