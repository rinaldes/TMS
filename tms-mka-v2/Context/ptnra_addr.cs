using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ptnra_addr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ptnra_oid { get; set; }
        public int ptnra_dom_id { get; set; }
        public int ptnra_en_id { get; set; }
        public string ptnra_add_by { get; set; }
        public DateTime ptnra_add_date { get; set; }
        public int ptnra_id { get; set; }
        public string ptnra_line_1 { get; set; }
        public string ptnra_line_2 { get; set; }
        public string ptnra_phone_1 { get; set; }
        public string ptnra_fax_1 { get; set; }
        public string ptnra_ptnr_oid { get; set; }
        public int ptnra_addr_type { get; set; }
        public string ptnra_active { get; set; }
        public DateTime ptnra_dt { get; set; }
   }
}