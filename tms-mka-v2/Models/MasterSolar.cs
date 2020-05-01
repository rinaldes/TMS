using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class MasterSolar
    {
        public int Id { get; set; }
        [Display(Name = "Harga")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public decimal Harga { get; set; }
        [Display(Name = "Start")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime Start { get; set; }
        [Display(Name = "End")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime End { get; set; }
        [Display(Name = "Selisih")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int Selisih { get; set; }
        public MasterSolar()
        {
        }
        public MasterSolar(Context.MasterSolar dbitem)
        {
            Id = dbitem.Id;
            Harga = dbitem.Harga;
            Start = dbitem.Start;
            End = dbitem.End;
            Selisih = dbitem.Selisih;
        }
        public void setDb(Context.MasterSolar dbitem)
        {
            dbitem.Id = Id;
            dbitem.Harga = Harga;
            dbitem.Start = Start;
            dbitem.End = End;
            dbitem.Selisih = Selisih;
        }
    }
}