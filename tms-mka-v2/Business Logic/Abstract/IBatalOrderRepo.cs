using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IBatalOrderRepo
    {
        void save(BatalOrder dbitem, int id);
        List<BatalOrder> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);        
        int Count(FilterInfo filters = null);
        BatalOrder FindByPK(int id);
        void delete(BatalOrder dbitem);
        BatalOrder FindBySO(int id);
        bool IsExist(int SalesOrderId, int VehicleId, int DriverId);
    }
}