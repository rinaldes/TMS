using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RevisiRute
    {
        public RevisiRute()
        {
            this.RevisiRuteLoadUnLoadAddress = new HashSet<RevisiRuteLoadUnLoadAddress>();
        }

        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [ForeignKey("SalesOrderOld")]
        [Column(Order = 1)]
        public int? IdSalesOrderOld { get; set; }
        
        [ForeignKey("SalesOrder")]
        [Column(Order = 2)]
        public int? IdSalesOrder { get; set; }
                
        public String Keterangan { get; set; }
        public String Code { get; set; }
                
        public int? IdDaftarHargaItemLama { get; set; }
                
        public String StrDaftarHargaItemLama { get; set; }

        public System.DateTime? ModifiedDate { get; set; }
        
        public virtual SalesOrder SalesOrderOld { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
        public virtual ICollection<RevisiRuteLoadUnLoadAddress> RevisiRuteLoadUnLoadAddress { get; set; }

    }
}