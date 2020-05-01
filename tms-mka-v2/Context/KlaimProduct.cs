using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class KlaimProduct
    {
        public KlaimProduct()
        {
        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Klaim")]
        public int IdKlaim { get; set; }
        [ForeignKey("MasterProduct")]
        public int? IdProduct { get; set; }
        public int Jumlah { get; set; }
        public int NilaiKlaim { get; set; }
        public string Keterangan { get; set; }
        public string KodeBarang { get; set; }

        public virtual MasterProduct MasterProduct { get; set; }
        public virtual Klaim Klaim { get; set; }
    }
}