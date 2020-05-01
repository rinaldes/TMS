using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class MonitoringDetailSo
    {
        public MonitoringDetailSo()
        { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("MonitoringVehicle")]
        public string VehicleNo { get; set; }
        public string NoSo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerNama { get; set; }
        public int DriverId1 { get; set; }
        public string DriverNama1 { get; set; }
        public string DriverTlp1 { get; set; }
        public int? DriverId2 { get; set; }
        public string DriverNama2 { get; set; }
        public string DriverTlp2 { get; set; }
        public string JenisOrder { get; set; }
        public string RangeSuhu { get; set; }
        public string SuhuAvg { get; set; }
        public string Dari { get; set; }
        public string Tujuan { get; set; }
        public DateTime TglMuat { get; set; }
        public DateTime TglBerangkat { get; set; }
        public DateTime TglTiba { get; set; }
        public DateTime TglBongkar { get; set; }
        public string EstimasiTiba { get; set; }
        public string Delay { get; set; }
        public string Deviasi { get; set; }
        public string Muat { get; set; }
        public string Perjalanan { get; set; }
        public string Bongkar { get; set; }
        public string Precooling { get; set; }
        public string AcMati { get; set; }
        public string SuhuSesuai { get; set; }
        public string TargetWaktu { get; set; }
        public DateTime TargetTiba { get; set; }
        public int TotalMoving { get; set; }
        public int TotalStop { get; set; }
        public int MaxStop { get; set; }
        public string MaxStopPosition { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime AcOn { get; set; }
        public int AcOff { get; set; }
        public DateTime MaxOff { get; set; }
        public string MaxOffPosition { get; set; }
        public DateTime AcOffTime { get; set; }
        public DateTime LastEdit { get; set; }

        public virtual MonitoringVehicle MonitoringVehicle { get; set; }

    }
}