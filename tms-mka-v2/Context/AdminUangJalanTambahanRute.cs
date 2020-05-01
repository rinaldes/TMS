using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class AdminUangJalanTambahanRute
    {
        public AdminUangJalanTambahanRute()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("AdminUangJalan")]
        public int? IdAdminUangJalan { get; set; }
        [ForeignKey("DataBorongan")]
        public int? IdDataBorongan { get; set; }
        public Decimal? values { get; set; }
        
        public virtual AdminUangJalan AdminUangJalan { get; set; }
        public virtual DataBorongan DataBorongan { get; set; }
    }
}