using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaKontrak
    {
        public DaftarHargaKontrak()
        {
            this.DaftarHargaKontrakItem = new HashSet<DaftarHargaKontrakItem>();
            this.DaftarHargaKontrakAttachment = new HashSet<DaftarHargaKontrakAttachment>();
            this.DaftarHargaKontrakKondisi = new HashSet<DaftarHargaKontrakKondisi>();
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int? IdCust { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        [ForeignKey("LookUpTypeKontrak")]
        public int? IdTypeKontrak { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual LookupCode LookUpTypeKontrak { get; set; }
        public virtual ICollection<DaftarHargaKontrakItem> DaftarHargaKontrakItem { get; set; }
        public virtual ICollection<DaftarHargaKontrakAttachment> DaftarHargaKontrakAttachment { get; set; }
        public virtual ICollection<DaftarHargaKontrakKondisi> DaftarHargaKontrakKondisi { get; set; }
    }
}