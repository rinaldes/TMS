using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using Newtonsoft.Json;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using tms_mka_v2.Security;
using tms_mka_v2.Infrastructure;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace tms_mka_v2.Controllers
{
    public class MenuController : BaseController
    {
        private IMenuRepo RepoMenu;
        private IRoleRepo RepoRole;
        private IUserRepo RepoUser;
        public MenuController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IMenuRepo repoMenu, IRoleRepo repoRole, IUserRepo repoUser)
            : base(repoBase, repoLookup)
        {
            RepoMenu = repoMenu;
            RepoRole = repoRole;
            RepoUser = repoUser;
        }
        public string Binding(string modul, int role_id)
        {
            Context.Role role = RepoRole.FindByPK(role_id);
            List<Context.Menu> items = RepoMenu.FindAll().Where(d => d.Modul == modul && !role.RoleMenus.Select(f => f.IdMenu).Contains(d.Id)).ToList();

            List<RoleMenu> ListModel = new List<RoleMenu>();
            foreach (Context.Menu item in items)
            {
                ListModel.Add(new RoleMenu(item));
            }

            foreach (Context.RoleMenus item in role.RoleMenus.Where(d => RepoMenu.FindAll().Where(f => f.Modul == modul).Select(e => e.Id).ToList().Contains(d.IdMenu.Value)))
            {
                ListModel.Add(new RoleMenu(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
        public string BindingByUser(string modul, int user_id)
        {
            Context.User usr = RepoUser.FindByPK(user_id);
            List<UserMenu> ListModel = new List<UserMenu>();
            foreach (var item in usr.UserMenus.Where(m => m.Menu.Modul == modul))
	        {
		        ListModel.Add(new UserMenu(item));
	        }

            List<Context.Menu> items = RepoMenu.FindAll().Where(d => d.Modul == modul && !usr.UserMenus.Select(f => f.IdMenu).Contains(d.Id)).ToList();
            foreach (Context.Menu item in items)
            {
                ListModel.Add(new UserMenu(item));
            }
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
    }
}