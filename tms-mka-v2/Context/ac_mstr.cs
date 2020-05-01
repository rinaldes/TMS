using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ac_mstr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ac_id { get; set; }
        public string ac_code { get; set; }
        public string ac_name { get; set; }
        public string ac_active { get; set; }
        public string ac_is_sumlevel { get; set; }
    }
}