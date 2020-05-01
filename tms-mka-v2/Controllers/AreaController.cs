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
    public class AreaController : BaseController
    {
        private IAreaRepo RepoArea;
        private ILocationRepo RepoLocation;
        private IRuteRepo RepoRute;
        public AreaController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IAreaRepo repoArea, ILocationRepo repoLocation, IRuteRepo repoRute)
            : base(repoBase, repoLookup)
        {
            RepoArea = repoArea;
            RepoLocation = repoLocation;
            RepoRute = repoRute;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "MasterArea").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.MasterArea> items = RepoArea.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<MasterArea> ListModel = new List<MasterArea>();
            foreach (Context.MasterArea item in items)
            {
                ListModel.Add(new MasterArea(item));
            }

            int total = RepoArea.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public ActionResult Add()
        {
            MasterArea model = new MasterArea();
            return View("Form", model);
        }
        public string BindingCal()
        {
            List<Context.MasterArea> dbArea = RepoArea.FindAll();

            List<MasterArea> modelArea = new List<MasterArea>();
            foreach (var item in dbArea)
            {
                modelArea.Add(new Models.MasterArea(item));
            }

            return new JavaScriptSerializer().Serialize(modelArea);
        }

        [HttpPost]
        public ActionResult Add(MasterArea model)
        {
            if (ModelState.IsValid)
            {
                Context.MasterArea dbitem = new Context.MasterArea();
                model.setDb(dbitem);
                //generate code
                dbitem.Urutan = RepoArea.getUrutan() + 1;
                dbitem.Kode = RepoArea.generateCode(dbitem.Urutan);

                RepoArea.save(dbitem);

                return RedirectToAction("Index");
            }
            LocationArea[] result = JsonConvert.DeserializeObject<LocationArea[]>(model.strArea);
            model.listArea = result.ToList();
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.MasterArea dbitem = RepoArea.FindByPK(id);
            MasterArea model = new MasterArea(dbitem);
            ViewBag.name = model.Kode;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(MasterArea model)
        {
            if (ModelState.IsValid)
            {
                Context.MasterArea dbitem = RepoArea.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoArea.save(dbitem);

                return RedirectToAction("Index");
            }
            LocationArea[] result = JsonConvert.DeserializeObject<LocationArea[]>(model.strArea);
            model.listArea = result.ToList();
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.MasterArea dbItem = RepoArea.FindByPK(id);

            RepoArea.delete(dbItem);

            return Json(response);
        }
        #region options
        public string GetLocation(string Type)
        {
            List<Context.Location> dbitems = new List<Context.Location>();
            dbitems = RepoLocation.FindAll();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.Serialize(dbitems);
        }
        public string GetAreaByLocation(int idLoc)
        {
            var dbitems = RepoArea.FindAll().Where(d => d.ListLocationArea.Any(c => c.LocationId.Value == idLoc)).Select(s => new { Id = s.Id, Kode = s.Kode, Nama = s.Nama }).ToList();
            return new JavaScriptSerializer().Serialize(dbitems);
        }
        #endregion

        #region import export
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

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null)
                                {
                                    int id = 0;
                                    int resId;

                                    if (workSheet.Cells[rowIterator, 3].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 3].Value.ToString(), out resId))
                                        {
                                            id = resId;
                                        }
                                    }


                                    Context.MasterArea dbitem = new Context.MasterArea();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoArea.FindByPK(id);
                                        }
                                        else
                                        {
                                            dbitem.Urutan = RepoArea.getUrutan() + 1;
                                            dbitem.Kode = RepoArea.generateCode(dbitem.Urutan);
                                        }

                                        // parent

                                        if (RepoArea.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString(), id))
                                        {
                                            continue;
                                        }

                                        dbitem.Nama = workSheet.Cells[rowIterator, 1].Value.ToString();

                                        //child
                                        int x = 0;
                                        List<int> listIdData = new List<int>();
                                        List<Context.Rute> listRute = RepoRute.FindAll();
                                        List<Context.ListLocationArea> listAreaAdd = new List<Context.ListLocationArea>();
                                        //cek data existing dengan excell
                                        for (x = rowIterator; x <= noOfRow; x++)
                                        {
                                            if (workSheet.Cells[x, 1].Value == null || x == rowIterator)
                                            {
                                                try
                                                {
                                                    Context.ListLocationArea listLocationArea = new Context.ListLocationArea();
                                                    int? idLoc = RepoLocation.FindByCode(workSheet.Cells[x, 2].Value.ToString()).Id;
                                                    if (idLoc.HasValue)
                                                    {
                                                        if (id != 0)
                                                        {
                                                            if (dbitem.ListLocationArea.Any(d => d.LocationId == idLoc))
                                                            {
                                                                listIdData.Add(idLoc.Value);
                                                            }
                                                            else
                                                            {
                                                                listLocationArea.LocationId = idLoc;
                                                                listAreaAdd.Add(listLocationArea);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            listLocationArea.LocationId = idLoc;
                                                            listAreaAdd.Add(listLocationArea);
                                                        }
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    
                                                }
                                            }
                                            else
                                            {
                                                rowIterator = x - 1;
                                                break;
                                            }
                                        }

                                        List<Context.ListLocationArea> dbremove = dbitem.ListLocationArea.Where(d => !listIdData.Contains(d.Id) && d.Id != 0).ToList();
                                        //remove
                                        foreach (Context.ListLocationArea item in dbremove)
                                        {
                                            if (listRute.Any(r => r.IdAsal == item.LocationId && r.IdTujuan == item.LocationId))
                                            {

                                            }
                                            else
                                            {
                                                dbitem.ListLocationArea.Remove(item);
                                            }
                                        }
                                        //add
                                        foreach (Context.ListLocationArea item in listAreaAdd)
                                        {
                                            dbitem.ListLocationArea.Add(item);
                                        }
                                        
                                        RepoArea.save(dbitem);

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
            List<Context.MasterArea> dbitems = RepoArea.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Nama Area";
            ws.Cells[1, 2].Value = "Lokasi";
            ws.Cells[1, 3].Value = "Id Database";

            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[idx, 1].Value = dbitems[i].Nama;
                ws.Cells[idx, 3].Value = dbitems[i].Id;

                int idx2 = idx;
                foreach (Context.ListLocationArea item in dbitems[i].ListLocationArea)
                {
                    ws.Cells[idx2, 2].Value = item.Location.Code;
                    idx2++;
                }
                idx = idx2;
                //idx++;
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Area.xls";

            return fsr;
        }
        #endregion
    }
}