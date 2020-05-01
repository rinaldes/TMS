using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataGPS
    {
        public DataGPS()
        {
            this.DataGPSHistory = new HashSet<DataGPSHistory>();    
        }

        [Key]
        public int Id { get; set; }
        public string NoGPS { get; set; }
        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        [ForeignKey("VendorGps")]
        public int? IdVendor { get; set; }
        public string ModelGps { get; set; }
        public string NoDevice { get; set; }
        public bool? SensorSuhu { get; set; }
        public bool? SensorPintu { get; set; }
        public int? Tahun { get; set; }
        public DateTime? TanggalPasang { get; set;}
        public DateTime? TanggalGaransi { get; set;}
        //public bool IsDelete { get; set; }
        public int urutan { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual VendorGps VendorGps { get; set; }
        public virtual ICollection<DataGPSHistory> DataGPSHistory { get; set; }
    }
}