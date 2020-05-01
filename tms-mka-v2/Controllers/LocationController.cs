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
using System.Threading.Tasks;

namespace tms_mka_v2.Controllers
{
    public class LocationController : BaseController
    {
        private ILocationRepo RepoLocation;
        public LocationController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ILocationRepo repoLocation)
            : base(repoBase, repoLookup)
        {
            RepoLocation = repoLocation;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Location").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Location> items = RepoLocation.FindAll(param.Skip, param.Take , (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Location> ListModel = new List<Location>();
            foreach (Context.Location item in items)
            {
                ListModel.Add(new Location(item));
            }

            int total = RepoLocation.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingCombo(int id = 0)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            param.Take = 50;

            List<Context.Location> items = RepoLocation.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Location> ListModel = new List<Location>();
            foreach (Context.Location item in items)
            {
                ListModel.Add(new Location(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string BindingComboKotaKec()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            param.Take = 50;

            if (param.Filters.Filters == null)
            {
                param.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
                param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
                {
                    Field = "Type",
                    Operator = "eq",
                    Value = "Kab/Kota",
                });
                param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
                {
                    Field = "Type",
                    Operator = "eq",
                    Value = "Kecamatan",
                });
            }

            List<Context.Location> items = RepoLocation.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
                //.Where(d => d.Nama.ToLower().Contains(str) && (d.Type == "Kab/Kota" || d.Type == "Kecamatan")).ToList();

            List<Location> ListModel = new List<Location>();
            foreach (Context.Location item in items)
            {
                ListModel.Add(new Location(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string BindingComboType(string type)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            param.Take = 5;
            if (param.Filters.Filters == null)
                param.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
            param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
            {
                Field = "Type",
                Operator = "eq",
                Value = type,
            });

            List<Context.Location> dbitems = RepoLocation.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);


            //GridRequestParameters param = GridRequestParameters.Current;
            //string str = param.Filters.Filters == null ? "" : param.Filters.Filters[0].Value;

            //List<Context.Location> items = new List<Context.Location>();
            //if (str != "")
            //    items = RepoLocation.FindAll().Where(d => d.Type == type && d.Nama.ToLower().Contains(str)).ToList();

            List<Location> ListModel = new List<Location>();
            foreach (Context.Location item in dbitems)
            {
                ListModel.Add(new Location(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public ActionResult Add()
        {
            Location model = new Location();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(Location model)
        {
            if (ModelState.IsValid)
            {
                //more validation
                bool IsExist = RepoLocation.IsExist(model.Code);

                if (IsExist)
                {
                    ModelState.AddModelError("Code", "Code sudah terdaftar");
                    return View("Form", model);
                }

                if (model.Type != "Provinsi")
                {
                    if (!model.ParentId.HasValue)
                    {
                        ModelState.AddModelError("ParentId", "Parent harus diisi");
                        return View("Form", model);
                    }
                }
                
                Context.Location dbitem = new Context.Location();
                model.setDb(dbitem);
                
                RepoLocation.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.Location dbitem = RepoLocation.FindByPK(id);
            Location model = new Location(dbitem);
            ViewBag.name = model.Nama;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(Location model)
        {
            if (ModelState.IsValid)
            {
                //more validation
                bool IsExist = RepoLocation.IsExist(model.Code, model.Id);

                if (IsExist)
                {
                    ModelState.AddModelError("Code", "Code sudah terdaftar");
                    return View("Form", model);
                }

                if (model.Type != "Provinsi")
                {
                    if (!model.ParentId.HasValue)
                    {
                        ModelState.AddModelError("ParentId", "Parent harus diisi");
                        return View("Form", model);
                    }
                }

                Context.Location dbitem = RepoLocation.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoLocation.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Location dbItem = RepoLocation.FindByPK(id);

            RepoLocation.delete(dbItem);

            return Json(response);
        }

        #region option
        public string GetParent(string Type)
        {
            List<Context.Location> dbitems = new List<Context.Location>();
            if (Type == "Provinsi")
            {

            }
            else if (Type == "Kab/Kota")
            {
                dbitems = RepoLocation.FindAll().Where(d => d.Type == "Provinsi").ToList();
            }
            else if (Type == "Kecamatan")
            {
                dbitems = RepoLocation.FindAll().Where(d => d.Type == "Kab/Kota").ToList();
            }
            else if (Type == "Kelurahan")
            {
                dbitems = RepoLocation.FindAll().Where(d => d.Type == "Kecamatan").ToList();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            return serializer.Serialize(dbitems);
        }
        public string GetLocationByCode(string code)
        {
            Context.Location dbitems = RepoLocation.FindAll().Where(d => d.Code == code).FirstOrDefault();

            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string GetLocationByName(string name)
        {
            Context.Location dbitems = RepoLocation.FindByNama(name);

            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string BindingComboProvinsi()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            param.Take = 50;

            if (param.Filters.Filters == null)
            {
                param.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
                param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
                {
                    Field = "Type",
                    Operator = "eq",
                    Value = "Provinsi",
                });
                param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
                {
                    Field = "Type",
                    Operator = "eq",
                    Value = "Provinsi",
                });
            }

            List<Context.Location> items = RepoLocation.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Location> ListModel = new List<Location>();
            foreach (Context.Location item in items)
            {
                ListModel.Add(new Location(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string GetLocationByType(string type)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            param.Take = 5;
            if (param.Filters.Filters == null)
                param.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
            param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
            {
                Field = "Type",
                Operator = "eq",
                Value = type,
            });

            List<Context.Location> dbitems = RepoLocation.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            //List<Context.Location> dbitems = RepoLocation.FindAll().Where(d => d.Type == type).ToList();

            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string GetLocationRute()
        {
            List<Context.Location> dbitems = RepoLocation.FindAll().Where(d => d.Type == "Kab/Kota" || d.Type == "Kecamatan" ).ToList();

            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string BindingComboKabupaten()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            param.Take = 50;

            if (param.Filters.Filters == null)
            {
                param.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
                param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
                {
                    Field = "Type",
                    Operator = "eq",
                    Value = "Kab/Kota",
                });
                param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
                {
                    Field = "Type",
                    Operator = "eq",
                    Value = "Kab/Kota",
                });
            }

            List<Context.Location> items = RepoLocation.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Location> ListModel = new List<Location>();
            foreach (Context.Location item in items)
            {
                ListModel.Add(new Location(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        #endregion

        #region upload
        public string UploadLokasi(IEnumerable<HttpPostedFileBase> files)
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
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null)
                                {
                                    try
                                    {
                                        int id = 0;
                                        int resId;
                                        Context.Location dbitem = new Context.Location();
                                        //if (workSheet.Cells[rowIterator, 5].Value != null)
                                        if (workSheet.Cells[rowIterator, 8].Value != null)
                                        {
                                            if (int.TryParse(workSheet.Cells[rowIterator, 8].Value.ToString(), out resId))
                                            {
                                                id = resId;
                                            }
                                        }

                                        string code = workSheet.Cells[rowIterator, 3].Value.ToString().Replace(',', '.');
                                        
                                        if (RepoLocation.IsExist(code, id)) continue;

                                        if (id != 0)
                                        {
                                            dbitem = RepoLocation.FindByCode(code);
                                        }

                                        //parent
                                        if (workSheet.Cells[rowIterator, 2].Value.ToString() != "PROV")
                                        {
                                            dbitem.ParentId = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        }
                                        //code
                                        dbitem.Code = code;
                                        //cek type (PROV, KOTA, KAB)
                                        //PROV
                                        if (workSheet.Cells[rowIterator, 2].Value.ToString() == "PROV")
                                        {
                                            dbitem.Type = "Provinsi";
                                            dbitem.Nama = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        }
                                        else if (workSheet.Cells[rowIterator, 2].Value.ToString() == "KOTA")
                                        {
                                            dbitem.Type = "Kab/Kota";
                                            dbitem.Nama = "Kota " + workSheet.Cells[rowIterator, 4].Value.ToString();
                                        }
                                        else if (workSheet.Cells[rowIterator, 2].Value.ToString() == "KAB")
                                        {
                                            dbitem.Type = "Kab/Kota";
                                            dbitem.Nama = "Kabupaten " + workSheet.Cells[rowIterator, 4].Value.ToString();
                                        }
                                        else if (workSheet.Cells[rowIterator, 2].Value.ToString() == "KEC")
                                        {
                                            dbitem.Type = "Kecamatan";
                                            dbitem.Nama = "Kecamatan " + workSheet.Cells[rowIterator, 4].Value.ToString();
                                        }
                                        else if (workSheet.Cells[rowIterator, 2].Value.ToString() == "DES")
                                        {
                                            dbitem.Type = "Kelurahan";
                                            dbitem.Nama = "Kelurahan " + workSheet.Cells[rowIterator, 4].Value.ToString();
                                        }

                                      Task t = Task.Run( () => RepoLocation.save(dbitem) );
                                      t.Wait();
                                    }
                                    catch (Exception e)
                                    {
                                        string msg = e.Message.ToString();
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
            List<Context.Location> dbitems = RepoLocation.FindAll().OrderBy(d=>d.Code).ToList();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Parent";
            ws.Cells[1, 2].Value = "Type";
            ws.Cells[1, 3].Value = "Code";
            ws.Cells[1, 4].Value = "Nama";
            ws.Cells[1, 5].Value = "Id Database";
            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].LocationParent != null ? dbitems[i].LocationParent.Code.ToString() : "";
                string type_ = "";
                if (dbitems[i].Type == "Provinsi")
                {
                    type_ = "PROV";
                }
                else if (dbitems[i].Type == "Kab/Kota")
                {
                    if (dbitems[i].Nama.Contains("Kabupaten"))
                    {
                        type_ = "KAB";
                    }
                    else {
                        type_ = "KOTA";
                    }
                }
                else if (dbitems[i].Type == "Kecamatan")
                {
                    type_ = "KEC";
                }
                else if (dbitems[i].Type == "Kelurahan")
                {
                    type_ = "DES";
                }
                ws.Cells[i + 2, 2].Value = type_;
                ws.Cells[i + 2, 3].Value = dbitems[i].Code.ToString();
                ws.Cells[i + 2, 4].Value = dbitems[i].Nama;
                ws.Cells[i + 2, 5].Value = dbitems[i].Id;

            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Lokasi.xls";

            return fsr;
        }
        #endregion
    }
}