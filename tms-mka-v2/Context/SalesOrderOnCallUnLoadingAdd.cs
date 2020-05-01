using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderOnCallUnLoadingAdd
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SalesOrderOncall")]
        public int SalesOrderOnCallId { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("CustomerUnloadingAddress")]
        public int? CustomerUnloadingAddressId { get; set; }
        [Key]
        [Column(Order = 3)]
        [ForeignKey("CustomerUnloadingAddress")]
        public int? CustomerId { get; set; }
        public int urutan { get; set; }
        public bool IsSelect { get; set; }
        
        public virtual SalesOrderOncall SalesOrderOncall { get; set; }
        public virtual CustomerUnloadingAddress CustomerUnloadingAddress { get; set; }
    }
}