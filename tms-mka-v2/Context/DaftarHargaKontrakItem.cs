using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaKontrakItem
    {
        public DaftarHargaKontrakItem()
        {

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DaftarHargaKontrak")]
        public int IdDaftarHargaKontrak { get; set; }
        public string NamaRuteDaftarHarga { get; set; }
        public string ListIdRute { get; set; }
        public string ListNamaRute { get; set; }
        [ForeignKey("JenisTrucks")]
        public int IdJenisTruck { get; set; }
        public int BeratMinimum { get; set; }
        public Decimal Harga { get; set; }
        [ForeignKey("LookupCodeSatuan")]
        public int IdSatuanHarga { get; set; }
        public Decimal? HargaRit2 { get; set; }
        public Decimal? Overtime { get; set; }
        public Decimal? RitaseBulan { get; set; }
        public bool IsAsuransi { get; set; }
        public string PihakPenanggung { get; set; }
        public string TipeNilaiTanggungan { get; set; }
        public Decimal? NilaiTanggungan { get; set; }
        public Decimal? Premi { get; set; }        
        public string Keterangan { get; set; }

        public virtual DaftarHargaKontrak DaftarHargaKontrak { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual LookupCode LookupCodeSatuan { get; set; }
    }
}