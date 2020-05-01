using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class LookupCodeCategoriesRepo : ILookupCodeCategoriesRepo
    {
        private ContextModel context = new ContextModel();
        public void save(LookupCodeCategories dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.LookupCodeCategories.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Lookup Code Categories", QueryDetail = "Add " + dbitem.Description, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.LookupCodeCategories.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Lookup Code Categories", QueryDetail = "Edit " + dbitem.Description, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public List<LookupCodeCategories> FindAll()
        {
            return context.LookupCodeCategories.ToList();
        }
        public LookupCodeCategories FindByPK(int id)
        {
            return context.LookupCodeCategories.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(LookupCodeCategories dbitem)
        {
            context.LookupCodeCategories.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Lookup Code Categories", QueryDetail = "Delete " + dbitem.Description, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}