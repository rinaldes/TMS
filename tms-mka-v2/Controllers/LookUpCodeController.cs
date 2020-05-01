using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using System.Web.Script.Serialization;
using tms_mka_v2.Infrastructure;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace tms_mka_v2.Controllers
{
    public class LookUpCodeController : BaseController
    {
        private ILookupCodeCategoriesRepo RepoKategori;
        public LookUpCodeController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, 
            ILookupCodeCategoriesRepo repoKategori)
            : base(repoBase, repoLookup)
        {
            RepoLookup = repoLookup;
            RepoKategori = repoKategori;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "LookUpCode").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            
            List<Context.LookupCode> items = RepoLookup.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<LookupCode> ListModel = new List<LookupCode>();
            foreach (Context.LookupCode item in items)
            {
                ListModel.Add(new LookupCode(item));
            }

            int total = RepoLookup.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingKonsolidasi()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            
            List<Context.LookupCode> items = RepoLookup.FindAll(param.Skip, 200, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).Where(d => d.LookupCodeCategories.Category == "tms_customer_konsolidasi").ToList();

            List<LookupCode> ListModel = new List<LookupCode>();
            foreach (Context.LookupCode item in items)
            {
                LookupCode lc = new LookupCode(item);
                    ListModel.Add(lc);
            }

            int total = RepoLookup.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public ActionResult Add()
        {
            LookupCode model = new LookupCode();

            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(LookupCode model)
        {
            if (ModelState.IsValid)
            {
                Context.LookupCode dbitem = new Context.LookupCode();
                model.setDb(dbitem);
                RepoLookup.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            LookupCode model = new LookupCode(RepoLookup.FindByPK(id));
            ViewBag.name = model.Nama;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(LookupCode model)
        {
            if (ModelState.IsValid)
            {
                Context.LookupCode dbitem = RepoLookup.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoLookup.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.LookupCode dbItem = RepoLookup.FindByPK(id);

            RepoLookup.delete(dbItem);

            return Json(response);
        }
        public string GetKategori(LookupCode model)
        {
            model.listKategori = RepoKategori.FindAll().ToList();

            return new JavaScriptSerializer().Serialize(model.listKategori);
        }
        public string GetLookUpByName(string nama)
        {
            Context.LookupCode db = RepoLookup.FindByName(nama);

            return new JavaScriptSerializer().Serialize(db);
        }

        #region upload
        public string UploadLookUpCode(IEnumerable<HttpPostedFileBase> filesLookUp)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesLookUp != null)
            {
                foreach (var file in filesLookUp)
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
                                if (workSheet.Cells[rowIterator, 1].Value != null || workSheet.Cells[rowIterator, 2].Value != null ||
                                    workSheet.Cells[rowIterator, 3].Value != null || workSheet.Cells[rowIterator, 4].Value != null)
                                {
                                    string namaKategori = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    int? idKategori = RepoKategori.FindAll().Where(k => k.Category == namaKategori).Select(k => k.Id).FirstOrDefault();
                                    string namaLookUp = RepoLookup.FindAll().Where(l => l.Nama == workSheet.Cells[rowIterator, 2].Value.ToString()).Select(l => l.Nama).FirstOrDefault();
                                    Context.LookupCode dbitem = new Context.LookupCode();

                                    if (idKategori.HasValue && namaLookUp == null && idKategori > 0)
                                    {
                                        dbitem.IdKategori = idKategori.Value;
                                        dbitem.Nama = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        dbitem.Order = int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                        dbitem.Deskripsi = workSheet.Cells[rowIterator, 4].Value.ToString();

                                        RepoLookup.save(dbitem);
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
        #endregion
    }
}