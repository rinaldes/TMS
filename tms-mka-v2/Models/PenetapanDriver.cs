using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class PenetapanDriver
    {
        public int Id { get; set; }
        [Display(Name = "Vehicle No")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdDataTruck { get; set; }
        public string VehicleNo { get; set; }
        [Display(Name = "Jenis Truk")]
        public string JenisTruck { get; set; }
        [Display(Name = "ID Driver 1")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdDriver1 { get; set; }
        public string Kode1 { get; set; }
        [Display(Name = "Nama")]
        public string Nama1 { get; set; }
        [Display(Name = "Panggilan")]
        public string Panggilan1 { get; set; }
        [Display(Name = "No HP 1")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoHp1Driver1 { get; set; }
        [Display(Name = "No HP 2")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoHp2Driver1 { get; set; }
        [Display(Name = "Jenis SIM")]
        public string Sim1 { get; set; }
        [Display(Name = "Masa Berlaku")]
        public DateTime? BerlakuSim1 { get; set; }
        [Display(Name = "ID Driver 2")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdDriver2 { get; set; }
        public string Kode2 { get; set; }
        [Display(Name = "Nama")]
        public string Nama2 { get; set; }
        [Display(Name = "Panggilan")]
        public string Panggilan2 { get; set; }
        [Display(Name = "No HP 1")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoHp1Driver2 { get; set; }
        [Display(Name = "No HP 2")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NoHp2Driver2 { get; set; }
        [Display(Name = " Jenis SIM")]
        public string Sim2 { get; set; }
        [Display(Name = "Masa Berlaku")]
        public DateTime? BerlakuSim2 { get; set; }
        [Display(Name = "Ditetapkan Oleh")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public String IdDitetapkanOleh1 { get; set; }
        [Display(Name = "Ditetapkan Oleh")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public String IdDitetapkanOleh2 { get; set; }        

        public PenetapanDriver()
        {

        }

        public PenetapanDriver(Context.PenetapanDriver dbitem)
        {
            Id = dbitem.Id;
            IdDataTruck = dbitem.IdDataTruck;
            VehicleNo = dbitem.DataTruck.VehicleNo;
            JenisTruck = dbitem.DataTruck.IdJenisTruck.HasValue ? dbitem.DataTruck.JenisTrucks.StrJenisTruck : "";

            IdDriver1 = dbitem.IdDriver1;
            Kode1 = dbitem.Driver1.KodeDriver;
            Nama1 = dbitem.Driver1.NamaDriver;
            Panggilan1 = dbitem.Driver1.NamaPangilan;
            NoHp1Driver1 = dbitem.NoHp1Driver1;
            NoHp2Driver1 = dbitem.NoHp2Driver1;
            Sim1 = dbitem.Driver1.LookupCodeJenisSim.Nama;
            BerlakuSim1 = dbitem.Driver1.TglBerlakuSim.Value.Date;
            IdDitetapkanOleh1 = dbitem.DitetapkanOleh1;

            if (dbitem.IdDriver2.HasValue)
            {
                IdDriver2 = dbitem.IdDriver2;
                Kode2 = dbitem.Driver2.KodeDriver;
                Nama2 = dbitem.Driver2.NamaDriver;
                Panggilan2 = dbitem.Driver2.NamaPangilan;
                NoHp1Driver2 = dbitem.NoHp1Driver2;
                NoHp2Driver2 = dbitem.NoHp2Driver2;
                Sim2 = dbitem.Driver2.LookupCodeJenisSim.Nama;
                BerlakuSim2 = dbitem.Driver2.TglBerlakuSim.Value.Date;
                IdDitetapkanOleh2 = dbitem.DitetapkanOleh2;
            }
        }

        public void SetDb(Context.PenetapanDriver dbitem, string user)
        {
            dbitem.IdDataTruck = IdDataTruck;

            dbitem.IdDriver1 = IdDriver1;
            dbitem.NoHp1Driver1 = NoHp1Driver1;
            dbitem.NoHp2Driver1 = NoHp1Driver1;

            dbitem.IdDriver2 = IdDriver2;
            dbitem.NoHp1Driver2 = NoHp1Driver2;
            dbitem.NoHp2Driver2 = NoHp2Driver2;

            dbitem.DitetapkanOleh1 = IdDitetapkanOleh1;
            dbitem.DitetapkanOleh2 = IdDitetapkanOleh2;

            dbitem.ModifiedBy = user;
            dbitem.ModifiedDate = DateTime.Now;
        }
        public void SetDbHistory(Context.PenetapanDriverHistory dbitem, string user)
        {
            dbitem.IdPenetapanDriver = Id;
            dbitem.Driver1 = Nama1;
            dbitem.Driver2 = Nama2;
            dbitem.ModifiedBy = user;
            dbitem.ModifiedDate = DateTime.Now;
        }
    }
}