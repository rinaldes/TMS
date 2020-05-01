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
    public class VendorGpsRepo : IVendorGpsRepo
    {
        private ContextModel context = new ContextModel();
        public void save(VendorGps dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                context.VendorGps.Add(dbitem);
                var query = "INSERT INTO dbo.\"VendorGps\" (\"Nama\", \"Merk\", \"Alamat\", \"Email\", \"Telp\", \"Web\") VALUES (" + dbitem.Nama + ", " + dbitem.Merk + ", " + dbitem.Alamat + ", " +
                    dbitem.Email + ", " + dbitem.Telp + ", " + dbitem.Web + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Vendor List", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.VendorGps.Attach(dbitem);
                var query = "UPDATE dbo.\"VendorGps\" SET \"Nama\" = " + dbitem.Nama + ", \"Merk\" = " + dbitem.Merk + ", \"Alamat\" = " + dbitem.Alamat + ", \"Email\" = " + dbitem.Email + ", \"Telp\" = " +
                    dbitem.Telp + ", \"Web\" = " + dbitem.Web + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Vendor List", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<VendorGps> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<VendorGps> list = context.VendorGps;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<VendorGps>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<VendorGps>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<VendorGps>("Id"); //default, wajib ada atau EF error
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
            List<VendorGps> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<VendorGps> items = context.VendorGps;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<VendorGps>(filters, ref items);
            }

            return items.Count();
        }
        public VendorGps FindByPK(int id)
        {
            return context.VendorGps.Where(d => d.Id == id).FirstOrDefault();
        }
        public VendorGps FindByName(string name)
        {
            return context.VendorGps.Where(d => d.Nama == name).FirstOrDefault();
        }
        public void delete(VendorGps dbitem, int id)
        {
            context.VendorGps.Remove(dbitem);
            var query = "DELETE FROM dbo.\"VendorGps\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Vendor List", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}