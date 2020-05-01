using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataBorongan
    {
        public DataBorongan()
        {
            this.DataBoronganRute = new HashSet<DataBoronganRute>();
            this.DataBoronganSPBU = new HashSet<DataBoronganSPBU>();
            this.DataBoronganKapal = new HashSet<DataBoronganKapal>();
            this.DataBoronganTf = new HashSet<DataBoronganTf>();
            this.DataBoronganHistory = new HashSet<DataBoronganHistory>();
        }

        [Key]
        public int Id { get; set; }
        public bool IsTambahan { get; set; }
        [ForeignKey("MasterPool")]
        public int? IdMasterPool { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? IdJenisTruck { get; set; }
        public string NamaBorongan { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
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
        public decimal Pembulatan { get; set; }
        public Decimal? AlokasiCash { get; set; }
        public Decimal? TotalAlokasiPembulatan { get; set; }
        //public bool IsDelete { get; set; }
        public virtual MasterPool MasterPool { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<DataBoronganRute> DataBoronganRute { get; set; }
        public virtual ICollection<DataBoronganSPBU> DataBoronganSPBU { get; set; }
        public virtual ICollection<DataBoronganKapal> DataBoronganKapal { get; set; }
        public virtual ICollection<DataBoronganTf> DataBoronganTf { get; set; }
        public virtual ICollection<DataBoronganHistory> DataBoronganHistory { get; set; }
    }
}