using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class NewCoordinateGps
    {
        public NewCoordinateGps()
        { }

        [Key]
        public string VehicleNoNew { get; set; }
        public string LatNew { get; set; }
        public string LongNew { get; set; }
        public string KecepatanNew { get; set; }
        public string SuhuNew { get; set; }
        public string MesinNew { get; set; }
        public string AcNew { get; set; }
        public string ProvinsiNew { get; set; }
        public string KabupatenNew { get; set; }
        public string AlamatNew { get; set; }
        public DateTime CreatedDateNew { get; set; }
    }
}