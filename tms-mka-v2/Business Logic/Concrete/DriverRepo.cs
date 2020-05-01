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
    public class DriverRepo : IDriverRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Driver dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.Driver.Add(dbitem);
            }
            else //edit
            {
                context.Driver.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<Driver> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Driver> list = context.Driver;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Driver>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<Driver>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Driver>("Id"); //default, wajib ada atau EF error
            }

            //take & skip
            var takeList = list;
            if (skip != null && skip != 0)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null && skip != 0)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            //var sql = takeList.ToString();
            List<Driver> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Driver> items = context.Driver;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Driver>(filters, ref items);
            }

            return items.Count();
        }
        public Driver FindByPK(int id)
        {
            return context.Driver.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(Driver dbitem)
        {
            context.Driver.Remove(dbitem);
            context.SaveChanges();
        }
        public Driver FindByCode(string code)
        {
            return context.Driver.Where(d => d.KodeDriver == code).FirstOrDefault();
        }
        public string generateCode(DateTime tglGabung, int urutan)
        {
            return "DR-" + tglGabung.ToString("yyMMdd") + "." + (urutan).ToString().PadLeft(2, '0');
        }
        public int getUrutan(DateTime tglGbg)
        {
            return context.Driver.Where(d => d.TglGabung == tglGbg).Count() == 0 ? 0 : context.Driver.Where(d => d.TglGabung == tglGbg).Max(d => d.Urutan);
        }
        public bool IsKtpExist(string ktp, int id = 0)
        {
            if (id == 0)
                return context.Driver.Any( d => d.NoKtp == ktp);
            else
                return context.Driver.Any(d => d.NoKtp == ktp && d.Id != id);
        }
        public bool IsSimExist(string sim, int id = 0)
        {
            if (id == 0)
                return context.Driver.Any(d => d.NoSim == sim);
            else
                return context.Driver.Any(d => d.NoSim == sim && d.Id != id);
        }
    }
}