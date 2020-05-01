using Npgsql;
using System.Data;
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
    public class SettlementRegController : BaseController
    {
        private ISettlementRegRepo RepoSettlementReg;
        private ISalesOrderRepo RepoSalesOrder;
        private IAtmRepo RepoAtm;
        private IDataBoronganRepo RepoBor;
        private ISalesOrderKontrakListSoRepo RepoListSo;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private IAdminUangJalanRepo RepoAdminUangJalan;
        private IAuditrailRepo RepoAuditrail;
        private Igr_mstrRepo Repogr_mstr;
        private ICustomerRepo RepoCustomer;
        private Iso_mstrRepo Reposo_mstr;
        private IMasterPoolRepo RepoMasterPool;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        public SettlementRegController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISettlementRegRepo repoSettlementReg, ISalesOrderRepo repoSalesOrder, IAtmRepo repoAtm,
            IDataBoronganRepo repoBor, ISalesOrderKontrakListSoRepo repoListSo, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, IMasterPoolRepo repoMasterPool,
            Ibk_mstrRepo repobk_mstr, IAdminUangJalanRepo repoAdminUangJalan, IAuditrailRepo repoAuditrail, Iso_mstrRepo reposo_mstr, ICustomerRepo repoCustomer, Igr_mstrRepo repogr_mstr, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo)
            : base(repoBase, repoLookup)
        {
            RepoSettlementReg = repoSettlementReg;
            RepoSalesOrder = repoSalesOrder;Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            RepoAtm = repoAtm;
            RepoBor = repoBor;
            RepoListSo = repoListSo;
            RepoMasterPool = repoMasterPool;
            Repobk_mstr = repobk_mstr;
            RepoAdminUangJalan = repoAdminUangJalan;
            RepoAuditrail = repoAuditrail;
            Reposo_mstr = reposo_mstr;
            RepoCustomer = repoCustomer;
            Repogr_mstr = repogr_mstr;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
        }
        [MyAuthorize(Menu = "Settlement Reguler", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SettlementReg").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SettlementReguler> items = RepoSettlementReg.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<SettlementRegIndex> ListModel = new List<SettlementRegIndex>();
            foreach (Context.SettlementReguler item in items)
            {
                if (item.SalesOrder.SalesOrderKontrakId.HasValue) 
                {
                    List<int> ListIdDumy = item.LisSoKontrak.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                    List<Context.SalesOrderKontrakListSo> dbsoDummy = item.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                    ListModel.Add(new SettlementRegIndex(item, dbsoDummy));
                }
                else
                {
                    ListModel.Add(new SettlementRegIndex(item));
                }
            }

            int total = RepoSettlementReg.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
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
                        }
                    }
                    else
                    {
                        ListModel.Add(new AdminUangJalanIndex(item));
                    }
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllAdminDispatched(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters), data = ListModel });
        }
        public string BindingAUJ()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllSettlRegSO(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.AdminUangJalan auj in items)
            {
                if (auj.DaftarHargaKontrakId == null)
                {
                    Context.SalesOrder item = RepoSalesOrder.FindByAUJ(auj.Id);
                    if (item != null)
                        ListModel.Add(new AdminUangJalanIndex(item));
                }
                else
                {
                    List<Context.SalesOrderKontrakListSo> itemGroup = RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id);
                    AdminUangJalanIndex aujj = new AdminUangJalanIndex(auj, itemGroup, "hiburan");
                    ListModel.Add(aujj);
                }
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllSettlRegSO(param.Filters), data = ListModel });
        }
        [MyAuthorize(Menu = "Settlement Reguler", Action = "create")]
        public ActionResult Add()
        {
            SettlementReg model = new SettlementReg();
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Add(SettlementReg model)
        {
            SettlementRegTambahanBiaya[] res = JsonConvert.DeserializeObject<SettlementRegTambahanBiaya[]>(model.StrBiayaTambahan);
            model.ListBiayaTambahan = res.ToList();
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            string sod_oid = Guid.NewGuid().ToString();
            if (RepoAuditrail.getKasirHistoryUnder1Minutes(dbso, DateTime.Now.AddMinutes(-1)) != null) //keklik (kesubmit) 2 kali sama usernya
                return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
                int? urutan = RepoSettlementReg.FindAll().Where(d => d.IdSalesOrder == dbso.Id).Count() + 1;
                Context.SettlementReguler dbitem = new Context.SettlementReguler();
                string code = "SRT-" + dbso.SONumber + "-" + urutan;
                string codeSRJ = "SRJ-" + dbso.SONumber + "-" + urutan;
                Context.AdminUangJalan db = null;
                if (dbso != null && dbso.AdminUangJalanId != null)
                    db = RepoAdminUangJalan.FindByPK(dbso.AdminUangJalanId.Value);
                int idx = 1;
                Context.SalesOrderKontrakListSo sokls = null;
                if (model.listIdSoKontrak != "" && model.listIdSoKontrak != null)
                {
                    dbso = RepoSalesOrder.FindByKontrak(model.IdSalesOrder.Value);
                    List<int> ListIdDumy = model.listIdSoKontrak.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                    List<Context.SalesOrderKontrakListSo> dbsoDummy = RepoSalesOrderKontrakListSo.FindAll().Where(d => ListIdDumy.Contains(d.Id)).ToList();
                    sokls = dbsoDummy.FirstOrDefault();
                    db = RepoAdminUangJalan.FindByPK(dbsoDummy.FirstOrDefault().IdAdminUangJalan.Value);
                    code = "SRT-" + db.SONumber + "-" + urutan;
                    codeSRJ = "SRJ-" + db.SONumber + "-" + urutan;
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dbso);
                    RepoAuditrail.saveSettRegHistory(dbso, string.Join(",", dbsoDummy.Select(d => d.Id)));
                }
                else
                {
                    dbso.UpdatedBy = UserPrincipal.id;
                    if (dbso.oidErp == null)
                        dbso.oidErp = sod_oid;
                    RepoSalesOrder.save(dbso);
                    RepoAuditrail.saveSettRegHistory(dbso);
                }
                model.SetDb(dbitem);
                dbitem.IdSalesOrder = dbso.Id;
                dbitem.IdAdminUangJalan = db.Id;
                dbitem.ModifiedDate = DateTime.Now;
                dbitem.ModifiedBy = UserPrincipal.id.ToString();
                RepoSettlementReg.save(dbitem, UserPrincipal.id);
                Context.LookupCode biaya = null;
                foreach (Context.SettlementRegulerTambahanBiaya srtb in dbitem.SettlementRegulerTambahanBiaya)
                {
                    RepoAuditrail.saveSettlementRegulerBiayaTambahanQuery(srtb, UserPrincipal.id);
                    biaya = RepoLookup.FindByName(srtb.NamaBiaya);
                    if (biaya != null)
                    {
                        Repoglt_det.saveFromAc(idx, code, srtb.Value, 0, Repoac_mstr.FindByPk(biaya.ac_id), dbso, srtb.Keterangan, sokls);
                        Repoglt_det.saveFromAc(idx + 1, code, 0, srtb.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso, null, sokls);
                        idx += 2;
                    }
                }

                string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                {
                    idx++;
                    Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                    if (spbu != null && model.SPBUKembali != null && model.SPBUKembali.Contains(aujvs.Keterangan))
                    {//jurnal balik, ga jadi hutangnya
                        Repoglt_det.saveFromAc(idx, codeSRJ, aujvs.Value, 0, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbso, aujvs.Keterangan, sokls);
                        Repoglt_det.saveFromAc(idx, codeSRJ, 0, aujvs.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso, null, sokls);
                        Context.Customer customer = RepoCustomer.FindByPK(spbu.VendorId.Value);
                        NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                        con.Open();
                        using (DataTable dt = new DataTable())
                        {
                            var query = RepoSalesOrder.saveQueryGrMstrKontrak(UserPrincipal.username, customer, db.DataTruck.VehicleNo, decimal.Parse(aujvs.Value.ToString()) * -1);
                            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dt);
                            cmd.Dispose();
                            con.Close();
                        }
                    }
                }
                if (dbitem.KapalAktual > 0)
                {
                    foreach (Context.AdminUangJalanVoucherKapal aujvs in db.AdminUangJalanVoucherKapal)
                    {
                        Context.LookupCode kapal = RepoLookup.FindByName(aujvs.Keterangan);
                        idx++;
                        Context.Customer customer = RepoCustomer.FindByPK(kapal.VendorId.Value);
                        Repoglt_det.saveFromAc(idx, "SRJ-" + dbso.SONumber, aujvs.Value, 0, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbso, aujvs.Keterangan, sokls);
                        Repoglt_det.saveFromAc(idx, "SRJ-" + dbso.SONumber, 0, aujvs.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso, null, sokls);
                        NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                        con.Open();
                        using (DataTable dt = new DataTable())
                        {
                            var query = RepoSalesOrder.saveQueryGrMstrKontrak(UserPrincipal.username, customer, db.DataTruck.VehicleNo, decimal.Parse(aujvs.Value.ToString()) * -1);
                            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dt);
                            cmd.Dispose();
                            con.Close();
                        }
                    }
                }
                return RedirectToAction("Index");
            }

            return View("Form", model);
        }

        [MyAuthorize(Menu = "Settlement Reguler", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.SettlementReguler dbitem = RepoSettlementReg.FindByPK(id);
            SettlementReg model = new SettlementReg(dbitem, RepoAtm.FindAll());
            if (model.ModelOncall != null)
                ViewBag.name = model.ModelOncall.SONumber;
            if (model.ModelPickup != null)
                ViewBag.name = model.ModelPickup.SONumber;
            if (model.ModelKonsolidasi != null)
                ViewBag.name = model.ModelKonsolidasi.SONumber;
            if (model.ModelKontrak != null)
                ViewBag.name = model.ModelKontrak.SONumber;

            return View("Detail", model);
        }
        public ActionResult ShowPrint(int id, string listSo, int idSo)
        {
            Context.SettlementReguler dbitem2 = RepoSettlementReg.FindByPK(id);
            SettlementReg model = new SettlementReg(dbitem2, RepoAtm.FindAll());
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);
            if (dbitem == null)
                dbitem = RepoSalesOrder.FindByKontrak(idSo);
            var items = model.ListBiayaTambahan;
            ViewBag.item = items;

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                model.ModelOncall = new SalesOrderOncall(dbitem);
                ViewBag.NoSO = model.ModelOncall.SONumber;
                ViewBag.Customer = model.ModelOncall.NamaCustomer;
                ViewBag.VNo = model.ModelOncall.VehicleNo;
                ViewBag.Driver = model.ModelOncall.NamaDriver1;
                return View("Print", model);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                model.ModelPickup = new SalesOrderPickup(dbitem);
                ViewBag.NoSO = model.ModelPickup.SONumber;
                ViewBag.Customer = model.ModelPickup.NamaCustomer;
                ViewBag.VNo = model.ModelPickup.VehicleNo;
                ViewBag.Driver = model.ModelPickup.NamaDriver1;
                return View("Print", model);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                ViewBag.NoSO = model.ModelKonsolidasi.SONumber;
                ViewBag.Customer = "";
                ViewBag.VNo = model.ModelKonsolidasi.VehicleNo;
                ViewBag.Driver = model.ModelKonsolidasi.NamaDriver1;
                return View("Print", model);
            }
            else if (dbitem.SalesOrderKontrakId.HasValue)
                return View("Print", model);

            return View("Print", model);
        }
        [HttpPost]
        public ActionResult Edit(SettlementReg model)
        {
            SettlementRegTambahanBiaya[] res = JsonConvert.DeserializeObject<SettlementRegTambahanBiaya[]>(model.StrBiayaTambahan);
            model.ListBiayaTambahan = res.ToList();

            if (ModelState.IsValid)
            {
                Context.SettlementReguler dbitem = RepoSettlementReg.FindByPK(model.Id);
                model.SetDb(dbitem);
                RepoSettlementReg.save(dbitem, UserPrincipal.id);
                RepoAuditrail.saveDelAllSettlementRegulerBiayaTambahanQuery(dbitem, UserPrincipal.id);
                foreach (Context.SettlementRegulerTambahanBiaya srtb in dbitem.SettlementRegulerTambahanBiaya) {
                    RepoAuditrail.saveSettlementRegulerBiayaTambahanQuery(srtb, UserPrincipal.id);
                }

                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        [HttpGet]
        public PartialViewResult GetPartialSo(int? idSo, string ListIdSo, int? aujId)
        {
            Context.SalesOrder dbitem = null;
            if (idSo!=null)
            dbitem = RepoSalesOrder.FindByPK(idSo.Value);
            if (idSo != null && (ListIdSo == null || ListIdSo == "" || ListIdSo == "null"))
            {
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
            }
            else if (aujId != null)
            {
                Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(aujId.Value);
                List<Context.SalesOrderKontrakListSo> dbsoDummy = RepoSalesOrderKontrakListSo.FindAll().Where(d => auj.SONumber.Contains(d.NoSo)).ToList();
                dbitem = RepoSalesOrder.FindByKontrak(dbsoDummy.FirstOrDefault().SalesKontrakId.Value);
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
                AdminUangJalan model = new AdminUangJalan(auj);
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
                ViewBag.NoSo = auj.SONumber;
                return PartialView("SalesOrderKontrak/_PartialFormReadOnly", model);
            }
            else
            {
                List<int> ListIdDumy = ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = RepoSalesOrderKontrakListSo.FindAll().Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem = RepoSalesOrder.FindByKontrak(dbsoDummy.FirstOrDefault().SalesKontrakId.Value);
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
                AdminUangJalan model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
                ViewBag.NoSo = dbsoDummy.FirstOrDefault().AdminUangJalan.SONumber;
                return PartialView("SalesOrderKontrak/_PartialFormReadOnly", model);
            }
            return PartialView("");
        }
        public string GetDetailSo(int idSo, string ListIdSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);
            AdminUangJalan model = new AdminUangJalan();

            if (ListIdSo != null && ListIdSo != "" && ListIdSo != "null")
            {
                dbitem = RepoSalesOrder.FindByKontrak(idSo);
                List<int> ListIdDumy = ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;

                if (dbsoDummy.FirstOrDefault().IdAdminUangJalan.HasValue)
                    model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
            }
            else
            {
                model = new AdminUangJalan(dbitem.AdminUangJalan);
            }

            var tempModel = model.ModelListRemoval.Where(d => d.Status == "dispatched");
            if (tempModel.Count() > 0){
                return new JavaScriptSerializer().Serialize(tempModel.OrderBy(d => d.Id).Last());
            }
            else
                return new JavaScriptSerializer().Serialize(model);
        }

        public string GenerateAUJID()
        {
            foreach (Context.SettlementReguler sr in RepoSettlementReg.FindAll().Where(d => d.IdAdminUangJalan == null))
            {
                if (sr.LisSoKontrak != null && sr.LisSoKontrak != "")
                {
                    int kontid = int.Parse(sr.LisSoKontrak.Split('.')[0]);
                    Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(kontid);
                    sr.IdAdminUangJalan = sokls.IdAdminUangJalan;
                }
                else
                    sr.IdAdminUangJalan = sr.SalesOrder.AdminUangJalanId;
                RepoSettlementReg.save(sr, UserPrincipal.id);
            }
            return "aa";
        }

        public string generateModifiedDate()
        {
            foreach (Context.SettlementReguler sr in RepoSettlementReg.FindAll().Where(d => d.ModifiedDate == null))
            {
                if (sr.LisSoKontrak != null && sr.LisSoKontrak != "")
                {
                    int kontid = int.Parse(sr.LisSoKontrak.Split('.')[0]);
                    Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(kontid);
                    sr.ModifiedDate = sokls.MuatDate;
                }
                else
                {
                    Context.OrderHistory oh = RepoSalesOrder.oh().Where(d => d.SalesOrderId == sr.IdSalesOrder && d.StatusFlow == 7).FirstOrDefault();
                    if (oh == null)
                    {
                        sr.ModifiedDate = sr.AdminUangJalan.AUJTanggalMuat;
                    }
                    else
                    {
                        sr.ModifiedDate = oh.ProcessedAt;
                        sr.ModifiedBy = oh.PIC.ToString();
                    }
                }
                RepoSettlementReg.save(sr, UserPrincipal.id);
            }
            return "aa";
        }
    }
}