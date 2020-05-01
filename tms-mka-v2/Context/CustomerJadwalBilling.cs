using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerJadwalBilling
    {
        public CustomerJadwalBilling()
        {

        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("CustomerBilling")]
        public int CustomerBillingId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("CustomerBilling")]
        public int CustomerId { get; set; }
        public string Hari { get; set; }
        public string Jam { get; set; }
        public string Catatan { get; set; }
        public string PIC { get; set; }
        public virtual CustomerBilling CustomerBilling { get; set; }
    }
}