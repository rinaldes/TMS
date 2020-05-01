using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Spk
    {
        [Key]
        public int Id { get; set; }
        public string Jenis { get; set; }
        public string Permintaan { get; set; }
        public string KeteranganSPK { get; set; }
        public string Keterangan { get; set; }
        [ForeignKey("Mekanik")]
        public int? Mekanik1 { get; set; }
        [ForeignKey("Mekanikk")]
        public int? Mekanik2 { get; set; }
        [ForeignKey("Workshop")]
        public int? Workshop_id { get; set; }
        public int? Urutan { get; set; }
        public string NoSpk { get; set; }
        public string Status { get; set; }
        public DateTime? ServiceIn { get; set; }
        public DateTime? Estimasi { get; set; }
        public DateTime? ServiceOut { get; set; }
        public DateTime? TglSPK { get; set; }
        public int? RevEstimasi { get; set; }
        public TimeSpan JamServiceIn { get; set; }
        public TimeSpan JamEstimasi { get; set; }
        public TimeSpan JamServiceOut { get; set; }
        public virtual Workshop Workshop { get; set; }
        public virtual Mekanik Mekanik { get; set; }
        public virtual Mekanik Mekanikk { get; set; }
    }
}