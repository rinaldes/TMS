using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DataPendingin
    {
        public int Id { get; set; }
        [Display(Name = "No Pendingin")]
        public string NoPendingin { get; set; }
        [Display(Name = "Vehicle No")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdDataTruk { get; set; }
        public string VechileNo { get; set; }
        [Display(Name = "Merk")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Merk { get; set; }
        [Display(Name = "Model")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string ModelPendingin { get; set; }
        [Display(Name = "HM Limit")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string HmLimit { get; set; }
        [Display(Name = "Tahun")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Tahun { get; set; }
        [Display(Name = "Jenis")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdJenisPendingin{ get; set; }
        public string NamaJenisPendingin{ get; set; }
        [Display(Name = "No Mesin")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoMesin { get; set; }
        [Display(Name = "No Kompresor")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoKompresor { get; set; }
        [Display(Name = "Tanggal Pasang")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? tglPasang { get; set; }
        public DateTime? tanggal { get; set; }
        public string username { get; set; }
        public DataPendingin()
        {
        }
        public DataPendingin(Context.DataPendingin dbitem)
        {
            Id = dbitem.Id;
            NoPendingin = dbitem.NoPendingin;
            IdDataTruk = dbitem.IdDataTruk;
            VechileNo = dbitem.DataTruk.VehicleNo;
            Merk = dbitem.Merk;
            ModelPendingin = dbitem.Model;
            HmLimit = dbitem.HmLimit;
            Tahun = dbitem.Tahun;
            IdJenisPendingin = dbitem.IdJenisPendingin;
            NamaJenisPendingin = dbitem.IdJenisPendingin != null ? dbitem.LookupCodeJenis.Nama : "";
            NoMesin = dbitem.NoMesin;
            NoKompresor = dbitem.NoKompresor;
            tglPasang = dbitem.tglPasang;
        }
        public DataPendingin(Context.DataPendinginHistory dbitem)
        {
            Id = dbitem.Id;
            NoPendingin = dbitem.ForPendingin.NoPendingin;
            VechileNo = dbitem.strDataTruk;
            Merk = dbitem.Merk;
            ModelPendingin = dbitem.Model;
            HmLimit = dbitem.HmLimit;
            Tahun = dbitem.Tahun;
            NamaJenisPendingin = dbitem.strJenisPendingin;
            NoMesin = dbitem.NoMesin;
            NoKompresor = dbitem.NoKompresor;
            tglPasang = dbitem.tglPasang;
            tanggal = dbitem.Tanggal;
            username = dbitem.user;
        }
        public DataPendingin(Context.DataTruckPendinginHistory dbitem)
        {
            Id = dbitem.Id;
            NoPendingin = dbitem.NoPendingin;
            VechileNo = dbitem.strDataTruk;
            Merk = dbitem.Merk;
            ModelPendingin = dbitem.Model;
            HmLimit = dbitem.HmLimit;
            Tahun = dbitem.Tahun;
            NamaJenisPendingin = dbitem.strJenisPendingin;
            NoMesin = dbitem.NoMesin;
            NoKompresor = dbitem.NoKompresor;
            tglPasang = dbitem.tglPasang;
            tanggal = dbitem.Tanggal;
            username = dbitem.user;
        }
        public void setDb(Context.DataPendingin dbitem)
        {
            dbitem.Id = Id;
            dbitem.NoPendingin = NoPendingin;
            dbitem.IdDataTruk = IdDataTruk;
            dbitem.Merk = Merk;
            dbitem.Model = ModelPendingin;
            dbitem.HmLimit = HmLimit;
            dbitem.Tahun = Tahun;
            dbitem.IdJenisPendingin = IdJenisPendingin;
            dbitem.NoMesin = NoMesin;
            dbitem.NoKompresor = NoKompresor;
            dbitem.tglPasang = tglPasang;
        }

        public void setDbHistory(Context.DataPendinginHistory dbitem, string user)
        {
            dbitem.Id = Id;
            dbitem.Tanggal = DateTime.Now;
            dbitem.NoPendingin = NoPendingin;
            dbitem.user = user;
            dbitem.strDataTruk = VechileNo;
            dbitem.Merk = Merk;
            dbitem.Model = ModelPendingin;
            dbitem.HmLimit = HmLimit;
            dbitem.Tahun = Tahun;
            dbitem.strJenisPendingin = NamaJenisPendingin;
            dbitem.NoMesin = NoMesin;
            dbitem.NoKompresor = NoKompresor;
            dbitem.tglPasang = tglPasang;
        }

        public void setDbTruckHistory(Context.DataTruckPendinginHistory dbitem, string user)
        {
            dbitem.Id = Id;
            dbitem.Tanggal = DateTime.Now;
            dbitem.NoPendingin = NoPendingin;
            dbitem.user = user;
            dbitem.strDataTruk = VechileNo;
            dbitem.Merk = Merk;
            dbitem.Model = ModelPendingin;
            dbitem.HmLimit = HmLimit;
            dbitem.Tahun = Tahun;
            dbitem.strJenisPendingin = NamaJenisPendingin;
            dbitem.NoMesin = NoMesin;
            dbitem.NoKompresor = NoKompresor;
            dbitem.tglPasang = tglPasang;
        }
    }    
}