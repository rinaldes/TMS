using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Quartz;
using tms_mka_v2.Context;

namespace tms_mka_v2.Services.Jobs
{
    public class UpdateDataTruk : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //InsertDataTrukMonitoring();
            //UpdateDataTrukMonitoringTrim();
//            UpdateDataTrukMonitoring();
        }

        public void UpdateDataTrukMonitoring()
        {
            ContextModel context = new ContextModel();

            var grpGps = context.HistoryGps.GroupBy(g => g.VehicleNo).Select(grp => grp.ToList()).ToList();


            foreach (var item in grpGps)
            {
                DateTime endDate = item.Max(d => d.CreatedDate).AddMinutes(-3);
                var dataHistory = item.OrderByDescending(d => d.CreatedDate).FirstOrDefault();
                var dummyItem = item.Where(d => d.CreatedDate >= endDate).OrderByDescending(d => d.CreatedDate).ToList();
                double avgSpedd = 0;
                if (dummyItem.Sum(s => s.Speed) > 0)
                    avgSpedd = dummyItem.Average(s => s.Speed);
                string rangSuhu = "0 to 0";
                float AvgSuhu = 0;
                if (dummyItem.Count > 0)
                {
                    rangSuhu = dummyItem.Min(s => s.Temp).ToString() + " to " + dummyItem.Max(s => s.Temp).ToString();
                    AvgSuhu = dummyItem.Average(s => s.Temp);
                }

                var truk = context.MonitoringVehicle.Where(t => t.VehicleNo == dataHistory.VehicleNo).FirstOrDefault();
                if (truk != null)
                {
                    truk.Gps = "ON";
                    truk.Engine = dataHistory.Mesin;
                    truk.Kecepatan = dataHistory.Speed;
                    truk.Ac = dataHistory.Ac;
                    truk.Suhu = dataHistory.Temp.ToString();
                    truk.LatNew = dataHistory.Lat;
                    truk.LongNew = dataHistory.Long;
                    truk.Provinsi = dataHistory.Provinsi;
                    truk.Kabupaten = dataHistory.Kabupaten;
                    truk.Alamat = dataHistory.Alamat;
                    truk.Zone = dataHistory.Geofence;
                    truk.RangeAc = rangSuhu;
                    truk.AvgSpeed = avgSpedd;
                    truk.LastUpdate = dataHistory.CreatedDate;
                    truk.AvgSuhu = AvgSuhu;

                    context.MonitoringVehicle.Attach(truk);
                    var entry = context.Entry(truk);
                    entry.State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            context.Dispose();            
        }

        public void UpdateDataTrukMonitoringTrim()
        {
            ContextModel context = new ContextModel();

            var dataTruck = context.DataTruck.ToList();
            foreach (var item in dataTruck)
            {
                item.VehicleNo = item.VehicleNo.Trim();
                context.DataTruck.Attach(item);
                var entry = context.Entry(item);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
            context.Dispose();
        }

        public void InsertDataTrukMonitoring()
        {
            ContextModel context = new ContextModel();

            var dataMonitoring = context.MonitoringVehicle.Select(d => d.VehicleNo).ToList();
            var dataTruck = context.DataTruck.ToList();
            dataTruck = dataTruck.Where(d => !dataMonitoring.Contains(d.VehicleNo)).ToList();

            foreach (var item in dataTruck)
            {
                context.MonitoringVehicle.Add(new MonitoringVehicle() { VehicleNo = item.VehicleNo, Type = item.JenisTrucks.StrJenisTruck, LastUpdate = DateTime.Now });
                context.SaveChanges();
            }
            context.Dispose();

        }
    }
}