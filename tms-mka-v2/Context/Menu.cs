using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        public string Modul { get; set; }
        public string MenuName { get; set; }
    }
}