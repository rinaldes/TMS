using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class UserReference
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Coloumn { get; set; }
        public string HideShow { get; set; }

        public virtual User User { get; set; }
    }
}