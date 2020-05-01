using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderKontrakTruck
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SalesOrderKontrak")]
        public int SalesKontrakId { get; set; }
        [ForeignKey("DataTruck")]
        public int? DataTruckId { get; set; }
        public string StatusTruk { get; set; }
        [ForeignKey("Driver1")]
        public int? IdDriver1 { get; set; }
        [ForeignKey("Driver2")]
        public int? IdDriver2 { get; set; }
        public bool PendapatanDiakui { get; set; }

        public virtual SalesOrderKontrak SalesOrderKontrak { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
    }
}