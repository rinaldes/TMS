using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class HistoryGps
    {
        public HistoryGps()
        {}

        [Key]
        public int Id { get; set; }
        public int MonitoringDetailSoId { get; set; }
        public string VehicleNo { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public double Speed { get; set; }
        public float Temp { get; set; }
        public string Mesin { get; set; }
        public string Ac { get; set; }
        public string Provinsi { get; set; }
        public string Kabupaten { get; set; }
        public string Alamat { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Geofence { get; set; }
        public string Provider { get; set; }
    }
}