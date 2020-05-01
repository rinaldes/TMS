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
    public class DaftarHargaKontrakRepo : IDaftarHargaKontrakRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DaftarHargaKontrak dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.DaftarHargaKontrak.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Daftar Harga Kontrak", QueryDetail = "Add " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.DaftarHargaKontrak.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Daftar Harga Kontrak", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
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

        public List<DaftarHargaKontrak> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DaftarHargaKontrak> list = context.DaftarHargaKontrak;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DaftarHargaKontrak>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<DaftarHargaKontrak>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DaftarHargaKontrak>("Id"); //default, wajib ada atau EF error
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
            List<DaftarHargaKontrak> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DaftarHargaKontrak> items = context.DaftarHargaKontrak;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DaftarHargaKontrak>(filters, ref items);
            }

            return items.Count();
        }
        public DaftarHargaKontrak FindByPK(int id)
        {
            return context.DaftarHargaKontrak.Where(d => d.Id == id).FirstOrDefault();
        }
        public DaftarHargaKontrakItem FindItemByPK(int id)
        {
            return context.DaftarHargaKontrakItem.Where(d => d.Id == id).FirstOrDefault();
        }

        public void delete(DaftarHargaKontrak dbitem)
        {
            context.DaftarHargaKontrak.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Daftar Harga Kontrak", QueryDetail = "Delete " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsPeriodeStartExist(DateTime startPeriode, int custId, int id = 0)
        {
            if (id == 0)
                return context.DaftarHargaKontrak.Any(d => d.PeriodStart == startPeriode && d.IdCust == custId);
            else
                return context.DaftarHargaKontrak.Any(d => d.PeriodStart == startPeriode && d.IdCust == custId && d.Id != id);
        }
        public bool IsPeriodValid(DateTime StrPeriode, DateTime EndPeriode, int custId, int id = 0)
        {
            if (id == 0)
                return context.DaftarHargaKontrak.Any(d => d.IdCust == custId && (StrPeriode <= d.PeriodEnd && EndPeriode >= d.PeriodStart));
            else
                return context.DaftarHargaKontrak.Any(d => d.IdCust == custId && (StrPeriode <= d.PeriodEnd && EndPeriode >= d.PeriodStart) && d.Id != id);
        }
    }
}