using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Role
    {
        public Role()
        {
            this.UserRole = new HashSet<UserRole>();
            this.RoleMenus = new HashSet<RoleMenus>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<RoleMenus> RoleMenus { get; set; }
    }
}