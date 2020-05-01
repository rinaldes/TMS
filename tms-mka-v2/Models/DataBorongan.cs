using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DataBorongan
    {
        public int Id { get; set; }
        [Display(Name = "Tambahan Rute Muat")]
        public bool IsTambahan { get; set; }
        [Display(Name = "Alokasi Pool")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdMasterPool { get; set; }
        public string StrMasterPool { get; set; }
        [Display(Name = "Jenis Truck")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdJenisTruck { get; set; }
        public string StrJenisTruck { get; set; }
        [Display(Name = "Nama Borongan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaBorongan { get; set; }
        [Display(Name = "Jarak")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public decimal? Jarak { get; set; }
        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string CustomerName { get; set; }
        public string KodeNama { get; set; }
        [Display(Name = "Rasio")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Rasio { get; set; }
        [Display(Name = "Solar (Ltr)")]
        public Decimal? LiterSolar { get; set; }
        [Display(Name = "Solar (Rp)")]
        public Decimal? HargaSolar { get; set; }
        [Display(Name = "Waktu Kerja (hari)")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? WaktuHariKerja { get; set; }
        [Display(Name = "Jumlah Makan (kali)")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? JumlahMakan { get; set; }
        [Display(Name = "Area Uang Makan")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string AreaUangMakan { get; set; }
        [Display(Name = "Uang Makan (Rp)")]
        public Decimal? UangMakan { get; set; }
        [Display(Name = "Biaya Tol")]
        public Decimal? BiayaTol { get; set; }
        [Display(Name = "Bobot Tips Parkir")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? BobotTipsParkir { get; set; }
        [Display(Name = "Tips Parkir")]
        public Decimal? TipsParkir { get; set; }
        [Display(Name = "Bobot Gaji 1")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? BobotGaji1 { get; set; }
        [Display(Name = "Gaji 1")]
        public Decimal? gaji1 { get; set; }
        [Display(Name = "Bobot Gaji 2")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? BobotGaji2 { get; set; }
        [Display(Name = "Gaji 2")]
        public Decimal? gaji2 { get; set; }
        [Display(Name = "Total Gaji")]
        public Decimal? TotalGaji { get; set; }
        [Display(Name = "Biaya Kapal")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Kapal { get; set; }
        public Decimal? BiayaKapal { get; set; }
        [Display(Name = "Borongan Dasar")]
        public Decimal? BoronganDasar { get; set; }
        [Display(Name = "Kawalan")]
        public Decimal? Kawalan { get; set; }
        [Display(Name = "Timbangan")]
        public Decimal? Timbangan { get; set; }
        [Display(Name = "Karantina")]
        public Decimal? Karantina { get; set; }
        [Display(Name = "SPSI")]
        public Decimal? SPSI { get; set; }
        [Display(Name = "Multidrop")]
        public Decimal? MultiDrop { get; set; }
        [Display(Name = "Total Borongan")]
        public Decimal? TotalBorongan { get; set; }
        [Display(Name = "Pembulatan")]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? Pembulatan { get; set; }
        public DateTime? Tanggal { get; set; }
        public Decimal? AlokasiCash { get; set; }
        public Decimal? TotalAlokasiPembulatan { get; set; }

        public List<DataBoronganRute> listRuteBorongan { get; set; }
        public List<DataBoronganSPBU> listSpbuBorongan { get; set; }
        public List<DataBoronganKapal> listKapalBorongan { get; set; }
        public List<DataBoronganTf> listTfBorongan { get; set; }
        public string JsonIdRute { get; set; }
        public DataBorongan()
        {

        }
        public DataBorongan(Context.DataBorongan dbitem)
        {
            Id = dbitem.Id;
            IsTambahan = dbitem.IsTambahan;
            IdMasterPool = dbitem.IdMasterPool;
            StrMasterPool = dbitem.IdMasterPool.HasValue ? dbitem.MasterPool.NamePool : "";
            IdJenisTruck = dbitem.IdJenisTruck;
            StrJenisTruck = dbitem.IdJenisTruck.HasValue ? dbitem.JenisTrucks.StrJenisTruck : "";
            NamaBorongan = dbitem.NamaBorongan;
            Jarak = dbitem.Jarak;
            CustomerId = dbitem.CustomerId;
            CustomerName = dbitem.CustomerId == null ? "" : dbitem.Customer.CustomerNama;
            KodeNama = dbitem.CustomerId == null ? "" : dbitem.Customer.CustomerCodeOld;
            Rasio = dbitem.Rasio;
            LiterSolar = dbitem.LiterSolar;
            HargaSolar = dbitem.HargaSolar;
            WaktuHariKerja = dbitem.WaktuHariKerja;
            JumlahMakan = dbitem.JumlahMakan;
            AreaUangMakan = dbitem.AreaUangMakan;
            UangMakan = dbitem.UangMakan;
            BiayaTol = dbitem.BiayaTol;
            BobotTipsParkir = dbitem.BobotTipsParkir;
            TipsParkir = dbitem.TipsParkir;
            BobotGaji1 = dbitem.BobotGaji1;
            gaji1 = dbitem.gaji1;
            BobotGaji2 = dbitem.BobotGaji2;
            gaji2 = dbitem.gaji2;
            TotalGaji = dbitem.TotalGaji;
            Kapal = dbitem.Kapal;
            BiayaKapal = dbitem.BiayaKapal;
            BoronganDasar = dbitem.BoronganDasar;
            Kawalan = dbitem.Kawalan;
            Timbangan = dbitem.Timbangan;
            Karantina = dbitem.Karantina;
            SPSI = dbitem.SPSI;
            MultiDrop = dbitem.MultiDrop;
            TotalBorongan = dbitem.TotalBorongan;
            Pembulatan = dbitem.Pembulatan;

            listRuteBorongan = new List<DataBoronganRute>();
            foreach (Context.DataBoronganRute item in dbitem.DataBoronganRute)
            {
                listRuteBorongan.Add(new DataBoronganRute() { 
                    Id = item.IdRute.Value,
                    Nama = item.Rute.Nama,
                    Dari = item.Rute.LocationAsal.Nama,
                    Tujuan = item.Rute.LocationTujuan.Nama,
                    MultiDrop = item.Rute.Multidrop == null ? "" : item.Rute.Multidrop.tujuan
                });                
            }
            AlokasiCash = dbitem.AlokasiCash;
            TotalAlokasiPembulatan = dbitem.TotalAlokasiPembulatan;
        }
        public DataBorongan(Context.DataBoronganHistory dbitem)
        {
            Id = dbitem.Id;
            Tanggal = dbitem.Tanggal;
            IsTambahan = dbitem.IsTambahan;
            NamaBorongan = dbitem.NamaBorongan;
            Jarak = dbitem.Jarak;
            CustomerName = dbitem.Customer;
            Rasio = dbitem.Rasio;
            LiterSolar = dbitem.LiterSolar;
            HargaSolar = dbitem.HargaSolar;
            WaktuHariKerja = dbitem.WaktuHariKerja;
            JumlahMakan = dbitem.JumlahMakan;
            AreaUangMakan = dbitem.AreaUangMakan;
            UangMakan = dbitem.UangMakan;
            BiayaTol = dbitem.BiayaTol;
            BobotTipsParkir = dbitem.BobotTipsParkir;
            TipsParkir = dbitem.TipsParkir;
            BobotGaji1 = dbitem.BobotGaji1;
            gaji1 = dbitem.gaji1;
            BobotGaji2 = dbitem.BobotGaji2;
            gaji2 = dbitem.gaji2;
            TotalGaji = dbitem.TotalGaji;
            Kapal = dbitem.Kapal;
            BiayaKapal = dbitem.BiayaKapal;
            BoronganDasar = dbitem.BoronganDasar;
            Kawalan = dbitem.Kawalan;
            Timbangan = dbitem.Timbangan;
            Karantina = dbitem.Karantina;
            SPSI = dbitem.SPSI;
            MultiDrop = dbitem.MultiDrop;
            TotalBorongan = dbitem.TotalBorongan;
            Pembulatan = dbitem.Pembulatan;
        }
        public void SetDb(Context.DataBorongan dbitem)
        {
            dbitem.IsTambahan = IsTambahan;
            dbitem.IdMasterPool = IdMasterPool;
            dbitem.IdJenisTruck = IdJenisTruck;
            dbitem.NamaBorongan = NamaBorongan;
            dbitem.Jarak = Jarak;
            dbitem.CustomerId = CustomerId;
            dbitem.Rasio = Rasio;
            dbitem.LiterSolar = LiterSolar;
            dbitem.HargaSolar = HargaSolar.HasValue ? HargaSolar.Value : 0;
            dbitem.WaktuHariKerja = WaktuHariKerja;
            dbitem.JumlahMakan = JumlahMakan;
            dbitem.AreaUangMakan = AreaUangMakan;
            dbitem.UangMakan = UangMakan.HasValue ? UangMakan.Value : 0;
            dbitem.BiayaTol = BiayaTol.HasValue ? BiayaTol.Value : 0;
            dbitem.BobotTipsParkir = BobotTipsParkir;
            dbitem.TipsParkir = TipsParkir.HasValue ? TipsParkir.Value : 0;
            dbitem.BobotGaji1 = BobotGaji1;
            dbitem.gaji1 = gaji1.HasValue ? gaji1.Value : 0;
            dbitem.BobotGaji2 = BobotGaji2;
            dbitem.gaji2 = gaji2.HasValue ? gaji1.Value : 0;
            dbitem.TotalGaji = TotalGaji.HasValue ? TotalGaji.Value : 0;
            dbitem.Kapal = Kapal;
            dbitem.BiayaKapal = BiayaKapal.HasValue ? BiayaKapal.Value : 0;
            dbitem.BoronganDasar = BoronganDasar;
            dbitem.Kawalan = Kawalan;
            dbitem.Timbangan = Timbangan;
            dbitem.Karantina = Karantina;
            dbitem.SPSI = SPSI.HasValue ? SPSI.Value : 0;
            dbitem.MultiDrop = MultiDrop;
            dbitem.TotalBorongan = TotalBorongan.HasValue ? TotalBorongan.Value : 0;
            dbitem.Pembulatan = Pembulatan.HasValue ? Pembulatan.Value : 0;

            dbitem.DataBoronganRute.Clear();
            if (JsonIdRute != "" && JsonIdRute != null)
            {
                foreach (string i in JsonIdRute.Split(','))
                {
                    dbitem.DataBoronganRute.Add(new Context.DataBoronganRute()
                    {
                        IdRute = int.Parse(i.Replace("a", ""))
                    });
                }
            }

            dbitem.DataBoronganSPBU.Clear();
            foreach (DataBoronganSPBU item in listSpbuBorongan.Where(d => d.IdSPBU != 0 && d.IdSPBU.HasValue))
            {
                dbitem.DataBoronganSPBU.Add(new Context.DataBoronganSPBU() { IdLookupCodeSpbu = item.IdSPBU, value = item.value });
            }

            dbitem.DataBoronganKapal.Clear();
            foreach (DataBoronganKapal item in listKapalBorongan.Where(d => d.IdKapal != 0 && d.IdKapal.HasValue))
            {
                dbitem.DataBoronganKapal.Add(new Context.DataBoronganKapal() { IdLookupCodeKapal = item.IdKapal, value = item.value });
            }
            dbitem.DataBoronganTf.Clear();
            foreach (DataBoronganTf item in listTfBorongan)
            {
                dbitem.DataBoronganTf.Add(new Context.DataBoronganTf() { value = item.value , LeadTime = item.LeadTime});
            }

            dbitem.AlokasiCash = AlokasiCash;
            dbitem.TotalAlokasiPembulatan = TotalAlokasiPembulatan;
        }
        public void SetDbHistory(Context.DataBoronganHistory dbitem)
        {
            dbitem.IsTambahan = IsTambahan;
            dbitem.Tanggal = DateTime.Now;
            dbitem.NamaBorongan = NamaBorongan;
            dbitem.Jarak = Jarak;
            dbitem.Customer = CustomerName;
            dbitem.Rasio = Rasio;
            dbitem.LiterSolar = LiterSolar;
            dbitem.HargaSolar = HargaSolar;
            dbitem.WaktuHariKerja = WaktuHariKerja;
            dbitem.JumlahMakan = JumlahMakan;
            dbitem.AreaUangMakan = AreaUangMakan;
            dbitem.UangMakan = UangMakan;
            dbitem.BiayaTol = BiayaTol;
            dbitem.BobotTipsParkir = BobotTipsParkir;
            dbitem.TipsParkir = TipsParkir;
            dbitem.BobotGaji1 = BobotGaji1;
            dbitem.gaji1 = gaji1;
            dbitem.BobotGaji2 = BobotGaji2;
            dbitem.gaji2 = gaji2;
            dbitem.TotalGaji = TotalGaji;
            dbitem.Kapal = Kapal;
            dbitem.BiayaKapal = BiayaKapal;
            dbitem.BoronganDasar = BoronganDasar;
            dbitem.Kawalan = Kawalan;
            dbitem.Timbangan = Timbangan;
            dbitem.Karantina = Karantina;
            dbitem.SPSI = SPSI;
            dbitem.MultiDrop = MultiDrop;
            dbitem.TotalBorongan = TotalBorongan;
            dbitem.Pembulatan = Pembulatan;
        }
    }
    public class DataBoronganRute
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Dari { get; set; }
        public string Tujuan { get; set; }
        public string MultiDrop { get; set; }
    }
    public class DataBoronganSPBU
    {
        public int Id { get; set; }
        public int? IdSPBU { get; set; }
        public string NamaSpbu { get; set; }
        public Decimal? value { get; set; }
    }
    public class DataBoronganKapal
    {
        public int Id { get; set; }
        public int? IdKapal { get; set; }
        public string NamaPenyebrangan { get; set; }
        public Decimal? value { get; set; }
    }
    public class DataBoronganTf
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public Decimal? value { get; set; }
        public int? LeadTime { get; set; }
    }
}