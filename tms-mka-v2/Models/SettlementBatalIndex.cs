using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class SettlementBatalIndex
    {
        public int Id { get; set; }
        public int? IdDriver { get; set; }
        public int? IdSalesOrder { get; set; }
        public string IdSoKontrak { get; set; }
        public string JenisOrder { get; set; }
        public string NoDn { get; set; }
        public string NoSo { get; set; }
        public string Customer { get; set; }
        public string KodeNama { get; set; }
        public string VehicleNo { get; set; }
        public string Driver { get; set; }
        public string JenisBatal { get; set; }
        public DateTime? Tanggal { get; set; }
        public bool IsProses { get; set; }
        public SettlementBatalIndex() { }
        public SettlementBatalIndex(Context.SettlementBatal dbitem)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSalesOrder;
            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                JenisOrder = "Oncall";
                NoDn = dbitem.SalesOrder.SalesOrderOncall.DN;
                NoSo = dbitem.SalesOrder.SalesOrderOncall.SONumber;
                KodeNama = dbitem.SalesOrder.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrder.SalesOrderOncall.Customer.CustomerNama;
                VehicleNo = dbitem.DataTruck == null ? (dbitem.SalesOrder.SalesOrderOncall.DataTruck == null ? null : dbitem.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo) : dbitem.DataTruck.VehicleNo;
                if (dbitem.IdDriver.HasValue || dbitem.SalesOrder != null && dbitem.SalesOrder.AdminUangJalan != null && dbitem.SalesOrder.AdminUangJalan.Driver1 != null)
                Driver = dbitem.IdDriver.HasValue ? dbitem.Driver.NamaDriver : dbitem.SalesOrder.AdminUangJalan.Driver1.NamaDriver;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue) {
                JenisOrder = "Pickup";
                NoSo = dbitem.SalesOrder.SalesOrderPickup.SONumber;
                KodeNama = dbitem.SalesOrder.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrder.SalesOrderPickup.Customer.CustomerNama;
                VehicleNo = dbitem.DataTruck == null ? (dbitem.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo) : dbitem.DataTruck.VehicleNo;
                Driver = dbitem.IdDriver.HasValue ? dbitem.Driver.NamaDriver : dbitem.SalesOrder.AdminUangJalan.Driver1.NamaDriver;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                JenisOrder = "Konsolidasi";
                NoDn = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DN;
                NoSo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                Customer = "";
                VehicleNo = dbitem.DataTruck == null ? (dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo) : dbitem.DataTruck.VehicleNo;
                Driver = dbitem.IdDriver.HasValue ? dbitem.Driver.NamaDriver : dbitem.SalesOrder.AdminUangJalan.Driver1.NamaDriver;
            }
            JenisBatal = dbitem.JenisBatal;
            Tanggal = dbitem.ModifiedDate;
            IsProses = dbitem.IsProses;
        }
        public SettlementBatalIndex(Context.SettlementBatal dbitem, Context.SalesOrderKontrakListSo dbso)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSalesOrder;
            IdSoKontrak = dbitem.IdSoKontrak;

            JenisOrder = "Kontrak";
            //NoDn = dbitem.SalesOrder.SalesOrderOncall.DN;
            NoSo = dbso.NoSo;
            Customer = dbitem.SalesOrder.SalesOrderKontrak.Customer.CustomerNama;
            VehicleNo = dbso.DataTruck == null ? "" : dbso.DataTruck.VehicleNo;
            Driver = dbitem.IdDriver.HasValue ? dbitem.Driver.NamaDriver : "";

            JenisBatal = dbitem.JenisBatal;
            Tanggal = dbitem.ModifiedDate;
            IsProses = dbitem.IsProses;
        }
    }
}