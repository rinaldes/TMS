using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace tms_mka_v2.Models
{
    public class SettlementBatal
    {
        public int Id { get; set; }
        [Display(Name = "Sales order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdSalesOrder { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public string Code { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
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
        
        public SettlementBatal() { }
        public SettlementBatal(Context.SettlementBatal dbitem)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSalesOrder;
            
            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem.SalesOrder);
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem.SalesOrder);
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem.SalesOrder);
            }
            KasDiterima = dbitem.KasDiterima;
            TransferDiterima = dbitem.TransferDiterima;
            SolarDiterima = dbitem.SolarDiterima;
            KapalDiterima = dbitem.KapalDiterima;
            KeteranganDiterima = dbitem.KeteranganDiterima;
            KasDiakui = dbitem.KasDiakui;
            TransferDiakui = dbitem.TransferDiakui;
            SolarDiakui = dbitem.SolarDiakui;
            KapalDiakui = dbitem.KapalDiakui;
            KeteranganDiakui = dbitem.KeteranganDiakui;
            KasKembali = dbitem.KasKembali;
            TransferKembali = dbitem.TransferKembali;
            SolarKembali = dbitem.SolarKembali;
            KapalKembali = dbitem.KapalKembali;
            KeteranganKembali = dbitem.KeteranganKembali;
            KasAktual = dbitem.KasAktual;
            TransferAktual = dbitem.TransferAktual;
            SolarAktual = dbitem.SolarAktual;
            KapalAktual = dbitem.KapalAktual;
            KeteranganAktual = dbitem.KeteranganAktual;
            KasSelisih = dbitem.KasSelisih;
            TransferSelisih = dbitem.TransferSelisih;
            SolarSelisih = dbitem.SolarSelisih;
            KapalSelisih = dbitem.KapalSelisih;
            KeteranganSelisih = dbitem.KeteranganSelisih;
            Keterangan = dbitem.Keterangan;
            IdAdminUangJalan = dbitem.IdAdminUangJalan;
        }
        public void SetDb(Context.SettlementBatal dbitem)
        {
            dbitem.IdSalesOrder = IdSalesOrder;
            dbitem.KasDiterima = KasDiterima;
            dbitem.TransferDiterima = TransferDiterima;
            dbitem.SolarDiterima = SolarDiterima;
            dbitem.KapalDiterima = KapalDiterima;
            dbitem.KeteranganDiterima = KeteranganDiterima;
            dbitem.KasDiakui = KasDiakui;
            dbitem.TransferDiakui = TransferDiakui;
            dbitem.SolarDiakui = SolarDiakui;
            dbitem.KapalDiakui = KapalDiakui;
            dbitem.KeteranganDiakui = KeteranganDiakui;
            dbitem.KasKembali = KasKembali;
            dbitem.TransferKembali = TransferKembali;
            dbitem.SolarKembali = SolarKembali;
            dbitem.KapalKembali = KapalKembali;
            dbitem.KeteranganKembali = KeteranganKembali;
            dbitem.KasAktual = KasAktual;
            dbitem.TransferAktual = TransferAktual;
            dbitem.SolarAktual = SolarAktual;
            dbitem.KapalAktual = KapalAktual;
            dbitem.KeteranganAktual = KeteranganAktual;
            dbitem.KasSelisih = KasSelisih;
            dbitem.TransferSelisih = TransferSelisih;
            dbitem.SolarSelisih = SolarSelisih;
            dbitem.KapalSelisih = KapalSelisih;
            dbitem.KeteranganSelisih = KeteranganSelisih;
            dbitem.Keterangan = Keterangan;
            dbitem.IsProses = true;
            dbitem.IdCreditTf = IdCreditTf;
        }
    }
}