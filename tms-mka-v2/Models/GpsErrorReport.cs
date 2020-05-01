using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class GpsErrorReport
    {
        public GpsErrorReport()
        {
        }

        public int Id { get; set; }
        public bool GpsOnOff { get; set; }
        public string KetGpsOnOff { get; set; }
        public bool LocationError { get; set; }
        public string KetLocationError { get; set; }
        public bool TemperatureSensorError { get; set; }
        public string KetTemperatureSensorError { get; set; }
        public bool TidakAdaGrafik { get; set; }
        public string KetTidakAdaGrafik { get; set; }
        public bool GrafikRata { get; set; }
        public string KetGrafikRata { get; set; }
        public bool GrafikPatah { get; set; }
        public string KetGrafikPatah { get; set; }
        public bool PerbedaanSuhu { get; set; }
        public string KetPerbedaanSuhu { get; set; }
        public bool EngineOnOff { get; set; }
        public string KetEngineOnOff { get; set; }
        public bool AcOnOff { get; set; }
        public string KetAcOnOff { get; set; }
    }
}