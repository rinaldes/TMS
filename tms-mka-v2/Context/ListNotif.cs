using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ListNotif
    {
        public ListNotif()
        {

        }

        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string EmailMsg { get; set; }
        public string PopupMsg { get; set; }
        public bool IsSend { get; set; }
        public DateTime CreateDate { get; set; }
    }
}