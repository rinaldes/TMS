using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Driver
    {
        private ContextModel context = new ContextModel();
        #region field
        public int Id { get; set; }
        [Display(Name = "ID Driver")]
        public string KodeDriver { get; set; }
        [Display(Name = "ID Driver Lama")]
        public string KodeDriverOld { get; set; }
        [Display(Name = "Tanggal Bergabung")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TglGabung { get; set; }
        [Display(Name = "No KTP")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoKtp { get; set; }
        [Display(Name = "Nama Sesuai KTP")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaDriver { get; set; }
        [Display(Name = "Nama Panggilan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaPangilan { get; set; }
        [Display(Name = "TTL")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string TempatLahir { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TglLahir { get; set; }
        [Display(Name = "Jenis SIM")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdJenisSim { get; set; }
        public string StrJenisSim { get; set; }
        [Display(Name = "No SIM")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoSim { get; set; }
        [Display(Name = "Masa Berlaku")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TglBerlakuSim { get; set; }
        public string KodeTlp { get; set; }
        [Display(Name = "No Telp")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoTlp { get; set; }
        [Display(Name = "No HP 1")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoHp1 { get; set; }
        [Display(Name = "No HP 2")]
        public string NoHp2 { get; set; }
        [Display(Name = "Alamat")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Alamat { get; set; }
        [Display(Name = "RT")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Rt { get; set; }
        [Display(Name = "RW")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Rw { get; set; }
        [Display(Name = "Provinsi")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdProvinsi { get; set; }
        public string StrProvinsi { get; set; }
        [Display(Name = "Kabupaten / Kota")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKabKota { get; set; }
        public string StrKabKota { get; set; }
        [Display(Name = "Kecamatan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKec { get; set; }
        public string StrKec { get; set; }
        [Display(Name = "Kelurahan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKel { get; set; }
        public string StrKel { get; set; }
        [Display(Name = "Sesuai KTP")]
        public bool IsSameKtp { get; set; }
        [Display(Name = "Alamat")]
        public string AlamatDomisili { get; set; }
        [Display(Name = "RT")]
        public string RtDomisili { get; set; }
        [Display(Name = "RW")]
        public string RwDomisili { get; set; }
        [Display(Name = "Provinsi")]
        public int? IdProvinsiDomisili { get; set; }
        public string StrProvinsiDomisili { get; set; }
        [Display(Name = "Kabupaten / Kota")]
        public int? IdKabKotaDomisili { get; set; }
        public string StrKabKotaDomisili { get; set; }
        [Display(Name = "Kecamatan")]
        public int? IdKecDomisili { get; set; }
        public string StrKecDomisili { get; set; }
        [Display(Name = "Kelurahan")]
        public int? IdKelDomisili { get; set; }
        public string StrKelDomisili { get; set; }
        [Display(Name = "Status")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdStatus { get; set; }
        public string Status { get; set; }
        [Display(Name = "Keterangan Status")]
        public string Keterangan { get; set; }
        [Display(Name = "SMS Notifikasi")]
        public bool IsSms { get; set; }
        [Display(Name = "Foto")]
        public string Pathfoto { get; set; }
        public string DokumenPending { get; set; }
        public string Training { get; set; }


        [Display(Name = "Referensi")]
        public int? IdReferensiDriver { get; set; }
        public int? IdRef { get; set; }
        public string NamaRef { get; set; }
        public string CodeRef { get; set; }
        public string KTPRef { get; set; }
        public string tlpRef { get; set; }
        public string hpRef { get; set; }
        public string HubunganRef { get; set; }
        public string KeteranganRef { get; set; }

        public bool IsKemitraan { get; set; }
        public bool IsKemitraanAsli { get; set; }
        public string UrlKemitraan { get; set; }
        public bool IsJaminanKel { get; set; }
        public bool IsJaminanKelAsli { get; set; }
        public string UrlJaminanKel { get; set; }
        public bool IsIjazah { get; set; }
        public bool IsIjazahAsli { get; set; }
        public string UrlIjazah { get; set; }
        public bool IsBukuNikah { get; set; }
        public bool IsBukuNikahAsli { get; set; }
        public string UrlBukuNikah { get; set; }
        public bool IsSKCK { get; set; }
        public bool IsSKCKAsli { get; set; }
        public string UrlSKCK { get; set; }
        public bool IsDomisili { get; set; }
        public bool IsDomisiliAsli { get; set; }
        public string UrlDomisili { get; set; }
        public bool IsKK { get; set; }
        public bool IsKKAsli { get; set; }
        public string UrlKK { get; set; }
        public bool IsKTP { get; set; }
        public bool IsKTPAsli { get; set; }
        public string UrlKTP { get; set; }
        public bool IsSIM { get; set; }
        public bool IsSIMAsli { get; set; }
        public string UrlSIM { get; set; }
        public string StatusSo { get; set; }
        public int? IdDataTruck { get; set; }
        [Display(Name = "Vehicle No")]
        public string VehicleNo { get; set; }
        [Display(Name = "Jenis Truk")]
        public string strJenisTruck { get; set; }
        [Display(Name = "Jenis Pendingin")]
        public string JenisPendingin { get; set; }
        #endregion

        public Driver()
        {

        }
        public Driver(Context.Driver dbitem)
        {
            Id = dbitem.Id;
            IdStatus = dbitem.IdStatus;
            Status = dbitem.LookupCodeStatus.Nama;
            KodeDriver = dbitem.KodeDriver;
            KodeDriverOld = dbitem.KodeDriverOld;
            TglGabung = dbitem.TglGabung;
            NoKtp = dbitem.NoKtp;
            NamaDriver = dbitem.NamaDriver;
            NamaPangilan = dbitem.NamaPangilan;
            TempatLahir = dbitem.TempatLahir;
            TglLahir = dbitem.TglLahir;
            IdJenisSim = dbitem.IdJenisSim;
            StrJenisSim = dbitem.LookupCodeJenisSim == null ? "" : dbitem.LookupCodeJenisSim.Nama;
            NoSim = dbitem.NoSim;
            TglBerlakuSim = dbitem.TglBerlakuSim;
            KodeTlp = !dbitem.NoTlp.Contains('-') ? "" : dbitem.NoTlp.Split('-')[0];
            NoTlp = !dbitem.NoTlp.Contains('-') ? dbitem.NoTlp : dbitem.NoTlp.Split('-')[1];
            NoHp1 = dbitem.NoHp1;
            NoHp2 = dbitem.NoHp2;
            Alamat = dbitem.Alamat;
            Rt = dbitem.Rt;
            Rw = dbitem.Rw;
            IdProvinsi = dbitem.IdProvinsi;
            StrProvinsi = dbitem.IdProvinsi == null ? "" : dbitem.LocProvinsi.Nama;
            IdKabKota = dbitem.IdKabKota;
            StrKabKota = dbitem.IdKabKota == null ? "" : dbitem.LocKabKota.Nama;
            IdKec = dbitem.IdKec;
            StrKec = dbitem.IdKec == null ? "" : dbitem.LocKecamatan.Nama;
            IdKel = dbitem.IdKel;
            StrKel = dbitem.IdKel == null ? "" : dbitem.LocKelurahan.Nama;
            IsSameKtp = dbitem.IsSameKtp;
            if (!IsSameKtp)
            {
                AlamatDomisili = dbitem.AlamatDomisili;
                RtDomisili = dbitem.RtDomisili;
                RwDomisili = dbitem.RwDomisili;
                IdProvinsiDomisili = dbitem.IdProvinsiDomisili;
                StrProvinsiDomisili = dbitem.IdProvinsiDomisili == null ? "" : dbitem.LocProvinsiDomisili.Nama;
                IdKabKotaDomisili = dbitem.IdKabKotaDomisili;
                StrKabKotaDomisili = dbitem.IdKabKotaDomisili == null ? "" : dbitem.LocKabKotaDomisili.Nama;
                IdKecDomisili = dbitem.IdKecDomisili;
                StrKabKotaDomisili = dbitem.IdKecDomisili == null ? "" : dbitem.LocKecamatanDomisili.Nama;
                IdKelDomisili = dbitem.IdKelDomisili;
                StrKelDomisili = dbitem.IdKelDomisili == null ? "" : dbitem.LocKelurahanDomisili.Nama;
            }
            Keterangan = dbitem.Keterangan;
            IsSms = dbitem.IsSms;
            Pathfoto = dbitem.Pathfoto;
            IdReferensiDriver = dbitem.IdReferensiDriver;
            IdRef = dbitem.IdRef;
            NamaRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NamaDriver;
            CodeRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.KodeDriver;
            KTPRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NoKtp;
            tlpRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NoTlp;
            hpRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NoHp1;
            HubunganRef = dbitem.HubunganRef;
            KeteranganRef = dbitem.KeteranganRef;

            IsKemitraan = dbitem.IsKemitraan;
            IsKemitraanAsli = dbitem.IsKemitraanAsli;
            UrlKemitraan = dbitem.UrlKemitraan;
            IsJaminanKel = dbitem.IsJaminanKel;
            IsJaminanKelAsli = dbitem.IsJaminanKelAsli;
            UrlJaminanKel = dbitem.UrlJaminanKel;
            IsIjazah = dbitem.IsIjazah;
            IsIjazahAsli = dbitem.IsIjazahAsli;
            UrlIjazah = dbitem.UrlIjazah;
            IsBukuNikah = dbitem.IsBukuNikah;
            IsBukuNikahAsli = dbitem.IsBukuNikahAsli;
            UrlBukuNikah = dbitem.UrlBukuNikah;
            IsSKCK = dbitem.IsSKCK;
            IsSKCKAsli = dbitem.IsSKCKAsli;
            UrlSKCK = dbitem.UrlSKCK;
            IsDomisili = dbitem.IsDomisili;
            IsDomisiliAsli = dbitem.IsDomisiliAsli;
            UrlDomisili = dbitem.UrlDomisili;
            IsKK = dbitem.IsKK;
            IsKKAsli = dbitem.IsKKAsli;
            UrlKK = dbitem.UrlKK;
            IsKTP = dbitem.IsKTP;
            IsKTPAsli = dbitem.IsKTPAsli;
            UrlKTP = dbitem.UrlKTP;
            IsSIM = dbitem.IsSIM;
            IsSIMAsli = dbitem.IsSIMAsli;
            UrlSIM = dbitem.UrlSIM;

            if (context.PenetapanDriver.Any(d => d.IdDriver1 == dbitem.Id)) {
                Context.PenetapanDriver pd = context.PenetapanDriver.Where(d => d.IdDriver1 == dbitem.Id).FirstOrDefault();
                VehicleNo = pd.DataTruck.VehicleNo;
                strJenisTruck = pd.DataTruck.JenisTrucks.StrJenisTruck;
                JenisPendingin = pd.DataTruck.DataPendingin.FirstOrDefault().Merk;
                IdDataTruck = pd.IdDataTruck;
            }
        }
        public Driver(Context.Driver dbitem, List<Context.SalesOrder> dbso, List<Context.SettlementBatal> dbsb)
        {
            if (dbitem != null){
                Id = dbitem.Id;
                IdStatus = dbitem.IdStatus;
                Status = dbitem.LookupCodeStatus.Nama;
                KodeDriver = dbitem.KodeDriver;
                KodeDriverOld = dbitem.KodeDriverOld;
                TglGabung = dbitem.TglGabung;
                NoKtp = dbitem.NoKtp;
                NamaDriver = dbitem.NamaDriver;
                NamaPangilan = dbitem.NamaPangilan;
                TempatLahir = dbitem.TempatLahir;
                TglLahir = dbitem.TglLahir;
                IdJenisSim = dbitem.IdJenisSim;
                StrJenisSim = dbitem.LookupCodeJenisSim == null ? "" : dbitem.LookupCodeJenisSim.Nama;
                NoSim = dbitem.NoSim;
                TglBerlakuSim = dbitem.TglBerlakuSim;
                KodeTlp = !dbitem.NoTlp.Contains('-') ? "" : dbitem.NoTlp.Split('-')[0];
                NoTlp = !dbitem.NoTlp.Contains('-') ? dbitem.NoTlp : dbitem.NoTlp.Split('-')[1];
                NoHp1 = dbitem.NoHp1;
                NoHp2 = dbitem.NoHp2;
                Alamat = dbitem.Alamat;
                Rt = dbitem.Rt;
                Rw = dbitem.Rw;
                IdProvinsi = dbitem.IdProvinsi;
                StrProvinsi = dbitem.IdProvinsi == null ? "" : dbitem.LocProvinsi.Nama;
                IdKabKota = dbitem.IdKabKota;
                StrKabKota = dbitem.IdKabKota == null ? "" : dbitem.LocKabKota.Nama;
                IdKec = dbitem.IdKec;
                StrKec = dbitem.IdKec == null ? "" : dbitem.LocKecamatan.Nama;
                IdKel = dbitem.IdKel;
                StrKel = dbitem.IdKel == null ? "" : dbitem.LocKelurahan.Nama;
                IsSameKtp = dbitem.IsSameKtp;
                if (!IsSameKtp)
                {
                    AlamatDomisili = dbitem.AlamatDomisili;
                    RtDomisili = dbitem.RtDomisili;
                    RwDomisili = dbitem.RwDomisili;
                    IdProvinsiDomisili = dbitem.IdProvinsiDomisili;
                    StrProvinsiDomisili = dbitem.IdProvinsiDomisili == null ? "" : dbitem.LocProvinsiDomisili.Nama;
                    IdKabKotaDomisili = dbitem.IdKabKotaDomisili;
                    StrKabKotaDomisili = dbitem.IdKabKotaDomisili == null ? "" : dbitem.LocKabKotaDomisili.Nama;
                    IdKecDomisili = dbitem.IdKecDomisili;
                    StrKabKotaDomisili = dbitem.IdKecDomisili == null ? "" : dbitem.LocKecamatanDomisili.Nama;
                    IdKelDomisili = dbitem.IdKelDomisili;
                    StrKelDomisili = dbitem.IdKelDomisili == null ? "" : dbitem.LocKelurahanDomisili.Nama;
                }
                Keterangan = dbitem.Keterangan;
                IsSms = dbitem.IsSms;
                Pathfoto = dbitem.Pathfoto;
                IdReferensiDriver = dbitem.IdReferensiDriver;
                IdRef = dbitem.IdRef;
                NamaRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NamaDriver;
                CodeRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.KodeDriver;
                KTPRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NoKtp;
                tlpRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NoTlp;
                hpRef = dbitem.DriverRef == null ? "" : dbitem.DriverRef.NoHp1;
                HubunganRef = dbitem.HubunganRef;
                KeteranganRef = dbitem.KeteranganRef;

                IsKemitraan = dbitem.IsKemitraan;
                IsKemitraanAsli = dbitem.IsKemitraanAsli;
                UrlKemitraan = dbitem.UrlKemitraan;
                IsJaminanKel = dbitem.IsJaminanKel;
                IsJaminanKelAsli = dbitem.IsJaminanKelAsli;
                UrlJaminanKel = dbitem.UrlJaminanKel;
                IsIjazah = dbitem.IsIjazah;
                IsIjazahAsli = dbitem.IsIjazahAsli;
                UrlIjazah = dbitem.UrlIjazah;
                IsBukuNikah = dbitem.IsBukuNikah;
                IsBukuNikahAsli = dbitem.IsBukuNikahAsli;
                UrlBukuNikah = dbitem.UrlBukuNikah;
                IsSKCK = dbitem.IsSKCK;
                IsSKCKAsli = dbitem.IsSKCKAsli;
                UrlSKCK = dbitem.UrlSKCK;
                IsDomisili = dbitem.IsDomisili;
                IsDomisiliAsli = dbitem.IsDomisiliAsli;
                UrlDomisili = dbitem.UrlDomisili;
                IsKK = dbitem.IsKK;
                IsKKAsli = dbitem.IsKKAsli;
                UrlKK = dbitem.UrlKK;
                IsKTP = dbitem.IsKTP;
                IsKTPAsli = dbitem.IsKTPAsli;
                UrlKTP = dbitem.UrlKTP;
                IsSIM = dbitem.IsSIM;
                IsSIMAsli = dbitem.IsSIMAsli;
                UrlSIM = dbitem.UrlSIM;

                StatusSo = "Available";
                if (dbso != null)
                {
                    if (context.SalesOrder.Any(d => d.DriverId == dbitem.Id && (d.Status == "save" || d.Status == "draft planning")))
                        StatusSo = "Planning";
                    else if (context.SalesOrder.Any(d => d.DriverId == dbitem.Id && (d.Status == "save planning" || d.Status == "draft konfirmasi")))
                        StatusSo = "Konfirmasi";
                    else if (dbso.Any(d => (d.Status == "admin uang jalan") && d.AdminUangJalan != null && d.AdminUangJalan.IdDriver1 == dbitem.Id))
                        StatusSo = "admin uang jalan";
                    else if (context.SalesOrder.Any(d => d.DriverId == dbitem.Id && (d.Status == "dispatched")))
                        StatusSo = "Dispatched";
                }
            }
        }
        public void setDb(Context.Driver dbitem)
        {
            dbitem.KodeDriver = KodeDriver;
            dbitem.KodeDriverOld = KodeDriverOld;
            dbitem.IdStatus = IdStatus;
            dbitem.TglGabung = TglGabung;
            dbitem.NoKtp = NoKtp;
            dbitem.NamaDriver = NamaDriver;
            dbitem.NamaPangilan = NamaPangilan;
            dbitem.TempatLahir = TempatLahir;
            dbitem.TglLahir = TglLahir;
            dbitem.IdJenisSim = IdJenisSim;
            dbitem.NoSim = NoSim;
            dbitem.TglBerlakuSim = TglBerlakuSim;
            dbitem.NoTlp = KodeTlp + '-' + NoTlp;
            dbitem.NoHp1 = NoHp1;
            dbitem.NoHp2 = NoHp2;
            dbitem.Alamat = Alamat;
            dbitem.Rt = Rt;
            dbitem.Rw = Rw;
            dbitem.IdProvinsi = IdProvinsi;
            dbitem.IdKabKota = IdKabKota;
            dbitem.IdKec = IdKec;
            dbitem.IdKel = IdKel;
            dbitem.IsSameKtp = IsSameKtp;
            if (IsSameKtp)
            {
                dbitem.AlamatDomisili = dbitem.Alamat;
                dbitem.RtDomisili = dbitem.Rt;
                dbitem.RwDomisili = dbitem.Rw;
                dbitem.IdProvinsiDomisili = dbitem.IdProvinsi;
                dbitem.IdKabKotaDomisili = dbitem.IdKabKota;
                dbitem.IdKecDomisili = dbitem.IdKec;
                dbitem.IdKelDomisili = dbitem.IdKel;
            }
            else
            {
                dbitem.AlamatDomisili = AlamatDomisili;
                dbitem.RtDomisili = RtDomisili;
                dbitem.RwDomisili = RwDomisili;
                dbitem.IdProvinsiDomisili = IdProvinsiDomisili;
                dbitem.IdKabKotaDomisili = IdKabKotaDomisili;
                dbitem.IdKecDomisili = IdKecDomisili;
                dbitem.IdKelDomisili = IdKelDomisili;            
            }
            dbitem.Keterangan = Keterangan;
            dbitem.IsSms = IsSms;
            dbitem.Pathfoto = Pathfoto;
            dbitem.IdReferensiDriver = IdReferensiDriver;
            dbitem.IdRef = IdRef;
            dbitem.HubunganRef = HubunganRef;
            dbitem.KeteranganRef = KeteranganRef;

            dbitem.IsKemitraan = IsKemitraan;
            if (IsKemitraan)
            {
                dbitem.IsKemitraanAsli = IsKemitraanAsli;
                dbitem.UrlKemitraan = UrlKemitraan;
            }
            dbitem.IsJaminanKel = IsJaminanKel;
            if (IsJaminanKel)
            {
                dbitem.IsJaminanKelAsli = IsJaminanKelAsli;
                dbitem.UrlJaminanKel = UrlJaminanKel;
            }
            dbitem.IsIjazah = IsIjazah;
            if (IsIjazah)
            {
                dbitem.IsIjazahAsli = IsIjazahAsli;
                dbitem.UrlIjazah = UrlIjazah;
            }
            dbitem.IsBukuNikah = IsBukuNikah;
            if (IsBukuNikah)
            {
                dbitem.IsBukuNikahAsli = IsBukuNikahAsli;
                dbitem.UrlBukuNikah = UrlBukuNikah;
            }
            dbitem.IsSKCK = IsSKCK;
            if (IsSKCK)
            {
                dbitem.IsSKCKAsli = IsSKCKAsli;
                dbitem.UrlSKCK = UrlSKCK;
            }
            dbitem.IsDomisili = IsDomisili;
            if (IsDomisili)
            {
                dbitem.IsDomisiliAsli = IsDomisiliAsli;
                dbitem.UrlDomisili = UrlDomisili;
            }
            dbitem.IsKK = IsKK;
            if (IsKK)
            {
                dbitem.IsKKAsli = IsKKAsli;
                dbitem.UrlKK = UrlKK;
            }
            dbitem.IsKTP = IsKTP;
            if (IsKTP)
            {
                dbitem.IsKTPAsli = IsKTPAsli;
                dbitem.UrlKTP = UrlKTP;
            }
            dbitem.IsSIM = IsSIM;
            if (IsSIM)
            {
                dbitem.IsSIMAsli = IsSIMAsli;
                dbitem.UrlSIM = UrlSIM;
            }
        }
        public void setHistoryStatus(Context.DriverStatusHistory dbStatHistory, string stat)
        {
            dbStatHistory.Tanggal = DateTime.Now;
            dbStatHistory.Status = stat;
            dbStatHistory.keterangan = Keterangan;
        }
    }
    public class DriverHisTransfer
    {
        public string JnsSo { get; set; }
        public string NoSo { get; set; }
        public string NoPol { get; set; }
        public string JnsTruck { get; set; }
        public string Customer { get; set; }
        public string Rute { get; set; }
        public string JnsBarang { get; set; }
        public decimal TargetSuhu { get; set; }
        public decimal Jumtf { get; set; }
        public DateTime DateTf { get; set; }
        public string KetTf { get; set; }
        public decimal Persentase { get; set; }
    }
    public class JasaDriver
    {
        public int Id {get;set;}
        public int IdDriver { get; set; }
        public DateTime TanggalJasa { get; set; }
        public string keterangan { get; set; }
    }
}