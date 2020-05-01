using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class gr_mstr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string gr_add_by { get; set; }
        public System.DateTime gr_add_date { get; set; }
        public string gr_code { get; set; }
        public System.DateTime gr_date { get; set; }
        public int gr_dom_id { get; set; }
        public int gr_en_id { get; set; }
        public string gr_oid { get; set; }
        public int gr_branch_id { get; set; }
        public int gr_ptnr_id { get; set; }
        public string gr_ptnr_code { get; set; }
        public int? gr_tax_amount { get; set; }
        public int? gr_tax_basis_amount { get; set; }
        public int? gr_total { get; set; }
    }
}