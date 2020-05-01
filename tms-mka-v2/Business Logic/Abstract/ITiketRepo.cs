using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ITiketRepo
    {
        void save(Tiket dbitem, int id);
        List<Tiket> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Tiket FindByPK(int id);
        string generateCodePPK(int urutan);
        int getUrutan();
        void delete(Tiket dbitem, int id);
    }
}