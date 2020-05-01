using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ZoneParkir
    {
        public ZoneParkir()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("LookUpCodeZone")]
        public int? IdZone { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("MasterPool")]
        public int IdPool { get; set; }
        public int Pit{ get; set; }
        public virtual LookupCode LookUpCodeZone { get; set; }
        public virtual MasterPool MasterPool { get; set; }
    }
}