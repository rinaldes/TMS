using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class so_mstr
    {
        public so_mstr()
        {
            this.sod_det = new HashSet<sod_det>();
       }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string so_oid { get; set; }//,	random uid
        public string so_ref_po_code { get; set; }//,	random uid
        public int so_dom_id { get; set; }//,	1
		public int so_en_id { get; set; }//,	1
		public string so_add_by { get; set; }//,	current_user.username
		public System.DateTime so_add_date { get; set; }//,	DateTime.now
		public string so_upd_by { get; set; }//,	current_user.username
		public System.DateTime so_upd_date { get; set; }//,	DateTime.now
		public string so_code { get; set; }//,	SONumber
		public int so_ptnr_id_sold { get; set; }//,	CustomerId
		public int so_ptnr_id_bill { get; set; }//,	CustomerId
		public System.DateTime so_date { get; set; }//,	TanggalOrder
		public int so_credit_term { get; set; }//,	Customer Term of Payment
		public string so_taxable { get; set; }//,	Customer.IsPPn ? 'Y' : 'N' Checkbox
		public int so_tax_class { get; set; }//,	from code_mstr where code_field ~~* 'taxclass_mstr'
		public int so_si_id { get; set; }//,	1
		public string so_type { get; set; }//,	R
		public int so_sales_person { get; set; }//,	from ptnr_mstr where ptnr_is_member ~~* 'Y' 
		public int so_pi_id { get; set; }//,	from pi_mstr
		public string so_pay_type { get; set; }//,	from code_mstr where code_field ~~* 'payment_type'
		public int so_pay_method { get; set; }// ,	from code_mstr where code_field ~~* 'payment_methode'
		public int so_ar_ac_id { get; set; } //,	From pla_mstr.pla_ac_id partnumber --> pt_mstr.pt_pl_id =pla_mstr.pl_id)
		public string so_ar_sb_id { get; set; }//,	From pla_mstr.pla_sb_id partnumber --> pt_mstr.pt_pl_id =pla_mstr.pl_id)
		public string so_ar_cc_id { get; set; }//	From pla_mstr.pla_cc_id partnumber --> pt_mstr.pt_pl_id =pla_mstr.pl_id)
		public decimal so_dp { get; set; }//,	nominal yg didapat dr form InputDP
		public decimal so_disc_header { get; set; }//,	Total Diskon
		public decimal so_total { get; set; }//,	Total SO
		public int so_print_count { get; set; }//,	Print Count Default 0)
		public System.DateTime so_payment_date { get; set; }//,	Null
		public System.DateTime so_close_date { get; set; }//,	Null
		public int so_tran_id { get; set; }//,	0
		public string so_trans_id { get; set; }//,	D
		public string so_trans_rmks { get; set; }//,	SO Approval Remarks Null)
		public string so_current_route { get; set; }//,	Current City Route
		public string so_next_route { get; set; }//,	Next City Route
		public System.DateTime so_dt { get; set; }//,	DateTime.now
		public int so_cu_id { get; set; }//,	from cu_mstr.cu_id currency)
		public decimal so_total_ppn { get; set; }//,	Total PPN From Detail SO
		public decimal so_total_pph { get; set; }//,	Total PPH From Detail SO
		public decimal so_payment { get; set; }//,	0
		public decimal so_exc_rate { get; set; }//,	kurs mata uang From exr_rate current Date and Current Currency
		public string so_tax_inc { get; set; }//,	Y or N termasuk pajak atau belum, checkbox)
		public string so_cons { get; set; }//,	Y or N konsinyasi atau bukan, checkbox)
		public string so_terbilang { get; set; }//,	Terbilang dalam bahasa indonesia
		public int so_bk_id { get; set; }//,	from bk_mstr.bk_id Nama Bank)
		public int so_interval { get; set; }//,	from code_mstr where code_field ~~* 'payment_type'  kolom code_user_1
		public string so_ppn_type { get; set; }//,	E=PPN BEBAS, A = PPN BAYAR, N = NON
		public int so_branch_id { get; set; }//,	SO Branch from branch_mstr.branch_id)
		public int so_group_id { get; set; }//,	from code_mstr where code_field ~~* 'so_group'
		public int so_exc_rate_pi { get; set; }//,	0
		public int so_prct_limit_return { get; set; }//,	0
		public int so_disc_type { get; set; }//,	0
		public int? so_vehicle { get; set; }//,	VehicleNo
		public System.DateTime? so_departure_date { get; set; }//,	Tanggal Berangkat Kendaraan
		public string so_start_route { get; set; }//,	Kota Awal Berangkat from code_mstr where code_field ~~* 'route_mstr'
		public string so_destination_route { get; set; }//	kota Tujuan from code_mstr where code_field ~~* 'route_mstr'
        public string so_start { get; set; }
        public string so_end { get; set; }
        public string so_vehicle_type { get; set; }

        public virtual ICollection<sod_det> sod_det { get; set; }
   }
}