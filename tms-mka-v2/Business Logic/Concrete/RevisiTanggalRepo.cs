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
    public class RevisiTanggalRepo : IRevisiTanggalRepo
    {
        private ContextModel context = new ContextModel();
        public void save(RevisiTanggal dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.RevisiTanggal.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Revisi Tanggal", QueryDetail = "Add " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.RevisiTanggal.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Revisi Rute", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
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

        public List<RevisiTanggal> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<RevisiTanggal> list = context.RevisiTanggal;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<RevisiTanggal>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<RevisiTanggal>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<RevisiTanggal>("Id"); //default, wajib ada atau EF error
            }

            //take & skip
            var takeList = list;
            if (skip != null && skip != 0)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null && take != 0)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            //var sql = takeList.ToString();
            List<RevisiTanggal> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<RevisiTanggal> items = context.RevisiTanggal;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<RevisiTanggal>(filters, ref items);
            }

            return items.Count();
        }
        public RevisiTanggal FindByPK(int id)
        {
            return context.RevisiTanggal.Where(d => d.Id == id).FirstOrDefault();
        }
        public RevisiTanggal FindBySo(int id)
        {
            return context.RevisiTanggal.Where(d => d.IdSalesOrder == id && !d.IsBP).FirstOrDefault();
        }
        public RevisiTanggal FindBySoAndBP(int id)
        {
            return context.RevisiTanggal.Where(d => d.IdSalesOrder == id && d.IsBP).FirstOrDefault();
        }
        public void delete(RevisiTanggal dbitem)
        {
            context.RevisiTanggal.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Revisi Rute", QueryDetail = "Delete " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }        
    }
}