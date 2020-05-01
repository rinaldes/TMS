using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class User
    {
        public User()
        {
            this.UserRole = new HashSet<UserRole>();
            this.UserReference = new HashSet<UserReference>();
            this.UserMenus = new HashSet<UserMenus>();
        }

        [Key]
        public int Id { get; set; }
        public string Nik { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fristname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string path_foto { get; set; }

        public virtual ICollection<UserMenus> UserMenus { get; set; } 
        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<UserReference> UserReference { get; set; }
    }
}