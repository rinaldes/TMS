using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface Iptnr_mstrRepo
    {
        ptnr_mstr FindByPK(int id);
        void save(Customer dbitem);
        void updateCustomer(ptnr_mstr dbitem);
        void saveDriver(Driver dbitem);
        ptnr_mstr FindByCode(string code);
    }
}