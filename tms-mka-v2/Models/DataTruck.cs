using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DataTruck
    {
        public int Id { get; set; }
        public string NoTruck { get; set; }
        [Display(Name = "Vehicle No")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string VehicleNo { get; set; }
        [Display(Name = "Merk")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdMerk { get; set; }
        public string strMerk { get; set; }
        [Display(Name = "Jenis Truck")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdJenisTruck { get; set; }
        public string strJenisTruck { get; set; }
        public string jenisPendingin { get; set; }
        [Display(Name = "Tahun Pembuatan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? TahunBuat { get; set; }
        [Display(Name = "Tahun Beli")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? TahunBeli { get; set; }
        [Display(Name = "Alokasi Pool")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdPool { get; set; }
        public string strPool { get; set; }
        [Display(Name = "Alokasi Unit")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdUnit { get; set; }
        public string strUnit { get; set; }
        [Display(Name = "Keterangan")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Keterangan { get; set; }
        [Display(Name = "Kondisi Khusus")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Kondisi { get; set; }
        [Display(Name = "Model")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string SpecModel { get; set; }
        [Display(Name = "KM Limit")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? KmLimit { get; set; }
        [Display(Name = "No Mesin")]
        public string NoMesin { get; set; }
        [Display(Name = "No Rangka")]
        public string NoRangka { get; set; }
        [Display(Name = "Mulai")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? GaransiStr { get; set; }
        [Display(Name = "Selesai")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? GaransiEnd { get; set; }
        [Display(Name = "Keterangan")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string SpecKeterangan { get; set; }
        [Display(Name = "Atas Nama")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string AtasNama { get; set; }
        [Display(Name = "BPKB")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string BPKB { get; set; }
        [Display(Name = "Masa berlaku STNK")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? STNK { get; set; }
        [Display(Name = "Masa berlaku KIR")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? KIR { get; set; }
        [Display(Name = "Masa berlaku KIU/SIPA")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? KIU { get; set; }
        [Display(Name = "Masa berlaku IBM")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? IBM { get; set; }
        [Display(Name = "Masa berlaku Asuransi")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? Asuransi { get; set; }
        [Display(Name = "Masa Pajak Reklame")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? Reklame { get; set; }
        [Display(Name = "No Polis")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string NoPolis { get; set; }
        [Display(Name = "Peminjam")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Peminjam { get; set; }
        [Display(Name = "Leasing")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Leasing { get; set; }

        public string urlBPKB { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganBPKB { get; set; }
        public string urlSTNK { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganSTNK { get; set; }
        public string urlKIR { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganKIR { get; set; }
        public string urlKIU { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganKIU { get; set; }
        public string urlIBM { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganIBM { get; set; }
        public string urlAsuransi { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganAsuransi { get; set; }
        public string urlReklame { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganReklame { get; set; }
        public string urlNoPolis { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganNoPolis { get; set; }
        public string urlPeminjam { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganPeminjam { get; set; }
        public string urlLeasing { get; set; }
        [Display(Name = "Keterangan")]
        public string keteranganLeasing { get; set; }
        [Display(Name = "Max Speed")]
        public float MaxSpeed { get; set; }
        public DataTruck()
        {

        }
        public DataTruck(Context.DataTruck dbitem)
        {
            Id = dbitem.Id;
            NoTruck = dbitem.NoTruck;
            VehicleNo = dbitem.VehicleNo;
            IdMerk = dbitem.IdMerk != null ? dbitem.IdMerk : null;
            strMerk = dbitem.IdMerk != null ? dbitem.LookupCodeMerk.Nama : "";
            IdJenisTruck = dbitem.IdJenisTruck != null ? dbitem.IdJenisTruck : null;
            strJenisTruck = dbitem.IdJenisTruck != null ? dbitem.JenisTrucks.StrJenisTruck : "";
            TahunBuat = dbitem.TahunBuat;
            TahunBeli = dbitem.TahunBeli;
            IdPool = dbitem.IdPool != null ? dbitem.IdPool : null;
            strPool = dbitem.IdPool != null ? dbitem.MasterPool.NamePool : "";
            IdUnit = dbitem.IdUnit != null ? dbitem.IdUnit : null;
            strUnit = dbitem.IdUnit != null ? dbitem.LookupCodeUnit.Nama : "";
            Keterangan = dbitem.Keterangan;
            Kondisi = dbitem.Kondisi;
            try
            {
                jenisPendingin = dbitem.DataPendingin.FirstOrDefault().Merk;
            }
            catch(Exception){}

            SpecModel = dbitem.SpecModel;
            KmLimit = dbitem.KmLimit;
            NoMesin = dbitem.NoMesin;
            NoRangka = dbitem.NoRangka;
            GaransiStr = dbitem.GaransiStr;
            GaransiEnd = dbitem.GaransiEnd;
            SpecKeterangan = dbitem.SpecKeterangan;
            MaxSpeed = dbitem.MaxSpeed;

            AtasNama = dbitem.AtasNama;
            BPKB = dbitem.BPKB;
            STNK = dbitem.STNK;
            KIR = dbitem.KIR;
            KIU = dbitem.KIU;
            IBM = dbitem.IBM;
            Asuransi = dbitem.Asuransi;
            Reklame = dbitem.Reklame;
            NoPolis = dbitem.NoPolis;
            Peminjam = dbitem.Peminjam;
            Leasing = dbitem.Leasing;

            keteranganBPKB = dbitem.keteranganBPKB;
            keteranganSTNK = dbitem.keteranganSTNK;
            keteranganKIR = dbitem.keteranganKIR;
            keteranganKIU = dbitem.keteranganKIU;
            keteranganIBM = dbitem.keteranganIBM;
            keteranganAsuransi = dbitem.keteranganAsuransi;
            keteranganReklame = dbitem.keteranganReklame;
            keteranganNoPolis = dbitem.keteranganNoPolis;
            keteranganPeminjam = dbitem.keteranganPeminjam;
            keteranganLeasing = dbitem.keteranganLeasing;

            urlBPKB = dbitem.urlBPKB;
            urlSTNK = dbitem.urlSTNK;
            urlKIR = dbitem.urlKIR;
            urlKIU = dbitem.urlKIU;
            urlIBM = dbitem.urlIBM;
            urlAsuransi = dbitem.urlAsuransi;
            urlReklame = dbitem.urlReklame;
            urlNoPolis = dbitem.urlNoPolis;
            urlPeminjam = dbitem.urlPeminjam;
            urlLeasing = dbitem.urlLeasing;

        }
        public void SetDb(Context.DataTruck dbitem)
        {
            dbitem.Id = Id;
            dbitem.NoTruck = NoTruck;
            dbitem.VehicleNo = VehicleNo;
            dbitem.IdMerk = IdMerk;
            dbitem.IdJenisTruck = IdJenisTruck;
            dbitem.TahunBuat = TahunBuat;
            dbitem.TahunBeli = TahunBeli;
            dbitem.IdPool = IdPool;
            dbitem.IdUnit = IdUnit;
            dbitem.Keterangan = Keterangan;
            dbitem.Kondisi = Kondisi;

            dbitem.SpecModel = SpecModel;
            dbitem.KmLimit = KmLimit;
            dbitem.NoMesin = NoMesin;
            dbitem.NoRangka = NoRangka;
            dbitem.GaransiStr = GaransiStr;
            dbitem.GaransiEnd = GaransiEnd;
            dbitem.SpecKeterangan = SpecKeterangan;
            dbitem.MaxSpeed = MaxSpeed;

            dbitem.AtasNama = AtasNama;
            dbitem.BPKB = BPKB;
            dbitem.STNK = STNK;
            dbitem.KIR = KIR;
            dbitem.KIU = KIU;
            dbitem.IBM = IBM;
            dbitem.Asuransi = Asuransi;
            dbitem.Reklame = Reklame;
            dbitem.NoPolis = NoPolis;
            dbitem.Peminjam = Peminjam;
            dbitem.Leasing = Leasing;

            dbitem.keteranganBPKB = keteranganBPKB;
            dbitem.keteranganSTNK = keteranganSTNK;
            dbitem.keteranganKIR = keteranganKIR;
            dbitem.keteranganKIU = keteranganKIU;
            dbitem.keteranganIBM = keteranganIBM;
            dbitem.keteranganAsuransi = keteranganAsuransi;
            dbitem.keteranganReklame = keteranganReklame;
            dbitem.keteranganNoPolis = keteranganNoPolis;
            dbitem.keteranganPeminjam = keteranganPeminjam;
            dbitem.keteranganLeasing = keteranganLeasing;

            dbitem.urlBPKB = urlBPKB;
            dbitem.urlSTNK = urlSTNK;
            dbitem.urlKIR = urlKIR;
            dbitem.urlKIU = urlKIU;
            dbitem.urlIBM = urlIBM;
            dbitem.urlAsuransi = urlAsuransi;
            dbitem.urlReklame = urlReklame;
            dbitem.urlNoPolis = urlNoPolis;
            dbitem.urlPeminjam = urlPeminjam;
            dbitem.urlLeasing = urlLeasing;
        }
    }
}