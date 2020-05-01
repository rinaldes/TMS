using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class OrderHistory
    {
        public OrderHistory()
        { }

        [Key]
        public int Id { get; set; }
        public int StatusFlow { get; set; }
        public DateTime FlowDate { get; set; }
        public DateTime SavedAt { get; set; }
        public DateTime ProcessedAt { get; set; }
        public int? PIC { get; set; }
        public int? SalesOrderId { get; set; }
        public string ListSo { get; set; }
    }
}