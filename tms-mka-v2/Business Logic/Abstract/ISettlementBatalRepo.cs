using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ISettlementBatalRepo
    {
        void save(SettlementBatal dbitem, int id, string modul);
        List<SettlementBatal> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        SettlementBatal FindByPK(int id);
        void delete(SettlementBatal dbitem);
        SettlementBatal FindBySo(int IdSalesOrder);
        SettlementBatal FindByAUJ(int IdAUJ);
        List<SettlementBatal> FindAllReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<SettlementBatal> FindAllReportKontrak(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
    }
}