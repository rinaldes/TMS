using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Role
    {
        public int id { get; set; }
        [Display(Name = "Role")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string name { get; set; }
        public string StrMenu { get; set; }
        public List<RoleMenu> ListMenu { get; set; }
        public bool isselect { get; set; }
        public Role()
        {
            ListMenu = new List<RoleMenu>();
        }
        public Role(Context.Role dbitem)
        {
            id = dbitem.Id;
            name = dbitem.RoleName;

            ListMenu = new List<RoleMenu>();
            foreach (var item in dbitem.RoleMenus)
            {
                ListMenu.Add(new RoleMenu(item));
            }
        }
        public void setDb(Context.Role dbitem)
        {
            dbitem.RoleName = name;
            dbitem.RoleMenus.Clear();
            foreach (var item in ListMenu)
            {
                dbitem.RoleMenus.Add(item.setDb(new Context.RoleMenus()));
            }
        }
    }

    public class RoleMenu
    {
        public int Id { get; set; }
        public int? IdRole { get; set; }
        public int? IdMenu { get; set; }
        public string StrMenu { get; set; }
        public string StrMenuModul { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPrint { get; set; }
        public bool IsProses { get; set; }

        public RoleMenu() { }
        public RoleMenu(Context.Menu dbitem) {
            IdMenu = dbitem.Id;
            StrMenu = dbitem.MenuName;
            StrMenuModul = dbitem.Modul;
        }
        public RoleMenu(Context.RoleMenus dbitem)
        {
            IdMenu = dbitem.IdMenu;
            StrMenuModul = dbitem.Menu == null ? "" : dbitem.Menu.Modul;
            IsCreate = dbitem.IsCreate;
            IsRead = dbitem.IsRead;
            IsUpdate = dbitem.IsUpdate;
            IsDelete = dbitem.IsDelete;
            IsPrint = dbitem.IsPrint;
            IsProses = dbitem.IsProses;
        }
        public RoleMenu(Context.RoleMenus dbitem, int role_id)
        {
            IdMenu = dbitem.IdMenu;
            StrMenu = dbitem.Menu.MenuName;
            StrMenuModul = dbitem.Menu.Modul;
            IsCreate = dbitem.IsCreate;
            IsRead = dbitem.IsRead;
            IsUpdate = dbitem.IsUpdate;
            IsDelete = dbitem.IsDelete;
            IsPrint = dbitem.IsPrint;
            IsProses = dbitem.IsProses;
        }
        public Context.RoleMenus setDb(Context.RoleMenus dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdMenu = IdMenu.Value;
            dbitem.IsCreate = IsCreate;
            dbitem.IsRead = IsRead;
            dbitem.IsUpdate = IsUpdate;
            dbitem.IsDelete = IsDelete;
            dbitem.IsPrint = IsPrint;
            dbitem.IsProses = IsProses;
            
            return dbitem;
        }
    }
}