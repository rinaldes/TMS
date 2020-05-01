using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerCreditStatusHistory
    {
        public CustomerCreditStatusHistory()
        {
            
        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("CustomerCreditStatus")]
        public int CustomerCreditStatusId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("CustomerCreditStatus")]
        public int CustomerId { get; set; }
        public string StatusSystem { get; set; }
        public string StatusOveride { get; set; }
        public string Keterangan { get; set; }
        public string Condition { get; set; }
        public int? MinTOPOverdue1 { get; set; }
        public int? MaxTOPOverdue1 { get; set; }
        public int? ValueOverdue2 { get; set; }
        public int? TOPOverdue2 { get; set; }
        public int? ShipmentDay1 { get; set; }
        public int? ShipmentDay2 { get; set; }
        public string StatAwal { get; set; }
        public string StatAkhir { get; set; }
        public DateTime Tanggal { get; set; }
        public string Username { get; set; }
        public virtual CustomerCreditStatus CustomerCreditStatus { get; set; }
    }
}