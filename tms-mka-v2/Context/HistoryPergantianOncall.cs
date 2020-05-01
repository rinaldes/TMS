using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class HistoryPergantianOncall
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("SalesOrderOncall")]
        public int? SalesOrderOncallId { get; set; }
        [ForeignKey("Driver")]
        public int? IdDriver { get; set; }
        [ForeignKey("DriverOld")]
        public int? IdDriverOld { get; set; }
        public string KeteranganGanti { get; set; }
        public DateTime Tanggal { get; set; }

        public virtual SalesOrderOncall SalesOrderOncall { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Driver DriverOld { get; set; }
    }
}