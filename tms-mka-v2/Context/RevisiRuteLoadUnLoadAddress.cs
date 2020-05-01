using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RevisiRuteLoadUnLoadAddress
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }                
        [ForeignKey("RevisiRute")]
        [Column(Order = 1)]
        public int RevisiRuteId { get; set; }       
        public int? CustomerLoadUnLoadAddressId { get; set; }
        public int? CustomerId { get; set; }
        public string LoadUnloadAddressType { get; set; }
        public int? Urutan { get; set; }
        public bool IsSelected { get; set; }
                
        public virtual RevisiRute RevisiRute { get; set; }
    }
}