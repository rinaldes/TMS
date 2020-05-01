using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataGPSHistory
    {
        public DataGPSHistory()
        {
            
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DataGPS")]
        public int IdDataGPS { get; set; }
        public string NoGPS { get; set; }
        public string Vehicle { get; set; }
        public string strVendor { get; set; }
        public string ModelGps { get; set; }
        public string NoDevice { get; set; }
        public bool? SensorSuhu { get; set; }
        public bool? SensorPintu { get; set; }
        public int? Tahun { get; set; }
        public DateTime? TanggalPasang { get; set; }
        public DateTime? TanggalGaransi { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Username { get; set; }
        public virtual DataGPS DataGPS { get; set; }
    }
}