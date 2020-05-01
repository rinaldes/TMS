using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class PenetapanDriver
    {
        public PenetapanDriver()
        {
            this.PenetapanDriverHistory = new HashSet<PenetapanDriverHistory>();    
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        [ForeignKey("Driver1")]
        public int? IdDriver1 { get; set; }
        public string NoHp1Driver1 { get; set; }
        public string NoHp2Driver1 { get; set; }
        [ForeignKey("Driver2")]
        public int? IdDriver2 { get; set; }
        public string NoHp1Driver2 { get; set; }
        public string NoHp2Driver2 { get; set; }
        public string DitetapkanOleh1 { get; set; }
        public string DitetapkanOleh2 { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual ICollection<PenetapanDriverHistory> PenetapanDriverHistory { get; set; }
    }
}