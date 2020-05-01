using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class TrainingSetting
    {
        public TrainingSetting()
        {
            this.TrainingSettingDetail = new HashSet<TrainingSettingDetail>();
        }

        [Key]
        public int Id { get; set; }
        public string Nama { get; set; }
        public int Interval { get; set; }

        public virtual ICollection<TrainingSettingDetail> TrainingSettingDetail { get; set; }
    }
}