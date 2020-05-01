using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IAreaRepo
    {
        void save(MasterArea dbitem);
        List<MasterArea> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        MasterArea FindByPK(int id);
        MasterArea FindByName(string name);
        MasterArea FindByCode(string kode);
        void delete(MasterArea dbitem);
        string generateCode(int urutan);
        int getUrutan();
        bool IsExist(string name, int id =0);
    }
}