using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class WorkshopMekanik
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("DataMekanik")]
        public int? IdMekanik { get; set; }
        [ForeignKey("Workshop")]
        public int? IdWorkshop { get; set; }
        public virtual Mekanik DataMekanik { get; set; }
        public virtual Spk Workshop { get; set; }
    }
}