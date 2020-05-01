using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IJenisTruckRepo
    {
        void save(JenisTrucks dbitem, int id);
        List<JenisTrucks> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        JenisTrucks FindByPK(int id);
        JenisTrucks FindByName(string name);
        JenisTrucks FindByStrJenisTruck(string jenisTruck);
        void delete(JenisTrucks dbitem, int id);
        bool IsExist(string nama, int id = 0);
    }
}