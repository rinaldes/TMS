using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Dashboard
    {
        #region initial setting

        public int Id { get; set; }
        public string name { get; set; }
        public double y { get; set; }

        #endregion initial setting

        #region initial value

        public Dashboard(string penghibur1, double penghibur2, string penghibur3)
        {
            name = penghibur1;
            y = penghibur2;
        }
        
        #endregion initial value
    }
}