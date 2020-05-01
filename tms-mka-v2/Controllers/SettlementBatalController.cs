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
    public class SettlementBatalController : BaseController
    {
        private ISettlementBatalRepo RepoSettlementBatal;
        private ISalesOrderRepo RepoSalesOrder;
        private IAtmRepo RepoAtm;
        private IDataBoronganRepo RepoBor;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private IAdminUangJalanRepo RepoAdminUangJalan;
        private Ipbyd_detRepo Repopbyd_det;
        private IMasterPoolRepo RepoMasterPool;
        private ILookupCodeRepo RepoLookupCode;
        private ICustomerRepo RepoCustomer;
        private Igr_mstrRepo Repogr_mstr;
        private IBatalOrderRepo RepoBatalOrder;

        public SettlementBatalController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISettlementBatalRepo repoSettlementBatal, ISalesOrderRepo repoSalesOrder, IAtmRepo repoAtm,
            IDataBoronganRepo repoBor, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr, IAdminUangJalanRepo repoAdminUangJalan,
            Ipbyd_detRepo repopbyd_det, IMasterPoolRepo repoIMasterPool, ILookupCodeRepo repoLookupCode, ICustomerRepo repoCustomer, Igr_mstrRepo repogr_mstr, IBatalOrderRepo repoBatalOrder)
            : base(repoBase, repoLookup)
        {
            RepoAdminUangJalan = repoAdminUangJalan;
            RepoSettlementBatal = repoSettlementBatal;
            RepoSalesOrder = repoSalesOrder;
            RepoBatalOrder = repoBatalOrder;
            RepoAtm = repoAtm;
            RepoBor = repoBor;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            Repopbyd_det = repopbyd_det;
            RepoMasterPool = repoIMasterPool;
            RepoLookupCode = repoLookupCode;
            RepoCustomer = repoCustomer;
            Repogr_mstr = repogr_mstr;
        }
        [MyAuthorize(Menu = "Settlement Batal", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SettlementBatal").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SettlementBatal> items = RepoSettlementBatal.FindAll();

            List<SettlementBatalIndex> ListModel = new List<SettlementBatalIndex>();
            foreach (Context.SettlementBatal item in items)
            {
                if (item.IdSoKontrak != "" && item.IdSoKontrak != null)
                {
                    var data = item.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(p => p.IsBatalTruck && p.Id == int.Parse(item.IdSoKontrak.Split(',')[0])).ToList();
                    foreach (var itemGroup in data.ToList())
                    {
                        ListModel.Add(new SettlementBatalIndex(item, itemGroup));
                    }
                }
                else
                {
                    ListModel.Add(new SettlementBatalIndex(item));
                }
            }
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
        [MyAuthorize(Menu = "Settlement Batal", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.SettlementBatal dbitem = RepoSettlementBatal.FindByPK(id);
            if (dbitem.IdSoKontrak == "" || dbitem.IdSoKontrak == null)
            {
                SettlementBatal model = new SettlementBatal(dbitem);
                Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(dbitem.IdAdminUangJalan.Value);
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(dbitem.IdSalesOrder.Value);
                Context.AdminUangJalan db = RepoAdminUangJalan.FindByPK(model.IdAdminUangJalan == null ? 0 : model.IdAdminUangJalan.Value);
                if (db == null)
                {
                    if (dbso.AdminUangJalanId == null)
                        db = RepoAdminUangJalan.FindBySalesOrderId(dbso.Id);
                    else
                        db = RepoAdminUangJalan.FindByPK(dbso.AdminUangJalanId.Value);
                }
                ViewBag.SPBU = db.AdminUangJalanVoucherSpbu;
                ViewBag.KeteranganBatal = dbso.KeteranganBatal;
                ViewBag.kondisi = "Settlement Batal";
                if (model.ModelOncall != null)
                {
                    ViewBag.name = model.ModelOncall.SONumber;
                    ViewBag.KodeDriver = dbitem.Driver.KodeDriver;
                    ViewBag.KodeDriverOld = dbitem.Driver.KodeDriverOld;
                    ViewBag.DriverName = dbitem.Driver.NamaDriver;
                    if (auj.DataTruck == null)
                        ViewBag.VehicleNo = dbso.DataTruck.VehicleNo;
                    else
                        ViewBag.VehicleNo = auj.DataTruck.VehicleNo;
                }
                if (model.ModelPickup != null)
                    ViewBag.name = model.ModelPickup.SONumber;
                if (model.ModelKonsolidasi != null)
                    ViewBag.name = model.ModelKonsolidasi.SONumber;

                return View("Form", model);
            }
            else{
                SettlementBatal model = new SettlementBatal(dbitem);
                model.KasAktual = 0;
                model.TransferAktual = 0;
                model.SolarAktual = 0;
                model.KapalAktual = 0;
                model.KasSelisih = dbitem.KasKembali;
                model.TransferSelisih = dbitem.TransferKembali;
                model.SolarSelisih = dbitem.SolarKembali;
                model.KapalSelisih = dbitem.KapalKembali;
                Context.SalesOrder dbso = RepoSalesOrder.FindByPK(dbitem.IdSalesOrder.Value);
                ViewBag.sokls = dbso.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == int.Parse(dbitem.IdSoKontrak)).FirstOrDefault();
                ViewBag.SPBU = ViewBag.sokls.AdminUangJalan.AdminUangJalanVoucherSpbu;
                if (dbitem.Driver != null && dbitem.DataTruck != null)
                {
                    ViewBag.KodeDriver = ViewBag.sokls.AdminUangJalan.Driver.KodeDriver;
                    ViewBag.KodeDriverOld = ViewBag.sokls.AdminUangJalan.Driver.KodeDriverOld;
                    ViewBag.DriverName = ViewBag.sokls.AdminUangJalan.Driver.NamaDriver;
                    ViewBag.VehicleNo = dbitem.DataTruck;
                }
                return View("Form", model);
            }
        }
        [HttpPost]
        public ActionResult Edit(SettlementBatal model)
        {
            if (ModelState.IsValid)
            {
                Context.SettlementBatal dbitem = RepoSettlementBatal.FindByPK(model.Id);
                if (dbitem.IdSoKontrak == "" || dbitem.IdSoKontrak == null)
                {
                    Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
                    Context.AdminUangJalan db = null;
                    if (dbso.AdminUangJalanId != null)
                        db = RepoAdminUangJalan.FindByPK(dbso.AdminUangJalanId.Value);
                    if (dbitem.IdAdminUangJalan != null)
                        db = RepoAdminUangJalan.FindByPK(dbitem.IdAdminUangJalan.Value);
                    if (db == null)
                    {
                        if (dbso.AdminUangJalanId == null)
                            db = RepoAdminUangJalan.FindBySalesOrderId(dbso.Id);
                        else
                            db = RepoAdminUangJalan.FindByPK(dbso.AdminUangJalanId.Value);
                    }
                    Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                    string codeBT = "BT-" + dbso.SONumber;
                    int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
                    dbitem.Code = "SB-" + (dbitem.JenisBatal == "Batal Truk" ? "BT" : "BO") + dbso.SONumber + "-" + urutanBatal;
                    model.SetDb(dbitem);

                    //Jurnal Pengembalian Uang Jalan //(uang, dll dikembalikan utuh), mau utuh mau ngga, nominal cash yg dikembalikan mah ambil dari form :p
                    decimal nominalHutangUangJalanDriver = (model.KasAktual == null ? 0 : model.KasAktual.Value) + (model.TransferAktual == null ? 0 : model.TransferAktual.Value);
                    decimal nomDiakui = (model.KasDiakui == null ? 0 : model.KasDiakui.Value) + (dbitem.TransferDiakui == null ? 0 : dbitem.TransferDiakui.Value) + (model.SolarDiakui == null ? 0 : model.SolarDiakui.Value) + (model.KapalDiakui == null ? 0 : model.KapalDiakui.Value);
                    decimal nomSelisih = (dbitem.KasSelisih == null ? 0 : dbitem.KasSelisih.Value) + (dbitem.TransferSelisih == null ? 0 : dbitem.TransferSelisih.Value) + (dbitem.KapalSelisih == null ? 0 : dbitem.KapalSelisih.Value);

                    nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                    nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                    nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                    nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                    nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());
                    string code = "SB-" + (dbitem.JenisBatal == "Batal Truk" ? "BT" : "BO") + dbso.SONumber + "-" + urutanBatal;
                    decimal? TotalBiaya = nomDiakui;
                    int idx = 2;
                    if (nomDiakui > 0)
                        Repoglt_det.saveFromAc(2, code, nomDiakui, 0, Repoac_mstr.FindByPk(erpConfig.IdBiayaBatalJalan), dbso, null, null, null, null, null, db.IdDriver1);//BIAYA BATAL JALAN
                    if (dbitem.SolarAktual > 0)
                    {
                        foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                        {
                            idx++;
                            Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                            if (model.SPBUKembali != null && model.SPBUKembali.Contains(aujvs.Keterangan))
                            {//jurnal balik, ga jadi hutangnya
                                if (aujvs.Value > 0) {
                                    Repoglt_det.saveFromAc(idx, code, aujvs.Value, 0, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbso, aujvs.Keterangan, null, null, null, null, db.IdDriver1);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                                    TotalBiaya += aujvs.Value;
                                    Context.Customer customer = RepoCustomer.FindByPK(spbu.VendorId.Value);
                                    NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                                    con.Open();
                                    using (DataTable dt = new DataTable())
                                    {
                                        var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbso, decimal.Parse(aujvs.Value.ToString()) * -1);
                                        NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                                        da.Fill(dt);
                                        cmd.Dispose();
                                        con.Close();
                                    }
                                }
                            }
                        }
                    }
                    Repoglt_det.saveFromAc(idx, code, dbitem.SolarSelisih, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalanSementaraSolar), dbso, null, null, null, null, null, db.IdDriver1);
                    if (dbitem.SolarSelisih > 0)
                        TotalBiaya += dbitem.SolarSelisih;
                    if (dbitem.KapalAktual > 0){
                        foreach (Context.AdminUangJalanVoucherKapal aujvs in db.AdminUangJalanVoucherKapal){
                            idx++;
                            Repoglt_det.saveFromAc(idx, code, aujvs.Value, 0, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbso, aujvs.Keterangan, null, null, null, null, db.IdDriver1);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                            if (aujvs.Value > 0)
                                TotalBiaya += aujvs.Value;
                        }
                    }
                    if (nomSelisih > 0){
                        idx++;
                        Repoglt_det.saveFromAc(idx, code, nomSelisih, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbso, "Selisih Kas/Transfer/Kapal", null, null, null, null, db.IdDriver1);//PIUTANG DRIVER BATAL JALAN
                        TotalBiaya += nomSelisih;
                    }
                    //mau utuh mau ngga, nominal cash yg dikembalikan mah ambil dari form :p
                    if (model.KasAktual != null)
                    {
                        Repoglt_det.saveFromAc(2, code, model.KasAktual, 0, Repoac_mstr.FindByPk(RepoMasterPool.FindByIPAddress().IdCreditCash), dbso, null, null, null, null, null, db.IdDriver1);//BCA Audy 386-7957777 atau
                        TotalBiaya += model.KasAktual;
                    }
                    if (model.TransferAktual != null)
                    {
                        Repoglt_det.saveFromAc(2, code, model.TransferAktual, 0, Repoac_mstr.FindByPk(dbitem.IdCreditTf), dbso, null, null, null, null, null, db.IdDriver1);//BCA Audy 386-7957777 atau
                        TotalBiaya += model.TransferAktual;
                    }
                    var glt_oid = Guid.NewGuid().ToString();
                    string codePby = "SB-PK-" + (dbitem.JenisBatal == "Batal Truk" ? "BT" : "BO") + "-" + dbso.SONumber + "-" + urutanBatal;
                    if (db.KasbonDriver1 != null)
                    {
                        Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Settlement Batal " + dbitem.Code, db.IdDriver1.Value + 7000000);
                        Repopbyd_det.save(
                            glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + dbitem.Code, db.IdDriver1.Value + 7000000, erpConfig.IdAUJCredit.Value,
                            Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, db.KasbonDriver1.Value
                        );
                        idx += 1;
                        Repoglt_det.saveFromAc(idx, code, db.KasbonDriver1.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbso, null, null, null, null, null, db.IdDriver1);//PIUTANG DRIVER BATAL JALAN
                        TotalBiaya += db.KasbonDriver1.Value;
                    }
                    Repoglt_det.saveFromAc(1, code, 0, TotalBiaya, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso, null, null, null, null, null, db.IdDriver1);//Hutang Uang Jalan Driver

                    glt_oid = Guid.NewGuid().ToString();
                    RepoSettlementBatal.save(dbitem, UserPrincipal.id, "Settlement Batal");
                    codePby = "SB-" + (dbitem.JenisBatal == "Batal Truk" ? "BT" : "BO") + "-" + dbso.SONumber + "-" + urutanBatal;
                    Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Settlement Batal " + dbitem.Code, db.IdDriver1.Value + 7000000);
                    Repopbyd_det.save(
                        glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Settlement Batal " + dbitem.Code, db.IdDriver1.Value + 7000000, erpConfig.IdAUJCredit.Value,
                        Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, nomSelisih + dbitem.SolarSelisih.Value
                    );
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dbso, 15);
                }
                return RedirectToAction("Index");
            }

            return View("Form", model);
        }

        [MyAuthorize(Menu = "Settlement Batal", Action="read")]
        public ActionResult View(int id)
        {
            Context.SettlementBatal dbitem = RepoSettlementBatal.FindByPK(id);
            Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(dbitem.IdAdminUangJalan.Value);
            SettlementBatal model = new SettlementBatal(dbitem);
            ViewBag.kondisi = "Settlement Batal";
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(dbitem.IdSalesOrder.Value);
            ViewBag.KeteranganBatal = dbso.KeteranganBatal;
            if (model.ModelOncall != null)
            {
                ViewBag.name = model.ModelOncall.SONumber;
                ViewBag.KodeDriver = dbitem.Driver.KodeDriver;
                ViewBag.KodeDriverOld = dbitem.Driver.KodeDriverOld;
                ViewBag.DriverName = dbitem.Driver.NamaDriver;
                if (auj.DataTruck == null)
                    ViewBag.VehicleNo = dbso.DataTruck.VehicleNo;
                else
                    ViewBag.VehicleNo = auj.DataTruck.VehicleNo;
            }
            if (model.ModelPickup != null)
                ViewBag.name = model.ModelPickup.SONumber;
            if (model.ModelKonsolidasi != null)
                ViewBag.name = model.ModelKonsolidasi.SONumber;
            return View(model);
        }
    }
}