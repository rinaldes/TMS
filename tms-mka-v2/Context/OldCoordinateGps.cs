using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class OldCoordinateGps
    {
        public OldCoordinateGps()
        { }

        [Key]
        public string VehicleNoOld { get; set; }
        public string LatOld { get; set; }
        public string LongOld { get; set; }
        public string KecepatanOld { get; set; }
        public string SuhuOld { get; set; }
        public string MesinOld { get; set; }
        public string AcOld { get; set; }
        public string ProvinsiOld { get; set; }
        public string KabupatenOld { get; set; }
        public string AlamatOld { get; set; }
        public DateTime CreatedDateOld { get; set; }
    }
}