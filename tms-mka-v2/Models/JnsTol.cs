using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class JnsTol
    {
        public int Id { get; set; }
        [Display(Name = "Nama Tol")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string NamaTol { get; set; }
        [Display(Name = "Golongan Tol 1")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? GolonganTol1 { get; set; }
        [Display(Name = "Golongan Tol 2")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? GolonganTol2 { get; set; }
        [Display(Name = "Golongan Tol 3")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? GolonganTol3 { get; set; }
        [Display(Name = "Golongan Tol 4")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Decimal? GolonganTol4 { get; set; }
        [Display(Name = "Keterangan")]
        public string Keterangan { get; set; }
        //public bool isDeleted { get; set; }
        public JnsTol()
        {

        }
        public JnsTol(Context.JnsTols dbitem)
        {
            Id = dbitem.Id;
            NamaTol = dbitem.NamaTol;
            GolonganTol1 = dbitem.GolonganTol1;
            GolonganTol2 = dbitem.GolonganTol2;
            GolonganTol3 = dbitem.GolonganTol3;
            GolonganTol4 = dbitem.GolonganTol4;
            Keterangan = dbitem.Keterangan;
        }
        public void setDb(Context.JnsTols dbitem)
        {
            dbitem.Id = Id;
            dbitem.NamaTol = NamaTol;
            dbitem.GolonganTol1 = GolonganTol1.Value;
            dbitem.GolonganTol2 = GolonganTol2.Value;
            dbitem.GolonganTol3 = GolonganTol3.Value;
            dbitem.GolonganTol4 = GolonganTol4.Value;
            dbitem.Keterangan = Keterangan;
        }

        public void setDbHistory(Context.HistoryJnsTols dbitem, int IdUser)
        {
            dbitem.IdTol = Id;
            dbitem.NamaTol = NamaTol;
            dbitem.GolonganTol1 = GolonganTol1.Value;
            dbitem.GolonganTol2 = GolonganTol2.Value;
            dbitem.GolonganTol3 = GolonganTol3.Value;
            dbitem.GolonganTol4 = GolonganTol4.Value;
            dbitem.Keterangan = Keterangan;
            dbitem.Tanggal = DateTime.Now;
            dbitem.IdUser = IdUser;
        }
    }
}