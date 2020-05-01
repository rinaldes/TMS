using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderPickup
    {
        public SalesOrderPickup()
        {
            this.SalesOrderPickupLoadingAdd = new HashSet<SalesOrderPickupLoadingAdd>();
            this.SalesOrderPickupUnLoadingAdd = new HashSet<SalesOrderPickupUnLoadingAdd>();
            this.SalesOrderPickupComment = new HashSet<SalesOrderPickupComment>();
            this.SalesOrderPickupDp = new HashSet<SalesOrderPickupDp>();
        }

        [Key]
        public int SalesOrderPickupId { get; set; }
        public string SONumber { get; set; }
        public int Urutan { get; set; }
        public System.DateTime TanggalOrder { get; set; }
        public TimeSpan JamOrder { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? JenisTruckId { get; set; }
        [ForeignKey("MasterProduct")]
        public int? ProductId { get; set; }
        public System.DateTime TanggalPickup { get; set; }
        public TimeSpan JamPickup { get; set; }
        [ForeignKey("Rute")]
        public int? RuteId { get; set; }
        public string StrMultidrop { get; set; }
        public string Keterangan { get; set; }
        public string KeteranganLoading { get; set; }
        public string KeteranganUnloading { get; set; }
        public bool IsSelect { get; set; }

        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        public string KeteranganDataTruck { get; set; }
        [ForeignKey("Driver1")]
        public int? Driver1Id { get; set; }
        public string KeteranganDriver1 { get; set; }
        [ForeignKey("Driver2")]
        public int? Driver2Id { get; set; }
        public string KeteranganDriver2 { get; set; }

        public bool IsCash { get; set; }
        [ForeignKey("DriverTitip")]
        public int? IdDriverTitip { get; set; }
        [ForeignKey("Atm")]
        public int? AtmId { get; set; }
        public string KeteranganRek { get; set; }



        public virtual Customer Customer { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual MasterProduct MasterProduct { get; set; }
        public virtual Rute Rute { get; set; } 
        public virtual ICollection<SalesOrderPickupLoadingAdd> SalesOrderPickupLoadingAdd { get; set; }
        public virtual ICollection<SalesOrderPickupUnLoadingAdd> SalesOrderPickupUnLoadingAdd { get; set; }
        public virtual ICollection<SalesOrderPickupComment> SalesOrderPickupComment { get; set; }
        public virtual ICollection<SalesOrderPickupDp> SalesOrderPickupDp { get; set; }

        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual Driver DriverTitip { get; set; }
        public virtual Atm Atm { get; set; }

    }
}