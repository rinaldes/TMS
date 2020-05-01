using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ISolarInapRepo
    {
        List<SolarInap> FindAllAUJ(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<SolarInap> FindAllPisan();
        void save(SolarInap dbitem, int id, int? StatusFlow = null);
        List<SolarInap> FindAll(string Step, int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        SolarInap FindByPK(int id);
        List<SolarInapHistory> FindAllHistory(int idSo);
        void delete(SolarInap dbitem, int id);
        SolarInap FindBySOAndDate(int? so_id, System.DateTime date);
        List<SolarInap> FindAllReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int CountTrans(string Step, FilterInfo filters = null);
        int CountMarketing(FilterInfo filters = null);
        List<SolarInap> FindAllMarketing(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
    }
}