using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using tms_mka_v2.Services.Jobs;

namespace tms_mka_v2.Services.Config
{
    public static class SchedulerConfig
    {
        public static void Start()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();

            #region service intelitrak
            IJobDetail jobGpsTrackerIntelitrak = JobBuilder.Create<GpsTrackerIntelitrak>()
                .WithIdentity("jobGpsTrackerIntelitrak")
                .Build();

            ITrigger triggerGpsTrackerIntelitrak = TriggerBuilder.Create()
              .WithIdentity("triggerGpsTrackerIntelitrak")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(180)
                  .RepeatForever())
              .Build();

            Quartz.IScheduler scGpsTrackerIntelitrak = sf.GetScheduler();
            scGpsTrackerIntelitrak.ScheduleJob(jobGpsTrackerIntelitrak, triggerGpsTrackerIntelitrak);
            scGpsTrackerIntelitrak.Start();
            #endregion

            #region service solofleet
            IJobDetail jobGpsTrackerSolofleet = JobBuilder.Create<GpsTrackerSoloflet>()
                .WithIdentity("jobGpsTrackerSolofleet")
                .Build();

            ITrigger triggerGpsTrackerSolofleet = TriggerBuilder.Create()
              .WithIdentity("triggerGpsTrackerSolofleet")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(180)
                  .RepeatForever())
              .Build();

            Quartz.IScheduler scGpsTrackerSolofleet = sf.GetScheduler();
            scGpsTrackerSolofleet.ScheduleJob(jobGpsTrackerSolofleet, triggerGpsTrackerSolofleet);
            scGpsTrackerSolofleet.Start();
            #endregion

            #region service trekq
            IJobDetail jobGpsTrackerTrekq = JobBuilder.Create<GpsTrackerTrekq>()
                .WithIdentity("jobGpsTrackerTrekq")
                .Build();

            ITrigger triggerGpsTrackerTreq = TriggerBuilder.Create()
              .WithIdentity("triggerGpsTrackerTreq")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(180)
                  .RepeatForever())
              .Build();

            Quartz.IScheduler scGpsTrackerTrekq = sf.GetScheduler();
            scGpsTrackerTrekq.ScheduleJob(jobGpsTrackerTrekq, triggerGpsTrackerTreq);
            scGpsTrackerTrekq.Start();
            #endregion

            #region service update data truk
            IJobDetail jobUpdateDataTruck = JobBuilder.Create<UpdateDataTruk>()
                .WithIdentity("jobUpdateDataTruck")
                .Build();

            ITrigger triggerUpdateDataTruck = TriggerBuilder.Create()
              .WithIdentity("triggerUpdateDataTruck")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(120)
                  .RepeatForever())
              .Build();

            Quartz.IScheduler scUpdateDataTruck = sf.GetScheduler();
            scUpdateDataTruck.ScheduleJob(jobUpdateDataTruck, triggerUpdateDataTruck);
            scUpdateDataTruck.Start();
            #endregion

            #region service Notif
            IJobDetail jobNotif = JobBuilder.Create<Notif>()
                .WithIdentity("jobNotif")
                .Build();

            ITrigger triggerNotif = TriggerBuilder.Create()
              .WithIdentity("triggerNotif")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(120)
                  .RepeatForever())
              .Build();

            Quartz.IScheduler scNotif = sf.GetScheduler();
            scNotif.ScheduleJob(jobNotif, triggerNotif);
            scNotif.Start();
            #endregion
        }
    }
}