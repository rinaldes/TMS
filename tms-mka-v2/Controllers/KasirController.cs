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
    public class KasirController : BaseController
    {
        private IBatalOrderRepo RepoBatalOrder;
        private ISalesOrderRepo RepoSalesOrder;
        private IAtmRepo Repoatm;
        private ISettlementRegRepo RepoSettlementReg;
        private IDataBoronganRepo RepoBor;
        private IRemovalRepo RepoRemoval;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private IAdminUangJalanRepo RepoAdminUangJalan;
        private IAuditrailRepo RepoAuditrail;
        private IDokumenRepo RepoDokumen;
        private IMasterPoolRepo RepoMasterPool;
        private ILookupCodeRepo RepoLookupCode;
        private ICustomerRepo RepoCustomer;
        private Igr_mstrRepo Repogr_mstr;
        private ISettlementBatalRepo RepoSettBatal;
        private IDaftarHargaOnCallRepo RepoDHO;
        private IRuteRepo RepoRute;
        private IDaftarHargaKonsolidasiRepo RepoDHK;
        public KasirController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder,
            IAtmRepo repoatm, IDataBoronganRepo repoBor, IRemovalRepo repoRemoval,
            ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig,
            Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr, IAdminUangJalanRepo repoAdminUangJalan,
            IAuditrailRepo repoAuditrail, IDokumenRepo repoDokumen, IMasterPoolRepo repoMasterPool,
            ILookupCodeRepo repoLookupCode, ICustomerRepo repoCustomer, Igr_mstrRepo repogr_mstr,
            ISettlementRegRepo repoSettlementReg, IBatalOrderRepo repoBatalOrder, ISettlementBatalRepo repoSettBatal,
            IDaftarHargaOnCallRepo repoDHO, IDaftarHargaKonsolidasiRepo repoDHK, IRuteRepo repoRute)
            : base(repoBase, repoLookup)
        {
            RepoDHO = repoDHO;
            RepoBatalOrder = repoBatalOrder;
            RepoSettBatal = repoSettBatal;
            RepoDokumen = repoDokumen;
            RepoLookup = repoLookup;
            RepoSalesOrder = repoSalesOrder;
            Repoatm = repoatm;
            RepoBor = repoBor;
            RepoRemoval = repoRemoval;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            RepoAdminUangJalan = repoAdminUangJalan;
            RepoAuditrail = repoAuditrail;
            RepoMasterPool = repoMasterPool;
            RepoLookupCode = repoLookupCode;
            RepoCustomer = repoCustomer;
            RepoSettlementReg = repoSettlementReg;
            Repogr_mstr = repogr_mstr;
            RepoDHK = repoDHK;
            RepoRute = repoRute;
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
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllTfBOBS(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
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

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllTfBOBS(param.Filters), data = ListModel });
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
            List<Context.AdminUangJalan> items = RepoSalesOrder.FindAllKasBOBS(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
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

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountAllKasBOBS(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action = "read")]
        public ActionResult IndexTf()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Kasir").ToList();
            return View("IndexTf");
        }
        [MyAuthorize(Menu = "Kasir Cash", Action = "read")]
        public ActionResult IndexKas()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Kasir").ToList();
            return View("IndexKas");
        }
        public bool Checking(int id)
        {
            Context.SalesOrder so = RepoSalesOrder.FindByPK(id);
            if (so.SalesOrderKontrakId == null)
                return (so.Status.ToLower() == "admin uang jalan" || so.Status.ToLower() == "dispatched");
            else
                return true;
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

        public string BindingTf()
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKasir().Where(d => d.OrderTanggalMuat > date2018 || d.OrderTanggalMuat == null).ToList();
            List<KasirTf> ListModel = new List<KasirTf>();
            List<Context.SettlementReguler> ItemsSettlement = RepoSettlementReg.FindAll().Where(d => d.TotalTf > 0 && d.TfExecuted != true).ToList();
            foreach (Context.SalesOrder item in items)
            {
                ListModel.Add(new KasirTf(item));
            }
            var data = RepoSalesOrderKontrakListSo.FindAllDispatched().GroupBy(d => new { d.IdDataTruck, d.Driver1Id, d.Status, d.Urutan, d.IdAdminUangJalan }).Select(grp => grp.ToList());
            foreach (var itemGroup in data.ToList())
            {
                Context.SalesOrder item = RepoSalesOrder.FindByKontrak(itemGroup.FirstOrDefault().SalesKontrakId.Value);
                ListModel.Add(new KasirTf(item, itemGroup));
            }

            List<Context.Removal> ItemsRemoval = RepoRemoval.FindAll().Where(d => d.Status == "dispatched" || d.Status == "admin uang jalan").ToList();
            foreach (Context.Removal item in ItemsRemoval)
            {
                ListModel.Add(new KasirTf(item));
            }
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

        public string BindingKas()
        {
            List<Kasirkas> ListModel = new List<Kasirkas>();
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKasirKas();
            foreach (Context.SalesOrder item in items)
            {
                ListModel.Add(new Kasirkas(item));
            }
            var data = RepoSalesOrderKontrakListSo.FindAllKasDispatched().GroupBy(d => new { d.IdDataTruck, d.Driver1Id, d.Status, d.Urutan }).Select(grp => grp.ToList());
            foreach (var itemGroup in data.ToList())
            {
                Context.SalesOrder item = RepoSalesOrder.FindByKontrak(itemGroup.FirstOrDefault().SalesKontrakId.Value);
                ListModel.Add(new Kasirkas(item, itemGroup));
            }
            List<Context.Removal> ItemsRemoval = RepoRemoval.FindAll().Where(d => d.Status == "dispatched" || d.Status == "admin uang jalan").ToList();
            foreach (Context.Removal item in ItemsRemoval)
            {
                ListModel.Add(new Kasirkas(item));
            }
            List<Context.SettlementReguler> ItemsSettlement = RepoSettlementReg.FindAll().Where(d => d.TotalCash > 0 && d.CashExecuted != true).ToList();
            foreach (Context.SettlementReguler item in ItemsSettlement)
            {
                Kasirkas settlement = new Kasirkas(item.SalesOrder, item);
                settlement.Status = "Settlement";
                settlement.Jumlah = item.TotalCash;
                settlement.Realisasi = 0;
                settlement.IdSettlement = item.Id;
                settlement.JadwalTransfer = item.TanggalTf == null ? "" : item.TanggalTf.Value.ToString("dd-MM-yyyy");
                ListModel.Add(settlement);
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action = "update")]
        public ActionResult EditTF(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            @ViewBag.KodeDriverOld = dbitem.Driver.KodeDriverOld;
            ViewBag.KeteranganAdmin = model.KeteranganAdmin;
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
                ViewBag.driverId = dbitem.SalesOrderOncall.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderOncall.Driver1.KodeDriver + " - " + dbitem.SalesOrderOncall.Driver1.NamaDriver;
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrderOncall.SONumber;
                ViewBag.KeteranganAdmin = model.KeteranganAdmin;
                //ViewBag.postData = "EditOncall";
                return View("FormTf", model);
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
                ViewBag.driverId = dbitem.SalesOrderPickup.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderPickup.Driver1.KodeDriver + " - " + dbitem.SalesOrderPickup.Driver1.NamaDriver;
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormTf", model);
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
                ViewBag.driverId = dbitem.SalesOrderProsesKonsolidasi.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver + " - " + dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                ViewBag.Title = "Kasir Transfer" + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormTf", model);
            }
            else if (dbitem.SalesOrderKontrakId.HasValue)
            {

            }

            return View();
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action = "update")]
        public ActionResult EditTFBatal(int id, int AdminUangJalanId)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(AdminUangJalanId);
            AdminUangJalan model = new AdminUangJalan(auj);
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
                ViewBag.driverId = auj.IdDriver1;
                ViewBag.driverName = auj.Driver1.KodeDriver + " - " + auj.Driver1.NamaDriver;
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrderOncall.SONumber;
                ViewBag.KeteranganAdmin = model.KeteranganAdmin;
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
                ViewBag.driverId = dbitem.SalesOrderPickup.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderPickup.Driver1.KodeDriver + " - " + dbitem.SalesOrderPickup.Driver1.NamaDriver;
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrderPickup.SONumber;
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
                ViewBag.driverId = dbitem.SalesOrderProsesKonsolidasi.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver + " - " + dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                ViewBag.Title = "Kasir Transfer" + dbitem.SalesOrderProsesKonsolidasi.SONumber;
            }
            ViewBag.AdminUangJalanId = AdminUangJalanId;
            return View("FormTfBatal", model);
        }

        public ActionResult EditTFRemoval(int id)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(id);
            RemovalAUJ model = new RemovalAUJ(dbitem);

            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrder.SalesOrderOncall.SONumber;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
            {
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrder.SalesOrderPickup.SONumber;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
            }
            return View("FormTfRemoval", model);
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

        public ActionResult EditKasSettlement(int id)
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

            return View("FormKasSettlement", model);
        }

        [HttpPost]
        public ActionResult PostEditTfSettlement(int Id, int _IdCreditTf)
        {
            Context.ERPConfig erpConf = RepoERPConfig.FindByFrist();
            Context.SettlementReguler dbitem = RepoSettlementReg.FindByPK(Id);
            dbitem.TfExecuted = true;
            RepoSettlementReg.save(dbitem, UserPrincipal.id);
            Context.SalesOrderKontrakListSo sokls = null;
            if (dbitem.LisSoKontrak != null)
                sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.LisSoKontrak.Split('.')[0]));
            string soNumber = "KT-SR-" + dbitem.AdminUangJalan.SONumber;
            Repoglt_det.saveFromAc(1, soNumber, 0, dbitem.TotalTf.Value, Repoac_mstr.FindByPk(_IdCreditTf), dbitem.SalesOrder, null, sokls);//BCA Audy 386-7957777 atau
            Repoglt_det.saveFromAc(2, soNumber, dbitem.TotalTf.Value, 0, Repoac_mstr.FindByPk(erpConf.IdAUJCredit), dbitem.SalesOrder, null, sokls);//BCA Audy 386-7957777 atau
            return RedirectToAction("IndexTf");
        }

        [HttpPost]
        public ActionResult PostEditKasSettlement(int Id)
        {
            Context.ERPConfig erpConf = RepoERPConfig.FindByFrist();
            Context.SettlementReguler dbitem = RepoSettlementReg.FindByPK(Id);
            dbitem.CashExecuted = true;
            RepoSettlementReg.save(dbitem, UserPrincipal.id);
            string soNumber = "KK-SR-" + dbitem.AdminUangJalan.SONumber;
            Context.SalesOrderKontrakListSo sokls = null;
            if (dbitem.LisSoKontrak != null)
                sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.LisSoKontrak.Split('.')[0]));
            Repoglt_det.saveFromAc(1, soNumber, 0, dbitem.TotalCash.Value, Repoac_mstr.FindByPk(RepoMasterPool.FindByIPAddress().IdCreditCash), dbitem.SalesOrder, null, sokls);//BCA Audy 386-7957777 atau
            Repoglt_det.saveFromAc(2, soNumber, dbitem.TotalCash.Value, 0, Repoac_mstr.FindByPk(erpConf.IdBiayaPerjalanan), dbitem.SalesOrder, null, sokls);//BCA Audy 386-7957777 atau
            return RedirectToAction("IndexTf");
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action = "update")]
        public ActionResult EditTfKontrak(int id, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //ambil admin uang jalan nya dari listSo yang pertama
            List<int> ListIdDumy = listSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
            AdminUangJalan model = new AdminUangJalan();

            if (dbsoDummy.FirstOrDefault().IdAdminUangJalan.HasValue)
                model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
            ViewBag.KeteranganAdmin = model.KeteranganAdmin;
            ViewBag.KodeDriverOld = dbsoDummy.FirstOrDefault().Driver1.KodeDriverOld;

            model.IdSalesOrder = dbitem.Id;
            model.ModelKontrak = new SalesOrderKontrak(dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
            model.ListIdSo = listSo;
            foreach (var item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.ToList())
            {
                model.ModelKontrak.ListValueModelSOKontrak.Add(new SalesOrderKontrakListSo(item));
            }

            return View("FormTf", model);
        }
        
        public string NyusulinPendapatan(string SONumber)
        {
            Context.SalesOrderKonsolidasi sok = RepoSalesOrder.FindByCode(SONumber).SalesOrderKonsolidasi;
            Context.SalesOrder dbitem = RepoSalesOrder.FindByCode(sok.IKSNo);
            var item = dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == sok.SalesOrderKonsolidasiId).FirstOrDefault();
            decimal harga = RepoSalesOrder.HargaKonsolidasiPerItem(item);
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            Context.Customer supplier = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.NamaTagihanId.Value);
            string customer = supplier.CustomerCodeOld + "-" + supplier.CustomerNama;
            Repoglt_det.saveFromAc(1, "PD-" + dbitem.SONumber + ", " + sok.SONumber, harga, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem, "Piutang Belum Ditagih - " + customer);
            Repoglt_det.saveFromAc(2, "PD-" + dbitem.SONumber + ", " + sok.SONumber, 0, harga, Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem, "Pendapatan Belum Ditagih - " + customer);
            return "HPD-" + dbitem.SONumber + ", " + sok.SONumber + ", " + harga;
        }

        [HttpPost]
        public ActionResult EditTf(AdminUangJalan model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            if (RepoAuditrail.getKasirHistoryUnder1Minutes(dbitem, DateTime.Now.AddMinutes(-1)) == null && RepoAuditrail.getHistoryUnderSpecificUser(dbitem, DateTime.Now.AddMinutes(-3), UserPrincipal.id) == null)
            {
                Context.AdminUangJalan db = dbitem.AdminUangJalan;
                Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                string codeBT = "BT-" + (dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderPickup.SONumber);
                int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
                AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
                model.ModelListTf = resUang.ToList();
                string soNumber = "";
                int idx = 1;
                soNumber = dbitem.SONumber;
                if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    foreach (var item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                    {
                        decimal harga = RepoSalesOrder.HargaKonsolidasiPerItem(item);
                        Context.Customer supplier = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.NamaTagihanId.Value);
                        string customer = supplier.CustomerCodeOld + "-" + supplier.CustomerNama;
                        Context.SalesOrder sok = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                        if (!sok.PendapatanDiakui)
                        {
                            sok.SalesOrderKonsolidasi.ProcessedByKasir = true;
                            sok.PendapatanDiakui = true;//END beruhubungan dgn utang customer pendapatan hanya sampai sini, hanya sekali per SO
                            Repoglt_det.saveFromAc(1, "PD-" + dbitem.SONumber + ", " + sok.SONumber, harga, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem, "Piutang Belum Ditagih - " + customer, null, null, supplier.Id);
                            Repoglt_det.saveFromAc(2, "PD-" + dbitem.SONumber + ", " + sok.SONumber, 0, harga, Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem, "Pendapatan Belum Ditagih - " + customer, null, null, supplier.Id);
                        }
                    }
                }
                else if (dbitem.PendapatanDiakui != true)
                {//START beruhubungan dgn utang piutang pendapatan hanya sampai sini, hanya sekali per SO
                    Repoglt_det.saveFromAc(1, "PD-" + soNumber, RepoSalesOrder.Harga(dbitem), 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem);
                    Repoglt_det.saveFromAc(2, "PD-" + soNumber, 0, RepoSalesOrder.Harga(dbitem), Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem);
                    dbitem.PendapatanDiakui = true;//END beruhubungan dgn utang customer pendapatan hanya sampai sini, hanya sekali per SO
                }
                string code = "KT-" + soNumber + "-" + urutanBatal;
                if (db.JurnalVoucher != true)
                {//START berhubungan dengan biaya jalan, sekali per AUJ, klo batal truk bisa berkali2
                    decimal nominalHutangUangJalanDriver = 0;
                    foreach (var item in model.ModelListTf)
                    {
                        if (item.isTf != true)
                            nominalHutangUangJalanDriver += item.JumlahTransfer.Value;
                    }
                    nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                    nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                    nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                    nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                    nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());

                    Repoglt_det.saveFromAc(1, code, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);//Hutang Uang Jalan Driver
                    foreach (var item in model.ModelListTf)
                    {
                        if (item.isTf != true && item.TanggalAktual != null && item.JamAktual != null)
                        {
                            idx++;
                            Repoglt_det.saveFromAc(idx, code, 0, item.JumlahTransfer.Value, Repoac_mstr.FindByPk(item.IdCreditTf), dbitem, DateTime.Now.ToString("ddMMyy-")+item.JamAktual.Value.ToString(), null, item.TanggalAktual.Value.AddDays(1));
                        }
                    }
                    string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                    foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu.Where(d => d.Value > 0))
                    {
                        Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                        if (spbu != null && aujvs.Value > 0)
                        {
                            idx++;
                            Context.Customer customer = RepoCustomer.FindByPK(spbu.VendorId.Value);
                            Repoglt_det.saveFromAc(idx, code, 0, aujvs.Value, Repoac_mstr.FindByPk(spbu.ac_id), dbitem, aujvs.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                            idx++;
                            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                            con.Open();
                            using (DataTable dt = new DataTable())
                            {
                                var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbitem, decimal.Parse(aujvs.Value.ToString()));
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
                        Repoglt_det.saveFromAc(idx, code, 0, aujvk.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvk.Keterangan).ac_id), dbitem, aujvk.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                        idx++;
                        NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                        con.Open();
                        using (DataTable dt = new DataTable())
                        {
                            var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbitem, decimal.Parse(aujvk.Value.ToString()));
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
                        Repoglt_det.saveFromAc(idx, code, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                    }
                    if (db.PotonganP > 0)
                    {//Pribadi
                        idx++;
                        Repoglt_det.saveFromAc(idx, code, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                    }
                    if (db.PotonganK > 0)
                    {//Klaim
                        idx++;
                        Repoglt_det.saveFromAc(idx, code, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem);
                    }
                    if (db.PotonganT > 0)
                    {//Tabungan
                        idx++;
                        Repoglt_det.saveFromAc(idx, code, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem);
                    }
                    db.JurnalVoucher = true;
                }
                else
                {
                    foreach (var item in model.ModelListTf)
                    {
                        if (item.JumlahTransfer.Value > 0)
                        {
                            if (item.isTf != true)
                            {
                                Repoglt_det.saveFromAc(idx, code, 0, item.JumlahTransfer.Value, Repoac_mstr.FindByPk(item.IdCreditTf), dbitem, DateTime.Now.ToString("ddMMyy-") + item.JamAktual.Value.ToString(), null, item.TanggalAktual.Value.AddDays(1));
                                idx++;
                                Repoglt_det.saveFromAc(idx, code, item.JumlahTransfer.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);//Hutang Uang Jalan Driver
                            }
                        }
                    }
                }
                foreach (var item in model.ModelListTf)
                {
                    if (item.JumlahTransfer.Value > 0)
                    {
                        item.isTf = true;
                    }
                    else
                        item.isTf = false;
                    item.setDbKasirKontrak(dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(d => d.Id == item.Id).FirstOrDefault());
                }
                if (dbitem.Status.ToLower().Contains("dispatched") || dbitem.Status.ToLower().Contains("admin uang jalan"))
                    dbitem.Status = "dispatched";
                dbitem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbitem);
                string strQuery = "";
                if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    foreach (var item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                    {
                        Context.Customer tagihan = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.NamaTagihanId.Value);
                        Context.SalesOrder so = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                        if (RepoDokumen.FindBySO(so.Id) == null)
                        {
                            strQuery += GenerateDokumen(so.Id, tagihan, "", item.SalesOrderKonsolidasi.StrDaftarHargaItem);
                        }
                    }
                }
                else if (RepoDokumen.FindBySO(dbitem.Id) == null)
                {
                    if (dbitem.SalesOrderOncallId.HasValue)
                        strQuery += GenerateDokumen(dbitem.Id, dbitem.SalesOrderOncall.Customer);
                    else if (dbitem.SalesOrderPickupId.HasValue)
                        strQuery += GenerateDokumen(dbitem.Id, dbitem.SalesOrderPickup.Customer);
                }

                RepoAuditrail.saveKasirTfHistory(dbitem, strQuery);
                return RedirectToAction("IndexTf");
            }
            else
            {
                ViewBag.ErrorMessage = "Kasir Transfer Keklik 2 kali ";
            }
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);

            ViewBag.Status = "batal";
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
                ViewBag.Title = "Kasir Transfer Keklik 2 kali " + dbitem.SalesOrderOncall.SONumber;
                return View("FormTf", model);
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
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormTf", model);
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
                ViewBag.Title = "Kasir Transfer" + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormTf", model);
            }
            return View("FormTf", model);
        }

        [HttpPost]
        public ActionResult EditTfBatal(AdminUangJalan model, int AdminUangJalanId)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.AdminUangJalan db = RepoAdminUangJalan.FindByPK(AdminUangJalanId);
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
            model.ModelListTf = resUang.ToList();
            int idx = 1;
            string soNumber = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.SONumber : dbitem.SalesOrderPickup.SONumber;
            if (db.JurnalVoucher != true)
            {//START berhubungan dengan biaya jalan, sekali per AUJ, klo batal truk bisa berkali2
                decimal nominalHutangUangJalanDriver = 0;
                foreach (var item in model.ModelListTf)
                {
                    if (item.isTf != true)
                        nominalHutangUangJalanDriver += item.JumlahTransfer.Value;
                }
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());

                Repoglt_det.saveFromAc(1, "KT-" + soNumber, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);//Hutang Uang Jalan Driver
                foreach (var item in model.ModelListTf)
                {
                    if (item.isTf != true)
                    {
                        idx++;
                        Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, item.JumlahTransfer.Value, Repoac_mstr.FindByPk(item.IdCreditTf), dbitem, DateTime.Now.ToString("ddMMyy-"), null, item.TanggalAktual.Value.AddDays(1));//BCA Audy 386-7957777 atau
                    }
                }
                string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                {
                    Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                    if (spbu != null && aujvs.Value > 0)
                    {
                        idx++;
                        Context.Customer customer = RepoCustomer.FindByPK(spbu.VendorId.Value);
                        Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, aujvs.Value, Repoac_mstr.FindByPk(spbu.ac_id), dbitem, aujvs.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                        idx++;
                        NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                        con.Open();
                        using (DataTable dt = new DataTable())
                        {
                            var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbitem, decimal.Parse(aujvs.Value.ToString()));
                            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                            da.Fill(dt);
                            cmd.Dispose();
                            con.Close();
                        }
                    }
                }
                foreach (Context.AdminUangJalanVoucherKapal aujvk in db.AdminUangJalanVoucherKapal)
                {
                    Context.LookupCode kapal = RepoLookup.FindByName(aujvk.Keterangan);
                    idx++;
                    Context.Customer customer = RepoCustomer.FindByPK(kapal.VendorId.Value);
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, aujvk.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvk.Keterangan).ac_id), dbitem, aujvk.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                    idx++;
                    NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                    con.Open();
                    using (DataTable dt = new DataTable())
                    {
                        var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbitem, decimal.Parse(aujvk.Value.ToString()));
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
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganP > 0)
                {//Pribadi
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganK > 0)
                {//Klaim
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem);
                }
                if (db.PotonganT > 0)
                {//Tabungan
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem);
                }
                db.JurnalVoucher = true;
            }
            else
            {
                foreach (var item in model.ModelListTf)
                {
                    if (item.JumlahTransfer.Value > 0)
                    {
                        if (item.isTf != true)
                        {
                            Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, item.JumlahTransfer.Value, Repoac_mstr.FindByPk(item.IdCreditTf), dbitem, DateTime.Now.ToString("ddMMyy-") + item.JamAktual.Value.ToString());//BCA Audy 386-7957777 atau
                            idx++;
                            Repoglt_det.saveFromAc(idx, "KT-" + soNumber, item.JumlahTransfer.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);//Hutang Uang Jalan Driver
                        }
                    }
                }
            }
            foreach (var item in model.ModelListTf)
            {
                if (item.JumlahTransfer.Value > 0)
                {
                    item.isTf = true;
                }
                else
                    item.isTf = false;
                item.setDbKasirKontrak(db.AdminUangJalanUangTf.Where(d => d.Id == item.Id).FirstOrDefault());
            }

            Context.AdminUangJalan dummyAdminUangJalan = db;
            Context.SettlementBatal dbsettlement = RepoSettBatal.FindByAUJ(dummyAdminUangJalan.Id);
            if (dbsettlement == null)
                dbsettlement = new Context.SettlementBatal();
            //ambil data 
            dbsettlement.IdSalesOrder = model.IdSalesOrder;
            if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai"))
            {
                dbsettlement.KasDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault().JumlahTransfer;
            }
            if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer")))
            {
                dbsettlement.TransferDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer")).Sum(t => t.JumlahTransfer);
            }
            dbsettlement.IdAdminUangJalan = dummyAdminUangJalan.Id;
            dbsettlement.SolarDiterima = dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(s => s.Value);
            dbsettlement.KapalDiterima = dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(s => s.Value);
            dbsettlement.JenisBatal = "Batal Order";
            dbsettlement.IdDriver = db.IdDriver1;
            RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Order");
            db.TfExecuted = true;
            RepoAdminUangJalan.save(db);
            string strQuery = "";
            RepoAuditrail.saveKasirTfHistory(dbitem, strQuery);
            return Redirect("/Kasir/IndexTfBOBS");
        }

        [HttpPost]
        public ActionResult EditTfRemoval(RemovalAUJ model)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(model.Id);
            if (dbitem.Status.ToLower().Contains("dispatched") || dbitem.Status.ToLower().Contains("admin uang jalan"))
            {
                AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
                model.ModelListTf = resUang.ToList();
                foreach (var item in model.ModelListTf)
                {
                    if (item.JumlahTransfer.Value > 0)
                    {
                        item.isTf = true;
                        item.Code = "RT" + item.Id.ToString() + "-" + model.Id;

                        //lebah dieu sync ERPna
                        Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                        if (dbitem.StatusTagihan == "Ditagih")
                            Repoglt_det.saveFromAc(1, item.Code, item.JumlahTransfer.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangCustomer), RepoSalesOrder.FindByAUJ(dbitem.IdAdminUangJalan.Value));//D Piutang Customer
                        else
                            Repoglt_det.saveFromAc(1, item.Code, item.JumlahTransfer.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdBiayaPerjalanan), RepoSalesOrder.FindByAUJ(dbitem.IdAdminUangJalan.Value));//D Biaya Uang Jalan
                        Repoglt_det.saveFromAc(2, item.Code, 0, item.JumlahTransfer.Value, Repoac_mstr.FindByPk(item.IdCreditTf), RepoSalesOrder.FindByAUJ(dbitem.IdAdminUangJalan.Value));//K Transfer Bank
                    }
                    else
                        item.isTf = false;
                    item.setDb(dbitem.RemovalUangTf.Where(d => d.Id == item.Id).FirstOrDefault());
                }
                dbitem.Status = "dispatched";
                RepoRemoval.save(dbitem);
                return RedirectToAction("IndexTf");
            }

            model = new RemovalAUJ(dbitem);

            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrder.SalesOrderOncall.SONumber;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
            {
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrder.SalesOrderPickup.SONumber;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
            }
            return View("FormTfRemoval", model);
        }

        [HttpPost]
        public ActionResult EditTfKontrak(AdminUangJalan model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            List<int> ListIdDumy = model.ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id) && d.IdAdminUangJalan != null).ToList();
            Context.SalesOrderKontrakTruck salesOrderKontrakTruck = dbitem.SalesOrderKontrak.SalesOrderKontrakTruck.Where(d => d.Id == dbsoDummy.FirstOrDefault().SalesKontrakTruckId).FirstOrDefault();
            AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
            model.ModelListTf = resUang.ToList();
            foreach (Context.SalesOrderKontrakListSo sokls in dbsoDummy)
            {
                Context.SalesOrderKontrakListSo sokls1 = RepoSalesOrderKontrakListSo.FindByPK(sokls.Id);
                sokls1.Status = "dispatched";
                sokls1.StatusFlow = "DISPATCHED";
                GenerateDokumen(dbitem.Id, dbitem.SalesOrderKontrak.Customer, sokls.Id.ToString());
                RepoSalesOrderKontrakListSo.save(sokls1);
            }
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            int idx = 0;
            Context.AdminUangJalan db = dbsoDummy.FirstOrDefault().AdminUangJalan;
            string soNumber = db.SONumber;
            if (salesOrderKontrakTruck == null)
            {
                salesOrderKontrakTruck = dbitem.SalesOrderKontrak.SalesOrderKontrakTruck.Where(d => d.DataTruckId == dbsoDummy.FirstOrDefault().IdDataTruck).FirstOrDefault();
                if (salesOrderKontrakTruck == null)
                {
                    salesOrderKontrakTruck = dbitem.SalesOrderKontrak.SalesOrderKontrakTruck.Where(d => d.DataTruckId == null).FirstOrDefault();
                    if (salesOrderKontrakTruck == null)
                    {
                        Context.SalesOrderKontrakTruck soktbt = new Context.SalesOrderKontrakTruck();
                        soktbt.SalesKontrakId = dbitem.SalesOrderKontrakId.Value;
                        soktbt.DataTruckId = dbsoDummy.FirstOrDefault().IdDataTruck;
                        soktbt.StatusTruk = "Available";
                        soktbt.IdDriver1 = dbsoDummy.FirstOrDefault().Driver1Id;
                        RepoSalesOrderKontrakListSo.saveParent(soktbt);
                        salesOrderKontrakTruck = dbitem.SalesOrderKontrak.SalesOrderKontrakTruck.Where(d => d.DataTruckId == dbsoDummy.FirstOrDefault().IdDataTruck).FirstOrDefault();
                    }
                    else if (salesOrderKontrakTruck != null)
                    {
                        salesOrderKontrakTruck.DataTruckId = dbsoDummy.FirstOrDefault().IdDataTruck;
                        RepoSalesOrder.save(dbitem);
                    }
                }
            }
            foreach (Context.SalesOrderKontrakListSo sokls in dbsoDummy)
            {
                Context.SalesOrderKontrakListSo sokls1 = RepoSalesOrderKontrakListSo.FindByPK(sokls.Id);
                if (salesOrderKontrakTruck != null)
                    sokls1.SalesKontrakTruckId = salesOrderKontrakTruck.Id;
                RepoSalesOrderKontrakListSo.save(sokls1);
            }
            if (salesOrderKontrakTruck.PendapatanDiakui != true && DateTime.Now.Month.ToString("00").PadLeft(2, '0') == dbsoDummy.FirstOrDefault().MuatDate.Month.ToString("00").PadLeft(2, '0'))
            {
                Repoglt_det.saveFromAc(1, "PD-" + soNumber, RepoSalesOrder.Harga(dbitem, dbsoDummy.FirstOrDefault()), 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem, null, dbsoDummy.FirstOrDefault());
                Repoglt_det.saveFromAc(2, "PD-" + soNumber, 0, RepoSalesOrder.Harga(dbitem, dbsoDummy.FirstOrDefault()), Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem, null, dbsoDummy.FirstOrDefault());
                salesOrderKontrakTruck.PendapatanDiakui = true;
            }
            if (db.JurnalVoucher != true)
            {
                db.JurnalVoucher = true;
                decimal nominalHutangUangJalanDriver = 0;
                foreach (var item in model.ModelListTf)
                {
                    if (item.isTf != true)
                    {
                        nominalHutangUangJalanDriver += item.JumlahTransfer.Value;
                    }
                }
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());

                Repoglt_det.saveFromAc(1, "KT-" + soNumber, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, dbsoDummy.FirstOrDefault());//Hutang Uang Jalan Driver
                foreach (var item in model.ModelListTf)
                {
                    if (item.isTf != true && item.JumlahTransfer > 0)
                    {
                        idx++;
                        Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, item.JumlahTransfer.Value, Repoac_mstr.FindByPk(item.IdCreditTf), dbitem, DateTime.Now.ToString("ddMMyy-") + item.JamAktual.Value.ToString(), dbsoDummy.FirstOrDefault(), item.TanggalAktual.Value.AddDays(1));
                    }
                }
                foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                {
                    Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);

                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, aujvs.Value, Repoac_mstr.FindByPk(spbu.ac_id), dbitem, aujvs.Keterangan, dbsoDummy.FirstOrDefault());//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                }
                foreach (Context.AdminUangJalanVoucherKapal aujvk in db.AdminUangJalanVoucherKapal)
                {
                    Context.LookupCode kapal = RepoLookup.FindByName(aujvk.Keterangan);
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, aujvk.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvk.Keterangan).ac_id), dbitem, aujvk.Keterangan, dbsoDummy.FirstOrDefault());//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                }
                string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu.Where(d => d.Value > 0))
                {
                    Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                    Context.Customer customer = RepoCustomer.FindByPK(spbu.VendorId.Value);

                    idx++;
                    NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                    con.Open();
                    using (DataTable dt = new DataTable())
                    {
                        var query = RepoSalesOrder.saveQueryGrMstrKontrak(UserPrincipal.username, customer, dbsoDummy.FirstOrDefault().DataTruck.VehicleNo, decimal.Parse(aujvs.Value.ToString()));
                        NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                        da.Fill(dt);
                        cmd.Dispose();
                        con.Close();
                    }
                }
                foreach (Context.AdminUangJalanVoucherKapal aujvk in db.AdminUangJalanVoucherKapal)
                {
                    Context.LookupCode kapal = RepoLookup.FindByName(aujvk.Keterangan);
                    Context.Customer customer = RepoCustomer.FindByPK(kapal.VendorId.Value);
                    idx++;
                    NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                    con.Open();
                    using (DataTable dt = new DataTable())
                    {
                        var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbitem, decimal.Parse(aujvk.Value.ToString()));
                        NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                        da.Fill(dt);
                        cmd.Dispose();
                        con.Close();
                    }
                }
                if (db.PotonganB > 0)
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, null, dbsoDummy.FirstOrDefault());//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganP > 0)
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem, null, dbsoDummy.FirstOrDefault());//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganK > 0)
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem, null, dbsoDummy.FirstOrDefault());
                }
                if (db.PotonganT > 0)
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem, null, dbsoDummy.FirstOrDefault());
                }
                if (!(db.PotonganT > 0 || db.PotonganK > 0 || db.PotonganP > 0 || db.PotonganB > 0))
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, db.KasbonDriver1, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem, null, dbsoDummy.FirstOrDefault());
                }
            }
            else
            {
                foreach (var item in model.ModelListTf)
                {
                    if (item.JumlahTransfer.Value > 0)
                    {
                        if (item.isTf != true)
                        {
                            Repoglt_det.saveFromAc(idx, "KT-" + soNumber, 0, item.JumlahTransfer.Value, Repoac_mstr.FindByPk(item.IdCreditTf), dbitem, DateTime.Now.ToString("ddMMyy-") + item.JamAktual.Value.ToString(), dbsoDummy.FirstOrDefault(), item.TanggalAktual.Value.AddDays(1));
                            idx++;
                            Repoglt_det.saveFromAc(idx, "KT-" + soNumber, item.JumlahTransfer.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, dbsoDummy.FirstOrDefault());//Hutang Uang Jalan Driver
                        }
                    }
                }
            }
            foreach (var item in model.ModelListTf)
            {
                if (item.JumlahTransfer.Value > 0)
                    item.isTf = true;
                else
                    item.isTf = false;
                item.setDbKasirKontrak(dbsoDummy.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(d => d.Id == item.Id).FirstOrDefault());
            }

            RepoSalesOrder.save(dbitem);
            string strQuery = "";
            RepoAuditrail.saveKasirTfHistory(dbitem, strQuery, string.Join(",", dbsoDummy.Select(d => d.Id)));
            return RedirectToAction("IndexTf");
        }

        [MyAuthorize(Menu = "Kasir Transfer", Action = "read")]
        public ActionResult ViewTF(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            if (dbitem.AdminUangJalanId.HasValue)
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
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
                return View("FormTfView", model);
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
                ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormTfView", model);
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
                ViewBag.Title = "Kasir Transfer" + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormTfView", model);
            }

            return View("");
        }
        public ActionResult ViewTFRemoval(int id)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(id);
            RemovalAUJ model = new RemovalAUJ(dbitem);
            ViewBag.Title = "Kasir Transfer " + dbitem.SalesOrder.SONumber;
            return View("FormTfRemovalView", model);
        }
        [MyAuthorize(Menu = "Kasir Transfer", Action = "read")]
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
        [MyAuthorize(Menu = "Kasir Cash", Action = "update")]
        public ActionResult EditKas(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);
            ViewBag.KeteranganAdmin = model.KeteranganAdmin;
            ViewBag.IpAddress = AppHelper.GetIPAddress();

            model.IdSalesOrder = dbitem.Id;
            GenerateModel(dbitem, model);

            return View("FormKas", model);
        }
        [MyAuthorize(Menu = "Kasir Cash", Action = "update")]
        public ActionResult EditKasBatal(int id, int AdminUangJalanId)
        {
            Context.AdminUangJalan auj = RepoAdminUangJalan.FindByPK(AdminUangJalanId);
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(auj.SalesOrderId);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            model = new AdminUangJalan(auj);
            ViewBag.IpAddress = AppHelper.GetIPAddress();

            model.IdSalesOrder = dbitem.Id;
            GenerateModel(dbitem, model);
            ViewBag.AdminUangJalanId = AdminUangJalanId;

            return View("FormKasBatal", model);
        }
        [MyAuthorize(Menu = "Kasir Cash", Action = "update")]
        public ActionResult EditKasRemoval(int id)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(id);
            RemovalAUJ model = new RemovalAUJ(dbitem);

            return View("FormKasRemoval", model);
        }
        [MyAuthorize(Menu = "Kasir Cash", Action = "update")]
        public ActionResult EditKasKontrak(int id, string listSo)
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
            ViewBag.KeteranganAdmin = model.KeteranganAdmin;

            model.IdSalesOrder = dbitem.Id;
            model.ModelKontrak = new SalesOrderKontrak(dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
            model.ListIdSo = listSo;
            foreach (var item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.ToList())
            {
                model.ModelKontrak.ListValueModelSOKontrak.Add(new SalesOrderKontrakListSo(item));
            }
            GenerateModel(dbitem, model);

            return View("FormKas", model);
        }
        [HttpPost]
        public ActionResult EditKas(AdminUangJalan model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            string soNumber = dbitem.SONumber;
            Context.AdminUangJalan db = dbitem.AdminUangJalan;
            string codeBT = "BT-" + (dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderPickup.SONumber);
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            if (dbitem.Status.ToLower().Contains("dispatched") || dbitem.Status.ToLower().Contains("admin uang jalan"))
            {
                if (dbitem.PendapatanDiakui != true)
                {//START beruhubungan dgn utang piutang pendapatan hanya sampai sini, hanya sekali per SO
                    if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        foreach (var item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                        {
                            decimal harga = RepoSalesOrder.HargaKonsolidasiPerItem(item);
                            Context.Customer supplier = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.NamaTagihanId.Value);
                            string customer = supplier.CustomerCodeOld + "-" + supplier.CustomerNama;
                            Context.SalesOrder sok = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                            if (!sok.PendapatanDiakui)
                            {
                                sok.SalesOrderKonsolidasi.ProcessedByKasir = true;
                                sok.PendapatanDiakui = true;//END beruhubungan dgn utang customer pendapatan hanya sampai sini, hanya sekali per SO
                                Repoglt_det.saveFromAc(1, "PD-" + dbitem.SONumber + ", " + sok.SONumber, harga, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem, "Piutang Belum Ditagih - " + customer, null, null, supplier.Id);
                                Repoglt_det.saveFromAc(2, "PD-" + dbitem.SONumber + ", " + sok.SONumber, 0, harga, Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem, "Pendapatan Belum Ditagih - " + customer, null, null, supplier.Id);
                            }
                        }
                    }
                    else if (dbitem.PendapatanDiakui != true)
                    {//START beruhubungan dgn utang piutang pendapatan hanya sampai sini, hanya sekali per SO
                        Repoglt_det.saveFromAc(1, "PD-" + soNumber, RepoSalesOrder.Harga(dbitem), 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem);
                        Repoglt_det.saveFromAc(2, "PD-" + soNumber, 0, RepoSalesOrder.Harga(dbitem), Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem);
                        dbitem.PendapatanDiakui = true;//END beruhubungan dgn utang customer pendapatan hanya sampai sini, hanya sekali per SO
                    }
                    string code = "KK-" + soNumber + "-" + urutanBatal;
                }
                for (int i = 0; i < model.ModelListTf.Count(); i++)
                {
                    if (model.ModelListTf[i].Nama == "Tunai")
                    {
                        if (validationManual(model))
                        {
                            Context.AdminUangJalanUangTf dbtf = dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault();
                            if (dbtf.JumlahTransfer != null && dbtf.JumlahTransfer != 0)
                                dbtf.JumlahTransfer = model.ModelListTf[i].JumlahTransfer;
                            else
                                dbtf.JumlahTransfer = model.ModelListTf[i].Value;
                            dbtf.TanggalAktual = model.ModelListTf[i].TanggalAktual;
                            dbtf.JamAktual = model.ModelListTf[i].JamAktual;
                            dbtf.KeteranganTf = model.ModelListTf[i].KeteranganTf;
                            dbtf.IdDriverPenerima = model.ModelListTf[i].IdDriverPenerima;
                            int? urutan = RepoAdminUangJalan.getUrutanUang(dbtf.TanggalAktual.Value) + 1;
                            dbtf.Urutan = urutan;
                            decimal nominalHutangUangJalanDriver = dbtf.JumlahTransfer.Value;
                            int idx = 3;
                            string code = "KK-" + soNumber + "-" + urutanBatal;
                            if (db.JurnalVoucher != true)
                            {
                                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                                nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                                nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());
                                string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                                foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                                {
                                    Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                                    if (spbu != null && aujvs.Value > 0)
                                    {
                                        idx++;
                                        Context.Customer customer = RepoCustomer.FindByPK(spbu.VendorId.Value);
                                        Repoglt_det.saveFromAc(idx, code, 0, aujvs.Value, Repoac_mstr.FindByPk(spbu.ac_id), dbitem, aujvs.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                                        idx++;
                                        NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                                        con.Open();
                                        using (DataTable dt = new DataTable())
                                        {
                                            var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbitem, decimal.Parse(aujvs.Value.ToString()));
                                            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                                            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                                            da.Fill(dt);
                                            cmd.Dispose();
                                            con.Close();
                                        }
                                    }
                                }
                                foreach (Context.AdminUangJalanVoucherKapal aujvk in db.AdminUangJalanVoucherKapal)
                                {
                                    Context.LookupCode kapal = RepoLookup.FindByName(aujvk.Keterangan);
                                    idx++;
                                    Context.Customer customer = RepoCustomer.FindByPK(kapal.VendorId.Value);
                                    Repoglt_det.saveFromAc(idx, code, 0, aujvk.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvk.Keterangan).ac_id), dbitem, aujvk.Keterangan, null, null, customer.Id);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                                    idx++;
                                    NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
                                    con.Open();
                                    using (DataTable dt = new DataTable())
                                    {
                                        var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbitem, decimal.Parse(aujvk.Value.ToString()));
                                        NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                                        da.Fill(dt);
                                        cmd.Dispose();
                                        con.Close();
                                    }
                                }

                                if (db.PotonganB > 0)
                                {
                                    idx++;
                                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem);//PIUTANG DRIVER BATAL JALAN
                                }
                                if (db.PotonganP > 0)
                                {
                                    idx++;
                                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                                }
                                if (db.PotonganK > 0)
                                {
                                    idx++;
                                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem);
                                }
                                if (db.PotonganT > 0)
                                {
                                    idx++;
                                    Repoglt_det.saveFromAc(idx, code, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem);
                                }
                                db.JurnalVoucher = true;
                            }
                            Repoglt_det.saveFromAc(1, code, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);//Hutang Uang Jalan Driver
                            Context.MasterPool mp = RepoMasterPool.FindByIPAddress();
                            Repoglt_det.saveFromAc(2, code, 0, dbtf.JumlahTransfer, Repoac_mstr.FindByPk(RepoMasterPool.FindByIPAddress().IdCreditCash), dbitem, DateTime.Now.ToString("ddMMyy-") + dbtf.JamAktual.Value.ToString(), null, dbtf.TanggalAktual);
                            //                            Repoglt_det.saveFromAc(2, code, 0, dbtf.JumlahTransfer, Repoac_mstr.FindByPk(RepoMasterPool.FindByIPAddress().IdCreditCash), dbitem, dbtf.TanggalAktual.Value.ToString("ddMMyy") + "-" + dbtf.JamAktual.Value.Hours.ToString().PadLeft(2, '0') + dbtf.JamAktual.Value.Minutes.ToString().PadLeft(2, '0'));
                            if (dbtf.TanggalAktual.HasValue)
                                dbtf.isTf = true;
                        }
                        else
                            return View("FormKas", model);
                    }
                }
                dbitem.Status = "dispatched";
                dbitem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbitem);
                string strQuery = "";
                if (RepoDokumen.FindBySO(dbitem.Id) == null)
                {
                    if (dbitem.SalesOrderOncallId.HasValue)
                        strQuery += GenerateDokumen(dbitem.Id, dbitem.SalesOrderOncall.Customer);
                    else if (dbitem.SalesOrderPickupId.HasValue)
                        strQuery += GenerateDokumen(dbitem.Id, dbitem.SalesOrderPickup.Customer);
                    else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        foreach (var item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                        {
                            Context.Customer tagihan = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.NamaTagihanId.Value);
                            Context.SalesOrder so = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                            strQuery += GenerateDokumen(so.Id, tagihan, "", item.SalesOrderKonsolidasi.StrDaftarHargaItem);
                        }
                    }
                }
                RepoAuditrail.saveKasirKasHistory(dbitem, strQuery);
                return RedirectToAction("IndexKas");
            }
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);
            model.IdSalesOrder = dbitem.Id;
            GenerateModel(dbitem, model);
            ViewBag.Status = "batal";
            return View("FormKas", model);
        }

        [HttpPost]
        public ActionResult EditKasBatal(AdminUangJalan model, int AdminUangJalanId)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            string soNumber = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.SONumber : dbitem.SalesOrderPickup.SONumber;
            Context.AdminUangJalan db = RepoAdminUangJalan.FindByPK(AdminUangJalanId);
            string codeBT = "BT-" + (dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderPickup.SONumber);
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
            for (int i = 0; i < model.ModelListTf.Count(); i++)
            {
                if (model.ModelListTf[i].Nama == "Tunai")
                {
                    if (validationManual(model))
                    {
                        Context.AdminUangJalanUangTf dbtf = db.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault();
                        if (dbtf.JumlahTransfer != null && dbtf.JumlahTransfer != 0)
                            dbtf.JumlahTransfer = model.ModelListTf[i].JumlahTransfer;
                        else
                            dbtf.JumlahTransfer = model.ModelListTf[i].Value;
                        dbtf.TanggalAktual = model.ModelListTf[i].TanggalAktual;
                        dbtf.JamAktual = model.ModelListTf[i].JamAktual;
                        dbtf.KeteranganTf = model.ModelListTf[i].KeteranganTf;
                        dbtf.IdDriverPenerima = model.ModelListTf[i].IdDriverPenerima;
                        int? urutan = RepoAdminUangJalan.getUrutanUang(dbtf.TanggalAktual.Value) + 1;
                        dbtf.Urutan = urutan;
                        decimal nominalHutangUangJalanDriver = model.ModelListTf[i].Value.Value;
                        int idx = 3;
                        Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                        string code = "KK-" + soNumber + "-" + urutanBatal;
                        if (db.JurnalVoucher != true)
                        {
                            nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                            nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                            nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                            nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                            nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());

                            foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, code, 0, aujvs.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbitem, aujvs.Keterangan);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                            }
                            foreach (Context.AdminUangJalanVoucherKapal aujvs in db.AdminUangJalanVoucherKapal)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, code, 0, aujvs.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbitem, aujvs.Keterangan);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                            }
                            if (db.PotonganB > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, code, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem);//PIUTANG DRIVER BATAL JALAN
                            }
                            if (db.PotonganP > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, code, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                            }
                            if (db.PotonganK > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, code, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem);
                            }
                            if (db.PotonganT > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, code, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem);
                            }
                            db.JurnalVoucher = true;
                        }
                        Repoglt_det.saveFromAc(1, code, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);//Hutang Uang Jalan Driver
                        Context.MasterPool mp = RepoMasterPool.FindByIPAddress();
                        Repoglt_det.saveFromAc(2, code, 0, dbtf.JumlahTransfer, Repoac_mstr.FindByPk(RepoMasterPool.FindByIPAddress().IdCreditCash), dbitem, DateTime.Now.ToString("ddMMyy-") + dbtf.JamAktual.Value.ToString(), null, dbtf.TanggalAktual);
                        if (dbtf.TanggalAktual.HasValue)
                            dbtf.isTf = true;
                        Context.SettlementBatal dbsettlement = RepoSettBatal.FindByAUJ(db.Id);
                        if (dbsettlement == null)
                            dbsettlement = new Context.SettlementBatal();
                        //ambil data 
                        dbsettlement.IdSalesOrder = model.IdSalesOrder;
                        if (db.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai"))
                        {
                            dbsettlement.KasDiterima = db.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").Sum(t => t.JumlahTransfer);
                        }
                        if (db.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer")))
                        {
                            dbsettlement.TransferDiterima = db.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer")).Sum(t => t.JumlahTransfer);
                        }
                        dbsettlement.IdAdminUangJalan = db.Id;
                        dbsettlement.SolarDiterima = db.AdminUangJalanVoucherSpbu.Sum(s => s.Value);
                        dbsettlement.KapalDiterima = db.AdminUangJalanVoucherKapal.Sum(s => s.Value);
                        dbsettlement.JenisBatal = dbitem.AdminUangJalanId == null ? "Batal Truk" : "Batal Order";
                        dbsettlement.IdDriver = db.IdDriver1;
                        db.CashExecuted = true;
                        RepoAdminUangJalan.save(db);
                        RepoSettBatal.save(dbsettlement, UserPrincipal.id, dbsettlement.JenisBatal);
                    }
                    else
                        return View("FormKas", model);
                }
                string strQuery = "";
                RepoAuditrail.saveKasirKasHistory(dbitem, strQuery);
                return Redirect("/Kasir/IndexKasBOBS");
            }
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);
            model.IdSalesOrder = dbitem.Id;
            GenerateModel(dbitem, model);
            ViewBag.Status = "batal";
            return View("FormKas", model);
        }
        [HttpPost]
        public ActionResult EditKasRemoval(RemovalAUJ model)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(model.Id);
            if (dbitem.Status.ToLower().Contains("dispatched") || dbitem.Status.ToLower().Contains("admin uang jalan"))
            {
                for (int i = 0; i < model.ModelListTf.Count(); i++)
                {
                    if (model.ModelListTf[i].Nama == "Tunai")
                    {
                        if (validationManual(model))
                        {
                            Context.RemovalUangTf dbtf = dbitem.RemovalUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault();
                            if (dbtf.JumlahTransfer != null && dbtf.JumlahTransfer != 0)
                                dbtf.JumlahTransfer = model.ModelListTf[i].JumlahTransfer;
                            else
                                dbtf.JumlahTransfer = model.ModelListTf[i].Value;
                            dbtf.TanggalAktual = model.ModelListTf[i].TanggalAktual;
                            dbtf.JamAktual = model.ModelListTf[i].JamAktual;
                            dbtf.KeteranganTf = model.ModelListTf[i].KeteranganTf;
                            dbtf.IdDriverPenerima = model.ModelListTf[i].IdDriverPenerima;
                            if (dbtf.TanggalAktual.HasValue)
                                dbtf.isTf = true;
                        }
                        else
                        {
                            return View("FormKasRemoval", model);
                        }
                    }
                }

                dbitem.Status = "dispatched";
                RepoRemoval.save(dbitem);
                return RedirectToAction("IndexKas");
            }
            model = new RemovalAUJ(dbitem);

            return View("FormKasRemoval", model);
        }

        [HttpPost]
        public ActionResult EditKasKontrak(AdminUangJalan model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            List<int> ListIdDumy = model.ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
            Context.SalesOrderKontrakTruck salesOrderKontrakTruck = dbitem.SalesOrderKontrak.SalesOrderKontrakTruck.Where(d => d.DataTruckId == dbsoDummy.FirstOrDefault().IdDataTruck).FirstOrDefault();

            for (int i = 0; i < model.ModelListTf.Count(); i++)
            {
                if (model.ModelListTf[i].Nama == "Tunai")
                {
                    if (validationManual(model))
                    {
                        Context.AdminUangJalan db = dbsoDummy.FirstOrDefault().AdminUangJalan;
                        Context.AdminUangJalanUangTf dbtf = db.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault();
                        if (dbtf.JumlahTransfer != null && dbtf.JumlahTransfer != 0)
                            dbtf.JumlahTransfer = model.ModelListTf[i].JumlahTransfer;
                        else
                            dbtf.JumlahTransfer = model.ModelListTf[i].Value;
                        dbtf.TanggalAktual = model.ModelListTf[i].TanggalAktual;
                        dbtf.JamAktual = model.ModelListTf[i].JamAktual;
                        dbtf.KeteranganTf = model.ModelListTf[i].KeteranganTf;
                        dbtf.IdDriverPenerima = model.ModelListTf[i].IdDriverPenerima;
                        if (dbtf.TanggalAktual.HasValue)
                            dbtf.isTf = true;
                        string soNumber = "";
                        foreach (var item in dbsoDummy)
                        {
                            GenerateDokumen(dbitem.Id, dbitem.SalesOrderKontrak.Customer, item.Id.ToString());
                        }
                        foreach (var item in dbsoDummy)
                        {
                            item.Status = "dispatched";
                            item.StatusFlow = "DISPATCHED";
                            soNumber += "," + item.NoSo;
                            item.DokumenId = RepoDokumen.FindAll().Where(d => d.ListIdSo == item.Id.ToString()).FirstOrDefault().Id;
                        }
                        decimal nominalHutangUangJalanDriver = dbtf.JumlahTransfer.Value;
                        int idx = 3;
                        Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                        //Pendapatan diakui dijurnal per truk saat bulan berjalan == bulan muat, sekali saja
                        if (salesOrderKontrakTruck != null && salesOrderKontrakTruck.PendapatanDiakui != true && DateTime.Now.Month.ToString("00").PadLeft(2, '0') == dbsoDummy.FirstOrDefault().MuatDate.Month.ToString("00").PadLeft(2, '0'))
                        {
                            Repoglt_det.saveFromAc(1, "PD-" + soNumber, RepoSalesOrder.Harga(dbitem, dbsoDummy.FirstOrDefault()), 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(2, "PD-" + soNumber, 0, RepoSalesOrder.Harga(dbitem, dbsoDummy.FirstOrDefault()), Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem, null, dbsoDummy.FirstOrDefault());
                            salesOrderKontrakTruck.PendapatanDiakui = true;
                        }
                        if (db.JurnalVoucher != true)
                        {
                            db.JurnalVoucher = true;
                            nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                            nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                            nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                            nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                            nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());

                            foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, "KK-" + soNumber, 0, aujvs.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbitem, aujvs.Keterangan, dbsoDummy.FirstOrDefault());//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                            }
                            foreach (Context.AdminUangJalanVoucherKapal aujvs in db.AdminUangJalanVoucherKapal)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, "KK-" + soNumber, 0, aujvs.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvs.Keterangan).ac_id), dbitem, aujvs.Keterangan, dbsoDummy.FirstOrDefault());//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                            }
                            if (db.PotonganB > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, "KK-" + soNumber, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, null, dbsoDummy.FirstOrDefault());//PIUTANG DRIVER BATAL JALAN
                            }
                            if (db.PotonganP > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, "KK-" + soNumber, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem, null, dbsoDummy.FirstOrDefault());//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                            }
                            if (db.PotonganK > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, "KK-" + soNumber, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem, null, dbsoDummy.FirstOrDefault());
                            }
                            if (db.PotonganT > 0)
                            {
                                idx++;
                                Repoglt_det.saveFromAc(idx, "KK-" + soNumber, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem, null, dbsoDummy.FirstOrDefault());
                            }
                        }
                        Repoglt_det.saveFromAc(1, "KK-" + soNumber, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, dbsoDummy.FirstOrDefault());//Hutang Uang Jalan Driver
                        Context.MasterPool mp = RepoMasterPool.FindByIPAddress();
                        Repoglt_det.saveFromAc(2, "KK-" + soNumber, 0, dbtf.JumlahTransfer, Repoac_mstr.FindByPk(RepoMasterPool.FindByIPAddress().IdCreditCash), dbitem, DateTime.Now.ToString("ddMMyy-") + dbtf.JamAktual.Value.ToString(), dbsoDummy.FirstOrDefault(), dbtf.TanggalAktual.Value.AddDays(1));
                    }
                    else
                    {
                        return View("FormKas", model);
                    }
                }
            }

            RepoSalesOrder.save(dbitem);
            string strQuery = "";
            RepoAuditrail.saveKasirKasHistory(dbitem, strQuery, string.Join(",", dbsoDummy.Select(d => d.Id)));
            return RedirectToAction("IndexKas");
        }

        public ActionResult NyusulinPengakuanPendapatan(string noso)
        {
            string soNumber = noso;
            List<Context.SalesOrderKontrakListSo> dbsoDummy = RepoSalesOrderKontrakListSo.FindAll().Where(d => d.NoSo == noso).ToList();
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(dbsoDummy.FirstOrDefault().SalesKontrakId.Value);
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            Repoglt_det.saveFromAc(1, "PD-" + soNumber, RepoSalesOrder.Harga(dbitem, dbsoDummy.FirstOrDefault()), 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem);
            Repoglt_det.saveFromAc(2, "PD-" + soNumber, 0, RepoSalesOrder.Harga(dbitem, dbsoDummy.FirstOrDefault()), Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem);
            return RedirectToAction("IndexKas");
        }

        [MyAuthorize(Menu = "Kasir Cash", Action = "read")]
        public ActionResult ViewKas(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            if (dbitem.AdminUangJalanId.HasValue)
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
                ViewBag.Title = "Kasir Cash " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
                return View("FormKasView", model);
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
                ViewBag.Title = "Kasir Cash " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormKasView", model);
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
                ViewBag.Title = "Kasir Cash " + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("FormKasView", model);
            }

            return View("");
        }
        [MyAuthorize(Menu = "Kasir Cash", Action = "read")]
        public ActionResult ViewKasRemoval(int id)
        {
            Context.Removal dbitem = RepoRemoval.FindByPK(id);
            RemovalAUJ model = new RemovalAUJ(dbitem);

            return View("FormKasRemovalView", model);
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
                ViewBag.Title = "Kasir Kas " + dbitem.SalesOrderOncall.SONumber;
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
                ViewBag.Title = "Kasir Kas " + dbitem.SalesOrderPickup.SONumber;
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
                ViewBag.Title = "Kasir Kas " + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
            }
        }
        public bool validationManual(AdminUangJalan model)
        {
            //if (dbtf.JumlahTransfer != null && dbtf.JumlahTransfer != 0){

            //}
            //    dbtf.JumlahTransfer = model.ModelListTf[i].JumlahTransfer;
            //else
            //    dbtf.JumlahTransfer = model.ModelListTf[i].Value;


            return true;
        }
        public bool validationManual(RemovalAUJ model)
        {
            //if (dbtf.JumlahTransfer != null && dbtf.JumlahTransfer != 0){

            //}
            //    dbtf.JumlahTransfer = model.ModelListTf[i].JumlahTransfer;
            //else
            //    dbtf.JumlahTransfer = model.ModelListTf[i].Value;


            return true;
        }
        public ActionResult ShowPrintKas(int id, string listSo, string terbilang)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            //cari
            AdminUangJalan model = new AdminUangJalan();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = new List<Context.SalesOrderKontrakListSo>();
            if (dbitem.AdminUangJalanId.HasValue)
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
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ViewBag.noBukti = items.FirstOrDefault().Code;
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrderOncall.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ViewBag.noBukti = items.FirstOrDefault().Code;
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.noBukti = items.FirstOrDefault().Code;
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbitem.AdminUangJalan.Driver1.NamaDriver;
                NoPol = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
            }
            else if (dbitem.SalesOrderKontrakId.HasValue)
            {
                ViewBag.noBukti = items.FirstOrDefault().Code;
                ViewBag.tanggal = DateTime.Now.ToShortDateString();
                ViewBag.kontak = dbsoDummy.FirstOrDefault().Driver1.NamaDriver;
                NoPol = dbsoDummy.FirstOrDefault().DataTruck.VehicleNo;
            }
            decimal? total = 0;
            if (items != null)
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

        private string GenerateDokumen(int IdSo, Context.Customer dbcust, string ListIdSo = "", string rute = "")
        {
            Context.Dokumen dbdokumen = new Context.Dokumen();
            Context.SalesOrder so = RepoSalesOrder.FindByPK(IdSo);
            if (ListIdSo != "")
            {
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(ListIdSo));
                dbdokumen.SONumber = sokls.NoSo;
                dbdokumen.DataTruckId = sokls.IdDataTruck;
                dbdokumen.DriverId = sokls.Driver1Id;
                dbdokumen.TanggalMuat = sokls.MuatDate;
            }
            else if (so != null)
            {
                dbdokumen.SONumber = so.SONumber;
                dbdokumen.DataTruckId = so.DataTruckId;
                dbdokumen.DriverId = so.DriverId;
                dbdokumen.TanggalMuat = so.OrderTanggalMuat;
            }
            dbdokumen.Status = "Open";
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

        [MyAuthorize(Menu = "Batal Order Belum Settlement", Action = "read")]
        public ActionResult BatalOrderBelumSettlement()
        {
            ViewBag.Title = "Batal Order Belum Settlement";
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "BatalOrderBelumSettlement" && d.Controller == "Report").ToList();
            return View();
        }

    }
}