using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IMonitoringVehicleRepo
    {
        List<MonitoringVehicle> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Context.MonitoringDetailSo FindMonitoringDetailSoByPK(int id);
        //Context.HistoryGps FindNextHistoryGpsByPK(int id, string VehicleNo);
        Context.MonitoringDetailSo FindMonitoringDetailSo(string NoSo);
        List<Context.MonitoringDetailSo> FindAllMonitoringSo(int driverId);
        Context.MonitoringVehicle FindByVehicleNo(string vehicleNo);
        string Area(string area);
        void FindAllTruck(List<Models.MonitoringAll> ListModel);
        List<Context.MonitoringDetailSo> FindAllMonitoringDetailSoByVehicleNo(string VehicleNo);
        void FindOnDuty(List<Models.MonitoringOnduty> ListModel);
        void FindOnTime(List<Models.MonitoringOntime> ListModel);
        void FindOnTemp(List<Models.MonitoringOntemp> ListModel);
        void FindService(List<Models.MonitoringService> ListModel);
        Context.MonitoringDetailSo FindMonitoringDetailSoByVehicleNo(string VehicleNo);
        void saveReportGpsError(GpsErrorReport dbitem);
        void saveAnalisaOnTemp(AnalisaOnTemp dbitem);
        void updateLocation(MonitoringVehicle dbitem);
        //List<Context.HistoryGps> FindAllHistoryGps(string VehicleNo, string StartTime, string EndTime, int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<Context.AnalisaOnTemp> FindAllAnalisaOnTemp(string VehicleNo, string StartTime, string EndTime, int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int CountAllAnalisaOnTemp(string VehicleNo, string StartTime, string EndTime, int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
    }
}