using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class TrainingSettingDetail
    {
        public TrainingSettingDetail()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(Order = 1)]
        [ForeignKey("TrainingSetting")]
        public int? TrainingSettingId { get; set; }
        public string Materi { get; set; }
        public int NilaiMinimum { get; set; }

        public virtual TrainingSetting TrainingSetting { get; set; }
    }
}