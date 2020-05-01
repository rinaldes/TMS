using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDokumenRepo
    {
        Dokumen FindBySONumber(string SONumber);
        void save(Dokumen dbitem, int id, string strQuery=null);
        List<Dokumen> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        int CountKontrak(FilterInfo filters = null);
        int CountKonsolidasiSJ(FilterInfo filters = null);
        Dokumen FindByPK(int id);
        Dokumen FindBySO(int idSO);
        List<Dokumen> FindAllOnCallSJ(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int CountOnCallSJReport(FilterInfo filters = null);
        List<Dokumen> FindAllOnCallSJReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<SalesOrderKontrakListSo> FindAllKontrakSJ(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int CountBilling(FilterInfo filters = null);
        List<Dokumen> FindAllBilling(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<Dokumen> FindAllKonsolidasiSJ(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        Context.Dokumen FindBySalesOrderProsesKonsolidasiIdAndCust(int SalesOrderProsesKonsoldiasiId, int CustomerId);
        int CountOncallSJ(FilterInfo filters = null);
    }
}