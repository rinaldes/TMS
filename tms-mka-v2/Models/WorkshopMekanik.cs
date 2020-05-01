using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class WorkshopMekanik
    {
        public int Id { get; set; }
        [Display(Name = "No Kartu")]
        public int? IdWorkshop { get; set; }
        public int? IdMekanik { get; set; }
        public string MekanikName { get; set; }
        public string Bagian { get; set; }
        public string NoPPK { get; set; }
        public string Mulai { get; set; }
        public string Selesai { get; set; }
        public string PPKSesudah { get; set; }
        //public bool IsDelete { get; set; }
        
        public WorkshopMekanik()
        {

        }
        public WorkshopMekanik(Context.WorkshopMekanik dbitem)
        {
             Id = dbitem.Id;
             IdMekanik = dbitem.IdMekanik;
             IdWorkshop = dbitem.IdWorkshop;
             MekanikName = dbitem.DataMekanik.NamaMekanik;
             Bagian = dbitem.Workshop.Jenis;
             NoPPK = dbitem.Workshop.Workshop.NoPPK;
             Mulai = (dbitem.Workshop.ServiceIn.ToString().Split())[0];
             Selesai = (dbitem.Workshop.Estimasi.ToString().Split())[0];
             PPKSesudah = "";
        }
        public WorkshopMekanik(Context.Spk dbitem)
        {
            Id = dbitem.Id;
            IdMekanik = dbitem.Mekanik1;
            IdWorkshop = dbitem.Workshop_id;
            MekanikName = dbitem.Mekanik.NamaMekanik;
            Bagian = dbitem.Jenis;
            NoPPK = dbitem.Workshop.NoPPK;
            Mulai = (dbitem.ServiceIn.ToString().Split())[0];
            Selesai = (dbitem.Estimasi.ToString().Split())[0];
            PPKSesudah = "";
        }
        public void setDb(Context.WorkshopMekanik dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdMekanik = IdMekanik;
            dbitem.IdWorkshop = IdWorkshop;
        }
    }
}