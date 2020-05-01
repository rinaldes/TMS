using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class TolPulang
    {
        public TolPulang()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("JnsTol")]
        public int? IdTol { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("RuteTol")]
        public int IdRuteTol { get; set; }
        public virtual JnsTols JnsTol { get; set; }
        public virtual RuteTol RuteTol { get; set; }
    }
}