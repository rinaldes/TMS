using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SettlementBatal
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("SalesOrder")]
        public int? IdSalesOrder { get; set; }
        public string IdSoKontrak { get; set; }
        public string Code { get; set; }
        [ForeignKey("BatalOrder")]
        public int? IdBatalOrder { get; set; }
        public string BatalTruckCode { get; set; }
        [ForeignKey("Driver")]
        public int? IdDriver { get; set; }
        public int? IdAdminUangJalan { get; set; }
        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        public decimal? KasDiterima { get; set; }
        public decimal? TransferDiterima { get; set; }
        public decimal? SolarDiterima { get; set; }
        public decimal? KapalDiterima { get; set; }
        public string KeteranganDiterima { get; set; }
        public decimal? KasDiakui { get; set; }
        public decimal? TransferDiakui { get; set; }
        public decimal? SolarDiakui { get; set; }
        public decimal? KapalDiakui { get; set; }
        public string KeteranganDiakui { get; set; }
        public decimal? KasKembali { get; set; }
        public decimal? TransferKembali { get; set; }
        public decimal? SolarKembali { get; set; }
        public decimal? KapalKembali { get; set; }
        public string KeteranganKembali { get; set; }
        public decimal? KasAktual { get; set; }
        public decimal? TransferAktual { get; set; }
        public decimal? SolarAktual { get; set; }
        public decimal? KapalAktual { get; set; }
        public string KeteranganAktual { get; set; }
        public decimal? KasSelisih { get; set; }
        public decimal? TransferSelisih { get; set; }
        public decimal? SolarSelisih { get; set; }
        public decimal? KapalSelisih { get; set; }
        public string KeteranganSelisih { get; set; }
        public string Keterangan { get; set; }
        public bool IsProses { get; set; }
        public string JenisBatal { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string SPBUKembali { get; set; }
        public int? IdCreditTf { get; set; }
        public int? jumSo { get; set; }
        public int? jumSoBatal { get; set; }
        public int? jumSoJalan { get; set; }

        public virtual BatalOrder BatalOrder { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual DataTruck DataTruck { get; set; }
    }
}