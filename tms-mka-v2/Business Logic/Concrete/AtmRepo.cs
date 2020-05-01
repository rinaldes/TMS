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
    public class AtmRepo : IAtmRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Atm dbitem, int usr_id)
        {
            if (dbitem.Id == 0) //create
            {
                context.Atm.Add(dbitem);
                var query = "INSERT INTO dbo.\"Atm\" (\"NoKartu\", \"IdBank\", \"NoRekening\", \"AtasNama\", \"IdDriver\") VALUES (" + dbitem.NoKartu + ", " + dbitem.IdBank + ", " + dbitem.NoRekening +
                    ", " + dbitem.AtasNama + ", " + dbitem.IdDriver + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Atm", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = usr_id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.Atm.Attach(dbitem);
                var query = "UPDATE dbo.\"Atm\" SET \"NoKartu\" = " + dbitem.NoKartu + ", \"IdBank\" = " + dbitem.IdBank + ", \"NoRekening\" = " + dbitem.NoRekening + ", \"AtasNama\" = " +
                    dbitem.AtasNama + ", \"IdDriver\" = " + dbitem.IdDriver + ", WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Atm", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = usr_id
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

        public List<Atm> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Atm> list = context.Atm;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Atm>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Atm>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Atm>("Id"); //default, wajib ada atau EF error
            }

            //take & skip
            var takeList = list;
            if (skip != null)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            //var sql = takeList.ToString();
            List<Atm> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Atm> items = context.Atm;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Atm>(filters, ref items);
            }

            return items.Count();
        }
        public Atm FindByPK(int id)
        {
            return context.Atm.Where(d => d.Id == id).FirstOrDefault();
        }
        public Atm FindByDriver(int id)
        {
            return context.Atm.Where(d => d.IdDriver == id).FirstOrDefault();
        }

        public void delete(Atm dbitem, int usr_id)
        {
            context.Atm.Remove(dbitem);
            var query = "DELETE FROM dbo.\"Atm\" WHERE \"Id\" = " + dbitem.Id +";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Atm", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = usr_id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}