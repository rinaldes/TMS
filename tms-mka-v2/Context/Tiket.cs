using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Tiket
    {
        public Tiket()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NoTiket { get; set; }
        [ForeignKey("Customer")]
        public int? IdCustomer { get; set; }
        [ForeignKey("User")]
        public int? CreatedBy { get; set; }
        public int Urutan { get; set; }
        public DateTime TanggalLapor{ get; set; }
        public string DitujukanKe { get; set; }
        public string Kategori { get; set; }
        public string Prioritas { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Keluhan { get; set; }
        public string Respon { get; set; }
        public string NamaPelapor { get; set; }
        [ForeignKey("CS")]
        public int? IdCS { get; set; }
        public string Attactment { get; set; }
        [ForeignKey("SalesOrder")]
        public int? IdSo { get; set; }
        public string IdSoKontrak { get; set; }
        public DateTime LastUpdate { get; set; }
        public string ResponseAttachment { get; set; }

        //public bool IsDelete { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User CS { get; set; }
        public virtual User User { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }        

        public virtual ICollection<TiketResponse> TiketResponse { get; set; } 
    }
}