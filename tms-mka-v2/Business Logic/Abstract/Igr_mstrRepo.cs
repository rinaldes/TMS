using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface Igr_mstrRepo
    {
        void save(int? total, Customer customer, string username, string code);
        void saveKapal(int? total, Customer customer, string username, string code);
        Context.gr_mstr FindByCode(string code);
        List<Context.gr_mstr> FindAllByCode(string code);
        void delete(gr_mstr dbitem);
    }
}