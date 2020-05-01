using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Klaim
    {
        public Klaim()
        {
            this.ListProduk = new HashSet<KlaimProduct>();
            this.ListAtts = new HashSet<KlaimAttachments>();
        }
        [Key]
        public int Id { get; set; }
        [ForeignKey("SalesOrder")]
        public int? SalesOrderId { get; set; }
        [ForeignKey("Driver")]
        public int? DriverId { get; set; }
        public int? SalesOrderKontrakId { get; set; }
        
        public string NoKlaim { get; set; }
        public System.DateTime? TanggalPengajuan { get; set; }
        [ForeignKey("LookUpStatusKlaim")]
        public int? StatusKlaim { get; set; }
        [ForeignKey("LookUpSumberInformasi")]
        public int? SumberInformasiId { get; set; }
        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        public string Code { get; set; }
        public string LaporanKejadian { get; set; }
        public string HasilPemeriksaan { get; set; }
        public string Penyelesaian { get; set; }
        public string IdBap { get; set; }
        public string NoBap { get; set; }
        public string Kesalahan { get; set; }
        public string KesalahanLain { get; set; }
        public string ClaimType { get; set; }
        public bool IsClaim { get; set; }

        public Double? TotalPengajuanClaim { get; set; }
        public Double? NilaiDisetujui { get; set; }
        public bool AsuransiFlag { get; set; }
        public Double? Asuransi { get; set; }
        public Double? BebanClaim { get; set; }
        public int? BebanClaimDriverPercentage { get; set; }
        public Double? BebanClaimDriver { get; set; }
        public int? BebanClaimKantorPercentage { get; set; }
        public Double? BebanClaimKantor { get; set; }
        public string Keterangan { get; set; }
        public System.DateTime? LastUpdate { get; set; }
        public string Status { get; set; }
        public System.DateTime? CreatedAt { get; set; }

        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
        public virtual LookupCode LookUpStatusKlaim { get; set; }
        public virtual LookupCode LookUpSumberInformasi { get; set; }
        public virtual ICollection<KlaimProduct> ListProduk{ get; set; }
        public virtual ICollection<KlaimAttachments> ListAtts { get; set; }
    }
}