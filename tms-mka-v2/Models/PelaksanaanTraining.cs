using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class PelaksanaanTraining
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? Tanggal { get; set; }
        [Display(Name = "Waktu Pelaksanaan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan WaktuPelaksanaan { get; set; }
        [Display(Name = "Waktu Selesai")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan WaktuSelesai { get; set; }
        [Display(Name = "Nama Training")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdTrainingSetting { get; set; }
        public string StrTrainingSetting { get; set; }
        [Display(Name = "Materi")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdTrainingMateri { get; set; }
        public string StrTrainingMateri { get; set; }
        [Display(Name = "Lokasi")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdLokasi { get; set; }
        public string StrLokasi { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Trainer { get; set; }
        public string Keterangan { get; set; }
        public int? JumlahPeserta { get; set; }
        public string strPelaksanaanTrainingDetail { get; set; }
        public List<PelaksanaanTrainingDetail> listPelaksanaanTraining { get; set; }
        public PelaksanaanTraining()
        {
            listPelaksanaanTraining = new List<PelaksanaanTrainingDetail>();
        }
        public PelaksanaanTraining(Context.PelaksanaanTraining dbitem)
        {
            Id = dbitem.Id;
            Tanggal = dbitem.Tanggal;
            WaktuPelaksanaan = dbitem.JamPelaksanaan;
            WaktuSelesai = dbitem.JamSelesai;
            IdTrainingSetting = dbitem.IdTrainingSetting;
            StrTrainingSetting = dbitem.trainingSetting.Nama;
            IdTrainingMateri = dbitem.IdTrainingMateri;
            StrTrainingMateri = dbitem.trainingSettingDetail.Materi;
            IdLokasi = dbitem.IdLokasi;
            StrLokasi = dbitem.masterPool.NamePool;
            Trainer = dbitem.Trainer;
            Keterangan = dbitem.Keterangan;
            listPelaksanaanTraining = new List<PelaksanaanTrainingDetail>();
            foreach (Context.PelaksanaanTrainingDetail item in dbitem.PelaksanaanTrainingDetail.ToList())
            {
                listPelaksanaanTraining.Add(new PelaksanaanTrainingDetail(item));
            }
            JumlahPeserta = listPelaksanaanTraining.Count();
        }
        public void setDb(Context.PelaksanaanTraining dbitem)
        {
            dbitem.Id = Id;
            dbitem.Tanggal = Tanggal;
            dbitem.JamPelaksanaan= WaktuPelaksanaan;
            dbitem.JamSelesai = WaktuSelesai;
            dbitem.IdTrainingSetting = IdTrainingSetting;
            dbitem.IdTrainingMateri = IdTrainingMateri;
            dbitem.IdLokasi = IdLokasi;
            dbitem.Trainer = Trainer;
            dbitem.Keterangan = Keterangan;

            dbitem.PelaksanaanTrainingDetail.Clear();

            PelaksanaanTrainingDetail[] result = JsonConvert.DeserializeObject<PelaksanaanTrainingDetail[]>(strPelaksanaanTrainingDetail);

            foreach (PelaksanaanTrainingDetail item in result)
            {
                dbitem.PelaksanaanTrainingDetail.Add(new Context.PelaksanaanTrainingDetail()
                {
                    IdDriver = item.IdDriver,
                    Nilai = item.Nilai,
                    Keterangan = item.Keterangan,
                });
            }
        }
    }

    public class PelaksanaanTrainingDetail
    {
        public int Id { get; set; }
        public int? PelaksanaanTrainingId { get; set; }
        public int? IdDriver { get; set; }
        public int Nilai { get; set; }
        public string Keterangan { get; set; }
        public string Status { get; set; }
        public string KodeDriver { get; set; }
        public string NamaDriver { get; set; }
        public string Training { get; set; }
        public string Materi { get; set; }
        public DateTime? Tanggal { get; set; }
        
        public PelaksanaanTrainingDetail()
        {

        }
        public PelaksanaanTrainingDetail(Context.PelaksanaanTrainingDetail dbitem)
        {
            if (dbitem.PelaksanaanTraining != null){
                Id = dbitem.Id;
                PelaksanaanTrainingId = dbitem.PelaksanaanTrainingId;
                IdDriver = dbitem.IdDriver;
                KodeDriver = dbitem.idDriver.KodeDriver;
                NamaDriver = dbitem.idDriver.NamaDriver;
                Nilai = dbitem.Nilai;
                Keterangan = dbitem.Keterangan;
                Status = dbitem.Status;
                Training = dbitem.PelaksanaanTraining != null && dbitem.PelaksanaanTraining.trainingSetting != null ? dbitem.PelaksanaanTraining.trainingSetting.Nama : "";
                Materi = dbitem.PelaksanaanTraining != null && dbitem.PelaksanaanTraining.trainingSetting != null ? dbitem.PelaksanaanTraining.trainingSettingDetail.Materi : "";
                Tanggal = dbitem.PelaksanaanTraining.Tanggal;
            }
        }
    }
}