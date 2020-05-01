using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RevisiJenisTruk
    {
        public RevisiJenisTruk()
        {
            
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("SalesOrderOld")]
        public int? IdSalesOrderOld { get; set; }
        [ForeignKey("SalesOrder")]
        public int? IdSalesOrder { get; set; }
        public String Keterangan { get; set; }
        public int? JenisTruckIdLama { get; set; }
        public int? JenisTruckIdBaru { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        
        public virtual SalesOrder SalesOrderOld { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }        

    }
}