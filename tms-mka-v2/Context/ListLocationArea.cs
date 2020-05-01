using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ListLocationArea
    {
        public ListLocationArea()
        {

        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("MasterArea")]
        public int AreaId { get; set; }
        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public virtual MasterArea MasterArea { get; set; }
        public virtual Location Location { get; set; }
    }
}