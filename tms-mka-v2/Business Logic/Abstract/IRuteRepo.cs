using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IRuteRepo
    {
        void save(Rute dbitem);
        List<Rute> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Rute FindByPK(int id);
        Rute FindByKode(string kode);
        void delete(Rute dbitem);
        string generateCode(int urutan);
        int getUrutan();
    }
}