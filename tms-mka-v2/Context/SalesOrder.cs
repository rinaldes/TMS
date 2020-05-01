using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrder
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("SalesOrderOncall")]
        public int? SalesOrderOncallId { get; set; }
        [ForeignKey("SalesOrderKontrak")]
        public int? SalesOrderKontrakId { get; set; }
        [ForeignKey("SalesOrderPickup")]
        public int? SalesOrderPickupId { get; set; }
        [ForeignKey("SalesOrderKonsolidasi")]
        public int? SalesOrderKonsolidasiId { get; set; }
        [ForeignKey("SalesOrderProsesKonsolidasi")]
        public int? SalesOrderProsesKonsolidasiId { get; set; }
        [ForeignKey("AdminUangJalan")]
        public int? AdminUangJalanId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Status { get; set; }
        public string StatusFlow { get; set; }
        public DateTime? DateStatus { get; set; }
        public bool isReturn { get; set; }
        public bool IsBatalTruk { get; set; }
        public string StatusBatal { get; set; }
        public bool DateRevised { get; set; }
        public bool RuteRevised { get; set; }
        public string oidErp { get; set; }
        public int urutan { get; set; }
        public string KeteranganBatal { get; set; }
        public bool PendapatanDiakui { get; set; }
        public string NoShipment { get; set; }
        public string DN { get; set; }
        public string SONumber { get; set; }
        [ForeignKey("DataTruck")]
        public int? DataTruckId { get; set; }
        [ForeignKey("JenisTruck")]
        public int? JenisTruckId { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        [ForeignKey("Driver")]
        public int? DriverId { get; set; }
        public System.DateTime? OrderTanggalMuat { get; set; }
        public string Rute { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual SalesOrderOncall SalesOrderOncall { get; set; }
        public virtual SalesOrderKontrak SalesOrderKontrak { get; set; }
        public virtual SalesOrderPickup SalesOrderPickup { get; set; }
        public virtual SalesOrderKonsolidasi SalesOrderKonsolidasi { get; set; }
        public virtual SalesOrderProsesKonsolidasi SalesOrderProsesKonsolidasi { get; set; }
        public virtual AdminUangJalan AdminUangJalan { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual JenisTrucks JenisTruck { get; set; }
        public virtual Driver Driver { get; set; }
    }
}