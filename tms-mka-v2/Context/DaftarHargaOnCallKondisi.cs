using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaOnCallKondisi
    {
        public DaftarHargaOnCallKondisi()
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
        public string kondisi { get; set; }
        public bool IsInclude { get; set; }
        public bool IsBill { get; set; }
        public decimal? value { get; set; }
        public bool IsDefault { get; set; }
        public bool IsKota { get; set; }
        public bool IsTitik { get; set; }
        public decimal? ValKota { get; set; }
        public decimal? ValTitik { get; set; }
        public bool IsDelete { get; set; }
        public virtual DaftarHargaOnCall DaftarHargaOnCall { get; set; }
        
    }
}