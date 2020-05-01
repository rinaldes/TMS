using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "Nama")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaProduk { get; set; }
        [Display(Name = "Kategori")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKategori { get; set; }
        public string Kategori { get; set; }
        [Display(Name = "Target Suhu")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public decimal? TargetSuhu { get; set; }
        [Display(Name = "Suhu Maksimal")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public decimal? MaxTemps { get; set; }
        [Display(Name = "Suhu Minimal")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public decimal? MinTemps { get; set; }
        [Display(Name = "Interval Alert")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Treshold { get; set; }
        [Display(Name = "Keterangan")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Remarks { get; set; }
        //public bool isDeleted { get; set; }
        public Product()
        {

        }
        public Product(Context.MasterProduct dbitem)
        {
            Id = dbitem.Id;
            NamaProduk = dbitem.NamaProduk;
            IdKategori = dbitem.IdKategori;
            Kategori = dbitem.LookupCode.Nama;
            TargetSuhu = dbitem.TargetSuhu;
            MaxTemps = dbitem.MaxTemps;
            MinTemps = dbitem.MinTemps;
            Treshold = dbitem.Treshold;
            Remarks = dbitem.Remarks;
        }
        public void setDb(Context.MasterProduct dbitem)
        {
            dbitem.Id = Id;
            dbitem.NamaProduk = NamaProduk;
            dbitem.IdKategori = IdKategori.Value;
            dbitem.TargetSuhu = TargetSuhu.Value;
            dbitem.MaxTemps = MaxTemps.Value;
            dbitem.MinTemps = MinTemps.Value;
            dbitem.Treshold = Treshold.Value;
            dbitem.Remarks = Remarks;
        }
    }
}