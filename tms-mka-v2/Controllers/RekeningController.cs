using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using System.Globalization;
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
    public class RekeningController : BaseController
    {
        private IRekeningRepo RepoRekening;
        public RekeningController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IRekeningRepo repoRekening)
            : base(repoBase, repoLookup)
        {
            RepoRekening = repoRekening;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Rekening").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Rekenings> items = RepoRekening.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Rekening> ListModel = new List<Rekening>();
            foreach (Context.Rekenings item in items)
            {
                ListModel.Add(new Rekening(item));
            }

            int total = RepoRekening.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public ActionResult Add()
        {
            Rekening model = new Rekening();

            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(Rekening model)
        {
            if (ModelState.IsValid)
            {
                if (RepoRekening.IsExist(model.IdBank.Value, model.NoRekening))
                {
                    ModelState.AddModelError("NoRekening", "No Rekening sudah terdaftar.");
                    return View("Form", model);
                }
                Context.Rekenings dbitem = new Context.Rekenings();
                model.setDb(dbitem);
                
                RepoRekening.save(dbitem);

                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.Rekenings dbitem = RepoRekening.FindByPK(id);
            Rekening model = new Rekening(dbitem);
            ViewBag.name = model.NoRekening;

            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(Rekening model)
        {
            if (ModelState.IsValid)
            {
                if (RepoRekening.IsExist(model.IdBank.Value, model.NoRekening, model.Id))
                {
                    ModelState.AddModelError("NoRekening", "No Rekening sudah terdaftar.");
                    return View("Form", model);
                }
                Context.Rekenings dbitem = RepoRekening.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoRekening.save(dbitem);

                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Rekenings dbItem = RepoRekening.FindByPK(id);

            RepoRekening.delete(dbItem);

            return Json(response);
        }

        #region upload
        public string UploadRekening(IEnumerable<HttpPostedFileBase> filesRekening)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesRekening != null)
            {
                foreach (var file in filesRekening)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfRow = workSheet.Dimension.End.Row;
                            var regexItem = new System.Text.RegularExpressions.Regex("^[0-9]*$");

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                //cek mandatory field
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null &&
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null &&
                                    regexItem.IsMatch(workSheet.Cells[rowIterator, 2].Value.ToString()))
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 5].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 5].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    try
                                    {
                                        Context.Rekenings dbitem = new Context.Rekenings(); ;
                                        if (id != 0)
                                        {
                                            if (RepoRekening.IsExist(RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id, workSheet.Cells[rowIterator, 2].Value.ToString(), id))
                                            {
                                                continue;
                                            }
                                            dbitem = RepoRekening.FindByPK(id);
                                        }
                                        else
                                        {
                                            if (RepoRekening.IsExist(RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id, workSheet.Cells[rowIterator, 2].Value.ToString()))
                                            {
                                                ModelState.AddModelError("NoRekening", "No Rekening sudah terdaftar.");
                                                continue;
                                            }
                                            
                                        }
                                        dbitem.Id = id;
                                        dbitem.NamaRekening = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbitem.NoRekening = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        dbitem.IdBank = RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                                        string type = workSheet.Cells[rowIterator, 4].Value.ToString().ToUpper();
                                        if (type != "PPN" && type != "NON PPN")
                                            continue;
                                        dbitem.Type = type;

                                        RepoRekening.save(dbitem);
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

        public FileContentResult ExportRekening()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Rekenings> dbitems = RepoRekening.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Nama Rekening";
            ws.Cells[1, 2].Value = "No Rekening";
            ws.Cells[1, 3].Value = "Bank";
            ws.Cells[1, 4].Value = "Type";
            ws.Cells[1, 5].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].NamaRekening;
                ws.Cells[i + 2, 2].Value = dbitems[i].NoRekening.ToString();
                ws.Cells[i + 2, 3].Value = dbitems[i].LookupCodeBank.Nama;
                ws.Cells[i + 2, 4].Value = dbitems[i].Type;
                ws.Cells[i + 2, 5].Value = dbitems[i].Id;
            }
            

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Rekening.xls";

            return fsr;
        }
        #endregion
    }
}