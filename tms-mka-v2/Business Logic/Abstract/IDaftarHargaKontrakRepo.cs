using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDaftarHargaKontrakRepo
    {
        void save(DaftarHargaKontrak dbitem);
        List<DaftarHargaKontrak> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        DaftarHargaKontrak FindByPK(int id);
        void delete(DaftarHargaKontrak dbitem);
        bool IsPeriodeStartExist(DateTime startPeriode, int custId, int id = 0);
        bool IsPeriodValid(DateTime StrPeriode, DateTime EndPeriode, int custId, int id = 0);
        DaftarHargaKontrakItem FindItemByPK(int id);
    }
}