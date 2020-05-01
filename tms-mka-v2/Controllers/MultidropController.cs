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
    public class MultidropController : BaseController
    {
        private IMultiDropRepo RepoMultidrop;
        private ILocationRepo RepoLocation;
        public MultidropController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IMultiDropRepo repoMultidrop, ILocationRepo repoLocation)
            : base(repoBase, repoLookup)
        {
            RepoMultidrop = repoMultidrop;
            RepoLocation = repoLocation;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Multidrop").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Multidrop> items = RepoMultidrop.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<MultiDrop> ListModel = new List<MultiDrop>();
            foreach (Context.Multidrop item in items)
            {
                ListModel.Add(new MultiDrop(item));
            }

            int total = RepoMultidrop.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public ActionResult Add()
        {
            MultiDrop model = new MultiDrop();
            ViewBag.Locations = RepoLocation.FindAll();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(MultiDrop model)
        {
            if (ModelState.IsValid)
            {
                Context.Multidrop dbitem = new Context.Multidrop();
                model.setDb(dbitem);
                
                RepoMultidrop.save(dbitem);

                return RedirectToAction("Index");
            }
            ViewBag.Locations = RepoLocation.FindAll();
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.Multidrop dbitem = RepoMultidrop.FindByPK(id);
            MultiDrop model = new MultiDrop(dbitem);
            ViewBag.name = model.tujuan;
            ViewBag.Locations = RepoLocation.FindAll();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(MultiDrop model)
        {
            if (ModelState.IsValid)
            {
                Context.Multidrop dbitem = RepoMultidrop.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoMultidrop.save(dbitem);

                return RedirectToAction("Index");
            }
            ViewBag.Locations = RepoLocation.FindAll();
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Multidrop dbItem = RepoMultidrop.FindByPK(id);

            RepoMultidrop.delete(dbItem);

            return Json(response);
        }
        #region options
        public string GetLocation(string Type)
        {
            List<Context.Location> dbitems = new List<Context.Location>();
            dbitems = RepoLocation.FindAll();
            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string GetMultidrop(string Type)
        {
            List<Context.Multidrop> dbitems = new List<Context.Multidrop>();
            dbitems = RepoMultidrop.FindAll();
            return new JavaScriptSerializer().Serialize(dbitems);
        }
        #endregion

        #region import export
        public string Upload(IEnumerable<HttpPostedFileBase> filesMultidrop)
        {
            ResponeModel response = new ResponeModel();
            if (filesMultidrop != null)
            {
                foreach (var file in filesMultidrop)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            bool isSaved = true;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {                                
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null &&
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null)
                                {
                                    int id = 0;
                                    int resId;

                                    if (workSheet.Cells[rowIterator, 5].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 5].Value.ToString(), out resId))
                                        {
                                            id = resId;
                                        }
                                    }

                                    List<String> listLocation = new List<string>();
                                    Context.Multidrop dbMultiDrop = new Context.Multidrop();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbMultiDrop = RepoMultidrop.FindByPK(id);
                                        }

                                        dbMultiDrop.JumlahKota = int.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());
                                        dbMultiDrop.WaktuTempuh = int.Parse(workSheet.Cells[rowIterator, 2].Value.ToString());
                                        dbMultiDrop.WaktuKerja = int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());

                                        Context.Location revLocation = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString());
                                        if (revLocation != null)
                                        {
                                            listLocation.Add(revLocation.Nama);
                                        }

                                        for (int x = rowIterator + 1; x <= noOfRow; x++)
                                        {
                                            if (workSheet.Cells[x, 4].Value != null && workSheet.Cells[x, 1].Value == null &&
                                                workSheet.Cells[x, 2].Value == null && workSheet.Cells[x, 3].Value == null)
                                            {
                                                Context.Location loc = RepoLocation.FindByCode(workSheet.Cells[x, 4].Value.ToString());
                                                if (loc != null)
                                                {
                                                    listLocation.Add(loc.Nama);
                                                }
                                                else
                                                {
                                                    isSaved = false;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //isSaved = false;
                                                break;
                                            }
                                        }
                                        if(listLocation.Count > 1)
                                            dbMultiDrop.tujuan = string.Join(" - ", listLocation);

                                        if (RepoMultidrop.FindByTujuan(dbMultiDrop.tujuan.ToString())!=null)
                                            continue;

                                        if (isSaved)
                                            RepoMultidrop.save(dbMultiDrop);
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
            List<Context.Multidrop> dbMultidrop = RepoMultidrop.FindAll();
            IList<String> listTujuan = new List<string>();
            String dataTujuan = "";
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Jumlah Kota";
            ws.Cells[1, 2].Value = "Tambahan Waktu Tempuh";
            ws.Cells[1, 3].Value = "Tambahan Waktu Kerja";
            ws.Cells[1, 4].Value = "Tujuan";            
            ws.Cells[1, 5].Value = "Id Database";

            // Inserts Data
            int idx = 0;
            int nextRow = 0;
            
            for (int i = 0; i < dbMultidrop.Count(); i++)
            {
                
                ws.Cells[i + 2 + nextRow, 1].Value = dbMultidrop[i].JumlahKota;
                ws.Cells[i + 2 + nextRow, 2].Value = dbMultidrop[i].WaktuTempuh;
                ws.Cells[i + 2 + nextRow, 3].Value = dbMultidrop[i].WaktuKerja;
                ws.Cells[i + 2 + nextRow, 5].Value = dbMultidrop[i].Id;
                dataTujuan = dbMultidrop[i].tujuan;
                listTujuan = dataTujuan.Split(new String[] { " - " }, StringSplitOptions.None);
                
                for (idx = 0; idx <= listTujuan.Count -1; idx++ )
                {
                    Context.Location loc = RepoLocation.FindByNama(listTujuan[idx]);
                    if (loc != null)
                        ws.Cells[i + 2 + nextRow, 4].Value = loc.Code;
                    nextRow++;
                }
                nextRow--;
                
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Export Data Multidrop.xls";

            return fsr;
        }

        #endregion
    }
}