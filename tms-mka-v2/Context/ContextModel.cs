using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace tms_mka_v2.Context
{
    public class ContextModel : DbContext
    {
        public ContextModel()
            : base("tmsDefault")
        {

        }

        public DbSet<LookupCode> LookupCode { get; set; }
        public DbSet<AnalisaOnTemp> AnalisaOnTemp { get; set; }
        public DbSet<GpsErrorReport> GpsErrorReport { get; set; }
        public DbSet<LookupCodeCategories> LookupCodeCategories { get; set; }
        public DbSet<MasterProduct> MasterProduct { get; set; }
        public DbSet<AuthorizationRule> AuthorizationRule { get; set; }
        public DbSet<JenisTrucks> JenisTrucks { get; set; }
        public DbSet<JnsTols> JnsTols { get; set; }
        public DbSet<ERPConfig> ERPConfig { get; set; }
        public DbSet<ERPDynamicConfig> ERPDynamicConfig { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserReference> UserReference { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerBilling> CustomerBilling { get; set; }
        public DbSet<CustomerPPN> CustomerPPN { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<ListLocationArea> ListLocationArea { get; set; }
        public DbSet<Rekenings> Rekenings { get; set; }
        public DbSet<Multidrop> Multidrop { get; set; }
        public DbSet<OrderHistory> OrderHistory { get; set; }
        public DbSet<SolarInapHistory> SolarInapHistory { get; set; }
        public DbSet<MasterArea> MasterArea { get; set; }
        public DbSet<Rute> Rute { get; set; }
        public DbSet<MasterPool> MasterPool { get; set; }
        public DbSet<Atm> Atm { get; set; }
        public DbSet<Mekanik> Mekanik { get; set; }
        public DbSet<VendorGps> VendorGps { get; set; }
        public DbSet<DataTruck> DataTruck { get; set; }
        public DbSet<DataBox> DataBox { get; set; }
        public DbSet<RuteTol> RuteTol { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<DriverTruckHistory> DriverTruckHistory { get; set; }
        public DbSet<Inventaris> Inventaris { get; set; }
        public DbSet<MasterSolar> MasterSolar { get; set; }
        public DbSet<DataGPS> DataGPS { get; set; }
        public DbSet<DataPendingin> DataPendingin { get; set; }
        public DbSet<FaktorBorongan> FaktorBorongan { get; set; }
        public DbSet<DataBorongan> DataBorongan { get; set; }
        public DbSet<PenetapanDriver> PenetapanDriver { get; set; }
        public DbSet<DaftarHargaOnCall> DaftarHargaOnCall { get; set; }
        public DbSet<DaftarHargaOnCallItem> DaftarHargaOnCallItem { get; set; }
        public DbSet<DaftarHargaKontrak> DaftarHargaKontrak { get; set; }
        public DbSet<DaftarHargaKonsolidasi> DaftarHargaKonsolidasi { get; set; }
        public DbSet<DaftarHargaKonsolidasiItem> DaftarHargaKonsolidasiItem { get; set; }
        public DbSet<DaftarHargaKontrakItem> DaftarHargaKontrakItem { get; set; }
        public DbSet<TrainingSetting> TrainingSetting { get; set; }
        public DbSet<PelaksanaanTraining> PelaksanaanTraining { get; set; }
        public DbSet<PelaksanaanTrainingDetail> PelaksanaanTrainingDetail { get; set; }
        public DbSet<TrainingSettingDetail> TrainingSettingDetail { get; set; }
        public DbSet<Auditrail> Auditrail { get; set; }
        public DbSet<SolarInap> SolarInap { get; set; }
        public DbSet<SalesOrder> SalesOrder { get; set; }
        public DbSet<SalesOrderOncall> SalesOrderOncall { get; set; }
        public DbSet<SalesOrderKontrak> SalesOrderKontrak { get; set; }
        public DbSet<SalesOrderPickup> SalesOrderPickup { get; set; }
        public DbSet<SalesOrderKonsolidasi> SalesOrderKonsolidasi { get; set; }
        public DbSet<SalesOrderProsesKonsolidasi> SalesOrderProsesKonsolidasi { get; set; }
        public DbSet<SalesOrderProsesKonsolidasiItem> SalesOrderProsesKonsolidasiItem { get; set; }
        public DbSet<RevisiTanggal> RevisiTanggal { get; set; }
        public DbSet<RevisiTanggalSOKontrak> RevisiTanggalSOKontrak { get; set; }
        public DbSet<RevisiJenisTruk> RevisiJenisTruk { get; set; }
        public DbSet<AdminUangJalan> AdminUangJalan { get; set; }
        public DbSet<AdminUangJalanUangTf> AdminUangJalanUangTf { get; set; }
        public DbSet<HistoryPergantianOncall> HistoryPergantianOncall { get; set; }
        public DbSet<SalesOrderKontrakTruck> SalesOrderKontrakTruck { get; set; }
        public DbSet<SalesOrderKontrakListSo> SalesOrderKontrakListSo { get; set; }
        public DbSet<HistoryGps> HistoryGps { get; set; }
        public DbSet<BAP> BAP { get; set; }
        public DbSet<Klaim> Klaim { get; set; }
        public DbSet<KlaimProduct> KlaimProduct { get; set; }
        public DbSet<Dokumen> Dokumen { get; set; }
        public DbSet<TimeAlert> TimeAlert { get; set; }
        public DbSet<Workshop> Workshop { get; set; }
        public DbSet<WorkshopMekanik> WorkshopMekanik { get; set; }
        public DbSet<Spk> Spk { get; set; }
        public DbSet<SpkHistory> SpkHistory { get; set; }
        public DbSet<Tiket> Tiket { get; set; }
        public DbSet<TiketResponse> TiketResponse { get; set; }
        public DbSet<BatalOrder> BatalOrder { get; set; }
        public DbSet<BatalTruk> BatalTruk { get; set; }
        public DbSet<SettlementReguler> SettlementReguler { get; set; }
        public DbSet<RevisiRute> RevisiRute { get; set; }
        public DbSet<RevisiRuteLoadUnLoadAddress> RevisiRuteLoadUnLoadAddress { get; set; }
        public DbSet<SettingGeneral> SettingGeneral { get; set; }
        public DbSet<SettlementBatal> SettlementBatal { get; set; }
        public DbSet<NewCoordinateGps> NewCoordinateGps { get; set; }
        public DbSet<OldCoordinateGps> OldCoordinateGps { get; set; }
        public DbSet<HistoryJalanTruck> HistoryJalanTruck { get; set; }
        public DbSet<MonitoringVehicle> MonitoringVehicle { get; set; }
        public DbSet<MonitoringDetailSo> MonitoringDetailSo { get; set; }
        public DbSet<MonitoringDetailSpeedAlert> MonitoringDetailSpeedAlert { get; set; }
        public DbSet<MonitoringDetailParkingAlert> MonitoringDetailParkingAlert { get; set; }
        public DbSet<MonitoringDetailOnTemp> MonitoringDetailOnTemp { get; set; }
        public DbSet<MonitoringDetailTemperatureAlert> MonitoringDetailTemperatureAlert { get; set; }
        public DbSet<Removal> Removal { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<RoleMenus> RoleMenus { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<BebanKlaimDriver> BebanKlaimDriver { get; set; }
        public DbSet<UserMenus> UserMenus { get; set; }
        public DbSet<ListNotif> ListNotif { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}