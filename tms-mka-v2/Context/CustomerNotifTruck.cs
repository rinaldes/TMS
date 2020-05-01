using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerNotifTruck
    {
        public CustomerNotifTruck()
        {

        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("CustomerNotification")]
        public int CustomerNotifId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("CustomerNotification")]
        public int CustomerId { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? IdTruck { get; set; }
        public virtual CustomerNotification CustomerNotification { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
    }
}