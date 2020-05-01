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
    public class ac_mstrRepo : Iac_mstrRepo
    {
        private ContextModelERP context = new ContextModelERP();

        public List<Models.ac_mstr> FindAll()
        {
            List<Models.ac_mstr> listModel = new List<Models.ac_mstr>();
            List<Context.ac_mstr> dt = context.ac_mstr.ToList();
             
            foreach (var item in dt.Where(d => d.ac_active == "Y" && d.ac_is_sumlevel != "Y"))
            {
                listModel.Add(new Models.ac_mstr() { 
                    id = item.ac_id,
                    ac_code = item.ac_code,
                    ac_name = item.ac_name
                });
            }

            return listModel;
        }
        public Models.ac_mstr FindByPk(int? id)
        {
            Models.ac_mstr model = new Models.ac_mstr();
            if (id.HasValue)
            {
                var data = context.ac_mstr.Where(d => d.ac_id == id).FirstOrDefault();
                if (data != null)
                {
                    model.id = data.ac_id;
                    model.ac_code = data.ac_code;
                    model.ac_name = data.ac_name;
                }
            }

            return model;
        }
    }
}