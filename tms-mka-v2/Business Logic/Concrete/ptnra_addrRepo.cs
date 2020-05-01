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
    public class ptnra_addrRepo : Iptnra_addrRepo
    {
        private ContextModelERP context = new ContextModelERP();
        public void save(CustomerAddress dbitem)
        {
//          Context.ptnra_addr ptnra_addr = context.ptnra_addr.Where(d => d.ptnra_oid == "0008c887-4458-4861-a36d-c79f75ff"+dbitem.Id.ToString().PadLeft(4, '0')).FirstOrDefault();
  //        if ())
            ptnra_addr model = new ptnra_addr();
            model.ptnra_oid = Guid.NewGuid().ToString();
            model.ptnra_dom_id = 1;
            model.ptnra_en_id = 1;
            model.ptnra_add_by = "";
            model.ptnra_add_date = DateTime.Now;
            model.ptnra_line_1 = dbitem.Alamat;
            if (dbitem.LocKabKota != null)
            model.ptnra_line_2 = dbitem.LocKabKota.Nama;
            model.ptnra_phone_1 = dbitem.Telp;
            model.ptnra_fax_1 = dbitem.Fax;
            model.ptnra_id = dbitem.Id;
            model.ptnra_ptnr_oid = context.ptnr_mstr.Where(d => d.ptnr_id == dbitem.CustomerId).FirstOrDefault() == null ? null : context.ptnr_mstr.Where(d => d.ptnr_id == dbitem.CustomerId).FirstOrDefault().ptnr_oid;
            model.ptnra_addr_type = 992;
            model.ptnra_active = "Y";
            model.ptnra_dt = DateTime.Now;
            context.ptnra_addr.Add(model);
            context.SaveChanges();
        }
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
          if ((ptnra_addr.ptnr_upd_date < data_supplier.sent_to_erp) rescue true)
            data_supplier.update_attributes(sent_to_erp: Time.now())
          end*/

        public void updateCustomerAddress(ptnra_addr dbitem){
            context.ptnra_addr.Attach(dbitem);
            var entry = context.Entry(dbitem);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }

        public ptnra_addr FindByPK(int id)
        {
            return context.ptnra_addr.Where(d => d.ptnra_id == id).FirstOrDefault();
        }

        public int FindLastPtnrId()
        {
            return context.ptnra_addr.OrderBy<ptnra_addr>("ptnra_id DESC").FirstOrDefault().ptnra_id;
        }
    }
}