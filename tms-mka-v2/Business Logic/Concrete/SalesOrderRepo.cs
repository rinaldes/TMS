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
    public class SalesOrderRepo : ISalesOrderRepo
    {
        private ContextModel context = new ContextModel();
        public void save(SalesOrder dbitem, int? StatusFlow = null, string listSo = null)
        {
            dbitem.DateStatus = DateTime.Now;
            dbitem.DN = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.DN : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.DN : null;
            dbitem.SONumber = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderKonsolidasiId.HasValue ? dbitem.SalesOrderKonsolidasi.SONumber : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.SONumber : dbitem.SalesOrderKontrakId.HasValue ? dbitem.SalesOrderKontrak.SONumber : null;
            dbitem.CustomerId = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.CustomerId.Value : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.CustomerId : dbitem.SalesOrderKontrakId.HasValue ? dbitem.SalesOrderKontrak.CustomerId : null;
            if (dbitem.SalesOrderOncallId.HasValue){
                if (dbitem.SalesOrderOncall.Driver1Id.HasValue)
                    dbitem.DriverId = dbitem.SalesOrderOncall.Driver1Id.Value;
                if (dbitem.SalesOrderOncall.IdDataTruck.HasValue)
                    dbitem.DataTruckId = dbitem.SalesOrderOncall.IdDataTruck.Value;
                if (dbitem.SalesOrderOncall.JenisTruckId.HasValue)
                    dbitem.JenisTruckId = dbitem.SalesOrderOncall.JenisTruckId.Value;
                dbitem.OrderTanggalMuat = dbitem.SalesOrderOncall.TanggalMuat.Value;
                dbitem.Rute = dbitem.SalesOrderOncall.StrDaftarHargaItem;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                if (dbitem.SalesOrderPickup.Driver1Id.HasValue)
                    dbitem.DriverId = dbitem.SalesOrderPickup.Driver1Id.Value;
                if (dbitem.SalesOrderPickup.IdDataTruck.HasValue)
                    dbitem.DataTruckId = dbitem.SalesOrderPickup.IdDataTruck.Value;
                if (dbitem.SalesOrderPickup.JenisTruckId.HasValue)
                    dbitem.JenisTruckId = dbitem.SalesOrderPickup.JenisTruckId.Value;
                dbitem.OrderTanggalMuat = dbitem.SalesOrderPickup.TanggalPickup;
            }
            else if (dbitem.SalesOrderKonsolidasiId.HasValue && context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == dbitem.SalesOrderKonsolidasiId).FirstOrDefault() != null)
            {
                dbitem.DataTruckId = context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == dbitem.SalesOrderKonsolidasiId).FirstOrDefault().SalesOrderProsesKonsolidasi.IdDataTruck;
                dbitem.DriverId = context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == dbitem.SalesOrderKonsolidasiId).FirstOrDefault().SalesOrderProsesKonsolidasi.Driver1Id;
                dbitem.OrderTanggalMuat = context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == dbitem.SalesOrderKonsolidasiId).FirstOrDefault().SalesOrderProsesKonsolidasi.TanggalMuat;
                dbitem.SalesOrderKonsolidasi.IKSNo = context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == dbitem.SalesOrderKonsolidasiId).FirstOrDefault().SalesOrderProsesKonsolidasi.SONumber;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                if (dbitem.SalesOrderProsesKonsolidasi.Driver1Id.HasValue)
                    dbitem.DriverId = dbitem.SalesOrderProsesKonsolidasi.Driver1Id.Value;
                if (dbitem.SalesOrderProsesKonsolidasi.IdDataTruck.HasValue)
                    dbitem.DataTruckId = dbitem.SalesOrderProsesKonsolidasi.IdDataTruck.Value;
                if (dbitem.SalesOrderProsesKonsolidasi.JenisTruckId.HasValue)
                    dbitem.JenisTruckId = dbitem.SalesOrderProsesKonsolidasi.JenisTruckId.Value;
                dbitem.OrderTanggalMuat = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat;
                dbitem.Rute = dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
            }
            if (dbitem.AdminUangJalan != null)
            {
                dbitem.AdminUangJalan.SONumber = dbitem.SONumber;
                dbitem.AdminUangJalan.CustomerId = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.CustomerId : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.CustomerId : null;
                dbitem.AdminUangJalan.DataTruckId = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.IdDataTruck : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.IdDataTruck : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.IdDataTruck : null;
                dbitem.AdminUangJalan.Rute = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.StrDaftarHargaItem : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.Rute.Nama : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem : null;
                dbitem.AdminUangJalan.AUJTanggalMuat = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.TanggalMuat : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.TanggalPickup : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.TanggalMuat : null;
            }
            if (dbitem.Status.ToLower() == "draft")
                dbitem.StatusFlow = "MARKETING";
            else if (dbitem.Status.ToLower() == "save" || dbitem.Status.ToLower() == "draft planning")
                dbitem.StatusFlow = "PLANNING";
            else if (dbitem.Status.ToLower() == "save planning" || dbitem.Status.ToLower() == "draft konfirmasi")
                dbitem.StatusFlow = "KONFIRMASI";
            else if (dbitem.Status.ToLower() == "save konfirmasi")
                dbitem.StatusFlow = "ADMIN UANG JALAN";
            else if (dbitem.Status.ToLower() == "admin uang jalan")
                dbitem.StatusFlow = "KASIR";
            else
                dbitem.StatusFlow = dbitem.Status.ToUpper();
            var order_history = new OrderHistory();
            if (dbitem.Id == 0) //create
            {
                context.SalesOrder.Add(dbitem);
            }
            else
            {
                context.SalesOrder.Attach(dbitem);
                if (dbitem.Status == "save"){
                    var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == dbitem.Id && d.StatusFlow == 1 ).FirstOrDefault();
                    if (last_oh == null)
                        order_history = new OrderHistory { SalesOrderId = dbitem.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = DateTime.Now, ProcessedAt = DateTime.Now, PIC = dbitem.UpdatedBy, ListSo = listSo };
                    else
                        order_history = new OrderHistory { SalesOrderId = dbitem.Id, StatusFlow = StatusFlow == null ? 1 : StatusFlow.Value, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = dbitem.UpdatedBy, ListSo = listSo };
                }
                else if (StatusFlow != null)
                {
                    order_history = new OrderHistory { SalesOrderId = dbitem.Id, StatusFlow = StatusFlow.Value, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = dbitem.UpdatedBy, ListSo = listSo };
                }
                context.OrderHistory.Add(order_history);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            dbitem.DateStatus = DateTime.Now;
