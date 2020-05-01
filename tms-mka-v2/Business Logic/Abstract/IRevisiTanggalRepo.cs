using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IRevisiTanggalRepo
    {
        void save(RevisiTanggal dbitem);
        List<RevisiTanggal> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        RevisiTanggal FindByPK(int id);
        RevisiTanggal FindBySo(int id);
        void delete(RevisiTanggal dbitem);
        RevisiTanggal FindBySoAndBP(int id);
 
    }
}