using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Tiket
    {
        public int Id { get; set; }
        [Display(Name = "No Tiket")]
        public string NoTiket { get; set; }
        [Display(Name = "Pelapor")]
        public int? IdCustomer { get; set; }
        public string KodeCustomer { get; set; }
        [Display(Name = "Nama Pelapor")]
        public string NamaCustomer { get; set; }
        [Display(Name = "Tanggal Lapor")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TanggalLapor { get; set; }
        [Display(Name = "Diajukan Ke")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string DitujukanKe { get; set; }
        [Display(Name = "Kategori")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Kategori { get; set; }
        [Display(Name = "Prioritas")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Prioritas { get; set; }
        [Display(Name = "Status")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Status { get; set; }
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Display(Name = "Keluhan")]
        public string Keluhan { get; set; }
        public string Respon { get; set; }
        public int? IdCS { get; set; }
        public string namaCs { get; set; }
        public string pathFotoCs { get; set; }
        public int Urutan { get; set; }
        [Display(Name = "Nama Pelapor")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaPelapor { get; set; }
        public string KodeNama { get; set; }
        public int? IdSo { get; set; }
        public string IdSoKontrak { get; set; }
        public string Attactment { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string ResponseAttachment { get; set; }

        public Tiket()
        {

        }
        public Tiket(Context.Tiket dbitem)
        {
             Id = dbitem.Id;  
             NoTiket = dbitem.NoTiket;
             IdCustomer = dbitem.IdCustomer;
             NamaCustomer = dbitem.Customer.CustomerNama;
             NamaPelapor = dbitem.NamaPelapor;
             KodeCustomer = dbitem.Customer.CustomerCode;
             KodeNama = dbitem.Customer.CustomerCodeOld;
             TanggalLapor = dbitem.TanggalLapor;
             DitujukanKe = dbitem.DitujukanKe;
             Kategori = dbitem.Kategori;
             Prioritas = dbitem.Prioritas;
             Status = dbitem.Status;
             Subject = dbitem.Subject;
             Keluhan = dbitem.Keluhan;
             IdCS = dbitem.IdCS;
             namaCs = dbitem.CS.Fristname + " " + dbitem.CS.Lastname;
             pathFotoCs = dbitem.CS.path_foto;
             Respon = dbitem.Respon;
             Attactment = dbitem.Attactment;
             IdSo = dbitem.IdSo;
             IdSoKontrak = dbitem.IdSoKontrak;
             LastUpdate = dbitem.LastUpdate;
        }
        public void setDb(Context.Tiket dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdCustomer = IdCustomer;
            dbitem.TanggalLapor = TanggalLapor.Value;
            dbitem.DitujukanKe = DitujukanKe;
            dbitem.Kategori = Kategori;
            dbitem.Prioritas = Prioritas;
            dbitem.Status = Status;
            dbitem.Subject = Subject;
            dbitem.Keluhan = Keluhan;
            dbitem.Respon = Respon;
            dbitem.NamaPelapor = NamaPelapor;
            dbitem.Attactment = Attactment;
            dbitem.ResponseAttachment = ResponseAttachment;
            if (IdSo != null)
                dbitem.IdSo = IdSo;
            if (IdSoKontrak != null)
                dbitem.IdSoKontrak = IdSoKontrak;
        }
    }
}