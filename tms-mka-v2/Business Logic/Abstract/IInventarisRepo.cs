using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IInventarisRepo
    {
        void save(Inventaris dbitem, int id);
        List<Inventaris> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        Inventaris FindByPK(int id);
        void delete(Inventaris dbitem, int id);
    }
}
