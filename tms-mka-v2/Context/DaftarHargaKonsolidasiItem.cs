using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaKonsolidasiItem
    {
        public DaftarHargaKonsolidasiItem()
        {

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DaftarHargaKonsolidasi")]
        public int IdDaftarHargaKonsolidasi { get; set; }
        public string NamaDaftarHargaRute { get; set; }
        public string ListIdRute { get; set; }
        public string ListNamaRute { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? IdJenisKendaraan { get; set; }
        public int MinKg { get; set; }
        public int MaxKg { get; set; }
        public Decimal Harga { get; set; }
        [ForeignKey("LookupCodeSatuan")]
        public int IdSatuanHarga { get; set; }
        public bool IsAsuransi { get; set; }
        public Decimal? Premi { get; set; }
        public string PihakPenanggung { get; set; }
        public string TipeNilaiTanggungan { get; set; }
        public Decimal? NilaiTanggungan { get; set; }
        public string Keterangan { get; set; }

        public virtual DaftarHargaKonsolidasi DaftarHargaKonsolidasi { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual LookupCode LookupCodeSatuan { get; set; }
    }
}