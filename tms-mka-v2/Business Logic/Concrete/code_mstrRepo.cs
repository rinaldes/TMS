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
    public class code_mstrRepo : Icode_mstrRepo
    {
        private ContextModelERP context = new ContextModelERP();

        public Models.code_mstr FindByCodeName(string vehicleNo)
        {
            Models.code_mstr model = new Models.code_mstr();
            var data = context.code_mstr.Where(d => d.code_name == vehicleNo).FirstOrDefault();
            if (data != null)
            {
                model.id = data.code_id;
                model.code_code = data.code_code;
                model.code_name = data.code_name;
                return model;
            }
            return null;
        }

        public Context.code_mstr FindByCodeCode(string vehicleNo)
        {
            return context.code_mstr.Where(d => d.code_name == vehicleNo).FirstOrDefault();
        }

        public void updateVehicle(code_mstr dbitem)
        {
            context.code_mstr.Attach(dbitem);
            var entry = context.Entry(dbitem);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }

        public void saveVehicle(DataTruck dbitem)
        {
            code_mstr model = new code_mstr();
            model.code_oid = Guid.NewGuid().ToString();
            model.code_code = dbitem.VehicleNo;// { get; set; }
            model.code_name = dbitem.VehicleNo;// { get; set; }
            model.code_dom_id = 1; //{ get; set; }// smallint, -- DomainId
            model.code_en_id = 1; //{ get; set; }// integer, -- EntityId
            model.code_add_date = DateTime.Now;// { get; set; }// timestamp without time zone, -- CreatedDate
            model.code_upd_date = DateTime.Now;// { get; set; }// timestamp without time zone, -- LastModified
            model.code_id = 900000+dbitem.Id;//{ get; set; }// integer, -- Id
            model.code_seq = 900000+dbitem.Id;// { get; set; }// integer, -- Sequence
            model.code_field  = "vehicle";//{ get; set; }// character varying45), -- Field
            model.code_desc = dbitem.JenisTrucks.StrJenisTruck;// { get; set; }// character varying255), -- Description
            model.code_active = "Y";// { get; set; }//  character1) DEFAULT 'N'::bpchar, -- IsActive
            model.code_dt = DateTime.Now;//{ get; set; }//  timestamp without time zone, -- RowVersion
            context.code_mstr.Add(model);
            context.SaveChanges();
        }
    }
}