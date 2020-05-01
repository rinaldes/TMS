using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class bk_mstr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bk_id { get; set; }
        public int bk_ac_id { get; set; }
        public string bk_code { get; set; }
        public string bk_name { get; set; }
        public string bk_active { get; set; }
   }
}