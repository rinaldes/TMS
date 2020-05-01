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
    public class RoleRepo : IRoleRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Role dbitem){
            if (dbitem.Id == 0) //create
            {
                context.Role.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Role", QueryDetail = "Add " + dbitem.RoleName, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.Role.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Role", QueryDetail = "Edit " + dbitem.RoleName, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
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

        public List<Role> FindAll()
        {
            return context.Role.ToList();
        }
        public Role FindByPK(int id)
        {
            return context.Role.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(Role dbitem)
        {
            context.Role.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Role", QueryDetail = "Delete " + dbitem.RoleName, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(string nama, int id = 0)
        {
            if (id == 0)
            { return context.Role.Any(p => p.RoleName.Contains(nama)); }
            else
            { return context.Role.Any(p => p.RoleName.Contains(nama) && p.Id != id); }
        }
    }
}