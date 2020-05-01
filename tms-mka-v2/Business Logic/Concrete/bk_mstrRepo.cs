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
//using tms_mka_v2.DataserErp;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class bk_mstrRepo : Ibk_mstrRepo
    {
        private ContextModelERP context = new ContextModelERP();

        public List<Models.bk_mstr> FindAll()
        {
            List<Models.bk_mstr> listModel = new List<Models.bk_mstr>();
            List<Context.bk_mstr> dt = context.bk_mstr.ToList();
            
            foreach (var item in dt.Where(d => d.bk_active == "Y"))
            {
                listModel.Add(new Models.bk_mstr() { 
                    id = item.bk_id,
                    bk_ac_id = item.bk_ac_id,
                    bk_code = item.bk_code,
                    bk_name = item.bk_name
                });
            }

            return listModel;
        }
        public Models.bk_mstr FindByPk(int? id)
        {
            Models.bk_mstr model = new Models.bk_mstr();
            if (id.HasValue)
            {
                var data = context.bk_mstr.Where(d => d.bk_id == id).FirstOrDefault();
                if (data != null)
                {
                    model.id = data.bk_id;
                    model.bk_ac_id = data.bk_ac_id;
                    model.bk_code = data.bk_code;
                    model.bk_name = data.bk_name;
                }
            }

            return model;
        }
    }
}