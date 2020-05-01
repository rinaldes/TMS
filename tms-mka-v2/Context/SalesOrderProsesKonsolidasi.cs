using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderProsesKonsolidasi
    {
        public SalesOrderProsesKonsolidasi()
        {
            this.SalesOrderProsesKonsolidasiItem = new HashSet<SalesOrderProsesKonsolidasiItem>();
            this.SalesOrderProsesKonsolidasiLoadingAdd = new HashSet<SalesOrderProsesKonsolidasiLoadingAdd>();
            this.SalesOrderProsesKonsolidasiUnLoadingAdd = new HashSet<SalesOrderProsesKonsolidasiUnLoadingAdd>();
            this.SalesOrderProsesKonsolidasiComment = new HashSet<SalesOrderProsesKonsolidasiComment>();
        }

        [Key]
        public int SalesOrderProsesKonsolidasiId { get; set; }
        public string SONumber { get; set; }
        public string DN { get; set; }
        public int Urutan { get; set; }
        public System.DateTime? TanggalProses { get; set; }
        public System.DateTime? TanggalMuat { get; set; }
        public TimeSpan JamMuat { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? JenisTruckId { get; set; }
        public int? IdDaftarHargaItem { get; set; }
        public string StrDaftarHargaItem { get; set; }
        public string Multidrop { get; set; }
        public string Keterangan { get; set; }

        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        public string KeteranganDataTruck { get; set; }
        [ForeignKey("Driver1")]
        public int? Driver1Id { get; set; }
        public string KeteranganDriver1 { get; set; }
        [ForeignKey("Driver2")]
        public int? Driver2Id { get; set; }
        public string KeteranganDriver2 { get; set; }
        public bool LangsungKeBilling { get; set; }

        public bool IsCash { get; set; }
        [ForeignKey("DriverTitip")]
        public int? IdDriverTitip { get; set; }
        [ForeignKey("Atm")]
        public int? AtmId { get; set; }
        public string KeteranganRek { get; set; }

        public virtual JenisTrucks JenisTrucks { get; set; }

        public virtual ICollection<SalesOrderProsesKonsolidasiItem> SalesOrderProsesKonsolidasiItem { get; set; }
        public virtual ICollection<SalesOrderProsesKonsolidasiLoadingAdd> SalesOrderProsesKonsolidasiLoadingAdd { get; set; }
        public virtual ICollection<SalesOrderProsesKonsolidasiUnLoadingAdd> SalesOrderProsesKonsolidasiUnLoadingAdd { get; set; }
        public virtual ICollection<SalesOrderProsesKonsolidasiComment> SalesOrderProsesKonsolidasiComment { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual Driver DriverTitip { get; set; }
        public virtual Atm Atm { get; set; }
    }
}