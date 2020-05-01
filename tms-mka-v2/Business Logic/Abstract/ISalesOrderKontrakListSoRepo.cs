using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ISalesOrderKontrakListSoRepo
    {
        void save(SalesOrderKontrakListSo dbitem);
        List<Context.SalesOrderKontrakListSo> FindAllByAUJ(int aujId);
        List<SalesOrderKontrakListSo> FindAllDispatched();
        List<SalesOrderKontrakListSo> FindAllKasDispatched();
        SalesOrderKontrakListSo FindByPK(int id);
        void OnlyAdd(SalesOrderKontrakListSo dbitem);
        void saveParent(SalesOrderKontrakTruck dbitem);
        void deleteAdd(int? id);
        void returnListSo(int? id);
        string generateCodeListSo(string NoKontrak, DateTime valdate, int rit, int urutan, int urutanInduk);
        List<Context.SalesOrderKontrakListSo> FindAllByUrutan(int Urutan);
        List<SalesOrderKontrakListSo> FindAllKontrak();
        List<SalesOrderKontrakListSo> FindAllPerKontrak(List<int> ListIdDumy);
        int getUrutanProses(int? id);
        List<SalesOrderKontrakListSo> returnListSoBatalTruckOnly();
        SalesOrderKontrakListSo FindByNoSo(string noso);
        void OnlyUpdate(SalesOrderKontrakListSo dbitem);
        int CountKontrakListSo(FilterInfo filters = null);
        List<SalesOrderKontrakListSo> FindAll();
        void delete(SalesOrderKontrakListSo dbitem, int id);
        List<SalesOrderKontrakListSo> AUJReport(int aujId);
        Context.SalesOrderKontrakListSo FindSONextMonth(Context.SalesOrderKontrakListSo sokontrak);
    }
}
