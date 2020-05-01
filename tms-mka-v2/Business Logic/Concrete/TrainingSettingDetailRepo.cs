using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Linq;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class TrainingSettingDetailRepo : ITrainingSettingDetailRepo
    {
        private ContextModel context = new ContextModel();
        public TrainingSettingDetail FindByPK(int id)
        {
            return context.TrainingSettingDetail.Where(d => d.Id == id).FirstOrDefault();
        }
    }
}