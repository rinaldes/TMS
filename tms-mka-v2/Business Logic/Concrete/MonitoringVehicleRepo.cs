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
    public class MonitoringVehicleRepo : IMonitoringVehicleRepo
    {
        private ContextModel context = new ContextModel();
        public void updateLocation(MonitoringVehicle dbitem)
        {
            context.MonitoringVehicle.Attach(dbitem);
            var entry = context.Entry(dbitem);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }
        public void saveReportGpsError(GpsErrorReport dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.GpsErrorReport.Add(dbitem);
            }
            else //edit
            {
                context.GpsErrorReport.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void saveAnalisaOnTemp(AnalisaOnTemp dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.AnalisaOnTemp.Add(dbitem);
            }
            else //edit
            {
                context.AnalisaOnTemp.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        /*
        public List<Context.HistoryGps> FindAllHistoryGps(string VehicleNo, string StartTime, string EndTime, int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime start = DateTime.Parse(StartTime);
            DateTime end = DateTime.Parse(EndTime);
            IQueryable<HistoryGps> list = context.HistoryGps.Where(d => d.VehicleNo == VehicleNo && d.CreatedDate >= start && d.CreatedDate <= end);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<HistoryGps>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<HistoryGps>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<HistoryGps>("VehicleNo"); //default, wajib ada atau EF error
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
            List<HistoryGps> result = takeList.ToList();
            return result;
        }
*/
        public List<Context.AnalisaOnTemp> FindAllAnalisaOnTemp(string VehicleNo, string StartTime, string EndTime, int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime start = DateTime.Parse(StartTime);
            DateTime end = DateTime.Parse(EndTime);
            IQueryable<AnalisaOnTemp> list = context.AnalisaOnTemp.Where(d => d.VehicleNo == VehicleNo && d.CreatedAt >= start && d.CreatedAt <= end);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AnalisaOnTemp>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AnalisaOnTemp>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AnalisaOnTemp>("VehicleNo"); //default, wajib ada atau EF error
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
            List<AnalisaOnTemp> result = takeList.ToList();
            return result;
        }

        public List<MonitoringVehicle> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<MonitoringVehicle> list = context.MonitoringVehicle;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MonitoringVehicle>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {

                    list = list.OrderBy<MonitoringVehicle>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<MonitoringVehicle>("VehicleNo"); //default, wajib ada atau EF error
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
            List<MonitoringVehicle> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<MonitoringVehicle> items = context.MonitoringVehicle;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<MonitoringVehicle>(filters, ref items);
            }
            return items.Count();
        }

        public int CountAllAnalisaOnTemp(string VehicleNo, string StartTime, string EndTime, int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime start = DateTime.Parse(StartTime);
            DateTime end = DateTime.Parse(EndTime);
            IQueryable<AnalisaOnTemp> items = context.AnalisaOnTemp.Where(d => d.VehicleNo == VehicleNo && d.CreatedAt >= start && d.CreatedAt <= end);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AnalisaOnTemp>(filters, ref items);
            }
            return items.Count();
        }

        public Context.MonitoringDetailSo FindMonitoringDetailSo(string NoSo)
        {
            return context.MonitoringDetailSo.Where(d => d.NoSo == NoSo).FirstOrDefault();
        }

        public Context.MonitoringDetailSo FindMonitoringDetailSoByPK(int id)
        {
            return context.MonitoringDetailSo.Where(d => d.Id == id).FirstOrDefault();
        }
        /*
        public Context.HistoryGps FindNextHistoryGpsByPK(int id, string VehicleNo)
        {
            return context.HistoryGps.Where(d => d.Id > id & d.VehicleNo == VehicleNo).OrderBy<HistoryGps>("\"Id\" ASC").FirstOrDefault();
        }
*/
        public Context.MonitoringDetailSo FindMonitoringDetailSoByVehicleNo(string VehicleNo)
        {
            Context.DataTruck truck = context.DataTruck.Where(d => d.VehicleNo == VehicleNo).FirstOrDefault();
            if (truck == null)
                return null;
            Context.SalesOrder so = context.SalesOrder.Where(d => d.Status == "dispatched" && d.SalesOrderOncall.TanggalMuat <= DateTime.Now && d.SalesOrderOncall.IdDataTruck == truck.Id).OrderBy<SalesOrder>("\"OrderTanggalMuat\" DESC").FirstOrDefault();
            return so == null ? null : context.MonitoringDetailSo.Where(d => d.NoSo == so.SONumber).FirstOrDefault();
        }

        public List<Context.MonitoringDetailSo> FindAllMonitoringDetailSoByVehicleNo(string VehicleNo)
        {
            Context.DataTruck truck = context.DataTruck.Where(d => d.VehicleNo == VehicleNo).FirstOrDefault();
            List<Context.SalesOrder> so = context.SalesOrder.Where(d => d.Status == "dispatched" && d.OrderTanggalMuat <= DateTime.Now && d.DataTruckId == truck.Id).OrderBy<SalesOrder>("\"OrderTanggalMuat\" DESC").ToList();
            String sonum = string.Join(", ", so.Select(e => e.SONumber));
            List<Context.SalesOrderKontrakListSo> sokls = context.SalesOrderKontrakListSo.Where(d => d.Status == "dispatched" && d.MuatDate <= DateTime.Now && d.IdDataTruck == truck.Id).OrderBy<SalesOrderKontrakListSo>("\"MuatDate\" DESC").ToList();
            String soklsnum = string.Join(", ", sokls.Select(e => e.NoSo));
            return context.MonitoringDetailSo.Where(d => sonum.Contains(d.NoSo) || soklsnum.Contains(d.NoSo)).ToList();
        }

        public List<Context.MonitoringDetailSo> FindAllMonitoringSo(int driverId)
        {
            return context.MonitoringDetailSo.Where(d => d.DriverId1 == driverId).ToList();
        }

        public Context.MonitoringVehicle FindByVehicleNo(string vehicleNo)
        {
            return context.MonitoringVehicle.Where(d => d.VehicleNo == vehicleNo).FirstOrDefault();
        }

        public Context.SalesOrderOncall FindSalesOrderOncall(string noSo)
        {
            return context.SalesOrderOncall.Where(d => d.SONumber == noSo).FirstOrDefault();
        }

        public string Area(string area)
        {
            Context.ListLocationArea ar = context.ListLocationArea.Where(d => d.Location.Nama.ToLower().Replace("kecamatan", "").Replace(" ", "") == area.ToLower().Replace("kecamatan", "").Replace(" ", "")).FirstOrDefault();
            return ar == null ? area : ar.MasterArea.Nama;
        }

        public void updateStatusToService(string vehicleNo)
        {
            Context.MonitoringVehicle dbitem = context.MonitoringVehicle.Where(d => d.VehicleNo == vehicleNo).FirstOrDefault();
            dbitem.Status = "Service";
            context.MonitoringVehicle.Attach(dbitem);
            var entry = context.Entry(dbitem);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }

        public void FindAllTruck(List<Models.MonitoringAll> ListModel)
        {
            List<MonitoringVehicle> ListMonitoringVehicle = context.MonitoringVehicle.Where(d => (d.LatNew != null && d.LatNew != "") && (d.LongNew != null && d.LongNew != "")).ToList();
            List<SalesOrder> listSO = context.SalesOrder.ToList();
            List<Workshop> listService = context.Workshop.ToList();

            foreach (var item in ListMonitoringVehicle)
            {
                Models.MonitoringAll model = new Models.MonitoringAll(item);
                model.Status = "Available";
                //liat so
                if (listSO != null)
                {
                    //selain kontrak
                    if (listSO.Any(d => (d.Status == "save planning" || d.Status == "draft konfirmasi") &&
                    ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.DataTruck.VehicleNo == item.VehicleNo : false) ||
                    (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.DataTruck.VehicleNo == item.VehicleNo : false) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo == item.VehicleNo : false))))
                    {
                        model.StatusFlow = "Konfirmasi";
                        model.Status = "On Duty";
                    }
                    else if (listSO.Any(d => (d.Status == "save konfirmasi" || d.Status == "admin uang jalan") &&
                    ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.DataTruck.VehicleNo == item.VehicleNo : false) ||
                    (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.DataTruck.VehicleNo == item.VehicleNo : false) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo == item.VehicleNo : false))))
                    {
                        model.StatusFlow = "Admin uang jalan";
                        model.Status = "On Duty";
                    }
                    else if (listSO.Any(d => (d.Status == "dispatched") &&
                    ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.DataTruck.VehicleNo == item.VehicleNo : false) ||
                    (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.DataTruck.VehicleNo == item.VehicleNo : false) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo == item.VehicleNo : false))))
                    {
                        model.StatusFlow = "Dispatched";
                        model.Status = "On Duty";
                    }

                    foreach (var itemKontrak in listSO.Where(d => d.SalesOrderKontrakId.HasValue))
                    {
                        if (itemKontrak.SalesOrderKontrak.SalesOrderKontrakListSo.Count > 0)
                        {
                            foreach (var itemKontrakListSo in itemKontrak.SalesOrderKontrak.SalesOrderKontrakListSo)
                            {
                                if (itemKontrakListSo.IdDataTruck.HasValue)
                                {
                                    if (itemKontrakListSo.Status == "draft planning" && itemKontrakListSo.DataTruck.VehicleNo == item.VehicleNo)
                                    {
                                        model.StatusFlow = "Planning";
                                        model.Status = "Ready";
                                    }
                                    if (itemKontrakListSo.Status == "admin uang jalan" && itemKontrakListSo.DataTruck.VehicleNo == item.VehicleNo)
                                    {
                                        model.StatusFlow = "Konfirmasi";
                                        model.Status = "Ready";
                                    }
                                    if (itemKontrakListSo.Status == "dispatched" && itemKontrakListSo.DataTruck.VehicleNo == item.VehicleNo)
                                    {
                                        model.StatusFlow = "Dispatched";
                                        model.Status = "On Duty";
                                    }
                                }
                            }
                        }
                    }
                }
                // liat service
                if (listService != null)
                {
                    foreach (var itemService in listService)
                    {
                        if (itemService.Truk.VehicleNo == item.VehicleNo)
                        {
                            model.StatusFlow = itemService.Status;
                            model.Status = "Service";
                        }
                    }
                }
                //last so
                HistoryJalanTruck LastSo = context.HistoryJalanTruck.Where(d => d.DataTruck.VehicleNo == model.Vehicle).OrderByDescending(d => d.TanggalMuat).FirstOrDefault();
                if (LastSo != null)
                {
                    model.LastSo = LastSo.NoSo;
                }

                ListModel.Add(model);
            }
        }

        public void FindOnDuty(List<Models.MonitoringOnduty> ListModel)
        {
            List<DataTruck> listTruck = context.DataTruck.Where(
                d => context.SalesOrder.Any(e => context.Dokumen.Any(f => f.IdSO == e.Id && f.IsAdmin) && e.SalesOrderOncall.IdDataTruck == d.Id && (e.Status == "dispatched"))
            ).ToList();
            foreach (var truck in listTruck)
            {
                Context.SalesOrder item = context.SalesOrder.Where(d => d.SalesOrderOncall.IdDataTruck == truck.Id && context.Dokumen.Any(e => e.IdSO == d.Id && e.IsAdmin) && (d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(i => i.Status == "dispatched")))).FirstOrDefault();
                if (item.SalesOrderOncallId.HasValue)
                {
                    var dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderOncall.DataTruck.VehicleNo).FirstOrDefault();
                    if (dataTruck != null)
                    {
                        Models.MonitoringOnduty model = new Models.MonitoringOnduty(dataTruck);

                        model.StatusFlow = "On Duty";
                        model.NoSo = item.SalesOrderOncall.SONumber;
                        model.Customer = item.SalesOrderOncall.Customer.CustomerNama;
                        model.Rute = item.SalesOrderOncall.StrDaftarHargaItem;
                        model.TglMuat = item.SalesOrderOncall.TanggalMuat.Value;

                        ListModel.Add(model);
                    }
                }
            }
