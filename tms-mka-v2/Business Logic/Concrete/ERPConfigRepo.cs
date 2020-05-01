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
    public class ERPConfigRepo : IERPConfigRepo
    {
        private ContextModel context = new ContextModel();
        public void save(ERPConfig dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.ERPConfig.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Area", QueryDetail = "Edit Account", RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.ERPConfig.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Area", QueryDetail = "Edit Account", RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void saveDynamic(int id, int ac_id, string type)
        {
            if (type == "SPBU"){
                Context.ERPDynamicConfig model = new Context.ERPDynamicConfig();
                model.ac_id = ac_id;
                model.lookup_code_id = id;
                context.ERPDynamicConfig.Add(model);
                context.SaveChanges();

                Context.LookupCode lc = context.LookupCode.Where(d => d.Id == id).FirstOrDefault();
                lc.ac_id = ac_id;
                context.LookupCode.Attach(lc);
                var entry = context.Entry(lc);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
            else if (type == "Pool"){
                Context.ERPDynamicConfig model = new Context.ERPDynamicConfig();
                model.ac_id = ac_id;
                model.pool_id = id;
                context.ERPDynamicConfig.Add(model);
                context.SaveChanges();

                Context.MasterPool lc = context.MasterPool.Where(d => d.Id == id).FirstOrDefault();
                lc.IdCreditCash = ac_id;
                context.MasterPool.Attach(lc);
                var entry = context.Entry(lc);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public ERPConfig FindByFrist()
        {
            return context.ERPConfig.FirstOrDefault();
        }

        public ERPDynamicConfig FindByIdAndType(int id, string type)
        {
            if (type == "SPBU" || type == "Kapal" || type == "Biaya Tambahan")
                return context.ERPDynamicConfig.Where(d => d.lookup_code_id == id).FirstOrDefault();
            return null;
        }
    }
}