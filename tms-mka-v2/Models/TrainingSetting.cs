using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class TrainingSetting
    {
        public int Id { get; set; }
        public string strTrainingSettingDetail { get; set; }
        public string DetailMateri { get; set; }
        public int DetailNilaiMinimum { get; set; }
        [Display(Name = "Nama Training")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Nama { get; set; }
        [Display(Name = "Interval Pengulangan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^\d+$", ErrorMessage = "Input hanya angka")]
        public int Interval { get; set; }
        public List<TrainingSettingDetail> listTrainingSetting { get; set; }
        
        public TrainingSetting()
        {
            listTrainingSetting = new List<TrainingSettingDetail>();
        }
        public TrainingSetting(Context.TrainingSetting dbitem)
        {
            Id = dbitem.Id;
            Nama = dbitem.Nama;
            Interval = dbitem.Interval;
            listTrainingSetting = new List<TrainingSettingDetail>();
            foreach (Context.TrainingSettingDetail item in dbitem.TrainingSettingDetail.ToList())
            {
                listTrainingSetting.Add(new TrainingSettingDetail(item));
            }
            DetailMateri = string.Join(", ", listTrainingSetting.Select(d => d.Materi));
            DetailNilaiMinimum = listTrainingSetting.Count() == 0 ? 0 : listTrainingSetting.Select(d => d.NilaiMinimum).Min();
        }
        public void setDb(Context.TrainingSetting dbitem)
        {
            dbitem.Id = Id;
            dbitem.Nama = Nama;
            dbitem.Interval = Interval;

            dbitem.TrainingSettingDetail.Clear();

            TrainingSettingDetail[] result = JsonConvert.DeserializeObject<TrainingSettingDetail[]>(strTrainingSettingDetail);

            foreach (TrainingSettingDetail item in result)
            {
                dbitem.TrainingSettingDetail.Add(new Context.TrainingSettingDetail()
                {
                    Materi = item.Materi,
                    NilaiMinimum = item.NilaiMinimum
                });
            }
        }
    }

    public class TrainingSettingDetail
    {
        public int Id { get; set; }
        public string Materi { get; set; }
        public int NilaiMinimum { get; set; }
        
        public TrainingSettingDetail()
        {

        }
        public TrainingSettingDetail(Context.TrainingSettingDetail dbitem)
        {
            Id = dbitem.Id;
            Materi = dbitem.Materi;
            NilaiMinimum = dbitem.NilaiMinimum;
        }
    }
}