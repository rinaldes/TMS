using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ILookupCodeCategoriesRepo
    {
        void save(LookupCodeCategories dbitem);
        List<LookupCodeCategories> FindAll();
        LookupCodeCategories FindByPK(int id);
        void delete(LookupCodeCategories dbitem);
    }
}