using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Auditrail
    {
        public Auditrail()
        {}
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string Actionnya { get; set; }
        [ForeignKey("User")]
        public int? IdUser { get; set; }
        public string RemoteAddress { get; set; }
        public string Modulenya { get; set; }
        public string QueryDetail { get; set; }
        //public bool IsDelete { get; set; }
        public virtual User User { get; set; }
    }
}