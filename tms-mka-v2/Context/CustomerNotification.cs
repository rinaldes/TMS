using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerNotification
    {
        public CustomerNotification()
        {
            this.CustomerNotifTruck = new HashSet<CustomerNotifTruck>();
            this.CustomerNotifRute = new HashSet<CustomerNotifRute>();
        }

        [Key]
        [Column(Order=0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        //[ForeignKey("CustomerPic")]
        public int? IdPic { get; set; }
        public string NotifType { get; set; }

        public virtual Customer Customer { get; set; }
        //public virtual CustomerPic CustomerPic { get; set; }
        public virtual ICollection<CustomerNotifTruck> CustomerNotifTruck { get; set; }
        public virtual ICollection<CustomerNotifRute> CustomerNotifRute { get; set; }
    }
}