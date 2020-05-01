using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface Ipbyd_detRepo
    {
        List<pbyd_det> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        void saveMstr(string oid, string code, int cc_id, string pby_remarks, int driver_id);
        void save(string oid, string code, int cc_id, string pby_remarks, int driver_id, int ac_id, string ac_desc, decimal piutang_driver, string pbyd_tms_type=null);
        decimal TotalPiutangDriver(int id);
        decimal TotalPiutangDriverPerType(int id, string pbyd_tms_type);
        int getUrutan(string code);
    }
}