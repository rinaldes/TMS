using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class code_mstr
    {
        #region variable
        public int id { get; set; }
        public string code_code { get; set; }
        public string code_name { get; set; }
        #endregion
        
    }
}