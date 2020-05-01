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
    public class DataGPSController : BaseController
    {
        private IDataGPSRepo RepoDataGps;
        private IDataTruckRepo RepoDataTruck;
        private IVendorGpsRepo RepoVendorGPS;
        public DataGPSController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDataGPSRepo repoDataGps, IDataTruckRepo repoDataTruck, IVendorGpsRepo repoVendorGPS)
            : base(repoBase, repoLookup)
        {
            RepoDataGps = repoDataGps;
            RepoDataTruck = repoDataTruck;
            RepoVendorGPS = repoVendorGPS;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DataGPS").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DataGPS> items = RepoDataGps.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<DataGPS> ListModel = new List<DataGPS>();
            foreach (Context.DataGPS item in items)
            {
                ListModel.Add(new DataGPS(item));
            }

            int total = RepoDataGps.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingHistory(int id)
        {
            Context.DataGPS db = RepoDataGps.FindByPK(id);

            List<DataGPS> listmodel = new List<DataGPS>();

            foreach (Context.DataGPSHistory item in db.DataGPSHistory.OrderByDescending(d => d.Id))
            {
                listmodel.Add(new DataGPS(item));
            }

            return new JavaScriptSerializer().Serialize(listmodel);
        }
        public ActionResult Add()
        {
            DataGPS model = new DataGPS();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DataGPS model)
        {
            if (ModelState.IsValid)
            {
                if (RepoDataGps.IsBoxExist(model.IdDataTruck.Value))
                {
                    ModelState.AddModelError("IdDataTruck", "Truck sudah tepasang gps, harap ganti dengan truck yang lain");
                    return View("Form", model);
                }
                Context.DataGPS dbitem = new Context.DataGPS();
                model.SetDb(dbitem);
                //generate code
                dbitem.urutan = RepoDataGps.getUrutan() + 1;
                dbitem.NoGPS = RepoDataGps.generateCode(dbitem.urutan);
                
                Context.DataGPSHistory dbitemHistory = new Context.DataGPSHistory();
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                dbitemHistory.Vehicle = RepoDataTruck.FindByPK(dbitem.IdDataTruck.Value).VehicleNo;
                dbitemHistory.NoGPS = dbitem.NoGPS;
                dbitemHistory.strVendor = dbitem.IdVendor.HasValue ? RepoVendorGPS.FindByPK(dbitem.IdVendor.Value).Nama : "";
                dbitem.DataGPSHistory.Add(dbitemHistory);
                Context.DataTruckGPSHistory dbtruckHistory = new Context.DataTruckGPSHistory();
                model.SetDbTruckHistory(dbtruckHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                RepoDataGps.save(dbitem, UserPrincipal.id, dbitemHistory);

                Context.DataTruck dbtruck = RepoDataTruck.FindByPK(model.IdDataTruck.Value);
                dbtruckHistory.Vehicle = dbtruck.VehicleNo;
                dbtruckHistory.NoGPS = dbitem.NoGPS;
                dbtruckHistory.strVendor = dbitem.IdVendor.HasValue ? RepoVendorGPS.FindByPK(dbitem.IdVendor.Value).Nama : "";
                dbtruck.DataTruckGPSHistory.Add(dbtruckHistory);
                RepoDataTruck.save(dbtruck, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.DataGPS dbitem = RepoDataGps.FindByPK(id);
            DataGPS model = new DataGPS(dbitem);
            ViewBag.name = model.NoGPS;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(DataGPS model)
        {
            if (ModelState.IsValid)
            {
                if (RepoDataGps.IsBoxExist(model.IdDataTruck.Value, model.Id))
                {
                    ModelState.AddModelError("IdDataTruck", "Truck sudah tepasang gps, harap ganti dengan truck yang lain");
                    return View("Form", model);
                }
                Context.DataGPS dbitem = RepoDataGps.FindByPK(model.Id);
                model.SetDb(dbitem);
                Context.DataGPSHistory dbitemHistory = new Context.DataGPSHistory();
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                dbitem.DataGPSHistory.Add(dbitemHistory);
                Context.DataTruckGPSHistory dbtruckHistory = new Context.DataTruckGPSHistory();
                model.SetDbTruckHistory(dbtruckHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                RepoDataGps.save(dbitem, UserPrincipal.id, dbitemHistory);

                Context.DataTruck dbtruck = RepoDataTruck.FindByPK(model.IdDataTruck.Value);
                dbtruck.DataTruckGPSHistory.Add(dbtruckHistory);
                RepoDataTruck.save(dbtruck, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.DataGPS dbItem = RepoDataGps.FindByPK(id);

            RepoDataGps.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        #region options

        #endregion

        #region export import
        public string UploadDataGPS(IEnumerable<HttpPostedFileBase> filesDataGPS)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesDataGPS != null)
            {
                foreach (var file in filesDataGPS)
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
                                if (workSheet.Cells[rowIterator, 1].Value != null /*&& workSheet.Cells[rowIterator, 2].Value != null &&
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null &&
                                    workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null &&
                                    workSheet.Cells[rowIterator, 9].Value != null*/)
                                {
                                    int id = 0;
                                    int resId;
                                    if (workSheet.Cells[rowIterator, 10].Value != null){
                                        if (int.TryParse(workSheet.Cells[rowIterator, 10].Value.ToString(), out resId))
                                            id = resId;
                                    }
                                    
                                    Context.DataGPS dbitem = new Context.DataGPS();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoDataGps.FindByPK(id);
                                            if (RepoDataGps.IsBoxExist(RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id, id))
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (RepoDataGps.IsBoxExist(RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id))
                                            {
                                                continue;
                                            }
                                            dbitem.urutan = RepoDataGps.getUrutan() + 1;
                                            dbitem.NoGPS = RepoDataGps.generateCode(dbitem.urutan);
                                        }
                                        dbitem.IdDataTruck = RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        dbitem.IdVendor = workSheet.Cells[rowIterator, 2].Value == null ? (int?)null : RepoVendorGPS.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        dbitem.ModelGps = workSheet.Cells[rowIterator, 3].Value == null ? null : workSheet.Cells[rowIterator, 3].Value.ToString();
                                        dbitem.NoDevice = workSheet.Cells[rowIterator, 4].Value == null ? null : workSheet.Cells[rowIterator, 4].Value.ToString();
                                        dbitem.SensorSuhu = workSheet.Cells[rowIterator, 5].Value == null ? (bool?)null : bool.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        dbitem.SensorPintu = workSheet.Cells[rowIterator, 6].Value == null ? (bool?)null : bool.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                                        dbitem.Tahun = workSheet.Cells[rowIterator, 7].Value == null ? (int?)null : int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString());
                                        dbitem.TanggalPasang = workSheet.Cells[rowIterator, 8].Value == null ? (DateTime?)null : DateTime.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                                        dbitem.TanggalGaransi = workSheet.Cells[rowIterator, 9].Value == null ? (DateTime?)null : DateTime.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());

                                        //history
                                        Context.DataGPSHistory dbhistory = new Context.DataGPSHistory();
                                        dbhistory.NoGPS =  dbitem.NoGPS;
                                        dbhistory.Vehicle = workSheet.Cells[rowIterator, 1].Value == null ? null : workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbhistory.strVendor = workSheet.Cells[rowIterator, 2].Value == null ? null : workSheet.Cells[rowIterator, 2].Value.ToString();
                                        dbhistory.ModelGps =  dbitem.ModelGps;
                                        dbhistory.NoDevice =  dbitem.NoDevice;
                                        dbhistory.SensorSuhu =  dbitem.SensorSuhu;
                                        dbhistory.SensorPintu =  dbitem.SensorPintu;
                                        dbhistory.Tahun =  dbitem.Tahun;
                                        dbhistory.TanggalPasang =  dbitem.TanggalPasang;
                                        dbhistory.TanggalGaransi =  dbitem.TanggalGaransi;
                                        dbhistory.Tanggal = DateTime.Now;
                                        dbhistory.Username = UserPrincipal.firstname + " " + UserPrincipal.lastname;

                                        
                                        dbitem.DataGPSHistory.Add(dbhistory);

                                        RepoDataGps.save(dbitem, UserPrincipal.id, dbhistory);
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

        public FileContentResult ExportDataGPS()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.DataGPS> dbitems = RepoDataGps.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Vehicle No";
            ws.Cells[1, 2].Value = "Vendor";
            ws.Cells[1, 3].Value = "Model";
            ws.Cells[1, 4].Value = "No Device";
            ws.Cells[1, 5].Value = "Sensor Suhu";
            ws.Cells[1, 6].Value = "Sensor Pintu";
            ws.Cells[1, 7].Value = "Tahun";
            ws.Cells[1, 8].Value = "Tgl Pasang";
            ws.Cells[1, 9].Value = "Garansi s/d";
            ws.Cells[1, 10].Value = "Id Database";


            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].DataTruck.VehicleNo;
                ws.Cells[i + 2, 2].Value = dbitems[i].VendorGps.Nama;
                ws.Cells[i + 2, 4].Value = dbitems[i].NoDevice;
                ws.Cells[i + 2, 3].Value = dbitems[i].ModelGps;
                ws.Cells[i + 2, 7].Value = dbitems[i].Tahun;
                ws.Cells[i + 2, 5].Value = dbitems[i].SensorSuhu;
                ws.Cells[i + 2, 6].Value = dbitems[i].SensorPintu;
                ws.Cells[i + 2, 8].Value = Convert.ToDateTime(dbitems[i].TanggalPasang).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 9].Value = Convert.ToDateTime(dbitems[i].TanggalGaransi).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 10].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Data_GPS.xlsx";

            return fsr;
        }
        #endregion
    }
}