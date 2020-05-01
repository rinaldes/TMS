using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDaftarHargaKonsolidasiRepo
    {
        void save(DaftarHargaKonsolidasi dbitem, int id, string dhki=null);
        List<DaftarHargaKonsolidasi> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        DaftarHargaKonsolidasi FindByPK(int id);
        DaftarHargaKonsolidasi FindByItemId(int id);
        void delete(DaftarHargaKonsolidasi dbitem, int id);
        bool IsPeriodeStartExist(DateTime startPeriode, int custId, int id = 0);
        bool IsPeriodValid(DateTime StrPeriode, DateTime EndPeriode, int custId, int id = 0);
        void FindRuteTruk(int id, out string IdRute, out int idJenisTruk);
        Context.DaftarHargaKonsolidasiItem FindItemByPK(int id);
    }
}