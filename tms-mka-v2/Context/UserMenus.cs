using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class UserMenus
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? IdUser { get; set; }
        [ForeignKey("Menu")]
        public int? IdMenu { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPrint { get; set; }
        public bool IsProses { get; set; }

        public virtual User User { get; set; }
        public virtual Menu Menu { get; set; }
    }
}