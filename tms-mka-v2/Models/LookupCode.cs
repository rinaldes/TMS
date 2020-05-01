using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class LookupCode
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Kategori")]
        public int? IdKategori { get; set; }
        public string Kategori { get; set; }
        [Required]
        [Display(Name = "Nama")]
        public string Nama { get; set; }
        [Required]
        [Display(Name = "Order")]
        public int? Order { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        [Display(Name = "Deskripsi")]
        public string Deskripsi { get; set; }
        public int ac_id { get; set; }
        public int? VendorId { get; set; }
        public bool DipakaidiAUJ { get; set; }
        public string ac_code { get; set; }
        public string ac_name { get; set; }
        public List<LookupCodeCategories> listKategori { get; set; }
        public LookupCode()
        {

        }
        public LookupCode(Context.LookupCode dbitem)
        {
            Id = dbitem.Id;
            IdKategori = dbitem.IdKategori;
            DipakaidiAUJ = dbitem.DipakaidiAUJ;
            Kategori = dbitem.LookupCodeCategories.Category;
            Nama = dbitem.Nama;
            Order = dbitem.Order;
            Deskripsi = dbitem.Deskripsi;
            VendorId = dbitem.VendorId;
        }
        public void setDb(Context.LookupCode dbitem)
        {
            dbitem.IdKategori = IdKategori.Value;
            dbitem.Nama = Nama;
            dbitem.Order = Order.Value;
            dbitem.Deskripsi = Deskripsi;
            dbitem.VendorId = VendorId;
            dbitem.DipakaidiAUJ = bool.Parse(DipakaidiAUJ.ToString());;
        }
    }
}