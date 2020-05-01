using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class LookupCode
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("LookupCodeCategories")]
        public int IdKategori { get; set; }
        [Required]
        public string Nama { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public string Deskripsi { get; set; }
        public int ac_id { get; set; }
        public string ac_code { get; set; }
        public string ac_name { get; set; }
        public int? VendorId { get; set; }
        public bool DipakaidiAUJ { get; set; }
        public virtual LookupCodeCategories LookupCodeCategories { get; set; }
    }
}