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
    public class ProductRepo : IProductRepo
    {
        private ContextModel context = new ContextModel();
        public void save(MasterProduct dbitem){
            if (dbitem.Id == 0) //create
            {
                context.MasterProduct.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Product", QueryDetail = "Add " + dbitem.NamaProduk, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.MasterProduct.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Product", QueryDetail = "Edit " + dbitem.NamaProduk, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
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

        public List<MasterProduct> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<MasterProduct> list = context.MasterProduct;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MasterProduct>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<MasterProduct>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<MasterProduct>("Id"); //default, wajib ada atau EF error
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
            List<MasterProduct> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<MasterProduct> items = context.MasterProduct;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MasterProduct>(filters, ref items);
            }

            return items.Count();
        }
        public MasterProduct FindByPK(int id)
        {
            return context.MasterProduct.Where(d => d.Id == id).FirstOrDefault();
        }
        public MasterProduct FindByName(string name)
        {
            return context.MasterProduct.Where(d => d.NamaProduk == name).FirstOrDefault();
        }
        public void delete(MasterProduct dbitem)
        {
            context.MasterProduct.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Product", QueryDetail = "Delete " + dbitem.NamaProduk, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(string nama, int id = 0)
        {
            if (id == 0)
            { return context.MasterProduct.Any(p => p.NamaProduk.Contains(nama)); }
            else
            { return context.MasterProduct.Any(p => p.NamaProduk.Contains(nama) && p.Id != id); }
        }
    }
}