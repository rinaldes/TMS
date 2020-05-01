using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Workshop
    {
        public int id { get; set; }
        [Display(Name = "NO PPK")]
        public string NoPPK { get; set; }
        public string NoSPK { get; set; }
        [Display(Name = "Tanggal Pre-PPK")]
        public DateTime TglPre { get; set; }
        [Display(Name = "Jam Order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamPre { get; set; }
        public TimeSpan JamPPK { get; set; }
        [Display(Name = "Tanggal PPK")]
        public DateTime TglPPK { get; set; }
        [Display(Name = "No Vehicle")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int IdVehicle { get; set; }
        [Display(Name = "Prioritas")]
        public int? IdPrioritas { get; set; }
        public DateTime? PrioritasFrom { get; set; }
        public DateTime? PrioritasTo { get; set; }
        public string Prioritas { get; set; }
        [Display(Name = "Keterangan Prioritas")]
        public string KetPrioritas { get; set; }
        [Display(Name = "Truk sudah di pool?")]
        public Boolean IsPool { get; set; }
        [Display(Name = "Truk")]
        public Boolean IsTruck { get; set; }
        [Display(Name = "AC")]
        public Boolean IsAc { get; set; }
        [Display(Name = "GPS")]
        public Boolean IsGps { get; set; }
        [Display(Name = "Ban")]
        public Boolean IsBan { get; set; }
        [Display(Name = "Box")]
        public Boolean IsBox { get; set; }
        public string KetTruck { get; set; }
        public string KetAc { get; set; }
        public string KetGps { get; set; }
        public string KetBan { get; set; }
        public string KetBox { get; set; }
        public string KetKerjaTruck { get; set; }
        public string KetKerjaAc { get; set; }
        public string KetKerjaGps { get; set; }
        public string KetKerjaBan { get; set; }
        public string KetKerjaBox { get; set; }
        public string Status { get; set; }
        public string VehicleNo { get; set; }
        public string JenisTruk { get; set; }
        public int Urutan { get; set; }
        public int? KmActual { get; set; }
        public int? HmActual { get; set; }
        public string JenisPerbaikan { get; set; }
        public string Mekanik1 { get; set; }
        public string Mekanik2 { get; set; }
        public System.DateTime? ServiceIn { get; set; }
        public System.DateTime? Estimasi { get; set; }
        public int? RevEstimasi { get; set; }
        public System.DateTime? ServiceOut { get; set; }
        public System.DateTime? ServiceInDateTime { get; set; }
        public System.DateTime? ServiceOutDateTime { get; set; }
        public System.DateTime? EstimasiDateTime { get; set; }
        public string JamServiceInString { get; set; }
        public string JamEstimasiString { get; set; }
        public string JamServiceOutString { get; set; }
        public Workshop()
        {

        }

        public Workshop(Context.Workshop dbitem)
        {
            id = dbitem.Id;
            NoPPK = dbitem.NoPPK;// Status == "Pre-PPK" ? dbitem.NoPrePPK : dbitem.NoPPK;
            TglPPK = dbitem.TglPPK;
            TglPre = dbitem.TglPre;
            IdVehicle = dbitem.IdVehicle;
            PrioritasFrom = dbitem.PrioritasFrom;
            IdPrioritas = dbitem.IdPrioritas;
            PrioritasTo = dbitem.PrioritasTo;
            Prioritas = dbitem.LookUp == null ? "" : dbitem.LookUp.Nama;
            KetPrioritas = dbitem.KetPrioritas;
            IsAc = dbitem.IsAc;
            IsBan = dbitem.IsBan;
            IsBox = dbitem.IsBox;
            IsGps = dbitem.IsGps;
            IsPool = dbitem.IsPool;
            IsTruck = dbitem.IsTruck;
            KetTruck = dbitem.KetTruck;
            KetGps = dbitem.KetGps;
            KetBox = dbitem.KetBox;
            KetBan = dbitem.KetBan;
            KetAc = dbitem.KetAc;
            KetKerjaTruck = dbitem.KetKerjaTruck;
            KetKerjaGps = dbitem.KetKerjaGps;
            KetKerjaBox = dbitem.KetKerjaBox;
            KetKerjaBan = dbitem.KetKerjaBan;
            KetKerjaAc = dbitem.KetKerjaAc;
            if (dbitem.Status == "SPK"){
                //Status = dbitem.Status == "SPK" ? "SPK-O" : dbitem.Status;
                if(dbitem.Spk.Count() == dbitem.Spk.Where(d => d.Status == "Closed").Count()){
                    dbitem.Status = "SPK-C";
                }
                else if(dbitem.Spk.Where(d => d.Status == null).Count() == dbitem.Spk.Count()){
                    dbitem.Status = "SPK-O";
                }
                else{
                    dbitem.Status = "SPK-W";
                }
            }
            else{
                Status = dbitem.Status;
            }
            Status = dbitem.Status == "SPK" ? "SPK-O" : dbitem.Status;
            Urutan = dbitem.Urutan;
            VehicleNo = dbitem.Truk.VehicleNo;
            if (dbitem.Truk.JenisTrucks != null)
                JenisTruk = dbitem.Truk.JenisTrucks.StrJenisTruck;
            KmActual = dbitem.KmActual;
            HmActual = dbitem.HmActual;
            if (dbitem.IsTruck == true)
                JenisPerbaikan += "Truck";
            if (dbitem.IsAc == true)
                JenisPerbaikan += ", Ac";
            if (dbitem.IsBan == true)
                JenisPerbaikan += ", Ban";
            if (dbitem.IsBox == true)
                JenisPerbaikan += ", Box";
            if (dbitem.IsGps == true)
                JenisPerbaikan += ", Gps";
        }

        public Workshop(Context.Workshop dbitem, string jenisPerbaikan)
        {
            id = dbitem.Id;
            NoPPK = dbitem.NoPPK;// Status == "Pre-PPK" ? dbitem.NoPrePPK : dbitem.NoPPK;
            TglPPK = dbitem.TglPPK;
            TglPre = dbitem.TglPre;
            IdVehicle = dbitem.IdVehicle;
            PrioritasFrom = dbitem.PrioritasFrom;
            IdPrioritas = dbitem.IdPrioritas;
            PrioritasTo = dbitem.PrioritasTo;
            Prioritas = dbitem.LookUp == null ? "" : dbitem.LookUp.Nama;
            KetPrioritas = dbitem.KetPrioritas;
            IsAc = dbitem.IsAc;
            IsBan = dbitem.IsBan;
            IsBox = dbitem.IsBox;
            IsGps = dbitem.IsGps;
            IsPool = dbitem.IsPool;
            IsTruck = dbitem.IsTruck;
            KetTruck = dbitem.KetTruck;
            KetGps = dbitem.KetGps;
            KetBox = dbitem.KetBox;
            KetBan = dbitem.KetBan;
            KetAc = dbitem.KetAc;
            KetKerjaTruck = dbitem.KetKerjaTruck;
            KetKerjaGps = dbitem.KetKerjaGps;
            KetKerjaBox = dbitem.KetKerjaBox;
            KetKerjaBan = dbitem.KetKerjaBan;
            KetKerjaAc = dbitem.KetKerjaAc;
            if (dbitem.Status == "SPK"){
                if(dbitem.Spk.Count() == dbitem.Spk.Where(d => d.Status == "Closed").Count()){
                    dbitem.Status = "SPK-C";
                }
                else if(dbitem.Spk.Where(d => d.Status == null).Count() == dbitem.Spk.Count()){
                    dbitem.Status = "SPK-O";
                }
                else{
                    dbitem.Status = "SPK-W";
                }
            }
            else{
                Status = dbitem.Status;
            }
            Status = dbitem.Status == "SPK" ? "SPK-O" : dbitem.Status;
            Urutan = dbitem.Urutan;
            VehicleNo = dbitem.Truk.VehicleNo;
            if (dbitem.Truk.JenisTrucks != null)
                JenisTruk = dbitem.Truk.JenisTrucks.StrJenisTruck;
            KmActual = dbitem.KmActual;
            HmActual = dbitem.HmActual;
            if (jenisPerbaikan == "Truck")
                JenisPerbaikan = "Truck";
            if (jenisPerbaikan == "AC")
                JenisPerbaikan = "AC";
            if (jenisPerbaikan == "Ban")
                JenisPerbaikan = "Ban";
            if (jenisPerbaikan == "Box")
                JenisPerbaikan = "Box";
            if (jenisPerbaikan == "GPS")
                JenisPerbaikan = "GPS";
            Context.Spk spk = dbitem.Spk.Where(d => d.Jenis == jenisPerbaikan).FirstOrDefault();
            if (spk != null)
            {
                ServiceIn = spk.ServiceIn;
                Estimasi = spk.Estimasi;
                RevEstimasi = spk.RevEstimasi;
                ServiceOut = spk.ServiceOut;
                NoSPK = spk.NoSpk;
                if (spk.Mekanik != null)
                    Mekanik1 = dbitem.Spk.Where(d => d.Jenis == jenisPerbaikan).FirstOrDefault().Mekanik.NamaMekanik;
                if (spk.Mekanikk != null)
                    Mekanik2 = dbitem.Spk.Where(d => d.Jenis == jenisPerbaikan).FirstOrDefault().Mekanikk.NamaMekanik;
                ServiceInDateTime = spk.ServiceIn;
                EstimasiDateTime = spk.Estimasi;
                ServiceOutDateTime = spk.ServiceOut;
                if (spk.ServiceIn != null && spk.ServiceIn.Value.ToString("HH:mm") != "00:00")
                    JamServiceInString = spk.ServiceIn.Value.ToString("HH:mm");
                else
                    JamServiceInString = string.Concat(spk.JamServiceIn.ToString().Reverse().Skip(3).Reverse());
                if (spk.Estimasi != null && spk.Estimasi.Value.ToString("HH:mm") != "00:00")
                    JamEstimasiString = spk.Estimasi.Value.ToString("HH:mm");
                else
                    JamEstimasiString = string.Concat(spk.JamEstimasi.ToString().Reverse().Skip(3).Reverse());
                if (spk.ServiceOut != null && spk.ServiceOut.Value.ToString("HH:mm") != "00:00")
                    JamServiceOutString = spk.ServiceOut.Value.ToString("HH:mm");
                else
                    JamServiceOutString = string.Concat(spk.JamServiceOut.ToString().Reverse().Skip(3).Reverse());
            }
        }

        public void setDb(Context.Workshop dbitem)
        {
            dbitem.NoPPK = dbitem.NoPPK == null ? NoPPK : dbitem.NoPPK;
            dbitem.Urutan = Urutan;
            dbitem.TglPPK = TglPPK;
            dbitem.TglPre = DateTime.Parse(TglPre.ToString("yyyy-MM-dd") + " " + JamPre.ToString().Replace(".", ":"));
            dbitem.IdVehicle = IdVehicle;
            dbitem.IdPrioritas = IdPrioritas;
            dbitem.PrioritasFrom = PrioritasFrom;
            dbitem.PrioritasTo = PrioritasTo;
            dbitem.KetPrioritas = KetPrioritas;
            dbitem.IsAc = IsAc;
            dbitem.IsBan = IsBan;
            dbitem.IsBox = IsBox;
            dbitem.IsGps = IsGps;
            dbitem.IsPool = IsPool;
            dbitem.IsTruck = IsTruck;
            dbitem.KetTruck = KetTruck;
            dbitem.KetGps = KetGps;
            dbitem.KetBox = KetBox;
            dbitem.KetBan = KetBan;
            dbitem.KetAc = KetAc;
            dbitem.KetKerjaTruck = KetKerjaTruck;
            dbitem.KetKerjaGps = KetKerjaGps;
            dbitem.KetKerjaBox = KetKerjaBox;
            dbitem.KetKerjaBan = KetKerjaBan;
            dbitem.KetKerjaAc = KetKerjaAc;
            dbitem.Status = Status;
            dbitem.KmActual = KmActual;
            dbitem.HmActual = HmActual;
        }
        public void setDbPpkIn(Context.Workshop dbitem)
        {
            dbitem.KetPrioritas = KetPrioritas;
            dbitem.IsAc = IsAc;
            dbitem.IsBan = IsBan;
            dbitem.IsBox = IsBox;
            dbitem.IsGps = IsGps;
            dbitem.IsPool = IsPool;
            dbitem.IsTruck = IsTruck;
            dbitem.KetTruck = KetTruck;
            dbitem.KetGps = KetGps;
            dbitem.KetBox = KetBox;
            dbitem.KetBan = KetBan;
            dbitem.KetAc = KetAc;
            dbitem.KetKerjaTruck = KetKerjaTruck;
            dbitem.KetKerjaGps = KetKerjaGps;
            dbitem.KetKerjaBox = KetKerjaBox;
            dbitem.KetKerjaBan = KetKerjaBan;
            dbitem.KetKerjaAc = KetKerjaAc;
            dbitem.Status = Status;
            dbitem.KmActual = KmActual;
            dbitem.HmActual = HmActual;
        }
        public void setDbSpk(Context.Workshop dbitem)
        {
            dbitem.IsAc = IsAc;
            dbitem.IsBan = IsBan;
            dbitem.IsBox = IsBox;
            dbitem.IsGps = IsGps;
            dbitem.IsTruck = IsTruck;
            dbitem.KetTruck = KetTruck;
            dbitem.KetGps = KetGps;
            dbitem.KetBox = KetBox;
            dbitem.KetBan = KetBan;
            dbitem.KetAc = KetAc;
            dbitem.KetKerjaTruck = KetKerjaTruck;
            dbitem.KetKerjaGps = KetKerjaGps;
            dbitem.KetKerjaBox = KetKerjaBox;
            dbitem.KetKerjaBan = KetKerjaBan;
            dbitem.KetKerjaAc = KetKerjaAc;
            dbitem.Status = Status;
            dbitem.KmActual = KmActual;
            dbitem.HmActual = HmActual;
        }
    }

}