using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Kontak
    {
        public Kontak()
        {
        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nama { get; set; }
        [ForeignKey("LookUpCodeJabatan")]
        public int? IdJabatan { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("VendorGps")]
        public int IdVendor { get; set; }
        public string Hp { get; set; }
        public string Email { get; set; }

        public virtual LookupCode LookUpCodeJabatan { get; set; }
        public virtual VendorGps VendorGps { get; set; }
    }
}