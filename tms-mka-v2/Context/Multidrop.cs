using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Multidrop
    {
        [Key]
        public int Id { get; set; }
        public string tujuan { get; set; }
        public int JumlahKota { get; set; }
        public int? WaktuTempuh { get; set; }
        public int? WaktuKerja { get; set; }
        //public bool isDeleted { get; set; }
    }
}