using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Mekanik
    {
        public int Id { get; set; }
        [Display(Name = "Nama Mekanik")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanummeric.")]
        public string NamaMekanik { get; set; }
        [Display(Name = "Bagian")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdBagian { get; set; }
        public string NamaBagian { get; set; }
        [Display(Name = "Grade")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdGrade { get; set; }
        public string NamaGrade { get; set; }
        [Display(Name = "Keterampilan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanummeric.")]
        public string Keterampilan { get; set; }
        //public bool IsDelete { get; set; }
        
        public Mekanik()
        {

        }
        public Mekanik(Context.Mekanik dbitem)
        {
             Id = dbitem.Id;
             NamaMekanik = dbitem.NamaMekanik;
             IdBagian = dbitem.IdBagian;
             NamaBagian = dbitem.LookUpCodeBagian.Nama;
             IdGrade = dbitem.IdGrade;
             NamaGrade = dbitem.IdGrade != null ? dbitem.LookUpCodeGrade.Nama : "";
             Keterampilan = dbitem.Keterampilan;
        }
        public void setDb(Context.Mekanik dbitem)
        {
            dbitem.Id = Id;
            dbitem.NamaMekanik = NamaMekanik;
            dbitem.IdBagian = IdBagian;
            dbitem.IdGrade = IdGrade;
            dbitem.Keterampilan = Keterampilan;
        }
    }
}