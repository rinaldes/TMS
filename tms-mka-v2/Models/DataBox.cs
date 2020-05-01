using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DataBox
    {
        public int Id { get; set; }
        [Display(Name = "No Box")]
        public string NoBox { get; set; }
        [Display(Name = "Vehicle No")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdDataTruck { get; set; }
        public string VehicleNo { get; set; }
        [Display(Name = "Karoseri")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Karoseri { get; set; }
        [Display(Name = "Tahun")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Tahun { get; set; }
        [Display(Name = "Type")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdType { get; set; }
        public string StrType { get; set; }
        [Display(Name = "Kategori")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdKategori { get; set; }
        public string StrKategori { get; set; }
        [Display(Name = "Lantai")]
        public string Lantai { get; set; }
        public string strLantai { get; set; }
        [Display(Name = "Dinding")]
        public string Dinding { get; set; }
        public string strDinding { get; set; }
        [Display(Name = "Pintu Samping")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]        
        public bool? PintuSamping { get; set; }
        [Display(Name = "Sekat")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public bool? Sekat { get; set; }
        [Display(Name = "Mulai")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? garansiStr { get; set; }
        [Display(Name = "Selesai")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? garansiEnd { get; set; }
        [Display(Name = "Mulai")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? asuransiStr { get; set; }
        [Display(Name = "Selesai")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? asuransiEnd { get; set; }
        [Display(Name = "Tanggal Pasang")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? tglPasang { get; set; }
        public DateTime? Tanggal { get; set; }
        public string username { get; set; }
        public List<LantaiModel> ListLantai { get; set; }
        public List<DindingModel> ListDinding { get; set; }
        public DataBox()
        {
            
        }

        public DataBox(Context.DataBox dbitem)
        {
            List<string> dummyLantai = new List<string>();
            List<string> dummyDinding = new List<string>();

            Id = dbitem.Id;
            NoBox = dbitem.NoBox;
            IdDataTruck = dbitem.IdDataTruck;
            VehicleNo = dbitem.DataTruck.VehicleNo;
            Karoseri = dbitem.Karoseri;
            Tahun = dbitem.Tahun;
            IdType = dbitem.IdType;
            StrType = dbitem.IdType != null ? dbitem.LookupCodeType.Nama : "";
            IdKategori = dbitem.IdKategori;
            StrKategori = dbitem.IdKategori != null ? dbitem.LookupCodeKategori.Nama : "";
            Lantai = dbitem.Lantai;
            dummyLantai.Add(dbitem.Lantai);
            Dinding = dbitem.Dinding;
            dummyDinding.Add(dbitem.Dinding);
            PintuSamping = dbitem.PintuSamping;
            Sekat = dbitem.Sekat;
            garansiStr = dbitem.garansiStr;
            garansiEnd = dbitem.garansiEnd;
            asuransiStr = dbitem.asuransiStr;
            asuransiEnd = dbitem.asuransiEnd;
            tglPasang = dbitem.tglPasang;
            foreach (DataBoxLantai item in dbitem.DataBoxLantai)
            {
                dummyLantai.Add(item.LookupCodeLantaiCode.Nama);
            }
            foreach (DataBoxDinding item in dbitem.DataBoxDinding)
            {
                dummyDinding.Add(item.LookupCodeDinding.Nama);
            }
            if (dummyLantai.Count() > 0)
                strLantai = string.Join(", ", dummyLantai);
            if (dummyDinding.Count() > 0)
                strDinding = string.Join(", ", dummyDinding);
        }
        public DataBox(Context.DataBoxHistory dbitem)
        {
            Id = dbitem.Id;
            NoBox = dbitem.DataBox.NoBox;
            VehicleNo = dbitem.Vehicle;
            Karoseri = dbitem.Karoseri;
            Tahun = dbitem.Tahun;
            StrType = dbitem.strType;
            StrKategori = dbitem.strKategori;
            Lantai = dbitem.Lantai;
            Dinding = dbitem.Dinding;
            PintuSamping = dbitem.PintuSamping;
            Sekat = dbitem.Sekat;
            garansiStr = dbitem.garansiStr;
            garansiEnd = dbitem.garansiEnd;
            asuransiStr = dbitem.asuransiStr;
            asuransiEnd = dbitem.asuransiEnd;
            tglPasang = dbitem.tglPasang;
            Tanggal = dbitem.Tanggal;
            username = dbitem.username;
        }
        public DataBox(Context.DataTruckBoxHistory dbitem)
        {
            Id = dbitem.Id;
            NoBox = dbitem.NoBox;
            VehicleNo = dbitem.Vehicle;
            Karoseri = dbitem.Karoseri;
            Tahun = dbitem.Tahun;
            StrType = dbitem.strType;
            StrKategori = dbitem.strKategori;
            Lantai = dbitem.Lantai;
            Dinding = dbitem.Dinding;
            PintuSamping = dbitem.PintuSamping;
            Sekat = dbitem.Sekat;
            garansiStr = dbitem.garansiStr;
            garansiEnd = dbitem.garansiEnd;
            asuransiStr = dbitem.asuransiStr;
            asuransiEnd = dbitem.asuransiEnd;
            tglPasang = dbitem.tglPasang;
            Tanggal = dbitem.Tanggal;
            username = dbitem.username;
        }

        public void SetDb(Context.DataBox dbitem)
        {
            dbitem.Id = Id;
            dbitem.NoBox = NoBox;
            dbitem.IdDataTruck = IdDataTruck;
            dbitem.Karoseri = Karoseri;
            dbitem.Tahun = Tahun;
            dbitem.IdType = IdType;
            dbitem.IdKategori = IdKategori;
            dbitem.Lantai = Lantai;
            dbitem.Dinding = Dinding;
            dbitem.PintuSamping = PintuSamping;
            dbitem.Sekat = Sekat;
            dbitem.garansiStr = garansiStr;
            dbitem.garansiEnd = garansiEnd;
            dbitem.asuransiStr = asuransiStr;
            dbitem.asuransiEnd = asuransiEnd;
            dbitem.tglPasang = tglPasang;
            dbitem.DataBoxLantai.Clear();
            dbitem.DataBoxDinding.Clear();
            if (ListLantai.Count() > 0)
            {
                foreach (LantaiModel item in ListLantai)
                {
                    if (item.IsSelect)
                    {
                        dbitem.DataBoxLantai.Add(new DataBoxLantai() { IdLantaiCode = item.IdLantai });
                    }
                }            
            }

            if (ListDinding.Count() > 0)
            {
                foreach (DindingModel item in ListDinding)
                {
                    if (item.IsSelect)
                    {
                        dbitem.DataBoxDinding.Add(new DataBoxDinding() { IdDindingCode = item.IdDinding });
                    }
                }
            }
        }
        public void SetDbHistory(Context.DataBoxHistory dbitem, string user)
        {
            dbitem.NoBox = NoBox;
            dbitem.Vehicle = VehicleNo;
            dbitem.Karoseri = Karoseri;
            dbitem.Tahun = Tahun;
            dbitem.strType = StrType;
            dbitem.strKategori = StrKategori;
            dbitem.Lantai = Lantai;
            dbitem.Dinding = Dinding;
            dbitem.PintuSamping = PintuSamping;
            dbitem.Sekat = Sekat;
            dbitem.garansiStr = garansiStr;
            dbitem.garansiEnd = garansiEnd;
            dbitem.asuransiStr = asuransiStr;
            dbitem.asuransiEnd = asuransiEnd;
            dbitem.tglPasang = tglPasang;
            dbitem.Tanggal = DateTime.Now;
            dbitem.username = user;
            if (ListLantai.Count() > 0)
            {
                foreach (LantaiModel item in ListLantai)
                {
                    if (item.IsSelect)
                    {
                        dbitem.Lantai = dbitem.Lantai + ", " + item.StrLantai;
                    }
                }
            }
            if (ListDinding.Count() > 0)
            {
                foreach (DindingModel item in ListDinding)
                {
                    if (item.IsSelect)
                    {
                        dbitem.Dinding = dbitem.Dinding + ", " + item.StrDinding;
                    }
                }
            }
        }
        public void SetDbTruckHistory(Context.DataTruckBoxHistory dbitem, string user)
        {
            dbitem.NoBox = NoBox;
            dbitem.Vehicle = VehicleNo;
            dbitem.Karoseri = Karoseri;
            dbitem.Tahun = Tahun;
            dbitem.strType = StrType;
            dbitem.strKategori = StrKategori;
            dbitem.Lantai = Lantai;
            dbitem.Dinding = Dinding;
            dbitem.PintuSamping = PintuSamping;
            dbitem.Sekat = Sekat;
            dbitem.garansiStr = garansiStr;
            dbitem.garansiEnd = garansiEnd;
            dbitem.asuransiStr = asuransiStr;
            dbitem.asuransiEnd = asuransiEnd;
            dbitem.tglPasang = tglPasang;
            dbitem.Tanggal = DateTime.Now;
            dbitem.username = user;
            if (ListLantai.Count() > 0)
            {
                foreach (LantaiModel item in ListLantai)
                {
                    if (item.IsSelect)
                    {
                        dbitem.Lantai = dbitem.Lantai + ", " + item.StrLantai;
                    }
                }
            }

            if (ListDinding.Count() > 0)
            {
                foreach (DindingModel item in ListDinding)
                {
                    if (item.IsSelect)
                    {
                        dbitem.Dinding = dbitem.Dinding + ", " + item.StrDinding;
                    }
                }
            }
        }
    }
    public class DindingModel
    {
        public int IdDinding { get; set; }
        public string StrDinding { get; set; }
        public bool IsSelect { get; set; }
        public DindingModel()
        {

        }
        public DindingModel(Context.LookupCode dbitem)
        {
            IdDinding = dbitem.Id;
            StrDinding = dbitem.Nama;
        }
    }
    public class LantaiModel
    {
        public int IdLantai { get; set; }
        public string StrLantai { get; set; }
        public bool IsSelect { get; set; }
        public LantaiModel()
        {

        }
        public LantaiModel(Context.LookupCode dbitem)
        {
            IdLantai = dbitem.Id;
            StrLantai = dbitem.Nama;
        } 
    }
}