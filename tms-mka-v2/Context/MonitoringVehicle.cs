using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class MonitoringVehicle
    {
        public MonitoringVehicle()
        { }

        [Key]
        public string VehicleNo { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Gps { get; set; }
        public string Engine { get; set; }
        public double Kecepatan { get; set; }
        public string Ac { get; set; }
        public string Suhu { get; set; }
        public string Km { get; set; }
        public string Hm { get; set; }
        public string LatNew { get; set; }
        public string LongNew { get; set; }
        public string Zone { get; set; }
        public string Provinsi { get; set; }
        public string Kabupaten { get; set; }
        public string Alamat { get; set; }
        public string RangeAc { get; set; }
        public double AvgSpeed { get; set; }
        public double AvgSuhu { get; set; }


        public DateTime LastUpdate { get; set; }
    }
}