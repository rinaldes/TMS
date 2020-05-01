using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class SettlementReg
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        [Display(Name = "Sales order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdSalesOrder { get; set; }
        public string Code { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        public SalesOrderKontrak ModelKontrak { get; set; }
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
        public string SPBUKembali { get; set; }
        public List<SettlementRegTambahanBiaya> ListBiayaTambahan { get; set; }
        public string StrBiayaTambahan { get; set; }

        public Decimal? TotalCash { get; set; }
        public DateTime? TanggalCash { get; set; }
        public int? IdDriverTujuan { get; set; }
        public string NamaDriverTujuan { get; set; }
        public int? IdDriverTitip { get; set; }
        public string NamaDriverTitip { get; set; }
        public Decimal? TotalTf { get; set; }
        public DateTime? TanggalTf { get; set; }
        public int? IdAtm { get; set; }
        public string NoRekening { get; set; }
        public string AtasNamaRek { get; set; }
        public string Bank { get; set; }
        public string KeteranganPembayaran { get; set; }

        public Decimal? TotalBayar { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public string listIdSoKontrak { get; set; }
        public Atm DummyAtm { get; set; }
        
        public SettlementReg()
        {
            ListBiayaTambahan = new List<SettlementRegTambahanBiaya>();
        }
        public SettlementReg(Context.SettlementReguler dbitem, List<Context.Atm> listAtm)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSalesOrder;
            Code = dbitem.Code;
            if (dbitem.LisSoKontrak != null && dbitem.LisSoKontrak != "")
            {
                Context.SalesOrderKontrakListSo sokls = context.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == dbitem.IdAdminUangJalan).FirstOrDefault();
                Context.SalesOrder so = context.SalesOrder.Where(d => d.SalesOrderKontrakId == sokls.SalesKontrakId.Value).FirstOrDefault();
                ModelKontrak = new SalesOrderKontrak(so);
                IdSalesOrder = so.Id;
            }
            else if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
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
            else if (dbitem.SalesOrder.SalesOrderKontrakId.HasValue)
            {
                ModelKontrak = new SalesOrderKontrak(dbitem.SalesOrder);
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

            ListBiayaTambahan = new List<SettlementRegTambahanBiaya>();
            foreach (var item in dbitem.SettlementRegulerTambahanBiaya)
            {
                ListBiayaTambahan.Add(new SettlementRegTambahanBiaya(item));
            }

            TotalCash = dbitem.TotalCash;
            TanggalCash = dbitem.TanggalCash;
            IdDriverTujuan = dbitem.IdDriverTujuan;
            if (dbitem.IdDriverTujuan.HasValue)
                NamaDriverTujuan = dbitem.DriverTujuan.KodeDriver + " - " + dbitem.DriverTujuan.NamaDriver;
            IdDriverTitip = dbitem.IdDriverTitip;
            if (dbitem.IdDriverTitip.HasValue)
                NamaDriverTitip = dbitem.DriverTitip.KodeDriver + " - " + dbitem.DriverTitip.NamaDriver;
            TotalTf = dbitem.TotalTf;
            TanggalTf = dbitem.TanggalTf;
            if (dbitem.IdAtm.HasValue)
            {
                IdAtm = dbitem.IdAtm;
                NoRekening = dbitem.Atm.NoRekening;
                AtasNamaRek = dbitem.Atm.AtasNama;
                Bank = dbitem.Atm.LookupCodeBank.Nama;
            }
            else
            {
                //Context.Atm dbattm = listAtm.Where(d => d.IdDriver == dbitem.SalesOrder.AdminUangJalan.IdDriver1).FirstOrDefault();
                //if (dbattm != null)
                //{
                //    IdAtm = dbattm.Id;
                //    NoRekening = dbattm.NoRekening;
                //    AtasNamaRek = dbattm.AtasNama;
                //    Bank = dbattm.LookupCodeBank.Nama;
                //}
            }

            KeteranganPembayaran = dbitem.KeteranganPembayaran;
            listIdSoKontrak = dbitem.LisSoKontrak;
            TotalBayar = dbitem.TotalBayar;
        }
        public void SetDb(Context.SettlementReguler dbitem)
        {
            dbitem.Code = Code;
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

            dbitem.SettlementRegulerTambahanBiaya.Clear();
            foreach (var item in ListBiayaTambahan)
            {
                dbitem.SettlementRegulerTambahanBiaya.Add(item.SetDb(new Context.SettlementRegulerTambahanBiaya()));
            }

            dbitem.TotalCash = TotalCash;
            dbitem.TanggalCash = TanggalCash;
            dbitem.IdDriverTujuan = IdDriverTujuan;
            dbitem.IdDriverTitip = IdDriverTitip;
            dbitem.TotalTf = TotalTf;
            dbitem.TanggalTf = TanggalTf;
            dbitem.IdAtm = IdAtm;
            dbitem.KeteranganPembayaran = KeteranganPembayaran;
            dbitem.TotalBayar = TotalBayar;
            dbitem.LisSoKontrak = listIdSoKontrak;
        }
    }

    public class SettlementRegTambahanBiaya
    {
        public int Id { get; set; }
        public string Keterangan { get; set; }
        public string NamaBiaya { get; set; }
        public Decimal Value { get; set; }
        public SettlementRegTambahanBiaya() { }
        public SettlementRegTambahanBiaya(Context.SettlementRegulerTambahanBiaya dbitem) {
            Keterangan = dbitem.Keterangan;
            Value = dbitem.Value;
        }
        public Context.SettlementRegulerTambahanBiaya SetDb(Context.SettlementRegulerTambahanBiaya dbitem)
        {
            dbitem.Keterangan = Keterangan;
            dbitem.Value = Value;
            dbitem.NamaBiaya = NamaBiaya;
            return dbitem;
        }
    }
}