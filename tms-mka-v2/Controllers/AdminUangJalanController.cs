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
    public class AdminUangJalanController : BaseController
    {
        private IAdminUangJalanRepo RepoAdminUangJalan;
        private ISalesOrderRepo RepoSalesOrder;
        private IDaftarHargaOnCallRepo RepoDHO;
        private IDaftarHargaKonsolidasiRepo RepoDHK;
        private IDaftarHargaKontrakRepo RepoDHKontrak;
        private IRuteRepo RepoRute;
        private IAtmRepo RepoAtm;
        private IDataBoronganRepo RepoBor;
        private IBatalOrderRepo RepoBatalOrder;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private IHistoryJalanTruckRepo RepoHistoryJalanTruck;
        private IRemovalRepo RepoRemovalRepo;
        private ISettlementBatalRepo RepoSettlementBatal;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iso_mstrRepo Reposo_mstr;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private Ipbyd_detRepo Repopbyd_det;
        private ILookupCodeRepo RepoLookupCode;
        private Icashd_detRepo Repocashd_det;
        private IDokumenRepo RepoDokumen;
        private IDriverRepo RepoDriver;
        private IAuditrailRepo RepoAuditrail;
        private IMasterSolarRepo RepoSolar;
        private ICustomerRepo RepoCustomer;
        private IRevisiRuteRepo RepoRevisiRute;
        private IRevisiTanggalRepo RepoRevisiTanggal;
        private IRevisiJenisTrukRepo RepoRevisiJenisTruk;
        private IDataBoronganRepo RepoDataBorongan;
        public AdminUangJalanController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IAdminUangJalanRepo repoAdminUangJalan, ISalesOrderRepo repoSalesOrder, IDaftarHargaOnCallRepo repoDHO,
            IDaftarHargaKonsolidasiRepo repoDHK, IRuteRepo repoRute, IAtmRepo repoAtm, IDataBoronganRepo repoBor, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo, IAuditrailRepo repoAuditrail,
            IHistoryJalanTruckRepo repoHistoryJalanTruck, IRemovalRepo repoRemovalRepo, ISettlementBatalRepo repoSettlementBatal, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, IMasterSolarRepo repoSolar,
            Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr, Ipbyd_detRepo repopbyd_det, ILookupCodeRepo repoLookupCode, Icashd_detRepo repocashd_det, IDriverRepo repoDriver, ICustomerRepo repoCustomer,
            IDokumenRepo repoDokumen, IBatalOrderRepo repoBatalOrder, IRevisiRuteRepo repoRevisiRute, IRevisiTanggalRepo repoRevisiTanggal, IRevisiJenisTrukRepo repoRevisiJenisTruk,
            IDaftarHargaKontrakRepo repoDHKontrak, Iso_mstrRepo reposo_mstr, IDataBoronganRepo repoDataBorongan)
            : base(repoBase, repoLookup)
        {
            RepoAdminUangJalan = repoAdminUangJalan;
            RepoSalesOrder = repoSalesOrder;
            RepoBatalOrder = repoBatalOrder;
            RepoDHO = repoDHO;
            RepoDHK = repoDHK;
            RepoRute = repoRute;
            RepoAtm = repoAtm;
            RepoBor = repoBor;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
            RepoHistoryJalanTruck = repoHistoryJalanTruck;
            RepoRemovalRepo = repoRemovalRepo;
            RepoSettlementBatal = repoSettlementBatal;
            Reposo_mstr = reposo_mstr;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            RepoSolar = repoSolar;
            Repopbyd_det = repopbyd_det;
            RepoLookupCode = repoLookupCode;
            Repocashd_det = repocashd_det;
            RepoDriver = repoDriver;
            RepoAuditrail = repoAuditrail;
            RepoCustomer = repoCustomer;
            RepoDokumen = repoDokumen;
            RepoRevisiRute = repoRevisiRute;
            RepoRevisiTanggal = repoRevisiTanggal;
            RepoRevisiJenisTruk = repoRevisiJenisTruk;
            RepoDHKontrak = repoDHKontrak;
            RepoDataBorongan = repoDataBorongan;
        }

        public string generateDataAUJ()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.AdminUangJalan> items = RepoAdminUangJalan.FindAll().ToList();

            foreach (var item in items)
            {
                Context.SalesOrder so = RepoSalesOrder.FindByPK(item.SalesOrderId);
                if (so != null)
                    RepoSalesOrder.save(so);
            }
            return "aa";
        }

        public string GenerateAUJNoSoKontrak()
        {
            List<Context.AdminUangJalan> aujs = RepoAdminUangJalan.FindAll().Where(d => d.DaftarHargaKontrakId != null && d.SONumber == null).ToList();
            foreach (Context.AdminUangJalan auj in aujs)
            {
                auj.SONumber = string.Join(", ", RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id).Select(d => d.NoSo));
                if (RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id).FirstOrDefault() != null)
                    auj.CustomerId = RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id).FirstOrDefault().SalesOrderKontrak.CustomerId;
                RepoAdminUangJalan.save(auj);
            }
            return "aa";
        }

        [MyAuthorize(Menu = "Admin Uang Jalan", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "AdminUangJalan").ToList();
            return View();
        }
        public ActionResult IndexKontrak()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "AdminUangJalan").ToList();
            return View();
        }
        public string Binding()
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllAdminUangJalan().Where(d => d.SalesOrderProsesKonsolidasiId == null || d.SalesOrderProsesKonsolidasiId != null && !d.SalesOrderProsesKonsolidasi.LangsungKeBilling).ToList();
            List<Context.SalesOrder> itemsSusulanKonsol = RepoSalesOrder.FindAllAdminUangJalanSusulanKonsol();
            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.SalesOrder item in items)
            {
                if (!item.SalesOrderKontrakId.HasValue)
                {
                    var _model = new AdminUangJalanIndex(item);
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

                    ListModel.Add(_model);
                }
            }
            foreach (Context.SalesOrder item in itemsSusulanKonsol)
            {
                var _model = new AdminUangJalanIndex(item);
                int IdJenisTruck;
                string IdRute;
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
                _model.Status = "Susulan Konsolidasi";
                ListModel.Add(_model);
            }

            return new JavaScriptSerializer().Serialize(new { total = items.Count, data = ListModel });
        }

        public string BindingPerSO(int? IdSalesOrder)
        {
            List<Context.AdminUangJalan> items = RepoAdminUangJalan.FindAll().Where(d => d.SalesOrderId == IdSalesOrder).ToList();
            Context.SalesOrder item = RepoSalesOrder.FindByPK(IdSalesOrder.Value);
            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.AdminUangJalan auj in items)
            {
                var _model = new AdminUangJalanIndex(item);
                _model.Driver = auj.Driver1.NamaDriver;
                _model.Nominal = auj.AdminUangJalanUangTf.Where(d => d.isTf).Sum(d => d.JumlahTransfer);
                ListModel.Add(_model);
            }
            return new JavaScriptSerializer().Serialize(new { total = items.Count, data = ListModel });
        }

        public string BindingKontrak()
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllAdminUangJalanKontrak();

            List<AdminUangJalanIndex> ListModel = new List<AdminUangJalanIndex>();
            foreach (Context.SalesOrder item in items)
            {
                if (item.SalesOrderKontrakId.HasValue)
                {
                    var data = item.SalesOrderKontrak.SalesOrderKontrakListSo.Where(p => p.IsProses && (p.Status == "save konfirmasi"))
                        .GroupBy(d => new { d.IdDataTruck, d.Driver1Id, d.Status, d.Urutan }).Select(grp => grp.ToList());
                    foreach (var itemGroup in data.ToList())
                    {
                        ListModel.Add(new AdminUangJalanIndex(item, itemGroup));
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }

        public void GenerateModel(AdminUangJalan model, Context.SalesOrder dbitem)
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
                ViewBag.Title = "Admin Uang Jalan " + dbitem.SalesOrderOncall.SONumber;
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
                ViewBag.Title = "Admin Uang Jalan " + dbitem.SalesOrderPickup.SONumber;
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
                ViewBag.Title = "Admin Uang Jalan " + dbitem.SalesOrderProsesKonsolidasi.SONumber;
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
                ViewBag.Title = "Admin Uang Jalan " + dbitem.SalesOrderKontrak.SONumber;
            }
        }
        public bool validationKonsolidasi(AdminUangJalan model)
        {
            bool valid = true;
            decimal totalAlokasi = model.ModelListTf.Sum(d => decimal.Parse(d.Value.ToString())) + model.ModelListSpbu.Sum(d => decimal.Parse(d.Value.ToString())) + model.ModelListKapal.Sum(d => decimal.Parse(d.Value.ToString()));
            if (model.KasbonDriver1 != null)
                totalAlokasi += model.KasbonDriver1.Value;
            if (totalAlokasi != model.TotalBorongan)
            {
                ModelState.AddModelError("TotalAlokasi", "Harus sama dengan total borongan");
                valid = false;
            }
            return valid;
        }
        public bool validation(AdminUangJalan model)
        {
            bool valid = true;
            int idx = 0;

            foreach (AdminUangBorongan item in model.ModelListBorongan.Where(d => !d.IsDelete))
            {
                if (!item.IdDataBorongan.HasValue)
                {
                    ModelState.AddModelError("ModelListBorongan[" + idx.ToString() + "].IdDataBorongan", "Borongan harus diisi.");
                    valid = false;
                }
                idx++;
            }
            idx = 0;
            foreach (AdminUangJalanTambahanRute item in model.ModelListTambahanRute.Where(d => !d.IsDelete))
            {
                if (!item.IdDataBorongan.HasValue)
                {
                    ModelState.AddModelError("ModelListTambahanRute[" + idx.ToString() + "].IdDataBorongan", "Borongan harus diisi.");
                    valid = false;
                }
                if (!item.value.HasValue)
                {
                    ModelState.AddModelError("ModelListTambahanRute[" + idx.ToString() + "].value", "Nilai harus diisi.");
                    valid = false;
                }
                idx++;
            }
            idx = 0;
            foreach (AdminUangJalanTambahanLain item in model.ModelListTambahanLain.Where(d => !d.IsDelete))
            {
                if (item.Keterangan == "")
                {
                    ModelState.AddModelError("ModelListTambahanLain[" + idx.ToString() + "].IdDataBorongan", "Borongan harus diisi.");
                    valid = false;
                }
                if (!item.Value.HasValue)
                {
                    ModelState.AddModelError("ModelListTambahanLain[" + idx.ToString() + "].Value", "Nilai harus diisi.");
                    valid = false;
                }
                idx++;
            }

            return valid;
        }
        public bool validationRemoval(RemovalAUJ model)
        {
            bool valid = true;
            int idx = 0;
            foreach (AdminUangBorongan item in model.ModelListBorongan.Where(d => !d.IsDelete))
            {
                if (!item.IdDataBorongan.HasValue)
                {
                    ModelState.AddModelError("ModelListBorongan[" + idx.ToString() + "].IdDataBorongan", "Borongan harus diisi.");
                    valid = false;
                }
                idx++;
            }

            idx = 0;
            foreach (AdminUangJalanTambahanRute item in model.ModelListTambahanRute.Where(d => !d.IsDelete))
            {
                if (!item.IdDataBorongan.HasValue)
                {
                    ModelState.AddModelError("ModelListTambahanRute[" + idx.ToString() + "].IdDataBorongan", "Borongan harus diisi.");
                    valid = false;
                }
                if (!item.value.HasValue)
                {
                    ModelState.AddModelError("ModelListTambahanRute[" + idx.ToString() + "].value", "Nilai harus diisi.");
                    valid = false;
                }
                idx++;
            }

            idx = 0;
            foreach (AdminUangJalanTambahanLain item in model.ModelListTambahanLain.Where(d => !d.IsDelete))
            {
                if (item.Keterangan == "")
                {
                    ModelState.AddModelError("ModelListTambahanLain[" + idx.ToString() + "].IdDataBorongan", "Borongan harus diisi.");
                    valid = false;
                }
                if (!item.Value.HasValue)
                {
                    ModelState.AddModelError("ModelListTambahanLain[" + idx.ToString() + "].Value", "Nilai harus diisi.");
                    valid = false;
                }
                idx++;
            }

            return valid;
        }

        public decimal GetSaldoKlaim(int id, string piutang_type)
        {
            Context.Driver dbdriver = RepoDriver.FindByPK(id);
            List<DriverPiutangHistory> ListModel = new List<DriverPiutangHistory>();
            List<Context.Klaim> dbklaim = dbdriver.BebanKlaimDriver.Select(d => d.Klaim).ToList();
            decimal? saldo = 0;

            List<Klaim> modelklaim = new List<Klaim>();
            foreach (var item in dbklaim)
            {
                ListModel.Add(new DriverPiutangHistory("a", "b", item, saldo.Value));
                if (item.BebanClaimDriver != null)
                    saldo += decimal.Parse(item.BebanClaimDriver.Value.ToString());
            }

            List<Context.AdminUangJalan> dbAuj = RepoAdminUangJalan.FindAll().Where(d => d.IdDriver1 == id && d.PotonganK > 0).ToList();
            foreach (var item in dbAuj)
            {
                ListModel.Add(new DriverPiutangHistory(item, saldo.Value));
                saldo += decimal.Parse(item.PotonganK.Value.ToString());
            }
            return decimal.Parse(dbklaim.Where(d => d.BebanClaimDriver != null && d.BebanClaimDriver.Value > 0).Sum(e => e.BebanClaimDriver.Value).ToString()) - dbAuj.Sum(f => f.PotonganK.Value);// *-1;
            //return saldo.Value;
        }

        public decimal GetSaldoPiutangBatalJalan(int id, string piutang_type)
        {
            List<DriverPiutangHistory> ListModel = new List<DriverPiutangHistory>();
            decimal? saldo = 0;
            if (piutang_type != "Klaim")
            {
                saldo = Repopbyd_det.TotalPiutangDriverPerType(id, piutang_type);
            }
            return saldo.Value;
        }

        public decimal GetSaldoPiutang(int id, string piutang_type)
        {
            decimal? saldo = 0;
            if (piutang_type != "Klaim")
            {
                saldo = Repopbyd_det.TotalPiutangDriver(id);
            }
            return saldo.Value;
        }

        public string GetKasbon(int id)
        {
            return new JavaScriptSerializer().Serialize(new
            {
                PiutangBatalJalan = GetSaldoPiutangBatalJalan(id + 7000000, "B"),
                PiutangPribadi = GetSaldoPiutangBatalJalan(id + 7000000, null),
                PiutangTabungan = GetSaldoPiutangBatalJalan(id + 7000000, "T")
            });
        }

        [MyAuthorize(Menu = "Admin Uang Jalan", Action = "update")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            ViewBag.driverId = dbitem.DriverId;
            ViewBag.driverName = dbitem.Driver.KodeDriver + " - " + dbitem.Driver.NamaDriver;
            ViewBag.KodeDriver1Old = dbitem.Driver.KodeDriverOld;

            ViewBag.TotalKasbon = GetSaldoPiutang(dbitem.DriverId.Value + 7000000, "Kasbon");
            ViewBag.TotalKlaim = GetSaldoKlaim(dbitem.DriverId.Value, "Klaim");
            ViewBag.PiutangBatalJalan = GetSaldoPiutangBatalJalan(dbitem.DriverId.Value + 7000000, "B");
            ViewBag.PiutangPribadi = GetSaldoPiutangBatalJalan(dbitem.DriverId.Value + 7000000, null);
            ViewBag.PiutangTabungan = GetSaldoPiutangBatalJalan(dbitem.DriverId.Value + 7000000, "T");

            if (RepoBatalOrder.FindBySO(dbitem.Id) != null)
                ViewBag.KeteranganBatalTruk = RepoBatalOrder.FindBySO(dbitem.Id).Keterangan;
            if (RepoRevisiRute.FindBySO(dbitem.Id) != null)
                ViewBag.KeteranganRevisiRute = RepoRevisiRute.FindBySO(dbitem.Id).Keterangan;
            if (RepoRevisiTanggal.FindBySo(dbitem.Id) != null)
                ViewBag.KeteranganRevisiTanggal = RepoRevisiTanggal.FindBySo(dbitem.Id).Keterangan;
            if (RepoRevisiJenisTruk.FindBySo(dbitem.Id) != null)
                ViewBag.KeteranganRevisiJenisTruk = RepoRevisiJenisTruk.FindBySo(dbitem.Id).Keterangan;
            if (RepoRevisiTanggal.FindBySoAndBP(dbitem.Id) != null)
                ViewBag.KeteranganTanggalBP = RepoRevisiTanggal.FindBySo(dbitem.Id).Keterangan;
            AdminUangJalan model = new AdminUangJalan();
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            GenerateModel(model, dbitem);

            if (model.ModelListRemoval.Count > 0)
            {
                model.ModelListRemoval = model.ModelListRemoval.OrderBy(d => d.Id).ToList();
                return View("~/Views/Removal/FormRemoval.cshtml", model);
            }
            else
                return View("Form", model);
        }
        public ActionResult EditKonsolidasi(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            ViewBag.driverId = dbitem.SalesOrderProsesKonsolidasi.Driver1Id;
            ViewBag.driverName = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver + " - " + dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
            ViewBag.PiutangBatalJalan = GetSaldoPiutangBatalJalan(dbitem.SalesOrderProsesKonsolidasi.Driver1Id.Value + 7000000, "B");
            ViewBag.PiutangPribadi = GetSaldoPiutangBatalJalan(dbitem.SalesOrderProsesKonsolidasi.Driver1Id.Value + 7000000, null);
            ViewBag.PiutangTabungan = GetSaldoPiutangBatalJalan(dbitem.SalesOrderProsesKonsolidasi.Driver1Id.Value + 7000000, "T");
            Context.SettlementBatal sb = RepoSettlementBatal.FindAll().Where(
                d => d.IsProses == true && d.IdAdminUangJalan == null && d.SalesOrder != null && d.IdDriver == ViewBag.driverId
            ).FirstOrDefault();
            ViewBag.SisaPerjalananSblmny = 0;

            if (sb != null)
            {
                if (sb.SalesOrder.SalesOrderOncallId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderOncall.SONumber;
                else if (sb.SalesOrder.SalesOrderPickupId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderPickup.SONumber;
                else if (sb.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                else if (sb.SalesOrder.SalesOrderKontrakId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderKontrak.SONumber;
            }
            AdminUangJalan model = new AdminUangJalan();
            Context.AdminUangJalan auj = RepoAdminUangJalan.FindBySalesOrderId(dbitem.Id);
            if (dbitem.AdminUangJalanId.HasValue)
            {
                model = new AdminUangJalan(dbitem.AdminUangJalan);
                auj = dbitem.AdminUangJalan;
            }
            try
            {
                ViewBag.CountKonsol = auj.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer Tambahan Konsolidasi")).Count() + 1;
            }
            catch (Exception)
            {
                ViewBag.CountKonsol = 1;
            }

            model.IdSalesOrder = dbitem.Id;
            GenerateModel(model, dbitem);
            return View("FormKonsolidasi", model);
        }
        [MyAuthorize(Menu = "Admin Uang Jalan", Action = "update")]
        public ActionResult EditKontrak(int id, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            if (dbitem == null)
                dbitem = RepoSalesOrder.FindByKontrak(id);
            //ambil admin uang jalan nya dari listSo yang pertama
            List<int> ListIdDumy = listSo.Split('.').ToList().Select(int.Parse).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
            AdminUangJalan model = new AdminUangJalan();
            ViewBag.KodeDriver1Old = dbsoDummy.FirstOrDefault().Driver1.KodeDriverOld;

            if (dbsoDummy.FirstOrDefault().IdAdminUangJalan.HasValue)
                model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            GenerateModel(model, dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = model.ModelKontrak.ListModelSOKontrak;
            ViewBag.TanggalAwal = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => dbsoDummy.Select(e => e.MuatDate).Min() == d.MuatDate).FirstOrDefault();
            ViewBag.PiutangBatalJalan = GetSaldoPiutangBatalJalan(dbsoDummy.FirstOrDefault().Driver1Id.Value + 7000000, "B");
            ViewBag.PiutangPribadi = GetSaldoPiutangBatalJalan(dbsoDummy.FirstOrDefault().Driver1Id.Value + 7000000, null);
            ViewBag.PiutangTabungan = GetSaldoPiutangBatalJalan(dbsoDummy.FirstOrDefault().Driver1Id.Value + 7000000, "T");

            return View("FormKontrak", model);
        }

        [MyAuthorize(Menu = "Admin Uang Jalan", Action = "read")]
        public ActionResult View(int id)
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
                ViewBag.Title = "Admin Uang Jalan " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
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
                ViewBag.Title = "Admin Uang Jalan " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("View", model);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                return View("Form", model);
            }

            return View("");
        }
        [MyAuthorize(Menu = "Admin Uang Jalan", Action = "read")]
        public ActionResult ViewKontrak(int id, string listSo)
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
            GenerateModel(model, dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = model.ModelKontrak.ListModelSOKontrak;

            return View("ViewKontrak", model);
        }

        [HttpPost]
        public ActionResult Edit(AdminUangJalan model, string btnsave, string maju1Flow, int? Driver1IdPiutang=null)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            string codeBT = "BT-" + dbitem.SONumber;
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
            Context.AdminUangJalan db = new Context.AdminUangJalan();
            AdminUangJalanVoucherSpbu[] resSpbu = JsonConvert.DeserializeObject<AdminUangJalanVoucherSpbu[]>(model.StrSolar);
            model.ModelListSpbu = resSpbu.ToList();
            AdminUangJalanVoucherKapal[] resKapal = JsonConvert.DeserializeObject<AdminUangJalanVoucherKapal[]>(model.StrKapal);
            model.ModelListKapal = resKapal.ToList();
            AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
            model.ModelListTf = resUang.ToList();
            if (RepoAuditrail.getKasirHistoryUnder1Minutes(dbitem, DateTime.Now.AddMinutes(-1)) != null) //keklik (kesubmit) 2 kali sama usernya
                return RedirectToAction("Index");
            else if (validation(model))
            {
                int idAdm = 0;
                if (dbitem.AdminUangJalanId.HasValue)
                {
                    idAdm = dbitem.AdminUangJalanId.Value;
                    model.setDb(dbitem.AdminUangJalan);
                }
                else
                {
                    model.setDb(db);
                    db.Code = "AUJ-" + dbitem.SONumber;
                    dbitem.AdminUangJalan = db;
                }
                string code = db.Code + "-" + urutanBatal;
                string codeIKS = "AUJ-" + dbitem.SONumber + "-" + urutanBatal;
                db.SalesOrderId = dbitem.Id;
                if (model.IdDriver1 != model.IdDriverOld1)
                {
                    if (dbitem.SalesOrderOncallId.HasValue)
                        dbitem.SalesOrderOncall.Driver1Id = model.IdDriver1;
                    else if (dbitem.SalesOrderPickupId.HasValue)
                        dbitem.SalesOrderPickup.Driver1Id = model.IdDriver1;
                    else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                        dbitem.SalesOrderProsesKonsolidasi.Driver1Id = model.IdDriver1;
                    dbitem.DriverId = model.IdDriver1;
                }
                if (btnsave == "Submit")
                {
                    db.Status = "Sudah";
                    Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                    decimal? tambahanRute = db.AdminUangJalanTambahanRute.Sum(s => s.values);
                    decimal? boronganDasar = db.TotalBorongan - (db.Kawalan ?? 0) - (db.Timbangan ?? 0) - (db.Karantina ?? 0) - (db.SPSI ?? 0) - (db.Multidrop ?? 0) - tambahanRute - db.AdminUangJalanTambahanLain.Sum(s => s.Values);
                    decimal? total_potongan_driver = (db.AdminUangJalanPotonganDriver.Sum(s => s.Value) + db.KasbonDriver1);
                    if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        foreach (var item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                        {
                            decimal persentase = RepoSalesOrder.HargaKonsolidasiPerItem(item) / RepoSalesOrder.Harga(dbitem);
                            Context.Customer supplier = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.SupplierId.Value);
                            Context.SalesOrder sok = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                            sok.SalesOrderKonsolidasi.ProcessedByAUJ = true;
                            string customer = supplier.CustomerCodeOld + "-" + supplier.CustomerNama;
                            Repoglt_det.saveFromAc(1, code, persentase * boronganDasar, 0, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, "Biaya Borongan - " + customer + " - " + sok.SONumber, null, null, supplier.Id, codeIKS);
                            Repoglt_det.saveFromAc(2, code, persentase * db.Kawalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, "Biaya Kawalan - " + customer + " - " + sok.SONumber, null, null, supplier.Id, codeIKS);
                            Repoglt_det.saveFromAc(3, code, persentase * db.Timbangan, 0, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, "Biaya Timbangan - " + customer + " - " + sok.SONumber, null, null, supplier.Id, codeIKS);
                        }
                    }
                    else
                    {
                        Repoglt_det.saveFromAc(1, code, boronganDasar, 0, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, null, null, null, codeIKS);
                        Repoglt_det.saveFromAc(2, code, db.Kawalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, null, null, null, codeIKS);
                        Repoglt_det.saveFromAc(3, code, db.Timbangan, 0, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, null, null, null, codeIKS);
                    }
                    Repoglt_det.saveFromAc(4, code, db.Karantina, 0, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, null, null, null, codeIKS);
                    Repoglt_det.saveFromAc(5, code, db.SPSI, 0, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, null, null, null, codeIKS);
                    Repoglt_det.saveFromAc(6, code, db.Multidrop, 0, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, null, null, null, codeIKS);
                    int idx = 8;
                    foreach (AdminUangJalanTambahanLain aujtl in model.ModelListTambahanLain.Where(d => d.IsDelete == false))
                    {
                        Repoglt_det.saveFromAc(idx, code, aujtl.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, aujtl.Keterangan, null, null, null, codeIKS);
                        idx++;
                    }
                    foreach (AdminUangJalanTambahanRute aujtl in model.ModelListTambahanRute.Where(d => d.IsDelete == false))
                    {
                        aujtl.NamaDataBorongan = RepoDataBorongan.FindByPK(aujtl.IdDataBorongan.Value).NamaBorongan;
                        Repoglt_det.saveFromAc(idx, code, aujtl.value, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, aujtl.NamaDataBorongan, null, null, null, codeIKS);
                        idx++;
                    }
                    decimal? aujCredit = (db.KasbonDriver1 ?? 0) + (db.KlaimDriver1 ?? 0) + db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() + db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() +
                        db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() + db.AdminUangJalanUangTf.Select(d => d.Value).Sum() + (db.uangDM ?? 0);
                    Repoglt_det.saveFromAc(8, code, 0, aujCredit, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, null, null, null, codeIKS);
                    var glt_oid = Guid.NewGuid().ToString();
                    string codePby = code;
                    if (total_potongan_driver > 0)
                    {
                        if (db.PotonganK > 0)
                        {
                            total_potongan_driver = total_potongan_driver - db.PotonganK;
                            db.KlaimSubmittedAt = DateTime.Now;
                        }
                        if (db.PotonganB > 0)
                        {
                            Repopbyd_det.saveMstr(glt_oid, codePby + "-B", 0, "Potongan Kasbon Batal Jalan " + code, db.IdDriver1.Value + 7000000);
                            Repopbyd_det.save(glt_oid, codePby + "-B", 0, "Potongan Kasbon Batal Jalan" + db.Code, db.IdDriver1.Value + 7000000, 0, "", db.PotonganB.Value * -1);
                        }
                        if (db.PotonganP > 0)
                        {
                            glt_oid = Guid.NewGuid().ToString();
                            Repopbyd_det.saveMstr(glt_oid, codePby + "-P", 0, "Potongan Kasbon Pribadi " + code, db.IdDriver1.Value + 7000000);
                            Repopbyd_det.save(glt_oid, codePby + "-P", 0, "Potongan Kasbon Pribadi " + db.Code, db.IdDriver1.Value + 7000000, 0, "", db.PotonganP.Value * -1, "P");
                        }
                        if (db.PotonganT > 0)
                        {
                            glt_oid = Guid.NewGuid().ToString();
                            Repopbyd_det.saveMstr(glt_oid, codePby + "-T", 0, "Tabungan " + code, db.IdDriver1.Value + 7000000);
                            Repopbyd_det.save(glt_oid, codePby+"-T", 0, "Tabungan " + db.Code, db.IdDriver1.Value + 7000000, 0, "", db.PotonganT.Value * -1, "T");
                        }
                    }

                    dbitem.Status = "admin uang jalan";
                }
                dbitem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbitem);
                //simpan history driver dan truck
                Context.HistoryJalanTruck dbhistruck = RepoHistoryJalanTruck.FindByAdm(dbitem.AdminUangJalanId.Value);
                if (btnsave == "Submit")
                {
                    if (dbhistruck == null)
                    {
                        dbhistruck = new Context.HistoryJalanTruck();
                    }
                    dbhistruck.IdAdminUangJalan = dbitem.AdminUangJalanId.Value;
                    dbhistruck.IdDriver1 = dbitem.AdminUangJalan.IdDriver1.Value;
                    dbhistruck.IdDriver2 = dbitem.AdminUangJalan.IdDriver2;
                    if (dbitem.SalesOrderOncallId.HasValue)
                    {
                        dbhistruck.IdTruck = dbitem.SalesOrderOncall.IdDataTruck.Value;
                        dbhistruck.ShipmentId = dbitem.SalesOrderOncall.DN;
                        dbhistruck.NoSo = dbitem.SalesOrderOncall.SONumber;
                        dbhistruck.TanggalMuat = dbitem.SalesOrderOncall.TanggalMuat.Value;
                        dbhistruck.JenisOrder = dbitem.SalesOrderOncall.PrioritasId.HasValue ? dbitem.SalesOrderOncall.LookUpPrioritas.Nama : "Oncall";
                        dbhistruck.Rute = dbitem.SalesOrderOncall.StrDaftarHargaItem;
                    }
                    else if (dbitem.SalesOrderPickupId.HasValue)
                    {
                        dbhistruck.IdTruck = dbitem.SalesOrderPickup.IdDataTruck.Value;
                        dbhistruck.ShipmentId = dbitem.SalesOrderPickup.SONumber;
                        dbhistruck.TanggalMuat = dbitem.SalesOrderPickup.TanggalPickup;
                        dbhistruck.JenisOrder = "Pickup";
                        dbhistruck.Rute = dbitem.SalesOrderPickup.Rute.Nama;
                    }
                    else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        dbhistruck.IdTruck = dbitem.SalesOrderProsesKonsolidasi.IdDataTruck.Value;
                        dbhistruck.ShipmentId = dbitem.SalesOrderProsesKonsolidasi.DN;
                        dbhistruck.NoSo = dbitem.SalesOrderProsesKonsolidasi.SONumber;
                        dbhistruck.TanggalMuat = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat.Value;
                        dbhistruck.JenisOrder = "Konsolidasi";
                        dbhistruck.Rute = dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                    }
                    Context.SettlementBatal sb = RepoSettlementBatal.FindAll().Where(
                        d => d.IsProses == true && d.IdAdminUangJalan == null && dbitem.AdminUangJalan.IdDriver1.Value == d.IdDriver
                    ).FirstOrDefault();
                    if (sb != null)
                    {
                        sb.IdAdminUangJalan = dbitem.AdminUangJalanId;
                        RepoSettlementBatal.save(sb, UserPrincipal.id, "Admin Uang Jalan");
                    }
                    RepoHistoryJalanTruck.save(dbhistruck);
                    if (maju1Flow == "Y" || db.uangDM > 0 || db.TotalAlokasi == 0)
                        Maju1Flow(dbitem.Id);
                    if (db.uangDM > 0)
                    {
                        var glt_oid = Guid.NewGuid().ToString();
                        Repopbyd_det.saveMstr(glt_oid, "DM-" + code, 0, "Diambil DM " + code, Driver1IdPiutang.Value + 7000000);
                        Repopbyd_det.save(glt_oid, "DM-" + code, 0, "Diambil DM " + db.Code, Driver1IdPiutang.Value + 7000000, 0, "", db.uangDM.Value * -1);
                    }
                }
                RepoAuditrail.saveAUJHistory(dbitem, dbhistruck);
                return RedirectToAction("Index");
            }
            if (model.ModelOncall != null)
                ViewBag.Title = "Admin Uang Jalan " + model.ModelOncall.SONumber;
            if (model.ModelPickup != null)
                ViewBag.Title = "Admin Uang Jalan " + model.ModelPickup.SONumber;
            if (model.ModelKonsolidasi != null)
                ViewBag.Title = "Admin Uang Jalan " + model.ModelKonsolidasi.SONumber;
            ViewBag.Status = dbitem.Status.ToLower().Contains("konfirmasi") || dbitem.Status == "save planning" ? "data borongan belum diset" : dbitem.Status.ToLower().Contains("admin uang jalan") ?
                "sudah diproses" : "batal";
            GenerateModel(model, dbitem);
            return View("Form", model);
        }

        public string NyusulinTabungan()
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByCode("OC-1801-4821");
            string codeBT = "BT-" + dbitem.SONumber;
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
            Context.AdminUangJalan db = new Context.AdminUangJalan();
                int idAdm = 0;
                if (dbitem.AdminUangJalanId.HasValue)
                    idAdm = dbitem.AdminUangJalanId.Value;
                else
                {
                    db.Code = "AUJ-" + dbitem.SONumber;
                    dbitem.AdminUangJalan = db;
                }
                string code = db.Code + "-" + urutanBatal;
                string codeIKS = "AUJ-" + dbitem.SONumber + "-" + urutanBatal;
                db.PotonganT = 711986;
                RepoAdminUangJalan.save(db);
            var glt_oid = Guid.NewGuid().ToString();
                    string codePby = code;
                            glt_oid = Guid.NewGuid().ToString();
                            Repopbyd_det.saveMstr(glt_oid, codePby + "-T", 0, "Tabungan " + code, 337 + 7000000);
                            Repopbyd_det.save(glt_oid, codePby + "-T", 0, "Tabungan " + db.Code, 337 + 7000000, 0, "", -711986, "T");
                            return "aa";
        }

        [HttpPost]
        public ActionResult EditKonsolidasi(AdminUangJalan model, string btnsave, string maju1Flow)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            string codeBT = "BT-" + dbitem.SONumber;
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
            Context.AdminUangJalan db = dbitem.AdminUangJalan;
            AdminUangJalanVoucherSpbu[] resSpbu = JsonConvert.DeserializeObject<AdminUangJalanVoucherSpbu[]>(model.StrSolar);
            model.ModelListSpbu = resSpbu.ToList();
            AdminUangJalanVoucherKapal[] resKapal = JsonConvert.DeserializeObject<AdminUangJalanVoucherKapal[]>(model.StrKapal);
            model.ModelListKapal = resKapal.ToList();
            AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
            model.ModelListTf = resUang.ToList();
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            if (RepoAuditrail.getKasirHistoryUnder1Minutes(dbitem, DateTime.Now.AddMinutes(-1)) != null) //keklik (kesubmit) 2 kali sama usernya
                return RedirectToAction("Index");
            else if (validationKonsolidasi(model))
            {
                int idAdm = 0;
                idAdm = dbitem.AdminUangJalanId.Value;
                model.setDbTambahanKonsolidasi(dbitem.AdminUangJalan);
                string code = db.Code + "-" + urutanBatal;
                string codeIKS = "AUJ-" + dbitem.SONumber + "-" + urutanBatal;
                db.SalesOrderId = dbitem.Id;
                if (btnsave == "Submit")
                {
                    decimal? boronganDasar = db.NilaiBorongan;
                    foreach (var item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                    {
                        Context.Customer supplier = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.SupplierId.Value);
                        Context.SalesOrder sok = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                        decimal persentase = RepoSalesOrder.HargaKonsolidasiPerItem(item) / RepoSalesOrder.Harga(dbitem);
                        sok.SalesOrderKonsolidasi.ProcessedByAUJ = true;
                        string customer = supplier.CustomerCodeOld + "-" + supplier.CustomerNama;
                        string KSOCode = "AUJ-" + dbitem.SONumber + ", " + sok.SONumber;
                        List<Context.glt_det> glt_dets = Repoglt_det.FindAllByglt_codeAndglt_ref_detail_no(KSOCode, codeIKS);
                        queryManualERP("DELETE FROM glt_det WHERE glt_desc IN ('Biaya Borongan - " + customer + " - " + sok.SONumber + "', 'Biaya Kawalan - " + customer + " - " + sok.SONumber + "', 'Biaya Timbangan - " + customer + " - " + sok.SONumber + "')");
                        Repoglt_det.saveFromAc(1, KSOCode, persentase * boronganDasar, 0, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, "Biaya Borongan - " + customer + " - " + sok.SONumber, null, null, supplier.Id, codeIKS);
                        Repoglt_det.saveFromAc(2, KSOCode, persentase * db.Kawalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, "Biaya Kawalan - " + customer + " - " + sok.SONumber, null, null, supplier.Id, codeIKS);
                        Repoglt_det.saveFromAc(3, KSOCode, persentase * db.Timbangan, 0, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, "Biaya Timbangan - " + customer + " - " + sok.SONumber, null, null, supplier.Id, codeIKS);
                    }
                    foreach (AdminUangJalanTambahanRute aujtl in model.ModelListTambahanRute.Where(d => d.IsDelete == false))
                    {
                        if (aujtl.IdDataBorongan != null)
                        {
                            aujtl.NamaDataBorongan = RepoDataBorongan.FindByPK(aujtl.IdDataBorongan.Value).NamaBorongan;
                            Repoglt_det.saveFromAc(1, code, aujtl.value, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, aujtl.NamaDataBorongan, null, null, null, codeIKS);
                            Repoglt_det.saveFromAc(700000, code, 0, aujtl.value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, null, null, null, codeIKS);
                        }
                    }
                }
                dbitem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbitem);
                Context.HistoryJalanTruck dbhistruck = RepoHistoryJalanTruck.FindByAdm(dbitem.AdminUangJalanId.Value);
                RepoAuditrail.saveAUJHistory(dbitem, dbhistruck);
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Admin Uang Jalan " + model.ModelKonsolidasi.SONumber;
            GenerateModel(model, dbitem);
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult EditKonsolidasiNoEditBorongan(AdminUangJalan model, string btnsave, string maju1Flow)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            string codeBT = "BT-" + dbitem.SONumber;
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
            Context.AdminUangJalan db = dbitem.AdminUangJalan;
            AdminUangJalanVoucherSpbu[] resSpbu = JsonConvert.DeserializeObject<AdminUangJalanVoucherSpbu[]>(model.StrSolar);
            model.ModelListSpbu = resSpbu.ToList();
            AdminUangJalanVoucherKapal[] resKapal = JsonConvert.DeserializeObject<AdminUangJalanVoucherKapal[]>(model.StrKapal);
            model.ModelListKapal = resKapal.ToList();
            AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
            model.ModelListTf = resUang.ToList();
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            if (RepoAuditrail.getKasirHistoryUnder1Minutes(dbitem, DateTime.Now.AddMinutes(-1)) != null) //keklik (kesubmit) 2 kali sama usernya
                return RedirectToAction("Index");
            else if (validationKonsolidasi(model))
            {
                int idAdm = 0;
                idAdm = dbitem.AdminUangJalanId.Value;
                model.setDbTambahanKonsolidasi(dbitem.AdminUangJalan);
                string code = db.Code + "-" + urutanBatal;
                string codeIKS = "AUJ-" + dbitem.SONumber + "-" + urutanBatal;
                db.SalesOrderId = dbitem.Id;
                if (btnsave == "Submit")
                {
                    foreach (AdminUangJalanTambahanRute aujtl in model.ModelListTambahanRute.Where(d => d.IsDelete == false))
                    {
                        if (aujtl.IdDataBorongan != null)
                        {
                            aujtl.NamaDataBorongan = RepoDataBorongan.FindByPK(aujtl.IdDataBorongan.Value).NamaBorongan;
                            Repoglt_det.saveFromAc(1, code, aujtl.value, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, aujtl.NamaDataBorongan, null, null, null, codeIKS);
                            Repoglt_det.saveFromAc(700000, code, 0, aujtl.value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, null, null, null, codeIKS);
                        }
                    }
                }
                RepoSalesOrder.save(dbitem);
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Admin Uang Jalan " + model.ModelKonsolidasi.SONumber;
            GenerateModel(model, dbitem);
            return View("Form", model);
        }

        public void Maju1Flow(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);
            if (dbso.Status == "admin uang jalan" && !dbso.AdminUangJalan.AdminUangJalanUangTf.Any(d => d.JumlahTransfer > 0))
            {
                string strQuery = "";
                Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    foreach (var item in dbso.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                    {
                        Context.Customer supplier = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.NamaTagihanId.Value);
                        Context.SalesOrder sok = RepoSalesOrder.FindByKonsolidasi(item.SalesOrderKonsolidasiId);
                        Context.Dokumen dokumen = RepoDokumen.FindBySONumber(sok.SONumber);
                        if (dokumen == null)
                            strQuery = GenerateDokumen(sok.Id, supplier);
                    }
                }
                else if (dbso.SalesOrderOncallId.HasValue)
                {
                    Context.Dokumen dokumen = RepoDokumen.FindBySONumber(dbso.SONumber);
                    if (dokumen == null)
                        strQuery = GenerateDokumen(idSo, dbso.SalesOrderOncall.Customer);
                }
                dbso.Status = "dispatched";
                dbso.UpdatedBy = UserPrincipal.id;
                Context.AdminUangJalan db = dbso.AdminUangJalan;
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
                nominalHutangUangJalanDriver += db.uangDM == null ? 0 : db.uangDM.Value;
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
                if (db.uangDM > 0)
                {//Uang Dari DM
                    idx++;
                    Repoglt_det.saveFromAc(idx, code, 0, db.uangDM, Repoac_mstr.FindByPk(erpConfig.IdUangDM), dbso);
                }
                db.JurnalVoucher = true;
                RepoAuditrail.saveMajuHistory(dbso, strQuery);
                RepoSalesOrder.save(dbso);
            }
        }

        public void Maju1FlowKontrak(int idSo)
        {
            Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(idSo);
            Context.SalesOrder dbso = RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value);
            Context.Dokumen dokumen = RepoDokumen.FindBySONumber(sokls.NoSo);
            string strQuery = null;
            if (dokumen == null)
                strQuery = GenerateDokumen(dbso.Id, dbso.SalesOrderKontrak.Customer, sokls.Id.ToString());
            sokls.Status = "dispatched";
            sokls.StatusFlow = "DISPATCHED";
            dbso.UpdatedBy = UserPrincipal.id;
            RepoAuditrail.saveMajuHistory(dbso, strQuery);
            RepoSalesOrder.save(dbso);
            queryManual("UPDATE dbo.\"SalesOrderKontrakListSo\" SET \"Status\"='dispatched', \"StatusFlow\"='DISPATCHED' WHERE \"Id\"=" + sokls.Id);
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            Context.AdminUangJalan db = sokls.AdminUangJalan;
            int idx = 0;
            if (db == null)
                db = RepoAdminUangJalan.FindAll().Where(d => d.SONumber != null && d.SONumber.Contains(sokls.NoSo) && d.Status != "Batal").FirstOrDefault();
            string soNumber = db.SONumber;
            Context.SalesOrderKontrakTruck salesOrderKontrakTruck = dbso.SalesOrderKontrak.SalesOrderKontrakTruck.Where(d => d.Id == sokls.SalesKontrakTruckId).FirstOrDefault();
            if (salesOrderKontrakTruck.PendapatanDiakui != true && DateTime.Now.Month.ToString("00").PadLeft(2, '0') == sokls.MuatDate.Month.ToString("00").PadLeft(2, '0'))
            {
                Repoglt_det.saveFromAc(1, "PD-" + soNumber, RepoSalesOrder.Harga(dbso, sokls), 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbso, null, sokls);
                Repoglt_det.saveFromAc(2, "PD-" + soNumber, 0, RepoSalesOrder.Harga(dbso, sokls), Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbso, null, sokls);
                salesOrderKontrakTruck.PendapatanDiakui = true;
            }
            if (db.JurnalVoucher != true)
            {
                db.JurnalVoucher = true;
                decimal nominalHutangUangJalanDriver = 0;
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum().ToString());
                nominalHutangUangJalanDriver += db.KasbonDriver1 == null ? 0 : db.KasbonDriver1.Value;
                nominalHutangUangJalanDriver += db.KlaimDriver1 == null ? 0 : db.KlaimDriver1.Value;
                nominalHutangUangJalanDriver += decimal.Parse(db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum().ToString());

                Repoglt_det.saveFromAc(1, "MF-" + soNumber, nominalHutangUangJalanDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso, null, sokls);//Hutang Uang Jalan Driver
                foreach (Context.AdminUangJalanVoucherSpbu aujvs in db.AdminUangJalanVoucherSpbu)
                {
                    Context.LookupCode spbu = RepoLookup.FindByName(aujvs.Keterangan);
                    idx++;
                    Repoglt_det.saveFromAc(idx, "MF-" + soNumber, 0, aujvs.Value, Repoac_mstr.FindByPk(spbu.ac_id), dbso, aujvs.Keterangan, sokls);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
                }
                foreach (Context.AdminUangJalanVoucherKapal aujvk in db.AdminUangJalanVoucherKapal)
                {
                    Context.LookupCode kapal = RepoLookup.FindByName(aujvk.Keterangan);
                    idx++;
                    Repoglt_det.saveFromAc(idx, "MF-" + soNumber, 0, aujvk.Value, Repoac_mstr.FindByPk(RepoLookup.FindByName(aujvk.Keterangan).ac_id), dbso, aujvk.Keterangan, sokls);//HUTANG SPBU 34.171.04 Pangkalan 2 dan atau
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
                        var query = RepoSalesOrder.saveQueryGrMstrKontrak(UserPrincipal.username, customer, sokls.DataTruck.VehicleNo, decimal.Parse(aujvs.Value.ToString()));
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
                        var query = RepoSalesOrder.saveQueryGrMstr(UserPrincipal.username, customer, dbso, decimal.Parse(aujvk.Value.ToString()));
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
                    Repoglt_det.saveFromAc(idx, "MF-" + soNumber, 0, db.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbso, null, sokls);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganP > 0)
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "MF-" + soNumber, 0, db.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbso, null, sokls);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                }
                if (db.PotonganK > 0)
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "MF-" + soNumber, 0, db.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbso, null, sokls);
                }
                if (db.PotonganT > 0)
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "MF-" + soNumber, 0, db.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbso, null, sokls);
                }
                if (!(db.PotonganT > 0 || db.PotonganK > 0 || db.PotonganP > 0 || db.PotonganB > 0))
                {
                    idx++;
                    Repoglt_det.saveFromAc(idx, "MF-" + soNumber, 0, db.KasbonDriver1, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbso, null, sokls);
                }
            }
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
            }
            else if (so != null)
            {
                dbdokumen.SONumber = so.SONumber;
                dbdokumen.DataTruckId = so.DataTruckId;
                dbdokumen.DriverId = so.DriverId;
            }
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

        [HttpPost]
        public ActionResult EditRemoval(RemovalAUJ model, int idRev)
        {
            AdminUangJalanVoucherSpbu[] resSpbu = JsonConvert.DeserializeObject<AdminUangJalanVoucherSpbu[]>(model.StrSolar);
            model.ModelListSpbu = resSpbu.ToList();
            AdminUangJalanVoucherKapal[] resKapal = JsonConvert.DeserializeObject<AdminUangJalanVoucherKapal[]>(model.StrKapal);
            model.ModelListKapal = resKapal.ToList();
            AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
            model.ModelListTf = resUang.ToList();
            if (ModelState.IsValid)
            {
                if (validationRemoval(model))
                {
                    Context.Removal dbitem = RepoRemovalRepo.FindByPK(idRev);
                    model.setDb(dbitem);
                    dbitem.Status = "admin uang jalan";
                    RepoRemovalRepo.save(dbitem);
                    return RedirectToAction("Index", "AdminUangJalan");
                }
            }

            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSO.Value);
            if (dbso.SalesOrderOncallId.HasValue)
            {
                ViewBag.driverId = dbso.SalesOrderOncall.Driver1Id;
                ViewBag.driverName = dbso.SalesOrderOncall.Driver1.KodeDriver + " - " + dbso.SalesOrderOncall.Driver1.NamaDriver;
            }
            else if (dbso.SalesOrderPickupId.HasValue)
            {
                ViewBag.driverId = dbso.SalesOrderPickup.Driver1Id;
                ViewBag.driverName = dbso.SalesOrderPickup.Driver1.KodeDriver + " - " + dbso.SalesOrderPickup.Driver1.NamaDriver;
            }
            else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.driverId = dbso.SalesOrderProsesKonsolidasi.Driver1Id;
                ViewBag.driverName = dbso.SalesOrderProsesKonsolidasi.Driver1.KodeDriver + " - " + dbso.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
            }
            Context.SettlementBatal sb = RepoSettlementBatal.FindAll().Where(
                d => d.IsProses == true && d.IdAdminUangJalan == null && d.SalesOrder != null &&
                (
                    d.SalesOrder.SalesOrderOncallId.HasValue && (d.SalesOrder.SalesOrderOncall.Driver1Id == ViewBag.driverId || d.SalesOrder.SalesOrderOncall.Driver2Id == ViewBag.driverId ||
                    d.SalesOrder.SalesOrderOncall.Driver2Id == ViewBag.driverId || d.SalesOrder.SalesOrderOncall.Driver1Id == ViewBag.driverId) ||
                    d.SalesOrder.SalesOrderPickupId.HasValue && (d.SalesOrder.SalesOrderPickup.Driver1Id == ViewBag.driverId || d.SalesOrder.SalesOrderPickup.Driver2Id == ViewBag.driverId ||
                    d.SalesOrder.SalesOrderPickup.Driver2Id == ViewBag.driverId || d.SalesOrder.SalesOrderPickup.Driver1Id == ViewBag.driverId) ||
                    d.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrder.SalesOrderProsesKonsolidasi.Driver1Id == ViewBag.driverId ||
                    d.SalesOrder.SalesOrderProsesKonsolidasi.Driver2Id == ViewBag.driverId || d.SalesOrder.SalesOrderProsesKonsolidasi.Driver2Id == ViewBag.driverId ||
                    d.SalesOrder.SalesOrderProsesKonsolidasi.Driver1Id == ViewBag.driverId)
                )
            ).FirstOrDefault();
            ViewBag.SisaPerjalananSblmny = 0;
            if (sb != null)
            {
                ViewBag.SisaPerjalananSblmny += sb.TransferSelisih == null ? 0 : sb.TransferSelisih;
                ViewBag.SisaPerjalananSblmny += sb.KasSelisih == null ? 0 : sb.KasSelisih;
                ViewBag.SisaPerjalananSblmny += sb.SolarSelisih == null ? 0 : sb.SolarSelisih;
                if (sb.SalesOrder.SalesOrderOncallId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderOncall.SONumber;
                else if (sb.SalesOrder.SalesOrderPickupId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderPickup.SONumber;
                else if (sb.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
            }
            AdminUangJalan modelauj = new AdminUangJalan();
            if (dbso.AdminUangJalanId.HasValue)
                modelauj = new AdminUangJalan(dbso.AdminUangJalan);

            modelauj.IdSalesOrder = dbso.Id;
            GenerateModel(modelauj, dbso);

            var dummyModel = modelauj.ModelListRemoval.Where(d => d.Id == model.Id).FirstOrDefault();
            var idx = modelauj.ModelListRemoval.IndexOf(dummyModel);
            modelauj.ModelListRemoval[idx] = model;

            return View("~/Views/Removal/FormRemoval.cshtml", model);
        }

        public string generateDataTruck()
        {
            foreach (Context.AdminUangJalan auj in RepoAdminUangJalan.FindAll().Where(d => d.DaftarHargaKontrakId != null && (d.DataTruckId == null || d.AUJTanggalMuat == null)))
            {
                if (RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id).FirstOrDefault() != null)
                {
                    auj.DataTruckId = RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id).FirstOrDefault().IdDataTruck;
                    auj.AUJTanggalMuat = RepoSalesOrderKontrakListSo.FindAllByAUJ(auj.Id).FirstOrDefault().MuatDate;
                    RepoAdminUangJalan.save(auj);
                }
            }
            return "aa";
        }

        [HttpPost]
        public ActionResult EditKontrak(AdminUangJalan model, string btnsave, string ListRuteId, string DaftarHargaKontrakId, string maju1Flow, int? Driver1IdPiutang)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.AdminUangJalan db = new Context.AdminUangJalan();
            SalesOrderKontrakListSo[] resSo = JsonConvert.DeserializeObject<SalesOrderKontrakListSo[]>(model.SelectedListIdSo);
            SalesOrderKontrakListSo[] resSelSo = JsonConvert.DeserializeObject<SalesOrderKontrakListSo[]>(model.SelectedListIdSo);
            AdminUangJalanVoucherSpbu[] resSpbu = JsonConvert.DeserializeObject<AdminUangJalanVoucherSpbu[]>(model.StrSolar);
            model.ModelListSpbu = resSpbu.ToList();
            AdminUangJalanVoucherKapal[] resKapal = JsonConvert.DeserializeObject<AdminUangJalanVoucherKapal[]>(model.StrKapal);
            model.ModelListKapal = resKapal.ToList();
            AdminUangJalanUangTf[] resUang = JsonConvert.DeserializeObject<AdminUangJalanUangTf[]>(model.StrUang);
            model.ModelListTf = resUang.ToList();
            List<int> SelectedSo = resSelSo.Select(d => d.Id).ToList();
            List<int> UnSelectedSo = resSo.Where(s => !SelectedSo.Contains(s.Id)).Select(d => d.Id).ToList();
            List<int> ListIdDumy = resSo.Select(d => d.Id).ToList();
            List<Context.SalesOrderKontrakListSo> dbsoDummy = RepoSalesOrderKontrakListSo.FindAllPerKontrak(ListIdDumy).ToList();
            db.SONumber = "";
            try{
            if (validation(model))
            {
                model.setDb(db);//bikin dulu data admin uang jalan
                int idAdm = 0;
                if (dbitem.AdminUangJalanId.HasValue)
                {
                    idAdm = dbitem.AdminUangJalanId.Value;
                    db.ListIdRute = ListRuteId;
                    db.DaftarHargaKontrakId = int.Parse(DaftarHargaKontrakId);
                }
                else
                {
                    model.setDb(db);
                    db.Code = "AUJ-" + dbitem.SalesOrderKontrak.SONumber;
                    db.ListIdRute = ListRuteId;
                    db.DaftarHargaKontrakId = int.Parse(DaftarHargaKontrakId);
                }
                foreach (Context.SalesOrderKontrakListSo sokls in dbsoDummy)
                {
                    Context.SalesOrderKontrakListSo sokls1 = RepoSalesOrderKontrakListSo.FindByPK(sokls.Id);
                    db.Code += "," + sokls1.NoSo;
                    db.SONumber += "," + sokls1.NoSo;
                }
                string codeBT = "BTB-" + dbitem.SONumber;
                int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
                string code = db.Code + "-" + urutanBatal;
                if (btnsave == "Submit")//setiap so isi admin uang jalannya
                {
                    Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                    decimal? tambahanRute = db.AdminUangJalanTambahanRute.Sum(s => s.values);
                    decimal? boronganDasar = db.TotalBorongan - (db.Kawalan ?? 0) - (db.Timbangan ?? 0) - (db.Karantina ?? 0) - (db.SPSI ?? 0) - (db.Multidrop ?? 0) - tambahanRute - db.AdminUangJalanTambahanLain.Sum(s => s.Values);
                    decimal? total_potongan_driver = (db.AdminUangJalanPotonganDriver.Sum(s => s.Value) + db.KasbonDriver1);
                    decimal? hutangDriver = (db.KasbonDriver1 ?? 0) + (db.KlaimDriver1 ?? 0) + db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                        db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                        db.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                    if (int.Parse(DateTime.Now.Month.ToString("00").PadLeft(2, '0')) >= int.Parse(dbsoDummy.FirstOrDefault().MuatDate.Month.ToString("00").PadLeft(2, '0')))
                    {
                        Repoglt_det.saveFromAc(1, db.Code, boronganDasar, 0, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, dbsoDummy.FirstOrDefault());
                        Repoglt_det.saveFromAc(2, db.Code, db.Kawalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, dbsoDummy.FirstOrDefault());
                        Repoglt_det.saveFromAc(3, db.Code, db.Timbangan, 0, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, dbsoDummy.FirstOrDefault());
                        Repoglt_det.saveFromAc(4, db.Code, db.Karantina, 0, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, dbsoDummy.FirstOrDefault());
                        Repoglt_det.saveFromAc(5, db.Code, db.SPSI, 0, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, dbsoDummy.FirstOrDefault());
                        Repoglt_det.saveFromAc(6, db.Code, db.Multidrop, 0, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, dbsoDummy.FirstOrDefault());
                        int idx = 8;
                        foreach (Context.AdminUangJalanTambahanRute aujtl in db.AdminUangJalanTambahanRute)
                        {
                            if (aujtl.DataBorongan != null)
                            {
                                Repoglt_det.saveFromAc(idx, db.Code, aujtl.values, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, aujtl.DataBorongan.NamaBorongan, dbsoDummy.FirstOrDefault());
                                idx++;
                            }
                        }
                        foreach (Context.AdminUangJalanTambahanLain aujtl in db.AdminUangJalanTambahanLain)
                        {
                            Context.LookupCode biaya = RepoLookup.FindByName(aujtl.Keterangan);
                            Repoglt_det.saveFromAc(idx, db.Code, aujtl.Values, 0, Repoac_mstr.FindByPk(biaya.ac_id), dbitem, aujtl.Keterangan, dbsoDummy.FirstOrDefault());
                            idx++;
                        }
                    }
                    else
                    {
                        Repoglt_det.saveFromAc(2, db.Code, hutangDriver, 0, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPosSementara), dbitem, null, dbsoDummy.FirstOrDefault());
                        db.PerluJurnalBalikPiutangPos = true;
                    }
                    Repoglt_det.saveFromAc(8, db.Code, 0, hutangDriver, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, dbsoDummy.FirstOrDefault());
                    var glt_oid_pby = Guid.NewGuid().ToString();
                    string codePby = "P-AUJ-"+dbsoDummy.Select(d => d.NoSo).Min() + '~' + dbsoDummy.Select(d => d.NoSo).Max();
                    int codePbyUrutan = Repopbyd_det.getUrutan(codePby)+1;
                    codePby = codePby + "-" + codePbyUrutan;
                    if (total_potongan_driver > 0)
                    {
                        Repopbyd_det.saveMstr(glt_oid_pby, codePby, 0, "Potongan " + code, db.IdDriver1.Value + 7000000);
                        Repopbyd_det.save(glt_oid_pby, codePby, 0, "Potongan " + db.Code, db.IdDriver1.Value + 7000000, 0, "", total_potongan_driver.Value * -1);
                    }
                    dbitem.Status = "admin uang jalan";
                    foreach (Context.SalesOrderKontrakListSo item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => SelectedSo.Contains(d.Id)))
                    {
                        item.Status = "admin uang jalan";
                        item.StatusFlow = "KASIR";
                        item.AdminUangJalan = db;
                    }
                    Context.DaftarHargaKontrak dhk = RepoDHKontrak.FindItemByPK(db.DaftarHargaKontrakId.Value).DaftarHargaKontrak;
                    if (dhk.LookUpTypeKontrak.Nama == "TYPE 2")
                    {
                        for (int i = 1; i <= dbitem.SalesOrderKontrak.JumlahTruck; i++ )
                        {
                            Context.SalesOrderKontrak sok = dbitem.SalesOrderKontrak;
                            Context.SalesOrderKontrakListSo sokls = sok.SalesOrderKontrakListSo.Where(d => d.NoSo.Contains("."+i)).FirstOrDefault();
                            if (sokls != null)
                            {
                                string codeSoMstr = dbitem.SalesOrderKontrak.SONumber + "." + sokls.NoSo.Split('.')[1];
                                Context.so_mstr presentSOInERP = Reposo_mstr.FindByPK(codeSoMstr);
                                if (presentSOInERP == null)
                                {
                                    string ship_guid = Guid.NewGuid().ToString();
                                    string sod_oid = Guid.NewGuid().ToString();
                                    SyncToERP(dbitem, sod_oid, db, sokls, codeSoMstr);
                                    string guid = Reposo_mstr.FindByPK(codeSoMstr).so_oid;
                                    Reposo_mstr.saveSoShipMstr(dbitem, UserPrincipal.username, guid, ship_guid, null, codeSoMstr);
                                    Reposo_mstr.saveSoShipDet(dbitem, UserPrincipal.username, ship_guid, sod_oid);
                                }
                            }
                        }
                    }
                    db.SONumber = string.Join(",", dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => SelectedSo.Contains(d.Id)).Select(e => e.NoSo));
                    db.CustomerId = dbitem.SalesOrderKontrak.CustomerId;
                    db.DataTruckId = dbsoDummy.FirstOrDefault().IdDataTruck;
                    db.AUJTanggalMuat = dbsoDummy.FirstOrDefault().MuatDate;
                    //rubah status yang tidak dipilih
                    foreach (Context.SalesOrderKontrakListSo item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => UnSelectedSo.Contains(d.Id)))
                    {
                        item.Status = "save konfirmasi";
                        item.IdAdminUangJalan = null;
                    }
                    Context.SalesOrderKontrakListSo soklsPertama = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan != null && d.AdminUangJalan.PerluJurnalBalikPiutangPos).FirstOrDefault();
                    if (soklsPertama != null)
                    {
                        Context.AdminUangJalan aujPertama = soklsPertama.AdminUangJalan;
                        List<Context.SalesOrderKontrakListSo> ListSoklsPertama = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == aujPertama.Id).ToList();
                        if (aujPertama.PerluJurnalBalikPiutangPos && soklsPertama.MuatDate.AddHours(7).Month.ToString("00").PadLeft(2, '0') != dbitem.SalesOrderKontrak.DocDate.Month.ToString("00").PadLeft(2, '0') && DateTime.Now.Month.ToString("00").PadLeft(2, '0') == soklsPertama.MuatDate.AddHours(7).Month.ToString("00").PadLeft(2, '0'))
                        {
                            tambahanRute = aujPertama.AdminUangJalanTambahanRute.Sum(s => s.values);
                            boronganDasar = aujPertama.TotalBorongan - (aujPertama.Kawalan ?? 0) - (aujPertama.Timbangan ?? 0) - (aujPertama.Karantina ?? 0) - (aujPertama.SPSI ?? 0) - (aujPertama.Multidrop ?? 0) - tambahanRute - aujPertama.AdminUangJalanTambahanLain.Sum(s => s.Values);
                            hutangDriver = (aujPertama.KasbonDriver1 ?? 0) + (aujPertama.KlaimDriver1 ?? 0) + aujPertama.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                                aujPertama.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + aujPertama.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                                aujPertama.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                            string kodeListSoklsPertama = "AUJ-" + string.Join(",", ListSoklsPertama.Select(d => d.NoSo));
                            Repoglt_det.saveFromAc(3, kodeListSoklsPertama, boronganDasar, 0, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(4, kodeListSoklsPertama, aujPertama.Kawalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(5, kodeListSoklsPertama, aujPertama.Timbangan, 0, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(6, kodeListSoklsPertama, aujPertama.Karantina, 0, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(7, kodeListSoklsPertama, aujPertama.SPSI, 0, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(8, kodeListSoklsPertama, aujPertama.Multidrop, 0, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(9, kodeListSoklsPertama, tambahanRute + aujPertama.AdminUangJalanTambahanLain.Sum(s => s.Values), 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, null, dbsoDummy.FirstOrDefault());
                            Repoglt_det.saveFromAc(10, kodeListSoklsPertama, 0, hutangDriver, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPosSementara), dbitem, null, dbsoDummy.FirstOrDefault());
                            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsDefault"].ConnectionString);
                            con.Open();
                            using (DataTable dt = new DataTable())
                            {
                                var query = "UPDATE dbo.\"AdminUangJalan\" SET \"PerluJurnalBalikPiutangPos\"=FALSE WHERE \"Id\"=" + aujPertama.Id;
                                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                                da.Fill(dt);
                                cmd.Dispose();
                                con.Close();
                            }
                        }
                    }
                }
                dbitem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbitem);
                foreach (Context.SalesOrderKontrakListSo item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => SelectedSo.Contains(d.Id)))
                {
                    if (maju1Flow == "Y" || db.uangDM > 0 || db.TotalAlokasi == 0)
                        Maju1FlowKontrak(item.Id);
                }
                if (db.uangDM > 0)
                {
                    var glt_oid = Guid.NewGuid().ToString();
                    Repopbyd_det.saveMstr(glt_oid, "DM-" + code, 0, "Diambil DM " + code, Driver1IdPiutang.Value + 7000000);
                    Repopbyd_det.save(glt_oid, "DM-" + code, 0, "Diambil DM " + db.Code, Driver1IdPiutang.Value + 7000000, 0, "", db.uangDM.Value * -1);
                }
                //simpan history dirver
                Context.HistoryJalanTruck dbhisdriver = RepoHistoryJalanTruck.FindByAdm(db.Id);
                if (dbhisdriver == null)
                    dbhisdriver = new Context.HistoryJalanTruck();
                dbhisdriver.IdAdminUangJalan = db.Id;
                dbhisdriver.IdDriver1 = db.IdDriver1.Value;
                dbhisdriver.IdDriver2 = db.IdDriver2;
                dbhisdriver.IdTruck = dbsoDummy.FirstOrDefault().IdDataTruck.Value;
                dbhisdriver.NoSo = string.Join(", ", dbsoDummy.Select(s => s.NoSo).ToList());
                dbhisdriver.TanggalMuat = dbsoDummy.FirstOrDefault().MuatDate;
                dbhisdriver.JenisOrder = "Kontrak";
                RepoHistoryJalanTruck.save(dbhisdriver);
                RepoAuditrail.saveAUJHistory(dbitem, dbhisdriver, db, string.Join(",", SelectedSo));
                return RedirectToAction("IndexKontrak");
            }
            }
            catch (Exception)
            {
                return RedirectToAction("IndexKontrak");
            }
            ViewBag.Title = "Admin Uang Jalan " + model.ModelKontrak.SONumber;
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
            GenerateModel(model, dbitem);
            model.ModelKontrak.ListValueModelSOKontrak = resSo.ToList();
            model.ModelKontrak.ListValueModelSOKontrak = resSelSo.ToList();
            return View("FormKontrak", model);
        }

        public void updateAUJId(string SONumber)
        {
            Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByNoSo(SONumber);
            Context.AdminUangJalan auj = RepoAdminUangJalan.FindAll().Where(d => d.SONumber != null && d.SONumber.Contains(sokls.NoSo) && d.Status != "Batal" && d.IdDriver1 == sokls.Driver1Id).FirstOrDefault();
            sokls.IdAdminUangJalan = auj.Id;
            RepoSalesOrderKontrakListSo.save(sokls);
        }

        public void updateAUJIdPerParent(string SONumber)
        {
            foreach(Context.SalesOrderKontrakListSo soklss in RepoSalesOrder.FindByCode(SONumber).SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == null && (d.Status == "admin uang jalan" || d.Status == "dispatched"))){
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByNoSo(soklss.NoSo);
                Context.AdminUangJalan auj = RepoAdminUangJalan.FindAll().Where(d => d.SONumber != null && d.SONumber.Contains(sokls.NoSo) && d.Status != "Batal" && d.IdDriver1 == sokls.Driver1Id).FirstOrDefault();
                sokls.IdAdminUangJalan = auj.Id;
                RepoSalesOrderKontrakListSo.save(sokls);
            }
        }

        public void nyusulinKpdAUJ(int aujId, int soklsId)
        {
            //kpd
            Context.AdminUangJalan db = RepoAdminUangJalan.FindByPK(aujId);
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(soklsId);
            List<Context.SalesOrderKontrakListSo> dbsoDummy = RepoSalesOrderKontrakListSo.FindAll().Where(d => db.SONumber.Contains(sokls.NoSo)).ToList();
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value);
            decimal? hutangDriver = (db.KasbonDriver1 ?? 0) + (db.KlaimDriver1 ?? 0) + db.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                db.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + db.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                db.AdminUangJalanUangTf.Select(d => d.Value).Sum();
            var glt_oid_pby = Guid.NewGuid().ToString();
            string codePby = "P-AUJ-" + dbsoDummy.Select(d => d.NoSo).Min() + '~' + dbsoDummy.Select(d => d.NoSo).Max();
            int codePbyUrutan = Repopbyd_det.getUrutan(codePby) + 1;
            codePby = codePby + "-" + codePbyUrutan;
            decimal? total_potongan_driver = (db.AdminUangJalanPotonganDriver.Sum(s => s.Value) + db.KasbonDriver1);
            string codeBT = "BTB-" + dbitem.SONumber;
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == codeBT).Max(d => d.UrutanBatal) + 1;
            string code = db.Code + "-" + urutanBatal;
