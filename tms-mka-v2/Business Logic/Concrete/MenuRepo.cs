using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class MenuRepo : IMenuRepo
    {
        private ContextModel context = new ContextModel();
        public List<Menu> FindAll()
        {
            return context.Menu.ToList();
        }
        public Menu FindByPK(int id)
        {
            return context.Menu.Where(d => d.Id == id).FirstOrDefault();
        }
    }
}