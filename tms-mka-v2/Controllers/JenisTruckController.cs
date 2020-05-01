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
    public class JenisTruckController : BaseController
    {
        private IJenisTruckRepo RepoJenisTruck;
        public JenisTruckController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookUp, IJenisTruckRepo repoJenisTruck)
            : base(repoBase, repoLookUp)
        {
            RepoJenisTruck = repoJenisTruck;
        }
        [MyAuthorize(Menu = "Jenis Truk", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "JenisTruck").ToList();

            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.JenisTrucks> items = RepoJenisTruck.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<JenisTruck> ListModel = new List<JenisTruck>();
            foreach (Context.JenisTrucks item in items)
            {
                ListModel.Add(new JenisTruck(item));
            }

            int total = RepoJenisTruck.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [MyAuthorize(Menu = "Jenis Truk", Action="create")]
        public ActionResult Add()
        {
            JenisTruck model = new JenisTruck();

            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(JenisTruck model)
        {
            if (ModelState.IsValid)
            {
                bool Isexist = RepoJenisTruck.IsExist(model.StrJenisTruck);

                if (Isexist)
                {
                    ModelState.AddModelError("StrJenisTruck", "Jenis Truck sudah ada.");
                    return View("Form", model);
                }

                Context.JenisTrucks dbitem = new Context.JenisTrucks();
                model.setDb(dbitem);
                RepoJenisTruck.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Jenis Truk", Action="update")]
        public ActionResult Edit(int id)
        {
            JenisTruck model = new JenisTruck(RepoJenisTruck.FindByPK(id));
            ViewBag.name = model.StrJenisTruck;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(JenisTruck model)
        {
            if (ModelState.IsValid)
            {
                bool Isexist = RepoJenisTruck.IsExist(model.StrJenisTruck, model.Id);

                if (Isexist)
                {
                    ModelState.AddModelError("StrJenisTruck", "Jenis Truck telah dipakai.");
                    return View("Form", model);
                }

                Context.JenisTrucks dbitem = RepoJenisTruck.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoJenisTruck.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Jenis Truk", Action = "delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.JenisTrucks dbItem = RepoJenisTruck.FindByPK(id);

            RepoJenisTruck.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        #region option
        public string GetJnsTruck()
        {
            return new JavaScriptSerializer().Serialize(RepoJenisTruck.FindAll());
        }
        public string GetJnsTruckById(int id)
        {
            return new JavaScriptSerializer().Serialize(RepoJenisTruck.FindByPK(id));
        }
        public string GetGolongan(int id)
        {
            return new JavaScriptSerializer().Serialize(RepoJenisTruck.FindByPK(id).GolTol);
        }
        #endregion

        #region upload
        public string UploadTruck(IEnumerable<HttpPostedFileBase> filesTruck)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesTruck != null)
            {
                foreach (var file in filesTruck)
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
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null &&
                                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null)
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 6].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    Context.JenisTrucks dbitem = new Context.JenisTrucks();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoJenisTruck.FindByPK(id);
                                            if (RepoJenisTruck.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString(), id))
                                            {
                                                continue;
                                            }
                                        }
                                        else 
                                        {
                                            if (RepoJenisTruck.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString()))
                                            {
                                                continue;
                                            }
                                        }
                                            

                                        dbitem.StrJenisTruck = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbitem.GolTol = int.Parse(workSheet.Cells[rowIterator, 2].Value.ToString());
                                        dbitem.Alias = workSheet.Cells[rowIterator, 3].Value == null ? "" : workSheet.Cells[rowIterator, 3].Value.ToString();
                                        dbitem.Biaya = decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                        dbitem.AcInterval = int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());

                                        RepoJenisTruck.save(dbitem, UserPrincipal.id);
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

        [MyAuthorize(Menu = "Jenis Truk", Action = "read")]
        public FileContentResult ExportJenisTruck()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.JenisTrucks> dbitems = RepoJenisTruck.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Jenis Truck";
            ws.Cells[1, 2].Value = "Golongan";
            ws.Cells[1, 3].Value = "Alias";
            ws.Cells[1, 4].Value = "Biaya Solar";
            ws.Cells[1, 5].Value = "Ac Mati Interval";
            ws.Cells[1, 6].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].StrJenisTruck;
                ws.Cells[i + 2, 2].Value = dbitems[i].GolTol;
                ws.Cells[i + 2, 3].Value = dbitems[i].Alias;
                ws.Cells[i + 2, 4].Value = dbitems[i].Biaya;
                ws.Cells[i + 2, 5].Value = dbitems[i].AcInterval;
                ws.Cells[i + 2, 6].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Jenis Truck.xls";

            return fsr;
        }
        #endregion
    }
}