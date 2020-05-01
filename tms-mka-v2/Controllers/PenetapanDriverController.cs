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
    public class PenetapanDriverController : BaseController
    {
        private IPenetapanaDriverRepo RepoPenetapanDriver;
        private IDriverRepo RepoDriver;
        private IDataTruckRepo RepoTruck;
        private IUserRepo RepoUser;
        private IAuditrailRepo RepoAuditrail;
        public PenetapanDriverController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IPenetapanaDriverRepo repoPenetapanDriver,
            IDriverRepo repoDriver, IDataTruckRepo repoTruck, IUserRepo repoUser, IAuditrailRepo repoAuditTrail)
            : base(repoBase, repoLookup)
        {
            RepoPenetapanDriver = repoPenetapanDriver;
            RepoDriver = repoDriver;
            RepoTruck = repoTruck;
            RepoUser = repoUser;
            RepoAuditrail = repoAuditTrail;
        }

        [MyAuthorize(Menu = "Driver Batangan", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "PenetapanDriver").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            //List<Context.PenetapanDriver> items = RepoPenetapanDriver.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<Context.PenetapanDriver> items = RepoPenetapanDriver.FindAll();
            List<PenetapanDriver> ListModel = new List<PenetapanDriver>();
            foreach (Context.PenetapanDriver item in items)
            {
                ListModel.Add(new PenetapanDriver(item));
            }

            int total = RepoPenetapanDriver.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
        public string BindingHistory(int id)
        {
            Context.PenetapanDriver db = RepoPenetapanDriver.FindByPK(id);

            return new JavaScriptSerializer().Serialize(db.PenetapanDriverHistory.
                Select(d => new {
                    Driver1 =d.Driver1,
                    Driver2 = d.Driver2,
                    CreatedBy = d.CreatedBy,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedDate = d.ModifiedDate
                }));
        }
        [MyAuthorize(Menu = "Driver Batangan", Action = "create")]
        public ActionResult Add()
        {
            PenetapanDriver model = new PenetapanDriver();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(PenetapanDriver model)
        {
            if (ModelState.IsValid)
            {
                bool isPalid = true;
                if (RepoPenetapanDriver.IsExist(model.IdDataTruck.Value))
                {
                    isPalid = false;
                    ModelState.AddModelError("IdDataTruck","Data truck sudah digunakan.");
                }

                if (RepoPenetapanDriver.isExistDriver(model.IdDriver1.Value, 0))
                {
                    isPalid = false;
                    ModelState.AddModelError("idDriver1", "Driver sudah digunakan.");
                }

                if (model.IdDriver2.HasValue)
                {
                    if (RepoPenetapanDriver.isExistDriver(model.IdDriver2.Value, 0))
                    {
                        isPalid = false;
                        ModelState.AddModelError("IdDriver2", "Driver sudah digunakan.");
                    }
                    if (model.IdDriver1 == model.IdDriver2)
                    {
                        isPalid = false;
                        ModelState.AddModelError("IdDriver1", "Driver tidak boleh sama.");
                        ModelState.AddModelError("IdDriver2", "Driver tidak boleh sama.");
                    }
                    if (model.IdDitetapkanOleh2 == null)
                    {
                        isPalid = false;
                        ModelState.AddModelError("IdDitetapkanOleh2", "Penetap Driver 2 Belum Ditentukan.");
                    }
                }

                if (model.IdDitetapkanOleh1 == null)
                {
                    isPalid = false;
                    ModelState.AddModelError("IdDitetapkanOleh1", "Penetap Driver 1 Belum Ditentukan.");
                }

                if (!isPalid)
                    return View("Form", model);

                Context.PenetapanDriver dbitem = new Context.PenetapanDriver();
                Context.PenetapanDriverHistory dbitemHistory = new Context.PenetapanDriverHistory();
                //history
                model.SetDb(dbitem, UserPrincipal.firstname + ' ' + UserPrincipal.lastname);
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                dbitemHistory.CreatedBy = UserPrincipal.firstname + " " + UserPrincipal.lastname;
                dbitemHistory.CreatedDate = DateTime.Now;
                dbitem.PenetapanDriverHistory.Add(dbitemHistory);
                //history driver
                Context.DataTruck dbtruck = RepoTruck.FindByPK(model.IdDataTruck.Value);

                Context.Driver dbdriver1 = RepoDriver.FindByPK(model.IdDriver1.Value);
                Context.DriverTruckHistory dbdriverHistory1 = new Context.DriverTruckHistory();
                dbdriver1.NoHp1 = model.NoHp1Driver1;
                dbdriver1.NoHp2 = model.NoHp2Driver1;
                dbdriverHistory1.Tanggal = DateTime.Now;
                dbdriverHistory1.Nopol = dbtruck.VehicleNo;
                dbdriverHistory1.Type = model.JenisTruck;
                dbdriver1.DriverTruckHistory.Add(dbdriverHistory1);
                RepoDriver.save(dbdriver1);
                string query = "UPDATE dbo.\"Driver\" SET \"NoHp1\" = " + dbdriver1.NoHp1 + ", \"NoHp2\" = " + dbdriver1.NoHp2 + " WHERE \"Id\" = " + dbdriver1.Id + ";INSERT INTO dbo.\"DriverTruckHistory\" (\"IdDriver\", " +
                    "\"Tanggal\", \"Type\", \"Nopol\") VALUES (" + dbdriverHistory1.IdDriver + ", " + dbdriverHistory1.Tanggal + ", " + dbdriverHistory1.Type + ", " + dbdriverHistory1.Nopol + ");";
                RepoAuditrail.SetAuditTrail(query, "Penetapan Driver", "Add", UserPrincipal.id);

                if (model.IdDriver2.HasValue)
                {
                    Context.Driver dbdriver2 = RepoDriver.FindByPK(model.IdDriver2.Value);
                    Context.DriverTruckHistory dbdriverHistory2 = new Context.DriverTruckHistory();
                    dbdriver2.NoHp1 = model.NoHp1Driver2;
                    dbdriver2.NoHp2 = model.NoHp2Driver2;
                    dbdriverHistory2.Tanggal = DateTime.Now;
                    dbdriverHistory2.Nopol = dbtruck.VehicleNo;
                    dbdriverHistory2.Type = model.JenisTruck;
                    dbdriver2.DriverTruckHistory.Add(dbdriverHistory2);
                    RepoDriver.save(dbdriver2);
                    query = "UPDATE dbo.\"Driver\" SET \"NoHp1\" = " + dbdriver2.NoHp1 + ", \"NoHp2\" = " + dbdriver2.NoHp2 + " WHERE \"Id\" = " + dbdriver2.Id + ";INSERT INTO dbo.\"DriverTruckHistory\" ( " +
                        "\"IdDriver\", \"Tanggal\", \"Type\", \"Nopol\") VALUES (" + dbdriverHistory2.IdDriver + ", " + dbdriverHistory2.Tanggal + ", " + dbdriverHistory2.Type + ", " + dbdriverHistory2.Nopol + ");";
                    RepoAuditrail.SetAuditTrail(query, "Penetapan Driver", "Add", UserPrincipal.id);
                }


                RepoPenetapanDriver.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Driver Batangan", Action = "update")]
        public ActionResult Edit(int id)
        {
            Context.PenetapanDriver dbitem = RepoPenetapanDriver.FindByPK(id);
            PenetapanDriver model = new PenetapanDriver(dbitem);
            ViewBag.name = model.VehicleNo;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(PenetapanDriver model)
        {
            if (ModelState.IsValid)
            {
                bool isPalid = true;
                Context.PenetapanDriver dbitem = RepoPenetapanDriver.FindByPK(model.Id);
                
                if (model.IdDriver1 == model.IdDriver2)
                {
                    isPalid = false;

                    ModelState.AddModelError("IdDriver1", "Driver tidak boleh sama.");
                    ModelState.AddModelError("IdDriver2", "Driver tidak boleh sama.");
                }

                if (!dbitem.IdDriver1.Value.Equals(model.IdDriver1.Value))
                {
                    if (RepoPenetapanDriver.isExistDriver(model.IdDriver1.Value, model.Id))
                    {
                        isPalid = false;
                        ModelState.AddModelError("idDriver1", "Driver sudah digunakan.");
                    }                    
                }

                if (model.IdDriver2.HasValue)
                {
                    if (!dbitem.IdDriver2.Value.Equals(model.IdDriver2.Value))
                    {
                        if (RepoPenetapanDriver.isExistDriver(model.IdDriver2.Value, model.Id))
                        {
                            isPalid = false;
                            ModelState.AddModelError("IdDriver2", "Driver sudah digunakan.");
                        }
                    }
                    if (model.IdDitetapkanOleh2 == null)
                    {
                        isPalid = false;
                        ModelState.AddModelError("IdDitetapkanOleh2", "Penetap Driver 2 Belum Ditentukan.");
                    }
                }


                if (model.IdDitetapkanOleh1 == null)
                {
                    isPalid = false;
                    ModelState.AddModelError("IdDitetapkanOleh1", "Penetap Driver 1 Belum Ditentukan.");
                }
                                

                if (!isPalid)
                {
                    return View("Form", model);
                }

                
                model.SetDb(dbitem, UserPrincipal.firstname + " " + UserPrincipal.lastname);
                Context.PenetapanDriverHistory dbitemHistory = new Context.PenetapanDriverHistory();
                
                model.SetDbHistory(dbitemHistory, UserPrincipal.firstname + " " + UserPrincipal.lastname);                
                dbitem.PenetapanDriverHistory.Add(dbitemHistory);

                //history driver
                Context.DataTruck dbtruck = RepoTruck.FindByPK(model.IdDataTruck.Value);
                Context.Driver dbdriver1 = RepoDriver.FindByPK(model.IdDriver1.Value);
                Context.DriverTruckHistory dbdriverHistory1 = new Context.DriverTruckHistory();
                dbdriver1.NoHp1 = model.NoHp1Driver1;
                dbdriver1.NoHp2 = model.NoHp2Driver1;
                dbdriverHistory1.Tanggal = DateTime.Now;
                dbdriverHistory1.Nopol = dbtruck.VehicleNo;
                dbdriverHistory1.Type = model.JenisTruck;
                dbdriver1.DriverTruckHistory.Add(dbdriverHistory1);
                RepoDriver.save(dbdriver1);
                string query = "UPDATE dbo.\"Driver\" SET \"NoHp1\" = " + dbdriver1.NoHp1 + ", \"NoHp2\" = " + dbdriver1.NoHp2 + " WHERE \"Id\" = " + dbdriver1.Id + ";INSERT INTO dbo.\"DriverTruckHistory\" ( " +
                    "\"IdDriver\", \"Tanggal\", \"Type\", \"Nopol\") VALUES (" + dbdriverHistory1.IdDriver + ", " + dbdriverHistory1.Tanggal + ", " + dbdriverHistory1.Type + ", " + dbdriverHistory1.Nopol + ");";
                RepoAuditrail.SetAuditTrail(query, "Penetapan Driver", "Add", UserPrincipal.id);

                if (model.IdDriver2.HasValue)
                {
                    Context.Driver dbdriver2 = RepoDriver.FindByPK(model.IdDriver2.Value);
                    Context.DriverTruckHistory dbdriverHistory2 = new Context.DriverTruckHistory();
                    dbdriver2.NoHp1 = model.NoHp1Driver2;
                    dbdriver2.NoHp2 = model.NoHp2Driver2;
                    dbdriverHistory2.Tanggal = DateTime.Now;
                    dbdriverHistory2.Nopol = dbtruck.VehicleNo;
                    dbdriverHistory2.Type = model.JenisTruck;
                    dbdriver2.DriverTruckHistory.Add(dbdriverHistory2);
                    RepoDriver.save(dbdriver2);
                    query = "UPDATE dbo.\"Driver\" SET \"NoHp1\" = " + dbdriver2.NoHp1 + ", \"NoHp2\" = " + dbdriver2.NoHp2 + " WHERE \"Id\" = " + dbdriver2.Id + ";INSERT INTO dbo.\"DriverTruckHistory\" ( " +
                        "\"IdDriver\", \"Tanggal\", \"Type\", \"Nopol\") VALUES (" + dbdriverHistory2.IdDriver + ", " + dbdriverHistory2.Tanggal + ", " + dbdriverHistory2.Type + ", " + dbdriverHistory2.Nopol + ");";
                    RepoAuditrail.SetAuditTrail(query, "Penetapan Driver", "Add", UserPrincipal.id);
                }

                RepoPenetapanDriver.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.PenetapanDriver dbItem = RepoPenetapanDriver.FindByPK(id);

            RepoPenetapanDriver.delete(dbItem);

            return Json(response);
        }
        #region options

        #endregion

        #region export import
        public string Upload(IEnumerable<HttpPostedFileBase> filesPenetapanDriver)
        {
            ResponeModel response = new ResponeModel();
            

            if (filesPenetapanDriver != null)
            {
                foreach (var file in filesPenetapanDriver)
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
                                
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && RepoTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()) != null)
                                {
                                    int id = 0;
                                    int resId;
                                    

                                    if (workSheet.Cells[rowIterator, 10].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 10].Value.ToString(), out resId))
                                        {
                                            id = resId;
                                        }
                                    }
                                    
                                    Context.PenetapanDriver db = new Context.PenetapanDriver();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            db = RepoPenetapanDriver.FindByPK(id);
                                        }
                                        Context.Driver dr1 = RepoDriver.FindByCode(workSheet.Cells[rowIterator, 2].Value.ToString());
                                        Context.Driver dr2 = workSheet.Cells[rowIterator, 6].Value == null ? null : RepoDriver.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString());
                                        db.IdDataTruck = RepoTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;

                                        if (id == 0 && RepoPenetapanDriver.IsExist(db.IdDataTruck.Value))
                                        {
                                            continue;
                                        }

                                        if (dr1 != null)
                                        {
                                            if (RepoPenetapanDriver.isExistDriver(dr1.Id, id))
                                            {
                                                continue;
                                            }
                                        }

                                        if (dr2 != null)
                                        {
                                            if (RepoPenetapanDriver.isExistDriver(dr2.Id, id))
                                            {
                                                continue;
                                            }
                                        }
                                        
                                        if(dr1 != null && dr2 != null)
                                        {
                                            if (dr2.Id.Equals(dr1.Id))
                                            {
                                                continue;
                                            }
                                        }

                                        db.IdDriver1 = dr1.Id;
                                        db.NoHp1Driver1 = workSheet.Cells[rowIterator, 3].Value == null ? dr1.NoHp1 : workSheet.Cells[rowIterator, 3].Value.ToString();
                                        db.NoHp2Driver1 = workSheet.Cells[rowIterator, 4].Value == null ? dr1.NoHp2 : workSheet.Cells[rowIterator, 4].Value.ToString();
                                        db.DitetapkanOleh1 = workSheet.Cells[rowIterator, 5].Value == null ? null : workSheet.Cells[rowIterator, 5].Value.ToString();

                                        if (dr2 != null){
                                            db.IdDriver2 = dr2.Id;
                                            db.NoHp1Driver2 = workSheet.Cells[rowIterator, 7].Value == null ? dr2.NoHp1 : workSheet.Cells[rowIterator, 7].Value.ToString();
                                            db.NoHp2Driver2 = workSheet.Cells[rowIterator, 8].Value == null ? dr2.NoHp2 : workSheet.Cells[rowIterator, 8].Value.ToString();
                                            db.DitetapkanOleh2 = workSheet.Cells[rowIterator, 9].Value == null ? null : workSheet.Cells[rowIterator, 9].Value.ToString();
                                            dr2.NoHp1 = db.NoHp1Driver2;
                                            dr2.NoHp2 = db.NoHp2Driver2;
                                        }                                        

                                        db.ModifiedBy = UserPrincipal.username;
                                        db.ModifiedDate = DateTime.Now;

                                        dr1.NoHp1 = db.NoHp1Driver1;
                                        dr1.NoHp2 = db.NoHp2Driver1;

                                        RepoPenetapanDriver.save(db);

                                        int resultId = db.Id;

                                        //save penetapan driver history
                                        Context.PenetapanDriverHistory dbItemHistory = new Context.PenetapanDriverHistory();
                                        Context.PenetapanDriver pd = RepoPenetapanDriver.FindByPK(resultId);

                                        dbItemHistory.IdPenetapanDriver = resultId;
                                        dbItemHistory.Driver1 = dr1.NamaDriver;
                                        if (dr2 != null)
                                            dbItemHistory.Driver2 = dr2.NamaDriver;
                                        dbItemHistory.CreatedBy = UserPrincipal.firstname + " " + UserPrincipal.lastname;
                                        dbItemHistory.ModifiedBy = UserPrincipal.firstname + " " + UserPrincipal.lastname;
                                        dbItemHistory.ModifiedDate = DateTime.Now;
                                        pd.PenetapanDriverHistory.Add(dbItemHistory);

                                        //history driver

                                        Context.DriverTruckHistory dbdriverHistory1 = new Context.DriverTruckHistory();
                                        Context.DriverTruckHistory dbdriverHistory2 = new Context.DriverTruckHistory();
                                        Context.DataTruck dbtruck = RepoTruck.FindByPK(db.IdDataTruck.Value);

                                        dbdriverHistory1.Tanggal = DateTime.Now;
                                        dbdriverHistory1.Nopol = dbtruck.VehicleNo;
                                        dbdriverHistory1.Type = dbtruck.JenisTrucks.StrJenisTruck;

                                        dbdriverHistory2.Tanggal = DateTime.Now;
                                        dbdriverHistory2.Nopol = dbtruck.VehicleNo;
                                        dbdriverHistory2.Type = dbtruck.JenisTrucks.StrJenisTruck;

                                        dr1.DriverTruckHistory.Add(dbdriverHistory1);

                                        RepoDriver.save(dr1);
                                        string query = "UPDATE dbo.\"Driver\" SET \"NoHp1\" = " + dr1.NoHp1 + ", \"NoHp2\" = " + dr1.NoHp2 + " WHERE \"Id\" = " + dr1.Id + ";INSERT INTO dbo.\"DriverTruckHistory\" ( " +
                                            "\"IdDriver\", \"Tanggal\", \"Type\", \"Nopol\") VALUES (" + dbdriverHistory1.IdDriver + ", " + dbdriverHistory1.Tanggal + ", " + dbdriverHistory1.Type + ", " + 
                                            dbdriverHistory1.Nopol + ");";
                                        RepoAuditrail.SetAuditTrail(query, "Penetapan Driver", "Import", UserPrincipal.id);
                                        if (dr2 != null){
                                            dr2.NoHp1 = db.NoHp1Driver2;
                                            dr2.NoHp2 = db.NoHp2Driver2;
                                            dr2.DriverTruckHistory.Add(dbdriverHistory2);
                                            RepoDriver.save(dr2);
                                            query = "UPDATE dbo.\"Driver\" SET \"NoHp1\" = " + dr2.NoHp1 + ", \"NoHp2\" = " + dr2.NoHp2 + " WHERE \"Id\" = " + dr2.Id + ";INSERT INTO dbo.\"DriverTruckHistory\" ( " +
                                                "\"IdDriver\", \"Tanggal\", \"Type\", \"Nopol\") VALUES (" + dbdriverHistory2.IdDriver + ", " + dbdriverHistory2.Tanggal + ", " + dbdriverHistory2.Type + ", " + 
                                                dbdriverHistory2.Nopol + ");";
                                            RepoAuditrail.SetAuditTrail(query, "Penetapan Driver", "Import", UserPrincipal.id);
                                        }
                                        RepoPenetapanDriver.save(pd);
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
            List<Context.PenetapanDriver> dbitems = RepoPenetapanDriver.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Vehicle No";
            ws.Cells[1, 2].Value = "ID Driver 1";
            ws.Cells[1, 3].Value = "No HP 1 Driver 1";
            ws.Cells[1, 4].Value = "No Hp Driver 2";
            ws.Cells[1, 5].Value = "Penetap Driver 1";
            ws.Cells[1, 6].Value = "ID Driver 2";
            ws.Cells[1, 7].Value = "No HP 1 Driver 2";
            ws.Cells[1, 8].Value = "No Hp Driver 2";            
            ws.Cells[1, 9].Value = "Penetap Driver 2";
            ws.Cells[1, 10].Value = "ID Database";

            // Inserts Data
            //int idx = 0;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].DataTruck.VehicleNo;
                ws.Cells[i + 2, 2].Value = dbitems[i].Driver1.KodeDriver;
                ws.Cells[i + 2, 3].Value = dbitems[i].Driver1.NoHp1;
                ws.Cells[i + 2, 4].Value = dbitems[i].Driver1.NoHp2;
                ws.Cells[i + 2, 5].Value = dbitems[i].User1 == null? "": dbitems[i].User1.Username;
                ws.Cells[i + 2, 6].Value = dbitems[i].Driver2 == null ? null : dbitems[i].Driver2.KodeDriver;
                ws.Cells[i + 2, 7].Value = dbitems[i].Driver2 == null ? null : dbitems[i].Driver2.NoHp1;
                ws.Cells[i + 2, 8].Value = dbitems[i].Driver2 == null ? null : dbitems[i].Driver2.NoHp2;
                ws.Cells[i + 2, 9].Value = dbitems[i].User2 == null? "": dbitems[i].User2.Username;
                ws.Cells[i + 2, 10].Value = dbitems[i].Id;
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Export Data Penetapan Driver.xls";

            return fsr;
        }
    }
    #endregion
}