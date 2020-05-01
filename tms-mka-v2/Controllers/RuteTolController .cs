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
    public class RuteTolController : BaseController
    {
        private IRuteTolRepo RepoRuteTol;
        private IRuteRepo RepoRute;
        private IJnsTolRepo RepoJnsTol;
        public RuteTolController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IRuteTolRepo repoRuteTol, IRuteRepo repoRute, IJnsTolRepo repoJnsTol)
            : base(repoBase, repoLookup)
        {
            RepoRuteTol = repoRuteTol;
            RepoRute = repoRute;
            RepoJnsTol = repoJnsTol;
        }
        [MyAuthorize(Menu = "Rute Tol", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "MasterPool").ToList();
            return View();
        }
        [MyAuthorize(Menu = "Rute Tol", Action = "read")]
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.RuteTol> items = RepoRuteTol.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<RuteTol> ListModel = new List<RuteTol>();
            foreach (Context.RuteTol item in items)
            {
                ListModel.Add(new RuteTol(item));
            }

            int total = RepoRuteTol.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [MyAuthorize(Menu = "Rute Tol", Action = "create")]
        public ActionResult Add()
        {
            RuteTol model = new RuteTol();
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Rute Tol", Action = "create")]
        [HttpPost]
        public ActionResult Add(RuteTol model)
        {
            if (ModelState.IsValid)
            {
                Context.RuteTol dbitem = new Context.RuteTol();
                model.setDb(dbitem);
                //generate code
                
                RepoRuteTol.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            TolPP[] resultBerangkat = JsonConvert.DeserializeObject<TolPP[]>(model.strBerangkat);
            TolPP[] resultPulang = JsonConvert.DeserializeObject<TolPP[]>(model.strPulang);
            model.ListTolBerangkat = resultBerangkat.ToList();
            model.ListTolPulang = resultPulang.ToList();
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Rute Tol", Action = "update")]
        public ActionResult Edit(int id)
        {
            Context.RuteTol dbitem = RepoRuteTol.FindByPK(id);
            RuteTol model = new RuteTol(dbitem);
            ViewBag.name = model.NamaRute;
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Rute Tol", Action = "update")]
        [HttpPost]
        public ActionResult Edit(RuteTol model)
        {
            if (ModelState.IsValid)
            {
                Context.RuteTol dbitem = RepoRuteTol.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoRuteTol.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            TolPP[] resultBerangkat = JsonConvert.DeserializeObject<TolPP[]>(model.strBerangkat);
            TolPP[] resultPulang = JsonConvert.DeserializeObject<TolPP[]>(model.strPulang);
            model.ListTolBerangkat = resultBerangkat.ToList();
            model.ListTolPulang = resultPulang.ToList();
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Rute Tol", Action = "delete")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.RuteTol dbItem = RepoRuteTol.FindByPK(id);

            RepoRuteTol.delete(dbItem);

            return Json(response);
        }

        #region upload
        [MyAuthorize(Menu = "Rute Tol", Action = "create")]
        public string UploadRuteTol(IEnumerable<HttpPostedFileBase> filesRuteTol)
        {
            ResponeModel response = new ResponeModel();
            if (filesRuteTol != null)
            {
                foreach (var file in filesRuteTol)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfRow = workSheet.Dimension.End.Row;

                            // sheet 1
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null)
                                {
                                    int id = 0;
                                    Context.RuteTol dbitem = new Context.RuteTol();
                                    if (workSheet.Cells[rowIterator, 5].Value != null)
                                    {
                                        int resId;
                                        if (int.TryParse(workSheet.Cells[rowIterator, 5].Value.ToString(), out resId))
                                            id = resId;
                                    }
                                    if (id != 0)
                                    {
                                        dbitem = RepoRuteTol.FindByPK(id);
                                        dbitem.ListTolBerangkat.Clear();
                                        dbitem.ListTolPulang.Clear();
                                    }

                                    try
                                    {
                                        //parent
                                        dbitem.IdRute = RepoRute.FindByKode(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        dbitem.NamaRuteTol = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        //child 1
                                        int idx = 0;
                                        for (idx = rowIterator; idx <= noOfRow; idx++)
                                        {
                                            if (workSheet.Cells[idx, 1].Value == null || idx == rowIterator)
                                            {
                                                if (workSheet.Cells[idx, 3].Value != null)
                                                {
                                                    Context.TolBerangkat item = new Context.TolBerangkat();
                                                    item.IdTol = RepoJnsTol.FindByNamaTol(workSheet.Cells[idx, 3].Value.ToString()).Id;
                                                    dbitem.ListTolBerangkat.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        //child 1
                                        idx = 0;
                                        for (idx = rowIterator; idx <= noOfRow; idx++)
                                        {
                                            if (workSheet.Cells[idx, 1].Value == null || idx == rowIterator)
                                            {
                                                if (workSheet.Cells[idx, 4].Value != null)
                                                {
                                                    Context.TolPulang item = new Context.TolPulang();
                                                    item.IdTol = RepoJnsTol.FindByNamaTol(workSheet.Cells[idx, 4].Value.ToString()).Id;
                                                    dbitem.ListTolPulang.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        if (idx != 0)
                                            rowIterator = idx - 1;
                                        RepoRuteTol.save(dbitem, UserPrincipal.id);
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

        [MyAuthorize(Menu = "Rute Tol", Action = "read")]
        public FileContentResult Export()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.RuteTol> dbRuteTol = RepoRuteTol.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Rute Tol");
        
            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Kode Rute";
            ws.Cells[1, 2].Value = "Nama Rute Tol";
            ws.Cells[1, 3].Value = "Tol Berangkat";
            ws.Cells[1, 4].Value = "Tol Pulang";
            ws.Cells[1, 5].Value = "Id Database";

            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbRuteTol.Count(); i++)
            {
                ws.Cells[idx, 1].Value = dbRuteTol[i].Rute.Kode;
                ws.Cells[idx, 2].Value = dbRuteTol[i].NamaRuteTol;
                ws.Cells[idx, 5].Value = dbRuteTol[i].Id;
                int i1 = idx;
                foreach (Context.TolBerangkat item in dbRuteTol[i].ListTolBerangkat)
                {
                    ws.Cells[i1, 3].Value = item.JnsTol.NamaTol;
                    i1++;
                }
                int i2 = idx;
                foreach (Context.TolPulang item in dbRuteTol[i].ListTolPulang)
                {
                    ws.Cells[i2, 4].Value = item.JnsTol.NamaTol;
                    i2++;
                }
                if (i1 > i2)
                    idx = i1;
                else
                    idx = i2;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Export Data Rute Tol.xlsx";

            return fsr;
        }
        #endregion
    }
}