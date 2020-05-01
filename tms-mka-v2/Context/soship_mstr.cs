using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class soship_mstr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string soship_oid { get; set; }// uuid NOT NULL,	
		public int soship_dom_id { get; set; }// smallint,	1
		public int soship_en_id { get; set; }// integer NOT NULL,	1
		public string soship_add_by { get; set; }// character varying25),	
		public System.DateTime? soship_add_date { get; set; }// timestamp without time zone,	
		public string soship_upd_by { get; set; }// character varying25),	
		public System.DateTime? soship_upd_date { get; set; }// timestamp without time zone,	
		public string soship_code { get; set; }// character varying25),	samain dgn no SO
		public System.DateTime? soship_date { get; set; }// date,	tgl settlement
		public string soship_so_oid { get; set; }// uuid NOT NULL,	so_mstr
		public int soship_si_id { get; set; }// integer NOT NULL DEFAULT 1,	1
		public string soship_is_shipment { get; set; }// character1),	Y
		public System.DateTime? soship_dt { get; set; }// timestamp without time zone,	jam input
		public int soship_exc_rate { get; set; }// numeric26,8),	1
		public int soship_cu_id { get; set; }// integer NOT NULL DEFAULT 1,	1
		public int soship_branch_id { get; set; }// integer NOT NULL,	10001
		public int soship_expeditur { get; set; }// integer NOT NULL DEFAULT 0,	0
   }
}