using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class pbyd_det
    {
        [Key]
        public string pbyd_oid { get; set; }
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("pby_mstr")]
        public string pbyd_pby_oid { get; set; }
        public System.DateTime? pbyd_dt { get; set; }
        public decimal pbyd_amount_pay { get; set; }
        public int pbyd_seq { get; set; }
        public int pbyd_pjc_id { get; set; }
        public string pbyd_desc { get; set; }
        public int pbyd_cc_id { get; set; }
        public decimal pbyd_amount { get; set; }
        public int pbyd_ac_id { get; set; }
        public string pbyd_tms_type { get; set; }
        public string pbyd_remarks { get; set; }

        public virtual pby_mstr pby_mstr { get; set; }
    }
}