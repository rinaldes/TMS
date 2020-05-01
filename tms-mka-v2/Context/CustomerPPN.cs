using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerPPN
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public bool PPN { get; set; }
        [ForeignKey("Rekenings")]
        public int? IdRekening { get; set; }
        public string NomorNPWP { get; set; }
        public string NamaNPWP { get; set; }
        public string AddressNPWP { get; set; }
        
        public virtual Rekenings Rekenings { get; set; }
        public virtual Customer Customer { get; set; }
    }
}