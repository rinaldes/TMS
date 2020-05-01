using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Models;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface Iglt_detRepo
    {
		void save(int seq, Context.glt_det model, Context.SalesOrder soitem, string code, decimal? nominalCr, decimal? nominalDb, Models.ac_mstr ac_mstr);
        void saveFromBk(int seq, string code, decimal? nominalDb, decimal? nominalCr, Models.bk_mstr bk_mstr, Context.SalesOrder soitem);
        void saveFromAc(int seq, string code, decimal? nominalCr, decimal? nominalDb, Models.ac_mstr ac_mstr, Context.SalesOrder soitem, string desc = null, Context.SalesOrderKontrakListSo sok = null, System.DateTime? TanggalAktual = null, int? supplier_id = null, string glt_ref_detail_no = null, int? driver_id = null);
        void updateFromAc(Context.glt_det model, decimal? nominalDb, decimal? nominalCr);
        void delete(string glt_ref_detail_no);
        List<Context.glt_det> FindAllByglt_codeAndglt_ref_detail_no(string glt_code, string glt_ref_detail_no);
        List<Context.glt_det> FindAllByglt_desc(string glt_desc);
    }
}