using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class JenisTruck
    {
        public int Id { get; set; }
        [Display(Name = "Jenis Truk")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string StrJenisTruck { get; set; }
        [Display(Name = "Golongan Tol")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? GolTol { get; set; }
        [Display(Name = "Alias")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Alias { get; set; }
        [Display(Name = "Biaya Solar Inap")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public decimal? Biaya { get; set; }
        [Display(Name = "AC Mati Interval")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? AcInterval { get; set; }
        //public bool isDeleted { get; set; }
        public JenisTruck()
        {

        }
        public JenisTruck(Context.JenisTrucks dbitem)
        {
            Id = dbitem.Id;
            StrJenisTruck = dbitem.StrJenisTruck;
            GolTol = dbitem.GolTol;
            Alias = dbitem.Alias;
            Biaya = dbitem.Biaya;
            AcInterval = dbitem.AcInterval;
        }
        public void setDb(Context.JenisTrucks dbitem)
        {
            dbitem.Id = Id;
            dbitem.StrJenisTruck = StrJenisTruck;
            dbitem.GolTol = GolTol.Value;
            dbitem.Alias = Alias;
            dbitem.Biaya = Biaya.Value;
            dbitem.AcInterval = AcInterval.Value;
        }
    }
}