using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class MasterPool
    {
        public int Id { get; set; }
        public string strPool { get; set; }
        [Display(Name = "Is Active")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public bool IsActive { get; set; }
        //public bool IsDelete { get; set; }
        [Display(Name = "Nama Pool")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamePool { get; set; }
        [Display(Name = "Kapasitas")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Capacity{ get; set; }
        [Display(Name = "Alamat")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Address { get; set; }
        [Display(Name = "Provinsi")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdProvinsi { get; set; }
        public string NameProv { get; set; }
        [Display(Name = "Kabupaten / Kota")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKabKota { get; set; }
        public string NameKabKot { get; set; }
        [Display(Name = "Kecamatan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKec { get; set; }
        public string NameKec { get; set; }
        [Display(Name = "Kelurahan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKel { get; set; }
        public string NameKel { get; set; }
        [Display(Name = "Longitude")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Longitude { get; set; }
        [Display(Name = "Latitude")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Latitude { get; set; }
        [Display(Name = "Radius")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Radius { get; set; }
        [Display(Name = "KodeTelp")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string KodeTelp { get; set; }
        [Display(Name = "Telp")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Telp { get; set; }
        [Display(Name = "PIC")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Pic { get; set; }
        [Display(Name = "Handphone")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Handphone { get; set; }
        public string IpAddress { get; set; }
        public int? IdCreditCash { get; set; }
        public string NamaCreditCash { get; set; }
        public string CodeCreditCash { get; set; }
        [Display(Name = "Zone")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public List<ZoneParkir> ListZoneParkir { get; set; }
        public MasterPool()
        {
            ListZoneParkir = new List<ZoneParkir>();
        }
        public MasterPool(Context.MasterPool dbitem)
        {
            Id = dbitem.Id;
            IsActive = dbitem.IsActive;
            NamePool = dbitem.NamePool;
            Capacity = dbitem.Capacity;
            Address = dbitem.Address;
            IdProvinsi = dbitem.IdProvinsi;
            IdKabKota = dbitem.IdKabKota;
            IdKec = dbitem.IdKec;
            IdKel = dbitem.IdKel;
            NameProv = dbitem.IdProvinsi != null ? dbitem.LocProvinsi.Nama : "";
            NameKec = dbitem.IdKec != null ? dbitem.LocKecamatan.Nama : "";
            NameKabKot = dbitem.IdKabKota != null ? dbitem.LocKabKota.Nama : "";
            NameKel = dbitem.IdKel != null ? dbitem.LocKelurahan.Nama : "";
            Latitude = dbitem.Latitude;
            Longitude = dbitem.Longitude;
            Radius = dbitem.Radius;
            KodeTelp = dbitem.KodeTelp;
            Telp = dbitem.Telp;
            Pic = dbitem.Pic;
            Handphone = dbitem.Handphone;
            IpAddress = dbitem.IpAddress;
            ListZoneParkir = new List<ZoneParkir>();
            IdCreditCash = dbitem.IdCreditCash;

            foreach (Context.ZoneParkir item in dbitem.ListZoneParkir.ToList())
            {
                ListZoneParkir.Add(new ZoneParkir(item));
            }
        }
        public void setDb(Context.MasterPool dbitem)
        {
            dbitem.Id = Id;
            dbitem.IsActive = IsActive;
            dbitem.NamePool = NamePool;
            dbitem.Capacity = Capacity;
            dbitem.Address = Address;
            dbitem.IdProvinsi = IdProvinsi;
            dbitem.IdKabKota = IdKabKota;
            dbitem.IdKec = IdKec;
            dbitem.IdKel = IdKel;
            dbitem.Latitude = Latitude;
            dbitem.Longitude = Longitude;
            dbitem.Radius = Radius;
            dbitem.KodeTelp = KodeTelp;
            dbitem.Telp = Telp != null ? Telp.Replace('-',' ').Replace('_',' '): Telp;
            dbitem.Pic = Pic;
            dbitem.Handphone = Handphone != null ? Handphone.Replace('-', ' ').Replace('_', ' ') : Handphone;
            dbitem.IpAddress = IpAddress;
            dbitem.IdCreditCash = IdCreditCash;

            dbitem.ListZoneParkir.Clear();

            ZoneParkir[] result = JsonConvert.DeserializeObject<ZoneParkir[]>(strPool);

            foreach (ZoneParkir item in result)
            {
                dbitem.ListZoneParkir.Add(new Context.ZoneParkir()
                {
                    IdZone = item.IdZone, Pit = item.Pit
                });
            }
        }
    }
    public class ZoneParkir
    {
        public int Id { get; set; }
        public int IdZone { get; set; }
        public string NameZone { get; set; }
        public int Pit { get; set; }

        public ZoneParkir()
        {

        }

        public ZoneParkir(Context.ZoneParkir dbitem)
        {
            Id = dbitem.Id;
            IdZone = dbitem.Id;
            NameZone = dbitem.LookUpCodeZone.Nama;
            Pit = dbitem.Pit;
        }
    }
}