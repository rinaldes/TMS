using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Atm
    {
        public int Id { get; set; }
        [Display(Name = "No Kartu")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya angka dan -.")]
        public string NoKartu { get; set; }
        [Display(Name = "Nama Bank")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdBank { get; set; }
        public string NamaBank { get; set; }
        [Display(Name = "No Rekening")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya angka dan -.")]
        public string NoRekening { get; set; }
        [Display(Name = "Atas Nama")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string AtasNama { get; set; }
        [Display(Name = "Kode Driver")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdDriver { get; set; }
        public string KodeDriver { get; set; }
        [Display(Name = "Nama Driver")]
        public string NamaDriver { get; set; }
        [Display(Name = "Nama Panggilan")]
        public string Panggilan { get; set; }
        //public bool IsDelete { get; set; }
        
        public Atm()
        {

        }
        public Atm(Context.Atm dbitem)
        {
             Id = dbitem.Id;
             NoKartu = dbitem.NoKartu;
             IdBank = dbitem.IdBank;
             NamaBank = dbitem.LookupCodeBank.Nama;
             NoRekening = dbitem.NoRekening;
             AtasNama = dbitem.AtasNama;
             IdDriver = dbitem.IdDriver;
             KodeDriver = dbitem.Driver.KodeDriver;
             NamaDriver = dbitem.Driver.NamaDriver;
             Panggilan = dbitem.Driver.NamaPangilan;
        }
        public void setDb(Context.Atm dbitem)
        {
            dbitem.Id = Id;
            dbitem.NoKartu = NoKartu;
            dbitem.IdBank = IdBank;
            dbitem.NoRekening = NoRekening;
            dbitem.AtasNama = AtasNama;
            dbitem.IdDriver = IdDriver;
        }
    }
}