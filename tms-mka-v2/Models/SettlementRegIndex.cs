using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;
using System.Net;
using System.Data.Entity;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Linq;

namespace tms_mka_v2.Models
{
    public class SettlementRegIndex
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public string JenisOrder { get; set; }
        public string NoSo { get; set; }
        public string Customer { get; set; }
        public string KodeNama { get; set; }
        public string VehicleNo { get; set; }
        public string Driver { get; set; }
        public string DriverCode { get; set; }
        public DateTime TanggalMuat { get; set; }
        public Decimal? TotalTambahan { get; set; }
        public string ListIdSo { get; set; }

        public SettlementRegIndex()
        {

        }
        public SettlementRegIndex(Context.SettlementReguler dbitem)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSalesOrder;
            if (dbitem.LisSoKontrak != null && dbitem.LisSoKontrak != "")
            {
                JenisOrder = "Kontrak";
                NoSo = dbitem.AdminUangJalan.SONumber;
                KodeNama = dbitem.AdminUangJalan.Customer.CustomerCodeOld;
                Customer = dbitem.AdminUangJalan.Customer.CustomerNama;
                VehicleNo = dbitem.AdminUangJalan.DataTruck.VehicleNo;

                Driver = dbitem.AdminUangJalan == null ? null : dbitem.AdminUangJalan.Driver1 == null ? null : dbitem.AdminUangJalan.Driver1.NamaDriver;
                TanggalMuat = dbitem.AdminUangJalan.AUJTanggalMuat.Value;
            }
            else if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                JenisOrder = "Oncall";
                NoSo = dbitem.SalesOrder.SalesOrderOncall.SONumber;
                KodeNama = dbitem.SalesOrder.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrder.SalesOrderOncall.Customer.CustomerNama;
                VehicleNo = dbitem.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo;
                Driver = dbitem.SalesOrder.AdminUangJalan == null ? null : dbitem.SalesOrder.AdminUangJalan.Driver1 == null ? null : dbitem.SalesOrder.AdminUangJalan.Driver1.NamaDriver;
                TanggalMuat = dbitem.SalesOrder.SalesOrderOncall.TanggalMuat.Value;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue) {
                JenisOrder = "Pickup";
                NoSo = dbitem.SalesOrder.SalesOrderPickup.SONumber;
                KodeNama = dbitem.SalesOrder.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrder.SalesOrderPickup.Customer.CustomerNama;
                VehicleNo = dbitem.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo;
                Driver = dbitem.SalesOrder.AdminUangJalan.Driver1.NamaDriver;
                TanggalMuat = dbitem.SalesOrder.SalesOrderPickup.TanggalPickup;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                JenisOrder = "Konsolidasi";
                NoSo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                Customer = "";
                VehicleNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                Driver = dbitem.SalesOrder.AdminUangJalan.Driver1.NamaDriver;
                TanggalMuat = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.TanggalMuat.Value;
            }
            ListIdSo = "0";
            TotalTambahan = dbitem.SettlementRegulerTambahanBiaya.Sum(d => d.Value);
        }
        public SettlementRegIndex(Context.SettlementReguler dbitem, List<Context.SalesOrderKontrakListSo> dbso)
        {
            if (dbso == null)
                dbso = context.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == dbitem.IdAdminUangJalan).ToList();
            Id = dbitem.Id;
            IdSalesOrder = dbitem.SalesOrder.Id;
            JenisOrder = "Kontrak";
            NoSo = string.Join(", ", dbso.Select(s => s.NoSo).ToList());
            Customer = dbitem.SalesOrder.SalesOrderKontrak.Customer.CustomerNama;
            KodeNama = dbitem.SalesOrder.SalesOrderKontrak.Customer.CustomerCodeOld;
            try
            {
                VehicleNo = dbso.FirstOrDefault().DataTruck.VehicleNo;
                TanggalMuat = dbso.OrderBy(d => d.MuatDate).FirstOrDefault().MuatDate;
            }
            catch (Exception)
            {
                VehicleNo = dbitem.AdminUangJalan.DataTruck.VehicleNo;
            }
            Driver = dbitem.AdminUangJalan.Driver1.NamaDriver;
            TotalTambahan = dbitem.SettlementRegulerTambahanBiaya.Sum(d => d.Value);
            ListIdSo = dbitem.LisSoKontrak;
            DriverCode = dbitem.AdminUangJalan == null ? null : dbitem.AdminUangJalan.Driver1 == null ? null : dbitem.AdminUangJalan.Driver1.KodeDriver;
        }
    }
}