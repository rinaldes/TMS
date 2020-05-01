using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class AdminUangJalan
    {
        private ContextModel context = new ContextModel();
        private ContextModelERP contextERP = new ContextModelERP();
        #region variable
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public string ListIdRute { get; set; }
        public string ListNamaRute { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderKontrak ModelKontrak { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        public List<AdminUangBorongan> ModelListBorongan { get; set; }
        public Decimal? NilaiBorongan { get; set; }
        public Decimal? Kawalan { get; set; }
        public Decimal? Timbangan { get; set; }
        public Decimal? Karantina { get; set; }
        public Decimal? SPSI { get; set; }
        public Decimal? Multidrop { get; set; }
        public List<AdminUangJalanTambahanRute> ModelListTambahanRute { get; set; }
        public List<AdminUangJalanTambahanLain> ModelListTambahanLain { get; set; }
        public Decimal? TotalBorongan { get; set; }
        public string KeteranganAdmin { get; set; }
        public int? IdDriverOld1 { get; set; }
        public string NamaDriverOld1 { get; set; }
        public int? IdDriverOld2 { get; set; }
        public string NamaDriverOld2 { get; set; }
        public int? IdDriver1 { get; set; }
        public string NamaDriver1 { get; set; }
        public string KeteranganGanti1 { get; set; }
        public int? IdDriver2 { get; set; }
        public string NamaDriver2 { get; set; }
        public string KeteranganGanti2 { get; set; }
        public Decimal? TotalKasbon { get; set; }
        public Decimal? KasbonDriver1 { get; set; }
        public Decimal? KasbonDriver2 { get; set; }
        public Decimal? TotalKlaim { get; set; }
        public Decimal? KlaimDriver1 { get; set; }
        public Decimal? KlaimDriver2 { get; set; }
        public List<AdminUangJalanPotonganLain> ModelListPotonganLain { get; set; }
        public Decimal? TotalPotonganDriver { get; set; }
        public Decimal? uangDM { get; set; }
        public List<AdminUangJalanVoucherSpbu> ModelListSpbu { get; set; }
        public List<AdminUangJalanVoucherKapal> ModelListKapal { get; set; }
        public List<AdminUangJalanUangTf> ModelListTf { get; set; }
        public string StrSolar { get; set; }
        public string StrKapal { get; set; }
        public string StrUang { get; set; }
        public Decimal? TotalAlokasi { get; set; }
        public Decimal? TotalSolar { get; set; }
        public Decimal? TotalKapal { get; set; }
        public Decimal? TotalUang { get; set; }
        public string ListIdSo { get; set; }
        public string SelectedListIdSo { get; set; }
        public decimal? PotonganB { get; set; }
        public decimal? PotonganP { get; set; }
        public decimal? PotonganK { get; set; }
        public decimal? PotonganT { get; set; }
        public string NamaDriverUangDM { get; set; }
        public int IdDriverUangDM { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public Atm DummyAtm { get; set; }
        public string StatusSo { get; set; }

        public List<RemovalAUJ> ModelListRemoval { get; set; }
        #endregion

        public AdminUangJalan()
        {
            ModelListTambahanRute = new List<AdminUangJalanTambahanRute>();
            ModelListTambahanLain = new List<AdminUangJalanTambahanLain>();
            ModelListPotonganLain = new List<AdminUangJalanPotonganLain>();
            ModelListBorongan = new List<AdminUangBorongan>();
            ModelListBorongan.Add(new AdminUangBorongan());
            ModelListSpbu = new List<AdminUangJalanVoucherSpbu>();
            ModelListKapal = new List<AdminUangJalanVoucherKapal>();
            ModelListTf = new List<AdminUangJalanUangTf>();
            ModelListRemoval = new List<RemovalAUJ>();
        }
        public AdminUangJalan(Context.AdminUangJalan dbitem)
        {
            IdSalesOrder = dbitem.SalesOrderId;
            Id = dbitem.Id;
            ModelListTambahanRute = new List<AdminUangJalanTambahanRute>();
            foreach (var item in dbitem.AdminUangJalanTambahanRute)
            {
                ModelListTambahanRute.Add(new AdminUangJalanTambahanRute(item));
            }
            ModelListTambahanLain = new List<AdminUangJalanTambahanLain>();
            foreach (var item in dbitem.AdminUangJalanTambahanLain)
            {
                ModelListTambahanLain.Add(new AdminUangJalanTambahanLain(item));
            }
            ModelListPotonganLain = new List<AdminUangJalanPotonganLain>();
            foreach (var item in dbitem.AdminUangJalanPotonganDriver)
            {
                ModelListPotonganLain.Add(new AdminUangJalanPotonganLain(item));
            }
            ModelListBorongan = new List<AdminUangBorongan>();
            foreach (var item in dbitem.IdDataBorongan.Split(','))
            {
                try
                {
                    ModelListBorongan.Add(new AdminUangBorongan() { IdDataBorongan = int.Parse(item), NamaDataBorongan = context.DataBorongan.Where(d => d.Id == int.Parse(item)).FirstOrDefault().NamaBorongan });
                }
                catch (Exception)
                {
                    ModelListBorongan.Add(new AdminUangBorongan() { IdDataBorongan = int.Parse(item), NamaDataBorongan = "" });
                }
            }
            NilaiBorongan = dbitem.NilaiBorongan;
            Kawalan = dbitem.Kawalan;
            Timbangan = dbitem.Timbangan;
            Karantina = dbitem.Karantina;
            SPSI = dbitem.SPSI;
            Multidrop = dbitem.Multidrop;
            TotalBorongan = dbitem.TotalBorongan;
            KeteranganAdmin = dbitem.KeteranganAdmin;
            IdDriverOld1 = dbitem.IdDriverOld1;
            NamaDriverOld1 = dbitem.IdDriverOld1.HasValue ? dbitem.DriverOld1.KodeDriver + " - " + dbitem.DriverOld1.NamaDriver : "";
            IdDriverOld2 = dbitem.IdDriverOld2;
            NamaDriverOld2 = dbitem.IdDriverOld2.HasValue ? dbitem.DriverOld2.KodeDriver + " - " + dbitem.DriverOld2.NamaDriver : "";
            IdDriver1 = dbitem.IdDriver1;
            NamaDriver1 = dbitem.IdDriver1.HasValue ? dbitem.Driver1.KodeDriver + " - " + dbitem.Driver1.NamaDriver : "";
            KeteranganGanti1 = dbitem.KeteranganGanti1;
            IdDriver2 = dbitem.IdDriver2;
            NamaDriver2 = dbitem.IdDriver2.HasValue ? dbitem.Driver2.KodeDriver + " - " + dbitem.Driver2.NamaDriver : "";
            KeteranganGanti2 = dbitem.KeteranganGanti2;
            TotalKasbon = dbitem.TotalKasbon;
            KasbonDriver1 = dbitem.KasbonDriver1;
            KasbonDriver2 = dbitem.KasbonDriver2;
            TotalKlaim = dbitem.TotalKlaim;
            KlaimDriver1 = dbitem.KlaimDriver1;
            KlaimDriver2 = dbitem.KlaimDriver2;
            TotalPotonganDriver = dbitem.TotalPotonganDriver;
            PotonganB = dbitem.PotonganB;
            PotonganP = dbitem.PotonganP;
            PotonganK = dbitem.PotonganK;
            PotonganT = dbitem.PotonganT;
            uangDM = dbitem.uangDM;
            try
            {
                IdDriverUangDM = contextERP.pby_mstr.Where(d => d.pby_code.Contains("DM-") && d.pby_code.Contains(dbitem.SONumber)).FirstOrDefault().pby_driver-7000000;
                NamaDriverUangDM = context.Driver.Where(d => d.Id == IdDriverUangDM).FirstOrDefault().NamaDriver;
            }
            catch (Exception) { }

            ModelListSpbu = new List<AdminUangJalanVoucherSpbu>();
            foreach (var item in dbitem.AdminUangJalanVoucherSpbu)
            {
                ModelListSpbu.Add(new AdminUangJalanVoucherSpbu(item));
            }
            ModelListKapal = new List<AdminUangJalanVoucherKapal>();
            foreach (var item in dbitem.AdminUangJalanVoucherKapal)
            {
                ModelListKapal.Add(new AdminUangJalanVoucherKapal(item));
            }
            ModelListTf = new List<AdminUangJalanUangTf>();
            foreach (var item in dbitem.AdminUangJalanUangTf)
            {
                if (item.Value >= 0)
                    ModelListTf.Add(new AdminUangJalanUangTf(item));
            }
            TotalAlokasi = dbitem.TotalAlokasi;

            if (context.Atm.Where(d => d.IdDriver == IdDriver1).FirstOrDefault() != null)
                DummyAtm = new Atm(context.Atm.Where(d => d.IdDriver == IdDriver1).FirstOrDefault());

            ModelListRemoval = new List<RemovalAUJ>();
            foreach (Context.Removal item in dbitem.Removal)
            {
                ModelListRemoval.Add(new RemovalAUJ(item));
            }
        }
        public void setDb(Context.AdminUangJalan dbitem)
        {
            List<string> listBor = new List<string>();
            foreach (var item in ModelListBorongan.Where(d => d.IsDelete == false))
            {
                listBor.Add(item.IdDataBorongan.Value.ToString());
            }
            dbitem.IdDataBorongan = string.Join(",", listBor);
            dbitem.NilaiBorongan = NilaiBorongan;
            dbitem.ListIdRute = ListIdRute;
            dbitem.ListNamaRute = ListNamaRute;
            dbitem.Kawalan = Kawalan;
            dbitem.Timbangan = Timbangan;
            dbitem.Karantina = Karantina;
            dbitem.SPSI = SPSI;
            dbitem.Multidrop = Multidrop;
            dbitem.TotalBorongan = TotalBorongan;
            dbitem.KeteranganAdmin = KeteranganAdmin;
            dbitem.IdDriver1 = IdDriver1;
            dbitem.IdDriver2 = IdDriver2;
            dbitem.IdDriverOld1 = IdDriverOld1;
            dbitem.IdDriverOld2 = IdDriverOld2;
            dbitem.KeteranganGanti1 = KeteranganGanti1;
            dbitem.KeteranganGanti2 = KeteranganGanti2;
            dbitem.KasbonDriver1 = KasbonDriver1;
            dbitem.KasbonDriver2 = KasbonDriver2;
            dbitem.TotalKasbon = TotalKasbon;
            dbitem.KlaimDriver1 = KlaimDriver1;
            dbitem.KlaimDriver2 = KlaimDriver2;
            dbitem.TotalKlaim = TotalKlaim;
            dbitem.TotalPotonganDriver = TotalPotonganDriver;
            dbitem.PotonganB = PotonganB;
            dbitem.PotonganP = PotonganP;
            dbitem.PotonganK = PotonganK;
            dbitem.PotonganT = PotonganT;
            dbitem.uangDM = uangDM;
            dbitem.AdminUangJalanTambahanRute.Clear();
            decimal TotalTambahanRuteMuat = 0;
            foreach (AdminUangJalanTambahanRute item in ModelListTambahanRute.Where(d => d.IsDelete == false))
            {
                dbitem.AdminUangJalanTambahanRute.Add(item.setDb(new Context.AdminUangJalanTambahanRute()));
                TotalTambahanRuteMuat += item.value.Value;
            }
            dbitem.TotalTambahanRuteMuat = TotalTambahanRuteMuat;
            dbitem.AdminUangJalanTambahanLain.Clear();
            decimal TotalTambahanLain = 0;
            foreach (AdminUangJalanTambahanLain item in ModelListTambahanLain.Where(d => d.IsDelete == false))
            {
                dbitem.AdminUangJalanTambahanLain.Add(item.setDb(new Context.AdminUangJalanTambahanLain()));
                TotalTambahanLain += item.Value.Value;
            }
            dbitem.TotalTambahanLain = TotalTambahanLain;
            dbitem.AdminUangJalanPotonganDriver.Clear();
            foreach (AdminUangJalanPotonganLain item in ModelListPotonganLain.Where(d => d.IsDelete == false))
            {
                dbitem.AdminUangJalanPotonganDriver.Add(item.setDb(new Context.AdminUangJalanPotonganDriver()));
            }
            dbitem.AdminUangJalanVoucherSpbu.Clear();
            foreach (AdminUangJalanVoucherSpbu item in ModelListSpbu)
            {
                dbitem.AdminUangJalanVoucherSpbu.Add(item.setDb(new Context.AdminUangJalanVoucherSpbu()));
            }
            dbitem.AdminUangJalanVoucherKapal.Clear();
            foreach (AdminUangJalanVoucherKapal item in ModelListKapal)
            {
                dbitem.AdminUangJalanVoucherKapal.Add(item.setDb(new Context.AdminUangJalanVoucherKapal()));
            }
            dbitem.AdminUangJalanUangTf.Clear();
            foreach (AdminUangJalanUangTf item in ModelListTf.Where(d => d.Value > 0))
            {
                dbitem.AdminUangJalanUangTf.Add(item.setDb(new Context.AdminUangJalanUangTf()));
            }
            dbitem.TotalAlokasi = TotalAlokasi;
        }
        public void setDbTambahanKonsolidasi(Context.AdminUangJalan dbitem)
        {
            dbitem.TotalBorongan = TotalBorongan;
            decimal TotalTambahanRuteMuat = dbitem.TotalTambahanRuteMuat == null ? 0 : dbitem.TotalTambahanRuteMuat.Value;
            foreach (AdminUangJalanTambahanRute item in ModelListTambahanRute.Where(d => !d.IsDelete))
            {
                if (item.IdDataBorongan != null)
                {
                    dbitem.AdminUangJalanTambahanRute.Add(item.setDb(new Context.AdminUangJalanTambahanRute()));
                    TotalTambahanRuteMuat += item.value.Value;
                }
            }
            dbitem.TotalTambahanRuteMuat = TotalTambahanRuteMuat;
            foreach (AdminUangJalanUangTf item in ModelListTf)
            {
                if (item.Nama.Contains("Transfer Tambahan Konsolidasi") && !dbitem.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains(item.Nama)))
                    dbitem.AdminUangJalanUangTf.Add(item.setDb(new Context.AdminUangJalanUangTf()));
            }
            dbitem.TotalAlokasi = TotalAlokasi;
            dbitem.TotalSolar = TotalSolar;
            dbitem.TotalKapal = TotalKapal;
            dbitem.TotalUang = TotalUang;
        }
    }

    public class AdminUangBorongan
    {
        public int Id { get; set; }
        public int? IdDataBorongan { get; set; }
        public string NamaDataBorongan { get; set; }
        public bool IsDelete { get; set; }
    }
    public class AdminUangJalanTambahanRute
    {
        public int Id { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public int? IdDataBorongan { get; set; }
        public string NamaDataBorongan { get; set; }
        public Decimal? value { get; set; }
        public bool IsDelete { get; set; }
        public AdminUangJalanTambahanRute() { }
        public AdminUangJalanTambahanRute(Context.AdminUangJalanTambahanRute dbitem)
        {
            Id = dbitem.Id;
            IdAdminUangJalan = dbitem.IdAdminUangJalan;
            IdDataBorongan = dbitem.IdDataBorongan;
            if (dbitem.DataBorongan != null)
                NamaDataBorongan = dbitem.DataBorongan.NamaBorongan;
            value = dbitem.values;
        }
        public Context.AdminUangJalanTambahanRute setDb(Context.AdminUangJalanTambahanRute dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdAdminUangJalan = IdAdminUangJalan;
            dbitem.IdDataBorongan = IdDataBorongan;
            dbitem.values = value;
            return dbitem;
        }
        public AdminUangJalanTambahanRute(Context.RemovalTambahanRute dbitem)
        {
            Id = dbitem.Id;
            IdAdminUangJalan = dbitem.IdRemoval;
            IdDataBorongan = dbitem.IdDataBorongan;
            NamaDataBorongan = dbitem.DataBorongan.NamaBorongan;
            value = dbitem.values;
        }
        public Context.RemovalTambahanRute setDb(Context.RemovalTambahanRute dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdRemoval = IdAdminUangJalan;
            dbitem.IdDataBorongan = IdDataBorongan;
            dbitem.values = value;
            return dbitem;
        }
    }
    public class AdminUangJalanTambahanLain
    {
        public int Id { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public string Keterangan { get; set; }
        public Decimal? Value { get; set; }
        public bool IsDelete { get; set; }
        public AdminUangJalanTambahanLain() { }
        public AdminUangJalanTambahanLain(Context.AdminUangJalanTambahanLain dbitem)
        {
            Id = dbitem.Id;
            IdAdminUangJalan = dbitem.IdAdminUangJalan;
            Keterangan = dbitem.Keterangan;
            Value = dbitem.Values;
        }
        public Context.AdminUangJalanTambahanLain setDb(Context.AdminUangJalanTambahanLain dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdAdminUangJalan = IdAdminUangJalan;
            dbitem.Keterangan = Keterangan;
            dbitem.Values = Value;
            return dbitem;
        }
        public AdminUangJalanTambahanLain(Context.RemovalTambahanLain dbitem)
        {
            Id = dbitem.Id;
            IdAdminUangJalan = dbitem.IdRemoval;
            Keterangan = dbitem.Keterangan;
            Value = dbitem.Values;
        }
        public Context.RemovalTambahanLain setDb(Context.RemovalTambahanLain dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdRemoval = IdAdminUangJalan;
            dbitem.Keterangan = Keterangan;
            dbitem.Values = Value;
            return dbitem;
        }
    }
    public class AdminUangJalanPotonganLain
    {
        public int Id { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public string Keterangan { get; set; }
        public int TypeDriver { get; set; }
        public int? Value { get; set; }
        public bool IsDelete { get; set; }
        public AdminUangJalanPotonganLain() { }
        public AdminUangJalanPotonganLain(Context.AdminUangJalanPotonganDriver dbitem)
        {
            Id = dbitem.Id;
            IdAdminUangJalan = dbitem.IdAdminUangJalan;
            Keterangan = dbitem.Keterangan;
            Value = dbitem.Value;
            TypeDriver = dbitem.TypeDriver;
        }
        public Context.AdminUangJalanPotonganDriver setDb(Context.AdminUangJalanPotonganDriver dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdAdminUangJalan = IdAdminUangJalan;
            dbitem.Keterangan = Keterangan;
            dbitem.Value = Value;
            dbitem.TypeDriver = TypeDriver;
            return dbitem;
        }
        public AdminUangJalanPotonganLain(Context.RemovalPotonganDriver dbitem)
        {
            Id = dbitem.Id;
            IdAdminUangJalan = dbitem.IdRemoval;
            Keterangan = dbitem.Keterangan;
            Value = dbitem.Value;
            TypeDriver = dbitem.TypeDriver;
        }
        public Context.RemovalPotonganDriver setDb(Context.RemovalPotonganDriver dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdRemoval = IdAdminUangJalan;
            dbitem.Keterangan = Keterangan;
            dbitem.Value = Value;
            dbitem.TypeDriver = TypeDriver;
            return dbitem;
        }
    }
    public class AdminUangJalanVoucherSpbu
    {
        public int Id { get; set; }
        public string NamaSpbu { get; set; }
        public int? Value { get; set; }
        public AdminUangJalanVoucherSpbu() { }
        public AdminUangJalanVoucherSpbu(Context.AdminUangJalanVoucherSpbu dbitem)
        {
            Id = dbitem.Id;
            NamaSpbu = dbitem.Keterangan;
            Value = dbitem.Value;
        }
        public Context.AdminUangJalanVoucherSpbu setDb(Context.AdminUangJalanVoucherSpbu dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = NamaSpbu;
            dbitem.Value = Value;
            return dbitem;
        }
        public AdminUangJalanVoucherSpbu(Context.RemovalVoucherSpbu dbitem)
        {
            Id = dbitem.Id;
            NamaSpbu = dbitem.Keterangan;
            Value = dbitem.Value;
        }
        public Context.RemovalVoucherSpbu setDb(Context.RemovalVoucherSpbu dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = NamaSpbu;
            dbitem.Value = Value;
            return dbitem;
        }
    }
    public class AdminUangJalanVoucherKapal
    {
        public int Id { get; set; }
        public string NamaPenyebrangan { get; set; }
        public int? Value { get; set; }
        public AdminUangJalanVoucherKapal() { }
        public AdminUangJalanVoucherKapal(Context.AdminUangJalanVoucherKapal dbitem)
        {
            Id = dbitem.Id;
            NamaPenyebrangan = dbitem.Keterangan;
            Value = dbitem.Value;
        }
        public Context.AdminUangJalanVoucherKapal setDb(Context.AdminUangJalanVoucherKapal dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = NamaPenyebrangan;
            dbitem.Value = Value;
            return dbitem;
        }
        public AdminUangJalanVoucherKapal(Context.RemovalVoucherKapal dbitem)
        {
            Id = dbitem.Id;
            NamaPenyebrangan = dbitem.Keterangan;
            Value = dbitem.Value;
        }
        public Context.RemovalVoucherKapal setDb(Context.RemovalVoucherKapal dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = NamaPenyebrangan;
            dbitem.Value = Value;
            return dbitem;
        }
    }
    public class AdminUangJalanUangTf
    {
        private ContextModelERP context = new ContextModelERP();
        private ContextModel contextBiasa = new ContextModel();
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Code { get; set; }
        public int IdCreditTf { get; set; }
        public string NamaCreditTf { get; set; }
        public string CodeCreditTf { get; set; }
        public int? Value { get; set; }
        public DateTime? Tanggal { get; set; }
        public decimal? JumlahTransfer { get; set; }
        public int? idRekening { get; set; }
        public string NoRekening { get; set; }
        public string AtasNama { get; set; }
        public string NamaBank { get; set; }
        public DateTime? TanggalAktual { get; set; }
        public TimeSpan? JamAktual { get; set; }
        public string KeteranganTf { get; set; }
        public string KeteranganAdmin { get; set; }
        public int? IdDriverPenerima { get; set; }
        public string DriverPenerima { get; set; }
        public bool isTf { get; set; }
        public string NamaBankAsal { get; set; }
        public string KodeBankAsal { get; set; }

        public AdminUangJalanUangTf() { }
        public AdminUangJalanUangTf(Context.AdminUangJalanUangTf dbitem)
        {
            Id = dbitem.Id;
            Nama = dbitem.Keterangan;
            Value = dbitem.Value;
            Tanggal = dbitem.Tanggal;
            Code = dbitem.Code;
            JumlahTransfer = dbitem.JumlahTransfer.HasValue ? dbitem.JumlahTransfer : 0;
            if (context.ac_mstr.Where(d => d.ac_id == dbitem.IdCreditTf).FirstOrDefault() != null)
            {
                KodeBankAsal = context.ac_mstr.Where(d => d.ac_id == dbitem.IdCreditTf).FirstOrDefault().ac_code;
                NamaBankAsal = context.ac_mstr.Where(d => d.ac_id == dbitem.IdCreditTf).FirstOrDefault().ac_name;
            }
            idRekening = dbitem.idRekenings;
            if (dbitem.idRekenings.HasValue)
            {
                NoRekening = dbitem.Atm.NoRekening;
                AtasNama = dbitem.Atm.AtasNama;
                NamaBank = dbitem.Atm.LookupCodeBank.Nama;
            }
            else
            {
                Context.Atm dbattm = contextBiasa.Atm.Where(d => d.IdDriver == dbitem.AdminUangJalan.IdDriver1).FirstOrDefault();
                if (dbattm != null)
                {
                    idRekening = dbattm.Id;
                    NoRekening = dbattm.NoRekening;
                    AtasNama = dbattm.AtasNama;
                    NamaBank = dbattm.LookupCodeBank.Nama;
                }
            }
            TanggalAktual = dbitem.TanggalAktual.HasValue ? dbitem.TanggalAktual : DateTime.Now;
            JamAktual = dbitem.JamAktual;
            KeteranganTf = dbitem.KeteranganTf;
            KeteranganAdmin = dbitem.AdminUangJalan.KeteranganAdmin;
            if (dbitem.IdDriverPenerima.HasValue)
            {
                IdDriverPenerima = dbitem.IdDriverPenerima;
                DriverPenerima = dbitem.Driver.NamaDriver;
            }
            isTf = dbitem.isTf;
        }
        public Context.AdminUangJalanUangTf setDb(Context.AdminUangJalanUangTf dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = Nama;
            dbitem.Value = Value;
            dbitem.Code = Code;
            dbitem.Tanggal = Tanggal.Value.AddDays(1);
            dbitem.JumlahTransfer = JumlahTransfer;
            dbitem.idRekenings = idRekening;
            dbitem.TanggalAktual = TanggalAktual.HasValue ? TanggalAktual.Value.AddDays(1) : TanggalAktual;
            dbitem.JamAktual = JamAktual;
            dbitem.KeteranganTf = KeteranganTf;
            dbitem.isTf = isTf;
            dbitem.IdDriverPenerima = IdDriverPenerima;
            dbitem.IdCreditTf = IdCreditTf;
            return dbitem;
        }
        public Context.AdminUangJalanUangTf setDbKasir(Context.AdminUangJalanUangTf dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = Nama;
            dbitem.Value = Value;
            dbitem.Code = Code;
            dbitem.Tanggal = Tanggal;
            dbitem.JumlahTransfer = JumlahTransfer;
            dbitem.idRekenings = idRekening;
            dbitem.TanggalAktual = TanggalAktual;
            dbitem.JamAktual = JamAktual;
            dbitem.KeteranganTf = KeteranganTf;
            dbitem.isTf = isTf;
            dbitem.IdDriverPenerima = IdDriverPenerima;
            dbitem.IdCreditTf = IdCreditTf;
            return dbitem;
        }
        public Context.AdminUangJalanUangTf setDbKasirKontrak(Context.AdminUangJalanUangTf dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = Nama;
            dbitem.Value = Value;
            dbitem.Code = Code;
            dbitem.JumlahTransfer = JumlahTransfer;
            dbitem.idRekenings = idRekening;
            dbitem.TanggalAktual = TanggalAktual.Value.AddHours(7);
            dbitem.JamAktual = JamAktual;
            dbitem.KeteranganTf = KeteranganTf;
            dbitem.isTf = isTf;
            dbitem.IdDriverPenerima = IdDriverPenerima;
            dbitem.IdCreditTf = IdCreditTf;
            return dbitem;
        }
        public AdminUangJalanUangTf(Context.RemovalUangTf dbitem)
        {
            Id = dbitem.Id;
            Nama = dbitem.Keterangan;
            Value = dbitem.Value;
            Tanggal = dbitem.Tanggal;
            JumlahTransfer = dbitem.JumlahTransfer.HasValue ? dbitem.JumlahTransfer : 0;
            KodeBankAsal = "C";
            NamaBankAsal = "D";
            idRekening = dbitem.idRekenings;
            if (dbitem.idRekenings.HasValue)
            {
                NoRekening = dbitem.Atm.NoRekening;
                AtasNama = dbitem.Atm.AtasNama;
                NamaBank = dbitem.Atm.LookupCodeBank.Nama;
            }
            else
            {
                Context.Atm dbattm = contextBiasa.Atm.Where(d => d.IdDriver == dbitem.Removal.IdDriver1).FirstOrDefault();
                if (dbattm != null)
                {
                    idRekening = dbattm.Id;
                    NoRekening = dbattm.NoRekening;
                    AtasNama = dbattm.AtasNama;
                    NamaBank = dbattm.LookupCodeBank.Nama;
                }
            }
            TanggalAktual = dbitem.TanggalAktual.HasValue ? dbitem.TanggalAktual : DateTime.Now;
            JamAktual = dbitem.JamAktual;
            KeteranganTf = dbitem.KeteranganTf;
            KeteranganAdmin = dbitem.Removal.KeteranganAdmin;
            if (dbitem.IdDriverPenerima.HasValue)
            {
                IdDriverPenerima = dbitem.IdDriverPenerima;
                DriverPenerima = dbitem.Driver.NamaDriver;
            }
            isTf = dbitem.isTf;
        }
        public Context.RemovalUangTf setDb(Context.RemovalUangTf dbitem)
        {
            dbitem.Id = Id;
            dbitem.Keterangan = Nama;
            dbitem.Value = Value;
            dbitem.Tanggal = Tanggal.Value.AddDays(1);
            dbitem.JumlahTransfer = JumlahTransfer;
            dbitem.idRekenings = idRekening;
            dbitem.TanggalAktual = TanggalAktual.HasValue ? TanggalAktual.Value.AddDays(1) : TanggalAktual;
            dbitem.JamAktual = JamAktual;
            dbitem.KeteranganTf = KeteranganTf;
            dbitem.isTf = isTf;
            dbitem.IdDriverPenerima = IdDriverPenerima;
            dbitem.IdCreditTf = IdCreditTf;
            return dbitem;
        }
    }
}