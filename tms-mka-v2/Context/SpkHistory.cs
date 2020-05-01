using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SpkHistory
    {
        [Key]
        public int Id { get; set; }
        public string Jenis { get; set; }
        public string Status { get; set; }
        public DateTime Estimasi { get; set; }
        public TimeSpan JamEstimasi { get; set; }
        public DateTime Tanggal { get; set; }
        public int RevEstimasi { get; set; }
        [ForeignKey("Workshop")]
        public int? WorkshopId { get; set; }
        public virtual Workshop Workshop { get; set; }
    }
}