using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class FaktorBorongan
    {
        public FaktorBorongan()
        {
            this.FaktorBoronganHistory = new HashSet<FaktorBoronganHistory>();    
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("MasterPool")]
        public int? IdMasterPool { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? IdJenisTruck { get; set; }
        public Decimal RasioDlmKota { get; set; }
        public Decimal RasioDlmKota2 { get; set; }
        public Decimal RasioJawaBali { get; set; }
        public Decimal RasioSumatra { get; set; }
        public Decimal RasioKosong { get; set; }
        //public Decimal RasioSolar { get; set; }
        public Decimal UangMakanJawaBali { get; set;}
        public Decimal UangMakanSumatra { get; set; }
        public Decimal FaktorPengaliGaji { get; set; }
        public Decimal FaktorPengaliTips { get; set; }
        public Decimal PotonganDriver1 { get; set; }
        public Decimal PotonganDriver2 { get; set; }
        public Decimal BiayaKapalBali { get; set; }
        public Decimal BiayaKapalBaliNTB { get; set; }
        public Decimal BiayaKapalSumatra { get; set; }
        public Decimal BiayaKapalKalimantan { get; set; }
        public Decimal BiayaKapalSulawesi { get; set; }
        //public bool IsDelete { get; set; }
        public virtual MasterPool MasterPool { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual ICollection<FaktorBoronganHistory> FaktorBoronganHistory { get; set; }
    }
}