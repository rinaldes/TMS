using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class HistoryJnsTols
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ForJnsTol")]
        public int IdTol { get; set; }
        public DateTime Tanggal { get; set; }
        public string NamaTol { get; set; }
        public Decimal GolonganTol1 { get; set; }
        public Decimal GolonganTol2 { get; set; }
        public Decimal GolonganTol3 { get; set; }
        public Decimal GolonganTol4 { get; set; }
        public string Keterangan { get; set; }
        [ForeignKey("ForUser")]
        public int? IdUser { get; set; }

        public virtual User ForUser { get; set; }
        public virtual JnsTols ForJnsTol { get; set; }
        
    }
}