using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Spk
    {
        public int Id { get; set; }
        [Display(Name = "Jenis Perbaikan")]
        public string Jenis { get; set; }
        public TimeSpan JamServiceIn { get; set; }
        public TimeSpan JamEstimasi { get; set; }
        public TimeSpan JamServiceOut { get; set; }
        [Display(Name = "Permintaan Perbaikan")]
        public string Permintaan { get; set; }
        [Display(Name = "Keterangan Perbaikan")]
        public string Keterangan { get; set; }
        [Display(Name = "Mekanik 1")]
        public int? Mekanik1 { get; set; }
        [Display(Name = "Mekanik 2")]
        public int? Mekanik2 { get; set; }
        public int? Workshop_id { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Service In")]
        public System.DateTime? ServiceInDateTime { get; set; }
        public string ServiceIn { get; set; }
        [Display(Name = "Service Out")]
        public System.DateTime? ServiceOutDateTime { get; set; }
        [Display(Name = "Estimasi")]
        public System.DateTime? EstimasiDateTime { get; set; }
        [Display(Name = "Estimasi Revisi")]
        public string Estimasi { get; set; }
        [Display(Name = "Service Out")]
        public string ServiceOut { get; set; }
        public string OmMekanik1 { get; set; }
        public string OmMekanik2 { get; set; }
        [Display(Name = "Revisi Estimasi")]
        public int? RevEstimasi { get; set; }
        public string KeteranganSpk { get; set; }
        public string NoSPK { get; set; }
        public string JamServiceInString { get; set; }
        public string JamEstimasiString { get; set; }
        public string JamServiceOutString { get; set; }
        public virtual Workshop Workshop { get; set; }
        
        public Spk()
        {

        }

        public Spk(Context.Spk dbitem)
        {
            Id = dbitem.Id;
            Jenis = dbitem.Jenis;
            Permintaan = dbitem.Permintaan;
            KeteranganSpk = dbitem.KeteranganSPK;
            Keterangan = dbitem.Keterangan;
            Mekanik1 = dbitem.Mekanik1;
            Mekanik2 = dbitem.Mekanik2;
            Status = dbitem.Status;
            ServiceInDateTime = dbitem.ServiceIn;
            EstimasiDateTime = dbitem.Estimasi;
            ServiceOutDateTime = dbitem.ServiceOut;
            ServiceIn = (dbitem.ServiceIn.ToString().Split())[0];
            if (dbitem.ServiceIn != null && dbitem.ServiceIn.Value.ToString("HH:mm") != "00:00")
                JamServiceInString = dbitem.ServiceIn.Value.ToString("HH:mm");
            else
                JamServiceInString = string.Concat(dbitem.JamServiceIn.ToString().Reverse().Skip(3).Reverse());
            if (dbitem.Estimasi != null && dbitem.Estimasi.Value.ToString("HH:mm") != "00:00")
                JamEstimasiString = dbitem.Estimasi.Value.ToString("HH:mm");
            else
                JamEstimasiString = string.Concat(dbitem.JamEstimasi.ToString().Reverse().Skip(3).Reverse());
            if (dbitem.ServiceOut != null && dbitem.ServiceOut.Value.ToString("HH:mm") != "00:00")
                JamServiceOutString = dbitem.ServiceOut.Value.ToString("HH:mm");
            else
                JamServiceOutString = string.Concat(dbitem.JamServiceOut.ToString().Reverse().Skip(3).Reverse());
            Estimasi = (dbitem.Estimasi.ToString().Split())[0];
            ServiceOut = (dbitem.ServiceOut.ToString().Split())[0];
            Workshop_id = dbitem.Workshop_id;
            RevEstimasi = dbitem.RevEstimasi;
            if(dbitem.Mekanik != null)
                OmMekanik1 = dbitem.Mekanik.NamaMekanik;
            if (dbitem.Mekanikk != null)
                OmMekanik2 = dbitem.Mekanikk.NamaMekanik;
        }
        public void setDb(Context.Spk dbitem)
        {
            dbitem.Id = Id;
            dbitem.Jenis = Jenis;
            dbitem.Workshop_id = Workshop_id;
            dbitem.Permintaan = Permintaan;
            dbitem.KeteranganSPK = KeteranganSpk;
            dbitem.Keterangan = Keterangan;
            dbitem.Mekanik1 = Mekanik1;
            dbitem.Mekanik2 = Mekanik2;
            dbitem.Status = Status;
            if (ServiceIn != "" && ServiceIn != null)
            {
                dbitem.ServiceIn = DateTime.ParseExact(ServiceIn, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                dbitem.JamServiceIn = JamServiceIn;
            }
            if (Estimasi != "" && Estimasi != null)
            {
                dbitem.Estimasi = DateTime.ParseExact(Estimasi, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                dbitem.JamEstimasi = JamEstimasi;
            }
            if (ServiceOut != "" && ServiceOut != null)
            {
                dbitem.ServiceOut = DateTime.ParseExact(ServiceOut, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                dbitem.JamServiceOut = JamServiceOut;
            }
            dbitem.RevEstimasi = RevEstimasi;
        }
    }
    public class SpkHistory
    {
        public int Id { get; set; }
        public string Jenis { get; set; }
        public string Status { get; set; }
        public string Estimasi { get; set; }
        public string Tanggal { get; set; }
        public int RevEstimasi { get; set; }
        public int? WorkshopId { get; set; }
        public virtual Workshop Workshop { get; set; }

        public SpkHistory()
        {

        }

        public SpkHistory(Context.SpkHistory dbitem)
        {
            Id = dbitem.Id;
            Jenis = dbitem.Jenis;
            Tanggal = (dbitem.Tanggal.Date.ToString().Split())[0];
            Status = dbitem.Status;
            Estimasi = (dbitem.Estimasi.Date.ToString().Split())[0] + " " + string.Concat(dbitem.JamEstimasi.ToString().Reverse().Skip(3).Reverse());
            RevEstimasi = dbitem.RevEstimasi;
        }
    }

}