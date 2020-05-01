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
    public class JenisTruckRepo : IJenisTruckRepo
    {
        private ContextModel context = new ContextModel();
        public void save(JenisTrucks dbitem, int id){
            if (dbitem.Id == 0) //create
            {
                context.JenisTrucks.Add(dbitem);
                var query = "INSERT INTO dbo.\"JenisTrucks\" (\"StrJenisTruck\", \"GolTol\", \"Alias\", \"Biaya\", \"AcInterval\") VALUES (" + dbitem.StrJenisTruck + ", " + dbitem.GolTol + ", " + dbitem.Alias + ", " +
                    dbitem.Biaya + ", " + dbitem.AcInterval + ");";
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Jenis Truk", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.JenisTrucks.Attach(dbitem);
                var query = "UPDATE dbo.\"JenisTrucks\" SET \"StrJenisTruck\" = " + dbitem.StrJenisTruck + ", \"GolTol\" = " + dbitem.GolTol + ", \"Alias\" = " + dbitem.Alias + ", \"Biaya\" = " + dbitem.Biaya +
                    ", \"AcInterval\" = " + dbitem.AcInterval + "WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Jenis Truk", QueryDetail = "Edit " + dbitem.StrJenisTruck, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
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

        public List<JenisTrucks> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<JenisTrucks> list = context.JenisTrucks;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<JenisTrucks>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<JenisTrucks>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<JenisTrucks>("id"); //default, wajib ada atau EF error
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
            List<JenisTrucks> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<JenisTrucks> items = context.JenisTrucks;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<JenisTrucks>(filters, ref items);
            }

            return items.Count();
        }
        public JenisTrucks FindByPK(int id)
        {
            return context.JenisTrucks.Where(d => d.Id == id).FirstOrDefault();
        }
        public JenisTrucks FindByName(string name)
        {
            return context.JenisTrucks.Where(d => d.StrJenisTruck == name).FirstOrDefault();
        }
        public JenisTrucks FindByStrJenisTruck(string jenisTruck)
        {
            return context.JenisTrucks.Where(d => d.StrJenisTruck == jenisTruck).FirstOrDefault();
        }
        public void delete(JenisTrucks dbitem, int id)
        {
            context.JenisTrucks.Remove(dbitem);
            var query = "DELETE FROM dbo.\"JenisTrucks\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Jenis Truk", QueryDetail = "Delete " + dbitem.StrJenisTruck, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(string nama, int id = 0)
        {
            if (id == 0)
            { return context.JenisTrucks.Any(p => p.StrJenisTruck.Contains(nama)); }
            else
            { return context.JenisTrucks.Any(p => p.StrJenisTruck.Contains(nama) && p.Id != id); }
        }
    }
}