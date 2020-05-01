using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DriverAlert
    {
        public string Keterangan { get; set; }
        public string Value { get; set; }
    }
}