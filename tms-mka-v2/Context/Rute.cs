using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Rute
    {
        public Rute()
        {
            this.RuteCheckPoint = new HashSet<RuteCheckPoint>();
        }

        [Key]
        public int Id { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        [ForeignKey("LocationAsal")]
        public int? IdAsal { get; set; }
        [ForeignKey("AreaAsal")]
        public int? IdAreaAsal { get; set; }
        [ForeignKey("LocationTujuan")]
        public int? IdTujuan { get; set; }
        [ForeignKey("AreaTujuan")]
        public int? IdAreaTujuan { get; set; }
        [ForeignKey("Multidrop")]
        public int? IdMultiDrop { get; set; }
        public Decimal Jarak { get; set; }
        public int WaktuKerja { get; set; }
        //public string WaktuTempuh { get;set; }
        public int? WaktuTempuhJam { get; set; }
        public int? WaktuTempuhMenit { get; set; }
        public int? Toleransi { get; set; }
        public bool IsAreaPulang { get; set; }
        public bool IsKotaKota { get; set; }
        public string Keterangan { get; set; }
        //public bool IsDelete { get; set; }
        public int Urutan { get; set; }
        
        public virtual Location LocationAsal { get; set; }
        public virtual MasterArea AreaAsal { get; set; }
        public virtual Location LocationTujuan { get; set; }
        public virtual MasterArea AreaTujuan { get; set; }
        public virtual Multidrop Multidrop { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<RuteCheckPoint> RuteCheckPoint { get; set; }
    }
}