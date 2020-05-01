using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class MonitoringDetailParkingAlert
    {
        public MonitoringDetailParkingAlert()
        { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("MonitoringVehicle")]
        public string VehicleNo { get; set; }
        public int DriverId1 { get; set; }
        public string DriverNama1 { get; set; }
        public string DriverTlp1 { get; set; }
        public int DriverId2 { get; set; }
        public string DriverNama2 { get; set; }
        public string DriverTlp2 { get; set; }
        public DateTime StartParking { get; set; }
        public DateTime EndParking { get; set; }
        public string Durasi { get; set; }
        public string Provinsi { get; set; }
        public string Kabupaten { get; set; }
        public string Alamat { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual MonitoringVehicle MonitoringVehicle { get; set; }
    }
}