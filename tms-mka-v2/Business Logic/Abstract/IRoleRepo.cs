using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IRoleRepo
    {
        void save(Role dbitem);
        List<Role> FindAll();
        Role FindByPK(int id);
        void delete(Role dbitem);
        bool IsExist(string nama, int id = 0);
    }
}