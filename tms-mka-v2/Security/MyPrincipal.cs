using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using tms_mka_v2.Context;

namespace tms_mka_v2.Security
{
    public class MyPrincipal : IPrincipal
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string path_foto { get; set; }
        public List<string> RoleUser { get; set; }
        public List<PrincipalMenu> menus { get; set; }

        public IIdentity Identity { get; private set; }

        public bool HasMenuAccess(string menu)
        {
            return menus.Any(m => m.MenuName == menu && m.Action.Count > 0);
        }

        public bool HasActionAccess(string menu, string action)
        {
            return menus.Any(m => m.MenuName == menu && m.Action.Contains(action));
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public bool IsSuperadmin()
        {
            if (RoleUser.Contains("Superadmin"))
                return true;
            else
                return false;
        }

        public MyPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }
    }

    public class MyPrincipalSerializeModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string path_foto { get; set; }

        public MyPrincipalSerializeModel()
        {

        }

        public MyPrincipalSerializeModel(Context.User dbitem)
        {
            id = dbitem.Id;
            username = dbitem.Username;
            password = dbitem.Password;
            firstname = dbitem.Fristname;
            lastname = dbitem.Lastname;
            path_foto = dbitem.path_foto;
        }        
    }

    public class PrincipalMenu
    {
        public string MenuName { get; set; }
        public List<string> Action { get; set; }
    }
}