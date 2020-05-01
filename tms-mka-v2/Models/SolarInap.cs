using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class SolarInap
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdSO { get; set; }
        [Display(Name = "Tanggal")]
        public string Status { get; set; }
        public string TanggalDari { get; set; }
        public string TanggalHingga { get; set; }
        [Display(Name = "Nilai Yang Diajukan")]
        public int NilaiYgDiajukan { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganOperation { get; set; }
        [Display(Name = "Nominal")]
        public int Nominal { get; set; }
        public int? SalesOrderKontrakListSOId { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganMarketing { get; set; }
        [Display(Name = "Keterangan Admin")]
        public string KeteranganAdmin { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganBatal { get; set; }
        [Display(Name = "Status Tagihan")]
        public string StatusTagihan { get; set; }
        [Display(Name = "Diberikan Untuk")]
        public string KasirDiberikanUntuk { get; set; }
        [Display(Name = "Diberikan Untuk")]
        public int? IdDriver { get; set; }
//        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdCreditTf { get; set; }

        [Display(Name = "Cash")]
        public int Cash { get; set; }
        [Display(Name = "Transfer")]
        public int Transfer { get; set; }
        public int CashBack { get; set; }
        [Display(Name = "Tanggal")]
        public string TglTransfer { get; set; }
        [Display(Name = "Tanggal Tiba")]
        public System.DateTime? TanggalTiba { get; set; }
        [Display(Name = "Jam Tiba")]
        public TimeSpan JamTiba { get; set; }
        [Display(Name = "Tanggal")]
        public string TglCash { get; set; }
        [Display(Name = "Dititipkan Ke")]
        public string DititipKe { get; set; }

        [Display(Name = "Rekening")]
        public int? IdAtm { get; set; }

        [Display(Name = "Jumlah Cash")]
        public int AktualCash { get; set; }

        [Display(Name = "Tanggal Aktual")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime TanggalAktualCash { get; set; }

        [Display(Name = "Jam")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamAktualCash { get; set; }

        [Display(Name = "Tanggal Aktual")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime TanggalAktualTransfer { get; set; }

        [Display(Name = "Tanggal Batal")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime TanggalBatal { get; set; }

        [Display(Name = "Jam")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamAktualTransfer { get; set; }

        [Display(Name = "Dititipkan Kepada")]
        public string AktualDititipkanKepada { get; set; }

        [Display(Name = "Keterangan Kasir")]
        public string KeteranganKasirCash { get; set; }

        [Display(Name = "Keterangan Kasir")]
        public string KeteranganKasirTransfer { get; set; }

        [Display(Name = "Jumlah Transfer")]
        public int AktualTransfer { get; set; }

        public int StepKe { get; set; }
        public string NamaDriver { get; set; }
        public string KodeDriver { get; set; }
        public string AtasNama { get; set; }
        public string BankNama { get; set; }
        public string NoRek { get; set; }
        public string NoSO { get; set; }
        public string VehicleNo { get; set; }
        public string JenisTruk { get; set; }
        public string Customer { get; set; }
        public string KodeNama { get; set; }
        public System.DateTime? TanggalBerangkat { get; set; }
        public System.DateTime? TanggalMuat { get; set; }
        private ContextModel context = new ContextModel();

        public SolarInap()
        {

        }
        public SolarInap(Context.SolarInap dbitem, Context.SalesOrderKontrakListSo sokontrak = null)
        {
             Id = dbitem.Id;
             IdSO = dbitem.IdSO;
             KasirDiberikanUntuk = dbitem.SO.SalesOrderOncallId.HasValue ? dbitem.SO.SalesOrderOncall.Driver1.NamaDriver : dbitem.SO.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SO.SalesOrderProsesKonsolidasi.Driver1.NamaDriver : dbitem.SO.SalesOrderPickupId.HasValue ? dbitem.SO.SalesOrderPickup.Driver1.NamaDriver : "";
             Status = dbitem.StepKe == 1 ? "Marketing" : dbitem.StepKe == 2 ? "Admin Uang Jalan" : dbitem.StepKe == 3 ? (dbitem.AktualCash > 0 && dbitem.AktualTransfer > 0 ? "Sudah" : dbitem.AktualCash > 0 ? (dbitem.Transfer > 0 ? "Kasir Transfer" : "Sudah") : dbitem.AktualTransfer > 0 ? (dbitem.Cash > 0 ? "Kasir Kas" : "Sudah") : "Kasir") : dbitem.StepKe == 4 ? "Batal Inap" : "Sudah";
             if (dbitem.SO != null){
                 if (sokontrak != null)
                     TanggalMuat = sokontrak.MuatDate;
                 else if (dbitem.SO.SalesOrderOncallId.HasValue)
                     TanggalMuat = dbitem.SO.SalesOrderOncall.TanggalMuat;
                 else if (dbitem.SO.SalesOrderProsesKonsolidasiId.HasValue)
                     TanggalMuat = dbitem.SO.SalesOrderProsesKonsolidasi.TanggalMuat;
                 else if (dbitem.SO.SalesOrderPickup != null)
                     TanggalMuat = dbitem.SO.SalesOrderPickup.TanggalPickup;
             }
             TanggalDari = (dbitem.TanggalDari.Date.ToString().Split())[0];
             TanggalHingga = (dbitem.TanggalHingga.Date.ToString().Split())[0];
             NilaiYgDiajukan = dbitem.NilaiYgDiajukan;
             CashBack = dbitem.CashBack;
             KeteranganAdmin = dbitem.KeteranganAdmin;
             KeteranganMarketing = dbitem.KeteranganMarketing;
             KeteranganOperation = dbitem.KeteranganOperation;
             KeteranganKasirCash = dbitem.KeteranganKasirCash;
             Nominal = dbitem.Nominal;
             StatusTagihan = dbitem.StatusTagihan;
             IdDriver = dbitem.IdDriver;
             Cash = dbitem.Cash;
             Transfer = dbitem.Transfer;
             TglCash = (dbitem.TglCash.Date.ToString().Split())[0];
             TglTransfer = (dbitem.TglTransfer.Date.ToString().Split())[0];
             DititipKe = dbitem.DititipKe;
             IdAtm = dbitem.IdAtm;
             StepKe = dbitem.StepKe;
             TanggalTiba = dbitem.TanggalTiba;
             JamTiba = dbitem.JamTiba;
             if (dbitem.IdAtm.HasValue)
            {
                AtasNama = dbitem.Atm.AtasNama;
                BankNama = dbitem.Atm.LookupCodeBank.Nama;
                NoRek = dbitem.Atm.NoRekening;
            }
            if (dbitem.SO.SalesOrderOncallId.HasValue){
                NoSO = dbitem.SO.SalesOrderOncall.SONumber;
                VehicleNo = dbitem.SO.SalesOrderOncall.DataTruck.VehicleNo;
                NamaDriver = dbitem.SO.SalesOrderOncall.Driver1.NamaDriver;
                KodeDriver = dbitem.SO.SalesOrderOncall.Driver1.KodeDriver;
                JenisTruk = dbitem.SO.SalesOrderOncall.JenisTrucks.StrJenisTruck;
                Customer = dbitem.SO.SalesOrderOncall.Customer.CustomerNama;
                KodeNama = dbitem.SO.SalesOrderOncall.Customer.CustomerCodeOld;
                TanggalBerangkat = dbitem.SO.SalesOrderOncall.TanggalMuat;
            }
            else if (dbitem.SO.SalesOrderPickupId.HasValue){
                NoSO = dbitem.SO.SalesOrderPickup.SONumber;
                VehicleNo = dbitem.SO.SalesOrderPickup.DataTruck.VehicleNo;
                NamaDriver = dbitem.SO.SalesOrderPickup.Driver1.NamaDriver;
                KodeDriver = dbitem.SO.SalesOrderPickup.Driver1.KodeDriver;
                JenisTruk = dbitem.SO.SalesOrderPickup.JenisTrucks.StrJenisTruck;
                Customer = dbitem.SO.SalesOrderPickup.Customer.CustomerNama;
            }
            else if (dbitem.SO.SalesOrderProsesKonsolidasiId.HasValue){
                NoSO = dbitem.SO.SalesOrderProsesKonsolidasi.SONumber;
                VehicleNo = dbitem.SO.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                NamaDriver = dbitem.SO.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                KodeDriver = dbitem.SO.SalesOrderProsesKonsolidasi.Driver1.KodeDriver;
                JenisTruk = dbitem.SO.SalesOrderProsesKonsolidasi.JenisTrucks.StrJenisTruck;
                TanggalBerangkat = dbitem.SO.SalesOrderProsesKonsolidasi.TanggalMuat;
            }
            else if (sokontrak != null){
                NoSO = sokontrak.NoSo;
                try
                {
                    VehicleNo = sokontrak.DataTruck.VehicleNo;
                    JenisTruk = sokontrak.DataTruck.JenisTrucks.StrJenisTruck;
                }
                catch (Exception) { }
                try
                {
                    NamaDriver = sokontrak.Driver1.NamaDriver;
                    KodeDriver = sokontrak.Driver1.KodeDriver;
                }
                catch (Exception) { }
                TanggalBerangkat = sokontrak.MuatDate;
            }
        }
        public void setDb(Context.SolarInap dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdSO = IdSO;
            if(TanggalDari != null && TanggalDari != "")
                dbitem.TanggalDari = DateTime.ParseExact(TanggalDari, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TanggalHingga != null && TanggalHingga != "")
                dbitem.TanggalHingga = DateTime.ParseExact(TanggalHingga, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            dbitem.SalesOrderKontrakListSOId = SalesOrderKontrakListSOId;
            dbitem.NilaiYgDiajukan = NilaiYgDiajukan;
            dbitem.KeteranganAdmin = KeteranganAdmin;
            dbitem.KeteranganMarketing = KeteranganMarketing;
            dbitem.KeteranganOperation = KeteranganOperation;
            dbitem.Nominal = Nominal;
            dbitem.StatusTagihan = StatusTagihan;
            dbitem.IdDriver = IdDriver;
            dbitem.Cash = Cash;
            dbitem.Transfer = Transfer;
            if (TglCash != null && TglCash != "")
                dbitem.TglCash = DateTime.ParseExact(TglCash, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TglTransfer != null && TglTransfer != "")
                dbitem.TglTransfer = DateTime.ParseExact(TglTransfer, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            dbitem.DititipKe = DititipKe;
            dbitem.IdAtm = IdAtm;
            dbitem.StepKe = StepKe;
            dbitem.TanggalTiba = TanggalTiba.Value;
            dbitem.JamTiba = JamTiba;
        }

        public void setDbMarketing(Context.SolarInap dbitem)
        {
            dbitem.KeteranganMarketing = KeteranganMarketing;
            dbitem.Nominal = Nominal;
            dbitem.StatusTagihan = StatusTagihan;
        }

        public void setDbNew(Context.SolarInap dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdSO = IdSO;
            if (TanggalDari != null && TanggalDari != "")
                dbitem.TanggalDari = DateTime.ParseExact(TanggalDari, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TanggalHingga != null && TanggalHingga != "")
                dbitem.TanggalHingga = DateTime.ParseExact(TanggalHingga, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            dbitem.SalesOrderKontrakListSOId = SalesOrderKontrakListSOId;
            dbitem.NilaiYgDiajukan = NilaiYgDiajukan;
            dbitem.KeteranganAdmin = KeteranganAdmin;
            dbitem.KeteranganMarketing = KeteranganMarketing;
            dbitem.KeteranganOperation = KeteranganOperation;
            dbitem.Nominal = Nominal;
            dbitem.StatusTagihan = StatusTagihan;
            dbitem.IdDriver = IdDriver;
            dbitem.Cash = Cash;
            dbitem.Transfer = Transfer;
            if (TglCash != null && TglCash != "")
                dbitem.TglCash = DateTime.ParseExact(TglCash, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TglTransfer != null && TglTransfer != "")
                dbitem.TglTransfer = DateTime.ParseExact(TglTransfer, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            dbitem.DititipKe = DititipKe;
            dbitem.IdAtm = IdAtm;
            dbitem.StepKe = 2;
            dbitem.TanggalTiba = TanggalTiba.Value;
            dbitem.JamTiba = JamTiba;
            try
            {
                dbitem.UrutanBatal = context.SolarInap.Where(d => d.IdSO == IdSO && d.TanggalDari == dbitem.TanggalDari).Select(e => e.UrutanBatal).Max() + 1;
            }
            catch (Exception)
            { }
            if (dbitem.UrutanBatal == null)
                dbitem.UrutanBatal = 1;
        }

        public void setDbKasirCash(Context.SolarInap dbitem)
        {
            dbitem.AktualCash = Cash;
            dbitem.TanggalAktualCash = TanggalAktualCash;
            dbitem.JamAktualCash = JamAktualCash;
            dbitem.AktualDititipkanKepada = AktualDititipkanKepada;
            dbitem.KeteranganKasirCash = KeteranganKasirCash;
        }

        public void setPengembalianUangSolarInap(Context.SolarInap dbitem)
        {
            dbitem.CashBack = CashBack;
        }
        public void setDbKasirTransfer(Context.SolarInap dbitem)
        {
            dbitem.AktualTransfer = Transfer;
            dbitem.AktualIdAtm = IdAtm;
            dbitem.TanggalAktualTransfer = TanggalAktualTransfer;
            dbitem.JamAktualTransfer = JamAktualTransfer;
            dbitem.KeteranganKasirTransfer = KeteranganKasirTransfer;
        }
        public void setDbSolarInap(Context.SolarInap dbitem)
        {
            dbitem.TanggalBatal = TanggalBatal;
            dbitem.KeteranganBatal = KeteranganBatal;
            dbitem.StepKe = 4;
        }
    }
}