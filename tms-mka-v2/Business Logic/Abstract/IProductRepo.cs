using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IProductRepo
    {
        void save(MasterProduct dbitem);
        List<MasterProduct> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        MasterProduct FindByPK(int id);
        MasterProduct FindByName(string name);
        void delete(MasterProduct dbitem);
        bool IsExist(string nama, int id = 0);
    }
}