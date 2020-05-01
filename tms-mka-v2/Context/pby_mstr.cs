using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class pby_mstr
    {
        [Key]
        public string pby_oid { get; set; }
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string pby_code { get; set; }
        public int pby_driver { get; set; }
        public int pby_dom_id { get; set; }
        public int pby_en_id { get; set; }
        public string pby_add_by { get; set; }
        public DateTime pby_add_date { get; set; }
        public string pby_upd_by { get; set; }
        public System.DateTime? pby_upd_date { get; set; }
        public DateTime pby_date { get; set; }
        public string pby_type { get; set; }
        public int pby_cc_id { get; set; }
        public string pby_remarks { get; set; }
        public int pby_tran_id { get; set; }
        public string pby_trans_id { get; set; }
        public DateTime pby_dt { get; set; }
        public int pby_cu_id { get; set; }
        public int pby_exc_rate { get; set; }
        public int pby_peruntukan_id { get; set; }
        public DateTime pby_due_date { get; set; }
        public int pby_xemp_id { get; set; }
        public int pby_branch_id { get; set; }
        public int pby_ac_id { get; set; }
   }
}