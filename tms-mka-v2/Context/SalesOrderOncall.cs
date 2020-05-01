using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderOncall
    {
        public SalesOrderOncall()
        {
            this.SalesOrderOnCallLoadingAdd = new HashSet<SalesOrderOnCallLoadingAdd>();
            this.SalesOrderOnCallUnLoadingAdd = new HashSet<SalesOrderOnCallUnLoadingAdd>();
            this.SalesOrderOnCallComment = new HashSet<SalesOrderOnCallComment>();
            this.SalesOrderOncallDp = new HashSet<SalesOrderOncallDp>();
        }

        [Key]
        public int SalesOrderOnCallId { get; set; }
        public string SONumber { get; set; }
        public string DN { get; set; }
        public int Urutan { get; set; }
        public System.DateTime TanggalOrder { get; set; }
        public TimeSpan JamOrder { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        [ForeignKey("LookUpPrioritas")]
        public int? PrioritasId { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? JenisTruckId { get; set; }
        [ForeignKey("MasterProduct")]
        public int? ProductId { get; set; }
        public System.DateTime? TanggalMuat { get; set; }
        public TimeSpan JamMuat { get; set; }
        public int? IdDaftarHargaItem { get; set; }
        public string StrDaftarHargaItem { get; set; }
        public string StrMultidrop { get; set; }
        public string Keterangan { get; set; }
        public string KeteranganLoading { get; set; }
        public string KeteranganUnloading { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual LookupCode LookUpPrioritas { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual MasterProduct MasterProduct { get; set; }

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

        public virtual ICollection<SalesOrderOnCallLoadingAdd> SalesOrderOnCallLoadingAdd { get; set; }
        public virtual ICollection<SalesOrderOnCallUnLoadingAdd> SalesOrderOnCallUnLoadingAdd { get; set; }
        public virtual ICollection<SalesOrderOnCallComment> SalesOrderOnCallComment { get; set; }
        public virtual ICollection<SalesOrderOncallDp> SalesOrderOncallDp { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual Driver DriverTitip { get; set; }
        public virtual Atm Atm { get; set; }
    }
}