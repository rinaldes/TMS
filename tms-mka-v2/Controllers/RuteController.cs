using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
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
    public class RuteController : BaseController
    {
        private IRuteRepo RepoRute;
        private ILocationRepo RepoLocation;
        private IAreaRepo RepoArea;
        private IMultiDropRepo RepoMultidrop;
        public RuteController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IRuteRepo repoRute, ILocationRepo repoLocation, IAreaRepo repoArea, IMultiDropRepo repoMultidrop)
            : base(repoBase, repoLookup)
        {
            RepoRute = repoRute;
            RepoLocation = repoLocation;
            RepoArea = repoArea;
            RepoMultidrop = repoMultidrop;
        }
        [MyAuthorize(Menu = "Rute", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Rute").ToList();

            return View();
        }

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Rute> items = RepoRute.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Rute> ListModel = new List<Rute>();
            foreach (Context.Rute item in items)
            {
                ListModel.Add(new Rute(item));
            }

            int total = RepoRute.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string BindingPerCustomer(int CustId)
        {
            List<Rute> listRute = new List<Rute>();
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.Rute> rute = RepoRute.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).Where(r => r.CustomerId == CustId || r.CustomerId.HasValue == false).ToList();
            foreach (Context.Rute item in rute)
            {
                listRute.Add(new Rute(item));
            }

            int total = RepoRute.Count();

            return new JavaScriptSerializer().Serialize(new { total = total, data = listRute });
        }
        [MyAuthorize(Menu = "Rute", Action = "create")]
        public ActionResult Add()
        {
            Rute model = new Rute();

            return View("Form", model);
        }
        [MyAuthorize(Menu = "Rute", Action = "create")]
        [HttpPost]
        public ActionResult Add(Rute model, string submitBasicData)
        {
            if (ModelState.IsValid)
            {

                Context.Rute dbitem = new Context.Rute();
                model.setDb(dbitem);

                //generate code
                dbitem.Urutan = RepoRute.getUrutan() + 1;
                dbitem.Kode = RepoRute.generateCode(dbitem.Urutan);

                RepoRute.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Rute", Action = "update")]
        public ActionResult Edit(int id)
        {
            Context.Rute dbitem = RepoRute.FindByPK(id);
            Rute Viewmodel = new Rute(dbitem);

            ViewBag.name = Viewmodel.Kode;
            return View("Form", Viewmodel);
        }
        [MyAuthorize(Menu = "Rute", Action = "update")]
        [HttpPost]
        public ActionResult Edit(Rute model)
        {
            if (ModelState.IsValid)
            {
                Context.Rute dbitem = RepoRute.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoRute.save(dbitem);

                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        [MyAuthorize(Menu = "Rute", Action = "delete")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Rute dbItem = RepoRute.FindByPK(id);

            RepoRute.delete(dbItem);

            return Json(response);
        }
        #region option
        public string GetDataForSo(int id)
        {
            Context.Rute dbitem = RepoRute.FindByPK(id);
            Rute model = new Rute(dbitem);
            return new JavaScriptSerializer().Serialize(new { data = model });
        }

        #endregion
        #region Export import
        [MyAuthorize(Menu = "Rute", Action = "create")]
        public string Upload(IEnumerable<HttpPostedFileBase> fileRute)
        {
            ResponeModel response = new ResponeModel();
            if (fileRute != null)
            {
                foreach (var file in fileRute)
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
                                    workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 7].Value != null)
                                {
                                    int id = 0;
                                    int resId;

                                    if (workSheet.Cells[rowIterator, 14].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 14].Value.ToString(), out resId))
                                        {
                                            id = resId;
                                        }
                                    }

                                    Context.Rute db = new Context.Rute();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            db = RepoRute.FindByPK(id);
                                            db.RuteCheckPoint.Clear();
                                        }

                                        //parent
                                        db.Urutan = RepoRute.getUrutan() + 1;
                                        db.Kode = RepoRute.generateCode(db.Urutan);
                                        db.Nama = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        db.IdAsal = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        db.IdAreaAsal = RepoArea.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                                        db.IdTujuan = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                                        db.IdAreaTujuan = RepoArea.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 6].Value != null && workSheet.Cells[rowIterator, 6].Value.ToString() != "")
                                            db.IdMultiDrop = RepoMultidrop.FindByTujuan(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                                        db.WaktuKerja = int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString());
                                        if (workSheet.Cells[rowIterator, 8].Value != null)
                                            db.Toleransi = int.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                                        db.IsAreaPulang = workSheet.Cells[rowIterator, 9].Value == null ? false : bool.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                        db.IsKotaKota = workSheet.Cells[rowIterator, 10].Value == null ? false : bool.Parse(workSheet.Cells[rowIterator, 10].Value.ToString());

                                        // child;
                                        int idx = 0;
                                        for (idx = rowIterator; idx <= noOfRow; idx++)
                                        {
                                            if (workSheet.Cells[idx, 1].Value == null || idx == rowIterator)
                                            {
                                                if (workSheet.Cells[idx, 10].Value != null && workSheet.Cells[idx, 11].Value != null &&
                                                    workSheet.Cells[idx, 12].Value != null && workSheet.Cells[idx, 13].Value != null)
                                                {
                                                    Context.RuteCheckPoint item = new Context.RuteCheckPoint();
                                                    item.longitude = workSheet.Cells[idx, 10].Value.ToString();
                                                    item.langitude = workSheet.Cells[idx, 11].Value.ToString();
                                                    item.radius = int.Parse(workSheet.Cells[idx, 12].Value.ToString());
                                                    item.toleransi = int.Parse(workSheet.Cells[idx, 13].Value.ToString());
                                                    db.RuteCheckPoint.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        if (idx != 0)
                                            rowIterator = idx - 1;

                                        RepoRute.save(db);
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

        [MyAuthorize(Menu = "Rute", Action = "read")]
        public FileContentResult Export()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Rute> dbRute = RepoRute.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Rute");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Kode Rute";
            ws.Cells[1, 2].Value = "Nama Rute";
            ws.Cells[1, 3].Value = "Asal";
            ws.Cells[1, 4].Value = "Area Asal";
            ws.Cells[1, 5].Value = "Tujuan";
            ws.Cells[1, 6].Value = "Area Tujuan";
            ws.Cells[1, 7].Value = "Multidrop";
            ws.Cells[1, 8].Value = "Waktu Kerja";
            ws.Cells[1, 9].Value = "Toleransi Delay";
            ws.Cells[1, 10].Value = "Ara Pulang";
            ws.Cells[1, 11].Value = "Longitude";
            ws.Cells[1, 12].Value = "Latitude";
            ws.Cells[1, 13].Value = "Radius";
            ws.Cells[1, 14].Value = "Toleransi Delay";
            ws.Cells[1, 15].Value = "Id Database";

            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbRute.Count(); i++)
            {
                ws.Cells[idx, 1].Value = dbRute[i].Kode;
                ws.Cells[idx, 2].Value = dbRute[i].Nama;
                ws.Cells[idx, 3].Value = dbRute[i].LocationAsal.Code;
                ws.Cells[idx, 4].Value = dbRute[i].AreaAsal.Kode;
                ws.Cells[idx, 5].Value = dbRute[i].LocationTujuan.Code;
                ws.Cells[idx, 6].Value = dbRute[i].AreaTujuan.Kode;
                ws.Cells[idx, 7].Value = dbRute[i].Multidrop == null ? "" : dbRute[i].Multidrop.tujuan;
                ws.Cells[idx, 8].Value = dbRute[i].WaktuKerja;
                ws.Cells[idx, 9].Value = dbRute[i].Toleransi;
                ws.Cells[idx, 10].Value = dbRute[i].IsAreaPulang;

                int j = idx;
                foreach (Context.RuteCheckPoint item in dbRute[i].RuteCheckPoint)
                {
                    ws.Cells[j, 11].Value = item.longitude;
                    ws.Cells[j, 12].Value = item.langitude;
                    ws.Cells[j, 13].Value = item.radius;
                    ws.Cells[j, 14].Value = item.toleransi;
                    j++;
                }
                ws.Cells[idx, 15].Value = dbRute[i].Id;
                idx = j;
                idx++;
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Export Data Rute.xlsx";

            return fsr;
        }

        #endregion
    }
}