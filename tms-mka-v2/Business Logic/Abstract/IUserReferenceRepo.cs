using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IUserReferenceRepo
    {
        void save(UserReference dbitem, int id);
        UserReference find(int iduser, string action, string controller, string kolom);
        List<UserReference> FindByUser(int iduser);
        List<UserReference> FindAll();
    }
}