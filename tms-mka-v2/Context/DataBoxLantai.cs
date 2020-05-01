using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataBoxLantai
    {
        public DataBoxLantai()
        {
            
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DataBox")]
        public int IdDataBox { get; set; }
        [ForeignKey("LookupCodeLantaiCode")]
        public int? IdLantaiCode { get; set; }

        public virtual DataBox DataBox { get; set; }
        public virtual LookupCode LookupCodeLantaiCode { get; set; }
    }
}