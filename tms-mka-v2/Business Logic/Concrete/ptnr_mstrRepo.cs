using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Linq;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class ptnr_mstrRepo : Iptnr_mstrRepo
    {
        private ContextModelERP context = new ContextModelERP();
        public void save(Customer dbitem)
        {
            ptnr_mstr model = new ptnr_mstr();
            model.ptnr_oid = Guid.NewGuid().ToString();
            model.ptnr_dom_id =  1;
            model.ptnr_en_id =  1;
            model.ptnr_add_date = DateTime.Now;
            model.ptnr_upd_date = DateTime.Now;
            model.ptnr_id = dbitem.Id;
            model.ptnr_code = dbitem.CustomerCodeOld;
            model.ptnr_name = dbitem.CustomerNama;
            model.ptnr_ptnrg_id = 10174;
            model.ptnr_is_cust = "Y";
            model.ptnr_is_vend = dbitem.IsVendor ? "Y" : "N";
            model.ptnr_active = "Y";
            model.ptnr_dt = DateTime.Now;
            model.ptnr_remarks = dbitem.CustomerCode;
            model.ptnr_ac_ar_id = 0;
            model.ptnr_sb_ar_id = 0;
            model.ptnr_cc_ar_id = 0;
            model.ptnr_ac_ap_id = 0;
            model.ptnr_sb_ap_id = 0;
            model.ptnr_cc_ap_id = 0;
            model.ptnr_cu_id = 1;
            model.ptnr_limit_credit = 0;
            model.ptnr_institution_id = 1000100;
            model.ptnr_branch_id = 10001;
            model.ptnr_type_id = 991440;
            model.ptnr_credit_terms_id = 991025;
            model.ptnr_sales_id = 0;
            model.ptnr_tax_class = 0;
            model.ptnr_tax_include = 'N';
            model.ptnr_taxable = "Y";
            model.ptnr_bk_id = 0;
            model.ptnr_ppn_type = dbitem.CustomerPPN != null && dbitem.CustomerPPN.FirstOrDefault() != null && dbitem.CustomerPPN.FirstOrDefault().PPN == true ? "Y" : "N";
            //rekening customer ?
            if (dbitem.CustomerPPN.Count() > 0){
                model.ptnr_npwp = dbitem.CustomerPPN.FirstOrDefault().NomorNPWP;
                model.ptnr_contact_tax = dbitem.CustomerPPN.FirstOrDefault().NamaNPWP;
                model.ptnr_address_tax = dbitem.CustomerPPN.FirstOrDefault().AddressNPWP;
            }
            context.ptnr_mstr.Add(model);
            context.SaveChanges();
        }

        public void updateCustomer(ptnr_mstr dbitem){
            context.ptnr_mstr.Attach(dbitem);
            var entry = context.Entry(dbitem);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }

        public void saveDriver(Driver dbitem)
        {
            try{
            ptnr_mstr model = new ptnr_mstr();
            model.ptnr_oid = Guid.NewGuid().ToString();
            model.ptnr_dom_id =  1;
            model.ptnr_en_id =  1;
            model.ptnr_add_date = DateTime.Now;
            model.ptnr_id = dbitem.Id+7000000;
            model.ptnr_code = dbitem.KodeDriver;
            model.ptnr_name = dbitem.NamaDriver + " - " + dbitem.KodeDriver;
            model.ptnr_ptnrg_id = 10172;
            model.ptnr_dt = DateTime.Now;
            model.ptnr_is_cust = "N";
            model.ptnr_is_vend = "N";
            model.ptnr_active = "Y";
            model.ptnr_ac_ar_id = 0;
            model.ptnr_sb_ar_id = 0;
            model.ptnr_cc_ar_id = 0;
            model.ptnr_ac_ap_id = 0;
            model.ptnr_sb_ap_id = 0;
            model.ptnr_cc_ap_id = 0;
            model.ptnr_cu_id = 1;
            model.ptnr_limit_credit = 0;
            model.ptnr_institution_id = 1000100;
            model.ptnr_branch_id = 10001;
            model.ptnr_type_id = 991440;
            model.ptnr_credit_terms_id = 991025;
            model.ptnr_sales_id = 0;
            model.ptnr_tax_class = 0;
            model.ptnr_tax_include = 'N';
            model.ptnr_is_member = "Y";
            model.ptnr_bk_id = 0;
            context.ptnr_mstr.Add(model);
            context.SaveChanges();
            }
            catch (Exception)
            {
            }

//          #== address ==#
/*          if PtnraAddr.find_by_ptnra_line_1_and_ptnra_ptnr_oid(data_supplier.address, ptnr_mstr.ptnr_oid).nil?
            ptnra_id = PtnraAddr.order(:ptnra_id).last.ptnra_id
            if ptnra_id.to_s.slice(0) == '2'
              ptnra_id = (("#{('%07d')}" % ((PtnraAddr.order(:ptnra_id).last.ptnra_id)+1)).to_i)
            else
              ptnra_id = (("#{('2%07d')}" % ((PtnraAddr.order(:ptnra_id).last.ptnra_id)+1)).to_i)
            end
            ptnra_addr = PtnraAddr.new(ptnra_oid: SecureRandom.uuid, ptnra_dom_id: 1, ptnra_en_id: 1, ptnra_add_by: "", ptnra_add_date: DateTime.now, ptnra_id: ptnra_id, ptnra_line_1: data_supplier.address, ptnra_line_2: data_supplier.city, ptnra_phone_1: data_supplier.phone, ptnra_fax_1: data_supplier.fax, ptnra_ptnr_oid: ptnr_mstr.ptnr_oid, ptnra_addr_type: 993, ptnra_active: 'Y', ptnra_dt: DateTime.now)
            ptnra_addr.save
          else
            ptnra_addr = PtnraAddr.find_by_ptnra_line_1_and_ptnra_ptnr_oid(data_supplier.address, ptnr_mstr.ptnr_oid)
            if ((ptnr_mstr.ptnra_upd_date < data_supplier.sent_to_erp) rescue true)
              ptnra_addr.update_attributes(ptnra_upd_by: "", ptnra_upd_date: DateTime.now, ptnra_line_1: data_supplier.address, ptnra_line_2: data_supplier.city, ptnra_phone_1: data_supplier.phone, ptnra_fax_1: data_supplier.fax)
            end
          end*/
/*  //        #== contact ==#
          data_supplier.contact_people.each do |su_cp|
            if PtnracCntc.find_by_ptnrac_contact_name_and_addrc_ptnra_oid((ContactPerson.find(su_cp.id.to_i).name rescue ''), ptnra_addr.ptnra_oid).nil?
              ptnrac_cntc = PtnracCntc.new(ptnrac_oid: SecureRandom.uuid, addrc_ptnra_oid: ptnra_addr.ptnra_oid, ptnrac_add_by: "", ptnrac_add_date: DateTime.now, ptnrac_seq: 1, ptnrac_function: 9945, ptnrac_contact_name: su_cp.name, ptnrac_phone_1: su_cp.phone, ptnrac_email: su_cp.email, ptnrac_dt: DateTime.now)
              ptnrac_cntc.save
            else
              update_cp = ContactPerson.find(su_cp.id.to_i).name
              ptnrac_cntc = PtnracCntc.find_by_ptnrac_contact_name_and_addrc_ptnra_oid(update_cp, ptnra_addr.ptnra_oid)
              if ((ptnrac_mstr.ptnr_upd_date < data_supplier.sent_to_erp) rescue true)
                ptnrac_cntc.update_attributes(ptnrac_upd_by: "", ptnrac_upd_date: DateTime.now, ptnrac_contact_name: su_cp.name, ptnrac_phone_1: su_cp.phone, ptnrac_email: su_cp.email)
              end
            end
          end
          supplier_count += 1
          if ((ptnr_mstr.ptnr_upd_date < data_supplier.sent_to_erp) rescue true)
            data_supplier.update_attributes(sent_to_erp: Time.now())
          end*/
        }

        public ptnr_mstr FindByPK(int id)
        {
            return context.ptnr_mstr.Where(d => d.ptnr_id == id).FirstOrDefault();
        }

        public ptnr_mstr FindByCode(string code)
        {
            return context.ptnr_mstr.Where(d => d.ptnr_code == code).FirstOrDefault();
        }

        public int FindLastPtnrId()
        {
            return context.ptnr_mstr.OrderBy<ptnr_mstr>("ptnr_id DESC").FirstOrDefault().ptnr_id;
        }
    }
}