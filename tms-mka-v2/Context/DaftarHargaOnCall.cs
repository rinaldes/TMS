using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaOnCall
    {
        public DaftarHargaOnCall()
        {
            this.DaftarHargaOnCallItem = new HashSet<DaftarHargaOnCallItem>();
            this.DaftarHargaOnCallAttachment = new HashSet<DaftarHargaOnCallAttachment>();
            this.DaftarHargaOnCallKondisi = new HashSet<DaftarHargaOnCallKondisi>();
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int? IdCust { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<DaftarHargaOnCallItem> DaftarHargaOnCallItem { get; set; }
        public virtual ICollection<DaftarHargaOnCallKondisi> DaftarHargaOnCallKondisi { get; set; }
        public virtual ICollection<DaftarHargaOnCallAttachment> DaftarHargaOnCallAttachment { get; set; }
    }
}