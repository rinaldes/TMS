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
    public class RevisiJenisTrukRepo : IRevisiJenisTrukRepo
    {
        private ContextModel context = new ContextModel();
        public void save(RevisiJenisTruk dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.RevisiJenisTruk.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Revisi Jenis Truk", QueryDetail = "Add " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.RevisiJenisTruk.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Revisi Jenis Truk", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
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

        public List<RevisiJenisTruk> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<RevisiJenisTruk> list = context.RevisiJenisTruk;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<RevisiJenisTruk>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<RevisiJenisTruk>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<RevisiJenisTruk>("Id"); //default, wajib ada atau EF error
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
            List<RevisiJenisTruk> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<RevisiJenisTruk> items = context.RevisiJenisTruk;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<RevisiJenisTruk>(filters, ref items);
            }

            return items.Count();
        }
        public RevisiJenisTruk FindByPK(int id)
        {
            return context.RevisiJenisTruk.Where(d => d.Id == id).FirstOrDefault();
        }
        public RevisiJenisTruk FindBySo(int id)
        {
            return context.RevisiJenisTruk.Where(d => d.IdSalesOrder == id).FirstOrDefault();
        }

        public void delete(RevisiJenisTruk dbitem)
        {
            context.RevisiJenisTruk.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Revisi Jenis Truk", QueryDetail = "Delete " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }        
    }
}