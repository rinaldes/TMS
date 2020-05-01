using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IMultiDropRepo
    {
        void save(Multidrop dbitem);
        List<Multidrop> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Multidrop FindByPK(int id);
        Multidrop FindByTujuan(string tujuan);
        void delete(Multidrop dbitem);
    }
}