using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ERPDynamicConfig
    {
        public ERPDynamicConfig()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public int? ac_id { get; set; }
        public int? lookup_code_id { get; set; }
        public int? pool_id { get; set; }
    }
}