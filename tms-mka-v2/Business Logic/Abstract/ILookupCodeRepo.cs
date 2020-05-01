using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ILookupCodeRepo
    {
        void save(LookupCode dbitem);
        List<LookupCode> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        LookupCode FindByPK(int? id);
        LookupCode FindByName(string name);
        LookupCode FindByNameAndCat(string name);
        void delete(LookupCode dbitem);
        List<Context.LookupCode> FindAllSPBU();
    }
}