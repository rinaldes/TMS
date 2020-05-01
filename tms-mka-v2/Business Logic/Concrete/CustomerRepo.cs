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
    public class CustomerRepo : ICustomerRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Customer dbitem){
            if (dbitem.Id == 0) //create
            {
                context.Customer.Add(dbitem);
            }
            else //edit
            {
                context.Customer.Attach(dbitem);
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

        public List<Customer> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Customer> list = context.Customer;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Customer>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Customer>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Customer>("CustomerNama"); //default, wajib ada atau EF error
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
            List<Customer> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Customer> items = context.Customer;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Customer>(filters, ref items);
            }

            return items.Count();
        }
        public Customer FindByPK(int id)
        {
            return context.Customer.Where(d => d.Id == id).FirstOrDefault();
        }
        public Customer FindByCode(string Code)
        {
            return context.Customer.Where(d => d.CustomerCode == Code).FirstOrDefault();
        }
        public Customer FindByName(string Code)
        {
            return context.Customer.Where(d => d.CustomerNama == Code).FirstOrDefault();
        }
        public void delete(Customer dbitem, int id)
        {
            context.Customer.Remove(dbitem);
            var query = "DELETE FROM dbo.\"Customer\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(string code, int id = 0)
        {
            if (id == 0)
            { return context.Customer.Any(p => p.CustomerCode.Contains(code)); }
            else
            { return context.Customer.Any(p => p.CustomerCode.Contains(code) && p.Id != id); }
        }
        public string generateCode(int urutan)
        {
            return "C-" + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutan()
        {
            return context.Customer.Count() == 0 ? 0 : context.Customer.Max(d => d.urutan);
        }
    }
}