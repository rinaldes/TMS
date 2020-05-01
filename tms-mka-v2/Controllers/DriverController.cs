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
    public class DriverController : BaseController
    {
        private IDriverRepo RepoDriver; private ILocationRepo RepoLokasi; private ISalesOrderRepo RepoSalesOrder; private IHistoryJalanTruckRepo RepoHistoryJalanTruck; private IKlaimRepo RepoKlaim;
        private ISettlementBatalRepo RepoSettlementBatal; private Ipbyd_detRepo Repopbyd_det; private Icashd_detRepo Repocashd_det; private Iptnr_mstrRepo Repoptnr_mstr;
        private Iptnra_addrRepo Repoptnra_addr; private IBAPRepo RepoBAP; private IInventarisRepo RepoInventaris; private IPelaksanaanTrainingRepo RepoPelaksanaanTraining;
        private IAdminUangJalanRepo RepoAuj; private IAtmRepo RepoAtm; private IDataBoronganRepo RepoBorongang; private IDokumenRepo RepoDokumen; private IMonitoringVehicleRepo RepoMonitoringVehicle;
        private IBatalOrderRepo RepoBatalOrder; private IAuditrailRepo RepoAuditrail; private ITrainingSettingRepo RepoTraining;
        public DriverController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDriverRepo repoDriver, ILocationRepo repoLokasi, ISalesOrderRepo repoSalesOrder, ITrainingSettingRepo repoTraining,
            IHistoryJalanTruckRepo repoHistoryJalanTruck, ISettlementBatalRepo repoSettlementBatal, Ipbyd_detRepo repopbyd_det, Icashd_detRepo repocashd_det, Iptnr_mstrRepo repoptnr_mstr,
            Iptnra_addrRepo repoptnra_addr, IBAPRepo repoBAP, IPelaksanaanTrainingRepo repoPelaksanaanTraining, IAdminUangJalanRepo repoAuj, IAtmRepo repoAtm, IDataBoronganRepo repoBorongang,
            IDokumenRepo repoDokumen, IInventarisRepo repoInventaris, IMonitoringVehicleRepo repoMonitoringVehicle, IKlaimRepo repoKlaim, IBatalOrderRepo repoBatalOrder, IAuditrailRepo repoAuditTrail)
            : base(repoBase, repoLookup)
        {
            RepoDriver = repoDriver; RepoLokasi = repoLokasi;
            RepoSalesOrder = repoSalesOrder; RepoHistoryJalanTruck = repoHistoryJalanTruck;
            RepoSettlementBatal = repoSettlementBatal; Repopbyd_det = repopbyd_det;
            Repocashd_det = repocashd_det; Repoptnr_mstr = repoptnr_mstr;
            Repoptnra_addr = repoptnra_addr; RepoBAP = repoBAP;
            RepoPelaksanaanTraining = repoPelaksanaanTraining; RepoAuj = repoAuj;
            RepoAtm = repoAtm; RepoBorongang = repoBorongang;
            RepoDokumen = repoDokumen;
            RepoInventaris = repoInventaris;
            RepoMonitoringVehicle = repoMonitoringVehicle;
            RepoKlaim = repoKlaim;
            RepoBatalOrder = repoBatalOrder;
            RepoAuditrail = repoAuditTrail;
            RepoTraining = repoTraining;
        }

        [MyAuthorize(Menu = "Data Driver", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Driver").ToList();
            return View();
        }

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Driver> items = RepoDriver.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Driver> ListModel = new List<Driver>();
            foreach (Context.Driver item in items)
            {
                ListModel.Add(new Driver(item));
            }

            int total = RepoDriver.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string BindingSettlementBatal(int id){
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SettlementBatal> items = RepoSettlementBatal.FindAll().Where(d => d.IdDriver == id).ToList();

            List<SettlementBatalIndex> ListModel = new List<SettlementBatalIndex>();
            foreach (Context.SettlementBatal item in items)
            {
                if (item.IdSoKontrak != "" && item.IdSoKontrak != null)
                {
                    var data = item.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(p => p.IsBatalTruck).ToList();
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

        public string BindingInventaris(int id)
        {
            Context.Driver dbdriver = RepoDriver.FindByPK(id);

            List<Context.Inventaris> dbInventaris = dbdriver.Inventaris.ToList();

            List<Inventaris> modelInventaris = new List<Inventaris>();
            foreach (var item in dbInventaris)
            {
                modelInventaris.Add(new Models.Inventaris(item));
            }

            return new JavaScriptSerializer().Serialize(modelInventaris);
        }

        public string CheckArea(int id)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindAll().Where(d =>
                (d.Status == "save" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                    (d.SalesOrderOncallId.HasValue && (d.SalesOrderOncall.Driver1Id == id || d.SalesOrderOncall.Driver2Id == id)) ||
                    (d.SalesOrderPickupId.HasValue && (d.SalesOrderPickup.Driver1Id == id || d.SalesOrderPickup.Driver2Id == id)) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrderProsesKonsolidasi.Driver1Id == id || d.SalesOrderProsesKonsolidasi.Driver2Id == id))
                )
            ).FirstOrDefault();
            if (dbso.SalesOrderOncallId.HasValue)
                return new JavaScriptSerializer().Serialize(new { area = RepoSalesOrder.FindArea(dbso.SalesOrderOncall.IdDaftarHargaItem.Value) });
            return null;
        }

        public string BindingCombobox()
        {
            List<Context.Driver> items = RepoDriver.FindAll();

            List<Driver> ListModel = new List<Driver>();
            foreach (Context.Driver item in items)
            {
                ListModel.Add(new Driver(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel.Select(d => new { value = d.Id, text = d.KodeDriver + " | " + d.NamaDriver, code = d.KodeDriver, nama = d.NamaDriver }));
        }
        public string GetHistoryJalan(int id)
        {
            List<Context.HistoryJalanTruck> dblist = RepoHistoryJalanTruck.FindByDriver(id);

            return new JavaScriptSerializer().Serialize(dblist.Select(d => new
            {
                shipmentId = d.ShipmentId,
                noSo = d.NoSo,
                tanggalMuat = d.TanggalMuat,
                jenisOrder = d.JenisOrder,
                customer = d.IdCustomer.HasValue ? d.Customer.CustomerNama : "",
                rute = d.Rute,
            }));
        }
        public string GetDriverByCode(string code)
        {
            Context.Driver dbitems = RepoDriver.FindByCode(code);

            return new JavaScriptSerializer().Serialize(dbitems.NamaDriver);
        }
        public string BindingStatHistory(int id)
        {
            Context.Driver db = RepoDriver.FindByPK(id);
            if (db != null)
            {
                return new JavaScriptSerializer().Serialize(db.DriverStatusHistory.
                    Select(d => new
                    {
                        Tanggal = d.Tanggal,
                        Status = d.Status,
                        keterangan = d.keterangan,
                    }));
            }
            else
            {
                return null;
            }

        }
        public string BindingTruckHistory(int id)
        {
            Context.Driver db = RepoDriver.FindByPK(id);

            return new JavaScriptSerializer().Serialize(db.DriverTruckHistory.
                Select(d => new
                {
                    Tanggal = d.Tanggal,
                    Type = d.Type,
                    Nopol = d.Nopol,
                }));
        }
        public string BindingBAPHistory(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.BAP> items = RepoBAP.FindAll().Where(d => d.Driver1Id == id || d.Driver2Id == id).ToList();

            List<BAP> ListModel = new List<BAP>();
            foreach (Context.BAP item in items)
            {
                ListModel.Add(new BAP(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string BindingTrainingHistory(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.PelaksanaanTrainingDetail> items = RepoPelaksanaanTraining.FindAllDetail().Where(d => d.IdDriver == id).ToList();

            List<PelaksanaanTrainingDetail> ListModel = new List<PelaksanaanTrainingDetail>();
            foreach (Context.PelaksanaanTrainingDetail item in items)
            {
                ListModel.Add(new PelaksanaanTrainingDetail(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string BindingPiutangHistory(int id, string piutang_type, string kasbon_type=null)
        {
            /*(
            SELECT pbyd_amount_pay, pby_driver, pbyd_dt AS tr_date, 'Pencairan ' || pby_code FROM pbyd_det INNER JOIN pby_mstr ON pby_oid=pbyd_pby_oid
            UNION 
            SELECT (cashd_amount+cashd_refund_amount)*-1, pby_driver, cashd_dt AS tr_date, 'Realisasi ' || pby_code FROM cashd_det INNER JOIN pbyd_det ON cashd_pbyd_oid=pbyd_oid
                INNER JOIN pby_mstr ON pbyd_pby_oid=pby_oid
            )
            ORDER BY tr_date
            */
            Context.Driver dbdriver = RepoDriver.FindByPK(id);
            List<DriverPiutangHistory> ListModel = new List<DriverPiutangHistory>();
            List<DriverPiutangHistory> ListModelNew = new List<DriverPiutangHistory>();
            decimal? saldo = 0;
            List<Context.pbyd_det> pbyd_dets = Repopbyd_det.FindAll().Where(d => d.pby_mstr.pby_driver - 7000000 == id && d.pbyd_amount_pay != 0).ToList();
            List<Context.cashd_det> cashd_dets = null;
            if (piutang_type == "Klaim" || piutang_type=="All")
            {
                cashd_dets = Repocashd_det.FindAll().Where(d => d.pbyd_det != null && d.pbyd_det.pby_mstr.pby_driver - 7000000 == id).ToList();
                List<Context.Klaim> dbklaim = dbdriver.BebanKlaimDriver.Select(d => d.Klaim).ToList();

                List<Klaim> modelklaim = new List<Klaim>();
                foreach (var item in dbklaim)
                {
                    if (item.BebanClaimDriver != 0)
                    {
                        ListModel.Add(new DriverPiutangHistory("a", "b", item, saldo.Value));
                        saldo += decimal.Parse(item.BebanClaimDriver.Value.ToString());
                    }
                }
                List<Context.AdminUangJalan> dbAuj = RepoAuj.FindAll().Where(d => d.IdDriver1 == id && d.PotonganK > 0).ToList();
                foreach (var item in dbAuj)
                {
                    ListModel.Add(new DriverPiutangHistory(item, saldo.Value));
                    saldo += decimal.Parse(item.PotonganK.Value.ToString());
                }
            }
            if (piutang_type != "Klaim")
            {
                if (piutang_type == "P")
                {
                    pbyd_dets = Repopbyd_det.FindAll().Where(d => d.pby_mstr.pby_driver - 7000000 == id && d.pbyd_amount_pay != 0 && d.pbyd_tms_type == null).ToList();
                    cashd_dets = Repocashd_det.FindAll().Where(d => d != null).ToList();
                }
                else if (piutang_type == "B")
                {
                    pbyd_dets = Repopbyd_det.FindAll().Where(d => d.pby_mstr.pby_driver - 7000000 == id && d.pbyd_amount_pay != 0 && d.pbyd_tms_type == "B").ToList();
                }
                else if (piutang_type == "T")
                {
                    pbyd_dets = Repopbyd_det.FindAll().Where(d => d.pby_mstr.pby_driver - 7000000 == id && d.pbyd_amount_pay != 0 && d.pbyd_tms_type == "T").ToList();
                }

                foreach (Context.pbyd_det item in pbyd_dets)
                {
                    DriverPiutangHistory dph = new DriverPiutangHistory(item, saldo.Value);
                    ListModel.Add(dph);
                    saldo += item.pbyd_amount_pay;
                }
                if (cashd_dets != null)
                {
                    foreach (Context.cashd_det item in cashd_dets)
                    {
                        if (item != null && item.pbyd_det != null && item.pbyd_det.pby_mstr.pby_driver - 7000000 == id)
                        {
                            ListModel.Add(new DriverPiutangHistory("a", item, saldo.Value));
                            saldo += ((item.cashd_amount + item.cashd_refund_amount) * -1);
                        }
                    }
                }
            }
            saldo = 0;
            foreach (DriverPiutangHistory dph in ListModel.OrderBy(o => o.Tanggal))
            {
                saldo += dph.Jumlah;
                dph.Saldo = saldo;
                ListModelNew.Add(dph);
            }

            return new JavaScriptSerializer().Serialize(ListModelNew);
        }
        public decimal GetSaldoPiutang(int id, string piutang_type)
        {
            List<DriverPiutangHistory> ListModel = new List<DriverPiutangHistory>();
            decimal? saldo = 0;
            if (piutang_type != "Klaim")
            {
                List<Context.pbyd_det> pbyd_dets = Repopbyd_det.FindAll().Where(d => d.pby_mstr.pby_driver == id && d.pbyd_amount_pay != 0).ToList();

                foreach (Context.pbyd_det item in pbyd_dets)
                {
                    DriverPiutangHistory dph = new DriverPiutangHistory(item, saldo.Value);
                    ListModel.Add(dph);
                    saldo += item.pbyd_amount_pay;
                }
            }
            return saldo.Value;
        }
        public decimal GetSaldoPiutangBatalJalan(int id, string piutang_type)
        {
            List<DriverPiutangHistory> ListModel = new List<DriverPiutangHistory>();
            decimal? saldo = 0;
            if (piutang_type != "Klaim")
            {
                List<Context.pbyd_det> pbyd_dets = Repopbyd_det.FindAll().Where(d => d.pby_mstr.pby_driver == id && d.pbyd_amount_pay != 0 && d.pbyd_tms_type == "B").ToList();

                foreach (Context.pbyd_det item in pbyd_dets)
                {
                    DriverPiutangHistory dph = new DriverPiutangHistory(item, saldo.Value);
                    ListModel.Add(dph);
                    saldo += item.pbyd_amount_pay;
                }
            }
            return saldo.Value;
        }
        public string BindingKlaim(int id)
        {
            Context.Driver dbdriver = RepoDriver.FindByPK(id);

            List<Context.Klaim> dbklaim = dbdriver.BebanKlaimDriver.Select(d => d.Klaim).ToList();

            List<Klaim> modelklaim = new List<Klaim>();
            foreach (var item in dbklaim)
            {
                modelklaim.Add(new Models.Klaim(item));
            }

            return new JavaScriptSerializer().Serialize(modelklaim);
        }

        public string BindingAllHistoryDelivery(int id)
        {
            List<Context.MonitoringDetailSo> mdso = RepoMonitoringVehicle.FindAllMonitoringSo(id);
            List<MonitoringOnduty> listModel = new List<MonitoringOnduty>();
            foreach (var item in mdso)
            {
                decimal suhu = item == null || item.SuhuAvg == null ? 0 : decimal.Parse(item.SuhuAvg);
                Context.SalesOrder so = RepoSalesOrder.FindByOnCallCode(item.NoSo);
                if (so != null && so.SalesOrderOncallId.HasValue)
                {
                    Context.MasterProduct product = RepoSalesOrder.FindByOnCallCode(item.NoSo).SalesOrderOncall.MasterProduct;
                    Context.DataTruck truck = RepoSalesOrder.FindByOnCallCode(item.NoSo).SalesOrderOncall.DataTruck;
                    Context.Customer customer = RepoSalesOrder.FindByOnCallCode(item.NoSo).SalesOrderOncall.Customer;
                    Context.Rute rute = RepoSalesOrder.FindRute(so.SalesOrderOncall.IdDaftarHargaItem.Value);
                    listModel.Add(new MonitoringOnduty()
                    {
                        //Muat = (item.TglMuat - RepoSalesOrder.FindByOnCallCode(item.NoSo).SalesOrderOncall.TanggalMuat.Value).TotalHours > 1 ? "Late" : "On-Time",
                        //Perjalanan = (item.TglTiba - item.TargetTiba).TotalHours > 1 ? "Late" : "On-Time",
                        //Bongkar = "",
                        //Precooling = "",
                        //AcMati = "",
                        //SuhuSesuai = suhu > product.MinTemps && suhu < product.MaxTemps ? "YA" : "TIDAK",
                        //Klaim = RepoKlaim.FindBySoId(item.NoSo) == null ? "TIDAK" : "YA",
                        //JenisOrder = "On Call",
                        //NoSo = item.NoSo,
                        //VehicleNo = item.VehicleNo,
                        //JenisTruck = truck.JenisTrucks.StrJenisTruck,
                        //CustomerNama = customer.CustomerCodeOld + " - " + customer.CustomerNama,
                        //Rute = so.SalesOrderOncall.StrDaftarHargaItem,
                        //JenisBarang = product.LookupCode.Nama,
                        //TargetSuhu = product.TargetSuhu,
                        //TargetMuat = so.SalesOrderOncall.TanggalMuat.ToString(),
                        //TanggalTiba = item.TglTiba.ToString(),
                        //TglBerangkat = item.TglBerangkat.ToString(),
                        //TargetWaktu = rute.WaktuTempuhJam + " Jam " + rute.WaktuTempuhMenit + " Menit",
                        //TargetTiba = item.TargetTiba.ToString(),
                        //Delay = (item.TglTiba - item.TargetTiba).Hours + " h " + (item.TglTiba - item.TargetTiba).Minutes + " m",
                        //RangeSuhu = product.MinTemps + " - " + product.MaxTemps,
                        //SuhuAvg = item.SuhuAvg,
                        //AcOff = item.AcOff
                    });
                }
            }
            return new JavaScriptSerializer().Serialize(listModel);
        }

        public string BindingTransfer(int id)
        {
            List<Context.BatalOrder> dbbo = RepoBatalOrder.FindAll().Where(d => d.IsTransfer == true).ToList();
            List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().Where(d => dbbo.Any(e => e.IdSalesOrder == d.Id)).ToList();

            List<DriverHisTransfer> listModel = new List<DriverHisTransfer>();
            foreach (var item in dbso)
            {
                if (item.SalesOrderOncallId.HasValue && RepoSettlementBatal.FindBySo(item.Id) != null && RepoSettlementBatal.FindBySo(item.Id).IdDriver == id)
                {
                    listModel.Add(new DriverHisTransfer()
                    {
                        JnsSo = "Oncall",
                        NoSo = item.SalesOrderOncall.SONumber,
                        NoPol = RepoSettlementBatal.FindBySo(item.Id).DataTruck.VehicleNo,
                        JnsTruck = RepoSettlementBatal.FindBySo(item.Id).DataTruck.JenisTrucks.StrJenisTruck,
                        Customer = item.SalesOrderOncall.Customer.CustomerNama,
                        Rute = item.SalesOrderOncall.StrDaftarHargaItem,
                        JnsBarang = item.SalesOrderOncall.MasterProduct.NamaProduk,
                        TargetSuhu = item.SalesOrderOncall.MasterProduct.TargetSuhu,
                        KetTf = RepoBatalOrder.FindBySO(item.Id) == null ? "" : RepoBatalOrder.FindBySO(item.Id).Keterangan
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    var itemtf = item.AdminUangJalan.AdminUangJalanUangTf.Where(d => d.IdDriverPenerima == id && d.JumlahTransfer.HasValue).ToList();
                    foreach (var tf in itemtf)
                    {
                        listModel.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Konsolidasi",
                            NoSo = item.SalesOrderProsesKonsolidasi.SONumber,
                            NoPol = item.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = "",
                            Rute = item.SalesOrderProsesKonsolidasi.StrDaftarHargaItem,
                            JnsBarang = "",
                            TargetSuhu = 0,
                            Jumtf = tf.JumlahTransfer.Value,
                            DateTf = tf.TanggalAktual.Value,
                            KetTf = tf.KeteranganTf
                        });
                    }
                }
                else if (item.SalesOrderPickupId.HasValue)
                {
                    var itemtf = item.AdminUangJalan.AdminUangJalanUangTf.Where(d => d.IdDriverPenerima == id && d.JumlahTransfer.HasValue).ToList();
                    foreach (var tf in itemtf)
                    {
                        listModel.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Pickup",
                            NoSo = item.SalesOrderPickup.SONumber,
                            NoPol = item.SalesOrderPickup.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrderPickup.Customer.CustomerNama,
                            Rute = item.SalesOrderPickup.Rute.Nama,
                            JnsBarang = item.SalesOrderPickup.MasterProduct.NamaProduk,
                            TargetSuhu = item.SalesOrderPickup.MasterProduct.TargetSuhu,
                            Jumtf = tf.JumlahTransfer.Value,
                            DateTf = tf.TanggalAktual.Value,
                            KetTf = tf.KeteranganTf
                        });
                    }
                }
                else if (item.SalesOrderKontrakId.HasValue)
                {
                    foreach (var itemKontrak in item.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan.HasValue).GroupBy(g => new { g.IdAdminUangJalan }).Select(grp => grp.ToList()))
                    {
                        foreach (var tf in itemKontrak.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(d => d.IdDriverPenerima == id))
                        {
                            listModel.Add(new DriverHisTransfer()
                            {
                                JnsSo = "Kontrak",
                                NoSo = string.Join(", ", itemKontrak.Select(s => s.NoSo).ToList()),
                                NoPol = itemKontrak.FirstOrDefault().DataTruck.VehicleNo,
                                JnsTruck = itemKontrak.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck,
                                Customer = item.SalesOrderKontrak.Customer.CustomerNama,
                                Rute = item.SalesOrderPickup.Rute.Nama,
                                JnsBarang = "",
                                TargetSuhu = 0,
                                Jumtf = tf.JumlahTransfer.Value,
                                DateTf = tf.TanggalAktual.Value,
                                KetTf = tf.KeteranganTf
                            });
                        }
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(listModel);
        }
        public string BindingAllHst(int id)
        {
            Context.Driver dbdriver = RepoDriver.FindByPK(id);
            List<Context.Dokumen> dbdokumen = RepoDokumen.FindAll().Where(d => d.IsComplete == true && d.SalesOrder.DriverId == id).ToList();
            List<MonitoringOnduty> listModel = new List<MonitoringOnduty>();
            List<Context.MonitoringDetailSo> mdso = RepoMonitoringVehicle.FindAllMonitoringSo(id);

            foreach (var item in dbdokumen)
            {
                if (RepoKlaim.FindBySoId(item.SalesOrder.SalesOrderOncall.SONumber) != null)
                    continue;
                var persentase = (decimal.Parse(item.DokumenItem.Count().ToString()) * 100) / item.DokumenItem.Where(d => d.IsLengkap).Count();
                if (item.SalesOrder.SalesOrderOncallId.HasValue)
                {
                    Context.MonitoringDetailSo mitem = RepoMonitoringVehicle.FindMonitoringDetailSo(item.SalesOrder.SalesOrderOncall.SONumber);
                    Context.MasterProduct product = item.SalesOrder.SalesOrderOncall.MasterProduct;
                    Context.DataTruck truck = item.SalesOrder.SalesOrderOncall.DataTruck;
                    Context.Customer customer = item.SalesOrder.SalesOrderOncall.Customer;
                    Context.Rute rute = RepoSalesOrder.FindRute(item.SalesOrder.SalesOrderOncall.IdDaftarHargaItem.Value);
                    decimal suhu = (mitem == null ? 0 : decimal.Parse(mitem.SuhuAvg));

                    listModel.Add(new MonitoringOnduty(){
                        //Muat = mitem == null ? "-" : (mitem.TglMuat - RepoSalesOrder.FindByOnCallCode(mitem.NoSo).SalesOrderOncall.TanggalMuat.Value).TotalHours > 1 ? "Late" : "On-Time",
                        //Perjalanan = mitem == null ? "-" : (mitem.TglTiba - mitem.TargetTiba).TotalHours > 1 ? "Late" : "On-Time", Bongkar = "", Precooling = "", AcMati = "",
                        //SuhuSesuai = suhu > product.MinTemps && suhu < product.MaxTemps ? "YA" : "TIDAK", Klaim = mitem == null ? "-" : RepoKlaim.FindBySoId(mitem.NoSo) == null ? "TIDAK" : "YA", JenisOrder = "On Call",
                        //NoSo = item.SalesOrder.SalesOrderOncall.SONumber, VehicleNo = truck.VehicleNo, JenisTruck = item.SalesOrder.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck, CustomerNama = customer.CustomerNama,
                        //Rute = item.SalesOrder.SalesOrderOncall.StrDaftarHargaItem, JenisBarang = product.NamaProduk, TargetSuhu = item.SalesOrder.SalesOrderOncall.MasterProduct.TargetSuhu,
                        //TargetMuat = item.SalesOrder.SalesOrderOncall.TanggalMuat.ToString(), TanggalTiba = mitem == null ? "-" : mitem.TglTiba.ToString(), TglBerangkat = mitem == null ? "-" : mitem.TglBerangkat.ToString(),
                        //TargetWaktu = rute.WaktuTempuhJam + " Jam " + rute.WaktuTempuhMenit + " Menit", TargetTiba = mitem == null ? "-" : mitem.TargetTiba.ToString(),
                        //Delay = mitem == null ? "-" : (mitem.TglTiba - mitem.TargetTiba).Hours + " h " + (mitem.TglTiba - mitem.TargetTiba).Minutes + " m", RangeSuhu = product.MinTemps + " - " + product.MaxTemps,
                        //SuhuAvg = mitem == null ? "-" : mitem.SuhuAvg, AcOff = mitem == null ? 0 : mitem.AcOff
                    });
                }
                else if (item.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                {
                }
                else if (item.SalesOrder.SalesOrderPickupId.HasValue)
                {
                }
                else if (item.SalesOrder.SalesOrderKontrakId.HasValue)
                {
                }
            }
            List<Context.Klaim> dbklaim = dbdriver.BebanKlaimDriver.Select(d => d.Klaim).ToList();

            foreach (var item in dbklaim)
            {
                Context.MonitoringDetailSo mitem = RepoMonitoringVehicle.FindMonitoringDetailSo(item.SalesOrder.SalesOrderOncall.SONumber);
                Context.MasterProduct product = item.SalesOrder.SalesOrderOncall.MasterProduct;
                Context.DataTruck truck = item.SalesOrder.SalesOrderOncall.DataTruck;
                Context.Customer customer = item.SalesOrder.SalesOrderOncall.Customer;
                Context.Rute rute = RepoSalesOrder.FindRute(item.SalesOrder.SalesOrderOncall.IdDaftarHargaItem.Value);
                decimal suhu = (mitem == null ? 0 : decimal.Parse(mitem.SuhuAvg));
                listModel.Add(new MonitoringOnduty(){
                    //Muat = mitem == null ? "-" : (mitem.TglMuat - RepoSalesOrder.FindByOnCallCode(mitem.NoSo).SalesOrderOncall.TanggalMuat.Value).TotalHours > 1 ? "Late" : "On-Time",
                    //Perjalanan = mitem == null ? "-" : (mitem.TglTiba - mitem.TargetTiba).TotalHours > 1 ? "Late" : "On-Time", Bongkar = "", Precooling = "", AcMati = "",
                    //SuhuSesuai = suhu > product.MinTemps && suhu < product.MaxTemps ? "YA" : "TIDAK", Klaim = mitem == null ? "-" : RepoKlaim.FindBySoId(mitem.NoSo) == null ? "TIDAK" : "YA", JenisOrder = "On Call",
                    //NoSo = item.SalesOrder.SalesOrderOncall.SONumber, VehicleNo = truck.VehicleNo, JenisTruck = item.SalesOrder.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck, CustomerNama = customer.CustomerNama,
                    //Rute = item.SalesOrder.SalesOrderOncall.StrDaftarHargaItem, JenisBarang = product.NamaProduk, TargetSuhu = item.SalesOrder.SalesOrderOncall.MasterProduct.TargetSuhu,
                    //TargetMuat = item.SalesOrder.SalesOrderOncall.TanggalMuat.ToString(), TanggalTiba = mitem == null ? "-" : mitem.TglTiba.ToString(), TglBerangkat = mitem == null ? "-" : mitem.TglBerangkat.ToString(),
                    //TargetWaktu = rute.WaktuTempuhJam + " Jam " + rute.WaktuTempuhMenit + " Menit", TargetTiba = mitem == null ? "-" : mitem.TargetTiba.ToString(),
                    //Delay = mitem == null ? "-" : (mitem.TglTiba - mitem.TargetTiba).Hours + " h " + (mitem.TglTiba - mitem.TargetTiba).Minutes + " m", RangeSuhu = product.MinTemps + " - " + product.MaxTemps,
                    //SuhuAvg = mitem == null ? "-" : mitem.SuhuAvg, AcOff = mitem == null ? 0 : mitem.AcOff
                });
            }



            List<Context.BatalOrder> dbbo = RepoBatalOrder.FindAll().Where(d => d.IsTransfer == true).ToList();
            List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().Where(d => dbbo.Any(e => e.IdSalesOrder == d.Id)).ToList();

            foreach (var item in dbso)
            {
                Context.MonitoringDetailSo mitem = RepoMonitoringVehicle.FindMonitoringDetailSo(item.SalesOrderOncall.SONumber);
                Context.MasterProduct product = item.SalesOrderOncall.MasterProduct;
                Context.DataTruck truck = item.SalesOrderOncall.DataTruck;
                Context.Customer customer = item.SalesOrderOncall.Customer;
                Context.Rute rute = RepoSalesOrder.FindRute(item.SalesOrderOncall.IdDaftarHargaItem.Value);
                decimal suhu = (mitem == null || mitem.SuhuAvg == null ? 0 : decimal.Parse(mitem.SuhuAvg));
                if (item.SalesOrderOncallId.HasValue && RepoSettlementBatal.FindBySo(item.Id) != null && RepoSettlementBatal.FindBySo(item.Id).IdDriver == id)
                {
                    listModel.Add(new MonitoringOnduty(){
/*                        Muat = mitem == null ? "-" : (mitem.TglMuat - RepoSalesOrder.FindByOnCallCode(mitem.NoSo).SalesOrderOncall.TanggalMuat.Value).TotalHours > 1 ? "Late" : "On-Time",
                        Perjalanan = mitem == null ? "-" : (mitem.TglTiba - mitem.TargetTiba).TotalHours > 1 ? "Late" : "On-Time", Bongkar = "", Precooling = "", AcMati = "",
                        SuhuSesuai = suhu > product.MinTemps && suhu < product.MaxTemps ? "YA" : "TIDAK", Klaim = mitem == null ? "-" : RepoKlaim.FindBySoId(mitem.NoSo) == null ? "TIDAK" : "YA", JenisOrder = "On Call",
                        NoSo = item.SalesOrderOncall.SONumber, VehicleNo = truck.VehicleNo, JenisTruck = item.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck, CustomerNama = customer.CustomerNama,
                        Rute = item.SalesOrderOncall.StrDaftarHargaItem, JenisBarang = product.NamaProduk, TargetSuhu = item.SalesOrderOncall.MasterProduct.TargetSuhu,
                        TargetMuat = item.SalesOrderOncall.TanggalMuat.ToString(), TanggalTiba = mitem == null ? "-" : mitem.TglTiba.ToString(), TglBerangkat = mitem == null ? "-" : mitem.TglBerangkat.ToString(),
                        TargetWaktu = rute.WaktuTempuhJam + " Jam " + rute.WaktuTempuhMenit + " Menit", TargetTiba = mitem == null ? "-" : mitem.TargetTiba.ToString(),
                        Delay = mitem == null ? "-" : (mitem.TglTiba - mitem.TargetTiba).Hours + " h " + (mitem.TglTiba - mitem.TargetTiba).Minutes + " m", RangeSuhu = product.MinTemps + " - " + product.MaxTemps,
                        SuhuAvg = mitem == null ? "-" : mitem.SuhuAvg, AcOff = mitem == null ? 0 : mitem.AcOff*/
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                }
                else if (item.SalesOrderPickupId.HasValue)
                {
                }
                else if (item.SalesOrderKontrakId.HasValue)
                {
                }
            }
            return new JavaScriptSerializer().Serialize(new { listModel1 = listModel, listModel2 = listModel, listModel3 = listModel });
        }

        public string BindingDokumen(int id)
        {
            List<Context.Dokumen> dbdokumen = RepoDokumen.FindAll().Where(d => d.IsAdmin == false && (d.SalesOrder.SalesOrderOncall != null && d.SalesOrder.SalesOrderOncall.Driver1Id == id || d.SalesOrder.SalesOrderPickup != null && d.SalesOrder.SalesOrderPickup.Driver1Id == id || d.SalesOrder.SalesOrderProsesKonsolidasi != null && d.SalesOrder.SalesOrderProsesKonsolidasi.Driver1Id == id)).ToList();
            List<DriverHisTransfer> listModel1 = new List<DriverHisTransfer>();
            List<DriverHisTransfer> listModel2 = new List<DriverHisTransfer>();
            List<DriverHisTransfer> listModel3 = new List<DriverHisTransfer>();

            foreach (var item in dbdokumen)
            {
                var persentase = (decimal.Parse(item.DokumenItem.Count().ToString()) * 100) / item.DokumenItem.Where(d => d.IsLengkap).Count();
                if (item.SalesOrder.SalesOrderOncallId.HasValue)
                {
                    if (persentase >= 80)
                    {
                        listModel1.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Oncall",
                            NoSo = item.SalesOrder.SalesOrderOncall.SONumber,
                            NoPol = item.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderOncall.Customer.CustomerNama,
                            Rute = item.SalesOrder.SalesOrderOncall.StrDaftarHargaItem,
                            JnsBarang = item.SalesOrder.SalesOrderOncall.MasterProduct.NamaProduk,
                            TargetSuhu = item.SalesOrder.SalesOrderOncall.MasterProduct.TargetSuhu,
                            KetTf = "Lengkap",
                            Persentase = persentase
                        });
                    }
                    else if (persentase >= 40 && persentase < 80)
                    {
                        listModel2.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Oncall",
                            NoSo = item.SalesOrder.SalesOrderOncall.SONumber,
                            NoPol = item.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderOncall.Customer.CustomerNama,
                            Rute = item.SalesOrder.SalesOrderOncall.StrDaftarHargaItem,
                            JnsBarang = item.SalesOrder.SalesOrderOncall.MasterProduct.NamaProduk,
                            TargetSuhu = item.SalesOrder.SalesOrderOncall.MasterProduct.TargetSuhu,
                            KetTf = "Tidak Lengkap",
                            Persentase = persentase
                        });
                    }
                    else if (persentase < 40)
                    {
                        listModel3.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Oncall",
                            NoSo = item.SalesOrder.SalesOrderOncall.SONumber,
                            NoPol = item.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderOncall.Customer.CustomerNama,
                            Rute = item.SalesOrder.SalesOrderOncall.StrDaftarHargaItem,
                            JnsBarang = item.SalesOrder.SalesOrderOncall.MasterProduct.NamaProduk,
                            TargetSuhu = item.SalesOrder.SalesOrderOncall.MasterProduct.TargetSuhu,
                            KetTf = "Tidak Ada",
                            Persentase = persentase
                        });
                    }
                }
                else if (item.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    if (persentase >= 80)
                    {
                        listModel1.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Konsolidasi",
                            NoSo = item.SalesOrder.SalesOrderProsesKonsolidasi.SONumber,
                            NoPol = item.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = "",
                            Rute = item.SalesOrder.SalesOrderProsesKonsolidasi.StrDaftarHargaItem,
                            JnsBarang = "",
                            TargetSuhu = 0,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                    else if (persentase >= 40 && persentase < 80)
                    {
                        listModel2.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Konsolidasi",
                            NoSo = item.SalesOrder.SalesOrderProsesKonsolidasi.SONumber,
                            NoPol = item.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = "",
                            Rute = item.SalesOrder.SalesOrderProsesKonsolidasi.StrDaftarHargaItem,
                            JnsBarang = "",
                            TargetSuhu = 0,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                    else if (persentase < 40)
                    {
                        listModel3.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Konsolidasi",
                            NoSo = item.SalesOrder.SalesOrderProsesKonsolidasi.SONumber,
                            NoPol = item.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = "",
                            Rute = item.SalesOrder.SalesOrderProsesKonsolidasi.StrDaftarHargaItem,
                            JnsBarang = "",
                            TargetSuhu = 0,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                }
                else if (item.SalesOrder.SalesOrderPickupId.HasValue)
                {
                    if (persentase >= 80)
                    {
                        listModel1.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Pickup",
                            NoSo = item.SalesOrder.SalesOrderPickup.SONumber,
                            NoPol = item.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderPickup.Customer.CustomerNama,
                            Rute = item.SalesOrder.SalesOrderPickup.Rute.Nama,
                            JnsBarang = item.SalesOrder.SalesOrderPickup.MasterProduct.NamaProduk,
                            TargetSuhu = item.SalesOrder.SalesOrderPickup.MasterProduct.TargetSuhu,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                    else if (persentase >= 40 && persentase < 80)
                    {
                        listModel2.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Pickup",
                            NoSo = item.SalesOrder.SalesOrderPickup.SONumber,
                            NoPol = item.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderPickup.Customer.CustomerNama,
                            Rute = item.SalesOrder.SalesOrderPickup.Rute.Nama,
                            JnsBarang = item.SalesOrder.SalesOrderPickup.MasterProduct.NamaProduk,
                            TargetSuhu = item.SalesOrder.SalesOrderPickup.MasterProduct.TargetSuhu,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                    else if (persentase < 40)
                    {
                        listModel3.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Pickup",
                            NoSo = item.SalesOrder.SalesOrderPickup.SONumber,
                            NoPol = item.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo,
                            JnsTruck = item.SalesOrder.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderPickup.Customer.CustomerNama,
                            Rute = item.SalesOrder.SalesOrderPickup.Rute.Nama,
                            JnsBarang = item.SalesOrder.SalesOrderPickup.MasterProduct.NamaProduk,
                            TargetSuhu = item.SalesOrder.SalesOrderPickup.MasterProduct.TargetSuhu,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                }
                else if (item.SalesOrder.SalesOrderKontrakId.HasValue)
                {
                    List<int> dummyId = item.ListIdSo.Split(',').Select(int.Parse).ToList();
                    var data = item.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => dummyId.Contains(d.Id)).ToList();
                    if (persentase >= 80)
                    {
                        listModel1.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Kontrak",
                            NoSo = string.Join(", ", data.Select(d => d.NoSo)),
                            NoPol = data.FirstOrDefault().DataTruck.VehicleNo,
                            JnsTruck = data.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderKontrak.Customer.CustomerNama,
                            Rute = "",
                            JnsBarang = "",
                            TargetSuhu = 0,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                    else if (persentase >= 40 && persentase < 80)
                    {
                        listModel2.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Kontrak",
                            NoSo = string.Join(", ", data.Select(d => d.NoSo)),
                            NoPol = data.FirstOrDefault().DataTruck.VehicleNo,
                            JnsTruck = data.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderKontrak.Customer.CustomerNama,
                            Rute = "",
                            JnsBarang = "",
                            TargetSuhu = 0,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                    else if (persentase < 40)
                    {
                        listModel3.Add(new DriverHisTransfer()
                        {
                            JnsSo = "Kontrak",
                            NoSo = string.Join(", ", data.Select(d => d.NoSo)),
                            NoPol = data.FirstOrDefault().DataTruck.VehicleNo,
                            JnsTruck = data.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck,
                            Customer = item.SalesOrder.SalesOrderKontrak.Customer.CustomerNama,
                            Rute = "",
                            JnsBarang = "",
                            TargetSuhu = 0,
                            KetTf = item.IsComplete ? "Lengkap" : "Tidak Lengkap",
                        });
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(new { listModel1 = listModel1, listModel2 = listModel2, listModel3 = listModel3 });
        }

        public string BindingJasa(int idDriver)
        {
            Context.Driver dbitem = RepoDriver.FindByPK(idDriver);

            return new JavaScriptSerializer().Serialize(dbitem.DriverJasa.Select(d => new { Id = d.Id, Tanggal = d.Tanggal, Keterangan = d.keterangan }).ToList());
        }

        [HttpPost]
        [MyAuthorize(Menu = "Data Driver", Action = "delete")]
        public JsonResult DeleteInventaris(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Inventaris dbItem = RepoInventaris.FindByPK(id);

            RepoInventaris.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        [HttpPost]
        public JsonResult SaveInventaris(Inventaris model)
        {
            Context.Driver dbitem = RepoDriver.FindByPK(model.DriverId);
            Context.Inventaris dbjasa = dbitem.Inventaris.Where(d => d.Id == model.Id).FirstOrDefault();
            if (dbjasa == null)
                dbjasa = new Context.Inventaris();

            model.setDb(dbjasa);
            if (model.Id == 0)
                dbitem.Inventaris.Add(dbjasa);

            RepoDriver.save(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        [HttpPost]
        public JsonResult SaveJasa(JasaDriver model)
        {
            Context.Driver dbitem = RepoDriver.FindByPK(model.IdDriver);
            Context.DriverJasa dbjasa = dbitem.DriverJasa.Where(d => d.Id == model.Id).FirstOrDefault();
            if (dbjasa == null)
                dbjasa = new Context.DriverJasa();

            dbjasa.Tanggal = model.TanggalJasa;
            dbjasa.keterangan = model.keterangan;
            if (model.Id == 0){
                dbitem.DriverJasa.Add(dbjasa);
                RepoAuditrail.SetAuditTrail(
                    "INSERT INTO dbo.\"DriverJasa\" (\"IdDriver\", \"Tanggal\", keterangan) VALUES (" + dbjasa.IdDriver + ", " + dbjasa.Tanggal + ", " + dbjasa.keterangan + ");", "Driver Jasa", "Add", UserPrincipal.id);
            }
            else{
                RepoAuditrail.SetAuditTrail("UPDATE dbo.\"DriverJasa\" SET \"Tanggal\" = "+dbjasa.Tanggal+", keterangan = "+dbjasa.keterangan+" WHERE \"Id\" = "+dbjasa.Id+";", "Driver Jasa", "Edit", UserPrincipal.id);
            }

            RepoDriver.save(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        public string BindingDetailSo()
        {
            List<Context.Driver> items = RepoDriver.FindAll().Where(d => d.LookupCodeStatus.Nama != "NON ACTIVE").ToList();
            List<Driver> ListModel = new List<Driver>();
            foreach (Context.Driver item in items)
            {
                ListModel.Add(new Driver(item));
            }

            int total = RepoDriver.Count();

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string CheckDriverAvailability(int id)
        {
            List<Context.Driver> items = RepoDriver.FindAll().Where(d => d.LookupCodeStatus.Nama != "NON ACTIVE" && d.Id == id).ToList();
            List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().ToList();
            List<Context.SettlementBatal> dbsb = RepoSettlementBatal.FindAll().Where(s => s.IsProses == false).ToList();

            List<Driver> ListModel = new List<Driver>();
            foreach (Context.Driver item in items)
            {
                ListModel.Add(new Driver(item, dbso, dbsb));
            }

            return new JavaScriptSerializer().Serialize(new { data = ListModel.FirstOrDefault() });
        }

        [MyAuthorize(Menu = "Data Driver", Action = "create")]
        public ActionResult Add()
        {
            Driver model = new Driver();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(Driver model)
        {
            if (ModelState.IsValid)
            {
                if (RepoDriver.IsKtpExist(model.NoKtp))
                {

                }
                Context.Driver dbitem = new Context.Driver();
                model.setDb(dbitem);
                //generate code
                dbitem.Urutan = RepoDriver.getUrutan(DateTime.Parse(dbitem.TglGabung.Value.ToString())) + 1;
                dbitem.KodeDriver = RepoDriver.generateCode(DateTime.Parse(dbitem.TglGabung.Value.ToString()), dbitem.Urutan);
                Context.DriverStatusHistory dbstathistory = new Context.DriverStatusHistory();
                model.setHistoryStatus(dbstathistory, RepoLookup.FindByPK(model.IdStatus.Value).Nama);
                dbitem.DriverStatusHistory.Add(dbstathistory);
                RepoDriver.save(dbitem);
                string query = "INSERT INTO dbo.\"Driver\" (\"IdStatus\", \"KodeDriver\", \"KodeDriverOld\", \"TglGabung\", \"NoKtp\", \"NamaDriver\", \"NamaPangilan\", \"TempatLahir\", \"TglLahir\", \"IdJenisSim\", " +
                    "\"NoSim\", \"TglBerlakuSim\", \"NoTlp\", \"NoHp1\", \"NoHp2\", \"Alamat\", \"Rt\", \"Rw\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\", \"IsSameKtp\", \"AlamatDomisili\", \"RtDomisili\", " +
                    "\"RwDomisili\", \"IdProvinsiDomisili\", \"IdKabKotaDomisili\", \"IdKecDomisili\", \"IdKelDomisili\", \"Keterangan\", \"IsSms\", \"Pathfoto\", \"IdReferensiDriver\", \"IdRef\", \"HubunganRef\", " +
                    "\"KeteranganRef\", \"IsKemitraan\", \"IsKemitraanAsli\", \"UrlKemitraan\", \"IsJaminanKel\", \"IsJaminanKelAsli\", \"UrlJaminanKel\", \"IsIjazah\", \"IsIjazahAsli\", \"UrlIjazah\", \"IsBukuNikah\", " +
                    "\"IsBukuNikahAsli\", \"UrlBukuNikah\", \"IsSKCK\", \"IsSKCKAsli\", \"UrlSKCK\", \"IsDomisili\", \"IsDomisiliAsli\", \"UrlDomisili\", \"IsKK\", \"IsKKAsli\", \"UrlKK\", \"IsKTP\", \"IsKTPAsli\", " +
                    "\"UrlKTP\", \"IsSIM\", \"IsSIMAsli\", \"UrlSIM\", \"Urutan\", \"SaldoPiutang\", \"SaldoKlaim\", \"LastPbyOid\", \"LastSync\") VALUES (" + dbitem.IdStatus + ", " + dbitem.KodeDriver + ", " + 
                    dbitem.KodeDriverOld + ", " + dbitem.TglGabung + ", " + dbitem.NoKtp + ", " + dbitem.NamaDriver + ", " + dbitem.NamaPangilan + ", " + dbitem.TempatLahir + ", " + dbitem.TglLahir + ", " +
                    dbitem.IdJenisSim + ", " + dbitem.NoSim + ", " + dbitem.TglBerlakuSim + ", " + dbitem.NoTlp + ", " + dbitem.NoHp1 + ", " + dbitem.NoHp2 + ", " + dbitem.Alamat + ", " + dbitem.Rt + ", " + dbitem.Rw +
                    ", " + dbitem.IdProvinsi + ", " + dbitem.IdKabKota + ", " + dbitem.IdKec + ", " + dbitem.IdKel + ", " + dbitem.IsSameKtp + ", " + dbitem.AlamatDomisili + ", " + dbitem.RtDomisili + ", " +
                    dbitem.RwDomisili + ", " + dbitem.IdProvinsiDomisili + ", " + dbitem.IdKabKotaDomisili + ", " + dbitem.IdKecDomisili + ", " + dbitem.IdKelDomisili + ", " + dbitem.Keterangan + ", " + dbitem.IsSms + ", " +
                    dbitem.Pathfoto + ", " + dbitem.IdReferensiDriver + ", " + dbitem.IdRef + ", " + dbitem.HubunganRef + ", " + dbitem.KeteranganRef + ", " + dbitem.IsKemitraan + ", " + dbitem.IsKemitraanAsli + ", " +
                    dbitem.UrlKemitraan + ", " + dbitem.IsJaminanKel + ", " + dbitem.IsJaminanKelAsli + ", " + dbitem.UrlJaminanKel + ", " + dbitem.IsIjazah + ", " + dbitem.IsIjazahAsli + ", " + dbitem.UrlIjazah + ", " +
                    dbitem.IsBukuNikah + ", " + dbitem.IsBukuNikahAsli + ", " + dbitem.UrlBukuNikah + ", " + dbitem.IsSKCK + ", " + dbitem.IsSKCKAsli + ", " + dbitem.UrlSKCK + ", " + dbitem.IsDomisili + ", " +
                    dbitem.IsDomisiliAsli + ", " + dbitem.UrlDomisili + ", " + dbitem.IsKK + ", " + dbitem.IsKKAsli + ", " + dbitem.UrlKK + ", " + dbitem.IsKTP + ", " + dbitem.IsKTPAsli + ", " + dbitem.UrlKTP + ", " +
                    dbitem.IsSIM + ", " + dbitem.IsSIMAsli + ", " + dbitem.IsSIMAsli + ", " + dbitem.UrlSIM + ", " + dbitem.Urutan + ", " + dbitem.SaldoPiutang + ", " + dbitem.SaldoKlaim + ", " + dbitem.LastPbyOid + ", " +
                    dbitem.LastSync + ");INSERT INTO dbo.\"DriverStatusHistory\" (\"IdDriver\", \"Tanggal\", \"Status\", keterangan) VALUES (" + dbstathistory.IdDriver + ", " +  dbstathistory.Tanggal + ", " + 
                    dbstathistory.Status + ", " +  dbstathistory.keterangan + ");";
                RepoAuditrail.SetAuditTrail(query, "Driver", "Add", UserPrincipal.id);
                sync_one_driver(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Data Driver", Action = "update")]
        public ActionResult Edit(int id)
        {
            Context.Driver dbitem = RepoDriver.FindByPK(id);
            Driver model = new Driver(dbitem);
            ViewBag.name = model.KodeDriver;
            return View("Form", model);
        }

        [MyAuthorize(Menu = "Data Driver", Action = "read")]
        public ActionResult View(int id)
        {
            Context.Driver dbitem = RepoDriver.FindByPK(id);
            Driver model = new Driver(dbitem);
            ViewBag.name = model.KodeDriver;
            return View("View", model);
        }

        public ActionResult sync_driver()
        {
            foreach (Context.Driver dbitem in RepoDriver.FindAll())
            {
                if (Repoptnr_mstr.FindByPK(dbitem.Id) == null)
                {
                    Repoptnr_mstr.saveDriver(dbitem);
                }
                else
                {
                    Context.ptnr_mstr dbptnr = Repoptnr_mstr.FindByPK(dbitem.Id);
                    dbptnr.ptnr_code = dbitem.KodeDriver;
                    dbptnr.ptnr_name = dbitem.NamaDriver;
                    Repoptnr_mstr.updateCustomer(dbptnr);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Driver model)
        {
            if (ModelState.IsValid)
            {
                Context.Driver dbitem = RepoDriver.FindByPK(model.Id);
                Context.DriverStatusHistory dbstathistory = new Context.DriverStatusHistory();
                if (model.IdStatus != dbitem.IdStatus)
                {
                    model.setDb(dbitem);
                    model.setHistoryStatus(dbstathistory, RepoLookup.FindByPK(model.IdStatus.Value).Nama);
                    dbitem.DriverStatusHistory.Add(dbstathistory);
                }
                else
                {
                    model.setDb(dbitem);
                }

                if (Repoptnr_mstr.FindByPK(dbitem.Id+7000000) == null)
                {
                    Repoptnr_mstr.saveDriver(dbitem);
                }
                else
                {
                    Context.ptnr_mstr dbptnr = Repoptnr_mstr.FindByPK(dbitem.Id + 7000000);
                    dbptnr.ptnr_name = dbitem.NamaDriver + " - " + dbitem.KodeDriver;
                    Repoptnr_mstr.updateCustomer(dbptnr);
                }
                RepoDriver.save(dbitem);
                RepoAuditrail.SetAuditTrail(
                    "UPDATE dbo.\"Driver\" SET \"IdStatus\" = " + dbitem.IdStatus + ", \"KodeDriver\" = " + dbitem.KodeDriver + ", \"KodeDriverOld\" = " + dbitem.KodeDriverOld + ", \"TglGabung\" = " + dbitem.TglGabung +
                     ", \"NoKtp\" = " + dbitem.NoKtp + ", \"NamaDriver\" = " + dbitem.NamaDriver + ", \"NamaPangilan\" = " + dbitem.NamaPangilan + ", \"TempatLahir\" = " + dbitem.TempatLahir + ", \"TglLahir\" = " +
                     dbitem.TglLahir + ", \"IdJenisSim\" = " + dbitem.IdJenisSim + ", \"NoSim\" = " + dbitem.NoSim + ", \"TglBerlakuSim\" = " + dbitem.TglBerlakuSim + ", \"NoTlp\" = " + dbitem.NoTlp + ", \"NoHp1\" = " +
                     dbitem.NoHp1 + ", \"NoHp2\" = " + dbitem.NoHp2 + ", \"Alamat\" = " + dbitem.Alamat + ", \"Rt\" = " + dbitem.Rt + ", \"Rw\" = " + dbitem.Rw + ", \"IdProvinsi\" = " + dbitem.IdProvinsi +
                     ", \"IdKabKota\" = " + dbitem.IdKabKota + ", \"IdKec\" = " + dbitem.IdKec + ", \"IdKel\" = " + dbitem.IdKel + ", \"IsSameKtp\" = " + dbitem.IsSameKtp + ", \"AlamatDomisili\" = " + 
                     dbitem.AlamatDomisili + ", \"RtDomisili\" = " + dbitem.RtDomisili + ", \"RwDomisili\" = " + dbitem.RwDomisili + ", \"IdProvinsiDomisili\" = " + dbitem.IdProvinsiDomisili + ", \"IdKabKotaDomisili\" = " +
                     dbitem.IdKabKotaDomisili + ", \"IdKecDomisili\" = " + dbitem.IdKecDomisili + ", \"IdKelDomisili\" = " + dbitem.IdKelDomisili + ", \"Keterangan\" = " + dbitem.Keterangan + ", \"IsSms\" = " + dbitem.IsSms +
                     ", \"Pathfoto\" = " + dbitem.Pathfoto + ", \"IdReferensiDriver\" = " + dbitem.IdReferensiDriver + ", \"IdRef\" = " + dbitem.IdRef + ", \"HubunganRef\" = " + dbitem.HubunganRef + ", \"KeteranganRef\" = " +
                     dbitem.KeteranganRef + ", \"IsKemitraan\" = " + dbitem.IsKemitraan + ", \"IsKemitraanAsli\" = " + dbitem.IsKemitraanAsli + ", \"UrlKemitraan\" = " + dbitem.UrlKemitraan + ", \"IsJaminanKel\" = " +
                     dbitem.IsJaminanKel + ", \"IsJaminanKelAsli\" = " + dbitem.IsJaminanKelAsli + ", \"UrlJaminanKel\" = " + dbitem.UrlJaminanKel + ", \"IsIjazah\" = " + dbitem.IsIjazah + ", \"IsIjazahAsli\" = " +
                     dbitem.IsIjazahAsli + ", \"UrlIjazah\" = " + dbitem.UrlIjazah + ", \"IsBukuNikah\" = " + dbitem.IsBukuNikah + ", \"IsBukuNikahAsli\" = " + dbitem.IsBukuNikahAsli + ", \"UrlBukuNikah\" = " +
                     dbitem.UrlBukuNikah + ", \"IsSKCK\" = " + dbitem.IsSKCK + ", \"IsSKCKAsli\" = " + dbitem.IsSKCKAsli + ", \"UrlSKCK\" = " + dbitem.UrlSKCK + ", \"IsDomisili\" = " + dbitem.IsDomisili +
                     ", \"IsDomisiliAsli\" = " + dbitem.IsDomisiliAsli + ", \"UrlDomisili\" = " + dbitem.UrlDomisili + ", \"IsKK\" = " + dbitem.IsKK + ", \"IsKKAsli\" = " + dbitem.IsKKAsli + ", \"UrlKK\" = " + dbitem.UrlKK +
                     ", \"IsKTP\" = " + dbitem.IsKTP + ", \"IsKTPAsli\" = " + dbitem.IsKTPAsli + ", \"UrlKTP\" = " + dbitem.UrlKTP + ", \"IsSIM\" = " + dbitem.IsSIM + ", \"IsSIMAsli\" = " + dbitem.IsSIMAsli +
                     ", \"UrlSIM\" = " + dbitem.UrlSIM + ", \"Urutan\" = " + dbitem.Urutan + ", \"SaldoPiutang\" = " + dbitem.SaldoPiutang + ", \"SaldoKlaim\" = " + dbitem.SaldoKlaim + ", \"LastPbyOid\" = " +
                     dbitem.LastPbyOid + ", \"LastSync\" = " + dbitem.LastSync + " WHERE \"Id\" = " + dbitem.Id + ";INSERT INTO dbo.\"DriverStatusHistory\" (\"IdDriver\", \"Tanggal\", \"Status\", keterangan) VALUES (" +
                     dbstathistory.IdDriver + ", " +  dbstathistory.Tanggal + ", " + dbstathistory.Status + ", " +  dbstathistory.keterangan + ";", "Driver", "Edit", UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Driver dbItem = RepoDriver.FindByPK(id);

            RepoDriver.delete(dbItem);
            RepoAuditrail.SetAuditTrail("DELETE FROM dbo.\"Driver\" WHERE \"Id\" = " + dbItem.Id + ";", "Driver", "Delete", UserPrincipal.id);

            return Json(response);
        }
        [HttpPost]
        public JsonResult DeleteJasa(int idDriver, int id)
        {
            Context.Driver dbItem = RepoDriver.FindByPK(idDriver);
            dbItem.DriverJasa = dbItem.DriverJasa.Where(d => d.Id != id).ToList();

            RepoDriver.save(dbItem);
            RepoAuditrail.SetAuditTrail("DELETE FROM dbo.\"DriverJasa\" WHERE \"Id\" = " + dbItem.Id + ";", "Driver Jasa", "Delete", UserPrincipal.id);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        public string GetAlert(int id)
        {
            Driver model = new Driver(RepoDriver.FindByPK(id));

            List<DriverAlert> alert = new List<DriverAlert>();

            if (model.TglBerlakuSim.HasValue)
            {
                if (model.TglBerlakuSim <= DateTime.Now)
                {
                    alert.Add(new DriverAlert() { Keterangan = "Sim Expired", Value = model.TglBerlakuSim.Value.ToShortDateString() });
                }
            }

            foreach (Context.TrainingSetting ts in RepoTraining.FindAll()){
                if (RepoPelaksanaanTraining.FindAll().Where(d => d.PelaksanaanTrainingDetail.Any(e => e.IdDriver == id) && d.IdTrainingSetting == ts.Id).Count() < 1)
                    alert.Add(new DriverAlert() { Keterangan = "Training", Value = ts.Nama });
                else if (RepoPelaksanaanTraining.FindAll().Where(d => d.PelaksanaanTrainingDetail.Any(e => e.IdDriver == id) && d.IdTrainingSetting == ts.Id && DateTime.Now.AddDays(ts.Interval*-30) > d.Tanggal).Count() > 0)
                    alert.Add(new DriverAlert() { Keterangan = "Training Pengulangan", Value = ts.Nama });
            }
            return new JavaScriptSerializer().Serialize(alert);
        }
        #region export import

        public void sync_one_driver(Context.Driver dbitem)
        {
            if (Repoptnr_mstr.FindByPK(dbitem.Id+7000000) == null)
            {
                Repoptnr_mstr.saveDriver(dbitem);
            }
            else
            {
                Context.ptnr_mstr dbptnr = Repoptnr_mstr.FindByPK(dbitem.Id + 7000000);
                dbptnr.ptnr_code = dbitem.KodeDriver;
                dbptnr.ptnr_name = dbitem.NamaDriver;
                Repoptnr_mstr.updateCustomer(dbptnr);
            }

        }

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
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null &&
                                    workSheet.Cells[rowIterator, 6].Value != null && workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 9].Value != null &&
                                    workSheet.Cells[rowIterator, 10].Value != null && workSheet.Cells[rowIterator, 11].Value != null && workSheet.Cells[rowIterator, 12].Value != null
                                    && workSheet.Cells[rowIterator, 14].Value != null && workSheet.Cells[rowIterator, 16].Value != null)
                                {
                                    int id = 0;
                                    int resId;
                                    if (workSheet.Cells[rowIterator, 35].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 35].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    Context.Driver dbitem = new Context.Driver();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoDriver.FindByPK(id);
                                        }
                                        else
                                        {
                                            dbitem.Urutan = RepoDriver.getUrutan(DateTime.Parse(workSheet.Cells[rowIterator, 4].Value.ToString())) + 1;
                                            dbitem.KodeDriver = RepoDriver.generateCode(DateTime.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()), dbitem.Urutan);
                                        }

                                        dbitem.IdStatus = RepoLookup.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 2].Value != null)
                                            dbitem.Keterangan = workSheet.Cells[rowIterator, 2].Value.ToString();
                                        if (workSheet.Cells[rowIterator, 3].Value != null)
                                        {
                                            bool isBoolean;
                                            if (bool.TryParse(workSheet.Cells[rowIterator, 3].Value.ToString(), out isBoolean))
                                            {
                                                dbitem.IsSms = isBoolean;
                                            }
                                        }

                                        dbitem.TglGabung = DateTime.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                        dbitem.NoKtp = workSheet.Cells[rowIterator, 5].Value.ToString();
                                        dbitem.NamaDriver = workSheet.Cells[rowIterator, 6].Value.ToString();
                                        dbitem.NamaPangilan = workSheet.Cells[rowIterator, 7].Value == null ? "" : workSheet.Cells[rowIterator, 7].Value.ToString();
                                        dbitem.TempatLahir = workSheet.Cells[rowIterator, 8].Value.ToString();
                                        dbitem.TglLahir = DateTime.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                        if (RepoLookup.FindByName(workSheet.Cells[rowIterator, 10].Value.ToString()) != null)
                                            dbitem.IdJenisSim = RepoLookup.FindByName(workSheet.Cells[rowIterator, 10].Value.ToString()).Id;
                                        dbitem.NoSim = workSheet.Cells[rowIterator, 11].Value.ToString();
                                        dbitem.TglBerlakuSim = DateTime.Parse(workSheet.Cells[rowIterator, 12].Value.ToString());
                                        dbitem.NoTlp = workSheet.Cells[rowIterator, 13].Value == null ? "" : workSheet.Cells[rowIterator, 13].Value.ToString();
                                        dbitem.NoHp1 = workSheet.Cells[rowIterator, 14].Value.ToString();
                                        dbitem.NoHp2 = workSheet.Cells[rowIterator, 15].Value == null ? "" : workSheet.Cells[rowIterator, 15].Value.ToString();
                                        dbitem.Alamat = workSheet.Cells[rowIterator, 16].Value == null ? "" : workSheet.Cells[rowIterator, 16].Value.ToString();
                                        dbitem.Rt = workSheet.Cells[rowIterator, 17].Value == null ? "" : workSheet.Cells[rowIterator, 17].Value.ToString();
                                        dbitem.Rw = workSheet.Cells[rowIterator, 18].Value == null ? "" : workSheet.Cells[rowIterator, 18].Value.ToString();
                                        if (workSheet.Cells[rowIterator, 19].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 19].Value.ToString()) != null)
                                            dbitem.IdProvinsi = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 19].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 20].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 20].Value.ToString()) != null)
                                            dbitem.IdKabKota = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 20].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 21].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 21].Value.ToString()) != null)
                                            dbitem.IdKec = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 21].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 22].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 22].Value.ToString()) != null)
                                            dbitem.IdKel = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 22].Value.ToString()).Id;
                                        if (workSheet.Cells[rowIterator, 23].Value != null)
                                        {
                                            bool IsBoolean;
                                            if (bool.TryParse(workSheet.Cells[rowIterator, 23].Value.ToString(), out IsBoolean))
                                            {
                                                dbitem.IsSameKtp = IsBoolean;
                                            }
                                        }
                                        if (dbitem.IsSameKtp)
                                        {
                                            dbitem.AlamatDomisili = dbitem.Alamat;
                                            dbitem.RtDomisili = dbitem.Rt;
                                            dbitem.RwDomisili = dbitem.Rw;
                                            dbitem.IdProvinsiDomisili = dbitem.IdProvinsi;
                                            dbitem.IdKabKotaDomisili = dbitem.IdKabKota;
                                            dbitem.IdKecDomisili = dbitem.IdKec;
                                            dbitem.IdKelDomisili = dbitem.IdKel;
                                        }
                                        else
                                        {
                                            if (workSheet.Cells[rowIterator, 24].Value != null)
                                            {
                                                dbitem.AlamatDomisili = workSheet.Cells[rowIterator, 24].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 25].Value != null)
                                            {
                                                dbitem.RtDomisili = workSheet.Cells[rowIterator, 25].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 26].Value != null)
                                            {
                                                dbitem.RwDomisili = workSheet.Cells[rowIterator, 26].Value.ToString();
                                            }
                                            if (workSheet.Cells[rowIterator, 27].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 27].Value.ToString()) != null)
                                            {
                                                dbitem.IdProvinsiDomisili = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 27].Value.ToString()).Id;
                                            }
                                            if (workSheet.Cells[rowIterator, 28].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 28].Value.ToString()) != null)
                                            {
                                                dbitem.IdKabKotaDomisili = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 28].Value.ToString()).Id;
                                            }
                                            if (workSheet.Cells[rowIterator, 29].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 29].Value.ToString()) != null)
                                            {
                                                dbitem.IdKecDomisili = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 29].Value.ToString()).Id;
                                            }
                                            if (workSheet.Cells[rowIterator, 30].Value != null && RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 30].Value.ToString()) != null)
                                            {
                                                dbitem.IdKelDomisili = RepoLokasi.FindByNama(workSheet.Cells[rowIterator, 30].Value.ToString()).Id;
                                            }
                                        }
                                        if (workSheet.Cells[rowIterator, 31].Value != null && RepoLookup.FindByName(workSheet.Cells[rowIterator, 31].Value.ToString()) != null)
                                        {
                                            dbitem.IdReferensiDriver = RepoLookup.FindByName(workSheet.Cells[rowIterator, 31].Value.ToString()).Id;
                                        }
                                        if (dbitem.IdReferensiDriver.HasValue && RepoDriver.FindByCode(workSheet.Cells[rowIterator, 32].Value.ToString()) != null)
                                        {
                                            dbitem.IdRef = RepoDriver.FindByCode(workSheet.Cells[rowIterator, 32].Value.ToString()).Id;
                                        }
                                        dbitem.HubunganRef = workSheet.Cells[rowIterator, 33].Value == null ? "" : workSheet.Cells[rowIterator, 33].Value.ToString();
                                        dbitem.KeteranganRef = workSheet.Cells[rowIterator, 34].Value == null ? "" : workSheet.Cells[rowIterator, 34].Value.ToString();
                                        dbitem.KodeDriverOld = workSheet.Cells[rowIterator, 35].Value == null ? "" : workSheet.Cells[rowIterator, 35].Value.ToString();

                                        Context.DriverStatusHistory dbstathistory = new Context.DriverStatusHistory();
                                        dbstathistory.Tanggal = DateTime.Now;
                                        dbstathistory.Status = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbstathistory.keterangan = dbitem.Keterangan;

                                        dbitem.DriverStatusHistory.Add(dbstathistory);

                                        RepoDriver.save(dbitem);
                                        sync_one_driver(dbitem);
    RepoAuditrail.SetAuditTrail(
        "INSERT INTO dbo.\"Driver\" (\"IdStatus\", \"KodeDriver\", \"KodeDriverOld\", \"TglGabung\", \"NoKtp\", \"NamaDriver\", \"NamaPangilan\", \"TempatLahir\", \"TglLahir\", \"IdJenisSim\", \"NoSim\", " +
        "\"TglBerlakuSim\", \"NoTlp\", \"NoHp1\", \"NoHp2\", \"Alamat\", \"Rt\", \"Rw\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\", \"IsSameKtp\", \"AlamatDomisili\", \"RtDomisili\", \"RwDomisili\", " +
        "\"IdProvinsiDomisili\", \"IdKabKotaDomisili\", \"IdKecDomisili\", \"IdKelDomisili\", \"Keterangan\", \"IsSms\", \"Pathfoto\", \"IdReferensiDriver\", \"IdRef\", \"HubunganRef\", \"KeteranganRef\", " +
        "\"IsKemitraan\", \"IsKemitraanAsli\", \"UrlKemitraan\", \"IsJaminanKel\", \"IsJaminanKelAsli\", \"UrlJaminanKel\", \"IsIjazah\", \"IsIjazahAsli\", \"UrlIjazah\", \"IsBukuNikah\", \"IsBukuNikahAsli\", " +
        "\"UrlBukuNikah\", \"IsSKCK\", \"IsSKCKAsli\", \"UrlSKCK\", \"IsDomisili\", \"IsDomisiliAsli\", \"UrlDomisili\", \"IsKK\", \"IsKKAsli\", \"UrlKK\", \"IsKTP\", \"IsKTPAsli\", \"UrlKTP\", \"IsSIM\", " +
        "\"IsSIMAsli\", \"UrlSIM\", \"Urutan\", \"SaldoPiutang\", \"SaldoKlaim\", \"LastPbyOid\", \"LastSync\") VALUES (" + dbitem.IdStatus + ", " + dbitem.KodeDriver + ", " + dbitem.KodeDriverOld + ", " + 
        dbitem.TglGabung + ", " + dbitem.NoKtp + ", " + dbitem.NamaDriver + ", " + dbitem.NamaPangilan + ", " + dbitem.TempatLahir + ", " + dbitem.TglLahir + ", " + dbitem.IdJenisSim + ", " + dbitem.NoSim + 
        ", " + dbitem.TglBerlakuSim + ", " + dbitem.NoTlp + ", " + dbitem.NoHp1 + ", " + dbitem.NoHp2 + ", " + dbitem.Alamat + ", " + dbitem.Rt + ", " + dbitem.Rw + ", " + dbitem.IdProvinsi + ", " + 
        dbitem.IdKabKota + ", " + dbitem.IdKec + ", " + dbitem.IdKel + ", " + dbitem.IsSameKtp + ", " + dbitem.AlamatDomisili + ", " + dbitem.RtDomisili + ", " + dbitem.RwDomisili + ", " +
        dbitem.IdProvinsiDomisili + ", " + dbitem.IdKabKotaDomisili + ", " + dbitem.IdKecDomisili + ", " + dbitem.IdKelDomisili + ", " + dbitem.Keterangan + ", " + dbitem.IsSms + ", " + dbitem.Pathfoto + ", " +
        dbitem.IdReferensiDriver + ", " + dbitem.IdRef + ", " + dbitem.HubunganRef + ", " + dbitem.KeteranganRef + ", " + dbitem.IsKemitraan + ", " + dbitem.IsKemitraanAsli + ", " + dbitem.UrlKemitraan + ", " + 
        dbitem.IsJaminanKel + ", " + dbitem.IsJaminanKelAsli + ", " + dbitem.UrlJaminanKel + ", " + dbitem.IsIjazah + ", " + dbitem.IsIjazahAsli + ", " + dbitem.UrlIjazah + ", " + dbitem.IsBukuNikah + ", " + 
        dbitem.IsBukuNikahAsli + ", " + dbitem.UrlBukuNikah + ", " + dbitem.IsSKCK + ", " + dbitem.IsSKCKAsli + ", " + dbitem.UrlSKCK + ", " + dbitem.IsDomisili + ", " + dbitem.IsDomisiliAsli + ", " + 
        dbitem.UrlDomisili + ", " + dbitem.IsKK + ", " + dbitem.IsKKAsli + ", " + dbitem.UrlKK + ", " + dbitem.IsKTP + ", " + dbitem.IsKTPAsli + ", " + dbitem.UrlKTP + ", " + dbitem.IsSIM + ", " + dbitem.IsSIMAsli + ", " + 
        dbitem.IsSIMAsli + ", " + dbitem.UrlSIM + ", " + dbitem.Urutan + ", " + dbitem.SaldoPiutang + ", " + dbitem.SaldoKlaim + ", " + dbitem.LastPbyOid + ", " + dbitem.LastSync +
        ");INSERT INTO dbo.\"DriverStatusHistory\" (\"IdDriver\", \"Tanggal\", \"Status\", keterangan) VALUES (" + dbstathistory.IdDriver + ", " +  dbstathistory.Tanggal + ", " + dbstathistory.Status + ", " +
        dbstathistory.keterangan + ");", "Driver", "Add", UserPrincipal.id);
                                    }
                                    catch (Exception)
                                    {
                                        response.Success = true;
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

        [MyAuthorize(Menu = "Data Driver", Action = "read")]
        public FileContentResult Export()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Driver> dbitems = RepoDriver.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Basic Info");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Status";
            ws.Cells[1, 2].Value = "Keterangan";
            ws.Cells[1, 3].Value = "Sms Notifikasi";
            ws.Cells[1, 4].Value = "Tanggal Bergabung";
            ws.Cells[1, 5].Value = "No Ktp";
            ws.Cells[1, 6].Value = "Nama";
            ws.Cells[1, 7].Value = "Panggilan";
            ws.Cells[1, 8].Value = "Tempat Lahir";
            ws.Cells[1, 9].Value = "Tanggal Lahir";
            ws.Cells[1, 10].Value = "Jenis Sim";
            ws.Cells[1, 11].Value = "No Sim";
            ws.Cells[1, 12].Value = "Masa Berlaku";
            ws.Cells[1, 13].Value = "No Tlp";
            ws.Cells[1, 14].Value = "No Hp 1";
            ws.Cells[1, 15].Value = "No Hp 2";
            ws.Cells[1, 16].Value = "Alamat KTP";
            ws.Cells[1, 17].Value = "Rt";
            ws.Cells[1, 18].Value = "Rw";
            ws.Cells[1, 19].Value = "Provinsi";
            ws.Cells[1, 20].Value = "Kota / Kabupaten";
            ws.Cells[1, 21].Value = "Kecamatan";
            ws.Cells[1, 22].Value = "Kelurahan";
            ws.Cells[1, 23].Value = "Sama Dengan KTP";
            ws.Cells[1, 24].Value = "Alamat Domisili";
            ws.Cells[1, 25].Value = "Rt";
            ws.Cells[1, 26].Value = "Rw";
            ws.Cells[1, 27].Value = "Provinsi";
            ws.Cells[1, 28].Value = "Kota / Kabupaten";
            ws.Cells[1, 29].Value = "Kecamatan";
            ws.Cells[1, 30].Value = "Kelurahan";
            ws.Cells[1, 31].Value = "Referensi";
            ws.Cells[1, 32].Value = "ID Driver";
            ws.Cells[1, 33].Value = "Hubungan";
            ws.Cells[1, 34].Value = "Keterangan";
            ws.Cells[1, 35].Value = "Id Database";
            ws.Cells[1, 36].Value = "Kode Driver";
            ws.Cells[1, 37].Value = "Kode Driver Lama";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].LookupCodeStatus.Nama;
                ws.Cells[i + 2, 2].Value = dbitems[i].Keterangan;
                ws.Cells[i + 2, 3].Value = dbitems[i].IsSms;
                ws.Cells[i + 2, 4].Value = Convert.ToDateTime(dbitems[i].TglGabung).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 5].Value = dbitems[i].NoKtp;
                ws.Cells[i + 2, 6].Value = dbitems[i].NamaDriver;
                ws.Cells[i + 2, 7].Value = dbitems[i].NamaPangilan;
                ws.Cells[i + 2, 8].Value = dbitems[i].TempatLahir;
                ws.Cells[i + 2, 9].Value = Convert.ToDateTime(dbitems[i].TglLahir).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 10].Value = dbitems[i].LookupCodeJenisSim.Nama;
                ws.Cells[i + 2, 11].Value = dbitems[i].NoSim;
                ws.Cells[i + 2, 12].Value = Convert.ToDateTime(dbitems[i].TglBerlakuSim).ToString("dd/MM/yyyy"); ;
                ws.Cells[i + 2, 13].Value = dbitems[i].NoTlp.Replace('_', ' ');
                ws.Cells[i + 2, 14].Value = dbitems[i].NoHp1.Replace('_', ' ');
                ws.Cells[i + 2, 15].Value = dbitems[i].NoHp2 == null ? "" : dbitems[i].NoHp2.Replace('_', ' ');
                ws.Cells[i + 2, 16].Value = dbitems[i].Alamat;
                ws.Cells[i + 2, 17].Value = dbitems[i].Rt == null ? "" : dbitems[i].Rt.Replace('_', ' ');
                ws.Cells[i + 2, 18].Value = dbitems[i].Rw == null ? "" : dbitems[i].Rw.Replace('_', ' ');
                ws.Cells[i + 2, 19].Value = dbitems[i].LocProvinsi == null ? "" : dbitems[i].LocProvinsi.Code;
                ws.Cells[i + 2, 20].Value = dbitems[i].LocKabKota == null ? "" : dbitems[i].LocKabKota.Code;
                ws.Cells[i + 2, 21].Value = dbitems[i].LocKecamatan == null ? "" : dbitems[i].LocKecamatan.Code;
                ws.Cells[i + 2, 22].Value = dbitems[i].LocKelurahan == null ? "" : dbitems[i].LocKelurahan.Code;
                ws.Cells[i + 2, 23].Value = dbitems[i].IsSameKtp;
                ws.Cells[i + 2, 24].Value = dbitems[i].AlamatDomisili;
                ws.Cells[i + 2, 25].Value = dbitems[i].RtDomisili == null ? " " : dbitems[i].RtDomisili.Replace('_', ' '); ;
                ws.Cells[i + 2, 26].Value = dbitems[i].RwDomisili == null ? " " : dbitems[i].RwDomisili.Replace('_', ' '); ;
                ws.Cells[i + 2, 27].Value = dbitems[i].LocProvinsiDomisili == null ? " " : dbitems[i].LocProvinsiDomisili.Code;
                ws.Cells[i + 2, 28].Value = dbitems[i].LocKabKotaDomisili == null ? " " : dbitems[i].LocKabKotaDomisili.Code;
                ws.Cells[i + 2, 29].Value = dbitems[i].LocKecamatanDomisili == null ? " " : dbitems[i].LocKecamatanDomisili.Code;
                ws.Cells[i + 2, 30].Value = dbitems[i].LocKelurahanDomisili == null ? " " : dbitems[i].LocKelurahanDomisili.Code;
                ws.Cells[i + 2, 31].Value = dbitems[i].LookupCodeReferensiDriver == null ? "" : dbitems[i].LookupCodeReferensiDriver.Nama;
                ws.Cells[i + 2, 32].Value = dbitems[i].DriverRef == null ? "" : dbitems[i].DriverRef.KodeDriver;
                ws.Cells[i + 2, 33].Value = dbitems[i].HubunganRef;
                ws.Cells[i + 2, 34].Value = dbitems[i].KeteranganRef;
                ws.Cells[i + 2, 35].Value = dbitems[i].Id;
                ws.Cells[i + 2, 36].Value = dbitems[i].KodeDriver;
                ws.Cells[i + 2, 37].Value = dbitems[i].KodeDriverOld;
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Driver.xls";

            return fsr;
        }

        #endregion
    }
}