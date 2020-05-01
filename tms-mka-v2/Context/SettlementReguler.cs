using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SettlementReguler
    {
        public SettlementReguler()
        {
            this.SettlementRegulerTambahanBiaya = new HashSet<SettlementRegulerTambahanBiaya>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        [ForeignKey("SalesOrder")]
        public int? IdSalesOrder { get; set; }
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
        public string LisSoKontrak { get; set; }

        public Decimal? TotalCash { get; set; }
        public bool CashExecuted { get; set; }
        public bool TfExecuted { get; set; }
        public DateTime? TanggalCash { get; set; }
        [ForeignKey("DriverTujuan")]
        public int? IdDriverTujuan { get; set; }
        [ForeignKey("DriverTitip")]        
        public int? IdDriverTitip { get; set; }
        public Decimal? TotalTf { get; set; }
        public DateTime? TanggalTf { get; set; }
        [ForeignKey("Atm")]
        public int? IdAtm { get; set; }
        public string KeteranganPembayaran { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Decimal? TotalBayar { get; set; }
        [ForeignKey("AdminUangJalan")]
        public int? IdAdminUangJalan { get; set; }

        public virtual SalesOrder SalesOrder { get; set; }
        public virtual Atm Atm { get; set; }
        public virtual Driver DriverTujuan { get; set; }
        public virtual Driver DriverTitip { get; set; }
        public virtual AdminUangJalan AdminUangJalan { get; set; }
        public virtual ICollection<SettlementRegulerTambahanBiaya> SettlementRegulerTambahanBiaya { get; set; }
    }
}