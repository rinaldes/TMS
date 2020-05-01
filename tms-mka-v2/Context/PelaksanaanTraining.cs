using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class PelaksanaanTraining
    {
        public PelaksanaanTraining()
        {
            this.PelaksanaanTrainingDetail = new HashSet<PelaksanaanTrainingDetail>();
        }

        [Key]
        public int Id { get; set; }

        public DateTime? Tanggal { get; set; }
        public TimeSpan JamPelaksanaan { get; set; }
        public TimeSpan JamSelesai { get; set; }
        [ForeignKey("trainingSetting")]
        public int? IdTrainingSetting { get; set; }
        [ForeignKey("trainingSettingDetail")]
        public int? IdTrainingMateri { get; set; }
        [ForeignKey("masterPool")]
        public int? IdLokasi { get; set; }
        public string Trainer { get; set; }
        public string Keterangan { get; set; }

        public virtual TrainingSetting trainingSetting { get; set; }
        public virtual TrainingSettingDetail trainingSettingDetail { get; set; }
        public virtual MasterPool masterPool { get; set; }

        public virtual ICollection<PelaksanaanTrainingDetail> PelaksanaanTrainingDetail { get; set; }
    }
}