using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Atm
    {
        public Atm()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NoKartu { get; set; }
        [ForeignKey("LookupCodeBank")]
        public int? IdBank { get; set; }
        public string NoRekening{ get; set; }
        public string AtasNama { get; set; }
        [ForeignKey("Driver")]
        public int? IdDriver { get; set; }
        //public bool IsDelete { get; set; }
        public virtual LookupCode LookupCodeBank { get; set; }
        public virtual Driver Driver { get; set; }
    }
}