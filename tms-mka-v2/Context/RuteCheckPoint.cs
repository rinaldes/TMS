using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RuteCheckPoint
    {
        public RuteCheckPoint()
        {

        }

        [Key]
        [Column(Order=0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Rute")]
        public int IdRute { get; set; }
        public string code { get; set; }
        public string longitude { get; set; }
        public string langitude { get; set; }
        public int radius { get; set; }
        public string alamat { get; set; }
        public int? waktuJam { get; set; }
        public int? waktuMenit { get; set; }
        public int toleransi { get; set; }
        public string hapus { get; set; }
        //public int urutan { get; set; }
        public virtual Rute Rute { get; set; }
    }
}