using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerLoadingAddress
    {
        public CustomerLoadingAddress()
        {

        }

        [Key]
        [Column(Order=0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public string Alamat { get; set; }
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
        public string Zona { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public int urutan { get; set; }
        public virtual Location LocProvinsi { get; set; }
        public virtual Location LocKabKota { get; set; }
        public virtual Location LocKecamatan { get; set; }
        public virtual Location LocKelurahan { get; set; }
        public virtual Customer Customer { get; set; }        
    }
}