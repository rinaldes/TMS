using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class sod_det
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string sod_oid { get; set; } // uuid NOT NULL,	Random uid
		public int sod_dom_id { get; set; } //  smallint,	1
		public int sod_en_id { get; set; } //  integer,	1
		public string sod_add_by { get; set; } //  character varying25),	current_user.name
		public System.DateTime sod_add_date { get; set; } //  timestamp without time zone,	DateTime.Now
		public string sod_upd_by { get; set; } //  character varying25),	current_user.name
		public System.DateTime sod_upd_date { get; set; } //  timestamp without time zone,	DateTime.Now
        [ForeignKey("so_mstr")]
		public string sod_so_oid { get; set; } //  uuid,	so_mstr.so_uid
		public int sod_seq { get; set; } //  smallint,	urutan sod_det dlm 1 so_mstr
		public string sod_is_additional_charge { get; set; } //  character1),	0
		public int sod_si_id { get; set; } //  integer DEFAULT 1,	
		public int sod_pt_id { get; set; } // ,	9999999
		public string sod_rmks { get; set; } //  character varying100),	remak biasa we
		public int sod_qty { get; set; } //  numeric18,8),	banyaknya truk yg disewa
		public int sod_qty_allocated { get; set; } //  numeric18,8) DEFAULT 0,	0
		public int sod_qty_picked { get; set; } //  numeric18,8),	0
		public int sod_qty_shipment { get; set; } //  numeric18,8),	  sod_qty numeric18,8),
		public int sod_qty_pending_inv { get; set; } //  numeric18,8),	0
		public int sod_qty_invoice { get; set; } //  numeric18,8),	  sod_qty numeric18,8),
		public int sod_um { get; set; } //  integer,	0
		public int sod_cost { get; set; } //  numeric26,8),	0
		public decimal sod_price { get; set; } //  numeric26,8),	kl dh ada harganya harga masuk k sini
		public int sod_disc { get; set; } //  numeric11,8),	0
		public int sod_sales_ac_id { get; set; } //  integer,	0
		public int sod_sales_sb_id { get; set; } //  integer,	0
		public int sod_sales_cc_id { get; set; } //  integer,	0
		public int sod_disc_ac_id { get; set; } //  integer,	0
		public int sod_um_conv { get; set; } //  numeric18,8),	1
		public int sod_qty_real { get; set; } //  numeric18,8),	  sod_qty numeric18,8),
		public string sod_taxable { get; set; } //  character1),	Customer.PPN Yes ? 'Y' : 'N'
		public string sod_tax_inc { get; set; } //  character1),	N
		public int sod_tax_class { get; set; } //  integer,	Customer.PPN Yes ? '991404' : '1034'
		public System.DateTime sod_dt { get; set; } //  without time zone,	biasa we
		public int sod_payment { get; set; } //  numeric26,8), -- ini untuk angsuran/bulan	0
		public int sod_dp { get; set; } //  numeric26,8),	0
		public int sod_sales_unit { get; set; } //  numeric26,8),	0
		public int sod_loc_id { get; set; } //  integer,	0
        public string sod_serial { get; set; } //  character varying100),	0
		public int sod_qty_return { get; set; } //  numeric26,8),	0
		public string sod_ppn_type { get; set; } //  character1),	A
		public decimal sod_ppn { get; set; } //  numeric26,8) DEFAULT 0,	0
		public int sod_pph { get; set; } //  numeric26,8) DEFAULT 0,	0
        public decimal sod_price_line { get; set; } //  numeric26,8),	  sod_qty numeric18,8),
		public int sod_disc1 { get; set; } //  numeric11,8) DEFAULT 0,	0
		public int sod_disc2 { get; set; } //  numeric11,8) DEFAULT 0,	0
		public int sod_packaging_id { get; set; } //  integer NOT NULL DEFAULT 0,	0
		public int sod_qty_packaging { get; set; } //  numeric26,8) DEFAULT 0,	0
		public int sod_qty_add { get; set; } //  numeric26,8) DEFAULT 0,	0
        public string sod_nopol { get; set; }

        public virtual so_mstr so_mstr { get; set; }
   }
}