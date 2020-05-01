using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataBoronganSPBU
    {
        public DataBoronganSPBU()
        {
            
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DataBorongan")]
        public int IdBorongan { get; set; }
        [ForeignKey("LookupCodeSpbu")]
        public int? IdLookupCodeSpbu { get; set; }
        public Decimal? value { get; set; }
        public virtual DataBorongan DataBorongan { get; set; }
        public virtual LookupCode LookupCodeSpbu { get; set; }
    }
}