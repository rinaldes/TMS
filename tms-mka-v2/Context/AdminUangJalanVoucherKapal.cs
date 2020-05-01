using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class AdminUangJalanVoucherKapal
    {
        public AdminUangJalanVoucherKapal()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("AdminUangJalan")]
        public int? IdAdminUangJalan { get; set; }
        public string Keterangan { get; set; }
        public int? Value { get; set; }

        public virtual AdminUangJalan AdminUangJalan { get; set; }
    }
}