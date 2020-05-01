using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class glt_det
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string glt_oid { get; set; }
		public int glt_dom_id { get; set; }
		public int glt_en_id { get; set; }
        public int glt_ptnr_id { get; set; }
        public int glt_det_ptnr_id { get; set; }
        public string glt_add_by { get; set; }
		public DateTime glt_add_date { get; set; }
		public string glt_upd_by { get; set; }
		public DateTime glt_upd_date { get; set; }
		public string glt_gl_oid { get; set; }
		public string glt_code { get; set; }
		public DateTime glt_date { get; set; }
		public string glt_type { get; set; }
		public int glt_cu_id { get; set; }
		public decimal glt_exc_rate { get; set; }
		public int glt_seq { get; set; }
		public int glt_ac_id { get; set; }
		public int glt_sb_id { get; set; }
		public int glt_cc_id { get; set; }
		public string glt_desc { get; set; }
		public decimal? glt_debit { get; set; }
		public decimal? glt_credit { get; set; }
		public string glt_ref_tran_id { get; set; }
		public string glt_ref_trans_code { get; set; }
		public string glt_posted { get; set; }
		public System.DateTime glt_dt { get; set; }
		public string glt_daybook { get; set; }
		public string glt_ref_oid { get; set; }
		public string glt_is_reverse { get; set; }
		public string glt_is_gen_ros { get; set; }
		public string glt_desc_detail { get; set; }
		public string glt_ref_detail_no { get; set; }
		public int glt_branch_id { get; set; }
		public int glt_driver_id { get; set; }
		public string glt_no_pol { get; set; }
   }
}