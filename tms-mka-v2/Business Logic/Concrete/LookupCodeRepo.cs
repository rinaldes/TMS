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
    public class LookupCodeRepo : ILookupCodeRepo
    {
        private ContextModel context = new ContextModel();
        public void save(LookupCode dbitem){
            if (dbitem.Id == 0) //create
            {
                context.LookupCode.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Lookup Code", QueryDetail = "Add " + dbitem.Deskripsi, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.LookupCode.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Lookup Code", QueryDetail = "Edit " + dbitem.Deskripsi, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<LookupCode> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<LookupCode> list = context.LookupCode;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<LookupCode>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    //
                    list = list.OrderBy<LookupCode>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<LookupCode>("Id"); //default, wajib ada atau EF error
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
            List<LookupCode> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<LookupCode> items = context.LookupCode;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<LookupCode>(filters, ref items);
            }

            return items.Count();
        }
        public LookupCode FindByPK(int? id)
        {
            return context.LookupCode.Where(d => d.Id == id).FirstOrDefault();
        }
        public LookupCode FindByName(string name)
        {
            var a = context.LookupCode.Where(d => d.Nama == name).FirstOrDefault();
            return context.LookupCode.Where(d => d.Nama == name).FirstOrDefault();
        }
        public LookupCode FindByNameAndCat(string name)
        {
            return context.LookupCode.Where(d => d.Nama == name && d.LookupCodeCategories.Category == "tms_grade").FirstOrDefault();
        }
        public List<Context.LookupCode> FindAllSPBU()
        {
            return context.LookupCode.Where(d => d.LookupCodeCategories.Category == "tms_SPBU").ToList();
        }
        public void delete(LookupCode dbitem)
        {
            context.LookupCode.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Lookup Code", QueryDetail = "Delete " + dbitem.Deskripsi, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}