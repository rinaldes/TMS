using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class TimeAlert
    {
        public TimeAlert()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string idProses { get; set; }
        [Required]
        public string keteranganBagian { get; set; }
        public bool status { get; set; }
        [Required]
        public int over { get; set; }
        [Required]
        public string overSatuan { get; set; }
        [Required]
        public string idUserAlert { get; set; }
        public bool AlertPopup { get; set; }
        public bool AlertSound { get; set; }
        public bool AlertEmail { get; set; }
        [Required]
        public string rowColor { get; set; }
        //public bool IsDelete { get; set; }
    }
}