using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IAdminUangJalanRepo
    {
        void save(AdminUangJalan dbitem);
        List<AdminUangJalan> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        AdminUangJalan FindByPK(int id);
        void delete(AdminUangJalan dbitem);
        int? getUrutanUang(DateTime valdate);
        string satuanTerbilang(int number);
        string terbilang(decimal number);
        AdminUangJalan FindBySalesOrderIdForKasirReport(int id);
        AdminUangJalan FindBySalesOrderId(int id);
    }
}