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
    public class DokumenRepo : IDokumenRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Dokumen dbitem, int id, string strQuery=null)
        {
            dbitem.SONumberUniq = dbitem.SONumber;
            if (dbitem.Id == 0) //create
            {
                context.Dokumen.Add(dbitem);
            }
            else //edit
            {
                dbitem.IsLengkap = dbitem.DokumenItem.Count() == dbitem.DokumenItem.Where(d => d.IsLengkap).Count();
                context.Dokumen.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
                var query = "UPDATE dbo.\"Dokumen\" SET \"IsComplete\" = " + dbitem.IsComplete + " \"ModifiedDate\" = " + dbitem.ModifiedDate + "\"IsReturn\" = " + dbitem.IsReturn + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Dokumen", QueryDetail = query + (strQuery == null ? "" : strQuery), RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
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

        public List<Dokumen> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Dokumen> list = context.Dokumen;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Dokumen>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Dokumen>("Id"); //default, wajib ada atau EF error
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
            List<Dokumen> result = takeList.ToList();
            return result;
        }

        public List<Dokumen> FindAllBilling(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Dokumen> list = context.Dokumen.Where(d => !d.IsAdmin && !d.IsComplete && d.SalesOrder.Status != "batal order");

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Dokumen>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Dokumen>("Id"); //default, wajib ada atau EF error
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
            List<Dokumen> result = takeList.ToList();
            return result;
        }

        public List<Dokumen> FindAllOnCallSJ(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Dokumen> list = context.Dokumen.Where(d => (d.ListIdSo == "" || d.ListIdSo == null) && d.SalesOrder.SalesOrderOncallId != null && d.IsAdmin && !d.IsComplete && (d.SalesOrder.Status == "dispatched" || d.SalesOrder.Status == "surat jalan"));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Dokumen>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Dokumen>("Id"); //default, wajib ada atau EF error
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
            List<Dokumen> result = takeList.ToList();
            return result;
        }

        public int CountOncallSJ(FilterInfo filters = null)
        {
            IQueryable<Dokumen> items = context.Dokumen.Where(d => (d.ListIdSo == "" || d.ListIdSo == null) && d.SalesOrder.SalesOrderOncallId != null && d.IsAdmin && !d.IsComplete && d.SalesOrder.Status != "batal order");
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
                GridHelper.ProcessFilters<Dokumen>(filters, ref items);
            return items.Count();
        }

        public List<Dokumen> FindAllOnCallSJReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Dokumen> list = context.Dokumen.Where(d => d.IsComplete);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Dokumen>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Dokumen>("Id"); //default, wajib ada atau EF error
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
            List<Dokumen> result = takeList.ToList();
            return result;
        }

        public Context.Dokumen FindBySalesOrderProsesKonsolidasiIdAndCust(int SalesOrderProsesKonsoldiasiId, int CustomerId)
        {
            Context.Dokumen dokumen = context.Dokumen.Where(d => d.IdSO == SalesOrderProsesKonsoldiasiId && d.IdCustomer == CustomerId).FirstOrDefault();
            return dokumen;
        }

        public List<Dokumen> FindAllKonsolidasiSJ(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Dokumen> list = context.Dokumen.Where(d => d.SONumber.Contains("KSO") && d.IsAdmin && !d.IsComplete);//.Take(150);
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
                GridHelper.ProcessFilters<Dokumen>(filters, ref list);
            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Dokumen>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
                list = list.OrderBy<Dokumen>("Id"); //default, wajib ada atau EF error
            var takeList = list;
            if (skip != null)
                takeList = takeList.Skip(skip.Value);
            if (take != null)
                takeList = takeList.Take(take.Value);
            List<Dokumen> result = takeList.ToList();
            return result;
        }

        public List<SalesOrderKontrakListSo> FindAllKontrakSJ(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrakListSo> list = context.SalesOrderKontrakListSo.Where(d => d.DokumenId != null && d.Dokumen.IsAdmin && !d.Dokumen.IsComplete && d.Status == "dispatched");

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKontrakListSo>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrderKontrakListSo>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrderKontrakListSo>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrderKontrakListSo> result = takeList.ToList();
            return result;
        }

        public int Count(FilterInfo filters = null)
        {
            IQueryable<Dokumen> items = context.Dokumen.Where(d => d.IsComplete != true && d.ListIdSo == "" && d.IsAdmin == true);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref items);
            }

            return items.Count();
        }

        public int CountOnCallSJReport(FilterInfo filters = null)
        {
            IQueryable<Dokumen> items = context.Dokumen.Where(d => d.IsComplete);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref items);
            }

            return items.Count();
        }

        public int CountKonsolidasiSJ(FilterInfo filters = null)
        {
            IQueryable<Dokumen> items = context.Dokumen.Where(d => d.SONumber.Contains("KSO") && d.IsAdmin && !d.IsComplete);//.Take(150);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref items);
            }

            return items.Count();
        }

        public int CountBilling(FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<Dokumen> items = context.Dokumen.Where(d => !d.IsAdmin && !d.IsComplete && (d.SalesOrder.SalesOrderKontrakId.HasValue && d.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Any(e => e.MuatDate > date2018) || d.SalesOrder.OrderTanggalMuat > date2018));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Dokumen>(filters, ref items);
            }

            return items.Count();
        }

        public int CountKontrak(FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrakListSo> items = context.SalesOrderKontrakListSo.Where(d => d.DokumenId != null && d.Dokumen.IsAdmin && !d.Dokumen.IsComplete);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKontrakListSo>(filters, ref items);
            }

            return items.Count();
        }
        public Dokumen FindByPK(int id)
        {
            return context.Dokumen.Where(d => d.Id == id).FirstOrDefault();
        }
        public Dokumen FindBySONumber(string SONumber)
        {
            return context.Dokumen.Where(d => d.SONumber == SONumber).FirstOrDefault();
        }
        public Dokumen FindBySO(int idSO)
        {
            return context.Dokumen.Where(d => d.IdSO == idSO && d.IsAdmin == true).FirstOrDefault();
        }
    }
}