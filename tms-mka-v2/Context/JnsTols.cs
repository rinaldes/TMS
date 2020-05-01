using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class JnsTols
    {
        public JnsTols()
        {
            this.ListHistoryJnsTols = new HashSet<HistoryJnsTols>();
        }

        [Key]
        public int Id { get; set; }
        public string NamaTol { get; set; }
        public Decimal GolonganTol1 { get; set; }
        public Decimal GolonganTol2 { get; set; }
        public Decimal GolonganTol3 { get; set; }
        public Decimal GolonganTol4 { get; set; }
        public string Keterangan { get; set; }
        //public bool IsDelete { get; set; }

        public virtual ICollection<HistoryJnsTols> ListHistoryJnsTols { get; set; }
    }
}