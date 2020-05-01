using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;
using System.Web.Script.Serialization;

namespace tms_mka_v2.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Display(Name = "Kode Customer")]
        public string CustomerCode { get; set; }
        [Display(Name = "Kode Nama")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string CustomerCodeOld { get; set; }
        [Display(Name = "Nama Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression("[a-zA-Z0-9., ]+", ErrorMessage = "Format Nama Customer tidak valid")]
        public string CustomerNama { get; set; }
        [Display(Name = "Prioritas")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? PrioritasId { get; set; }
        public string StrPrioritas { get; set; }
        [Display(Name = "Wajib PO")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public bool WajibPO { get; set; }
        [Display(Name = "Wajib GPS")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public bool WajibGPS { get; set; }
        [Display(Name = "PIC")]
        public int? CustomerPicId { get; set; }
        [Display(Name = "Penanganan Khusus")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression("[a-zA-Z0-9&()-_,. ]+", ErrorMessage = "Format Penanganan Khusus tidak valid")]
        public string SpecialTreatment { get; set; }
        [Display(Name = "Keterangan")]
        [RegularExpression("[a-zA-Z0-9&()-_,. ]+", ErrorMessage = "Format Penanganan Khusus tidak valid")]
        public string Keterangan { get; set; }
        //ppn
        public int IdPPN { get; set; }
        [Display(Name = "PPN")]
        public bool IsPPn { get; set; }
        public bool IsVendor { get; set; }
        [Display(Name = "Rekening")]
        public int? IdRekening { get; set; }
        [Display(Name = "Nomor NPWP")]
        public string NoNpwp { get; set; }
        [Display(Name = "Nama NPWP")]
        public string NamaNpwp { get; set; }
        [Display(Name = "Alamat NPWP")]
        public string AddressNpwp { get; set; }
        public string CustomerFullName { get; set; }
        //credit status
        public int IdCreditStatus { get; set; }
        public string StatusSystem { get; set; }
        public string StatusOveride { get; set; }
        public string KeteranganCS { get; set; }
        public string ConditionCS { get; set; }
        public int? MinTOPOverdue1 { get; set; }
        public int? MaxTOPOverdue1 { get; set; }
        public int? ValueOverdue2 { get; set; }
        public int? TOPOverdue2 { get; set; }
        public int? ShipmentDay1 { get; set; }
        public int? ShipmentDay2 { get; set; }

        public List<Context.CustomerPic> ListPic { get; set; }
        public List<Context.CustomerAddress> ListAddress { get; set; }
        public List<Context.CustomerLoadingAddress> ListLoadingAddress { get; set; }
        public List<Context.CustomerUnloadingAddress> ListUnLoadingAddress { get; set; }
        public List<Context.CustomerProductType> ListProduct { get; set; }
        public List<Context.LookupCode> ListTreatment { get; set; }
        public List<Context.LookupCode> ListWarna { get; set; }
        public List<Context.JenisTrucks> ListTruck { get; set; }
        public List<Context.CustomerTypeTrucks> ListCustTruck { get; set; }
        public List<Context.CustomerBilling> ListBilling { get; set; }
        public List<Context.CustomerSupplier> ListSupplier { get; set; }
        public List<Context.CustomerNotification> ListNotif { get; set; }
        public List<CustTruckType> ListCustTruckType { get; set; }
        public Customer()
        {

        }
        public Customer(Context.Customer dbitem)
        {
            Id = dbitem.Id;
            IsVendor = dbitem.IsVendor;
            CustomerCode = dbitem.CustomerCode;
            CustomerCodeOld = dbitem.CustomerCodeOld;
            CustomerNama = dbitem.CustomerNama;
            PrioritasId = dbitem.PrioritasId;
            StrPrioritas = dbitem.LookupCode.Nama;
            WajibPO = dbitem.WajibPO;
            WajibGPS = dbitem.WajibGPS;
            SpecialTreatment = dbitem.SpecialTreatment;
            Keterangan = dbitem.Keterangan;
            CustomerPicId = dbitem.CustomerPicId;


            ListPic = dbitem.CustomerPic.ToList();
            ListAddress = dbitem.CustomerAddress.ToList();
            ListProduct = dbitem.CustomerProductType.ToList();
            ListLoadingAddress = dbitem.CustomerLoadingAddress.ToList();
            ListUnLoadingAddress = dbitem.CustomerUnloadingAddress.ToList();
            ListSupplier = dbitem.CustomerSupplier.ToList();
            ListBilling = dbitem.CustomerBilling.ToList();
            ListNotif = dbitem.CustomerNotification.ToList();
            CustomerFullName = dbitem.CustomerCodeOld + " - " + dbitem.CustomerNama;

            ////ppn
            if (dbitem.CustomerPPN.Count() > 0)
            {
                Context.CustomerPPN ppnItem = dbitem.CustomerPPN.FirstOrDefault();
                IdPPN = ppnItem.Id;
                IsPPn = ppnItem.PPN;
                IdRekening = ppnItem.IdRekening;
                NoNpwp = ppnItem.NomorNPWP;
                NamaNpwp = ppnItem.NamaNPWP;
                AddressNpwp = ppnItem.AddressNPWP;
            }
            else
            {
                IsPPn = true;
            }
            //cs
            if(dbitem.CustomerCreditStatus.Count() > 0)
            {
                Context.CustomerCreditStatus dbcs = dbitem.CustomerCreditStatus.FirstOrDefault();
                IdCreditStatus = dbcs.Id;
                StatusSystem = dbcs.StatusSystem;
                StatusOveride = dbcs.StatusOveride;
                KeteranganCS = dbcs.Keterangan;
                ConditionCS = dbcs.Condition;
                MinTOPOverdue1 = dbcs.MinTOPOverdue1;
                MaxTOPOverdue1 = dbcs.MaxTOPOverdue1;
                ValueOverdue2 = dbcs.ValueOverdue2;
                TOPOverdue2 = dbcs.TOPOverdue2;
                ShipmentDay1 = dbcs.ShipmentDay1;
                ShipmentDay2 = dbcs.ShipmentDay2;
            }
        }
        public void setDb(Context.Customer dbitem)
        {
            dbitem.Id = Id;
            dbitem.CustomerCode = CustomerCode;
            dbitem.CustomerCodeOld = CustomerCodeOld;
            dbitem.CustomerNama = CustomerNama;
            dbitem.PrioritasId = PrioritasId.Value;
            dbitem.WajibPO = bool.Parse(WajibPO.ToString());
            dbitem.WajibGPS = bool.Parse(WajibGPS.ToString());
            dbitem.IsVendor = bool.Parse(IsVendor.ToString());
            dbitem.SpecialTreatment = SpecialTreatment;
            dbitem.Keterangan = Keterangan;
            dbitem.CustomerPicId = CustomerPicId;
        }
    }
    public class CustPIC
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public string Nama { get; set; }
        public int? DepartemenId { get; set; }
        public string Dept { get; set; }
        public int? JabatanId { get; set; }
        public string Jabatan { get; set; }
        public string EmailAdd { get; set; }
        public string Mobile { get; set; }
        public int Urutan { get; set; }

        public CustPIC()
        {

        }
        public CustPIC(Context.CustomerPic dbitem)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            Code = dbitem.Code;
            Nama = dbitem.Name;
            DepartemenId = dbitem.DepartemenId;
            Dept = dbitem.LookUpCodesDept.Nama;
            JabatanId = dbitem.JabatanId;
            Jabatan = dbitem.LookUpCodesJabatan.Nama;
            EmailAdd = dbitem.EmailAdd;
            Mobile = dbitem.Mobile;
        }
    }
    public class CustAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public string Alamat { get; set; }
        public int? IdProvinsi { get; set; }
        public string provinsi { get; set; }
        public int? IdKabKota { get; set; }
        public string kota { get; set; }
        public int? IdKec { get; set; }
        public string kecamatan { get; set; }
        public int? IdKel { get; set; }
        public string kelurahan { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? Radius { get; set; }
        public string Zona { get; set; }
        public int? OfficeTypeId { get; set; }
        public string office { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public CustAddress()
        {

        }
        public CustAddress(Context.CustomerAddress dbitem)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            Code = dbitem.Code;
            Alamat = dbitem.Alamat;
            IdProvinsi = dbitem.IdProvinsi;
            provinsi = dbitem.LocProvinsi == null ? "" : dbitem.LocProvinsi.Nama;
            IdKabKota = dbitem.IdKabKota;
            kota = dbitem.LocKabKota == null ? "" : dbitem.LocKabKota.Nama;
            IdKec = dbitem.IdKec;
            kecamatan = dbitem.LocKecamatan == null ? "" : dbitem.LocKecamatan.Nama;
            IdKel = dbitem.IdKel;
            kelurahan = dbitem.LocKelurahan == null ? "" : dbitem.LocKelurahan.Nama;
            Longitude = dbitem.Longitude;
            Latitude = dbitem.Latitude;
            Radius = dbitem.Radius;
            Zona = dbitem.Zona;
            OfficeTypeId = dbitem.OfficeTypeId;
            office = dbitem.OfficeTypeId == null ? "" : dbitem.LookUpCodesOffice.Nama;
            Telp = dbitem.Telp;
            Fax = dbitem.Fax;
        }
    }
    public class CustProduct
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? idProduk { get; set; }
        public string NamaProduct { get; set; }
        public string Kategori { get; set; }
        public Decimal TargetSuhu { get; set; }
        public Decimal SuhuMax { get; set; }
        public Decimal SuhuMin { get; set; }
        public int Interval { get; set; }
        public string PenangananKhusus { get; set; }
        public string Keterangan { get; set; }
        public CustProduct()
        {

        }
        public CustProduct(Context.CustomerProductType dbitem)
        {
                Id = dbitem.Id;
                CustomerId = dbitem.CustomerId;
                idProduk = dbitem.idProduk;
                if (dbitem.MasterProduct != null)
                {
                    NamaProduct = dbitem.MasterProduct.NamaProduk;
                Kategori = dbitem.MasterProduct.LookupCode.Nama;
                TargetSuhu = dbitem.MasterProduct.TargetSuhu;
                SuhuMax = dbitem.MasterProduct.MaxTemps;
                SuhuMin = dbitem.MasterProduct.MinTemps;
                Interval = dbitem.MasterProduct.Treshold;
                PenangananKhusus = dbitem.PenangananKhusus;
                //List<string> s = PenangananKhusus.Split(',').ToList();
                //List<int> i = s.Select(int.Parse).ToList();
                //StrPenangananKhusus = string.Join(", ",dblookup.Where(p => i.Contains(p.Id)).Select(d=>d.Nama));
                Keterangan = dbitem.Keterangan;
            }
        }
    }
    public class CustLoadUnload
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public string Alamat { get; set; }
        public int? IdProvinsi { get; set; }
        public string Provinsi { get; set; }
        public int? IdKabKota { get; set; }
        public string Kota { get; set; }
        public int? IdKec { get; set; }
        public string Kecamatan { get; set; }
        public int? IdKel { get; set; }
        public string Kelurahan { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? Radius { get; set; }
        public string Zona { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public CustLoadUnload()
        {

        }
        public CustLoadUnload(Context.CustomerLoadingAddress dbitem)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            Code = dbitem.Code;
            Alamat = dbitem.Alamat;
            IdProvinsi = dbitem.IdProvinsi;
            Provinsi = dbitem.LocProvinsi == null ? "": dbitem.LocProvinsi.Nama;
            IdKabKota = dbitem.IdKabKota;
            Kota = dbitem.LocKabKota == null ? "": dbitem.LocKabKota.Nama;
            IdKec = dbitem.IdKec;
            Kecamatan = dbitem.LocKecamatan == null ? "": dbitem.LocKecamatan.Nama;
            IdKel = dbitem.IdKel;
            Kelurahan = dbitem.LocKelurahan == null ? "" : dbitem.LocKelurahan.Nama;
            Longitude = dbitem.Longitude;
            Latitude = dbitem.Latitude;
            Radius = dbitem.Radius;
            Zona = dbitem.Zona;
            Telp = dbitem.Telp;
            Fax = dbitem.Fax;
        }
        public CustLoadUnload(Context.CustomerUnloadingAddress dbitem)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            Code = dbitem.Code;
            Alamat = dbitem.Alamat;
            IdProvinsi = dbitem.IdProvinsi;
            Provinsi = dbitem.LocProvinsi == null ? "" : dbitem.LocProvinsi.Nama;
            IdKabKota = dbitem.IdKabKota;
            Kota = dbitem.LocKabKota == null ? "" : dbitem.LocKabKota.Nama;
            IdKec = dbitem.IdKec;
            Kecamatan = dbitem.LocKecamatan == null ? "" : dbitem.LocKecamatan.Nama;
            IdKel = dbitem.IdKel;
            Kelurahan = dbitem.LocKelurahan == null ? "" : dbitem.LocKelurahan.Nama;
            Longitude = dbitem.Longitude;
            Latitude = dbitem.Latitude;
            Radius = dbitem.Radius;
            Zona = dbitem.Zona;
            Telp = dbitem.Telp;
            Fax = dbitem.Fax;
        }
    }
    public class CustSupp
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public int? IdProvinsi { get; set; }
        public string Provinsi { get; set; }
        public int? IdKabKota { get; set; }
        public string Kota { get; set; }
        public int? IdKec { get; set; }
        public string Kecamatan { get; set; }
        public int? IdKel { get; set; }
        public string Kelurahan { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? Radius { get; set; }
        public string Zona { get; set; }
        public string Pic { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public CustSupp()
        {

        }
        public CustSupp(Context.CustomerSupplier dbitem)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            Code = dbitem.Code;
            Nama = dbitem.Nama;
            Alamat = dbitem.Alamat;
            IdProvinsi = dbitem.IdProvinsi;
            Provinsi = dbitem.LocProvinsi == null ? "" : dbitem.LocProvinsi.Nama;
            IdKabKota = dbitem.IdKabKota;
            Kota = dbitem.LocKabKota == null ? "" : dbitem.LocKabKota.Nama;
            IdKec = dbitem.IdKec;
            Kecamatan = dbitem.LocKecamatan == null ? "" : dbitem.LocKecamatan.Nama;
            IdKel = dbitem.IdKel;
            Kelurahan = dbitem.LocKelurahan == null ? "" : dbitem.LocKelurahan.Nama;
            Longitude = dbitem.Longitude;
            Latitude = dbitem.Latitude;
            Radius = dbitem.Radius;
            Zona = dbitem.Zona;
            Pic = dbitem.Pic;
            Telp = dbitem.Telp;
            Fax = dbitem.Fax;
        }
    }
    public class CustNotif
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        public int? IdPic { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string Email { get; set; }
        public string Sms { get; set; }
        public string NotifType { get; set; }
        public string strIdRute { get; set; }
        public string strRute { get; set; }
        public string strIdTruck { get; set; }
        public string strTruck { get; set; }
        public CustNotif()
        {

        }
        public CustNotif(Context.CustomerNotification dbitem, Context.CustomerPic dbpic)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            IsActive = dbitem.IsActive;
            IdPic = dbitem.IdPic;
            Nama = dbpic.Name;
            Jabatan = dbpic.LookUpCodesJabatan.Nama;
            Email = dbpic.EmailAdd;
            Sms = dbpic.Mobile;
            NotifType = dbitem.NotifType;
            strIdRute = string.Join(",", dbitem.CustomerNotifRute.Select(d => d.IdRute.Value));
            strRute = string.Join(", ", dbitem.CustomerNotifRute.Select(d => d.Rute.Nama));
            strIdTruck = string.Join(",", dbitem.CustomerNotifTruck.Select(d => d.IdTruck.Value));
            strTruck = string.Join(", ", dbitem.CustomerNotifTruck.Select(d => d.JenisTrucks.StrJenisTruck));
        }
    }
    public class CustBilling
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string DocumentName { get; set; }
        public int Lembar { get; set; }
        public string Warna { get; set; }
        public bool Stempel { get; set; }
        public bool IsFax { get; set; }
        public string Fax { get; set; }
        public bool IsEmail { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public bool IsTukarFaktur { get; set; }
        public bool IsJasaPengiriman { get; set; }
        public string UrlAtt { get; set; }
        public string FileName { get; set; }
        public string srtDataJadwal { get; set; }
        public CustBilling()
        {

        }
        public CustBilling(Context.CustomerBilling dbitem)
        {
            Id = dbitem.Id;
            CustomerId = dbitem.CustomerId;
            DocumentName = dbitem.DocumentName;
            Lembar = dbitem.Lembar;
            Warna = dbitem.Warna;
            Stempel = dbitem.Stempel;
            IsFax = dbitem.IsFax;
            Fax = dbitem.Fax;
            IsEmail = dbitem.IsEmail;
            Email = dbitem.Email;
            IsTukarFaktur = dbitem.IsTukarFaktur;
            IsJasaPengiriman = dbitem.IsJasaPengiriman;
            UrlAtt = dbitem.UrlAtt;
            FileName = dbitem.FileName;
            IsActive = dbitem.IsActive;

            List<ObjDataJadwal> dataJadwal = new List<ObjDataJadwal>();
            foreach (tms_mka_v2.Context.CustomerJadwalBilling items in dbitem.CustomerJadwalBilling)
            {
                dataJadwal.Add(new ObjDataJadwal()
                {
                    Id = items.Id,
                    Hari = items.Hari,
                    Jam = items.Jam,
                    Catatan = items.Catatan,
                    Email = items.PIC
                });
            }
            srtDataJadwal = new JavaScriptSerializer().Serialize(dataJadwal);
        }
    }
    public class UserAttachment
    {
        public int id { get; set; }
        public int CustId { get; set; }
        public string url { get; set; }
        public string filename { get; set; }
        public string realfname { get; set; }
    }
    public class CustTruckType
    {
        public int id { get; set; }
        public int TruckId { get; set; }
        public bool IsKode { get; set; }
        public string Kode { get; set; }
        public bool IsAlias { get; set; }
        public string Alias { get; set; }
    }
    public class ObjDataJadwal
    {
        public int Id { get; set; }
        public string Hari { get; set; }
        public string Jam { get; set; }
        public string Catatan { get; set; }
        public string Email { get; set; }
    }
}