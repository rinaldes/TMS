using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class HistoryJalanTruck
    {
        public HistoryJalanTruck()
        {}

        [Key]
        public int Id { get; set; }
        [ForeignKey("AdminUangJalan")]
        public int IdAdminUangJalan { get; set; }
        [ForeignKey("Driver1")]
        public int IdDriver1 { get; set; }
        [ForeignKey("Driver2")]
        public int? IdDriver2 { get; set; }
        [ForeignKey("DataTruck")]
        public int IdTruck { get; set; }
        [ForeignKey("Customer")]
        public int? IdCustomer { get; set; }
        public string ShipmentId { get; set; }
        public string NoSo { get; set; }
        public DateTime TanggalMuat { get; set; }
        public string JenisOrder { get; set; }
        public string Rute { get; set; }

        public virtual AdminUangJalan AdminUangJalan { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual DataTruck DataTruck { get; set; }
    }
}