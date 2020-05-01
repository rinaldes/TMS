using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class MultiDrop
    {
        public int Id { get; set; }
        [Display(Name = "Tujuan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string tujuan { get; set; }
        [Display(Name = "Jumlah Kota")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? JumlahKota { get; set; }
        [Display(Name = "Tambahan waktu tempuh")]
        public int? WaktuTempuh { get; set; }
        [Display(Name = "Tambahan waktu kerja")]
        public int? WaktuKerja { get; set; }

        public MultiDrop()
        {

        }
        public MultiDrop(Context.Multidrop dbitem)
        {
            Id = dbitem.Id;
            tujuan = dbitem.tujuan;
            JumlahKota = dbitem.JumlahKota;
            WaktuTempuh = dbitem.WaktuTempuh;
            WaktuKerja = dbitem.WaktuKerja;
        }
        public void setDb(Context.Multidrop dbitem)
        {
            dbitem.Id = Id;
            dbitem.tujuan = tujuan;
            dbitem.JumlahKota = JumlahKota.Value;
            dbitem.WaktuTempuh = WaktuTempuh;
            dbitem.WaktuKerja = WaktuKerja;
        }
    }
}