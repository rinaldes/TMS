using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderKontrakDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SalesOrderKontrak")]
        public int SalesKontrakId { get; set; }
        public string NoSo { get; set; }
        public DateTime MuatDate { get; set; }
        public bool IsProses { get; set; }
        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        [ForeignKey("Driver1")]
        public int? Driver1Id { get; set; }
        [ForeignKey("Driver2")]
        public int? Driver2Id { get; set; }
        public int Urutan { get; set; }

        public virtual SalesOrderKontrak SalesOrderKontrak { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
    }
}