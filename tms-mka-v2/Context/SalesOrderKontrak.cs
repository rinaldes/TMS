using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderKontrak
    {
        public SalesOrderKontrak()
        {
            this.SalesOrderKontrakDetail = new HashSet<SalesOrderKontrakDetail>();
            this.SalesOrderKontrakTruck = new HashSet<SalesOrderKontrakTruck>();
            this.SalesOrderKontrakListSo = new HashSet<SalesOrderKontrakListSo>();
            this.SalesOrderKontrakDp = new HashSet<SalesOrderKontrakDp>();
        }
        [Key]
        public int SalesOrderKontrakId { get; set; }
        public string SONumber { get; set; }
        public string DN { get; set; }
        public int Urutan { get; set; }
        public DateTime DocDate { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? JenisTruckId { get; set; }
        [ForeignKey("MasterProduct")]
        public int? ProductId { get; set; }
        public DateTime PeriodStr { get; set; }
        public DateTime PeriodEnd { get; set; }
        public TimeSpan JamMuat { get; set; }
        public int JumlahTruck { get; set; }
        public int Rit { get; set; }
        public string Keterangan { get; set; }
        public int JumlahHari { get; set; }
        public int JumlahHariKerja { get; set; }
        public int JumlahHariLibur { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual MasterProduct MasterProduct { get; set; }

        public virtual ICollection<SalesOrderKontrakDetail> SalesOrderKontrakDetail { get; set; }
        public virtual ICollection<SalesOrderKontrakTruck> SalesOrderKontrakTruck { get; set; }
        public virtual ICollection<SalesOrderKontrakListSo> SalesOrderKontrakListSo { get; set; }
        public virtual ICollection<SalesOrderKontrakDp> SalesOrderKontrakDp { get; set; }
    }
}