using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataBoronganHistory
    {
        public DataBoronganHistory()
        {

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DataBorongan")]
        public int IdDataBorongan { get; set; }
        public DateTime? Tanggal { get; set; }
        public bool IsTambahan { get; set; }
        public string NamaBorongan { get; set; }
        public string Customer { get; set; }
        public decimal? Jarak { get; set; }
        public string Rasio { get; set; }
        public Decimal? LiterSolar { get; set; }
        public Decimal? HargaSolar { get; set; }
        public Decimal? WaktuHariKerja { get; set; }
        public Decimal? JumlahMakan { get; set; }
        public string AreaUangMakan { get; set; }
        public Decimal? UangMakan { get; set; }
        public Decimal? BiayaTol { get; set; }
        public int? BobotTipsParkir { get; set; }
        public Decimal? TipsParkir { get; set; }
        public Decimal? BobotGaji1 { get; set; }
        public Decimal? gaji1 { get; set; }
        public Decimal? BobotGaji2 { get; set; }
        public Decimal? gaji2 { get; set; }
        public Decimal? TotalGaji { get; set; }
        public string Kapal { get; set; }
        public Decimal? BiayaKapal { get; set; }
        public Decimal? BoronganDasar { get; set; }
        public Decimal? Kawalan { get; set; }
        public Decimal? Timbangan { get; set; }
        public Decimal? Karantina { get; set; }
        public Decimal? SPSI { get; set; }
        public Decimal? MultiDrop { get; set; }
        public Decimal? TotalBorongan { get; set; }
        public Decimal? Pembulatan { get; set; }
        //public bool IsDelete { get; set; }
        public virtual DataBorongan DataBorongan { get; set; }
    }
}