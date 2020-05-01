using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using Newtonsoft.Json;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using tms_mka_v2.Security;
using tms_mka_v2.Infrastructure;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace tms_mka_v2.Controllers
{
    public class CalendarViewController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IDriverRepo RepoDriver;
        private ISettlementBatalRepo RepoSettlementBatal;
        private IAuditrailRepo RepoAuditrail;
        private ISpkRepo RepoSpk;
        private IDataTruckRepo RepoDataTruck;
        private IMonitoringVehicleRepo RepoMonitoringVehicle;
        public CalendarViewController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IDriverRepo repoDriver, ISettlementBatalRepo repoSettlementBatal,
            IAuditrailRepo repoAuditrail, ISpkRepo repoSpk, IDataTruckRepo repoDataTruck, IMonitoringVehicleRepo repoMonitoringVehicle)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoDriver = repoDriver;
            RepoSettlementBatal = repoSettlementBatal;
            RepoAuditrail = repoAuditrail;
            RepoSpk = repoSpk;
            RepoDataTruck = repoDataTruck;
            RepoMonitoringVehicle = repoMonitoringVehicle;
        }
        public ActionResult Detail()
        {
            return View();
        }

        public string BindingOutStandingOrder(DateTime start_date, string strJnsTruck, string strArea)
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                //oncall
                (
                d.SalesOrderOncallId != null &&
                d.SalesOrderOncall.TanggalMuat > start_date.AddDays(-1) && d.SalesOrderOncall.TanggalMuat < start_date.AddDays(1) ||
                    //konsolidasi
                d.SalesOrderProsesKonsolidasiId != null &&
                d.SalesOrderProsesKonsolidasi.TanggalMuat > start_date.AddDays(-1) && d.SalesOrderProsesKonsolidasi.TanggalMuat < start_date.AddDays(1) ||
                    //pickup
                d.SalesOrderPickupId != null &&
                d.SalesOrderPickup.TanggalPickup > start_date.AddDays(-1) && d.SalesOrderPickup.TanggalPickup < start_date.AddDays(1)) &&
                (d.Status == "save" || d.Status == "draft planning")).ToList();

            List<ObjOutstanding> obj = new List<ObjOutstanding>();

            foreach (var item in items)
            {
                if (item.SalesOrderOncallId.HasValue)
                {
                    obj.Add(new ObjOutstanding()
                    {
                        Area = RepoSalesOrder.FindRute(item.SalesOrderOncall.IdDaftarHargaItem.Value).AreaAsal.Nama,
                        Customer = item.SalesOrderOncall.Customer.CustomerCodeOld,
                        To = RepoSalesOrder.FindRute(item.SalesOrderOncall.IdDaftarHargaItem.Value).LocationTujuan.Nama,
                        Type = item.SalesOrderOncall.JenisTrucks.StrJenisTruck
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    obj.Add(new ObjOutstanding()
                    {
                        Area = RepoSalesOrder.FindRute(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).AreaAsal.Nama,
                        Customer = "",
                        To = RepoSalesOrder.FindRute(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).LocationTujuan.Nama,
                        Type = item.SalesOrderProsesKonsolidasi.JenisTrucks.StrJenisTruck
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    obj.Add(new ObjOutstanding()
                    {
                        Area = item.SalesOrderPickup.Rute.AreaAsal.Nama,
                        Customer = item.SalesOrderPickup.Customer.CustomerNama,
                        To = item.SalesOrderPickup.Rute.LocationTujuan.Nama,
                        Type = item.SalesOrderPickup.JenisTrucks.StrJenisTruck
                    });
                }
            }
            if (strArea != "" && strArea != null)
                obj = obj.Where(d => d.Area == strArea).ToList();
            if (strJnsTruck != "" && strJnsTruck != null)
                obj = obj.Where(d => d.Type == strJnsTruck).ToList();

            return new JavaScriptSerializer().Serialize(obj);
        }

        public string BindingCurrentUnitReady(DateTime start_date, string strJnsTruck, string strArea)
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                (
                    d.SalesOrderOncallId != null && d.SalesOrderOncall.TanggalMuat == start_date && d.SalesOrderOncall.IdDataTruck != null ||
                    d.SalesOrderProsesKonsolidasiId != null && d.SalesOrderProsesKonsolidasi.TanggalMuat == start_date && d.SalesOrderProsesKonsolidasi.IdDataTruck != null ||
                    d.SalesOrderPickupId != null && d.SalesOrderPickup.TanggalPickup == start_date && d.SalesOrderProsesKonsolidasi.IdDataTruck != null
                ) && (d.Status == "save konfirmasi" || d.Status == "admin uang jalan")
            ).ToList();

            List<ObjReady> obj = new List<ObjReady>();

            foreach (var item in items)
            {
                if (item.SalesOrderOncallId.HasValue)
                {
                    obj.Add(new ObjReady()
                    {
                        Area = RepoSalesOrder.FindRute(item.SalesOrderOncall.IdDaftarHargaItem.Value).AreaAsal.Nama,
                        Position = RepoSalesOrder.FindRute(item.SalesOrderOncall.IdDaftarHargaItem.Value).LocationTujuan.Nama,
                        Vehicle = item.SalesOrderOncall.DataTruck.VehicleNo,
                        Type = item.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck,
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    obj.Add(new ObjReady()
                    {
                        Area = RepoSalesOrder.FindRute(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).AreaAsal.Nama,
                        Position = RepoSalesOrder.FindRute(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).LocationTujuan.Nama,
                        Vehicle = item.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo,
                        Type = item.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck,
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    obj.Add(new ObjReady()
                    {
                        Area = item.SalesOrderPickup.Rute.AreaAsal.Nama,
                        Position = item.SalesOrderPickup.Rute.LocationTujuan.Nama,
                        Vehicle = item.SalesOrderPickup.DataTruck.VehicleNo,
                        Type = item.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck,
                    });
                }
            }

            if (strArea != "" && strArea != null)
                obj = obj.Where(d => d.Area == strArea).ToList();
            if (strJnsTruck != "" && strJnsTruck != null)
                obj = obj.Where(d => d.Type == strJnsTruck).ToList();

            return new JavaScriptSerializer().Serialize(obj);
        }

        public string BindingCurrentUnitFromOnDuty(DateTime start_date, string strJnsTruck, string strArea)
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                (d.SalesOrderOncallId != null && d.SalesOrderOncall.TanggalMuat < start_date.AddDays(RepoSalesOrder.FindRute(d.SalesOrderOncall.IdDaftarHargaItem.Value).WaktuKerja * -1) &&
                 d.SalesOrderOncall.IdDataTruck != null ||
                 d.SalesOrderProsesKonsolidasiId != null &&
                  d.SalesOrderProsesKonsolidasi.TanggalMuat < start_date.AddDays(RepoSalesOrder.FindRute(d.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).WaktuKerja * -1)
                  && d.SalesOrderProsesKonsolidasi.IdDataTruck != null ||
                 d.SalesOrderPickupId != null && d.SalesOrderPickup.TanggalPickup < start_date.AddDays(d.SalesOrderPickup.Rute.WaktuKerja * -1)) &&
                (d.Status != "settlement")).ToList();
            List<Context.SalesOrder> current_items = RepoSalesOrder.FindAll().Where(d =>
                (d.SalesOrderOncallId != null && d.SalesOrderOncall.TanggalMuat == start_date && d.SalesOrderOncall.IdDataTruck != null ||
                d.SalesOrderProsesKonsolidasiId != null && d.SalesOrderProsesKonsolidasi.TanggalMuat == start_date &&
                d.SalesOrderProsesKonsolidasi.IdDataTruck != null || d.SalesOrderPickupId != null && d.SalesOrderPickup.TanggalPickup == start_date) &&
                (d.Status != "settlement")).ToList();


            List<ObjReady> obj = new List<ObjReady>();

            foreach (var item in items)
            {
                if (item.SalesOrderOncallId.HasValue)
                {
                    obj.Add(new ObjReady()
                    {
                        Area = RepoSalesOrder.FindRute(item.SalesOrderOncall.IdDaftarHargaItem.Value).AreaTujuan.Nama,
                        Position = RepoSalesOrder.FindRute(item.SalesOrderOncall.IdDaftarHargaItem.Value).LocationTujuan.Nama,
                        Vehicle = item.SalesOrderOncall.DataTruck.VehicleNo,
                        Type = item.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck,
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    obj.Add(new ObjReady()
                    {
                        Area = RepoSalesOrder.FindRute(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).AreaTujuan.Nama,
                        Position = RepoSalesOrder.FindRute(item.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).LocationTujuan.Nama,
                        Vehicle = item.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo,
                        Type = item.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck,
                    });
                }
                else if (item.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    obj.Add(new ObjReady()
                    {
                        Area = item.SalesOrderPickup.Rute.AreaTujuan.Nama,
                        Position = item.SalesOrderPickup.Rute.LocationTujuan.Nama,
                        Vehicle = item.SalesOrderPickup.DataTruck.VehicleNo,
                        Type = item.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck,
                    });
                }
            }

            if (strArea != "" && strArea != null)
                obj = obj.Where(d => d.Area == strArea).ToList();
            if (strJnsTruck != "" && strJnsTruck != null)
                obj = obj.Where(d => d.Type == strJnsTruck).ToList();

            return new JavaScriptSerializer().Serialize(obj);
        }
        
        public string BindingCurrentUnitAvailable(DateTime start_date)
        {
            List<Context.DataTruck> items = RepoDataTruck.FindAll();
            List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().Where(d => (d.SalesOrderOncallId != null && d.SalesOrderOncall.TanggalMuat == start_date ||
                                    d.SalesOrderProsesKonsolidasiId != null && d.SalesOrderProsesKonsolidasi.TanggalMuat == start_date ||
                                    d.SalesOrderPickupId != null && d.SalesOrderPickup.TanggalPickup == start_date) &&
                                    (d.Status == "save konfirmasi" || d.Status == "admin uang jalan")).ToList();
            List<Context.SalesOrder> dbsoOnduty = RepoSalesOrder.FindAll().Where(d => (d.SalesOrderOncallId != null && d.SalesOrderOncall.TanggalMuat < start_date.AddDays(RepoSalesOrder.FindRute(d.SalesOrderOncall.IdDaftarHargaItem.Value).WaktuKerja * -1) ||
                                    d.SalesOrderProsesKonsolidasiId != null && d.SalesOrderProsesKonsolidasi.TanggalMuat < start_date.AddDays(RepoSalesOrder.FindRute(d.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).WaktuKerja * -1) ||
                                    d.SalesOrderPickupId != null && d.SalesOrderPickup.TanggalPickup < start_date.AddDays(d.SalesOrderPickup.Rute.WaktuKerja * -1)) &&
                                    (d.Status != "settlement")).ToList();
            List<Context.Spk> dbspk = RepoSpk.FindAll().Where(d => d.Estimasi < start_date).ToList();
            List<ObjReady> obj = new List<ObjReady>();

            foreach (Context.DataTruck item in items)
            {
                //ready
                if (dbso.Any(d => (d.SalesOrderOncallId != null && d.SalesOrderOncall.TanggalMuat == start_date && d.SalesOrderOncall.IdDataTruck == item.Id ||
                                    d.SalesOrderProsesKonsolidasiId != null && d.SalesOrderProsesKonsolidasi.TanggalMuat == start_date && d.SalesOrderProsesKonsolidasi.IdDataTruck == item.Id ||
                                    d.SalesOrderPickupId != null && d.SalesOrderPickup.TanggalPickup == start_date && d.SalesOrderProsesKonsolidasi.IdDataTruck == item.Id) && 
                                    (d.Status == "save konfirmasi" || d.Status == "admin uang jalan")))
                {
                    continue;
                }
                //onduty
                if (dbsoOnduty.Any(d => (d.SalesOrderOncallId != null && d.SalesOrderOncall.TanggalMuat < start_date.AddDays(RepoSalesOrder.FindRute(d.SalesOrderOncall.IdDaftarHargaItem.Value).WaktuKerja * -1) && d.SalesOrderOncall.IdDataTruck == item.Id ||
                                    d.SalesOrderProsesKonsolidasiId != null && d.SalesOrderProsesKonsolidasi.TanggalMuat < start_date.AddDays(RepoSalesOrder.FindRute(d.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value).WaktuKerja * -1) && d.SalesOrderProsesKonsolidasi.IdDataTruck == item.Id ||
                                    d.SalesOrderPickupId != null && d.SalesOrderPickup.TanggalPickup < start_date.AddDays(d.SalesOrderPickup.Rute.WaktuKerja * -1) && d.SalesOrderPickup.IdDataTruck == item.Id) && 
                                    (d.Status == "dispatched")))
                {
                    continue;
                }
                //service
                if (dbspk.Any(d => d.Workshop.IdVehicle == item.Id))
                    continue;
                //if (dbso.Any(d => (d.Status == "save" || d.Status == "draft planning" || d.Status == "save planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched" || d.Status == "admin uang jalan") &&
                //    ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.IdDataTruck == item.Id : false && d.SalesOrderOncall.TanggalMuat == start_date) ||
                //    (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.IdDataTruck == item.Id : false && d.SalesOrderPickup.TanggalPickup == start_date) ||
                //    (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.IdDataTruck == item.Id : false && d.SalesOrderProsesKonsolidasi.TanggalMuat == start_date) ||
                //    (d.SalesOrderKontrakId.HasValue ? d.SalesOrderKontrak.SalesOrderKontrakListSo.Any(date => date.MuatDate == start_date && date.IdDataTruck == item.Id) : false))))
                //{
                //    continue;
                //}
                if (RepoMonitoringVehicle.FindByVehicleNo(item.VehicleNo) != null)
                {
                    obj.Add(new ObjReady()
                    {
                        Area = RepoMonitoringVehicle.Area(RepoMonitoringVehicle.FindByVehicleNo(item.VehicleNo).Kabupaten),
                        Position = RepoMonitoringVehicle.FindByVehicleNo(item.VehicleNo).Alamat,
                        Vehicle = item.VehicleNo,
                        Type = item.JenisTrucks == null ? "" : item.JenisTrucks.StrJenisTruck
                    });
                }
            }
            return new JavaScriptSerializer().Serialize(obj);
        }
        
        public string BindingCurrentUnitFromService(DateTime start_date)
        {
            List<int> items = RepoSpk.FindAll().Where(d => d.Estimasi < start_date).Select(p => p.Workshop.IdVehicle).Distinct().ToList();
            return new JavaScriptSerializer().Serialize(items.Select(d => new
            {
                Area = RepoMonitoringVehicle.Area(RepoMonitoringVehicle.FindByVehicleNo(RepoDataTruck.FindByPK(d).VehicleNo).Kabupaten),
                Position = RepoMonitoringVehicle.FindByVehicleNo(RepoDataTruck.FindByPK(d).VehicleNo).Alamat,
                Vehicle = RepoDataTruck.FindByPK(d).VehicleNo,
                Type = RepoDataTruck.FindByPK(d).JenisTrucks.StrJenisTruck
            }));
        }
    }

    public class ObjOutstanding
    {
        public string Area { get; set; }
        public string Customer { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
    }
    public class ObjReady
    {
        public string Area { get; set; }
        public string Position { get; set; }
        public string Vehicle { get; set; }
        public string Type { get; set; }
    }
}
