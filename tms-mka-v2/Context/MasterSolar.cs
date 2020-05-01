using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class MasterSolar
    {
        public MasterSolar()
        {}

        [Key]
        public int Id { get; set; }
        //public bool IsDelete { get; set; }
        public decimal Harga { get; set; }
        public DateTime Start{ get; set; }
        public DateTime End { get; set; }
        public int Selisih { get; set; }
    }
}