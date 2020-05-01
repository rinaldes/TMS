using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Customer
    {
        public Customer()
        {
            this.CustomerPic = new HashSet<CustomerPic>();
            this.CustomerAddress = new HashSet<CustomerAddress>();
            this.CustomerProductType = new HashSet<CustomerProductType>();
            this.CustomerLoadingAddress = new HashSet<CustomerLoadingAddress>();
            this.CustomerUnloadingAddress = new HashSet<CustomerUnloadingAddress>();
            this.CustomerPPN = new HashSet<CustomerPPN>();
            this.CustomerAttachment = new HashSet<CustomerAttachment>();
            this.CustomerSupplier = new HashSet<CustomerSupplier>();
            this.CustomerTypeTrucks = new HashSet<CustomerTypeTrucks>();
            this.CustomerBilling = new HashSet<CustomerBilling>();
            this.CustomerNotification = new HashSet<CustomerNotification>();
            this.CustomerCreditStatus = new HashSet<CustomerCreditStatus>();
        }

        [Key]
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerCodeOld { get; set; }
        public string CustomerNama { get; set; }
        [ForeignKey("LookupCode")]
        public int PrioritasId { get; set; }
        public bool WajibPO { get; set; }
        public bool WajibGPS { get; set; }
        public bool IsVendor { get; set; }
        public int? CustomerPicId { get; set; }
        public string SpecialTreatment { get; set; }
        public string Keterangan { get; set; }
        public int urutan { get; set; }
        public System.DateTime sent_to_erp { get; set; }

        public virtual LookupCode LookupCode { get; set; }

        public virtual ICollection<CustomerPic> CustomerPic { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<CustomerProductType> CustomerProductType { get; set; }
        public virtual ICollection<CustomerLoadingAddress> CustomerLoadingAddress { get; set; }
        public virtual ICollection<CustomerUnloadingAddress> CustomerUnloadingAddress { get; set; }
        public virtual ICollection<CustomerPPN> CustomerPPN { get; set; }
        public virtual ICollection<CustomerAttachment> CustomerAttachment { get; set; }
        public virtual ICollection<CustomerSupplier> CustomerSupplier { get; set; }
        public virtual ICollection<CustomerTypeTrucks> CustomerTypeTrucks { get; set; }
        public virtual ICollection<CustomerBilling> CustomerBilling { get; set; }
        public virtual ICollection<CustomerNotification> CustomerNotification { get; set; }
        public virtual ICollection<CustomerCreditStatus> CustomerCreditStatus { get; set; }
    }
}