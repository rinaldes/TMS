using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Nama { get; set; }
        [ForeignKey("LocationParent")]
        public int? ParentId { get; set; }
        public virtual Location LocationParent { get; set; }
    }
}