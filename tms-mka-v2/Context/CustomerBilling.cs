using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerBilling
    {
        public CustomerBilling()
        {
            this.CustomerJadwalBilling = new HashSet<CustomerJadwalBilling>();
        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public string DocumentName { get; set; }
        public int Lembar { get; set; }
        public string Warna { get; set; }
        public bool Stempel { get; set; }
        public bool IsFax { get; set; }
        public bool IsActive { get; set; }
        public string Fax { get; set; }
        public bool IsEmail { get; set; }
        public string Email { get; set; }
        public bool IsTukarFaktur { get; set; }
        public bool IsJasaPengiriman { get; set; }
        public string UrlAtt { get; set; }
        public string FileName { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<CustomerJadwalBilling> CustomerJadwalBilling { get; set; }
    }
}