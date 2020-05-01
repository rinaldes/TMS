using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "NIK")]
        public string Nik { get; set; }
        [Display(Name = "Username")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Username { get; set; }
        [Display(Name = "Nama Depan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Fristname { get; set; }
        [Display(Name = "Nama Belakang")]
        public string Lastname { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        public string Password { get; set; }
        public List<Role> ListRole { get; set; }
        public List<UserMenu> ListMenu { get; set; }
        public string StrMenu { get; set; }
        public string path_foto { get; set; }
        public User()
        {

        }
        public User(Context.User dbitem)
        {
            Id = dbitem.Id;
            Nik = dbitem.Nik;
            Username = dbitem.Username;
            Fristname = dbitem.Fristname;
            Lastname = dbitem.Lastname;
            Email = dbitem.Email;
            Phone = dbitem.Phone;
            Password = dbitem.Password;
            path_foto = dbitem.path_foto;
            //ListRole = new List<Role>();
            //foreach (var item in dbitem.UserRole)
            //{
            //    ListRole.Add(new Role(item.Role));
            //}
            
            ListMenu = new List<UserMenu>();
            foreach (var item in dbitem.UserMenus)
            {
                ListMenu.Add(new UserMenu(item));
            }
        }
        public void setDb(Context.User dbitem)
        {
            dbitem.Id = Id;
            dbitem.Nik = Nik;
            dbitem.Username = Username;
            dbitem.Fristname = Fristname;
            dbitem.Lastname = Lastname;
            dbitem.Email = Email;
            dbitem.Phone = Phone;
            dbitem.path_foto = path_foto;
            if (Password != null && Password != "")
            {
                using (MD5 md5Hash = MD5.Create())
                {
//                dbitem.Password = AppHelper.GetMd5Hash(md5Hash, Password);
                }
            }

            //foreach (Role item in ListRole.Where(d => d.isselect == true))
            //{
            //    dbitem.UserRole.Add(new UserRole() { IdRole = item.id });
            //}
        }
    }

    public class UserShow
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public UserShow()
        {
        }
    }

    public class UserMenu
    {
        public int Id { get; set; }
        public int? IdMenu { get; set; }
        public string StrMenu { get; set; }
        public string StrMenuModul { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPrint { get; set; }
        public bool IsProses { get; set; }

        public UserMenu() { }
        public UserMenu(Context.Menu dbitem)
        {
            IdMenu = dbitem.Id;
            StrMenu = dbitem.MenuName;
            StrMenuModul = dbitem.Modul;
        }
        public UserMenu(Context.UserMenus dbitem)
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
        public UserMenu(Context.RoleMenus dbitem, int role_id)
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
        public Context.UserMenus setDb(Context.UserMenus dbitem)
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