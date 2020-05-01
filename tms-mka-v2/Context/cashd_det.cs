using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class cashd_det
    {
        [Key]
        public string cashd_oid { get; set; }
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("pbyd_det")]
        public string cashd_pbyd_oid { get; set; }
		public System.DateTime? cashd_dt { get; set; }
		public decimal cashd_amount { get; set; }
        public decimal? cashd_refund_amount { get; set; }

        public virtual pbyd_det pbyd_det { get; set; }
   }
}