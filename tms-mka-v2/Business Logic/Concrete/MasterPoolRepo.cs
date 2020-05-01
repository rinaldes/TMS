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
    public class MasterPoolRepo : IMasterPoolRepo
    {
        private ContextModel context = new ContextModel();
        public void save(MasterPool dbitem, int id)
        {
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.MasterPool.Add(dbitem);
            }
            else //edit
            {
                context.MasterPool.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
                query += "DELETE FROM dbo.\"ZoneParkir\" WHERE \"IdPool\" = " + dbitem.Id + ";";
            }
            context.SaveChanges();
            foreach (Context.ZoneParkir item in dbitem.ListZoneParkir)
            {
                query += "INSERT INTO dbo.\"ZoneParkir\" (\"IdPool\", \"IdZone\", \"Pit\") VALUES (" + item.IdPool + ", " + dbitem.Id + ", " + item.Pit + ");";
            }
            var auditrail = new Auditrail {
                Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "Master Pool", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public List<MasterPool> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<MasterPool> list = context.MasterPool;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MasterPool>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<MasterPool>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<MasterPool>("Id"); //default, wajib ada atau EF error
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
            List<MasterPool> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<MasterPool> items = context.MasterPool;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MasterPool>(filters, ref items);
            }

            return items.Count();
        }
        public MasterPool FindByPK(int id)
        {
            return context.MasterPool.Where(d => d.Id == id).FirstOrDefault();
        }
        public MasterPool FindByNamePool(string namePool)
        {
            return context.MasterPool.Where(d => d.NamePool == namePool).FirstOrDefault();
        }
        public void delete(MasterPool dbitem)
        {
            context.MasterPool.Remove(dbitem);
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Master Pool", QueryDetail = "Delete " + dbitem.NamePool, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = 1 
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(string nama, int id = 0)
        {
            if (id == 0)
            { return context.MasterPool.Any(p => p.NamePool.Contains(nama)); }
            else
            { return context.MasterPool.Any(p => p.NamePool.Contains(nama) && p.Id != id); }
        }

        public Context.MasterPool FindByIPAddress(){
            string ip = null;// AppHelper.GetIPAddress().Split('.')[2];
            try
            {
                ip = AppHelper.GetIPAddress().Split('.')[2];
            }
            catch (Exception e){
                if (ip != "225" && ip != "224")
                    ip = "224";
            }
            return context.MasterPool.Where(d => d.IpAddress == ip).FirstOrDefault();
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
    }
}