using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ISpkRepo
    {
        void save(Spk spk_dbitem, int id);
        List<Spk> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<Spk> FindAllOnProgress(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<Spk> FindByWorkshop(int id);
        Spk FindByWorkshopAndType(int id, string type);
        List<SpkHistory> FindByWorkshopForHistory(int id);
        int Count(FilterInfo filters = null);
        int CountOnProgress(FilterInfo filters = null);
        Spk FindByPK(int id);
        void delete(Spk dbitem);
        string generateCode(DateTime tglGabung, int urutan);
        int? getUrutan(DateTime tglGbg);
    }
}