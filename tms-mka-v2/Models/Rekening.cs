using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Rekening
    {
        public int Id { get; set; }
        [Display(Name = "Nama Rekening")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Input hanya huruf.")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaRekening { get; set; }
        public string NamaNoRekening { get; set; }
        [Display(Name = "Nomor Rekening")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression("[0-9]+", ErrorMessage = "Input hanya angka.")]
        public string NoRekening { get; set; }
        [Display(Name = "BANK")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdBank { get; set; }
        public string StrBank { get; set; }
        [Display(Name = "Type")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Type_ { get; set; }
        [Display(Name = "Set As Default")]
        public bool SetAsDefault { get; set; }
        //public bool isDeleted { get; set; }

        public Rekening()
        {

        }
        public Rekening(Context.Rekenings dbitem)
        {
            Id = dbitem.Id;
            NamaRekening = dbitem.NamaRekening;
            NoRekening = dbitem.NoRekening;
            IdBank = dbitem.IdBank;
            StrBank = dbitem.LookupCodeBank.Nama;
            Type_ = dbitem.Type;
            SetAsDefault = dbitem.SetAsDefault;
            NamaNoRekening = dbitem.NamaRekening + " - " + dbitem.NoRekening + " - " + dbitem.LookupCodeBank.Nama;
        }
        public void setDb(Context.Rekenings dbitem)
        {
            dbitem.Id = Id;
            dbitem.NamaRekening = NamaRekening;
            dbitem.NoRekening = NoRekening;
            dbitem.IdBank = IdBank.Value;
            dbitem.Type = Type_;
            dbitem.SetAsDefault = SetAsDefault;
        }
    }
}