using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderKonsolidasiDp
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SalesOrderKonsolidasi")]
        public int SalesOrderKonsolidasiId { get; set; }
        public DateTime Tanggal { get; set; }
        public string Penerima { get; set; }
        public string Jenis { get; set; }
        [ForeignKey("Rekenings")]
        public int? RekeningId { get; set; }
        public decimal Jumlah { get; set; }

        public virtual SalesOrderKonsolidasi SalesOrderKonsolidasi { get; set; }
        public virtual Rekenings Rekenings { get; set; }
    }
}