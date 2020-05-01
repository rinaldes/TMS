using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class code_mstr
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string code_code { get; set; }
        public string code_name { get; set; }
		public string code_oid { get; set; }// uuid NOT NULL, -- Oid
		public int code_dom_id { get; set; }// smallint, -- DomainId
		public int code_en_id { get; set; }// integer, -- EntityId
		public string code_add_by { get; set; }// character varying15), -- CreatedBy
		public System.DateTime code_add_date { get; set; }// timestamp without time zone, -- CreatedDate
		public string code_upd_by { get; set; }// character varying15), -- ModifiedBy
		public System.DateTime? code_upd_date { get; set; }// timestamp without time zone, -- LastModified
		public int code_id { get; set; }// integer, -- Id
		public int code_seq { get; set; }// integer, -- Sequence
		public string code_field { get; set; }// character varying45), -- Field
		public string code_desc { get; set; }// character varying255), -- Description
		public string code_default { get; set; }//  character1) DEFAULT 'N'::bpchar, -- IsDefault
		public string code_parent { get; set; }//  integer, -- ParentId
		public string code_usr_1 { get; set; }//  character varying45), -- UserField_1
		public string code_usr_2 { get; set; }//  character varying45), -- UserField_2
		public string code_usr_3 { get; set; }//  character varying45), -- UserField_3
		public string code_usr_4 { get; set; }//  character varying45), -- UserField_4
		public string code_usr_5 { get; set; }//  character varying45), -- UserField_5
		public string code_active { get; set; }//  character1) DEFAULT 'N'::bpchar, -- IsActive
		public DateTime code_dt { get; set; }//  timestamp without time zone, -- RowVersion
   }
}