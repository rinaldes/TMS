using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderKontrakListSo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SalesOrderKontrak")]
        public int? SalesKontrakId { get; set; }
        public int? SalesKontrakTruckId { get; set; }
        [ForeignKey("Dokumen")]
        public int? DokumenId { get; set; }
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
        public string Status { get; set; }
        public string StatusFlow { get; set; }
        public string StatusBatal { get; set; }
        [ForeignKey("AdminUangJalan")]
        public int? IdAdminUangJalan { get; set; }
        public Boolean IsBatalTruck { get; set; }
        public Guid oidErp { get; set; }
        public bool IsKenaImbasBatalTruck { get; set; }

        public virtual Dokumen Dokumen { get; set; }
        public virtual SalesOrderKontrak SalesOrderKontrak { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual AdminUangJalan AdminUangJalan { get; set; }
    }
}