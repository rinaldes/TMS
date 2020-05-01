using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class RepairMaintenance
    {
        #region initial setting

        public string State { get; set; }
        public int Truk { get; set; }
        public int Pendingin { get; set; }
        public int Box { get; set; }
        public int Ban { get; set; }
        public int GPS { get; set; }

        #endregion initial setting

        #region initial value

        public RepairMaintenance(string state, int truk, int pendingin, int box, int ban, int gps)
        {
            State = state;
            Truk = truk;
            Pendingin = pendingin;
            Box = box;
            Ban = ban;
            GPS = gps;
        }
        
        #endregion initial value
    }
}