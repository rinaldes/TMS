using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class UserRoles
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? IdUser { get; set; }
        [ForeignKey("Role")]
        public int? IdRole { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}