using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderKonsolidasi
    {
        public SalesOrderKonsolidasi()
        {
            this.SalesOrderKonsolidasiDp = new HashSet<SalesOrderKonsolidasiDp>();
        }

        [Key]
        public int SalesOrderKonsolidasiId { get; set; }
        public string SONumber { get; set; }
        public string DN { get; set; }
        public string IKSNo { get; set; }
        public int Urutan { get; set; }
        public System.DateTime? TanggalOrder { get; set; }
        public TimeSpan JamOrder { get; set; }
        public System.DateTime? TanggalMasuk { get; set; }
        public System.DateTime? TanggalProses { get; set; }
        public TimeSpan JamMasuk { get; set; }
        public string SONumberCust { get; set; }
        [ForeignKey("LookupCustomer")]
        public int? CustomerId { get; set; }
        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }
        [ForeignKey("CustomerTagihan")]
        public int? NamaTagihanId { get; set; }
        public string Keterangan { get; set; }
        [ForeignKey("MasterProduct")]
        public int? ProductId { get; set; }
        public int? IdDaftarHargaItem { get; set; }
        public string StrDaftarHargaItem { get; set; }
        public string TypeKonsolidasi { get; set; }
        public decimal? Tonase { get; set; }
        public decimal? karton { get; set; }
        public decimal? Pallet { get; set; }
        public decimal? Container { get; set; }
        public decimal? m3 { get; set; }
        public bool isMinimumBerat { get; set; }
        public bool ProcessedByAUJ { get; set; }
        public bool ProcessedByKasir { get; set; }
        [ForeignKey("LookupCodeMinimum")]
        public int? MinimumId { get; set; }
        public String PerhitunganDasar { get; set; }
        public int? Harga { get; set; }
        public int? TotalHarga { get; set; }
        public string CaraBayar { get; set; }
        public bool IsSelect { get; set; }

        public virtual LookupCode LookupCustomer { get; set; }
        public virtual Customer Supplier { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Customer CustomerTagihan { get; set; }
        public virtual MasterProduct MasterProduct { get; set; }
        public virtual LookupCode LookupCodeMinimum { get; set; }
        public virtual LookupCode LookupCodeType { get; set; }

        public virtual ICollection<SalesOrderKonsolidasiDp> SalesOrderKonsolidasiDp { get; set; }
    }
}