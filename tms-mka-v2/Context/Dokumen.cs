using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Dokumen
    {
        public Dokumen()
        {
            this.DokumenItem = new HashSet<DokumenItem>();
            this.DokumenItemHistory = new HashSet<DokumenItemHistory>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("SalesOrder")]
        public int? IdSO { get; set; }
        public string ListIdSo { get; set; }
        public string SONumber { get; set; }
        public string SONumberUniq { get; set; }
        public bool IsComplete { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime ModifiedDate { get; set; }
        public System.DateTime? ReceivedDate { get; set; }
        public System.DateTime? TanggalMuat { get; set; }
        public bool IsReturn { get; set; }
        public bool IsLengkap { get; set; }
        [ForeignKey("Customer")]
        public int? IdCustomer { get; set; }
        [ForeignKey("Driver")]
        public int? DriverId { get; set; }
        [ForeignKey("DataTruck")]
        public int? DataTruckId { get; set; }
        public string RuteSo { get; set; }
        public string Kelengkapan { get; set; } //Lengkap, Tidak Lengkap, Tidak Ada
        public string Status { get; set; } //Lengkap, Tidak Lengkap, Tidak Ada

        public virtual SalesOrder SalesOrder { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual ICollection<DokumenItem> DokumenItem { get; set; }
        public virtual ICollection<DokumenItemHistory> DokumenItemHistory { get; set; }
    }
}