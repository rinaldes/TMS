using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaKonsolidasi
    {
        public DaftarHargaKonsolidasi()
        {
            this.DaftarHargaKonsolidasiItem = new HashSet<DaftarHargaKonsolidasiItem>();
            this.DaftarHargaKonsolidasiAttachment = new HashSet<DaftarHargaKonsolidasiAttachment>();
            this.DaftarHargaKonsolidasiKondisi = new HashSet<DaftarHargaKonsolidasiKondisi>();
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int? IdCust { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<DaftarHargaKonsolidasiItem> DaftarHargaKonsolidasiItem { get; set; }
        public virtual ICollection<DaftarHargaKonsolidasiAttachment> DaftarHargaKonsolidasiAttachment { get; set; }
        public virtual ICollection<DaftarHargaKonsolidasiKondisi> DaftarHargaKonsolidasiKondisi{ get; set; }
    }
}