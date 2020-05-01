using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Inventaris
    {
        public Inventaris()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime TanggalPemberian { get; set; }
        public DateTime TanggalPengembalian { get; set; }
        [Required]
        [ForeignKey("LookupCode")]
        public int IdNamaBarang { get; set; }
        public string Keterangan { get; set; }
        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual LookupCode LookupCode { get; set; }
    }
}