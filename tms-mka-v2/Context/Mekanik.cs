using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Mekanik
    {
        public Mekanik()
        {
            
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NamaMekanik { get; set; }
        [ForeignKey("LookUpCodeBagian")]
        public int? IdBagian { get; set; }
        [ForeignKey("LookUpCodeGrade")]
        public int? IdGrade { get; set; }
        public string Keterampilan { get; set; }
        //public bool IsDelete { get; set; }

        public virtual LookupCode LookUpCodeBagian { get; set; }
        public virtual LookupCode LookUpCodeGrade { get; set; }
    }
}