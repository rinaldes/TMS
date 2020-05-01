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
    public class PelaksanaanTrainingRepo : IPelaksanaanTrainingRepo
    {
        private ContextModel context = new ContextModel();
        public void save(PelaksanaanTraining dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.PelaksanaanTraining.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Pelaksanaan Training", QueryDetail = "Add " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.PelaksanaanTraining.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Pelaksanaan Training", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<PelaksanaanTraining> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<PelaksanaanTraining> list = context.PelaksanaanTraining;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<PelaksanaanTraining>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<PelaksanaanTraining>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<PelaksanaanTraining>("Id"); //default, wajib ada atau EF error
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
            List<PelaksanaanTraining> result = takeList.ToList();
            return result;
        }
        public List<PelaksanaanTrainingDetail> FindAllDetail(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<PelaksanaanTrainingDetail> list = context.PelaksanaanTrainingDetail;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<PelaksanaanTrainingDetail>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<PelaksanaanTrainingDetail>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<PelaksanaanTrainingDetail>("Id"); //default, wajib ada atau EF error
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
            List<PelaksanaanTrainingDetail> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<PelaksanaanTraining> items = context.PelaksanaanTraining;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<PelaksanaanTraining>(filters, ref items);
            }

            return items.Count();
        }
        public PelaksanaanTraining FindByPK(int id)
        {
            return context.PelaksanaanTraining.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(PelaksanaanTraining dbitem)
        {
            context.PelaksanaanTraining.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Pelaksanaan Training", QueryDetail = "Delete " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}