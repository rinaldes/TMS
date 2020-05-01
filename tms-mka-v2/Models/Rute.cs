using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace tms_mka_v2.Models
{
    public class Rute
    {
        public int Id { get; set; }
        [Display(Name = "Customer")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? CustomerId { get; set; }
        public string strCustomer { get; set; }
        [Display(Name = "Kode Rute")]
        public string Kode { get; set; }
        [Display(Name = "Nama Rute")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Nama { get; set; }
        [Display(Name = "Asal")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdAsal { get; set; }
        public string Asal { get; set; }
        [Display(Name = "Area Asal")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdAreaAsal { get; set; }
        public string AreaAsal { get; set; }
        [Display(Name = "Tujuan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdTujuan { get; set; }
        public string Tujuan { get; set; }
        [Display(Name = "Area Tujuan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdAreaTujuan { get; set; }
        public string AreaTujuan { get; set; }
        [Display(Name = "Multidrop")]
        public int? IdMultiDrop { get; set; }
        public string MultiDrop { get; set; }
        [Display(Name = "Jarak")]
        public Decimal? Jarak { get; set; }
        [Display(Name = "Waktu Kerja")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? WaktuKerja { get; set; }
        [Display(Name = "Waktu Tempuh")]
//        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? WaktuTempuhJam { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? WaktuTempuhMenit { get; set; }
        public string strWaktuTempuh { get; set; }
        [Display(Name = "Toleransi Delay")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Toleransi { get; set; }
        [Display(Name = "Area Pulang")]
        public bool IsAreaPulang { get; set; }
        [Display(Name = "Kota-Kota")]
        public bool IsKotaKota { get; set; }
        [Display(Name = "Keterangan")]
        public string Keterangan { get; set; }
        public string StrListCheckPoint { get; set; }
        public List<RuteCheckPoint> listCheckPoint { get; set; }

        public Rute()
        {
            listCheckPoint = new List<RuteCheckPoint>();
        }
        public Rute(Context.Rute dbitem)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            strCustomer = dbitem.CustomerId == null ? "UMUM" : dbitem.Customer.CustomerNama;  
            Kode = dbitem.Kode;
            Nama = dbitem.Nama;
            IdAsal = dbitem.IdAsal;
            Asal = dbitem.LocationAsal.Nama;
            IdAreaAsal = dbitem.IdAreaAsal;
            AreaAsal = dbitem.AreaAsal.Nama;
            IdTujuan = dbitem.IdTujuan;
            Tujuan = dbitem.LocationTujuan.Nama;
            IdAreaTujuan = dbitem.IdAreaTujuan;
            AreaTujuan = dbitem.AreaTujuan.Nama;
            if (dbitem.Multidrop != null)
            {
                IdMultiDrop = dbitem.IdMultiDrop;
                MultiDrop = dbitem.Multidrop.tujuan;
            }
            Jarak = dbitem.Jarak;
            WaktuKerja = dbitem.WaktuKerja;
            WaktuTempuhJam = dbitem.WaktuTempuhJam;
            WaktuTempuhMenit = dbitem.WaktuTempuhMenit;
            string wktjam = WaktuTempuhJam.HasValue ? WaktuTempuhJam.Value.ToString() : "0";
            string wktmenit = WaktuTempuhMenit.HasValue ? WaktuTempuhMenit.Value.ToString() : "0";
            strWaktuTempuh = wktjam + " Jam " + wktmenit + " menit";
            Toleransi = dbitem.Toleransi;
            IsAreaPulang = dbitem.IsAreaPulang;
            IsKotaKota = dbitem.IsKotaKota;
            Keterangan = dbitem.Keterangan;

            listCheckPoint = new List<RuteCheckPoint>();

            foreach (Context.RuteCheckPoint item in dbitem.RuteCheckPoint)
            {
                listCheckPoint.Add(new RuteCheckPoint(item));
            }
        }
        public void setDb(Context.Rute dbitem)
        {
            dbitem.CustomerId = CustomerId;
            dbitem.Kode = Kode;
            dbitem.Nama = Nama;
            dbitem.IdAsal = IdAsal;
            dbitem.IdAreaAsal = IdAreaAsal;
            dbitem.IdTujuan = IdTujuan;
            dbitem.IdAreaTujuan = IdAreaTujuan;
            dbitem.IdMultiDrop = IdMultiDrop;
            dbitem.Jarak = Jarak.Value;
            dbitem.WaktuKerja = WaktuKerja.Value;
            dbitem.WaktuTempuhJam = WaktuTempuhJam;
            dbitem.WaktuTempuhMenit = WaktuTempuhMenit;
            dbitem.Toleransi = Toleransi;
            dbitem.IsAreaPulang = IsAreaPulang;
            dbitem.IsKotaKota = IsKotaKota;
            dbitem.Keterangan = Keterangan;

            dbitem.RuteCheckPoint.Clear();

            if (StrListCheckPoint != null)
            {
                RuteCheckPoint[] result = JsonConvert.DeserializeObject<RuteCheckPoint[]>(StrListCheckPoint);

                foreach (RuteCheckPoint item in result)
                {
                    dbitem.RuteCheckPoint.Add(new Context.RuteCheckPoint()
                    {
                        code = item.code,
                        longitude = item.longitude,
                        langitude = item.langitude,
                        radius = item.radius,
                        alamat = item.alamat,
                        waktuJam = item.waktuJam,
                        waktuMenit = item.waktuMenit,
                        toleransi = item.toleransi,
                        hapus = item.hapus
                    });
                }
            }
        }
    }
    public class RuteCheckPoint
    {
        public int Id { get; set; }
        public int IdRute { get; set; }
        public string code { get; set; }
        public string longitude { get; set; }
        public string langitude { get; set; }
        public int radius { get; set; }
        public string alamat { get; set; }
        public int? waktuJam { get; set; }
        public int? waktuMenit { get; set; }
        public int toleransi { get; set; }
        public string hapus { get; set; }
        public RuteCheckPoint()
        {

        }
        public RuteCheckPoint(Context.RuteCheckPoint dbitem)
        {
            Id = dbitem.Id;
            IdRute = dbitem.IdRute;
            code = dbitem.code;
            longitude = dbitem.longitude;
            langitude = dbitem.langitude;
            radius = dbitem.radius;
            alamat = dbitem.alamat;
            waktuJam = dbitem.waktuJam;
            waktuMenit = dbitem.waktuMenit;
            toleransi = dbitem.toleransi;
            hapus = dbitem.hapus;
        }
    }
}