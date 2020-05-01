using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class BebanKlaimDriver
    {
        public BebanKlaimDriver()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Driver")]
        public int? IdDriver { get; set; }
        [ForeignKey("Klaim")]
        public int? IdKlaim { get; set; }

        public virtual Klaim Klaim { get; set; }
        public virtual Driver Driver { get; set; }
    }
}