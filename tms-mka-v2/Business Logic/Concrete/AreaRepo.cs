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
    public class AreaRepo : IAreaRepo
    {
        private ContextModel context = new ContextModel();
        public void save(MasterArea dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.MasterArea.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Area", QueryDetail = "Add " + dbitem.Nama, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.MasterArea.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Area", QueryDetail = "Edit " + dbitem.Nama, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<MasterArea> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<MasterArea> list = context.MasterArea;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MasterArea>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<MasterArea>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<MasterArea>("Id"); //default, wajib ada atau EF error
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
            List<MasterArea> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<MasterArea> items = context.MasterArea;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MasterArea>(filters, ref items);
            }

            return items.Count();
        }
        public MasterArea FindByPK(int id)
        {
            return context.MasterArea.Where(d => d.Id == id).FirstOrDefault();
        }
        public MasterArea FindByName(string name)
        {
            return context.MasterArea.Where(d => d.Nama == name).FirstOrDefault();
        }

        public MasterArea FindByCode(string kode)
        {
            return context.MasterArea.Where(d => d.Kode == kode).FirstOrDefault();
        }
        public void delete(MasterArea dbitem)
        {
            context.MasterArea.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Area", QueryDetail = "Delete " + dbitem.Nama, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public string generateCode(int urutan)
        {
            return "A-" + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutan()
        {
            return context.MasterArea.Count() == 0 ? 0 : context.MasterArea.Max(d => d.Urutan);
        }
        public bool IsExist(string name, int id = 0)
        {
            if (id == 0)
                return context.MasterArea.Any(d => d.Nama == name);
            else
                return context.MasterArea.Any(d => d.Nama == name && d.Id != id);
        }
    }
}