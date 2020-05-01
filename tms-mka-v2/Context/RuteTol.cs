using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RuteTol
    {
        public RuteTol()
        {
            this.ListTolBerangkat = new HashSet<TolBerangkat>();
            this.ListTolPulang = new HashSet<TolPulang>();
        }

        [Key]
        [Column(Order=0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public bool IsDelete { get; set; }
        [ForeignKey("Rute")]
        public int? IdRute { get; set; }
        public string NamaRuteTol { get; set; }
        public virtual Rute Rute { get; set; }
        public virtual ICollection<TolBerangkat> ListTolBerangkat { get; set; }
        public virtual ICollection<TolPulang> ListTolPulang { get; set; }
    }
}