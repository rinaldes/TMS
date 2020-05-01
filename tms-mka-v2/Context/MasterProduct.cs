using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class MasterProduct
    {
        [Key]
        public int Id { get; set; }
        public string NamaProduk { get; set; }
        [ForeignKey("LookupCode")]
        public int IdKategori { get; set; }
        public decimal TargetSuhu { get; set; }
        public decimal MaxTemps { get; set; }
        public decimal MinTemps { get; set; }
        public int Treshold { get; set; }
        public string Remarks { get; set; }
        public virtual LookupCode LookupCode { get; set; }
    }
}