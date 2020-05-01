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
    public class KlaimDriverRepo : IKlaimDriverRepo
    {
        private ContextModel context = new ContextModel();
        public void save(BebanKlaimDriver dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                var query = "INSERT INTO dbo.\"BebanKlaimDriver\" (\"IdDriver\", \"IdKlaim\") VALUES (" + dbitem.IdDriver + ", " + dbitem.IdKlaim + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Klaim", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
                context.BebanKlaimDriver.Add(dbitem);
            }
            else //edit
            {
                context.BebanKlaimDriver.Attach(dbitem);
                var query = "INSERT INTO dbo.\"BebanKlaimDriver\" (\"IdDriver\", \"IdKlaim\") VALUES (" + dbitem.IdDriver + ", " + dbitem.IdKlaim + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Klaim", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public Context.BebanKlaimDriver FindByKlaimId(int id){
            return context.BebanKlaimDriver.Where(d => d.IdKlaim == id).FirstOrDefault();
        }
    }
}