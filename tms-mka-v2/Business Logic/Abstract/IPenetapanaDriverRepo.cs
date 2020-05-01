using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IPenetapanaDriverRepo
    {
        void save(PenetapanDriver dbitem);
        List<PenetapanDriver> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        PenetapanDriver FindByPK(int id);
        void delete(PenetapanDriver dbitem);
        bool IsExist(int IdTruck);
        bool isExistDriver(int IdDriver1, int id);
        Context.DriverTruckHistory FindLastDriverHistory(string type);
    }
}