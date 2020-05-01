using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ptnr_mstr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ptnr_id { get; set; }
        public int ptnr_dom_id { get; set; }
        public int ptnr_en_id { get; set; }
        public System.DateTime ptnr_add_date { get; set; }
        public System.DateTime ptnr_upd_date { get; set; }
        public string ptnr_add_by { get; set; }
        public string ptnr_upd_by { get; set; }
        public string ptnr_code { get; set; }
        public string ptnr_name { get; set; }
        public int ptnr_ptnrg_id { get; set; }
        public string ptnr_is_cust { get; set; }
        public string ptnr_is_vend { get; set; }
        public string ptnr_is_member { get; set; }
        public string ptnr_active { get; set; }
        public System.DateTime ptnr_dt { get; set; }
        public int ptnr_ac_ar_id { get; set; }
        public int ptnr_sb_ar_id { get; set; }
        public int ptnr_cc_ar_id { get; set; }
        public int ptnr_ac_ap_id { get; set; }
        public int ptnr_sb_ap_id { get; set; }
        public int ptnr_cc_ap_id { get; set; }
        public int ptnr_cu_id { get; set; }
        public int ptnr_limit_credit { get; set; }
        public int ptnr_institution_id { get; set; }
        public int ptnr_branch_id { get; set; }
        public int ptnr_type_id { get; set; }
        public int ptnr_credit_terms_id { get; set; }
        public int ptnr_sales_id { get; set; }
        public int ptnr_tax_class { get; set; }
        public char ptnr_tax_include { get; set; }
        public int ptnr_bk_id { get; set; }
        public string ptnr_oid { get; set; }
        public string ptnr_ppn_type { get; set; }
        public string ptnr_npwp { get; set; }
        public string ptnr_contact_tax { get; set; }
        public string ptnr_address_tax { get; set; }
        public string ptnr_remarks { get; set; }
        public string ptnr_taxable { get; set; }
    }
}