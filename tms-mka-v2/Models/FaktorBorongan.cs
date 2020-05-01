using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class FaktorBorongan
    {
        public int Id { get; set; }
        [Display(Name = "Alokasi Pool")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdMasterPool { get; set; }
        public string StrMasterPool { get; set; }
        [Display(Name = "Jenis Truck")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdJenisTruck { get; set; }
        public string StrJenisTruck { get; set; }
        [Display(Name = "Rasio Dalam Kota 1")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? RasioDlmKota { get; set; }
        [Display(Name = "Rasio Dalam Kota 2")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? RasioDlmKota2 { get; set; }
        [Display(Name = "Rasio Jawa Bali")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? RasioJawaBali { get; set; }
        [Display(Name = "Rasio Sumatra")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? RasioSumatra { get; set; }
        [Display(Name = "Rasio Kosong")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? RasioKosong { get; set; }
        //[Display(Name = "Harga Solar")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        //public Decimal? RasioSolar { get; set; }
        [Display(Name = "Uang Makan Jawa Bali")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? UangMakanJawaBali { get; set; }
        [Display(Name = "Uang Makan Sumatra")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? UangMakanSumatra { get; set; }
        [Display(Name = "Faktor Pengali Gaji")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? FaktorPengaliGaji { get; set; }
        [Display(Name = "Faktor Pengali Tips Parkir")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? FaktorPengaliTips { get; set; }
        [Display(Name = "Potongan Driver 1")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? PotonganDriver1 { get; set; }
        [Display(Name = "Potongan Driver 2")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? PotonganDriver2 { get; set; }
        [Display(Name = "Biaya Kapal Bali")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? BiayaKapalBali { get; set; }
        [Display(Name = "Biaya Kapal NTB")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]        
        public Decimal? BiayaKapalBaliNTB { get; set; }
        [Display(Name = "Biaya Kapal Sumatra")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? BiayaKapalSumatra { get; set; }
        [Display(Name = "Biaya Kapal Kalimantan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? BiayaKapalKalimantan { get; set; }
        [Display(Name = "Biaya Kapal Sulawesi")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? BiayaKapalSulawesi { get; set; }
        public FaktorBorongan()
        {

        }

        public FaktorBorongan(Context.FaktorBorongan dbitem)
        {
            Id = dbitem.Id;
            IdMasterPool = dbitem.IdMasterPool;
            StrMasterPool = dbitem.MasterPool.NamePool;
            IdJenisTruck = dbitem.IdJenisTruck;
            StrJenisTruck = dbitem.JenisTrucks.StrJenisTruck;
            RasioDlmKota = dbitem.RasioDlmKota;
            RasioDlmKota2 = dbitem.RasioDlmKota2;
            RasioJawaBali = dbitem.RasioJawaBali;
            RasioSumatra = dbitem.RasioSumatra;
            RasioKosong = dbitem.RasioKosong;
            //RasioSolar = dbitem.RasioSolar;
            UangMakanJawaBali = dbitem.UangMakanJawaBali;
            UangMakanSumatra = dbitem.UangMakanSumatra;
            FaktorPengaliGaji = dbitem.FaktorPengaliGaji;
            FaktorPengaliTips = dbitem.FaktorPengaliTips;
            PotonganDriver1 = dbitem.PotonganDriver1;
            PotonganDriver2 = dbitem.PotonganDriver2;
            BiayaKapalBali = dbitem.BiayaKapalBali;
            BiayaKapalBaliNTB = dbitem.BiayaKapalBaliNTB;
            BiayaKapalSumatra = dbitem.BiayaKapalSumatra;
            BiayaKapalKalimantan = dbitem.BiayaKapalKalimantan;
            BiayaKapalSulawesi = dbitem.BiayaKapalSulawesi;
        }

        public void SetDb(Context.FaktorBorongan dbitem)
        {
            dbitem.IdMasterPool = IdMasterPool;
            dbitem.IdJenisTruck = IdJenisTruck;
            dbitem.RasioDlmKota = RasioDlmKota.Value;
            dbitem.RasioDlmKota2 = RasioDlmKota2.Value;
            dbitem.RasioJawaBali = RasioJawaBali.Value;
            dbitem.RasioSumatra = RasioSumatra.Value;
            dbitem.RasioKosong = RasioKosong.Value;
            //dbitem.RasioSolar = RasioSolar.Value;
            dbitem.UangMakanJawaBali = UangMakanJawaBali.Value;
            dbitem.UangMakanSumatra = UangMakanSumatra.Value;
            dbitem.FaktorPengaliGaji = FaktorPengaliGaji.Value;
            dbitem.FaktorPengaliTips = FaktorPengaliTips.Value;
            dbitem.PotonganDriver1 = PotonganDriver1.HasValue ? PotonganDriver1.Value : 0;
            dbitem.PotonganDriver2 = PotonganDriver2.HasValue ? PotonganDriver2.Value : 0;
            dbitem.BiayaKapalBali = BiayaKapalBali.HasValue ? BiayaKapalBali.Value : 0;
            dbitem.BiayaKapalBaliNTB = BiayaKapalBaliNTB.HasValue ? BiayaKapalBaliNTB.Value : 0;
            dbitem.BiayaKapalSumatra = BiayaKapalSumatra.HasValue ? BiayaKapalSumatra.Value : 0;
            dbitem.BiayaKapalKalimantan = BiayaKapalKalimantan.HasValue ? BiayaKapalKalimantan.Value : 0;
            dbitem.BiayaKapalSulawesi = BiayaKapalSulawesi.HasValue ? BiayaKapalSulawesi.Value : 0;
        }
        public void SetDbHistory(Context.FaktorBoronganHistory dbitem, string user)
        {
            dbitem.IdMasterPool = IdMasterPool;
            dbitem.IdJenisTruck = IdJenisTruck;
            dbitem.RasioDlmKota = RasioDlmKota.Value;
            dbitem.RasioDlmKota2 = RasioDlmKota2.Value;
            dbitem.RasioJawaBali = RasioJawaBali.Value;
            dbitem.RasioSumatra = RasioSumatra.Value;
            dbitem.RasioKosong = RasioKosong.Value;
            //dbitem.RasioSolar = RasioSolar.Value;
            dbitem.UangMakanJawaBali = UangMakanJawaBali.Value;
            dbitem.UangMakanSumatra = UangMakanSumatra.Value;
            dbitem.FaktorPengaliGaji = FaktorPengaliGaji.Value;
            dbitem.FaktorPengaliTips = FaktorPengaliTips.Value;
            dbitem.PotonganDriver1 = PotonganDriver1.HasValue ? PotonganDriver1.Value : 0;
            dbitem.PotonganDriver2 = PotonganDriver2.HasValue ? PotonganDriver2.Value : 0;
            dbitem.BiayaKapalBali = BiayaKapalBali.HasValue ? BiayaKapalBali.Value : 0;
            dbitem.BiayaKapalBaliNTB = BiayaKapalBaliNTB.HasValue ? BiayaKapalBaliNTB.Value : 0;
            dbitem.BiayaKapalSumatra = BiayaKapalSumatra.HasValue ? BiayaKapalSumatra.Value : 0;
            dbitem.BiayaKapalKalimantan = BiayaKapalKalimantan.HasValue ? BiayaKapalKalimantan.Value : 0;
            dbitem.BiayaKapalSulawesi = BiayaKapalSulawesi.HasValue ? BiayaKapalSulawesi.Value : 0;
            dbitem.Tanggal = DateTime.Now;
            dbitem.username = user;
        }
    }

    public class FaktorBoronganHistory
    {
        public int Id { get; set; }
        public int IdFaktorBorongan { get; set; }
        public int? IdMasterPool { get; set; }
        public string StrMasterPool { get; set; }
        public int? IdJenisTruck { get; set; }
        public string StrJenisTruck { get; set; }
        public Decimal RasioDlmKota { get; set; }
        public Decimal RasioDlmKota2 { get; set; }
        public Decimal RasioJawaBali { get; set; }
        public Decimal RasioSumatra { get; set; }
        public Decimal RasioKosong { get; set; }
        public Decimal RasioSolar { get; set; }
        public Decimal UangMakanJawaBali { get; set; }
        public Decimal UangMakanSumatra { get; set; }
        public Decimal FaktorPengaliGaji { get; set; }
        public Decimal FaktorPengaliTips { get; set; }
        public Decimal PotonganDriver1 { get; set; }
        public Decimal PotonganDriver2 { get; set; }
        public Decimal BiayaKapalBali { get; set; }
        public Decimal BiayaKapalBaliNTB { get; set; }
        public Decimal BiayaKapalSumatra { get; set; }
        public Decimal BiayaKapalKalimantan { get; set; }
        public Decimal BiayaKapalSulawesi { get; set; }
        public DateTime Tanggal { get; set; }
        public string username { get; set; }
        public FaktorBoronganHistory()
        {

        }

        public FaktorBoronganHistory(Context.FaktorBoronganHistory dbitem)
        {
            Id = dbitem.Id;
            IdMasterPool = dbitem.IdMasterPool;
            StrMasterPool = dbitem.MasterPool.NamePool;
            IdJenisTruck = dbitem.IdJenisTruck;
            StrJenisTruck = dbitem.JenisTrucks.StrJenisTruck;
            RasioDlmKota = dbitem.RasioDlmKota;
            RasioDlmKota2 = dbitem.RasioDlmKota2;
            RasioJawaBali = dbitem.RasioJawaBali;
            RasioSumatra = dbitem.RasioSumatra;
            RasioKosong = dbitem.RasioKosong;
            //RasioSolar = dbitem.RasioSolar;
            UangMakanJawaBali = dbitem.UangMakanJawaBali;
            UangMakanSumatra = dbitem.UangMakanSumatra;
            FaktorPengaliGaji = dbitem.FaktorPengaliGaji;
            FaktorPengaliTips = dbitem.FaktorPengaliTips;
            PotonganDriver1 = dbitem.PotonganDriver1;
            PotonganDriver2 = dbitem.PotonganDriver2;
            BiayaKapalBali = dbitem.BiayaKapalBali;
            BiayaKapalBaliNTB = dbitem.BiayaKapalBaliNTB;
            BiayaKapalSumatra = dbitem.BiayaKapalSumatra;
            BiayaKapalKalimantan = dbitem.BiayaKapalKalimantan;
            BiayaKapalSulawesi = dbitem.BiayaKapalSulawesi;
            Tanggal = dbitem.Tanggal;
            username = dbitem.username;
        }
    }
}