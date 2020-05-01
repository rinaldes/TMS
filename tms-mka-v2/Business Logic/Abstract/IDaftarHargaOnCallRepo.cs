using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDaftarHargaOnCallRepo
    {
        void save(DaftarHargaOnCall dbitem, int id, string dhki = null);
        List<DaftarHargaOnCall> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        DaftarHargaOnCall FindByPK(int id);
        void delete(DaftarHargaOnCall dbitem);
        bool IsPeriodeStartExist(DateTime startPeriode, int custId ,int id = 0);
        bool IsPeriodValid(DateTime StrPeriode, DateTime EndPeriode, int custId, int id = 0);
        void FindRuteTruk(int id, out string IdRute, out int idJenisTruk);
        DaftarHargaOnCall FindByPeriodAndCust(DateTime startPeriode, DateTime endPeriode, int custId);
    }
}