/*            foreach (var item in listSO)
            {
                if (x < 490)
                {
                MonitoringVehicle dataTruck = new MonitoringVehicle();
                if (item.SalesOrderOncallId.HasValue)
                {
                    dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderOncall.DataTruck.VehicleNo).FirstOrDefault();
                    if (dataTruck != null)
                    {
                        Models.MonitoringOnduty model = new Models.MonitoringOnduty(dataTruck);

                        model.StatusFlow = "On Duty";
                        model.NoSo = item.SalesOrderOncall.SONumber;
                        model.Customer = item.SalesOrderOncall.Customer.CustomerNama;
                        model.Rute = item.SalesOrderOncall.StrDaftarHargaItem;
                        model.TglMuat = item.SalesOrderOncall.TanggalMuat.Value;

                        ListModel.Add(model);
                    }
                }
                else if (item.SalesOrderPickupId.HasValue)
                {
                    dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderPickup.DataTruck.VehicleNo).FirstOrDefault();
                    if (dataTruck != null)
                    {
                        Models.MonitoringOnduty model = new Models.MonitoringOnduty(dataTruck);
                        model.StatusFlow = "On Duty";
                        model.NoSo = item.SalesOrderPickup.SONumber;
                        model.Customer = item.SalesOrderPickup.Customer.CustomerNama;
                        model.Rute = item.SalesOrderPickup.Rute.Nama;
                        model.TglMuat = item.SalesOrderPickup.TanggalPickup;
                        ListModel.Add(model);
                    }
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo).FirstOrDefault();
                    if (dataTruck != null)
                    {
                        Models.MonitoringOnduty model = new Models.MonitoringOnduty(dataTruck);
                        model.StatusFlow = "On Duty";
                        model.NoSo = item.SalesOrderProsesKonsolidasi.SONumber;
                        model.Rute = item.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                        model.TglMuat = item.SalesOrderProsesKonsolidasi.TanggalMuat.Value;
                        ListModel.Add(model);
                    }
                }
                else if (item.SalesOrderKontrakId.HasValue)
                {
                    foreach (var itemKontrak in item.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Status == "dispatched"))
                    {
                        dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == itemKontrak.DataTruck.VehicleNo).FirstOrDefault();
                        if (dataTruck != null)
                        {
                            Models.MonitoringOnduty model = new Models.MonitoringOnduty(dataTruck);
                            model.StatusFlow = "On Duty";
                            model.NoSo = itemKontrak.NoSo;
                            model.Customer = item.SalesOrderKontrak.Customer.CustomerNama;
                            model.TglMuat = itemKontrak.MuatDate;
                            ListModel.Add(model);
                        }
                    }
                }
                x++;
                }
            }*/
        }

        public void FindOnTime(List<Models.MonitoringOntime> ListModel)
        {
            List<DataTruck> listTruck = context.DataTruck.Where(
                d => context.SalesOrder.Any(e => context.Dokumen.Any(f => f.IdSO == e.Id && f.IsAdmin) && e.SalesOrderOncall.IdDataTruck == d.Id && (e.Status == "dispatched"))
            ).ToList();
            foreach (var truck in listTruck)
            {
                Context.SalesOrder item = context.SalesOrder.Where(d => d.SalesOrderOncall.IdDataTruck == truck.Id && context.Dokumen.Any(e => e.IdSO == d.Id && e.IsAdmin) && (d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(i => i.Status == "dispatched")))).FirstOrDefault();
                if (item.SalesOrderOncallId.HasValue)
                {
                    var dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderOncall.DataTruck.VehicleNo).FirstOrDefault();
                    if (dataTruck != null)
                    {
                        Models.MonitoringOntime model = new Models.MonitoringOntime(dataTruck);

                        model.StatusFlow = "On Duty";
                        model.NoSo = item.SalesOrderOncall.SONumber;
                        model.Customer = item.SalesOrderOncall.Customer.CustomerNama;
                        model.Rute = item.SalesOrderOncall.StrDaftarHargaItem;
                        model.TglMuat = item.SalesOrderOncall.TanggalMuat.Value;

                        ListModel.Add(model);
                    }
                }
            }
