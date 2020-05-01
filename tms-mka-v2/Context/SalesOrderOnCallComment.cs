using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderOnCallComment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SalesOrderOncall")]
        public int SalesOrderOnCallId { get; set; }
        public DateTime Tanggal { get; set; }
        public string CommentUser { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }


        public virtual SalesOrderOncall SalesOrderOncall { get; set; }
    }
}