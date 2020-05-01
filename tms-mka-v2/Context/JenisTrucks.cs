using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class JenisTrucks
    {
        [Key]
        public int Id { get; set; }
        public string StrJenisTruck { get; set; }
        public int GolTol { get; set; }
        public string Alias { get; set; }
        public decimal Biaya { get; set; }
        public int AcInterval { get; set; }
    }
}