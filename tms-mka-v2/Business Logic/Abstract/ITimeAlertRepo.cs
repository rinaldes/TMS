using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ITimeAlertRepo
    {
        void save(TimeAlert dbitem, int id);
        List<TimeAlert> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        TimeAlert FindByPK(int id);
        void delete(TimeAlert dbitem, int id);
    }
}
