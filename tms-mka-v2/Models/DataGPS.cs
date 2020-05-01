using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DataGPS
    {
        public int Id { get; set; }
        [Display(Name = "No GPS")]
        public string NoGPS { get; set; }
        [Display(Name = "Vehicle No")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdDataTruck { get; set; }
        public string VehicleNo { get; set; }
        [Display(Name = "Merk")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdVendor { get; set; }
        public string Vendor { get; set; }
        [Display(Name = "Model GPS")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string ModelGps { get; set; }
        [Display(Name = "No Device")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string NoDevice { get; set; }
        [Display(Name = "Sensor Suhu")]
        public bool? SensorSuhu { get; set; }
        [Display(Name = "Sensor Pintu")]
        public bool? SensorPintu { get; set; }
        [Display(Name = "Tahun")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Tahun { get; set; }
        [Display(Name = "Tanggal Pasang")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TanggalPasang { get; set; }
        [Display(Name = "Garansi s/d")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TanggalGaransi { get; set; }
        public DateTime? Tanggal { get; set; }
        public string username { get; set; }
        public DataGPS()
        {
            
        }

        public DataGPS(Context.DataGPS dbitem)
        {
            Id = dbitem.Id;
            NoGPS = dbitem.NoGPS;
            IdDataTruck = dbitem.IdDataTruck;
            VehicleNo = dbitem.DataTruck.VehicleNo;
            IdVendor = dbitem.IdVendor;
            Vendor = dbitem.IdVendor != null ? dbitem.VendorGps.Nama : "";
            ModelGps = dbitem.ModelGps;
            NoDevice = dbitem.NoDevice;
            SensorSuhu = dbitem.SensorSuhu;
            SensorPintu = dbitem.SensorPintu;
            Tahun = dbitem.Tahun;
            TanggalPasang = dbitem.TanggalPasang;
            TanggalGaransi = dbitem.TanggalGaransi;
        }
        public DataGPS(Context.DataGPSHistory dbitem)
        {
            Id = dbitem.Id;
            NoGPS = dbitem.DataGPS.NoGPS;
            VehicleNo = dbitem.Vehicle;
            Vendor = dbitem.strVendor;
            ModelGps = dbitem.ModelGps;
            NoDevice = dbitem.NoDevice;
            SensorSuhu = dbitem.SensorSuhu;
            SensorPintu = dbitem.SensorPintu;
            Tahun = dbitem.Tahun;
            TanggalPasang = dbitem.TanggalPasang;
            TanggalGaransi = dbitem.TanggalGaransi;
            Tanggal = dbitem.Tanggal;
            username = dbitem.Username;
        }
        public DataGPS(Context.DataTruckGPSHistory dbitem)
        {
            Id = dbitem.Id;
            NoGPS = dbitem.NoGPS;
            VehicleNo = dbitem.Vehicle;
            Vendor = dbitem.strVendor;
            ModelGps = dbitem.ModelGps;
            NoDevice = dbitem.NoDevice;
            SensorSuhu = dbitem.SensorSuhu;
            SensorPintu = dbitem.SensorPintu;
            Tahun = dbitem.Tahun;
            TanggalPasang = dbitem.TanggalPasang;
            TanggalGaransi = dbitem.TanggalGaransi;
            Tanggal = dbitem.Tanggal;
            username = dbitem.Username;
        }
        public void SetDb(Context.DataGPS dbitem)
        {
            dbitem.NoGPS = NoGPS;
            dbitem.IdDataTruck = IdDataTruck;
            dbitem.IdVendor = IdVendor;
            dbitem.ModelGps = ModelGps;
            dbitem.NoDevice = NoDevice;
            dbitem.SensorSuhu = SensorSuhu;
            dbitem.SensorPintu = SensorPintu;
            dbitem.Tahun = Tahun;
            dbitem.TanggalPasang = TanggalPasang;
            dbitem.TanggalGaransi = TanggalGaransi;
        }
        public void SetDbHistory(Context.DataGPSHistory dbitem, string user)
        {
            dbitem.NoGPS = NoGPS;
            dbitem.Vehicle = VehicleNo;
            dbitem.strVendor = Vendor;
            dbitem.ModelGps = ModelGps;
            dbitem.NoDevice = NoDevice;
            dbitem.SensorSuhu = SensorSuhu;
            dbitem.SensorPintu = SensorPintu;
            dbitem.Tahun = Tahun;
            dbitem.TanggalPasang = TanggalPasang;
            dbitem.TanggalGaransi = TanggalGaransi;
            dbitem.Tanggal = DateTime.Now;
            dbitem.Username = user;
        }
        public void SetDbTruckHistory(Context.DataTruckGPSHistory dbitem, string user)
        {
            dbitem.NoGPS = NoGPS;
            dbitem.Vehicle = VehicleNo;
            dbitem.strVendor = Vendor;
            dbitem.ModelGps = ModelGps;
            dbitem.NoDevice = NoDevice;
            dbitem.SensorSuhu = SensorSuhu;
            dbitem.SensorPintu = SensorPintu;
            dbitem.Tahun = Tahun;
            dbitem.TanggalPasang = TanggalPasang;
            dbitem.TanggalGaransi = TanggalGaransi;
            dbitem.Tanggal = DateTime.Now;
            dbitem.Username = user;
        }
    }
}