using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface Iac_mstrRepo
    {
        List<Models.ac_mstr> FindAll();
        Models.ac_mstr FindByPk(int? id);
    }
}