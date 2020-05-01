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
    public class MekanikController : BaseController 
    {
        private IMekanikRepo RepoMekanik;
        public MekanikController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IMekanikRepo repoMekanik)
            : base(repoBase, repoLookup)
        {   
            RepoMekanik = repoMekanik;
        }
//        [MyAuthorize(Menu = "Mekanik", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "Mekanik").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Mekanik> items = RepoMekanik.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Mekanik> ListModel = new List<Mekanik>();
            foreach (Context.Mekanik item in items)
            {
                ListModel.Add(new Mekanik(item));
            }

            int total = RepoMekanik.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [MyAuthorize(Menu = "Mekanik", Action="create")]
        public ActionResult Add()
        {
            Mekanik model = new Mekanik();
            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(Mekanik model)
        {
            if (ModelState.IsValid)
            {
                Context.Mekanik dbitem = new Context.Mekanik();
                model.setDb(dbitem);
                RepoMekanik.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        //[MyAuthorize(Menu = "Mekanik", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.Mekanik dbitem = RepoMekanik.FindByPK(id);
            Mekanik model = new Mekanik(dbitem);
            ViewBag.name = model.Id;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(Mekanik model)
        {
            if (ModelState.IsValid)
            {
                Context.Mekanik dbitem = RepoMekanik.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoMekanik.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Mekanik dbItem = RepoMekanik.FindByPK(id);

            RepoMekanik.delete(dbItem);

            return Json(response);
        }

        #region upload
        public string UploadMekanik(IEnumerable<HttpPostedFileBase> filesMekanik)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesMekanik != null)
            {
                foreach (var file in filesMekanik)
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
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null)
                                {
                                    if (workSheet.Cells[rowIterator, 5].Value == null)
                                    {
                                        // insert new row
                                        string namaMekaniks = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        //string namaMekanik = RepoMekanik.FindAll().Where(m => m.NamaMekanik == namaMekaniks).Select(m => m.NamaMekanik).FirstOrDefault();
                                        int? idBagian = RepoLookup.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        int? idGrade = RepoLookup.FindByNameAndCat(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                                        Context.Mekanik dbmekanik = new Context.Mekanik();

                                        if (idGrade.HasValue && idBagian.HasValue)
                                        {
                                            dbmekanik.NamaMekanik = namaMekaniks;
                                            dbmekanik.IdBagian = idBagian;
                                            dbmekanik.IdGrade = idGrade;
                                            dbmekanik.Keterampilan = workSheet.Cells[rowIterator, 4].Value == null ? "" : workSheet.Cells[rowIterator, 4].Value.ToString();

                                            RepoMekanik.save(dbmekanik);
                                        }
                                    }
                                    else
                                    {
                                        // update existing row
                                        Context.Mekanik dbmekanik = RepoMekanik.FindByPK(int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()));
                                        dbmekanik.NamaMekanik = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbmekanik.IdBagian = RepoLookup.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        dbmekanik.IdGrade = RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                                        dbmekanik.Keterampilan = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        RepoMekanik.save(dbmekanik);
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
            List<Context.Mekanik> dbitems = RepoMekanik.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");            

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Nama Mekanik";
            ws.Cells[1, 2].Value = "Bagian";
            ws.Cells[1, 3].Value = "Grade";
            ws.Cells[1, 4].Value = "Keterampilan";
            ws.Cells[1, 5].Value = "ID Database";

            // Inserts Data
            //int idx = 0;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].NamaMekanik;
                ws.Cells[i + 2, 2].Value = dbitems[i].LookUpCodeBagian.Nama;
                ws.Cells[i + 2, 3].Value = dbitems[i].LookUpCodeGrade.Nama;
                ws.Cells[i + 2, 4].Value = dbitems[i].Keterampilan;
                ws.Cells[i + 2, 5].Value = dbitems[i].Id;                
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "DataMekanik.xls";

            return fsr;
        }
        #endregion
    }
}