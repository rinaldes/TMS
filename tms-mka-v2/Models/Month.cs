using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Month
    {
        #region initial setting

        public string MonthName { get; set; }
        public List<int> Values { get; set; }

        #endregion initial setting

        #region initial value

        public Month(string monthName, List<int> valuess)
        {
            MonthName = monthName;
            Values = valuess;
        }
        
        #endregion initial value
    }
}