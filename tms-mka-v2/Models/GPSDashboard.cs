using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class GPSDashboard
    {
        #region initial setting

        public string Waktu { get; set; }
        public string Trekq { get; set; }
        public string Intellitrac { get; set; }
        public string Solofleet { get; set; }

        #endregion initial setting

        #region initial value

        public GPSDashboard(string waktu, List<MonitoringVehicle> trekq, List<MonitoringVehicle> intellitrac, List<MonitoringVehicle> solofleet)
        {
            Waktu = waktu;
            Trekq = trekq.Count() + " unit (10%)";
            Intellitrac = intellitrac.Count() + " unit (10%)";
            Solofleet = solofleet.Count() + " unit (10%)";
        }
        
        #endregion initial value
    }
}