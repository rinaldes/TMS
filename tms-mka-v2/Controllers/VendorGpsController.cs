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
    public class VendorGpsController : BaseController
    {
        private IVendorGpsRepo RepoVendor;
        public VendorGpsController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IVendorGpsRepo repoVendor)
            : base(repoBase, repoLookup)
        {
            RepoVendor = repoVendor;
        }
        [MyAuthorize(Menu = "Vendor Gps", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "VendorGps").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.VendorGps> items = RepoVendor.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<VendorGps> ListModel = new List<VendorGps>();
            foreach (Context.VendorGps item in items)
            {
                ListModel.Add(new VendorGps(item));
            }

            int total = RepoVendor.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [MyAuthorize(Menu = "Vendor Gps", Action="create")]
        public ActionResult Add()
        {
            VendorGps model = new VendorGps();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(VendorGps model)
        {
            if (ModelState.IsValid)
            {
                Context.VendorGps dbitem = new Context.VendorGps();
                model.setDb(dbitem);
                
                RepoVendor.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            Kontak[] result = JsonConvert.DeserializeObject<Kontak[]>(model.strVendor);
            model.ListKontak = result.ToList();
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Vendor Gps", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.VendorGps dbitem = RepoVendor.FindByPK(id);
            VendorGps model = new VendorGps(dbitem);
            ViewBag.name = model.Nama;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(VendorGps model)
        {
            if (ModelState.IsValid)
            {
                Context.VendorGps dbitem = RepoVendor.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoVendor.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            Kontak[] result = JsonConvert.DeserializeObject<Kontak[]>(model.strVendor);
            model.ListKontak = result.ToList();
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Vendor Gps", Action="delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.VendorGps dbItem = RepoVendor.FindByPK(id);

            RepoVendor.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        public string getGPS()
        {
            List<VendorGps> listmodel = new List<VendorGps>();
            foreach (Context.VendorGps item in RepoVendor.FindAll())
            {
                listmodel.Add(new VendorGps(item));
            }
            return new JavaScriptSerializer().Serialize(listmodel);
        }

        #region import export
        [MyAuthorize(Menu = "Vendor Gps", Action="create")]
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
                                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null)
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 6].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out resId))
                                            id = resId;                                    
                                    }
                                    //cara gancang ngarah teu kudu aya pengecekan tiap field
                                    Context.VendorGps dbitem = new Context.VendorGps();
                                    try
                                    {
                                        if (id != 0)
                                            dbitem = RepoVendor.FindByPK(id);

                                        dbitem.Nama = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        dbitem.Telp = workSheet.Cells[rowIterator, 3].Value.ToString();
                                        dbitem.Email = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        dbitem.Web = workSheet.Cells[rowIterator, 5].Value.ToString();

                                        RepoVendor.save(dbitem, UserPrincipal.id);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            //sheet 2
                            workSheet = currentSheet.Where(s => s.Index == 2).FirstOrDefault();
                            noOfRow = workSheet.Dimension.End.Row;
                            
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null)
                                {
                                    if (workSheet.Cells[rowIterator, 6].Value.ToString() != null && workSheet.Cells[rowIterator, 7].Value.ToString() != null)
                                    {
                                        //edit
                                        try
                                        {
                                            Context.VendorGps db = RepoVendor.FindByPK(int.Parse(workSheet.Cells[rowIterator, 6].Value.ToString()));
                                            int iditem = int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString());
                                            Context.Kontak dbitem = db.ListKontak.Where(d => d.Id == iditem).FirstOrDefault();
                                            dbitem.Nama = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            dbitem.IdJabatan = RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                                            dbitem.Hp = workSheet.Cells[rowIterator, 4].Value.ToString();
                                            dbitem.Email = workSheet.Cells[rowIterator, 5].Value.ToString();
                                            RepoVendor.save(db, UserPrincipal.id);
                                        }
                                        catch (Exception)
                                        {
                                            
                                        }
                                    }
                                    else
                                    { 
                                        //add
                                        try
                                        {
                                            Context.VendorGps db = RepoVendor.FindByPK(int.Parse(workSheet.Cells[rowIterator, 6].Value.ToString()));
                                            Context.Kontak dbitem = new Context.Kontak();
                                            dbitem.Nama = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            dbitem.IdJabatan = RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                                            dbitem.Hp = workSheet.Cells[rowIterator, 4].Value.ToString();
                                            dbitem.Email = workSheet.Cells[rowIterator, 5].Value.ToString();
                                            db.ListKontak.Add(dbitem);
                                            RepoVendor.save(db, UserPrincipal.id);
                                        }
                                        catch (Exception)
                                        {

                                        }
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
        [MyAuthorize(Menu = "Vendor Gps", Action="read")]
        public FileContentResult Export()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.VendorGps> dbitems = RepoVendor.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");
            ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Sheet 2");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Nama vendor";
            ws.Cells[1, 2].Value = "Alamat";
            ws.Cells[1, 3].Value = "Telepon";
            ws.Cells[1, 4].Value = "Email";
            ws.Cells[1, 5].Value = "Website";
            ws.Cells[1, 6].Value = "Id Database";

            ws2.Cells[1, 1].Value = "Nama Vendor";
            ws2.Cells[1, 2].Value = "Nama";
            ws2.Cells[1, 3].Value = "Jabatan";
            ws2.Cells[1, 4].Value = "Handphone";
            ws2.Cells[1, 5].Value = "Email";
            ws2.Cells[1, 6].Value = "Id Header";
            ws2.Cells[1, 7].Value = "Id Database";

            // Inserts Data
            int idx = 0;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].Nama;
                ws.Cells[i + 2, 2].Value = dbitems[i].Alamat;
                ws.Cells[i + 2, 3].Value = dbitems[i].Telp;
                ws.Cells[i + 2, 4].Value = dbitems[i].Email;
                ws.Cells[i + 2, 5].Value = dbitems[i].Web;
                ws.Cells[i + 2, 6].Value = dbitems[i].Id;
                foreach (Context.Kontak item in dbitems[i].ListKontak)
                {
                    ws2.Cells[idx + 2, 1].Value = item.VendorGps.Nama;
                    ws2.Cells[idx + 2, 2].Value = item.Nama;
                    ws2.Cells[idx + 2, 3].Value = item.LookUpCodeJabatan.Nama;
                    ws2.Cells[idx + 2, 4].Value = item.Hp;
                    ws2.Cells[idx + 2, 5].Value = item.Email;
                    ws2.Cells[idx + 2, 6].Value = item.IdVendor;
                    ws2.Cells[idx + 2, 7].Value = item.Id;
                    idx++;
                }
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Product.xls";

            return fsr;
        }
        #endregion
    }
}