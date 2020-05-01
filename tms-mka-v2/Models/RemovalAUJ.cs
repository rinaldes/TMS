using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class RemovalAUJ
    {
        private ContextModel context = new ContextModel();
        #region variable
        public int Id { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public int? IdSO { get; set; }
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
        public string KeteranganRemoval { get; set; }
        public int? IdDriver1 { get; set; }
        public string NamaDriver1 { get; set; }
        public int? IdDriver2 { get; set; }
        public string NamaDriver2 { get; set; }
        public Decimal? TotalKasbon { get; set; }
        public Decimal? KasbonDriver1 { get; set; }
        public Decimal? KasbonDriver2 { get; set; }
        public Decimal? TotalKlaim { get; set; }
        public Decimal? KlaimDriver1 { get; set; }
        public Decimal? KlaimDriver2 { get; set; }
        public List<AdminUangJalanPotonganLain> ModelListPotonganLain { get; set; }
        public Decimal? TotalPotonganDriver { get; set; }
        public List<AdminUangJalanVoucherSpbu> ModelListSpbu { get; set; }
        public List<AdminUangJalanVoucherKapal> ModelListKapal { get; set; }
        public List<AdminUangJalanUangTf> ModelListTf { get; set; }
        public string StrSolar { get; set; }
        public string StrKapal { get; set; }
        public string StrUang { get; set; }
        public Decimal? TotalAlokasi { get; set; }
        public string ListIdSo { get; set; }
        public string SelectedListIdSo { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public Atm DummyAtm { get; set; }
        public string Status { get; set; }

        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        #endregion
        
        public RemovalAUJ()
        {
            ModelListTambahanRute = new List<AdminUangJalanTambahanRute>();
            ModelListTambahanLain = new List<AdminUangJalanTambahanLain>();
            ModelListPotonganLain = new List<AdminUangJalanPotonganLain>();
            ModelListBorongan = new List<AdminUangBorongan>();
            ModelListBorongan.Add(new AdminUangBorongan());
            ModelListSpbu = new List<AdminUangJalanVoucherSpbu>();
            ModelListKapal = new List<AdminUangJalanVoucherKapal>();
            ModelListTf = new List<AdminUangJalanUangTf>();
        }
        public RemovalAUJ(Context.Removal dbitem)
        {
            Id = dbitem.Id;
            IdAdminUangJalan = dbitem.IdAdminUangJalan;
            IdSO = dbitem.IdSO;
            ModelListTambahanRute = new List<AdminUangJalanTambahanRute>();
            foreach (var item in dbitem.RemovalTambahanRute)
            {
                ModelListTambahanRute.Add(new AdminUangJalanTambahanRute(item));
            }
            ModelListTambahanLain = new List<AdminUangJalanTambahanLain>();
            foreach (var item in dbitem.RemovalTambahanLain)
            {
                ModelListTambahanLain.Add(new AdminUangJalanTambahanLain(item));
            }
            ModelListPotonganLain = new List<AdminUangJalanPotonganLain>();
            foreach (var item in dbitem.RemovalPotonganDriver)
            {
                ModelListPotonganLain.Add(new AdminUangJalanPotonganLain(item));
            }
            ModelListBorongan = new List<AdminUangBorongan>();
            if (dbitem.IdDataBorongan != null)
            {
                foreach (var item in dbitem.IdDataBorongan.Split(','))
                {
                    ModelListBorongan.Add(new AdminUangBorongan() { IdDataBorongan = int.Parse(item), NamaDataBorongan = context.DataBorongan.Where(d => d.Id == int.Parse(item)).FirstOrDefault().NamaBorongan });
                }
            }
            if (ModelListBorongan.Count == 0)
            {
                ModelListBorongan.Add(new AdminUangBorongan());
            }
            NilaiBorongan = dbitem.NilaiBorongan;
            Kawalan = dbitem.Kawalan;
            Timbangan = dbitem.Timbangan;
            Karantina = dbitem.Karantina;
            SPSI = dbitem.SPSI;
            Multidrop = dbitem.Multidrop;
            TotalBorongan = dbitem.TotalBorongan;
            KeteranganAdmin = dbitem.KeteranganAdmin;
            KeteranganRemoval = dbitem.KeteranganRemoval;
            IdDriver1 = dbitem.IdDriver1;
            NamaDriver1 = dbitem.IdDriver1.HasValue ? dbitem.Driver1.KodeDriver + " - " + dbitem.Driver1.NamaDriver : ""; 
            IdDriver2 = dbitem.IdDriver2;
            NamaDriver2 = dbitem.IdDriver2.HasValue ? dbitem.Driver2.KodeDriver + " - " + dbitem.Driver2.NamaDriver : "";
            TotalKasbon = dbitem.TotalKasbon;
            KasbonDriver1 = dbitem.KasbonDriver1;
            KasbonDriver2 = dbitem.KasbonDriver2;
            TotalKlaim = dbitem.TotalKlaim;
            KlaimDriver1 = dbitem.KlaimDriver1;
            KlaimDriver2 = dbitem.KlaimDriver2;
            TotalPotonganDriver = dbitem.TotalPotonganDriver;
            Status = dbitem.Status;
            ModelListSpbu = new List<AdminUangJalanVoucherSpbu>();
            foreach (var item in dbitem.RemovalVoucherSpbu)
            {
                ModelListSpbu.Add(new AdminUangJalanVoucherSpbu(item));
            }
            ModelListKapal = new List<AdminUangJalanVoucherKapal>();
            foreach (var item in dbitem.RemovalVoucherKapal)
            {
                ModelListKapal.Add(new AdminUangJalanVoucherKapal(item));
            }
            ModelListTf = new List<AdminUangJalanUangTf>();
            foreach (var item in dbitem.RemovalUangTf)
            {
                ModelListTf.Add(new AdminUangJalanUangTf(item));
            }
            TotalAlokasi = dbitem.TotalAlokasi;

            DummyAtm = new Atm(context.Atm.Where(d => d.IdDriver == IdDriver1).FirstOrDefault());

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
        }
        public void setDb(Context.Removal dbitem)
        {
            List<string> listBor = new List<string>();
            foreach (var item in ModelListBorongan.Where(d => d.IsDelete == false))
            {
                listBor.Add(item.IdDataBorongan.Value.ToString());
            }
            dbitem.IdDataBorongan = string.Join(",", listBor);
            dbitem.NilaiBorongan = NilaiBorongan;
            dbitem.Kawalan = Kawalan;
            dbitem.Timbangan = Timbangan;
            dbitem.Karantina = Karantina;
            dbitem.SPSI = SPSI;
            dbitem.Multidrop = Multidrop;
            dbitem.TotalBorongan = TotalBorongan;
            dbitem.KeteranganAdmin = KeteranganAdmin;
            dbitem.IdDriver1 = IdDriver1;
            dbitem.IdDriver2 = IdDriver2;
            dbitem.KasbonDriver1 = KasbonDriver1;
            dbitem.KasbonDriver2 = KasbonDriver2;
            dbitem.TotalKasbon = TotalKasbon;
            dbitem.KlaimDriver1 = KlaimDriver1;
            dbitem.KlaimDriver2 = KlaimDriver2;
            dbitem.TotalKlaim = TotalKlaim;
            dbitem.TotalPotonganDriver = TotalPotonganDriver;

            dbitem.RemovalTambahanRute.Clear();
            foreach (AdminUangJalanTambahanRute item in ModelListTambahanRute.Where(d => d.IsDelete == false))
            {
                dbitem.RemovalTambahanRute.Add(item.setDb(new Context.RemovalTambahanRute()));
            }
            dbitem.RemovalTambahanLain.Clear();
            foreach (AdminUangJalanTambahanLain item in ModelListTambahanLain.Where(d => d.IsDelete == false))
            {
                dbitem.RemovalTambahanLain.Add(item.setDb(new Context.RemovalTambahanLain()));
            }
            dbitem.RemovalPotonganDriver.Clear();
            foreach (AdminUangJalanPotonganLain item in ModelListPotonganLain.Where(d => d.IsDelete == false))
            {
                dbitem.RemovalPotonganDriver.Add(item.setDb(new Context.RemovalPotonganDriver()));
            }
            dbitem.RemovalVoucherSpbu.Clear();
            foreach (AdminUangJalanVoucherSpbu item in ModelListSpbu)
            {
                dbitem.RemovalVoucherSpbu.Add(item.setDb(new Context.RemovalVoucherSpbu()));
            }
            dbitem.RemovalVoucherKapal.Clear();
            foreach (AdminUangJalanVoucherKapal item in ModelListKapal)
            {
                dbitem.RemovalVoucherKapal.Add(item.setDb(new Context.RemovalVoucherKapal()));
            }
            dbitem.RemovalUangTf.Clear();
            foreach (AdminUangJalanUangTf item in ModelListTf)
            {
                dbitem.RemovalUangTf.Add(item.setDb(new Context.RemovalUangTf()));
            }
            dbitem.TotalAlokasi = TotalAlokasi;

        }

    }
}