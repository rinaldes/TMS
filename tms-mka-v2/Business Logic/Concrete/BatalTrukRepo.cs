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
    public class BatalTrukRepo : IBatalTrukRepo
    {
        private ContextModel context = new ContextModel();
        public void save(BatalTruk dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.BatalTruk.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Batal Truk", QueryDetail = "Add Batal Truk" + dbitem.SalesOrder, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.BatalTruk.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Batal Truk", QueryDetail = "Edit Batal Truk " + dbitem.SalesOrder, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<BatalTruk> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<BatalTruk> list = context.BatalTruk;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<BatalTruk>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<BatalTruk>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<BatalTruk>("Id"); //default, wajib ada atau EF error
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
            List<BatalTruk> result = takeList.ToList();
            return result;
        }        
        public int Count(FilterInfo filters = null)
        {
            IQueryable<BatalTruk> items = context.BatalTruk;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<BatalTruk>(filters, ref items);
            }

            return items.Count();
        }
        public BatalTruk FindByPK(int id)
        {
            return context.BatalTruk.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(BatalTruk dbitem)
        {
            context.BatalTruk.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Batal Truk", QueryDetail = "Delete Batal Truk " + dbitem.SalesOrder, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }        
    }
}