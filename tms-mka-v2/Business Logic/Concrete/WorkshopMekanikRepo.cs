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
    public class WorkshopMekanikRepo : IWorkshopMekanikRepo
    {
        private ContextModel context = new ContextModel();
        public void save(WorkshopMekanik dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.WorkshopMekanik.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Workshop Mekanik", QueryDetail = "Add Workshop Mekanik" + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.WorkshopMekanik.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Workshop Mekanik", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<WorkshopMekanik> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<WorkshopMekanik> list = context.WorkshopMekanik;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<WorkshopMekanik>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<WorkshopMekanik>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<WorkshopMekanik>("Id"); //default, wajib ada atau EF error
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
            List<WorkshopMekanik> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<WorkshopMekanik> items = context.WorkshopMekanik;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<WorkshopMekanik>(filters, ref items);
            }

            return items.Count();
        }
        public WorkshopMekanik FindByPK(int id)
        {
            return context.WorkshopMekanik.Where(d => d.Id == id).FirstOrDefault();
        }

        public void delete(WorkshopMekanik dbitem)
        {
            context.WorkshopMekanik.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Workshop Mekanik", QueryDetail = "Delete Workshop Mekanik" + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}