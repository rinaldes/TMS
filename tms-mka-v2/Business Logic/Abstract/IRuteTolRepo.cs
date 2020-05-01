using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IRuteTolRepo
    {
        void save(RuteTol dbitem, int id);
        List<RuteTol> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        RuteTol FindByPK(int id);
        RuteTol FindByNamaRuteTol(string namaRuteTol);
        RuteTol FindByUniqeTol(string kodeRute, string namaRuteTol);
        void delete(RuteTol dbitem);
    }
}