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
using tms_mka_v2.Models;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class gr_mstrRepo : Igr_mstrRepo
    {
        private ContextModelERP context = new ContextModelERP();

        public void save(int? total, Context.Customer customer, string username, string code)
        {
            Context.gr_mstr gr_mst = context.gr_mstr.Where(d => d.gr_code == code).FirstOrDefault();
         //   if (gr_mst == null)
           // {
            Context.gr_mstr model = new Context.gr_mstr();
                model.gr_oid = Guid.NewGuid().ToString();
                model.gr_dom_id = 1;
                model.gr_en_id = 1;
                model.gr_add_by = username;
                model.gr_add_date = DateTime.Now;
                model.gr_ptnr_id = customer.Id;
                model.gr_code = code;
                model.gr_date = DateTime.Now;
                model.gr_tax_basis_amount = total;
                model.gr_tax_amount = total;
                model.gr_total = total;
                model.gr_ptnr_code = customer.CustomerCodeOld;
                model.gr_branch_id = 10001;
                context.gr_mstr.Add(model);
                context.SaveChanges();
          //  }
        }

        public void saveKapal(int? total, Context.Customer customer, string username, string code)
        {
            gr_mstr model = new gr_mstr();
            model.gr_oid = Guid.NewGuid().ToString();
            model.gr_dom_id = 1;
            model.gr_en_id = 1;
            model.gr_add_by = username;
            model.gr_add_date = DateTime.Now;
            model.gr_ptnr_id = customer.Id;
            model.gr_code = code;
            model.gr_date = DateTime.Now;
            model.gr_tax_basis_amount = total;
            model.gr_tax_amount = total;
            model.gr_total = total;
            model.gr_ptnr_code = customer.CustomerCodeOld;
            model.gr_branch_id = 10001;
            context.gr_mstr.Add(model);
            context.SaveChanges();
        }

        public Context.gr_mstr FindByCode(string code)
        {
            return context.gr_mstr.Where(d => d.gr_code == code).FirstOrDefault();
        }

        public List<Context.gr_mstr> FindAllByCode(string code)
        {
            return context.gr_mstr.Where(d => d.gr_code == code).ToList();
        }

        public void delete(gr_mstr dbitem)
        {
            context.gr_mstr.Remove(dbitem);
         //   var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Area", QueryDetail = "Delete " + dbitem.Nama, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
           // context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}