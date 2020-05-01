using System;
using Npgsql;
using System.Data;
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
    public class ReportController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IRevisiTanggalRepo RepoRevisiTanggal;
        private ISettlementBatalRepo RepoSettBatal;
        private IBatalOrderRepo RepoBatalOrder;
        private IUserRepo RepoUser;
        private IDaftarHargaOnCallRepo RepoDHO;
        private IDaftarHargaKonsolidasiRepo RepoDHK;
        private IRuteRepo RepoRute;
        private IAtmRepo RepoAtm;
        private IDataBoronganRepo RepoBor;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private IHistoryJalanTruckRepo RepoHistoryJalanTruck;
        private IRemovalRepo RepoRemovalRepo;
        private IDokumenRepo RepoDokumen;
        private IRemovalRepo RepoRemoval;
        private IAtmRepo Repoatm;
        private ISolarInapRepo RepoSolarInap;
        private IAuditrailRepo RepoAuditrail;
        private IAdminUangJalanRepo RepoAdminUangJalan; private ICustomerRepo RepoCustomer; private ILookupCodeRepo RepoLookupCode; private IDaftarHargaKonsolidasiRepo RepoDHKonsolidasi;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private Ipbyd_detRepo Repopbyd_det;
        private IMasterPoolRepo RepoMasterPool;
        private ISettlementRegRepo RepoSettlementReg;
        private ISettlementBatalRepo RepoSettlementBatal;

        public ReportController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IRevisiTanggalRepo repoRevisiTanggal, ISettlementBatalRepo repoSettBatal,
            IBatalOrderRepo repoBatalOrder, IUserRepo repoUser, IDaftarHargaKonsolidasiRepo repoDHK, IRuteRepo repoRute, IAtmRepo repoAtm, IDataBoronganRepo repoBor, IAtmRepo repoatm,
            ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo, IHistoryJalanTruckRepo repoHistoryJalanTruck, IRemovalRepo repoRemovalRepo, IDaftarHargaOnCallRepo repoDHO, IDokumenRepo repoDokumen,
            IRemovalRepo repoRemoval, ISolarInapRepo repoSolarInap, IAuditrailRepo repoAuditrail, IAdminUangJalanRepo repoAdminUangJalan, ICustomerRepo repoCustomer, ILookupCodeRepo repoLookupCode,
            IDaftarHargaKonsolidasiRepo repoDHKonsolidasi, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr,
            Ipbyd_detRepo repopbyd_det, IMasterPoolRepo repoIMasterPool, ISettlementRegRepo repoSettlementReg, ISettlementBatalRepo repoSettlementBatal)
            : base(repoBase, repoLookup)
        {
            RepoDHO = repoDHO;
            RepoSalesOrder = repoSalesOrder;
            RepoDHKonsolidasi = repoDHKonsolidasi;
            RepoRevisiTanggal = repoRevisiTanggal;
            RepoSettBatal = repoSettBatal;
            RepoBatalOrder = repoBatalOrder;
            RepoUser = repoUser;
            RepoDHK = repoDHK;
            RepoRute = repoRute;
            RepoAtm = repoAtm;
            RepoBor = repoBor;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
            RepoHistoryJalanTruck = repoHistoryJalanTruck;
            RepoRemovalRepo = repoRemovalRepo;
            RepoDokumen = repoDokumen;
            RepoRemoval = repoRemoval;
            Repoatm = repoatm;
            RepoSolarInap = repoSolarInap;
            RepoAuditrail = repoAuditrail;
            RepoAdminUangJalan = repoAdminUangJalan;
            RepoCustomer = repoCustomer;
            RepoLookupCode = repoLookupCode;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            Repopbyd_det = repopbyd_det;
            RepoMasterPool = repoIMasterPool;
            RepoLookupCode = repoLookupCode;
            RepoSettlementReg = repoSettlementReg;
            RepoSettlementBatal = repoSettlementBatal;
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action = "read")]
        public ActionResult IndexTfBOBS()
        {
            ViewBag.Title = "Batal Order Belum Settlement - Transfer";
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "BatalOrderBelumSettlement" && d.Controller == "Report").ToList();
            return View();
        }

        public string BindingTfBOBS()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllTfBOBSReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.AdminUangJalan so in items)
            {
                Context.SalesOrder item = RepoSalesOrder.FindByPK(so.SalesOrderId);
                if (item == null)
                {
                    item = RepoSalesOrder.FindByAUJ(so.Id);
                }
                if (item != null)
                {
                    var _model = new AdminUangJalanIndex(item, so);
                    int IdJenisTruck;
                    string IdRute;
                    if (item.SalesOrderOncallId.HasValue)
                    {
                        RepoDHO.FindRuteTruk(item.SalesOrderOncall.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        if (IdRute != null && IdRute != "")
                        {
                            List<string> ListRute = new List<string>();
                            List<string> ListIdRute = IdRute.Split(',').ToList();
                            foreach (string idTruck in ListIdRute)
                            {
                                ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                            }
                            _model.Rute = string.Join(", ", ListRute);
                        }
                    }
                    else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        RepoDHK.FindRuteTruk(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        {
                            if (IdRute != "")
                            {
                                List<string> ListRute = new List<string>();
                                List<string> ListIdRute = IdRute.Split(',').ToList();
                                foreach (string idTruck in ListIdRute)
                                {
                                    ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                                }
                                _model.Rute = string.Join(", ", ListRute);
                            }
                        }
                    }
                    if (so.AdminUangJalanUangTf.Any(d => d.Keterangan != "Tunai" && d.Value > 0 && (d.JumlahTransfer == 0 || d.JumlahTransfer == null)))
                        _model.Status = "Transfer";
                    else if (so.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai" && d.Value > 0 && (d.JumlahTransfer == 0 || d.JumlahTransfer == null)))
                        _model.Status = "Tunai";
                    else
                        _model.Status = so.Status;
                    _model.TfExecuted = so.TfExecuted;
                    _model.CashExecuted = so.CashExecuted;
                    ListModel.Add(_model);
                }
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllTfBOBSReport(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Kasir Cash", Action = "read")]
        public ActionResult IndexKasBOBS()
        {
            ViewBag.Title = "Batal Order Belum Settlement - Kas";
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "BatalOrderBelumSettlement" && d.Controller == "Report").ToList();
            return View();
        }

        public string BindingKasBOBS()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllKasBOBSReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.AdminUangJalan so in items)
            {
                Context.SalesOrder item = RepoSalesOrder.FindByPK(so.SalesOrderId);
                if (item == null)
                {
                    item = RepoSalesOrder.FindByAUJ(so.Id);
                }
                if (item != null)
                {
                    var _model = new AdminUangJalanIndex(item, so);
                    int IdJenisTruck;
                    string IdRute;
                    if (item.SalesOrderOncallId.HasValue)
                    {
                        RepoDHO.FindRuteTruk(item.SalesOrderOncall.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        if (IdRute != null && IdRute != "")
                        {
                            List<string> ListRute = new List<string>();
                            List<string> ListIdRute = IdRute.Split(',').ToList();
                            foreach (string idTruck in ListIdRute)
                            {
                                ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                            }
                            _model.Rute = string.Join(", ", ListRute);
                        }
                    }
                    else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        RepoDHK.FindRuteTruk(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        {
                            if (IdRute != "")
                            {
                                List<string> ListRute = new List<string>();
                                List<string> ListIdRute = IdRute.Split(',').ToList();
                                foreach (string idTruck in ListIdRute)
                                {
                                    ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                                }
                                _model.Rute = string.Join(", ", ListRute);
                            }
                        }
                    }
                    if (so.AdminUangJalanUangTf.Any(d => d.Keterangan != "Tunai" && d.Value > 0 && (d.JumlahTransfer == 0 || d.JumlahTransfer == null)))
                        _model.Status = "Transfer";
                    else if (so.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai" && d.Value > 0 && (d.JumlahTransfer == 0 || d.JumlahTransfer == null)))
                        _model.Status = "Tunai";
                    else
                        _model.Status = so.Status;
                    _model.TfExecuted = so.TfExecuted;
                    _model.CashExecuted = so.CashExecuted;
                    ListModel.Add(_model);
                }
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllKasBOBSReport(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Settlement Batal", Action = "read")]
        public ActionResult SettlementBatal()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SettlementBatal").ToList();
            return View();
        }
        public string BindingSB()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SettlementBatal> items = RepoSettlementBatal.FindAllReport();

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

        [MyAuthorize(Menu = "Settlement Batal", Action = "read")]
        public ActionResult SettlementBatalKontrak()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SettlementBatal").ToList();
            return View();
        }
        public string BindingSBKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SettlementBatal> items = RepoSettlementBatal.FindAllReportKontrak();

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

        [MyAuthorize(Menu = "Daftar Barang", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SalesOrderKonsolidasi").ToList();
            return View();
        }

        public ActionResult ViewSr(int id)
        {
            Context.SettlementReguler dbitem = RepoSettlementReg.FindByPK(id);
            SettlementReg model = new SettlementReg(dbitem, Repoatm.FindAll());
            if (model.ModelOncall != null)
                ViewBag.name = model.ModelOncall.SONumber;
            if (model.ModelPickup != null)
                ViewBag.name = model.ModelPickup.SONumber;
            if (model.ModelKonsolidasi != null)
                ViewBag.name = model.ModelKonsolidasi.SONumber;
            if (model.ModelKontrak != null)
                ViewBag.name = model.ModelKontrak.SONumber;

            return View("FormTfSettlement", model);
        }

        public string BindingTfSR()
        {
            List<KasirTf> ListModel = new List<KasirTf>();
            List<Context.SettlementReguler> ItemsSettlement = RepoSettlementReg.FindAll().Where(d => d.TfExecuted == true || d.CashExecuted == true).ToList();
            foreach (Context.SettlementReguler item in ItemsSettlement)
            {
                KasirTf settlement = new KasirTf(item.SalesOrder);
                if (item.LisSoKontrak != "" && item.LisSoKontrak != null)
                {
                    Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(item.LisSoKontrak.Split('.')[0]));
                    settlement = new KasirTf(RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value), item.AdminUangJalan);
                    if (item.IdAdminUangJalan.HasValue)
                    {
                        settlement.SoNo = item.AdminUangJalan.SONumber;
                        settlement.IdDriver = item.AdminUangJalan.Driver1.KodeDriver;
                        settlement.KodeDriverOld = item.AdminUangJalan.Driver1.KodeDriverOld;
                        settlement.Driver = item.AdminUangJalan.Driver1.NamaDriver;
                        settlement.VehicleNo = item.AdminUangJalan.DataTruck.VehicleNo;

                        settlement.KodeNama = sokls.SalesOrderKontrak.Customer.CustomerCodeOld;
                        settlement.Customer = sokls.SalesOrderKontrak.Customer.CustomerNama;
                        settlement.Tanggal = sokls.MuatDate;
                    }
                    else if (sokls != null)
                    {
                        if (sokls.Driver1 != null)
                        {
                            settlement.IdDriver = sokls.Driver1.KodeDriver;
                            settlement.Driver = sokls.Driver1.NamaDriver;
                        }
                        if (sokls.DataTruck != null)
                            settlement.VehicleNo = sokls.DataTruck.VehicleNo;
                        settlement.Customer = sokls.SalesOrderKontrak.Customer.CustomerNama;
                        settlement.Tanggal = sokls.MuatDate;
                    }
                }
                settlement.Status = "Settlement";
                settlement.Jumlah = item.TotalTf;
                settlement.Realisasi = 0;
                settlement.IdSettlement = item.Id;
                settlement.JadwalTransfer = item.TanggalTf == null ? "" : item.TanggalTf.Value.ToString("dd-MM-yyyy");
                ListModel.Add(settlement);
            }
            ListModel = ListModel.Where(d => d.Jumlah > 0 && d.Keterangan != "Tunai").ToList();
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }

        public ActionResult SettlementReguler()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Kasir").ToList();
            return View("SettlementReguler");
        }
        
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllListOrder(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<ListOrder> ListModel = new List<ListOrder>();
            foreach (Context.SalesOrder item in items)
            {
                ListModel.Add(new ListOrder(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountListOrder(param.Filters), data = ListModel });
        }
        public ActionResult EditTfSettlement(int id)
        {
            Context.SettlementReguler dbitem = RepoSettlementReg.FindByPK(id);
            SettlementReg model = new SettlementReg(dbitem, Repoatm.FindAll());
            if (model.ModelOncall != null)
                ViewBag.name = model.ModelOncall.SONumber;
            if (model.ModelPickup != null)
                ViewBag.name = model.ModelPickup.SONumber;
            if (model.ModelKonsolidasi != null)
                ViewBag.name = model.ModelKonsolidasi.SONumber;
            if (model.ModelKontrak != null)
                ViewBag.name = model.ModelKontrak.SONumber;

            return View("FormTfSettlement", model);
        }
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderKontrak model = new SalesOrderKontrak(dbitem);

            ViewBag.name = model.SONumber;
            return View("FormKontrak", model);
        }

        public string BindingSOKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKontrak(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            List<SalesOrderKontrak> ListModel = new List<SalesOrderKontrak>();
            foreach (Context.SalesOrder item in items)
            {
                ListModel.Add(new SalesOrderKontrak(item));
            }

            int total = RepoSalesOrder.CountKontrak(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [HttpGet]
        public PartialViewResult GetPartialSo(int idSo, string ListIdSo)
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
                List<int> ListIdDumy = ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
                AdminUangJalan model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
                return PartialView("SalesOrderKontrak/_PartialFormReadOnly", model);
            }
            return PartialView("");
        }

        public ActionResult DetailSolarInap(int id)
        {
            Context.SolarInap dbitem = RepoSolarInap.FindByPK(id);
            SolarInap model = new SolarInap(dbitem);
            model.StepKe += 1;
            ViewBag.name = model.Id;
            ViewBag.IdSO = 0;
            ViewBag.BankAccount = Repoac_mstr.FindByPk(dbitem.IdCreditTf);
            return View("FormSolarInap", model);
        }
        public ActionResult SolarInap()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SolarInap").ToList();
            return View();
        }

        public ActionResult AdminUangJalanKontrak()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "AdminUangJalan").ToList();
            return View();
        }

        public ActionResult SalesOrderKontrak()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SalesOrderKontrak").ToList();
            return View();
        }

        public string BindingSolarInap(string user_types)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            Context.User user = RepoUser.FindByPK(UserPrincipal.id);
            List<SolarInap> ListModel = new List<SolarInap>();

            List<Context.SolarInap> items = RepoSolarInap.FindAllReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            foreach (Context.SolarInap item in items)
            {
                if (item.SalesOrderKontrakListSOId.HasValue)
                {
                    var soKontrak = item.SO.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == item.SalesOrderKontrakListSOId).FirstOrDefault();
                    ListModel.Add(new SolarInap(item, soKontrak));
                }
                else
                { ListModel.Add(new SolarInap(item)); }
            }
            int total = RepoSolarInap.Count(param.Filters);
            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        [MyAuthorize(Menu = "Order", Action = "read")]
        public ActionResult Detail(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);
            Context.RevisiTanggal dbitem = RepoRevisiTanggal.FindBySo(idSo);
            
            RevisiTanggal model = new RevisiTanggal(dbso);

            if (RepoRevisiTanggal.FindBySo(idSo) != null)
            {
                model = new RevisiTanggal(dbitem);
            }

            return View("Detail", model);
        }

        [MyAuthorize(Menu = "Order", Action = "read")]
        public ActionResult DetailKontrak(int id, int idsokontrak)
        {
            Context.SalesOrderKontrakListSo soKontrak = RepoSalesOrderKontrakListSo.FindByPK(id);
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(idsokontrak);
            //ambil jumlah so yang sama admin uang jalan nya
            AdminUangJalan model = new AdminUangJalan();
            if (soKontrak.IdAdminUangJalan.HasValue)
                model = new AdminUangJalan(soKontrak.AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            model.ListIdSo = idsokontrak.ToString();
            ViewBag.Status = soKontrak.Status;
            model.ModelKontrak = new SalesOrderKontrak(dbitem);
            ViewBag.noSo = soKontrak.NoSo;
            ViewBag.ListIdSo = id.ToString();
            if (model.IdDriver1.HasValue)
            {
                model.IdDriver1 = model.IdDriver1;
                model.NamaDriver1 = model.NamaDriver1;
                model.KeteranganGanti1 = model.KeteranganGanti1;
            }
            else
            {
                model.IdDriver1 = soKontrak.Driver1Id;
                try
                {
                    model.NamaDriver1 = soKontrak.Driver1.KodeDriver + " - " + soKontrak.Driver1.NamaDriver;
                }
                catch (Exception) { }
            }
            if (model.IdDriver2.HasValue)
            {
                model.IdDriver2 = model.IdDriver2;
                model.NamaDriver2 = model.NamaDriver2;
                model.KeteranganGanti2 = model.KeteranganGanti2;
            }
            else
            {
                if (soKontrak.Driver2Id.HasValue)
                {
                    model.IdDriver2 = soKontrak.Driver2Id;
                    model.NamaDriver2 = soKontrak.Driver2.KodeDriver + " - " + soKontrak.Driver2.NamaDriver;
                }
            }
            ViewBag.Title = "Order Detail " + dbitem.SalesOrderKontrak.SONumber;
            return View("Form", model);
        }

        [MyAuthorize(Menu = "Surat Jalan", Action = "read")]
        public ActionResult SuratJalan()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "Dokumen").ToList();
            ViewBag.caller = "admin";
            ViewBag.Title = "Report Dokumen Admin Surat Jalan";
            return View();
        }

        public string BindingHistoryDetail(int idSo, string idSODetail=null)
        {
            List<OrderHistory> ListModel = new List<OrderHistory>();
            if (idSODetail == null)
            {
                List<Context.OrderHistory> items = RepoSalesOrder.FindAllHistory(idSo);
                foreach (Context.OrderHistory item in items)
                {
                    if (item.ListSo == null || item.ListSo == "" || item.ListSo != null && item.ListSo != "" && item.ListSo.Contains(idSODetail))
                    {
                        if (item.PIC == null)
                            ListModel.Add(new OrderHistory(item, ""));
                        else
                            ListModel.Add(new OrderHistory(item, (RepoUser.FindByPK(item.PIC.Value).Fristname + " " + RepoUser.FindByPK(item.PIC.Value).Lastname)));
                    }
                }
            }
            else
            {
                List<Context.OrderHistory> items = RepoSalesOrder.FindAllHistory(idSo).Where(d => d.StatusFlow == 1 || d.StatusFlow == 2 || d.ListSo != null && d.ListSo.Split(',').Contains(idSODetail)).ToList();
                foreach (Context.OrderHistory item in items)
                {
                    if (item.PIC == null)
                        ListModel.Add(new OrderHistory(item, ""));
                    else
                        ListModel.Add(new OrderHistory(item, (RepoUser.FindByPK(item.PIC.Value).Fristname + " " + RepoUser.FindByPK(item.PIC.Value).Lastname)));
                }
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public string BindingHistoryInapDetail(int idSi)
        {
            List<Context.SolarInapHistory> items = RepoSolarInap.FindAllHistory(idSi);
            List<SolarInapHistory> ListModel = new List<SolarInapHistory>();
            foreach (Context.SolarInapHistory item in items)
            {
                if (item.PIC == null)
                    ListModel.Add(new SolarInapHistory(item, ""));
                else
                    ListModel.Add(new SolarInapHistory(item, (RepoUser.FindByPK(item.PIC.Value).Fristname + " " + RepoUser.FindByPK(item.PIC.Value).Lastname)));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }

        [MyAuthorize(Menu = "Order", Action = "read")]
        public ActionResult Order()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "ListOrder").ToList();
            return View();
        }

        [MyAuthorize(Menu = "Order", Action = "read")]
        public ActionResult OrderKontrak()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "ListOrder").ToList();
            return View();
        }

        public string BindingSJ(string caller)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.Dokumen> items = RepoDokumen.FindAllOnCallSJReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            foreach (var item in items)
            {
                DokumenIndex dok = new DokumenIndex(item);
                dok.NoSo = item.SONumber;
                if (dok.VehicleNo == null && RepoSalesOrderKontrakListSo.FindByNoSo(item.SONumber) != null)
                    dok.VehicleNo = RepoSalesOrderKontrakListSo.FindByNoSo(item.SONumber).DataTruck.VehicleNo;
                if (dok.NamaDriver == null && RepoSalesOrderKontrakListSo.FindByNoSo(item.SONumber) != null)
                    dok.NamaDriver = RepoSalesOrderKontrakListSo.FindByNoSo(item.SONumber).Driver1.NamaDriver;
                if (dok.TanggalMuat == null && RepoSalesOrderKontrakListSo.FindByNoSo(item.SONumber) != null)
                    dok.TanggalMuat = RepoSalesOrderKontrakListSo.FindByNoSo(item.SONumber).MuatDate;
                ListModel.Add(dok);
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoDokumen.CountOnCallSJReport(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Sales Order Konsolidasi Daftar Barang", Action = "update")]
        public ActionResult ViewDaftarBarang(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderKonsolidasi model = new SalesOrderKonsolidasi(dbitem);
            model.NamaCustomer = RepoLookupCode.FindByPK(model.CustomerId).Nama;
            model.KodeNama = RepoCustomer.FindByPK(model.SupplierId.Value).CustomerCodeOld;
            model.SupplierName = RepoCustomer.FindByPK(model.SupplierId.Value).CustomerNama;
            Context.DaftarHargaKonsolidasiItem dhoitem = RepoDHKonsolidasi.FindItemByPK(model.RuteId.Value);
            if (dhoitem != null)
                ViewBag.ListNamaRute = dhoitem.ListNamaRute;

            ViewBag.name = model.SONumber;
            return View("FormDaftarBarang", model);
        }

        public string BindingKonsolidasi()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllReportKonsolidasi(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            List<SalesOrderKonsolidasi> ListModel = new List<SalesOrderKonsolidasi>();
            foreach (Context.SalesOrder item in items)
            {
                SalesOrderKonsolidasi newItem = (new SalesOrderKonsolidasi(item));
                newItem.NamaCustomer = RepoLookupCode.FindByPK(newItem.CustomerId).Nama;
                if (RepoCustomer.FindByPK(newItem.SupplierId.Value) != null)
                {
                    newItem.KodeNama = RepoCustomer.FindByPK(newItem.SupplierId.Value).CustomerCodeOld;
                    newItem.SupplierName = RepoCustomer.FindByPK(newItem.SupplierId.Value).CustomerNama;
                }
                ListModel.Add(newItem);
            }

            int total = RepoSalesOrder.CountReportKonsolidasi(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string BindingKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrderKontrakListSo> items = RepoSalesOrder.FindAllKontrakListSo(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<ListOrder> ListModel = new List<ListOrder>();
            foreach (var itemKontrakPerOrder in items.OrderBy(t => t.MuatDate).ToList())
            {
                ListModel.Add(new ListOrder(itemKontrakPerOrder));
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrderKontrakListSo.CountKontrakListSo(param.Filters), data = ListModel });
        }

        public string BindingAUJKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllAdminUangJalanKontrakReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.AdminUangJalan auj in items)
            {
                List<Context.SalesOrderKontrakListSo> itemGroup = RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id);
                AdminUangJalanIndex aujj = new AdminUangJalanIndex(auj, itemGroup, "hiburan");
                ListModel.Add(aujj);
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountFindAllAdminUangJalanKontrakReport(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Admin Uang Jalan", Action = "read")]
        public ActionResult AdminUangJalan()
        {
            ViewBag.Title = "Report Admin Uang Jalan";
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "AdminUangJalan").ToList();
            return View();
        }

        [MyAuthorize(Menu = "Batal Order Belum Settlement", Action = "read")]
        public ActionResult BatalOrderBelumSettlement()
        {
            ViewBag.Title = "Batal Order Belum Settlement";
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "BatalOrderBelumSettlement" && d.Controller == "Report").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult PostView(AdminUangJalan model, string btnsave)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.AdminUangJalan db = dbitem.AdminUangJalan;
            db.KeteranganAdmin = model.KeteranganAdmin;
            dbitem.UpdatedBy = UserPrincipal.id;
            RepoSalesOrder.save(dbitem);
            return RedirectToAction("View", new {Id = dbitem.Id, AdminUangJalanId = db.Id});
        }

        [HttpPost]
        public ActionResult PostViewKontrak(int Id, int IdSalesOrder, string btnsave, string KeteranganAdmin, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(IdSalesOrder);
            Context.AdminUangJalan db = RepoAdminUangJalan.FindByPK(Id);
            db.KeteranganAdmin = KeteranganAdmin;
            RepoAdminUangJalan.save(db);//id=2044&listSo=12148.12160.12136.12172
            return RedirectToAction("ViewKontrak", new { id = dbitem.Id, listSo = listSo, aujId = db.Id });
        }

        public ActionResult GenerateCustomerId(int Id, int IdSalesOrder, string btnsave, string KeteranganAdmin, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(IdSalesOrder);
            Context.AdminUangJalan db = RepoAdminUangJalan.FindByPK(Id);
            db.KeteranganAdmin = KeteranganAdmin;
            RepoAdminUangJalan.save(db);//id=2044&listSo=12148.12160.12136.12172
            return RedirectToAction("ViewKontrak", new { id = dbitem.Id, listSo = listSo });
        }

        public ActionResult Maju1Flow(int idSo, int? idsokontrak=null)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            if (idsokontrak == null)
            {
                Context.AdminUangJalan db = dbso.AdminUangJalan;
                if (!dbso.AdminUangJalan.AdminUangJalanUangTf.Any(d => d.JumlahTransfer > 0))
                {
                    string strQuery = GenerateDokumen(idSo, dbso.SalesOrderOncall.Customer);
                    dbso.Status = "dispatched";
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoAuditrail.saveMajuHistory(dbso, strQuery);
                    RepoSalesOrder.save(dbso);
                }
                String code = "MF-" + dbso.SONumber;
                if (dbso.PendapatanDiakui != true)
                {//START beruhubungan dgn utang piutang pendapatan hanya sampai sini, hanya sekali per SO
                    if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        foreach (var item in dbso.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                        {
                            decimal harga = RepoSalesOrder.HargaKonsolidasiPerItem(item);
                            Context.Customer supplier = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.NamaTagihanId.Value);
                            string customer = supplier.CustomerCodeOld + "-" + supplier.CustomerNama;
                            Context.SalesOrder sok = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                            if (!sok.SalesOrderKonsolidasi.ProcessedByKasir)
                            {
                                sok.SalesOrderKonsolidasi.ProcessedByKasir = true;
                                Repoglt_det.saveFromAc(1, "PD-" + dbso.SONumber + ", " + sok.SONumber, harga, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbso, "Piutang Belum Ditagih - " + customer);
                                Repoglt_det.saveFromAc(2, "PD-" + dbso.SONumber + ", " + sok.SONumber, 0, harga, Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbso, "Pendapatan Belum Ditagih - " + customer);
                            }
                        }
                    }
                    else
                    {
                        Repoglt_det.saveFromAc(1, "PD-" + dbso.SONumber, RepoSalesOrder.Harga(dbso), 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbso);
                        Repoglt_det.saveFromAc(2, "PD-" + dbso.SONumber, 0, RepoSalesOrder.Harga(dbso), Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbso);
                    }
                    dbso.PendapatanDiakui = true;//END beruhubungan dgn utang customer pendapatan hanya sampai sini, hanya sekali per SO
                }

                decimal nominalHutangUangJalanDriver = 0;
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());

                Repoglt_det.saveFromAc(1, code, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso);//Hutang Uang Jalan Driver
                string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                int idx = 0;
                foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu.Where(d => d.Value > 0))
                {
                    Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                    if (spbu != null)
                    {
                        idx++;
                        Context.Customer customer = RepoCustomer.FindByPK(spbu.VendorId.Value);
                        Repoglt_det.saveFromAc(idx, code, 0, aujvs.Value, Repoac_mstr.FindByPk(spbu.ac_id), dbso, aujvs.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                        idx++;
                        NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                        con.Open();
                        using (DataTable dt = new DataTable())
                        {
                            var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbso, decimal.Parse(aujvs.Value.ToString()));
                            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dt);
                            cmd.Dispose();
                            con.Close();
                        }
                    }
                }
                foreach (Context.AdminUangJalanVoucherKapal aujvk in db.AdminUangJalanVoucherKapal.Where(d => d.Value > 0))
                {
                    Context.LookupCode kapal = RepoLookup.FindByName(aujvk.Keterangan);
                    idx++;
                    Context.Customer customer = RepoCustomer.FindByPK(kapal.VendorId.Value);
                    Repoglt_det.saveFromAc(idx, code, 0, aujvk.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvk.Keterangan).ac_id), dbso, aujvk.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                    idx++;
                    NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                    con.Open();
                    using (DataTable dt = new DataTable())
                    {
                        var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbso, decimal.Parse(aujvk.Value.ToString()));
                        NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                        da.Fill(dt);
                        cmd.Dispose();
                        con.Close();
                    }
                }

                if (db.PotonganB > 0)//Batal Jalan
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbso);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganP > 0)
                {//Pribadi
                    idx++;
                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbso);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganK > 0)
                {//Klaim
                    idx++;
                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbso);
                }
                if (db.PotonganT > 0)
                {//Tabungan
                    idx++;
                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbso);
                }
                db.JurnalVoucher = true;
                return RedirectToAction("Order");
            }
            else
            {
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(idSo);
                dbso = RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value);

                if (sokls.Status == "admin uang jalan" && !sokls.AdminUangJalan.AdminUangJalanUangTf.Any(d => d.Value > 0))
                {
                    string strQuery = GenerateDokumen(dbso.Id, dbso.SalesOrderKontrak.Customer, sokls.Id.ToString());
                    sokls.Status = "dispatched";
                    sokls.StatusFlow = "DISPATCHED";
                    RepoAuditrail.saveMajuHistory(dbso, strQuery);
                    RepoSalesOrderKontrakListSo.save(sokls);
                }
                return RedirectToAction("OrderKontrak");
            }
        }

        public string BindingAUJ()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllAdminUangJalanReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.AdminUangJalan so in items)
            {
                Context.SalesOrder item = RepoSalesOrder.FindByPK(so.SalesOrderId);
                if (item == null)
                    item = RepoSalesOrder.FindByAUJ(so.Id);
                if (item != null)//rute oncall dan konsolidasi
                {
                    var _model = new AdminUangJalanIndex(item, so);
                    int IdJenisTruck;
                    string IdRute;
                    if (item.SalesOrderOncallId.HasValue)
                    {
                        RepoDHO.FindRuteTruk(item.SalesOrderOncall.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        if (IdRute != null && IdRute != "")
                        {
                            List<string> ListRute = new List<string>();
                            List<string> ListIdRute = IdRute.Split(',').ToList();
                            foreach (string idTruck in ListIdRute)
                            {
                                ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                            }
                            _model.Rute = string.Join(", ", ListRute);
                        }
                    }
                    else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        RepoDHK.FindRuteTruk(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        {
                            if (IdRute != "")
                            {
                                List<string> ListRute = new List<string>();
                                List<string> ListIdRute = IdRute.Split(',').ToList();
                                foreach (string idTruck in ListIdRute)
                                {
                                    ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                                }
                                _model.Rute = string.Join(", ", ListRute);
                            }
                        }
                    }
                    _model.Status = so.Status;
                    if (so.DataTruck != null)
                        _model.VehicleNo = so.DataTruck.VehicleNo;
                    ListModel.Add(_model);
                }
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllAdminUangJalanReport(param.Filters), data = ListModel });
        }

        public FileContentResult ExportAdminUangJalan(string NoSo)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            ExcelPackage pck = new ExcelPackage();
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllAdminUangJalanReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");
            ws.Cells[1, 1].Value = "Status";
            ws.Cells[1, 2].Value = "Jenis";
            ws.Cells[1, 3].Value = "SO Number";
            ws.Cells[1, 4].Value = "Kode Nama";
            ws.Cells[1, 5].Value = "Customer";
            ws.Cells[1, 6].Value = "Vehicle No";
            ws.Cells[1, 7].Value = "Jenis Truk";
            ws.Cells[1, 8].Value = "ID Driver";
            ws.Cells[1, 9].Value = "Driver";
            ws.Cells[1, 10].Value = "Rute";
            ws.Cells[1, 11].Value = "Tanggal Muat";
            ws.Cells[1, 12].Value = "Jam Muat";
            ws.Cells[1, 13].Value = "Jumlah Rit";
            ws.Cells[1, 14].Value = "Keterangan Penggantian";
            for (int i = 0; i < items.Count(); i++)
            {
                Context.AdminUangJalan so = items[i];
                Context.SalesOrder item = RepoSalesOrder.FindByPK(so.SalesOrderId);
                if (item == null)
                    item = RepoSalesOrder.FindByAUJ(so.Id);
                if (item != null)//rute oncall dan konsolidasi
                {
                    var _model = new AdminUangJalanIndex(item, so);
                    int IdJenisTruck;
                    string IdRute;
                    if (item.SalesOrderOncallId.HasValue)
                    {
                        RepoDHO.FindRuteTruk(item.SalesOrderOncall.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        if (IdRute != null && IdRute != "")
                        {
                            List<string> ListRute = new List<string>();
                            List<string> ListIdRute = IdRute.Split(',').ToList();
                            foreach (string idTruck in ListIdRute)
                            {
                                ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                            }
                            _model.Rute = string.Join(", ", ListRute);
                        }
                    }
                    else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        RepoDHK.FindRuteTruk(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        {
                            if (IdRute != "")
                            {
                                List<string> ListRute = new List<string>();
                                List<string> ListIdRute = IdRute.Split(',').ToList();
                                foreach (string idTruck in ListIdRute)
                                {
                                    ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                                }
                                _model.Rute = string.Join(", ", ListRute);
                            }
                        }
                    }
                    _model.Status = so.Status;
                    if (so.DataTruck != null)
                        _model.VehicleNo = so.DataTruck.VehicleNo;
                    ws.Cells[i + 2, 1].Value = _model.Status;
                    ws.Cells[i + 2, 2].Value = _model.JenisOrder;
                    ws.Cells[i + 2, 3].Value = so.SONumber;
                    ws.Cells[i + 2, 4].Value = _model.KodeNama;
                    ws.Cells[i + 2, 5].Value = _model.Customer;
                    ws.Cells[i + 2, 6].Value = _model.VehicleNo;
                    ws.Cells[i + 2, 7].Value = _model.JenisTruk;
                    ws.Cells[i + 2, 8].Value = _model.IDDriver;
                    ws.Cells[i + 2, 9].Value = _model.Driver;
                    ws.Cells[i + 2, 10].Value = _model.Rute;
                    ws.Cells[i + 2, 11].Value = "'" + _model.TanggalMuat;
                    ws.Cells[i + 2, 12].Value = "'" + _model.JamMuat;
                    ws.Cells[i + 2, 13].Value = _model.JumlahRit;
                    ws.Cells[i + 2, 14].Value = _model.KeteranganPenggatian;
                }
            }
            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Order.xls";
            return fsr;
        }

        public string BindingBOBS()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllBOBS(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.AdminUangJalan so in items)
            {
                Context.SalesOrder item = RepoSalesOrder.FindByPK(so.SalesOrderId);
                if (item == null)
                {
                    item = RepoSalesOrder.FindByAUJ(so.Id);
                }
                if (item != null)
                {
                    var _model = new AdminUangJalanIndex(item, so);
                    int IdJenisTruck;
                    string IdRute;
                    if (item.SalesOrderOncallId.HasValue)
                    {
                        RepoDHO.FindRuteTruk(item.SalesOrderOncall.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        if (IdRute != null && IdRute != "")
                        {
                            List<string> ListRute = new List<string>();
                            List<string> ListIdRute = IdRute.Split(',').ToList();
                            foreach (string idTruck in ListIdRute)
                            {
                                ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                            }
                            _model.Rute = string.Join(", ", ListRute);
                        }
                    }
                    else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        RepoDHK.FindRuteTruk(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                        {
                            if (IdRute != "")
                            {
                                List<string> ListRute = new List<string>();
                                List<string> ListIdRute = IdRute.Split(',').ToList();
                                foreach (string idTruck in ListIdRute)
                                {
                                    ListRute.Add(RepoRute.FindByPK(int.Parse(idTruck)).Nama);
                                }
                                _model.Rute = string.Join(", ", ListRute);
                            }
                        }
                    }
                    if (so.AdminUangJalanUangTf.Any(d => d.Keterangan != "Tunai" && d.Value > 0 && (d.JumlahTransfer == 0 || d.JumlahTransfer == null)))
                        _model.Status = "Transfer";
                    else if (so.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai" && d.Value > 0 && (d.JumlahTransfer == 0 || d.JumlahTransfer == null)))
                        _model.Status = "Tunai";
                    else
                        _model.Status = so.Status;
                    ListModel.Add(_model);
                }
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllBOBS(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action="read")]
        public ActionResult KasirTf()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Kasir").ToList();
            return View("IndexTf");
        }
        [MyAuthorize(Menu = "Kasir Cash", Action="read")]
        public ActionResult KasirKas()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Kasir").ToList();
            return View("IndexKas");
        }

        public string BindingTf()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllKasirTfReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<KasirTf> ListModel = new List<KasirTf>();
            foreach (Context.AdminUangJalan auj in items)
            {
                ListModel.Add(new KasirTf(auj));
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountKasirTfReport(param.Filters), data = ListModel });
        }

        public string BindingKas()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllKasirKasReport(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Kasirkas> ListModel = new List<Kasirkas>();
            foreach (Context.AdminUangJalan so in items)
            {
                    Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(so.Id);
                    ListModel.Add(new Kasirkas(auj));
            }

            List<Context.Removal> ItemsRemoval = RepoRemoval.FindAll().Where(d => d.Status == "dispatched").ToList();
            foreach (Context.Removal item in ItemsRemoval)
            {
                ListModel.Add(new Kasirkas(item));
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllAdminUangJalanReport(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action="read")]
        public ActionResult ViewTF(int id, int AdminUangJalanId)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            AdminUangJalan model = new AdminUangJalan();
            Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(AdminUangJalanId);
            ViewBag.SONumber = auj.SONumber;
            ViewBag.KeteranganAdmin = auj.KeteranganAdmin;
            if (auj != null)
                model = new AdminUangJalan(auj);
            else if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);
            model.IdSalesOrder = dbitem.Id;
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                model.ModelOncall = new SalesOrderOncall(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelOncall.Driver1Id;
                    model.NamaDriver1 = model.ModelOncall.KodeDriver1 + " - " + model.ModelOncall.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelOncall.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelOncall.Driver2Id;
                    model.NamaDriver2 = model.ModelOncall.KodeDriver2 + " - " + model.ModelOncall.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelOncall.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Transfer " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                model.ModelPickup = new SalesOrderPickup(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelPickup.Driver1Id;
                    model.NamaDriver1 = model.ModelPickup.KodeDriver1 + " - " + model.ModelPickup.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelPickup.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelPickup.Driver2Id;
                    model.NamaDriver2 = model.ModelPickup.KodeDriver2 + " - " + model.ModelPickup.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelPickup.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Transfer " + dbitem.SalesOrderPickup.SONumber;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelKonsolidasi.Driver1Id;
                    model.NamaDriver1 = model.ModelKonsolidasi.KodeDriver1 + " - " + model.ModelKonsolidasi.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelKonsolidasi.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelKonsolidasi.Driver2Id;
                    model.NamaDriver2 = model.ModelKonsolidasi.KodeDriver2 + " - " + model.ModelKonsolidasi.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelKonsolidasi.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Transfer" + dbitem.SalesOrderProsesKonsolidasi.SONumber;
            }
            else
            {
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
            }
            return View("FormTfView", model);
        }
        public ActionResult ViewTFRemoval(int id)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(id);
            RemovalAUJ model = new RemovalAUJ(dbitem);

            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                ViewBag.Title = "Report Kasir Transfer " + dbitem.SalesOrder.SalesOrderOncall.SONumber;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
            {
                ViewBag.Title = "Report Kasir Transfer " + dbitem.SalesOrder.SalesOrderPickup.SONumber;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.Title = "Report Kasir Transfer " + dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
            }
            return View("FormTfRemovalView", model);
        }
        [MyAuthorize(Menu = "Kasir Transfer", Action="read")]
        public ActionResult ViewTFKontrak(int id, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //ambil admin uang jalan nya dari listSo yang pertama
            List<int> ListIdDumy = listSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
            AdminUangJalan model = new AdminUangJalan();

            if (dbsoDummy.FirstOrDefault().IdAdminUangJalan.HasValue)
                model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            model.ModelKontrak = new SalesOrderKontrak(dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
            model.ListIdSo = listSo;
            foreach (var item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.ToList())
            {
                model.ModelKontrak.ListValueModelSOKontrak.Add(new SalesOrderKontrakListSo(item));
            }

            return View("FormTfView", model);
        }
        public ActionResult ShowPrintKas(int id, string listSo, string terbilang, int? aujId)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = new List<Context.SalesOrderKontrakListSo>();
            Context.AdminUangJalan auj = aujId == null ? null : RepoAdminUangJalan.FindByPK(aujId.Value);
            if (auj != null)
            {
                model = new AdminUangJalan(auj);
            }
            else if (dbitem.AdminUangJalanId.HasValue)
            {
                if (dbitem.SalesOrderKontrakId.HasValue)
                {
                    List<int> ListIdDumy = listSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                    dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                    if (dbsoDummy.FirstOrDefault().AdminUangJalan != null)
                        model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
                }
                else
                {
                    model = new AdminUangJalan(dbitem.AdminUangJalan);
                }
            }

            model.IdSalesOrder = dbitem.Id;
            GenerateModel(dbitem, model);

            string NoPol = "";
            var items = model.ModelListTf.Where(d => d.Nama == "Tunai").ToList();
            ViewBag.noBukti = "KK-" + dbitem.SONumber;
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrderOncall.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrderKontrakId.HasValue)
            {
                ViewBag.noBukti = "KK" + string.Join(", ", dbsoDummy.Select(d => d.NoSo));
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbsoDummy.FirstOrDefault().Driver1.NamaDriver;
                NoPol = dbsoDummy.FirstOrDefault().DataTruck.VehicleNo;
            }
            decimal? total = 0;
            if(items != null)
            {
                foreach (var item in items)
                {
                    item.Nama = "Uang Jalan " + NoPol + " " + string.Format("{0:#,00}", item.JumlahTransfer);
                    total = total + item.JumlahTransfer;
                }
            }
            ViewBag.item = items;
            ViewBag.total = total;
            ViewBag.terbilang = terbilang;
            return View("PrintKas");
        }
        public ActionResult ShowPrint(int id)
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(id);
            DokumenIndex model = new DokumenIndex(dbitem);

            return View("Print", model);
        }
        public ActionResult ShowPrintKasRemoval(int id)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(id);
            RemovalAUJ model = new RemovalAUJ(dbitem);

            string NoPol = "";
            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                ViewBag.noBukti = "[Belum ada format nya]";
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
            {
                ViewBag.noBukti = "[Belum ada format nya]";
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.noBukti = "[Belum ada format nya]";
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
            }
            var items = model.ModelListTf.Where(d => d.Nama == "Tunai").ToList();
            decimal? total = 0;
            if (items != null)
            {
                foreach (var item in items)
                {
                    item.Nama = NoPol + " ( TOE [Format apa] - " + string.Format("{0:#,00}", item.JumlahTransfer) + ") ";
                    total = total + item.JumlahTransfer;
                }
            }
            ViewBag.item = items;
            ViewBag.total = total;
            return View("PrintKas");
        }

        public void GenerateModel(Context.SalesOrder dbitem, AdminUangJalan model)
        {
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                model.ModelOncall = new SalesOrderOncall(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelOncall.Driver1Id;
                    model.NamaDriver1 = model.ModelOncall.KodeDriver1 + " - " + model.ModelOncall.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelOncall.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelOncall.Driver2Id;
                    model.NamaDriver2 = model.ModelOncall.KodeDriver2 + " - " + model.ModelOncall.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelOncall.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Kas " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                model.ModelPickup = new SalesOrderPickup(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelPickup.Driver1Id;
                    model.NamaDriver1 = model.ModelPickup.KodeDriver1 + " - " + model.ModelPickup.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelPickup.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelPickup.Driver2Id;
                    model.NamaDriver2 = model.ModelPickup.KodeDriver2 + " - " + model.ModelPickup.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelPickup.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Kas " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelKonsolidasi.Driver1Id;
                    model.NamaDriver1 = model.ModelKonsolidasi.KodeDriver1 + " - " + model.ModelKonsolidasi.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelKonsolidasi.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelKonsolidasi.Driver2Id;
                    model.NamaDriver2 = model.ModelKonsolidasi.KodeDriver2 + " - " + model.ModelKonsolidasi.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelKonsolidasi.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Kas " + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
            }
        }

        [MyAuthorize(Menu = "Kasir Cash", Action="read")]
        public ActionResult ViewKas(int id, int? AdminUangJalanId)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            Context.AdminUangJalan auj = null;
            if (AdminUangJalanId == null){
                if (dbitem.AdminUangJalanId.HasValue)
                    model = new AdminUangJalan(dbitem.AdminUangJalan);
                auj = RepoAdminUangJalan.FindByPK(dbitem.AdminUangJalanId.Value);
                model.IdSalesOrder = dbitem.Id;
            }
            else {
                auj = RepoAdminUangJalan.FindByPK(AdminUangJalanId.Value);
                dbitem = RepoSalesOrder.FindByPK(auj.SalesOrderId);
                model = new AdminUangJalan(auj);
            }
            ViewBag.SONumber = auj.SONumber;
            ViewBag.KeteranganAdmin = auj.KeteranganAdmin;

            if (dbitem != null && dbitem.SalesOrderOncallId.HasValue)
            {
                model.ModelOncall = new SalesOrderOncall(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelOncall.Driver1Id;
                    model.NamaDriver1 = model.ModelOncall.KodeDriver1 + " - " + model.ModelOncall.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelOncall.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelOncall.Driver2Id;
                    model.NamaDriver2 = model.ModelOncall.KodeDriver2 + " - " + model.ModelOncall.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelOncall.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Cash " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
                return View("FormKasView", model);
            }
            else if (dbitem != null && dbitem.SalesOrderPickupId.HasValue)
            {
                model.ModelPickup = new SalesOrderPickup(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelPickup.Driver1Id;
                    model.NamaDriver1 = model.ModelPickup.KodeDriver1 + " - " + model.ModelPickup.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelPickup.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelPickup.Driver2Id;
                    model.NamaDriver2 = model.ModelPickup.KodeDriver2 + " - " + model.ModelPickup.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelPickup.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Cash " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormKasView", model);
            }
            else if (dbitem != null && dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelKonsolidasi.Driver1Id;
                    model.NamaDriver1 = model.ModelKonsolidasi.KodeDriver1 + " - " + model.ModelKonsolidasi.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelKonsolidasi.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelKonsolidasi.Driver2Id;
                    model.NamaDriver2 = model.ModelKonsolidasi.KodeDriver2 + " - " + model.ModelKonsolidasi.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelKonsolidasi.KeteranganDriver2;
                }
                ViewBag.Title = "Report Kasir Cash " + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormKasView", model);
            }
            else
            {
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindAll().Where(d => auj.SONumber.Contains(d.NoSo)).FirstOrDefault();
                dbitem = RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value);
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
            }
            return View("FormKasView", model);
        }

        [MyAuthorize(Menu = "Kasir Cash", Action = "update")]
        public ActionResult ViewKasKontrak(int id, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            //ambil admin uang jalan nya dari listSo yang pertama
            List<int> ListIdDumy = listSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
            AdminUangJalan model = new AdminUangJalan();
            if (dbsoDummy.FirstOrDefault().AdminUangJalan != null)
                model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            model.ModelKontrak = new SalesOrderKontrak(dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
            model.ListIdSo = listSo;
            foreach (var item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.ToList())
            {
                model.ModelKontrak.ListValueModelSOKontrak.Add(new SalesOrderKontrakListSo(item));
            }
            GenerateModel(dbitem, model);

            return View("ViewKasKontrak", model);
        }
        public ActionResult ViewSuratJalan(int id, string caller)
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(id);
            Dokumen model = new Dokumen(dbitem);
            ViewBag.caller = caller;
            if (caller == "admin")
                ViewBag.Title = "Report Dokumen Admin Surat Jalan";
            else
                ViewBag.Title = "Report Dokumen Billing";

            return View("ViewSuratJalan", model);
        }

        [MyAuthorize(Menu = "Admin Uang Jalan", Action="read")]
        public ActionResult View(int id, int AdminUangJalanId)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            Context.AdminUangJalan dbauj = RepoAdminUangJalan.FindByPK(AdminUangJalanId);
            if (dbauj.Driver1 != null)
            {
                ViewBag.DriverName = dbauj.Driver1.NamaDriver;
                ViewBag.KodeDriverOld = dbauj.Driver1.KodeDriverOld;
                ViewBag.KodeDriver = dbauj.Driver1.KodeDriver;
            }
            AdminUangJalan model = new AdminUangJalan(dbauj);
            if (dbauj.DataTruck != null)
                ViewBag.VehicleNo = dbauj.DataTruck.VehicleNo;
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                model.ModelOncall = new SalesOrderOncall(dbitem);
                if (model.IdDriver1.HasValue)
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                else
                    model.KeteranganGanti1 = model.ModelOncall.KeteranganDriver1;
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelOncall.Driver2Id;
                    model.NamaDriver2 = model.ModelOncall.KodeDriver2 + " - " + model.ModelOncall.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelOncall.KeteranganDriver2;
                }
                ViewBag.Title = "Report Admin Uang Jalan " + dbitem.SalesOrderOncall.SONumber;
                return View("View", model);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                model.ModelPickup = new SalesOrderPickup(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelPickup.Driver1Id;
                    model.NamaDriver1 = model.ModelPickup.KodeDriver1 + " - " + model.ModelPickup.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelPickup.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelPickup.Driver2Id;
                    model.NamaDriver2 = model.ModelPickup.KodeDriver2 + " - " + model.ModelPickup.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelPickup.KeteranganDriver2;
                }
                ViewBag.Title = "Report Admin Uang Jalan " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("View", model);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                return View("View", model);
            }

            return View("");
        }
        [MyAuthorize(Menu = "Admin Uang Jalan", Action="read")]
        public ActionResult ViewKontrak(int aujId)
        {
            //ambil admin uang jalan nya dari listSo yang pertama
            Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(aujId);
            List<String> ListNoSoDumy = auj.SONumber.Split(new string[] { "," }, StringSplitOptions.None).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = RepoSalesOrderKontrakListSo.FindAllKontrak().Where(d => ListNoSoDumy.Contains(d.NoSo)).ToList();
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(dbsoDummy.FirstOrDefault().SalesKontrakId.Value);
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
            AdminUangJalan model = new AdminUangJalan(auj);
            ViewBag.Id = auj.Id;
            ViewBag.NoSo = auj.SONumber;

            model.IdSalesOrder = dbitem.Id;
            GenerateModelAUJ(model, dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = model.ModelKontrak.ListModelSOKontrak;

            return View("ViewKontrak", model);
        }

        public void GenerateModelAUJ(AdminUangJalan model, Context.SalesOrder dbitem)
        {
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                model.ModelOncall = new SalesOrderOncall(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelOncall.Driver1Id;
                    model.NamaDriver1 = model.ModelOncall.KodeDriver1 + " - " + model.ModelOncall.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelOncall.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    if (model.ModelOncall.Driver2Id.HasValue)
                    {
                        model.IdDriver2 = model.ModelOncall.Driver2Id;
                        model.NamaDriver2 = model.ModelOncall.KodeDriver2 + " - " + model.ModelOncall.NamaDriver2;
                        model.KeteranganGanti2 = model.ModelOncall.KeteranganDriver2;
                    }
                }
                ViewBag.Title = "Report Admin Uang Jalan " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
                //return View("Form", model);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                model.ModelPickup = new SalesOrderPickup(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelPickup.Driver1Id;
                    model.NamaDriver1 = model.ModelPickup.KodeDriver1 + " - " + model.ModelPickup.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelPickup.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelPickup.Driver2Id;
                    model.NamaDriver2 = model.ModelPickup.KodeDriver2 + " - " + model.ModelPickup.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelPickup.KeteranganDriver2;
                }
                ViewBag.Title = "Report Admin Uang Jalan " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                //return View("Form", model);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelKonsolidasi.Driver1Id;
                    model.NamaDriver1 = model.ModelKonsolidasi.KodeDriver1 + " - " + model.ModelKonsolidasi.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelKonsolidasi.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelKonsolidasi.Driver2Id;
                    model.NamaDriver2 = model.ModelKonsolidasi.KodeDriver2 + " - " + model.ModelKonsolidasi.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelKonsolidasi.KeteranganDriver2;
                }
                ViewBag.Title = "Report Admin Uang Jalan " + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
                //return View("Form", model);
            }
            else if (dbitem.SalesOrderKontrakId.HasValue)
            {
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
                Context.SalesOrderKontrakListSo dummySo = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.FirstOrDefault();
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = dummySo.Driver1Id;
                    model.NamaDriver1 = dummySo.Driver1.KodeDriver + " - " + dummySo.Driver1.NamaDriver;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    if (dummySo.Driver2Id.HasValue)
                    {
                        model.IdDriver2 = dummySo.Driver2Id;
                        model.NamaDriver2 = dummySo.Driver2.KodeDriver + " - " + dummySo.Driver2.NamaDriver;
                    }
                }
                ViewBag.Title = "Report Admin Uang Jalan " + dbitem.SalesOrderKontrak.SONumber;
            }
        }

        private string GenerateDokumen(int IdSo, Context.Customer dbcust, string ListIdSo = "", string rute = "")
        {
            Context.Dokumen dbdokumen = new Context.Dokumen();
            //ambil dokumen dari customer
            dbdokumen.IdSO = IdSo;
            dbdokumen.ListIdSo = ListIdSo;
            dbdokumen.IsAdmin = true;
            dbdokumen.ModifiedDate = DateTime.Now;
            dbdokumen.IdCustomer = dbcust.Id;
            dbdokumen.RuteSo = rute;
            string strQuery = "INSERT INTO dbo.\"Dokumen\" (\"IdSO\", \"ListIdSo\", \"IsComplete\", \"IsAdmin\", \"ModifiedDate\", \"IdCustomer\", \"IsReturn\", \"RuteSo\") VALUES (" + dbdokumen.IdSO + ", " + dbdokumen.ListIdSo +
                ", " + dbdokumen.IsComplete + ", " + dbdokumen.IsAdmin + ", " + dbdokumen.ModifiedDate + ", " + dbdokumen.IdCustomer + ", " + dbdokumen.IsReturn + ", " + dbdokumen.RuteSo + ");";
            foreach (var item in dbcust.CustomerBilling)
            {
                dbdokumen.DokumenItem.Add(new Context.DokumenItem()
                {
                    IdBilling = item.Id,
                    CustomerId = item.CustomerId,
                    ModifiedDate = DateTime.Now,
                });
                strQuery += "INSERT INTO dbo.\"DokumenItem\" (\"IdBilling\", \"CustomerId\", \"ModifiedDate\") VALUES (" + item.Id + ", " + item.CustomerId + ", " + DateTime.Now + ");";
            }
            RepoDokumen.save(dbdokumen, UserPrincipal.id);
            return strQuery;
        }

        public ActionResult AddSB(int Id)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(Id);
            Context.AdminUangJalan dummyAdminUangJalan = dbso.AdminUangJalan;
            //batalkeun so na
            Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
            //settlement batal
            dbsettlement.IdDriver = dummyAdminUangJalan.IdDriver1;
            dbsettlement.IdSalesOrder = dbso.Id;
            if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai"))
            {
                dbsettlement.KasDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault().JumlahTransfer;
            }
            if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer")))
            {
                dbsettlement.TransferDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer")).Sum(t => t.JumlahTransfer);
            }
            dbsettlement.SolarDiterima = dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(s => s.Value);
            dbsettlement.KapalDiterima = dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(s => s.Value);
            dbsettlement.JenisBatal = "Batal Order";
            RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Order");
            return RedirectToAction("Order", "Report");
        }
    }
}