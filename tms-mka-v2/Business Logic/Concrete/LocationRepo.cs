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
    public class LocationRepo : ILocationRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Location dbitem){
            if (dbitem.Id == 0) //create
            {
                context.Location.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Jenis Tol", QueryDetail = "Add " + dbitem.Nama, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.Location.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Jenis Tol", QueryDetail = "Edit " + dbitem.Nama, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
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

        public List<Location> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Location> list = context.Location;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Location>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<Location>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Location>("Id"); //default, wajib ada atau EF error
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
            List<Location> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Location> items = context.Location;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Location>(filters, ref items);
            }

            return items.Count();
        }
        public Location FindByPK(int id)
        {
            return context.Location.Where(d => d.Id == id).FirstOrDefault();
        }
        public Location FindByCode(string code)
        {
            return context.Location.Where(d => d.Code == code).FirstOrDefault();
        }
        public Location FindByNama(string nama)
        {
            return context.Location.Where(d => d.Nama.ToLower() == nama.ToLower()).FirstOrDefault();
        }
        public void delete(Location dbitem)
        {
            context.Location.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Jenis Tol", QueryDetail = "Delete " + dbitem.Nama, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(string code, int id = 0)
        {
            if (id == 0)
            { return context.Location.Any(p => p.Code == code); }
            else
            { return context.Location.Any(p => p.Code == code && p.Id != id); }
        }
        public Location FindByNamaNCode(string nama, string code)
        {
            return context.Location.Where(d => d.Nama == nama && d.Code == code).FirstOrDefault();
        }
    }
}