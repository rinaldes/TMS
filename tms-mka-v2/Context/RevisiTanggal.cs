using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RevisiTanggal
    {
        public RevisiTanggal()
        {
            
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("SalesOrder")]
        public int? IdSalesOrder { get; set; }
        public String Keterangan { get; set; }
        public bool IsBP { get; set; }
        public System.DateTime? TanggalMuat { get; set; }
        public TimeSpan JamMuat { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        
        public virtual SalesOrder SalesOrder { get; set; }
    }
}