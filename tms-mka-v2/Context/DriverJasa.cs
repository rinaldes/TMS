using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DriverJasa
    {
        public DriverJasa()
        { }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Driver")]
        public int IdDriver { get; set; }
        public DateTime Tanggal { get; set; }
        public string keterangan { get; set; }
        public virtual Driver Driver { get; set; }
    }
}