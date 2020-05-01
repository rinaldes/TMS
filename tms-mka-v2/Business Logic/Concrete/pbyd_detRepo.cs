using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Linq;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class pbyd_detRepo : Ipbyd_detRepo
    {
        private ContextModelERP context = new ContextModelERP();
        public int getUrutan(string code) {
            try {
                return int.Parse(context.pby_mstr.Where(d => d.pby_code.Contains(code)).Select(d => d.pby_code).Max().Split('-').Last());
            }
            catch (Exception)
            {
            }
            return 0;
        }
        public void saveMstr(string oid, string code, int cc_id, string pby_remarks, int driver_id)
        {
            Context.pby_mstr model = new Context.pby_mstr();
            model.pby_oid = oid;
            model.pby_dom_id = 1;
            model.pby_en_id = 1;
            model.pby_add_by = "";
            model.pby_add_date = DateTime.Now;
            model.pby_upd_by = "";
            model.pby_upd_date = DateTime.Now;
            model.pby_code = code;
            model.pby_date = DateTime.Now;
            model.pby_type = "D";
            model.pby_cc_id = 0;
            model.pby_ac_id = 0;
            model.pby_remarks = pby_remarks;
            model.pby_tran_id = 0;
            model.pby_trans_id = "D";
            model.pby_dt = DateTime.Now;
            model.pby_cu_id = 1;
            model.pby_exc_rate = 1;
            model.pby_peruntukan_id = 991494;
            model.pby_due_date = DateTime.Now;
            model.pby_xemp_id = 1;
            model.pby_branch_id = 10001;
            model.pby_driver = driver_id;
            context.pby_mstr.Add(model);
            context.SaveChanges();
        }

        public void save(string oid, string code, int cc_id, string pby_remarks, int driver_id, int ac_id, string ac_desc, decimal piutang_driver, string pbyd_tms_type=null)
        {
            Context.pbyd_det model = new Context.pbyd_det();

            model.pbyd_oid = Guid.NewGuid().ToString();
            model.pbyd_pby_oid = oid;
            model.pbyd_seq = 0;
            model.pbyd_ac_id = ac_id;
            model.pbyd_cc_id = 0;
            model.pbyd_desc = ac_desc;
            model.pbyd_amount = 0;
            model.pbyd_dt = DateTime.Now;
            model.pbyd_pjc_id = 0;
            model.pbyd_amount_pay = piutang_driver;
            model.pbyd_tms_type = pbyd_tms_type == null ? "B" : pbyd_tms_type == "P" ? null : pbyd_tms_type;
            model.pbyd_remarks = pby_remarks;
            context.pbyd_det.Add(model);
            context.SaveChanges();
        }

        public decimal TotalPiutangDriver(int id) {
            decimal pby = context.pbyd_det.Where(d => d.pby_mstr.pby_driver == id && d.pbyd_amount_pay != 0).FirstOrDefault() == null ? 0 : context.pbyd_det.Where(d => d.pby_mstr.pby_driver == id && d.pbyd_amount_pay != 0).Sum(e => e.pbyd_amount_pay);
            decimal? cash = context.cashd_det.Where(d => d != null && d.pbyd_det != null && d.pbyd_det.pby_mstr.pby_driver == id).Sum(e => e.cashd_amount+e.cashd_refund_amount);
            if (cash != null)
                pby += cash.Value;
            return pby;
        }

        public decimal TotalPiutangDriverPerType(int id, string pbyd_tms_type)
        {
            if (context.pbyd_det.Where(d => d.pby_mstr.pby_driver == id && d.pbyd_amount_pay != 0 && d.pbyd_tms_type == pbyd_tms_type).FirstOrDefault() == null)
                return 0;
            else
                return context.pbyd_det.Where(d => d.pby_mstr.pby_driver == id && d.pbyd_amount_pay != 0 && d.pbyd_tms_type == pbyd_tms_type).Sum(e => e.pbyd_amount_pay);
        }

        public List<pbyd_det> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<pbyd_det> list = context.pbyd_det;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<pbyd_det>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<pbyd_det>(s.SortOn + " " + s.SortOrder);
                }
            }

            //take & skip
            var takeList = list;
            if (skip != null && skip != 0)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null && skip != 0)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            //var sql = takeList.ToString();
            List<pbyd_det> result = takeList.ToList();
            return result;
        }
    }
}