using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IMenuRepo
    {
        List<Menu> FindAll();
        Menu FindByPK(int id);
    }
}