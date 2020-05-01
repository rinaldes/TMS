using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        [RegularExpression(@"^[a-zA-Z0-9.''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Code { get; set; }
        [Display(Name = "Type")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Type { get; set; }
        [Display(Name = "Nama")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Nama { get; set; }
        [Display(Name = "Parent")]
        public int? ParentId { get; set; }
        public string strPArent { get; set; }
        //public bool isDeleted { get; set; }

        public Location()
        {

        }
        public Location(Context.Location dbitem)
        {
            Id = dbitem.Id;
            Code = dbitem.Code;
            Type = dbitem.Type;
            Nama = dbitem.Nama;
            ParentId = dbitem.ParentId;
            strPArent = dbitem.ParentId == null ? "" : dbitem.LocationParent.Nama;
        }
        public void setDb(Context.Location dbitem)
        {
            dbitem.Id = Id;
            dbitem.Code = Code;
            dbitem.Type = Type;
            dbitem.Nama = Nama;
            dbitem.ParentId = ParentId;
        }
    }
}