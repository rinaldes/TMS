using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RemovalTambahanRute
    {
        public RemovalTambahanRute()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Removal")]
        public int? IdRemoval { get; set; }
        [ForeignKey("DataBorongan")]
        public int? IdDataBorongan { get; set; }
        public Decimal? values { get; set; }

        public virtual Removal Removal { get; set; }
        public virtual DataBorongan DataBorongan { get; set; }
    }
}