using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IAtmRepo
    {
        void save(Atm dbitem, int usr_id);
        List<Atm> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Atm FindByPK(int id);
        Atm FindByDriver(int id);
        void delete(Atm dbitem, int usr_id);
    }
}