using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Rekenings
    {
        [Key]
        public int Id { get; set; }
        public string NamaRekening { get; set; }
        public string NoRekening { get; set; }
        [ForeignKey("LookupCodeBank")]
        public int IdBank { get; set; }
        public string Type { get; set; }
        public bool SetAsDefault { get; set; }
        //public bool isDeleted { get; set; }
        public virtual LookupCode LookupCodeBank { get; set; }
    }
}