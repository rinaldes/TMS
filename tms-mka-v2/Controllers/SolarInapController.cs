using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class SolarInapController : BaseController 
    {
        private ISolarInapRepo RepoSolarInap;
        private IDriverRepo RepoDriver;
        private ISalesOrderRepo RepoSalesOrder;
        private IAtmRepo RepoAtm;
        private IDataBoronganRepo RepoBor;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private Ipbyd_detRepo Repopbyd_det;
        private IUserRepo RepoUser;
        private Iso_mstrRepo Reposo_mstr;
        private IAuditrailRepo RepoAuditrail;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;

        public SolarInapController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISolarInapRepo repoSolarInap, IDriverRepo repoDriver, ISalesOrderRepo repoSalesOrder, IAtmRepo repoAtm,
            IDataBoronganRepo repoBor, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr, Ipbyd_detRepo repopbyd_det, IUserRepo repoUser,
            Iso_mstrRepo reposo_mstr, IAuditrailRepo repoAuditrail, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo)
            : base(repoBase, repoLookup)
        {
            RepoSolarInap = repoSolarInap;
            RepoDriver = repoDriver;
            RepoSalesOrder = repoSalesOrder;
            RepoAtm = repoAtm;
            RepoBor = repoBor;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            Repopbyd_det = repopbyd_det;
            RepoUser = repoUser;
            Reposo_mstr = reposo_mstr;
            RepoAuditrail = repoAuditrail;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "SolarInap").ToList();
            return View();
        }

        public ActionResult IndexMarketing()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SolarInap").ToList();
            return View();
        }

        public ActionResult IndexAUJ()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SolarInap").ToList();
            return View();
        }
        
        [MyAuthorize(Menu = "Batal Inap", Action="create")]
        public ActionResult batalInap(int id)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            model.TanggalBatal = DateTime.Now;
            ViewBag.name = model.Id;
            return View("batalInap", model);
        }

        [HttpPost]
        [MyAuthorize(Menu = "Batal Inap", Action="create")]
        public ActionResult batalInap(SolarInap model)
        {
            if (ModelState.IsValid)
            {
                Context.SolarInap dbitem = RepoSolarInap.FindByPK(model.Id);
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSO.Value);
                string so_code = dbso.SONumber;
                string code = "SIBI-" + dbitem.TanggalDari.ToString() + so_code + "-" + dbitem.UrutanBatal;
                model.setDbSolarInap(dbitem);
                if (dbitem.IdDriver == null)
                    dbitem.IdDriver = dbso.DriverId;
                RepoSolarInap.save(dbitem, UserPrincipal.id);
                Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                Repoglt_det.saveFromAc(1, code, model.Transfer, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso); //D, Kasbon Driver
                Repoglt_det.saveFromAc(2, code, 0, model.Transfer, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbso); //K, Biaya Inap
                if (dbitem.AktualTransfer > 0)
                {
                    var glt_oid = Guid.NewGuid().ToString();
                    Repopbyd_det.saveMstr(glt_oid, code, erpConfig.IdAUJCredit.Value, "Batal Inap " + code, dbitem.IdDriver.Value+7000000);
                    Repopbyd_det.save(glt_oid, code, erpConfig.IdAUJCredit.Value, "Batal Inap " + code, dbitem.IdDriver.Value + 7000000, erpConfig.IdAUJCredit.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, dbitem.AktualTransfer);
                }
                if (dbitem.StatusTagihan == "Ditagih")
                {
                    Repoglt_det.saveFromAc(1, code, dbitem.Nominal, 0, Repoac_mstr.FindByPk(erpConfig.IdBiayaInap), dbso); //D, Piutang Customer
                    Repoglt_det.saveFromAc(2, code, 0, dbitem.Nominal, Repoac_mstr.FindByPk(erpConfig.IdPiutangCustomer), dbso); //K, Biaya Inap
                }
                return Redirect("/Report/SolarInap");
            }
            return View("batalInap", model);
        }

        public ActionResult pengembalianUang(int id)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            ViewBag.name = model.Id;
            return View("pengembalianUang", model);
        }
        [HttpPost]
        public ActionResult pengembalianUang(SolarInap model)
        {
            if (ModelState.IsValid)
            {
                Context.SolarInap dbitem = RepoSolarInap.FindByPK(model.Id);
                model.setPengembalianUangSolarInap(dbitem);
                RepoSolarInap.save(dbitem, UserPrincipal.id);
                return RedirectToAction("Index");
            }
            return View("pengembalianUang", model);
        }
        public ActionResult kasirCash(int id)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            ViewBag.name = model.Id;
            return View("kasirCash", model);
        }
        [HttpPost]
        public ActionResult kasirCash(SolarInap model)
        {
            if (ModelState.IsValid)
            {
                Context.SolarInap dbitem = RepoSolarInap.FindByPK(model.Id);
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSO.Value);
                string code = "SIKK-" + dbitem.TanggalDari.ToString() + dbso.SONumber + "-" + dbitem.UrutanBatal;
                model.setDbKasirCash(dbitem);
                Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                Repoglt_det.saveFromAc(1, code, model.Transfer, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso); //D, Biaya Inap
                Repoglt_det.saveFromAc(2, code, 0, model.Transfer, Repoac_mstr.FindByPk(dbitem.IdCreditTf), dbso); //K, Transfer / Bank
                RepoSolarInap.save(dbitem, UserPrincipal.id, 5);
                return RedirectToAction("Index");
            }
            return View("kasirCash", model);
        }
        public ActionResult kasirTransfer(int id)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            ViewBag.name = model.Id;
            return View("kasirTransfer", model);
        }
        [HttpPost]
        public ActionResult kasirTransfer(SolarInap model)
        {
            bool palid = true;
            if (model.IdCreditTf == null){
                ModelState.AddModelError("IdCreditTf", "Bank Account Harus Diisi");
                palid = false;
                ViewBag.BankAccountHarusDiisi = 1;
            }
            if (ModelState.IsValid && palid)
            {
                Context.SolarInap dbitem = RepoSolarInap.FindByPK(model.Id);
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSO.Value);
                if (dbitem.Cash <= 0)
                    dbitem.Cash = 0;
                model.setDbKasirTransfer(dbitem);
                dbitem.IdCreditTf = model.IdCreditTf;
                //#Marketing, Status Ditagih, Piutang Customer pada Biaya Inapdbitem
                string code = "SIKT-" + dbitem.TanggalDari.ToString() + dbso.SONumber + "-" + dbitem.UrutanBatal;
                Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                Repoglt_det.saveFromAc(1, code, model.Transfer, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso); //D, Biaya Inap
                Repoglt_det.saveFromAc(2, code, 0, model.Transfer, Repoac_mstr.FindByPk(dbitem.IdCreditTf), dbso); //K, Transfer / Bank
                RepoSolarInap.save(dbitem, UserPrincipal.id, 4);
                return RedirectToAction("Index");
            }
            return View("kasirTransfer", model);
        }

        public string BindingMarketing()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<SolarInap> ListModel = new List<SolarInap>();
            List<Context.SolarInap> items = RepoSolarInap.FindAllMarketing();

            foreach (Context.SolarInap item in items)
            {
                if (item.SalesOrderKontrakListSOId.HasValue)
                {
                    var soKontrak = item.SO.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == item.SalesOrderKontrakListSOId).FirstOrDefault();
                    SolarInap si = new SolarInap(item, soKontrak);
                    if (si.Status != "Sudah")
                        ListModel.Add(new SolarInap(item, soKontrak));
                }
                else
                {
                    SolarInap si = new SolarInap(item);
                    if (si.Status != "Sudah")
                        ListModel.Add(new SolarInap(item));
                }
            }
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }

        public string BindingAUJ()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<SolarInap> ListModel = new List<SolarInap>();
            List<Context.SolarInap> items = RepoSolarInap.FindAllAUJ();

            foreach (Context.SolarInap item in items)
            {
                if (item.SalesOrderKontrakListSOId.HasValue)
                {
                    var soKontrak = item.SO.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == item.SalesOrderKontrakListSOId).FirstOrDefault();
                    SolarInap si = new SolarInap(item, soKontrak);
                    if (si.Status != "Sudah")
                        ListModel.Add(new SolarInap(item, soKontrak));
                }
                else
                {
                    SolarInap si = new SolarInap(item);
                    if (si.Status != "Sudah")
                        ListModel.Add(new SolarInap(item));
                }
            }
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }

        public string Binding(string user_types)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            Context.User user = RepoUser.FindByPK(UserPrincipal.id);
            List<SolarInap> ListModel = new List<SolarInap>();

            List<Context.SolarInap> items = RepoSolarInap.FindAll(user_types);

            foreach (Context.SolarInap item in items)
            {
                if (item.SalesOrderKontrakListSOId.HasValue)
                {
                    var soKontrak = item.SO.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == item.SalesOrderKontrakListSOId).FirstOrDefault();
                    SolarInap si = new SolarInap(item, soKontrak);
                    if (si.Status != "Sudah")
                        ListModel.Add(new SolarInap(item, soKontrak));
                }
                else
                {
                    SolarInap si = new SolarInap(item);
                    if (si.Status != "Sudah")
                        ListModel.Add(new SolarInap(item));
                }
            }
            int total = RepoSolarInap.CountTrans(user_types, param.Filters);
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }
        public string GenerateStatus(string user_types)
        {
            Context.User user = RepoUser.FindByPK(UserPrincipal.id);
            List<Context.SolarInap> items = RepoSolarInap.FindAllPisan();

            foreach (Context.SolarInap item in items)
            {
                RepoSolarInap.save(item, UserPrincipal.id);
            }
            return "aa";
        }

        public ActionResult Add()
        {
            SolarInap model = new SolarInap();
            model.StepKe = 1;
            ViewBag.IdSO = 0;
            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(SolarInap model)
        {
            bool palid = true;
            if (ModelState.IsValid)
            {
                Context.SolarInap dbitem = new Context.SolarInap();
                if (RepoSolarInap.FindBySOAndDate(model.IdSO, DateTime.Parse(model.TanggalDari)) != null)
                {
                    ModelState.AddModelError("IdSO", "Order terpilih sudah diset di tanggal "+model.TanggalDari);
                    palid = false;
                }
                if (palid){
                    model.setDbNew(dbitem);
                    Context.SalesOrder so = RepoSalesOrder.FindByPK(dbitem.IdSO.Value);
                    dbitem.IdDriver = so.DriverId;
                    RepoSolarInap.save(dbitem, UserPrincipal.id);
                    Context.SolarInap si = RepoSolarInap.FindBySOAndDate(so.Id, dbitem.TanggalDari);
                    RepoAuditrail.saveInapHistory(si, UserPrincipal.id);
                }
                else{
                    ViewBag.IdSO = model.IdSO;
                    return View("Form", model);
                }
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }

        public ActionResult Edit(int id, string NoSO=null, string Mode=null)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            model.StepKe += 1;
            model.SalesOrderKontrakListSOId = dbitem.SalesOrderKontrakListSOId;
            ViewBag.name = model.Id;
            ViewBag.IdSO = 0;
            ViewBag.NoSO = NoSO;
            ViewBag.Mode = Mode;
            return View("Form", model);
        }

        public ActionResult EditAUJ(int id, string NoSO = null, string Mode = null)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            model.StepKe += 1;
            model.SalesOrderKontrakListSOId = dbitem.SalesOrderKontrakListSOId;
            ViewBag.name = model.Id;
            ViewBag.IdSO = 0;
            ViewBag.NoSO = NoSO;
            ViewBag.Mode = Mode;
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Edit(SolarInap model, string NoSO=null)
        {
            if (model.Cash <= 0)
                model.Cash = 0;
            if (ModelState.IsValid)
            {
                bool palid = true;
                Context.SolarInap dbitem = RepoSolarInap.FindByPK(model.Id);
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSO.Value);
                Context.SalesOrderKontrakListSo sokls = null;
                if (model.SalesOrderKontrakListSOId != null)
                    sokls = RepoSalesOrderKontrakListSo.FindByPK(model.SalesOrderKontrakListSOId.Value);
                if (dbitem.StepKe == 2 && model.Transfer <= 0 && model.Cash <= 0)
                {
                    ModelState.AddModelError("Transfer", "Transfer dan atau cash harus lebih besar dari 0");
                    palid = false;
                }
                if (palid){
                    if (dbitem.StepKe == 1 && model.StatusTagihan == "Ditagih"){
                        //#Jurnal Piutang Customer pada Biaya
                        string so_code = dbso.SONumber;
                        string code = "SIM-" + dbitem.TanggalDari.ToString() + so_code + "-" + dbitem.UrutanBatal;
                        Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                        Repoglt_det.saveFromAc(1, code, model.Nominal, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangCustomer), dbso, null, sokls); //D, Piutang Customer
                        Repoglt_det.saveFromAc(2, code, 0, model.Nominal, Repoac_mstr.FindByPk(erpConfig.IdBiayaInap), dbso, null, sokls); //K, Biaya Inap

                        string guid = Guid.NewGuid().ToString();
                        dbitem.so_oid = guid;
                        string ship_guid = Guid.NewGuid().ToString();
                        int CustomerId = dbso.CustomerId == null ? dbso.SalesOrderKontrak.CustomerId.Value : dbso.CustomerId.Value;
                        decimal nominal = model.Nominal;
                        string so_mstr_code = Reposo_mstr.saveSoMstrSolarInap(dbso, UserPrincipal.username, guid, CustomerId, nominal, sokls, dbitem);
                        string sod_guid = Reposo_mstr.FindSoDet(so_mstr_code).so_oid;
                        Reposo_mstr.saveSoDetSolarInap(dbso, UserPrincipal.username, guid, sod_guid, model.Nominal);
                        Reposo_mstr.saveSoShipMstrSolarInap(dbso, UserPrincipal.username, guid, ship_guid, sokls, dbitem);
                        Reposo_mstr.saveSoShipDet(dbso, UserPrincipal.username, ship_guid, sod_guid);
                    }
                    else if (dbitem.StepKe == 2){
                        //#Jurnal Piutang Customer pada Biaya
                        string so_code = dbso.SONumber + "-" + dbitem.UrutanBatal;
                        string code = "SIAUJ-" + dbitem.TanggalDari.ToString() + so_code;
                        Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                        Repoglt_det.saveFromAc(1, code, dbitem.NilaiYgDiajukan, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbso); //D, Piutang Customer
                        Repoglt_det.saveFromAc(2, code, 0, dbitem.NilaiYgDiajukan, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso); //K, Biaya Inap
                    }
                    model.setDb(dbitem);
                    RepoSolarInap.save(dbitem, UserPrincipal.id);
                }
                else{
                    return View("Form", model);
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdSO = 0;
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult EditAUJ(SolarInap model, string NoSO = null)
        {
            if (model.Cash <= 0)
                model.Cash = 0;
            if (ModelState.IsValid)
            {
                bool palid = true;
                Context.SolarInap dbitem = RepoSolarInap.FindByPK(model.Id);
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSO.Value);
                Context.SalesOrderKontrakListSo sokls = null;
                if (model.SalesOrderKontrakListSOId != null)
                    sokls = RepoSalesOrderKontrakListSo.FindByPK(model.SalesOrderKontrakListSOId.Value);
                if (dbitem.StepKe == 2 && model.Transfer <= 0 && model.Cash <= 0)
                {
                    ModelState.AddModelError("Transfer", "Transfer dan atau cash harus lebih besar dari 0");
                    palid = false;
                }
                if (palid)
                {
                    string so_code = dbso.SONumber + "-" + dbitem.UrutanBatal;
                    string code = "SIAUJ-" + dbitem.TanggalDari.ToString() + so_code;
                    Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                    Repoglt_det.saveFromAc(1, code, dbitem.NilaiYgDiajukan, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbso); //D, Piutang Customer
                    Repoglt_det.saveFromAc(2, code, 0, dbitem.NilaiYgDiajukan, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso); //K, Biaya Inap
                    model.setDb(dbitem);
                    RepoSolarInap.save(dbitem, UserPrincipal.id);
                }
                else
                    return View("EditAUJ", model);
                return RedirectToAction("IndexAUJ");
            }
            ViewBag.IdSO = 0;
            return View("Form", model);
        }
        public ActionResult marketing(int id, string NoSO = null, string Mode = null)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            model.StepKe += 1;
            model.SalesOrderKontrakListSOId = dbitem.SalesOrderKontrakListSOId;
            ViewBag.name = model.Id;
            ViewBag.IdSO = 0;
            ViewBag.NoSO = NoSO;
            return View("Marketing", model);
        }
        [HttpPost]
        public ActionResult Marketing(SolarInap model, string NoSO = null)
        {
            if (model.Cash <= 0)
                model.Cash = 0;
            if (ModelState.IsValid)
            {
                bool palid = true;
                Context.SolarInap dbitem = RepoSolarInap.FindByPK(model.Id);
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSO.Value);
                Context.SalesOrderKontrakListSo sokls = null;
                if (model.SalesOrderKontrakListSOId != null)
                    sokls = RepoSalesOrderKontrakListSo.FindByPK(model.SalesOrderKontrakListSOId.Value);
                if (palid)
                {
                    if (model.StatusTagihan == "Ditagih")
                    {
                        //#Jurnal Piutang Customer pada Biaya
                        string so_code = dbso.SONumber;
                        string code = "SIM-" + dbitem.TanggalDari.ToString() + so_code + "-" + dbitem.UrutanBatal;
                        Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                        Repoglt_det.saveFromAc(1, code, model.Nominal, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangCustomer), dbso, null, sokls); //D, Piutang Customer
                        Repoglt_det.saveFromAc(2, code, 0, model.Nominal, Repoac_mstr.FindByPk(erpConfig.IdBiayaInap), dbso, null, sokls); //K, Biaya Inap

                        string guid = Guid.NewGuid().ToString();
                        dbitem.so_oid = guid;
                        string ship_guid = Guid.NewGuid().ToString();
                        int CustomerId = dbso.CustomerId == null ? dbso.SalesOrderKontrak.CustomerId.Value : dbso.CustomerId.Value;
                        decimal nominal = model.Nominal;
                        string so_mstr_code = Reposo_mstr.saveSoMstrSolarInap(dbso, UserPrincipal.username, guid, CustomerId, nominal, sokls, dbitem);
                        string sod_guid = Reposo_mstr.FindSoDet(so_mstr_code).so_oid;
                        Reposo_mstr.saveSoDetSolarInap(dbso, UserPrincipal.username, guid, sod_guid, model.Nominal);
                        Reposo_mstr.saveSoShipMstrSolarInap(dbso, UserPrincipal.username, guid, ship_guid, sokls, dbitem);
                        Reposo_mstr.saveSoShipDet(dbso, UserPrincipal.username, ship_guid, sod_guid);
                    }
                    model.setDbMarketing(dbitem);
                    RepoSolarInap.save(dbitem, UserPrincipal.id);
                }
                else
                    return View("Marketing", model);
                return RedirectToAction("IndexMarketing");
            }
            ViewBag.IdSO = 0;
            return View("Marketing", model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SolarInap dbItem = RepoSolarInap.FindByPK(id);

            RepoSolarInap.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        public string BindingSo()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllAdminDispatched(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.SalesOrder item in items)
            {
                if (item.SalesOrderKontrakId.HasValue)
                {
                    var data = item.SalesOrderKontrak.SalesOrderKontrakListSo.Where(p => p.IsProses && p.Status == "dispatched").GroupBy(d => new { d.IdDataTruck, d.Driver1Id, d.Status, d.Urutan }).Select(grp => grp.ToList());
                    foreach (var itemGroup in data.ToList())
                    {
                        ListModel.Add(new AdminUangJalanIndex(item, itemGroup));
                        foreach (var itemKontrakPerOrder in itemGroup.OrderBy(t => t.MuatDate).ToList()){
                            ListModel.Add(new AdminUangJalanIndex(item, itemGroup.Where(d => d.Id == itemKontrakPerOrder.Id).ToList()));
                        }
                    }
                }
                else
                {
                    ListModel.Add(new AdminUangJalanIndex(item));
                }

            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllAdminDispatched(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters), data = ListModel });
        }

        public string BindingSoKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrderKontrakListSo> items = RepoSalesOrder.FindAllKlaimKontrak(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.SalesOrderKontrakListSo item in items)
            {
                ListModel.Add(new AdminUangJalanIndex(item));
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountSoKlaimKontrak(param.Filters), data = ListModel });
        }

        public string GetDetailSo(int idSo, string ListIdSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);
            AdminUangJalan model = new AdminUangJalan();

            if (dbitem.SalesOrderKontrakId.HasValue)
            {
                List<int> ListIdDumy = ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;

                if (dbsoDummy.FirstOrDefault().IdAdminUangJalan.HasValue)
                    model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
            }
            else
            {
                try
                {
                    model = new AdminUangJalan(dbitem.AdminUangJalan);
                }
                catch { }
            }

            return new JavaScriptSerializer().Serialize(model);
        }
        [HttpGet]
        public PartialViewResult GetPartialSo(int idSo, string NoSO, string ListIdSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                AdminUangJalan model = new AdminUangJalan(dbitem.AdminUangJalan);
                model.ModelOncall = new SalesOrderOncall(dbitem);
                return PartialView("SalesOrderOncall/_PartialFormReadOnly", model.ModelOncall);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                AdminUangJalan model = new AdminUangJalan(dbitem.AdminUangJalan);
                model.ModelPickup = new SalesOrderPickup(dbitem);
                return PartialView("SalesOrderPickup/_PartialFormReadOnly", model.ModelPickup);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                AdminUangJalan model = new AdminUangJalan(dbitem.AdminUangJalan);
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                return PartialView("SalesOrderProsesKonsolidasi/_PartialFormReadOnly", model.ModelKonsolidasi);
            }
            else if (dbitem.SalesOrderKontrakId.HasValue)
            {
                List<int> ListIdDumy = null;
                if (ListIdSo != null)
                    ListIdDumy = ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = null;
                if (NoSO != null)
                    dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => NoSO.Contains(d.NoSo)).ToList();
                else
                    dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
                AdminUangJalan model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
                return PartialView("SalesOrderKontrak/_PartialFormReadOnly", model);
            }
            return PartialView("");
        }
    }
}