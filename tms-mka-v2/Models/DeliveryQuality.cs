using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DeliveryQuality
    {
        #region initial setting

        public string Category { get; set; }
        public string OnTimeMuat { get; set; }
        public string OnTimePerjalanan { get; set; }
        public string OnTemp { get; set; }

        #endregion initial setting

        #region initial value

        public DeliveryQuality(string category, string muat, string jalan, string ontemp)
        {
            Category = category;
            OnTimeMuat = muat;
            OnTimePerjalanan = jalan;
            OnTemp = ontemp;
        }
        
        #endregion initial value
    }
}