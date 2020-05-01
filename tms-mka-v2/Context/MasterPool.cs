using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class MasterPool
    {
        public MasterPool()
        {
            this.ListZoneParkir = new HashSet<ZoneParkir>();
        }
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        //public bool IsDelete { get; set; }
        public string NamePool { get; set; }
        public int? Capacity{ get; set; }
        public string Address { get; set; }
        [ForeignKey("LocProvinsi")]
        public int? IdProvinsi { get; set; }
        [ForeignKey("LocKabKota")]
        
        public int? IdKabKota { get; set; }
        [ForeignKey("LocKecamatan")]
        
        public int? IdKec { get; set; }
        [ForeignKey("LocKelurahan")]
        public int? IdKel { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? Radius { get; set; }
        public string KodeTelp { get; set; }
        public string Telp { get; set; }
        public string Pic { get; set; }
        public string Handphone { get; set; }
        public string IpAddress { get; set; }
        public int? IdCreditCash { get; set; }
        
        public virtual Location LocProvinsi { get; set; }
        public virtual Location LocKabKota { get; set; }
        public virtual Location LocKecamatan { get; set; }
        public virtual Location LocKelurahan { get; set; }
        public virtual ICollection<ZoneParkir> ListZoneParkir { get; set; }
    }
}