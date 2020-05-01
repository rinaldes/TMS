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
    public class MasterPoolController : BaseController
    {
        private IMasterPoolRepo RepoPool;
        private ILocationRepo RepoLoc;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        public MasterPoolController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IMasterPoolRepo repoPool, ILocationRepo repoLoc, IERPConfigRepo repoERPConfig,
            Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr)
            : base(repoBase, repoLookup)
        {
            RepoPool = repoPool;
            RepoLoc = repoLoc;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "MasterPool").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.MasterPool> items = RepoPool.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<MasterPool> ListModel = new List<MasterPool>();
            foreach (Context.MasterPool item in items)
            {
                ListModel.Add(new MasterPool(item));
            }

            int total = RepoPool.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public ActionResult Add()
        {
            MasterPool model = new MasterPool();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(MasterPool model)
        {
            if (ModelState.IsValid)
            {
                if (RepoPool.IsExist(model.NamePool))
                {
                    ModelState.AddModelError("NamePool", "Nama sudah dipakai.");
                    return View("Form", model);
                }
                Context.MasterPool dbitem = new Context.MasterPool();
                model.setDb(dbitem);
                RepoPool.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            ZoneParkir[] result = JsonConvert.DeserializeObject<ZoneParkir[]>(model.strPool);
            model.ListZoneParkir = result.ToList();
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.MasterPool dbitem = RepoPool.FindByPK(id);
            MasterPool model = new MasterPool(dbitem);
            ViewBag.name = model.NamePool;
            if (dbitem.IdCreditCash.HasValue)
            {
                ac_mstr bk_credit_cash = Repoac_mstr.FindByPk(dbitem.IdCreditCash);
                model.IdCreditCash = bk_credit_cash.id;
                model.CodeCreditCash = bk_credit_cash.ac_code;
                model.NamaCreditCash = bk_credit_cash.ac_name;
            }
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(MasterPool model)
        {
            if (ModelState.IsValid)
            {
                if (RepoPool.IsExist(model.NamePool, model.Id))
                {
                    ModelState.AddModelError("NamePool", "Nama sudah dipakai.");
                    return View("Form", model);
                }
                Context.MasterPool dbitem = RepoPool.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoPool.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            ZoneParkir[] result = JsonConvert.DeserializeObject<ZoneParkir[]>(model.strPool);
            model.ListZoneParkir = result.ToList();
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.MasterPool dbItem = RepoPool.FindByPK(id);

            RepoPool.delete(dbItem);

            return Json(response);
        }
        #region options
        public string GetPool()
        {
            List<MasterPool> ListModel = new List<MasterPool>();
            foreach (Context.MasterPool item in RepoPool.FindAll())
            {
                ListModel.Add(new MasterPool(item));
            }
            return new JavaScriptSerializer().Serialize(ListModel);
        }
        #endregion
        #region import export
        public string Upload(IEnumerable<HttpPostedFileBase> files)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (files != null)
            {
                foreach (var file in files)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfRow = workSheet.Dimension.End.Row;

                            //sheet 1
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 9].Value != null &&
                                    workSheet.Cells[rowIterator, 10].Value != null && workSheet.Cells[rowIterator, 11].Value != null && workSheet.Cells[rowIterator, 12].Value != null &&
                                    workSheet.Cells[rowIterator, 13].Value != null && workSheet.Cells[rowIterator, 14].Value != null && workSheet.Cells[rowIterator, 15].Value != null)
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 17].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 17].Value.ToString(), out resId))
                                            id = resId;
                                    }
                                    //cara gancang ngarah teu kudu aya pengecekan tiap field
                                    Context.MasterPool dbitem = new Context.MasterPool();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoPool.FindByPK(id);
                                            if (RepoPool.IsExist(workSheet.Cells[rowIterator, 2].Value.ToString(), id))
                                                continue;
                                        }

                                        if (RepoPool.IsExist(workSheet.Cells[rowIterator, 2].Value.ToString(), 2))
                                            continue;

                                        dbitem.IsActive = bool.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());
                                        dbitem.NamePool = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        dbitem.Capacity = int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                        dbitem.Address = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        dbitem.IdProvinsi = RepoLoc.FindByNama(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                                        dbitem.IdKabKota = RepoLoc.FindByNama(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                                        dbitem.IdKec = RepoLoc.FindByNama(workSheet.Cells[rowIterator, 7].Value.ToString()).Id;
                                        dbitem.IdKel = RepoLoc.FindByNama(workSheet.Cells[rowIterator, 8].Value.ToString()).Id;
                                        dbitem.Longitude = workSheet.Cells[rowIterator, 9].Value.ToString();
                                        dbitem.Latitude = workSheet.Cells[rowIterator, 10].Value.ToString();
                                        dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 11].Value.ToString());
                                        dbitem.KodeTelp = workSheet.Cells[rowIterator, 12].Value.ToString();
                                        dbitem.Telp = workSheet.Cells[rowIterator, 13].Value.ToString();
                                        dbitem.Pic = workSheet.Cells[rowIterator, 14].Value.ToString();
                                        dbitem.Handphone = workSheet.Cells[rowIterator, 15].Value.ToString();
                                        dbitem.ListZoneParkir.Clear();
                                        int idx = 0;
                                        for (idx = rowIterator; idx <= noOfRow; idx++)
                                        {
                                            if (workSheet.Cells[idx, 1].Value == null || idx == rowIterator)
                                            {
                                                if (workSheet.Cells[idx, 16].Value != null && workSheet.Cells[idx, 17].Value != null )
                                                {
                                                    try
                                                    {
                                                        Context.ZoneParkir dbparkir = new Context.ZoneParkir();
                                                        dbparkir.IdZone = RepoLookup.FindByName(workSheet.Cells[idx, 16].Value.ToString()).Id;
                                                        dbparkir.Pit = int.Parse(workSheet.Cells[idx, 17].Value.ToString());

                                                        dbitem.ListZoneParkir.Add(dbparkir);
                                                    }
                                                    catch (Exception)
                                                    {

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        rowIterator = idx - 1;
                                        RepoPool.save(dbitem, UserPrincipal.id);
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
        public FileContentResult Export()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.MasterPool> dbitems = RepoPool.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");
            ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Sheet 2");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Is Active";
            ws.Cells[1, 2].Value = "Nama Pool";
            ws.Cells[1, 3].Value = "Kapasitas";
            ws.Cells[1, 4].Value = "Alamat";
            ws.Cells[1, 5].Value = "Provinsi";
            ws.Cells[1, 6].Value = "Kabupaten";
            ws.Cells[1, 7].Value = "Kecamatan";
            ws.Cells[1, 8].Value = "Kelurahan";
            ws.Cells[1, 9].Value = "Longitude";
            ws.Cells[1, 10].Value = "Latitude";
            ws.Cells[1, 11].Value = "Radius";
            ws.Cells[1, 12].Value = "Kode Telp";
            ws.Cells[1, 13].Value = "Telp";
            ws.Cells[1, 14].Value = "PIC";
            ws.Cells[1, 15].Value = "Handphone";
            ws.Cells[1, 16].Value = "Zona";
            ws.Cells[1, 17].Value = "PIT";
            ws.Cells[1, 18].Value = "Id Database";

            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[idx, 1].Value = dbitems[i].IsActive;
                ws.Cells[idx, 2].Value = dbitems[i].NamePool;
                ws.Cells[idx, 3].Value = dbitems[i].Capacity;
                ws.Cells[idx, 4].Value = dbitems[i].Address;
                ws.Cells[idx, 5].Value = dbitems[i].LocProvinsi == null ? "" : dbitems[i].LocProvinsi.Nama;
                ws.Cells[idx, 6].Value = dbitems[i].LocKabKota == null ? "" : dbitems[i].LocKabKota.Nama;
                ws.Cells[idx, 7].Value = dbitems[i].LocKecamatan == null ? "" : dbitems[i].LocKecamatan.Nama;
                ws.Cells[idx, 8].Value = dbitems[i].LocKelurahan == null ? "" : dbitems[i].LocKelurahan.Nama;
                ws.Cells[idx, 9].Value = dbitems[i].Longitude == null ? "" : dbitems[i].Longitude;
                ws.Cells[idx, 10].Value = dbitems[i].Latitude == null ? "" : dbitems[i].Latitude;
                ws.Cells[idx, 11].Value = dbitems[i].Radius;
                ws.Cells[idx, 12].Value = dbitems[i].KodeTelp;
                ws.Cells[idx, 13].Value = dbitems[i].Telp;
                ws.Cells[idx, 14].Value = dbitems[i].Pic;
                ws.Cells[idx, 15].Value = dbitems[i].Handphone;
                ws.Cells[idx, 18].Value = dbitems[i].Id;
                if (dbitems[i].ListZoneParkir.Count() > 0)
                {
                    foreach (Context.ZoneParkir item in dbitems[i].ListZoneParkir)
                    {
                        ws.Cells[idx, 16].Value = item.LookUpCodeZone.Nama;
                        ws.Cells[idx, 17].Value = item.Pit;
                        idx++;
                    }
                }
                else
                {
                    idx++;
                }
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Master Pool.xls";

            return fsr;
        }
        #endregion
    }
}