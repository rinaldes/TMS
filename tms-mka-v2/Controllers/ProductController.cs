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
using tms_mka_v2.Security;

namespace tms_mka_v2.Controllers
{
    public class ProductController : BaseController
    {
        
        private IProductRepo RepoProduct;
        public ProductController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IProductRepo repoProduct)
            : base(repoBase, repoLookup)
        {
            RepoProduct = repoProduct;
        }

        [MyAuthorize(Menu = "Produk", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Product").ToList();

            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.MasterProduct> items = RepoProduct.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Product> ListModel = new List<Product>();
            foreach (Context.MasterProduct item in items)
            {
                ListModel.Add(new Product(item));
            }

            int total = RepoProduct.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingCbo()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.MasterProduct> items = RepoProduct.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Product> ListModel = new List<Product>();
            foreach (Context.MasterProduct item in items)
            {
                ListModel.Add(new Product(item));
            }

            int total = RepoProduct.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string FindByPk(int id)
        {
            Product model = new Product(RepoProduct.FindByPK(id));

            return new JavaScriptSerializer().Serialize(model);
        }
        public string FindByName(string name)
        {
            Product model = new Product(RepoProduct.FindByName(name));

            return new JavaScriptSerializer().Serialize(model);
        }
        [MyAuthorize(Menu = "Produk", Action = "create")]
        public ActionResult Add()
        {
            Product model = new Product();

            return View("Form",model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Produk", Action = "create")]
        public ActionResult Add(Product model)
        {
            if (ModelState.IsValid)
            {
                bool NamaProdukexist = RepoProduct.IsExist(model.NamaProduk);

                if (NamaProdukexist)
                {
                    ModelState.AddModelError("NamaProduk", "Nama Produk sudah terdaftar !");
                    return View("Form", model);
                }

                Context.MasterProduct dbitem = new Context.MasterProduct();
                model.setDb(dbitem);
                RepoProduct.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Produk", Action = "update")]
        public ActionResult Edit(int id)
        {
            Product model = new Product(RepoProduct.FindByPK(id));
            ViewBag.name = model.NamaProduk;
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Produk", Action = "update")]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                bool NamaProdukexist = RepoProduct.IsExist(model.NamaProduk, model.Id);

                if (NamaProdukexist)
                {
                    ModelState.AddModelError("NamaProduk", "Nama Produk sudah terdaftar !");
                    return View("Form", model);
                }

                Context.MasterProduct dbitem = RepoProduct.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoProduct.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Produk", Action = "delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.MasterProduct dbItem = RepoProduct.FindByPK(id);

            RepoProduct.delete(dbItem);

            return Json(response);
        }

        #region import export
        public string UploadProduct(IEnumerable<HttpPostedFileBase> filesProduct)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesProduct != null)
            {
                foreach (var file in filesProduct)
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
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                                    decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString()) >= -30 && decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString()) <= 30 &&
                                    decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()) >= -30 && decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()) <= 30 &&
                                    decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()) >= -30 && decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()) <= 30)
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 8].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 8].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    //cara gancang ngarah teu kudu aya pengecekan tiap field
                                    Context.MasterProduct dbitem = new Context.MasterProduct();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoProduct.FindByPK(id);
                                            if (RepoProduct.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString(), id))
                                                continue;
                                        }
                                        else
                                        {
                                            if (RepoProduct.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString()))
                                                continue;
                                        }
                                            

                                        dbitem.NamaProduk = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbitem.IdKategori = RepoLookup.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        dbitem.TargetSuhu = decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                        dbitem.MaxTemps = decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                        dbitem.MinTemps = decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        dbitem.Treshold = int.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                                        dbitem.Remarks = workSheet.Cells[rowIterator, 7].Value == null ? "" : workSheet.Cells[rowIterator, 7].Value.ToString();

                                        RepoProduct.save(dbitem);
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

        public FileContentResult ExportProduct()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.MasterProduct> dbitems = RepoProduct.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Nama Produk";
            ws.Cells[1, 2].Value = "Kategori Produk";
            ws.Cells[1, 3].Value = "Target Suhu";
            ws.Cells[1, 4].Value = "Suhu Maksimal";
            ws.Cells[1, 5].Value = "Suhu Minimal";
            ws.Cells[1, 6].Value = "Interval Alert";
            ws.Cells[1, 7].Value = "Keterangan";
            ws.Cells[1, 8].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].NamaProduk;
                ws.Cells[i + 2, 2].Value = dbitems[i].LookupCode.Nama;
                ws.Cells[i + 2, 3].Value = dbitems[i].TargetSuhu;
                ws.Cells[i + 2, 4].Value = dbitems[i].MaxTemps;
                ws.Cells[i + 2, 5].Value = dbitems[i].MinTemps;
                ws.Cells[i + 2, 6].Value = dbitems[i].Treshold;
                ws.Cells[i + 2, 7].Value = dbitems[i].Remarks;
                ws.Cells[i + 2, 8].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Product.xls";

            return fsr;
        }
        #endregion
    }
}