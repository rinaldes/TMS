using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IVendorGpsRepo
    {
        void save(VendorGps dbitem, int id);
        List<VendorGps> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        VendorGps FindByPK(int id);
        VendorGps FindByName(string name);
        void delete(VendorGps dbitem, int id);
    }
}