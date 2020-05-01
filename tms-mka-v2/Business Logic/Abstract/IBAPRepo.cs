using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IBAPRepo
    {
        void save(BAP dbitem, int id);
        List<BAP> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        BAP FindByPK(int id);
        BAP FindByNoBAP(string noBAP);
        void delete(BAP dbitem, int id);
        bool IsExist(int? IdSo, int? SalesOrderKontrakId, int? idDriver, int? idTruck, int? IdKategori, int id = 0);
        string GenerateCode(DateTime valdate, int urutan);
        int getUrutanOnCAll(DateTime valdate);
        List<BAP> FindAllAll();
    }
}