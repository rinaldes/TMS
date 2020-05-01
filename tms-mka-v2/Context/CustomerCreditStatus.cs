using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerCreditStatus
    {
        public CustomerCreditStatus()
        {
            this.CustomerCreditStatusHistory = new HashSet<CustomerCreditStatusHistory>();
        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Customer")]
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

        public virtual Customer Customer { get; set; }
        public virtual ICollection<CustomerCreditStatusHistory> CustomerCreditStatusHistory { get; set; }
    }
}