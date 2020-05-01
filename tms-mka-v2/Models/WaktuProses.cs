using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class WaktuProses
    {
        #region initial setting

        public string Waktu { get; set; }
        public int OrderCount { get; set; }

        #endregion initial setting

        #region initial value

        public WaktuProses(string waktuProses, int jumlahOrder)
        {
            Waktu = waktuProses;
            OrderCount = jumlahOrder;
        }
        
        #endregion initial value
    }
}