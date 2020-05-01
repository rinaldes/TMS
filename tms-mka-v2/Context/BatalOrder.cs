using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class BatalOrder
    {
        public BatalOrder()
        {
            
        }

        [Key]
        public int Id { get; set; }        
        [ForeignKey("SalesOrder")]
        public int? IdSalesOrder { get; set; }
        public int? UrutanBatal { get; set; }
        public string Code { get; set; }
        public string IdSoKontrak { get; set; }
        public bool? IsBatalTruk { get; set; }
        public bool? IsTransfer { get; set; }
        public String Keterangan { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public DateTime? TanggalBatal { get; set; }
        
        public virtual SalesOrder SalesOrder { get; set; }

    }
}