/*            List<SalesOrder> listSO = context.SalesOrder.Where(d => context.Dokumen.Any(e => e.IdSO == d.Id && e.IsAdmin) && (d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(i => i.Status == "dispatched")))).ToList();
            int x = 0;
            foreach (var item in listSO)
            {
                MonitoringVehicle dataTruck = new MonitoringVehicle();
                if (x < 490)
                {
                    if (item.SalesOrderOncallId.HasValue)
                    {
                        dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderOncall.DataTruck.VehicleNo).FirstOrDefault();
                        if (dataTruck != null)
                        {
                            Models.MonitoringOntime model = new Models.MonitoringOntime(dataTruck);
                            //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();

                            //if (listHistory.Count > 0)
                            //{
                            //    listHistory = listHistory.Where(d => d.CreatedDate >= item.SalesOrderOncall.TanggalMuat).ToList();
                            //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                            //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                            //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                            //    if(dummyStop2 != null)
                            //    {
                            //        model.MaxStop = dummyStop2.Count();
                            //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                            //    }
                            //}

                            model.StatusFlow = "On Duty";
                            model.NoSo = item.SalesOrderOncall.SONumber;
                            model.Customer = item.SalesOrderOncall.Customer.CustomerNama;
                            model.Rute = item.SalesOrderOncall.StrDaftarHargaItem;
                            model.TglMuat = item.SalesOrderOncall.TanggalMuat.Value;

                            //rute, bagaimana jika yang rute nya 2 didaftar harga nya
                            var dataRute = context.DaftarHargaOnCallItem.Where(d => d.Id == item.SalesOrderOncall.IdDaftarHargaItem).FirstOrDefault();
                            int IdRute = int.Parse(dataRute.ListIdRute.Split(',').FirstOrDefault());
                            var _rute = context.Rute.Where(d => d.Id == IdRute);

                            ListModel.Add(model);
                        }
                    }
                    else if (item.SalesOrderPickupId.HasValue)
                    {
                        dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderPickup.DataTruck.VehicleNo).FirstOrDefault();
                        if (dataTruck != null)
                        {
                            Models.MonitoringOntime model = new Models.MonitoringOntime(dataTruck);
                            //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();
                            //if (listHistory.Count > 0)
                            //{
                            //    listHistory = listHistory.Where(d => d.CreatedDate >= item.SalesOrderPickup.TanggalPickup).ToList();

                            //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                            //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                            //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                            //    if (dummyStop2 != null)
                            //    {
                            //        model.MaxStop = dummyStop2.Count();
                            //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                            //    }
                            //}

                            model.StatusFlow = "On Duty";
                            model.NoSo = item.SalesOrderPickup.SONumber;
                            model.Customer = item.SalesOrderPickup.Customer.CustomerNama;
                            model.Rute = item.SalesOrderPickup.Rute.Nama;
                            model.TglMuat = item.SalesOrderPickup.TanggalPickup;
                            ListModel.Add(model);
                        }
                    }
                    else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo).FirstOrDefault();
                        if (dataTruck != null)
                        {
                            Models.MonitoringOntime model = new Models.MonitoringOntime(dataTruck);
                            //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();
                            //if (listHistory.Count > 0)
                            //{
                            //    listHistory = listHistory.Where(d => d.CreatedDate >= item.SalesOrderProsesKonsolidasi.TanggalMuat).ToList();

                            //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                            //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                            //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                            //    if (dummyStop2 != null)
                            //    {
                            //        model.MaxStop = dummyStop2.Count();
                            //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                            //    }
                            //}

                            model.StatusFlow = "On Duty";
                            model.NoSo = item.SalesOrderProsesKonsolidasi.SONumber;
                            model.Rute = item.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                            model.TglMuat = item.SalesOrderProsesKonsolidasi.TanggalMuat.Value;
                            ListModel.Add(model);
                        }
                    }
                    else if (item.SalesOrderKontrakId.HasValue)
                    {
                        foreach (var itemKontrak in item.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Status == "dispatched"))
                        {
                            dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == itemKontrak.DataTruck.VehicleNo).FirstOrDefault();
                            if (dataTruck != null)
                            {
                                Models.MonitoringOntime model = new Models.MonitoringOntime(dataTruck);
                                //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();
                                //if (listHistory.Count > 0)
                                //{
                                //    listHistory = listHistory.Where(d => d.CreatedDate >= itemKontrak.MuatDate).ToList();

                                //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                                //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                                //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                                //    if (dummyStop2 != null)
                                //    {
                                //        model.MaxStop = dummyStop2.Count();
                                //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                                //    }
                                //}

                                model.StatusFlow = "On Duty";
                                model.NoSo = itemKontrak.NoSo;
                                model.Customer = item.SalesOrderKontrak.Customer.CustomerNama;
                                model.TglMuat = itemKontrak.MuatDate;
                                ListModel.Add(model);
                            }
                        }
                    }
                    x++;
                }
            }*/
        }

        public void FindOnTemp(List<Models.MonitoringOntemp> ListModel)
        {
            List<DataTruck> listTruck = context.DataTruck.Where(
                d => context.SalesOrder.Any(e => context.Dokumen.Any(f => f.IdSO == e.Id && f.IsAdmin) && e.SalesOrderOncall.IdDataTruck == d.Id && (e.Status == "dispatched"))
            ).ToList();
            foreach (var truck in listTruck)
            {
                Context.SalesOrder item = context.SalesOrder.Where(d => d.SalesOrderOncall.IdDataTruck == truck.Id && context.Dokumen.Any(e => e.IdSO == d.Id && e.IsAdmin) && (d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(i => i.Status == "dispatched")))).FirstOrDefault();
                if (item.SalesOrderOncallId.HasValue)
                {
                    var dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderOncall.DataTruck.VehicleNo).FirstOrDefault();
                    if (dataTruck != null)
                    {
                        Models.MonitoringOntemp model = new Models.MonitoringOntemp(dataTruck);

                        model.StatusFlow = "On Duty";
                        model.NoSo = item.SalesOrderOncall.SONumber;
                        model.Customer = item.SalesOrderOncall.Customer.CustomerNama;
                        model.Rute = item.SalesOrderOncall.StrDaftarHargaItem;
                        model.TglMuat = item.SalesOrderOncall.TanggalMuat.Value;

                        ListModel.Add(model);
                    }
                }
            }
            /*            List<SalesOrder> listSO = context.SalesOrder.Where(d => d.Status == "dispatched" || (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(i => i.Status == "dispatched"))).ToList();
                        foreach (var item in listSO)
                        {
                            MonitoringVehicle dataTruck = new MonitoringVehicle();
                            if (item.SalesOrderOncallId.HasValue)
                            {
                                dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderOncall.DataTruck.VehicleNo).FirstOrDefault();
                                if (dataTruck != null)
                                {
                                    Models.MonitoringOntemp model = new Models.MonitoringOntemp(dataTruck);
                                    //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();

                                    //if (listHistory.Count > 0)
                                    //{
                                    //    listHistory = listHistory.Where(d => d.CreatedDate >= item.SalesOrderOncall.TanggalMuat).ToList();
                                    //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                                    //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                                    //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                                    //    if(dummyStop2 != null)
                                    //    {
                                    //        model.MaxStop = dummyStop2.Count();
                                    //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                                    //    }
                                    //}

                                    model.StatusFlow = "On Duty";
                                    model.NoSo = item.SalesOrderOncall.SONumber;
                                    model.Customer = item.SalesOrderOncall.Customer.CustomerNama;
                                    model.Rute = item.SalesOrderOncall.StrDaftarHargaItem;
                                    model.TglMuat = item.SalesOrderOncall.TanggalMuat.Value;
                                    model.JenisProduct = item.SalesOrderOncall.MasterProduct.NamaProduk;
                                    ListModel.Add(model);
                                }
                            }
                            else if (item.SalesOrderPickupId.HasValue)
                            {
                                dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderPickup.DataTruck.VehicleNo).FirstOrDefault();
                                if (dataTruck != null)
                                {
                                    Models.MonitoringOntemp model = new Models.MonitoringOntemp(dataTruck);
                                    //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();
                                    //if (listHistory.Count > 0)
                                    //{
                                    //    listHistory = listHistory.Where(d => d.CreatedDate >= item.SalesOrderPickup.TanggalPickup).ToList();

                                    //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                                    //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                                    //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                                    //    if (dummyStop2 != null)
                                    //    {
                                    //        model.MaxStop = dummyStop2.Count();
                                    //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                                    //    }
                                    //}

                                    model.StatusFlow = "On Duty";
                                    model.NoSo = item.SalesOrderPickup.SONumber;
                                    model.Customer = item.SalesOrderPickup.Customer.CustomerNama;
                                    model.Rute = item.SalesOrderPickup.Rute.Nama;
                                    model.TglMuat = item.SalesOrderPickup.TanggalPickup;
                                    model.JenisProduct = item.SalesOrderPickup.MasterProduct.NamaProduk;

                                    ListModel.Add(model);
                                }
                            }
                            else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                            {
                                dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == item.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo).FirstOrDefault();
                                if (dataTruck != null)
                                {
                                    Models.MonitoringOntemp model = new Models.MonitoringOntemp(dataTruck);
                                    //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();
                                    //if (listHistory.Count > 0)
                                    //{
                                    //    listHistory = listHistory.Where(d => d.CreatedDate >= item.SalesOrderProsesKonsolidasi.TanggalMuat).ToList();

                                    //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                                    //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                                    //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                                    //    if (dummyStop2 != null)
                                    //    {
                                    //        model.MaxStop = dummyStop2.Count();
                                    //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                                    //    }
                                    //}

                                    model.StatusFlow = "On Duty";
                                    model.NoSo = item.SalesOrderProsesKonsolidasi.SONumber;
                                    model.Rute = item.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                                    model.TglMuat = item.SalesOrderProsesKonsolidasi.TanggalMuat.Value;
                        
                                    ListModel.Add(model);
                                }
                            }
                            else if (item.SalesOrderKontrakId.HasValue)
                            {
                                foreach (var itemKontrak in item.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Status == "dispatched"))
                                {
                                    dataTruck = context.MonitoringVehicle.Where(d => d.VehicleNo == itemKontrak.DataTruck.VehicleNo).FirstOrDefault();
                                    if (dataTruck != null)
                                    {
                                        Models.MonitoringOntemp model = new Models.MonitoringOntemp(dataTruck);
                                        //List<HistoryGps> listHistory = context.HistoryGps.Where(d => d.VehicleNo == dataTruck.VehicleNo).ToList();
                                        //if (listHistory.Count > 0)
                                        //{
                                        //    listHistory = listHistory.Where(d => d.CreatedDate >= itemKontrak.MuatDate).ToList();

                                        //    model.TotalStop = listHistory.Where(d => d.Ac == "OFF").Count();
                                        //    var dummyStop = listHistory.Where(d => d.Ac == "OFF").GroupBy(d => d.Kabupaten).ToList();
                                        //    var dummyStop2 = dummyStop.Where(d => d.Count() == dummyStop.Max(s => s.Count())).FirstOrDefault();
                                        //    if (dummyStop2 != null)
                                        //    {
                                        //        model.MaxStop = dummyStop2.Count();
                                        //        model.MaxStopPosition = dummyStop2.FirstOrDefault().Kabupaten;
                                        //    }
                                        //}

                                        model.StatusFlow = "On Duty";
                                        model.NoSo = itemKontrak.NoSo;
                                        model.Customer = item.SalesOrderKontrak.Customer.CustomerNama;
                                        model.TglMuat = itemKontrak.MuatDate;

                                        ListModel.Add(model);
                                    }
                                }
                            }
                        }*/
        }

        public void FindService(List<Models.MonitoringService> ListModel)
        {
            List<MonitoringVehicle> ListMonitoringVehicle = context.MonitoringVehicle.Where(d => (d.LatNew != null && d.LatNew != "") && (d.LongNew != null && d.LongNew != "")).ToList();
            List<SalesOrder> listSO = context.SalesOrder.ToList();
            List<Workshop> listService = context.Workshop.ToList();
            foreach (var item in ListMonitoringVehicle)
            {
                Models.MonitoringService model = new Models.MonitoringService(item);
                model.StatusFlow = "Available";
                if (listService != null)
                {
                    foreach (var itemService in listService)
                    {
                        if (itemService.Truk.VehicleNo == item.VehicleNo)
                        {
                            model.Status = itemService.Status;
                            //liat so
                            if (listSO != null)
                            {
                                var dataReady = listSO.Where(d => (d.Status == "draft planning" || d.Status == "save planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "admin uang jalan") &&
                                ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.DataTruck.VehicleNo == item.VehicleNo : false) ||
                                (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.DataTruck.VehicleNo == item.VehicleNo : false) ||
                                (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo == item.VehicleNo : false))).FirstOrDefault();

                                var dataOnduty = listSO.Where(d => (d.Status == "dispatched") &&
                                ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.DataTruck.VehicleNo == item.VehicleNo : false) ||
                                (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.DataTruck.VehicleNo == item.VehicleNo : false) ||
                                (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo == item.VehicleNo : false))).FirstOrDefault();
                                

                                //selain kontrak
                                if (dataReady != null)
                                {
                                    if(dataReady.SalesOrderOncallId.HasValue)
                                    {
                                        model.Customer = dataReady.SalesOrderOncall.Customer.CustomerNama;
                                        model.NoSo = dataReady.SalesOrderOncall.SONumber;
                                        model.Rute = dataReady.SalesOrderOncall.StrDaftarHargaItem;
                                    }
                                    else if(dataReady.SalesOrderPickupId.HasValue)
                                    {
                                        model.Customer = dataReady.SalesOrderPickup.Customer.CustomerNama;
                                        model.NoSo = dataReady.SalesOrderPickup.SONumber;
                                        model.Rute = dataReady.SalesOrderPickup.Rute.Nama;
                                    }
                                    else if(dataReady.SalesOrderProsesKonsolidasiId.HasValue)
                                    {
                                        model.NoSo = dataReady.SalesOrderProsesKonsolidasi.SONumber;
                                        model.Rute = dataReady.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                                    }
                                    model.StatusFlow = "Ready";
                                }
                                else if (dataOnduty != null)
                                {
                                    if(dataOnduty.SalesOrderOncallId.HasValue)
                                    {
                                        model.Customer = dataOnduty.SalesOrderOncall.Customer.CustomerNama;
                                        model.NoSo = dataOnduty.SalesOrderOncall.SONumber;
                                        model.Rute = dataOnduty.SalesOrderOncall.StrDaftarHargaItem;
                                    }
                                    else if(dataOnduty.SalesOrderPickupId.HasValue)
                                    {
                                        model.Customer = dataOnduty.SalesOrderPickup.Customer.CustomerNama;
                                        model.NoSo = dataOnduty.SalesOrderPickup.SONumber;
                                        model.Rute = dataOnduty.SalesOrderPickup.Rute.Nama;
                                    }
                                    else if(dataOnduty.SalesOrderProsesKonsolidasiId.HasValue)
                                    {
                                        model.NoSo = dataOnduty.SalesOrderProsesKonsolidasi.SONumber;
                                        model.Rute = dataOnduty.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                                    }
                                    model.StatusFlow = "On Duty";
                                }
                                //kontrak
                                foreach (var itemKontrak in listSO.Where(d => d.SalesOrderKontrakId.HasValue))
                                {
                                    if (itemKontrak.SalesOrderKontrak.SalesOrderKontrakListSo.Count > 0)
                                    {
                                        foreach (var itemKontrakListSo in itemKontrak.SalesOrderKontrak.SalesOrderKontrakListSo)
                                        {
                                            if (itemKontrakListSo.IdDataTruck.HasValue)
                                            {
                                                if ((itemKontrakListSo.Status == "draft planning" || itemKontrakListSo.Status == "admin uang jalan") && itemKontrakListSo.DataTruck.VehicleNo == item.VehicleNo)
                                                {
                                                    model.Customer = itemKontrak.SalesOrderKontrak.Customer.CustomerNama;
                                                    model.StatusFlow = "Ready";
                                                }
                                                if (itemKontrakListSo.Status == "dispatched" && itemKontrakListSo.DataTruck.VehicleNo == item.VehicleNo)
                                                {
                                                    model.Customer = itemKontrak.SalesOrderKontrak.Customer.CustomerNama;
                                                    model.StatusFlow = "On Duty";
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            var dataItemService =  itemService.Spk.OrderByDescending(d => d.Estimasi).FirstOrDefault();
                            if (dataItemService != null)
                            {
                                model.EstimasiService = dataItemService.Estimasi;
                                model.RevEstimasi = dataItemService.RevEstimasi;
                                model.KeteranganService = dataItemService.KeteranganSPK;
                            }

                            ListModel.Add(model);
                        }
                    }
                }
            }
        }
    }
}