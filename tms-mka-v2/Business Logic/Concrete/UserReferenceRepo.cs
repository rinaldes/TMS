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
    public class UserReferenceRepo : IUserReferenceRepo
    {
        private ContextModel context = new ContextModel();
        public void save(UserReference dbitem, int id){
            if (dbitem.Id == 0) //create
            {
                context.UserReference.Add(dbitem);
                var query = "INSERT INTO dbo.\"UserReference\" (\"IdUser\", \"Action\", \"Controller\", \"Coloumn\", \"HideShow\") VALUES (" + dbitem.IdUser + ", " + dbitem.Action + ", " +
                    dbitem.Controller + ", " + dbitem.Coloumn + ", " + dbitem.HideShow + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "User Reference", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.UserReference.Attach(dbitem);
                var query = "UPDATE dbo.\"UserReference\" SET \"IdUser\" = " + dbitem.IdUser + ", \"Action\" = " + dbitem.Action + ", \"Controller\" = " + dbitem.Controller + ", \"Coloumn\" = " +
                    dbitem.Coloumn + ", \"HideShow\" = " + dbitem.HideShow + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "User Reference", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
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

        public UserReference find(int iduser, string action, string controller, string kolom)
        {
            return context.UserReference.Where(d =>d.IdUser == iduser && d.Action == action && d.Controller == controller && d.Coloumn == kolom).FirstOrDefault();
        }
        public List<UserReference> FindByUser(int iduser)
        {
            return context.UserReference.Where(d => d.IdUser == iduser && d.HideShow == "hide").ToList();
        }
        public List<UserReference> FindAll()
        {
            return context.UserReference.ToList();
        }
    }
}