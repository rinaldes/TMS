using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IMasterPoolRepo
    {
        void save(MasterPool dbitem, int id);
        List<MasterPool> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        MasterPool FindByPK(int id);
        MasterPool FindByNamePool(string namePool);
        void delete(MasterPool dbitem);
        bool IsExist(string nama, int id = 0);
        Context.MasterPool FindByIPAddress();
    }
}