/*            if (total_potongan_driver > 0)
            {
                Repopbyd_det.saveMstr(glt_oid_pby, codePby, 0, "Potongan " + code, db.IdDriver1.Value + 7000000);
                Repopbyd_det.save(glt_oid_pby, codePby, 0, "Potongan " + db.Code, db.IdDriver1.Value + 7000000, 0, "", total_potongan_driver.Value * -1);
            }
  */          
            //jurnal kpd
            string soNumber = "";
            foreach (var item in dbsoDummy)
            {
                soNumber += "," + item.NoSo;
            }
            int idx = 0;
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

        public void SyncToERP(Context.SalesOrder dbso, string sod_guid, Context.AdminUangJalan auj, Context.SalesOrderKontrakListSo sokls, string codeSoMstr)
        {
            string guid = Guid.NewGuid().ToString();
            Reposo_mstr.saveSoMstr(dbso, UserPrincipal.username, guid, dbso.SalesOrderKontrak.CustomerId.Value, RepoDHKontrak.FindItemByPK(auj.DaftarHargaKontrakId.Value).Harga, sokls, auj, codeSoMstr);
            Reposo_mstr.saveSoDet(dbso, UserPrincipal.username, guid, sod_guid, sokls, 0, auj);
        }

        public void NyusulinType2(int aujId)
        {
            Context.AdminUangJalan db = RepoAdminUangJalan.FindByPK(aujId);
            Context.SalesOrderKontrak sok = RepoSalesOrderKontrakListSo.FindAllByAUJ(aujId).FirstOrDefault().SalesOrderKontrak;
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(sok.SalesOrderKontrakId);
            Context.DaftarHargaKontrak dhk = RepoDHKontrak.FindItemByPK(db.DaftarHargaKontrakId.Value).DaftarHargaKontrak;
            if (dhk.LookUpTypeKontrak.Nama == "TYPE 2")
            {
                for (int i = 1; i <= dbitem.SalesOrderKontrak.JumlahTruck; i++)
                {
                    Context.SalesOrderKontrakListSo sokls = sok.SalesOrderKontrakListSo.Where(d => d.NoSo.Contains("." + i)).FirstOrDefault();
                    if (sokls != null)
                    {
                        string codeSoMstr = dbitem.SalesOrderKontrak.SONumber + "." + sokls.NoSo.Split('.')[1];
                        Context.so_mstr presentSOInERP = Reposo_mstr.FindByPK(codeSoMstr);
                        if (presentSOInERP == null)
                        {
                            string ship_guid = Guid.NewGuid().ToString();
                            string sod_oid = Guid.NewGuid().ToString();
                            SyncToERP(dbitem, sod_oid, db, sokls, codeSoMstr);
                            string guid = Reposo_mstr.FindByPK(codeSoMstr).so_oid;
                            Reposo_mstr.saveSoShipMstr(dbitem, UserPrincipal.username, guid, ship_guid, null, codeSoMstr);
                            Reposo_mstr.saveSoShipDet(dbitem, UserPrincipal.username, ship_guid, sod_oid);
                        }
                        else
                        {
                            string guid = Reposo_mstr.FindByPK(codeSoMstr).so_oid;
                            Context.soship_mstr presentSOShipInERP = Reposo_mstr.FindSoShipBySo(guid);
                            if (presentSOShipInERP == null)
                            {
                                string ship_guid = Guid.NewGuid().ToString();
                                string sod_oid = Guid.NewGuid().ToString();
                                guid = Reposo_mstr.FindByPK(codeSoMstr).so_oid;
                                sod_oid = Reposo_mstr.FindSodDetBySo(guid).sod_oid;
                                Reposo_mstr.saveSoShipMstr(dbitem, UserPrincipal.username, guid, ship_guid, null, codeSoMstr);
                                Reposo_mstr.saveSoShipDet(dbitem, UserPrincipal.username, ship_guid, sod_oid);
                            }
                        }
                    }
                }
            }
        }

        [MyAuthorize(Menu = "Admin Uang Jalan", Action = "print")]
        public ActionResult ShowPrint(int idSo, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);
            AdminUangJalan model = new AdminUangJalan();

            if (!dbitem.SalesOrderKontrakId.HasValue)
            {
                if (dbitem.AdminUangJalanId.HasValue)
                    model = new AdminUangJalan(dbitem.AdminUangJalan);

                model.IdSalesOrder = dbitem.Id;
                GenerateModel(model, dbitem);

                ViewBag.kodedr = dbitem.AdminUangJalan.Driver1.KodeDriver;
                ViewBag.namadr = dbitem.AdminUangJalan.Driver1.NamaDriver;
            }


            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ViewBag.nopol = dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                ViewBag.tgljalan = dbitem.SalesOrderOncall.TanggalMuat;
                ViewBag.customer = dbitem.SalesOrderOncall.Customer.CustomerNama;
                ViewBag.rute = dbitem.SalesOrderOncall.StrDaftarHargaItem;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.nopol = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                ViewBag.tgljalan = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat;
                ViewBag.customer = "";
                ViewBag.rute = dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ViewBag.nopol = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
                ViewBag.tgljalan = dbitem.SalesOrderPickup.TanggalPickup;
                ViewBag.customer = dbitem.SalesOrderPickup.Customer.CustomerNama;
                ViewBag.rute = dbitem.SalesOrderPickup.Rute.Nama;
            }
            else if (dbitem.SalesOrderKontrakId.HasValue)
            {
                List<int> ListIdDumy = listSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;

                if (dbsoDummy.FirstOrDefault().IdAdminUangJalan.HasValue)
                    model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);

                model.IdSalesOrder = dbitem.Id;
                GenerateModel(model, dbitem);

                ViewBag.kodedr = dbsoDummy.FirstOrDefault().AdminUangJalan.Driver1.KodeDriver;
                ViewBag.namadr = dbsoDummy.FirstOrDefault().AdminUangJalan.Driver1.NamaDriver;

                ViewBag.nopol = dbsoDummy.FirstOrDefault().DataTruck.VehicleNo;
                ViewBag.tgljalan = dbsoDummy.FirstOrDefault().MuatDate;
                ViewBag.customer = dbitem.SalesOrderKontrak.Customer.CustomerNama;
                //ViewBag.rute = dbitem.SalesOrderPickup.Rute.Nama;
            }

            if (model.ModelListRemoval.Count > 0)
            {
                model.ModelListRemoval = model.ModelListRemoval.Where(r => r.Status != "" && r.Status != null).OrderBy(d => d.Id).ToList();
                if (model.ModelListRemoval.Count > 0)
                    return View("~/Views/Removal/Print.cshtml", model.ModelListRemoval.Last());
                else
                    return View("Print", model);
            }
            else
                return View("Print", model);
        }
        public ActionResult ShowPrintSuratJalan(int idSo, string listSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);
            if (dbitem == null)
            {
                Context.SalesOrderKontrakListSo sok = RepoSalesOrderKontrakListSo.FindByPK(idSo);
                List<int> ListIdDumy = idSo.ToString().Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                dbitem = RepoSalesOrder.FindByKontrak(sok.SalesKontrakId.Value);
                List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                Context.DaftarHargaKontrakItem dhki = RepoDHKontrak.FindItemByPK(sok.AdminUangJalan.DaftarHargaKontrakId.Value);
                ViewBag.cust = dbitem.SalesOrderKontrak.Customer.CustomerNama;
                ViewBag.nopol = dbsoDummy.FirstOrDefault().DataTruck.VehicleNo;
                ViewBag.type = dbsoDummy.FirstOrDefault().SalesOrderKontrak.JenisTrucks.StrJenisTruck;
                if (dbitem.SalesOrderKontrak.MasterProduct != null)
                    ViewBag.barang = dbitem.SalesOrderKontrak.MasterProduct.NamaProduk + " ; Suhu : " + dbitem.SalesOrderKontrak.MasterProduct.TargetSuhu + " °C";
                ViewBag.tgl = dbsoDummy.FirstOrDefault().MuatDate.ToShortDateString();
                ViewBag.namadr = dbsoDummy.FirstOrDefault().AdminUangJalan.Driver1.NamaDriver;
                ViewBag.dari = dhki.NamaRuteDaftarHarga.Split('-')[0];
                ViewBag.tujuan = dhki.NamaRuteDaftarHarga.Split('-')[1];
                ViewBag.SONo = dbsoDummy.FirstOrDefault().NoSo + " (" + dbitem.SalesOrderKontrak.Customer.CustomerCodeOld + ")";
            }
            else if (dbitem.SalesOrderOncallId.HasValue)
            {
                ViewBag.cust = dbitem.SalesOrderOncall.Customer.CustomerNama;
                int IdJenisTruck;
                string IdRute;

                RepoDHO.FindRuteTruk(dbitem.SalesOrderOncall.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);
                if (IdRute != null && IdRute != "")
                {
                    var dummyRute = RepoRute.FindByPK(int.Parse(IdRute.Split(',')[0]));
                    ViewBag.dari = dbitem.SalesOrderOncall.StrDaftarHargaItem.Split('-')[0];
                    ViewBag.tujuan = dbitem.SalesOrderOncall.StrDaftarHargaItem.Split(new string[] { dbitem.SalesOrderOncall.StrDaftarHargaItem.Split('-')[0] + "-" }, StringSplitOptions.None)[1];
                }
                ViewBag.nopol = dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                ViewBag.type = "1 " + dbitem.SalesOrderOncall.JenisTrucks.StrJenisTruck;
                ViewBag.barang = dbitem.SalesOrderOncall.MasterProduct.NamaProduk + " ; Suhu : " + dbitem.SalesOrderOncall.MasterProduct.TargetSuhu + " °C";
                ViewBag.tgl = dbitem.SalesOrderOncall.TanggalMuat.Value.ToShortDateString();
                ViewBag.namadr = dbitem.AdminUangJalan == null ? "" : dbitem.AdminUangJalan.Driver1.NamaDriver;
            }
            else if (dbitem.SalesOrderKonsolidasiId.HasValue)
            {
                Context.SalesOrderProsesKonsolidasi sopk = RepoSalesOrder.FindProsesKonsolidasiItem(dbitem.SalesOrderKonsolidasiId.Value).SalesOrderProsesKonsolidasi;
                Context.SalesOrder so = RepoSalesOrder.FindByProsesKonsolidasi(sopk.SalesOrderProsesKonsolidasiId);
                SalesOrderProsesKonsolidasi modelKonsolidasi = new SalesOrderProsesKonsolidasi(so);
                SalesOrderKonsolidasi modelBarang = new SalesOrderKonsolidasi(dbitem);
                ViewBag.cust = dbitem.SalesOrderKonsolidasi.CustomerTagihan.CustomerNama;
                var dummyRute = RepoRute.FindByPK(modelKonsolidasi.RuteId.Value);
                ViewBag.dari = dbitem.SalesOrderKonsolidasi.StrDaftarHargaItem.Split('-')[0];
                ViewBag.tujuan = dbitem.SalesOrderKonsolidasi.StrDaftarHargaItem.Split('-')[1];
                ViewBag.tgl = modelKonsolidasi.TanggalMuat.Value.ToShortDateString();
                ViewBag.namadr = so.Driver.NamaDriver;
                ViewBag.nopol = modelKonsolidasi.VehicleNo;
                ViewBag.barang = dbitem.SalesOrderKonsolidasi.MasterProduct.NamaProduk + " ; Suhu : " + dbitem.SalesOrderKonsolidasi.MasterProduct.TargetSuhu + " °C";
                string satuan = modelBarang.PerhitunganDasar != "Manual" ? modelBarang.PerhitunganDasar : modelBarang.TypeKonsolidasi;
                decimal harga = decimal.Parse("0.0");
                if (modelBarang.PerhitunganDasar == "Tonase")
                    harga = modelBarang.Tonase.Value;
                else if (modelBarang.PerhitunganDasar == "Karton")
                    harga = modelBarang.karton.Value;
                else if (modelBarang.PerhitunganDasar == "Pallet")
                    harga = modelBarang.Pallet.Value;
                else if (modelBarang.PerhitunganDasar == "Container")
                    harga = modelBarang.Container.Value;
                else if (modelBarang.PerhitunganDasar == "m3")
                    harga = modelBarang.m3.Value;
                else if (modelBarang.PerhitunganDasar == "Manual")
                {
                    if (modelBarang.TypeKonsolidasi == "Tonase")
                        harga = modelBarang.Tonase.Value;
                    else if (modelBarang.TypeKonsolidasi == "Karton")
                        harga = modelBarang.karton.Value;
                    else if (modelBarang.TypeKonsolidasi == "Pallet")
                        harga = modelBarang.Pallet.Value;
                    else if (modelBarang.TypeKonsolidasi == "Container")
                        harga = modelBarang.Container.Value;
                    else if (modelBarang.TypeKonsolidasi == "m3")
                        harga = modelBarang.m3.Value;
                }
                ViewBag.SONo = dbitem.SONumber + " (" + dbitem.SalesOrderKonsolidasi.CustomerTagihan.CustomerCodeOld + ")";
                ViewBag.type = harga + " " + (satuan == "Tonase" ? "Kg" : satuan);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                Context.SalesOrder so = RepoSalesOrder.FindByProsesKonsolidasi(dbitem.SalesOrderProsesKonsolidasiId.Value);
                Context.SalesOrderProsesKonsolidasi sopk = so.SalesOrderProsesKonsolidasi;
                SalesOrderProsesKonsolidasi model = new SalesOrderProsesKonsolidasi(so);
                ViewBag.tgl = model.TanggalMuat.Value.ToShortDateString();
                ViewBag.namadr = so.Driver.NamaDriver;
                ViewBag.nopol = model.VehicleNo;
                return View("PrintSJKonsolidasi", model);
            }
            if (ViewBag.SONo == null)
                ViewBag.SONo = dbitem.SONumber + " ("+ dbitem.Customer.CustomerCodeOld+")";
            return View("PrintSuratJalan");
        }
        public ActionResult ShowPrintVoucher(int idSo, string listSo)
        {
            AdminUangJalan model = new AdminUangJalan();
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);
            DateTime currDate = DateTime.Now.Date;
            Decimal? HaragaSolar = RepoSolar.FindAll().Where(d => currDate >= d.Start && currDate <= d.End).FirstOrDefault().Harga;
            ViewBag.HargaSolar = HaragaSolar.HasValue ? HaragaSolar : 0;
            if (listSo != null && listSo != "")
            {
                Context.SalesOrderKontrakListSo sok = RepoSalesOrderKontrakListSo.FindByPK(idSo);
                dbitem = RepoSalesOrder.FindByKontrak(idSo);
                List<int> ListIdDumy = listSo.ToString().Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                Context.SalesOrderKontrakListSo sokls = dbsoDummy.FirstOrDefault();
                ViewBag.driverId = sokls.Driver1Id;
                ViewBag.driverName = sokls.Driver1.KodeDriver + " - " + sokls.Driver1.NamaDriver;
                ViewBag.TanggalMuat = sokls.MuatDate.ToShortDateString();
                ViewBag.VehicleNo = sokls.DataTruck.VehicleNo;
                ViewBag.Jenis = RepoLookupCode.FindByPK(sokls.DataTruck.IdMerk).Nama;
                Context.DataBorongan bor = RepoBor.FindByPK(int.Parse(sokls.AdminUangJalan.IdDataBorongan));
                ViewBag.Liter = bor.LiterSolar;
                model = new AdminUangJalan(sokls.AdminUangJalan);
                model.IdSalesOrder = dbitem.Id;
                GenerateModel(model, dbitem);
                return View("PrintVoucher", model);
            }
            else if (dbitem != null && dbitem.SalesOrderOncallId.HasValue)
            {
                ViewBag.driverId = dbitem.SalesOrderOncall.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderOncall.Driver1.KodeDriver + " - " + dbitem.SalesOrderOncall.Driver1.NamaDriver;
                ViewBag.TanggalMuat = dbitem.SalesOrderOncall.TanggalMuat.Value.ToShortDateString();
                ViewBag.VehicleNo = dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                ViewBag.Jenis = RepoLookupCode.FindByPK(dbitem.SalesOrderOncall.DataTruck.IdMerk).Nama;
            }
            else if (dbitem != null && dbitem.SalesOrderPickupId.HasValue)
            {
                ViewBag.driverId = dbitem.SalesOrderPickup.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderPickup.Driver1.KodeDriver + " - " + dbitem.SalesOrderPickup.Driver1.NamaDriver;
            }
            else if (dbitem != null && dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ViewBag.driverId = dbitem.SalesOrderProsesKonsolidasi.Driver1Id;
                ViewBag.driverName = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver + " - " + dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                ViewBag.TanggalMuat = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat.Value.ToShortDateString();
                ViewBag.VehicleNo = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                ViewBag.Jenis = RepoLookupCode.FindByPK(dbitem.SalesOrderProsesKonsolidasi.DataTruck.IdMerk).Nama;
            }
            Context.SettlementBatal sb = RepoSettlementBatal.FindAll().Where(
                d => d.IsProses == true && d.IdAdminUangJalan == null && d.SalesOrder != null && d.IdDriver == ViewBag.driverId
            ).FirstOrDefault();
            ViewBag.SisaPerjalananSblmny = 0;
            if (sb != null)
            {
                ViewBag.SisaPerjalananSblmny += sb.TransferSelisih == null ? 0 : sb.TransferSelisih;
                ViewBag.SisaPerjalananSblmny += sb.KasSelisih == null ? 0 : sb.KasSelisih;
                ViewBag.SisaPerjalananSblmny += sb.SolarSelisih == null ? 0 : sb.SolarSelisih;
                if (sb.SalesOrder.SalesOrderOncallId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderOncall.SONumber;
                else if (sb.SalesOrder.SalesOrderPickupId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderPickup.SONumber;
                else if (sb.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                else if (sb.SalesOrder.SalesOrderKontrakId.HasValue)
                    ViewBag.SOSblmny = sb.SalesOrder.SalesOrderKontrak.SONumber;
            }
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);
            model.IdSalesOrder = dbitem.Id;
            GenerateModel(model, dbitem);

            if (model.ModelListRemoval.Count > 0)
            {
                model.ModelListRemoval = model.ModelListRemoval.OrderBy(d => d.Id).ToList();
                return View("~/Views/Removal/FormRemoval.cshtml", model);
            }
            else
                return View("PrintVoucher", model);
        }
        public ActionResult ShowPrintSJKonsolidasi(int idSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);
            SalesOrderProsesKonsolidasi model = new SalesOrderProsesKonsolidasi(dbitem);
            var dummyRute = RepoRute.FindByPK(model.RuteId.Value);
            ViewBag.dari = dummyRute.LocationAsal.Nama;
            ViewBag.tujuan = dummyRute.LocationTujuan.Nama;
            ViewBag.tgl = model.TanggalMuat.Value.ToShortDateString();
            ViewBag.namadr = dbitem.AdminUangJalan.Driver1.NamaDriver;
            ViewBag.nopol = model.VehicleNo;
            return View("PrintSJKonsolidasi", model);
        }
    }
}