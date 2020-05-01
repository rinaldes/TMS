using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaOnCallItem
    {
        public DaftarHargaOnCallItem()
        {

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DaftarHargaOnCall")]
        public int IdDaftarHargaOnCall { get; set; }
        public string NamaRuteDaftarHarga { get; set; }
        public string ListIdRute { get; set; }
        public string ListNamaRute { get; set; }
        [ForeignKey("JenisTrucks")]
        public int IdJenisTruck { get; set; }
        public int MinKg { get; set; }
        public Decimal Harga { get; set; }
        [ForeignKey("LookupCodeSatuan")]
        public int IdSatuanHarga { get; set; }
        public bool IsAdHoc { get; set; }
        public bool IsAsuransi { get; set; }
        public string PihakPenanggung { get; set; }
        public string TipeNilaiTanggungan { get; set; }
        public Decimal? NilaiTanggungan { get; set; }
        public Decimal? Premi { get; set; }        
        public string Keterangan { get; set; }

        public virtual DaftarHargaOnCall DaftarHargaOnCall { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual LookupCode LookupCodeSatuan { get; set; }
    }
}