//            try
  //          {
                context.SaveChanges();
    //        }
      //      catch (Exception) { }
        }

        public List<Context.OrderHistory> oh() {
            return context.OrderHistory.ToList();
        }

        public void saveUangTf(AdminUangJalanUangTf dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.AdminUangJalanUangTf.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Sales Order List", QueryDetail = "Add " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.AdminUangJalanUangTf.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Sales Order List", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public List<SalesOrder> FindAllListOrder(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderKontrakId == null && d.SalesOrderKonsolidasiId == null);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("\"SONumber\" DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public Context.SalesOrder FindYgBentrok(string SONumber, int DriverId, DateTime TglMuat7)
        {
            Context.SalesOrder currentSO = context.SalesOrder.Where(d => d.SONumber == SONumber).FirstOrDefault();
            Context.DaftarHargaOnCallItem dhoc = context.DaftarHargaOnCallItem.Where(d => d.Id == currentSO.SalesOrderOncall.IdDaftarHargaItem).FirstOrDefault();
            return context.SalesOrder.Where(d => 
                d.Status != "batal order" && d.DriverId == DriverId && d.OrderTanggalMuat != null && d.SalesOrderOncallId.HasValue && 
                d.SalesOrderOncallId.HasValue &&  context.Rute.Any(f => context.DaftarHargaOnCallItem.Where(e => e.Id == d.SalesOrderOncall.IdDaftarHargaItem).FirstOrDefault().ListNamaRute.Contains(f.Nama) && !f.IsKotaKota) &&
                (d.OrderTanggalMuat >= currentSO.OrderTanggalMuat && d.OrderTanggalMuat.Value <= TglMuat7)
            ).FirstOrDefault();
        }

        public List<SalesOrder> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrder> list = context.SalesOrder;
            
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<Context.OrderHistory> FindAllHistory(int idSo){
            IQueryable<OrderHistory> list = context.OrderHistory.Where(d => d.SalesOrderId == idSo);
            List<Context.OrderHistory> result = list.OrderBy<OrderHistory>("ProcessedAt").ToList();
            return result;
        }
        public List<SalesOrder> FindAllOnCall(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderOncallId.HasValue);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public List<SalesOrder> FindAllKontrak(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderKontrakId.HasValue);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public List<SalesOrderKontrakListSo> FindAllKontrakListSo(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrderKontrakListSo> list = context.SalesOrderKontrakListSo;

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
                list = list.OrderBy<SalesOrderKontrakListSo>("Id DESC"); //default, wajib ada atau EF error
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
            List<SalesOrderKontrakListSo> result = takeList.ToList();
            return result;
        }

        public List<SalesOrder> FindAllPickUp(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderPickupId.HasValue);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrder> FindAllKonsolidasi(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderKonsolidasiId.HasValue && d.SalesOrderKonsolidasi.IsSelect != true);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("\"SONumber\" DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrder> FindAllReportKonsolidasi(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderKonsolidasiId.HasValue && d.SalesOrderKonsolidasi.IsSelect == true);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("\"SONumber\" DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrder> FindAllProsesKonsolidasi(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderProsesKonsolidasiId.HasValue);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrder> FindAllAdminUangJalan(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => (d.Status == "save konfirmasi" || (d.Status == "dispatched" && d.AdminUangJalan.Removal.Count > 0)) && d.OrderTanggalMuat > date2018);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public List<SalesOrder> FindAllAdminUangJalanSusulanKonsol(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderProsesKonsolidasiId.HasValue && (d.Status == "admin uang jalan" || d.Status == "dispatched") && d.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem.Select(e => e.SalesOrderKonsolidasi).Any(f => !f.ProcessedByAUJ));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public List<SalesOrder> FindAllAdminUangJalanKontrak(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderKontrakId.HasValue);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public List<SalesOrder> FindAllAdminDispatched(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => (d.Status == "dispatched" || d.Status == "surat jalan" || d.Status == "billing") && d.OrderTanggalMuat > date2018 || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(i => i.Status == "dispatched" && i.MuatDate > date2018)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public int CountAllAdminDispatched(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(i => i.Status == "dispatched")));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }
            return list.Count();
        }

        public int CountAllAdminUangJalanReport(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && (d.Status == "Sudah" || d.Status == "Batal"));


            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public List<SalesOrder> FindAllKasir(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.SalesOrderKontrakId == null && (d.Status == "admin uang jalan" || d.Status == "dispatched" && d.AdminUangJalan.AdminUangJalanUangTf.Any(e => e.Value > 0 && e.Keterangan != "Tunai" && !e.isTf)));// || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(e => (e.Status == "admin uang jalan" || e.Status == "dispatched") && e.AdminUangJalan.AdminUangJalanUangTf.Any(f => f.Value > 0 && f.Keterangan != "Tunai"))));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrder> FindAllKasirKas(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => ((d.Status == "admin uang jalan" || d.Status == "dispatched") && d.AdminUangJalan.AdminUangJalanUangTf.Any(e => e.Value > 0 && e.Keterangan == "Tunai" && !e.isTf) && !d.AdminUangJalan.AdminUangJalanUangTf.Any(e => e.JumlahTransfer > 0 && e.Keterangan == "Tunai" && e.isTf)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrder> FindAllKasirReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.Status == "batal order" || d.Status == "settlement" || d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && (d.SalesOrderKontrak.SalesOrderKontrakListSo.Where(e => e.Status == "dispatched").Count() > 0)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrder> FindAllAUJReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => d.IsBatalTruk == true || d.Status == "batal order" || d.Status == "settlement" || d.Status == "admin uang jalan" || d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && (d.Status == "draft konfirmasi" || d.Status == "save konfirmasi")));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrder>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<SalesOrder> result = takeList.ToList();
            return result;
        }
        public List<SalesOrderKontrakListSo> FindAllPlanningBatalKontrak(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrakListSo> list = context.SalesOrderKontrakListSo.Where(d => d.Status == "draft planning" && d.IsBatalTruck == true);

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
            if (skip != null && skip != 0)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null && take != 0)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            List <SalesOrderKontrakListSo> result = takeList.ToList();
            return result;
        }
        public List<AdminUangJalan> FindAllAdminUangJalanReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && (d.Status == "Sudah" || d.Status == "Batal"));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }
        public List<AdminUangJalan> FindAllKasirTfReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => (d.DaftarHargaKontrakId != null || d.SalesOrderId > 0) && d.AdminUangJalanUangTf.Any(e => e.isTf && e.Keterangan != "Tunai"));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public int CountKasirTfReport(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => (d.DaftarHargaKontrakId != null || d.SalesOrderId > 0) && (d.Status == "Sudah" || d.Status == "Batal") && d.AdminUangJalanUangTf.Any(e => e.isTf && e.Keterangan != "Tunai"));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public List<AdminUangJalan> FindAllKasirKasReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => (d.DaftarHargaKontrakId != null || d.SalesOrderId > 0) && d.AdminUangJalanUangTf.Any(e => e.isTf && e.Keterangan == "Tunai"));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public int CountAllSettlRegSO(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.Status == "Sudah" && (d.SalesOrderId > 0 || d.DaftarHargaKontrakId != null));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public List<AdminUangJalan> FindAllSettlRegSO(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => (d.Status != "Batal" && d.SalesOrderId > 0) || (d.DaftarHargaKontrakId != null));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public List<AdminUangJalan> FindAllBOBS(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched"));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public int CountAllBOBS(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && (d.Status == "Batal" && d.StatusSblmBatal == "admin uang jalan"));


            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public int CountAllTfBOBS(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && !d.TfExecuted && d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Transfer") && e.Value > 0 && e.JumlahTransfer == null));
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public List<AdminUangJalan> FindAllTfBOBS(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && !d.TfExecuted && d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Transfer") && e.Value > 0 && e.JumlahTransfer == null));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public int CountAllKasBOBS(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && !d.CashExecuted && d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Tunai") && e.Value > 0 && e.JumlahTransfer == null));
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public List<AdminUangJalan> FindAllKasBOBS(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && !d.CashExecuted && d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Tunai") && e.Value > 0 && e.JumlahTransfer == null));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public int CountAllTfBOBSReport(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && (d.TfExecuted || d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Transfer") && e.Value > 0 && e.JumlahTransfer == null)));
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public List<AdminUangJalan> FindAllTfBOBSReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && (d.TfExecuted || d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Transfer") && e.Value > 0 && e.JumlahTransfer == null)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public int CountAllKasBOBSReport(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && (d.CashExecuted || d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Tunai") && e.Value > 0 && e.JumlahTransfer == null)));
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            return list.Count();
        }

        public List<AdminUangJalan> FindAllKasBOBSReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId == null && d.SalesOrderId > 0 && d.Status == "Batal" && (d.StatusSblmBatal == "admin uang jalan" || d.StatusSblmBatal == "dispatched") && (d.CashExecuted || d.AdminUangJalanUangTf.Any(e => e.Keterangan.Contains("Tunai") && e.Value > 0 && e.JumlahTransfer == null)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("SONumber DESC"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public List<AdminUangJalan> FindAllAdminUangJalanKontrakReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId != null);
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("Id"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }

        public List<SalesOrder> FindAllKlaim(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => !d.SalesOrderKonsolidasiId.HasValue && !d.SalesOrderKontrakId.HasValue && (d.Status == "dispatched" || d.Status == "settlement" || d.Status == "billing" || d.Status == "surat jalan"));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
                list = list.OrderBy<SalesOrder>("Id"); //default, wajib ada atau EF error
            var takeList = list;
            if (skip != null && skip != 0)
                takeList = takeList.Skip(skip.Value);
            if (take != null && take != 0)
                takeList = takeList.Take(take.Value);
            List<SalesOrder> result = takeList.ToList();
            return result;
        }

        public List<SalesOrderKontrakListSo> FindAllKlaimKontrak(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrakListSo> list = context.SalesOrderKontrakListSo.Where(d => d.Status == "dispatched" || d.Status == "settlement");
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
                GridHelper.ProcessFilters<SalesOrderKontrakListSo>(filters, ref list);
            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrderKontrakListSo>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
                list = list.OrderBy<SalesOrderKontrakListSo>("\"NoSo\" DESC"); //default, wajib ada atau EF error
            var takeList = list;
            if (skip != null && skip != 0)
                takeList = takeList.Skip(skip.Value);
            if (take != null && take != 0)
                takeList = takeList.Take(take.Value);
            List<SalesOrderKontrakListSo> result = takeList.ToList();
            return result;
        }

        public int CountSoKlaim(FilterInfo filters = null)
        {
            IQueryable<SalesOrder> list = context.SalesOrder.Where(d => !d.SalesOrderKonsolidasiId.HasValue && (d.Status == "dispatched" || d.Status == "settlement" || (d.SalesOrderKontrakId.HasValue && d.Status == "draft konfirmasi")));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref list);
            }
            return list.Count();
        }

        public int CountSoKlaimKontrak(FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrakListSo> list = context.SalesOrderKontrakListSo.Where(d => d.Status == "dispatched" || d.Status == "settlement");

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
                GridHelper.ProcessFilters<SalesOrderKontrakListSo>(filters, ref list);
            return list.Count();
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<SalesOrder> items = context.SalesOrder;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref items);
            }

            return items.Count();
        }
        public int CountListOrder(FilterInfo filters = null)
        {
            IQueryable<SalesOrder> items = context.SalesOrder.Where(d => d.SalesOrderKontrakId == null && d.SalesOrderKonsolidasiId == null);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref items);
            }

            return items.Count();
        }
        public int CountFindAllAdminUangJalanKontrakReport(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> items = context.AdminUangJalan.Where(d => d.DaftarHargaKontrakId != null);
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref items);
            }

            return items.Count();
        }

        public int CountOncall(FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrak> items = context.SalesOrderKontrak;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKontrak>(filters, ref items);
            }

            return items.Count();
        }
        public int CountKontrak(FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrak> items = context.SalesOrderKontrak;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKontrak>(filters, ref items);
            }

            return items.Count();
        }

        public int CountKonfirmasiKontrak(FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrderKontrak> items = context.SalesOrderKontrak.Where(d => d.SalesOrderKontrakListSo.Any(e => (e.Status == "draft konfirmasi" || e.Status == "save planning" || e.Status == null)));
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKontrak>(filters, ref items);
            }

            return items.Count();
        }

        public List<SalesOrderKontrak> FindAllKonfirmasiKontrak(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<SalesOrderKontrak> list = context.SalesOrderKontrak.Where(d => d.SalesOrderKontrakListSo.Any(e => (e.Status == "draft konfirmasi" || e.Status == "save planning" || e.Status == null)));
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKontrak>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SalesOrderKontrak>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SalesOrderKontrak>("Id"); //default, wajib ada atau EF error
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
            List<SalesOrderKontrak> result = takeList.ToList();
            return result;
        }

        public int CountPickup(FilterInfo filters = null)
        {
            IQueryable<SalesOrderPickup> items = context.SalesOrderPickup;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderPickup>(filters, ref items);
            }

            return items.Count();
        }

        public int CountReportKonsolidasi(FilterInfo filters = null)
        {
            IQueryable<SalesOrder> items = context.SalesOrder.Where(d => d.SalesOrderKonsolidasiId.HasValue && d.SalesOrderKonsolidasi.IsSelect == true);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref items);
            }

            return items.Count();
        }
        public int CountKonsolidasi(FilterInfo filters = null)
        {
            IQueryable<SalesOrderKonsolidasi> items = context.SalesOrderKonsolidasi.Where(d => !d.IsSelect);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKonsolidasi>(filters, ref items);
            }

            return items.Count();
        }
        public int CountProsesKonsolidasi(FilterInfo filters = null)
        {
            IQueryable<SalesOrder> items = context.SalesOrder.Where(d => d.SalesOrderProsesKonsolidasiId.HasValue);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrder>(filters, ref items);
            }

            return items.Count();
        }
        public SalesOrder FindByAUJ(int id)
        {
            return context.SalesOrder.Where(d => d.AdminUangJalanId == id).FirstOrDefault();
        }
        public SalesOrder FindByPK(int id)
        {
            return context.SalesOrder.Where(d => d.Id == id).FirstOrDefault();
        }
        public SalesOrder FindByCode(string number)
        {
            return context.SalesOrder.Where(d => d.SONumber == number).FirstOrDefault();
        }
        public SalesOrder FindByOnCall(int id)
        {
            return context.SalesOrder.Where(d => d.SalesOrderOncallId == id).FirstOrDefault();
        }
        public SalesOrder FindByOnCallCode(string number)
        {
            return context.SalesOrder.Where(d => d.SalesOrderOncallId.HasValue && d.SalesOrderOncall.SONumber == number).FirstOrDefault();
        }
        public SalesOrder FindByKontrakCode(string number)
        {
            return context.SalesOrder.Where(d => d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SONumber == number).FirstOrDefault();
        }
        public SalesOrder FindByPickupCode(string number)
        {
            return context.SalesOrder.Where(d => d.SalesOrderPickupId.HasValue && d.SalesOrderPickup.SONumber == number).FirstOrDefault();
        }
        public SalesOrder FindByKontrak(int id)
        {
            return context.SalesOrder.Where(d => d.SalesOrderKontrakId == id).FirstOrDefault();
        }
        public SalesOrder FindByPickup(int id)
        {
            return context.SalesOrder.Where(d => d.SalesOrderPickupId == id).FirstOrDefault();
        }
        public SalesOrder FindByKonsolidasi(int id)
        {
            return context.SalesOrder.Where(d => d.SalesOrderKonsolidasiId == id).FirstOrDefault();
        }
        public SalesOrder FindByProsesKonsolidasi(int id)
        {
            return context.SalesOrder.Where(d => d.SalesOrderProsesKonsolidasiId == id).FirstOrDefault();
        }
        public void delete(SalesOrder dbitem, int id)
        {
            var auditrail = new Auditrail();
            if (dbitem.SalesOrderOncall != null)
            {
                dbitem.SalesOrderOncall.SalesOrderOnCallLoadingAdd.Clear();
                dbitem.SalesOrderOncall.SalesOrderOnCallUnLoadingAdd.Clear();
                dbitem.SalesOrderOncall.SalesOrderOnCallComment.Clear();
                context.SalesOrderOncall.Remove(dbitem.SalesOrderOncall);
            }
            else if (dbitem.SalesOrderKontrak != null)
            {
                dbitem.SalesOrderKontrak.SalesOrderKontrakDetail.Clear();
                context.SalesOrderKontrak.Remove(dbitem.SalesOrderKontrak);
            }
            else if (dbitem.SalesOrderPickup != null)
            {
                dbitem.SalesOrderPickup.SalesOrderPickupLoadingAdd.Clear();
                dbitem.SalesOrderPickup.SalesOrderPickupUnLoadingAdd.Clear();
                context.SalesOrderPickup.Remove(dbitem.SalesOrderPickup);
                var query = "DELETE FROM dbo.\"SalesOrderPickupLoadingAdd\" WHERE \"SalesOrderPickupId\" = " + dbitem.SalesOrderPickupId + ";";
                auditrail = new Auditrail {
                    Actionnya="Delete SO Pickup", EventDate=DateTime.Now, Modulenya = "Sales Order", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else if (dbitem.SalesOrderKonsolidasi != null)
            {
                context.SalesOrderKonsolidasi.Remove(dbitem.SalesOrderKonsolidasi);
            }
            else if (dbitem.SalesOrderProsesKonsolidasi != null)
            {
                foreach (SalesOrderProsesKonsolidasiItem item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                {
                    item.SalesOrderKonsolidasi.IsSelect = false;
                    item.SalesOrderKonsolidasi.Harga = 0;
                    item.SalesOrderKonsolidasi.TotalHarga = 0;
                    context.SalesOrderKonsolidasi.Attach(item.SalesOrderKonsolidasi);

                    var entry = context.Entry(item.SalesOrderKonsolidasi);
                    entry.State = EntityState.Modified;
                    context.SaveChanges();
                }
                dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem.Clear();
                dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiLoadingAdd.Clear();
                dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiUnLoadingAdd.Clear();
                context.SalesOrderProsesKonsolidasi.Remove(dbitem.SalesOrderProsesKonsolidasi);
                var query = "DELETE FROM dbo.\"SalesOrderProsesKonsolidasi\" WHERE \"SalesOrderProsesKonsolidasiId\" = " + dbitem.SalesOrderProsesKonsolidasiId + ";";
                query += "DELETE FROM dbo.\"SalesOrderProsesKonsolidasiItem\" WHERE \"SalesOrderProsesKonsolidasiId\" = " + dbitem.SalesOrderProsesKonsolidasiId + ";";
                query += "DELETE FROM dbo.\"SalesOrderProsesKonsolidasiLoadingAdd\" WHERE \"SalesOrderProsesKonsolidasiId\" = " + dbitem.SalesOrderProsesKonsolidasiId + ";";
                query += "DELETE FROM dbo.\"SalesOrderProsesKonsolidasiUnLoadingAdd\" WHERE \"SalesOrderProsesKonsolidasiId\" = " + dbitem.SalesOrderProsesKonsolidasiId + ";";
                auditrail = new Auditrail {
                    Actionnya="Delete SO Konsolidasi", EventDate=DateTime.Now, Modulenya = "Sales Order", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), 
                    IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }

            context.SalesOrder.Remove(dbitem);
            auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Sales Order List", QueryDetail = "Delete " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = 1 
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public string generateCodeOnCall(DateTime valdate,int urutan)
        {
            return "OC-" + valdate.ToString("yy") + valdate.Month.ToString("00").PadLeft(2, '0') + '-' + (urutan).ToString().PadLeft(4, '0');
        }
        public SalesOrderProsesKonsolidasiItem FindProsesKonsolidasiItem(int id)
        {
            return context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == id).FirstOrDefault();
        }
        public int getUrutanOnCAll(DateTime valdate)
        {
            DateTime firstDayOfMonth = new DateTime(valdate.Year, valdate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<SalesOrderOncall> dboncall = context.SalesOrder.Where(d => d.SalesOrderOncallId != null).Select(d=>d.SalesOrderOncall).ToList();
            return dboncall.Where(d => d.TanggalMuat >= firstDayOfMonth && d.TanggalMuat <= lastDayOfMonth && d.SONumber.Contains("OC-" + valdate.ToString("yy") + valdate.Month.ToString("00").PadLeft(2, '0') + "-")).Count() == 0 ? 0 : dboncall.Where(d => d.TanggalMuat >= firstDayOfMonth && d.TanggalMuat <= lastDayOfMonth && d.SONumber.Contains("OC-" + valdate.ToString("yy") + valdate.Month.ToString("00").PadLeft(2, '0') + "-")).Max(d => d.Urutan);
        }
        public string generateCodeKontrak(int urutan, DateTime MuatDate)
        {
            return "K-" + MuatDate.AddDays(1).ToString("yyMM") + '-' + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutanKontrak(DateTime valDate)
        {
            DateTime firstDayOfMonth = new DateTime(valDate.Year, valDate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<SalesOrderKontrak> dbkontrak = context.SalesOrder.Where(d => d.SalesOrderKontrakId != null).Select(d => d.SalesOrderKontrak).ToList();
            return dbkontrak.Where(d => d.PeriodStr >= firstDayOfMonth && d.PeriodStr <= lastDayOfMonth).Count() == 0 ? 0 : dbkontrak.Where(d => d.PeriodStr >= firstDayOfMonth && d.PeriodStr <= lastDayOfMonth).Max(d => d.Urutan);
        }
        public string generatePickup(DateTime valdate,int urutan)
        {
            return "NSO-" + valdate.ToString("yy") + valdate.Month.ToString("00").PadLeft(2, '0') + '-' + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutanPickup(DateTime valdate)
        {
            DateTime firstDayOfMonth = new DateTime(valdate.Year, valdate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<SalesOrderPickup> dbpickup = context.SalesOrder.Where(d => d.SalesOrderPickupId != null).Select(d => d.SalesOrderPickup).ToList();
            return dbpickup.Where(d => d.TanggalPickup >= firstDayOfMonth && d.TanggalPickup <= lastDayOfMonth).Count() == 0 ? 0 : dbpickup.Where(d => d.TanggalPickup >= firstDayOfMonth && d.TanggalPickup <= lastDayOfMonth).Max(d => d.Urutan);
        }
        public string generateKonsolidasi(DateTime valdate,int urutan)
        {
            return "KSO-" + valdate.ToString("yy") + valdate.Month.ToString("00").PadLeft(2, '0') + '-' + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutanKonsolidasi(DateTime valdate)
        {
            DateTime firstDayOfMonth = new DateTime(valdate.Year, valdate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<SalesOrderKonsolidasi> dbkonsolidasi = context.SalesOrder.Where(d => d.SalesOrderKonsolidasiId != null).Select(d => d.SalesOrderKonsolidasi).ToList();
            return dbkonsolidasi.Where(d => d.TanggalMasuk >= firstDayOfMonth && d.TanggalMasuk <= lastDayOfMonth).Count() == 0 ? 0 : dbkonsolidasi.Where(d => d.TanggalMasuk >= firstDayOfMonth && d.TanggalMasuk <= lastDayOfMonth).Max(d => d.Urutan);
        }
        public string generateProsesKonsolidasi(DateTime valdate,int urutan)
        {
            return "IKS-" + valdate.ToString("yy") + valdate.Month.ToString("00").PadLeft(2, '0') + '-' + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutanProsesKonsolidasi(DateTime valdate)
        {
            DateTime firstDayOfMonth = new DateTime(valdate.Year, valdate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<SalesOrderProsesKonsolidasi> dbproses = context.SalesOrder.Where(d => d.SalesOrderProsesKonsolidasiId != null).Select(d => d.SalesOrderProsesKonsolidasi).ToList();
            return dbproses.Where(d => d.TanggalMuat >= firstDayOfMonth && d.TanggalMuat <= lastDayOfMonth).Count() == 0 ? 0 : dbproses.Where(d => d.TanggalMuat >= firstDayOfMonth && d.TanggalMuat <= lastDayOfMonth).Max(d => d.Urutan);
        }
        public Context.SalesOrderProsesKonsolidasiItem FindByKonsolidasiId(int SalesOrderKonsolidasiId)
        {
            return context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == SalesOrderKonsolidasiId).FirstOrDefault();     
        }
        public string FindArea(int id)
        {
/*
Sales Order Oncall - IdDaftarHargaItem
Daftar Harga Oncall Item - Id
Daftar Harga Oncall Item - ListIdRute
Rute - Id
Rute - IdAreaAsal = 36 && IdAreaTujuan = 36
*/
            string idRutes = context.DaftarHargaOnCallItem.Where(d => d.Id == id).FirstOrDefault().ListIdRute;
            string[] rutes = idRutes.Split(',');
            if (rutes.Count() < 2)
            {
                int idRute = int.Parse(context.DaftarHargaOnCallItem.Where(d => d.Id == id).FirstOrDefault().ListIdRute);
                if (context.Rute.Where(d => d.Id == idRute).FirstOrDefault().IsKotaKota == true)
                    return "YES";
            }
            
            return null;
        }

        public decimal HargaKonsolidasiPerItem(Context.SalesOrderProsesKonsolidasiItem Detail)
        {
            Context.DaftarHargaKonsolidasiItem dhocItem = context.DaftarHargaKonsolidasiItem.Where(d => d.Id == Detail.SalesOrderKonsolidasi.IdDaftarHargaItem).FirstOrDefault();
            try
            {
                Context.DaftarHargaKonsolidasi dhoc = context.DaftarHargaKonsolidasi.Where(d => d.Id == dhocItem.IdDaftarHargaKonsolidasi).FirstOrDefault();
                int dhkItem = dhoc.DaftarHargaKonsolidasiKondisi.Where(d => d.IsInclude == true && d.IsBill == false && d.value > 0).Count();
                return (Detail.SalesOrderKonsolidasi.TotalHarga == null ? 0 : Detail.SalesOrderKonsolidasi.TotalHarga.Value) + (dhkItem > 0 ? dhoc.DaftarHargaKonsolidasiKondisi.Where(d => d.IsInclude == true && d.IsBill == false && d.value > 0).Sum(d => d.value.Value) : 0);
            }
            catch (Exception) {}
            return Detail.SalesOrderKonsolidasi.TotalHarga == null ? 0 : Detail.SalesOrderKonsolidasi.TotalHarga.Value;
        }

        public decimal Harga(Context.SalesOrder dbso, Context.SalesOrderKontrakListSo currentKontrak = null)
        {
            if (dbso.SalesOrderPickupId.HasValue)
                return 0;
            else if (dbso.SalesOrderProsesKonsolidasiId.HasValue){
                decimal harga = 0;
                List<SalesOrderKonsolidasi> listModel = new List<SalesOrderKonsolidasi>();
                foreach (Context.SalesOrderProsesKonsolidasiItem Detail in dbso.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
                {
                    Context.DaftarHargaKonsolidasiItem dhocItem = context.DaftarHargaKonsolidasiItem.Where(d => d.Id == Detail.SalesOrderKonsolidasi.IdDaftarHargaItem).FirstOrDefault();
                    Context.DaftarHargaKonsolidasi dhoc = context.DaftarHargaKonsolidasi.Where(d => d.Id == dhocItem.IdDaftarHargaKonsolidasi).FirstOrDefault();
                    int dhkItem = dhoc.DaftarHargaKonsolidasiKondisi.Where(d => d.IsInclude == true && d.IsBill == false && d.value > 0).Count();
                    harga += (Detail.SalesOrderKonsolidasi.TotalHarga == null ? 0 : Detail.SalesOrderKonsolidasi.TotalHarga.Value) + (dhkItem > 0 ? dhoc.DaftarHargaKonsolidasiKondisi.Where(d => d.IsInclude == true && d.IsBill == false && d.value > 0).Sum(d => d.value.Value) : 0);
                }
                return harga;
            }
            else if (dbso.SalesOrderKontrakId.HasValue)
            {
                Context.AdminUangJalan auj = currentKontrak.AdminUangJalan == null ? context.AdminUangJalan.Where(d => d.SONumber != null && d.SONumber.Contains(currentKontrak.NoSo) && d.Status != "Batal").FirstOrDefault() : currentKontrak.AdminUangJalan;
                Context.DaftarHargaKontrakItem dhocItem = context.DaftarHargaKontrakItem.Where(d => d.Id == auj.DaftarHargaKontrakId).FirstOrDefault();
                Context.DaftarHargaKontrak dhoc = context.DaftarHargaKontrak.Where(d => d.Id == dhocItem.IdDaftarHargaKontrak).FirstOrDefault();
                decimal hargaPerBulan = dhocItem.Harga + dhoc.DaftarHargaKontrakKondisi.Where(d => d.IsInclude == true && d.IsBill == false && d.value > 0).Sum(d => d.value.Value);
                return hargaPerBulan;
            }
            else{
                Context.DaftarHargaOnCallItem dhocItem = context.DaftarHargaOnCallItem.Where(d => d.Id == dbso.SalesOrderOncall.IdDaftarHargaItem).FirstOrDefault();
                Context.DaftarHargaOnCall dhoc = context.DaftarHargaOnCall.Where(d => d.Id == dhocItem.IdDaftarHargaOnCall).FirstOrDefault();
                return dhocItem.Harga + dhoc.DaftarHargaOnCallKondisi.Where(d => d.IsInclude == true && d.IsBill == false && d.value > 0).Sum(d => d.value.Value);
            }
        }

        public Context.Rute FindRute(int id)
        {
            string idRutes = context.DaftarHargaOnCallItem.Where(d => d.Id == id).FirstOrDefault().ListIdRute;
            string[] rutes = idRutes.Split(',');
            int rute = int.Parse(rutes[0]);
            return context.Rute.Where(d => d.Id == rute).FirstOrDefault();
        }

        public Context.DaftarHargaOnCallItem FindDH(int id)
        {
            return context.DaftarHargaOnCallItem.Where(d => d.Id == id).FirstOrDefault();
        }

        public double GroupPerMonth(DateTime valdate){
            DateTime firstDayOfMonth = new DateTime(valdate.Year, valdate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<SalesOrderOncall> dboncall = context.SalesOrder.Where(d => d.SalesOrderOncallId != null).Select(d=>d.SalesOrderOncall).ToList();
            double totalHarga = 0;
            foreach (SalesOrderOncall dbso in dboncall.Where(d => d.TanggalMuat >= firstDayOfMonth && d.TanggalMuat <= lastDayOfMonth)) {
                double harga = double.Parse(context.DaftarHargaOnCallItem.Where(d => d.Id == dbso.IdDaftarHargaItem).FirstOrDefault().Harga.ToString());
                totalHarga = totalHarga + harga;
            };
            return totalHarga;
        }

        public string IsMuatDateExist(List<String> muatDate, int custId, int id = 0)
        {
            
            List<string> wadah = new List<string>();
                        
            foreach (string kituweh in muatDate)
            {
                int t = 0;
                DateTime dateNaon = DateTime.Parse(kituweh);
                if (id == 0)
                    t = context.SalesOrderKontrak.Where(k => k.CustomerId == custId && k.SalesOrderKontrakDetail.Any(v => v.MuatDate == dateNaon)).Count();
                else
                    t = context.SalesOrderKontrak.Where(k => k.CustomerId == custId && k.SalesOrderKontrakId == id && k.SalesOrderKontrakDetail.Any(v => v.MuatDate == dateNaon)).Count();
                    

                if (t != 0)
                    wadah.Add(kituweh);
            }

            return string.Join(",  ", wadah);            
        } 

        public List<Context.OrderHistory> FindAllAUJHistory(){
            return context.OrderHistory.Where(d => d.StatusFlow == 4).ToList();
        }

        public List<Context.OrderHistory> FindAllKonfirmasiHistory(){
            return context.OrderHistory.Where(d => d.StatusFlow == 3).ToList();
        }

        public List<Context.OrderHistory> FindAllPlanningHistory(){
            return context.OrderHistory.Where(d => d.StatusFlow == 2).ToList();
        }

        public Context.OrderHistory FindMarketingHistory(int idSO){
            return context.OrderHistory.Where(d => d.StatusFlow == 1 && d.SalesOrderId == idSO && d.ProcessedAt.ToString() != "01/01/0001 00.00.00").FirstOrDefault();
        }

        public Context.OrderHistory FindPlanningHistory(int idSO){
            return context.OrderHistory.Where(d => d.StatusFlow == 2 && d.SalesOrderId == idSO && d.ProcessedAt.ToString() != "01/01/0001 00.00.00").FirstOrDefault();
        }

        public Context.OrderHistory FindKonfirmasiHistory(int idSO){
            return context.OrderHistory.Where(d => d.StatusFlow == 3 && d.SalesOrderId == idSO && d.ProcessedAt.ToString() != "01/01/0001 00.00.00").FirstOrDefault();
        }

        public string TanggalTiba(Context.SalesOrderOncall so){
            try{
                return context.MonitoringDetailSo.Where(d => d.NoSo == so.SONumber).FirstOrDefault().TglTiba.ToString();
            }
            catch (Exception){
                return "-";
            }
        }
        public string saveQueryGrMstr(string username, Context.Customer customer, Context.SalesOrder dbso, decimal value){
            string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
            try
            {
                return "INSERT INTO gr_mstr(gr_oid, gr_dom_id, gr_en_id, gr_add_by, gr_add_date, gr_code, gr_date, gr_branch_id, gr_ptnr_id," +
                    "gr_tax_basis_amount, gr_tax_amount, gr_total, gr_ptnr_code) VALUES ('" + Guid.NewGuid().ToString() + "', 1, 1, '" +
                    username + "', '" + now + "', 'GR-" + customer.CustomerCodeOld + "-" + dbso.SONumber + "-" + dbso.DataTruck.VehicleNo + "', '" + now + "', 10001, " + customer.Id + ", " +
                    value + ", 0, " + value + ", '" + customer.CustomerCodeOld + "');";
            }
            catch (Exception) {
                return "";
            }
        }

        public string saveQueryGrMstrKontrak(string username, Context.Customer customer, String VehicleNo, decimal value)
        {
            string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
            return "INSERT INTO gr_mstr(gr_oid, gr_dom_id, gr_en_id, gr_add_by, gr_add_date, gr_code, gr_date, gr_branch_id, gr_ptnr_id," +
                "gr_tax_basis_amount, gr_tax_amount, gr_total, gr_ptnr_code) VALUES ('" + Guid.NewGuid().ToString() + "', 1, 1, '" +
                username + "', '" + now + "', 'GR-" + customer.CustomerCodeOld + "-" + VehicleNo + "', '" + now + "', 10001, " + customer.Id + ", " +
                value + ", 0, " + value + ", '" + customer.CustomerCodeOld + "');";
        }

        public Context.SalesOrderKontrakTruck FindByKontrakIdAndTruckId(int SalesKontrakId, int DataTruckId)
        {
            return context.SalesOrderKontrakTruck.Where(d => d.SalesKontrakId == SalesKontrakId && d.DataTruckId == DataTruckId).FirstOrDefault();
        }
    }
}