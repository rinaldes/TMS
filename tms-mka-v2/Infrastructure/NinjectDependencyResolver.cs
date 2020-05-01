using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Business_Logic.Concrete;

namespace tms_mka_v2.Infrastructure
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<Igr_mstrRepo>().To<gr_mstrRepo>();
            kernel.Bind<Iac_mstrRepo>().To<ac_mstrRepo>();
            kernel.Bind<Icode_mstrRepo>().To<code_mstrRepo>();
            kernel.Bind<Iptnr_mstrRepo>().To<ptnr_mstrRepo>();
            kernel.Bind<Ipbyd_detRepo>().To<pbyd_detRepo>();
            kernel.Bind<Icashd_detRepo>().To<cashd_detRepo>();
            kernel.Bind<Iglt_detRepo>().To<glt_detRepo>();
            kernel.Bind<Iso_mstrRepo>().To<so_mstrRepo>();
            kernel.Bind<Iptnra_addrRepo>().To<ptnra_addrRepo>();
            kernel.Bind<ILookupCodeRepo>().To<LookupCodeRepo>();
            kernel.Bind<IUserReferenceRepo>().To<UserReferenceRepo>();
            kernel.Bind<IUserRepo>().To<UserRepo>();
            kernel.Bind<IRoleRepo>().To<RoleRepo>();
            kernel.Bind<ILookupCodeCategoriesRepo>().To<LookupCodeCategoriesRepo>();
            kernel.Bind<IProductRepo>().To<ProductRepo>();
            kernel.Bind<ICustomerRepo>().To<CustomerRepo>();
            kernel.Bind<ILocationRepo>().To<LocationRepo>();
            kernel.Bind<IRekeningRepo>().To<RekeningRepo>();
            kernel.Bind<IJenisTruckRepo>().To<JenisTruckRepo>();
            kernel.Bind<IMultiDropRepo>().To<MultiDropRepo>();
            kernel.Bind<IInventarisRepo>().To<InventarisRepo>();
            kernel.Bind<IAreaRepo>().To<AreaRepo>();
            kernel.Bind<IRuteRepo>().To<RuteRepo>();
            kernel.Bind<IMasterPoolRepo>().To<MasterPoolRepo>();
            kernel.Bind<IAtmRepo>().To<AtmRepo>();
            kernel.Bind<IJnsTolRepo>().To<JnsTolRepo>();
            kernel.Bind<IMekanikRepo>().To<MekanikRepo>();
            kernel.Bind<IVendorGpsRepo>().To<VendorGpsRepo>();
            kernel.Bind<IDataTruckRepo>().To<DatatruckRepo>();
            kernel.Bind<IRuteTolRepo>().To<RuteTolRepo>();
            kernel.Bind<IDriverRepo>().To<DriverRepo>();
            kernel.Bind<IDriverTruckHistoryRepo>().To<DriverTruckHistoryRepo>();
            kernel.Bind<IMasterSolarRepo>().To<MasterSolarRepo>();
            kernel.Bind<IDataBoxRepo>().To<DataBoxRepo>();
            kernel.Bind<IDataPendinginRepo>().To<DataPendinginRepo>();
            kernel.Bind<IDataGPSRepo>().To<DataGPSRepo>();
            kernel.Bind<IFaktorBoronganRepo>().To<FaktorBoronganRepo>();
            kernel.Bind<IDataBoronganRepo>().To<DataBoronganRepo>();
            kernel.Bind<IPenetapanaDriverRepo>().To<PenetapanDriverRepo>();
            kernel.Bind<IDaftarHargaOnCallRepo>().To<DaftarHargaOnCallRepo>();
            kernel.Bind<IDaftarHargaKontrakRepo>().To<DaftarHargaKontrakRepo>();
            kernel.Bind<IDaftarHargaKonsolidasiRepo>().To<DaftarHargaKonsolidasiRepo>();
            kernel.Bind<ITrainingSettingRepo>().To<TrainingSettingRepo>();
            kernel.Bind<ITiketRepo>().To<TiketRepo>();
            kernel.Bind<ITimeAlertRepo>().To<TimeAlertRepo>();
            kernel.Bind<ISolarInapRepo>().To<SolarInapRepo>();
            kernel.Bind<IAuditrailRepo>().To<AuditrailRepo>();
            kernel.Bind<IPelaksanaanTrainingRepo>().To<PelaksanaanTrainingRepo>();
            kernel.Bind<ITrainingSettingDetailRepo>().To<TrainingSettingDetailRepo>();
            kernel.Bind<ISalesOrderRepo>().To<SalesOrderRepo>();
            kernel.Bind<IRevisiTanggalRepo>().To<RevisiTanggalRepo>();
            kernel.Bind<IAdminUangJalanRepo>().To<AdminUangJalanRepo>();
            kernel.Bind<IRevisiJenisTrukRepo>().To<RevisiJenisTrukRepo>();
            kernel.Bind<IERPConfigRepo>().To<ERPConfigRepo>();
            kernel.Bind<ISalesOrderKontrakListSoRepo>().To<SalesOrderKontrakListSoRepo>();
            kernel.Bind<IBatalOrderRepo>().To<BatalOrderRepo>();
            kernel.Bind<IDokumenRepo>().To<DokumenRepo>();
            kernel.Bind<IWorkshopRepo>().To<WorkshopRepo>();
            kernel.Bind<IWorkshopMekanikRepo>().To<WorkshopMekanikRepo>();
            kernel.Bind<IAuthorizationRuleRepo>().To<AuthorizationRuleRepo>();
            kernel.Bind<ISpkRepo>().To<SpkRepo>();
            kernel.Bind<IHistoryOncallRepo>().To<HistoryOncallRepo>();
            kernel.Bind<ISettlementRegRepo>().To<SettlementRegRepo>();
            kernel.Bind<IRevisiRuteRepo>().To<RevisiRuteRepo>();
            kernel.Bind<IGeneralSettingRepo>().To<GeneralSettingRepo>();
            kernel.Bind<ISettlementBatalRepo>().To<SettlementBatalRepo>();
            kernel.Bind<IBAPRepo>().To<BAPRepo>();
            kernel.Bind<IKlaimRepo>().To<KlaimRepo>();
            kernel.Bind<IHistoryJalanTruckRepo>().To<HistoryJalanTruckRepo>();
            kernel.Bind<IRemovalRepo>().To<RemovalRepo>();
            kernel.Bind<IMenuRepo>().To<MenuRepo>();
            kernel.Bind<Ibk_mstrRepo>().To<bk_mstrRepo>();
            kernel.Bind<IKlaimDriverRepo>().To<KlaimDriverRepo>();
            kernel.Bind<IMonitoringVehicleRepo>().To<MonitoringVehicleRepo>();
            kernel.Bind<IlistNotifRepo>().To<ListNotifRepo>();
        }
    }
}