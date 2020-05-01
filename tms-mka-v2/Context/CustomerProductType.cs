using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerProductType
    {
        public CustomerProductType()
        {

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("MasterProduct")]
        public int? idProduk { get; set; }
        public string PenangananKhusus { get; set; }
        public string Keterangan { get; set; }

        public virtual MasterProduct MasterProduct { get; set; }
        public virtual Customer Customer { get; set; }

        
    }
}