using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IRekeningRepo
    {
        void save(Rekenings dbitem);
        List<Rekenings> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Rekenings FindByPK(int id);
        Rekenings FindByName(string name);
        void delete(Rekenings dbitem);
        bool IsExist(int idBank, string code, int id = 0);
    }
}