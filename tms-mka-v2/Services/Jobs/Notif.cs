using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Microsoft.AspNet.SignalR.Client;
using tms_mka_v2.Context;
using System.Data.Entity;

namespace tms_mka_v2.Services.Jobs
{
    public class Notif : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                ProsesNotif();
                ProsesTimeAlertNotif();
            }
            catch (Exception)
            {

            }
        }

        private void ProsesNotif()
        {
            ContextModel dbcontext = new ContextModel();
            string baseUrl = System.Configuration.ConfigurationManager.AppSettings["baseUrl"];
            var hubConnection = new HubConnection(baseUrl + "signalr", useDefaultUrl: false);
            IHubProxy alphaProxy = hubConnection.CreateHubProxy("NotificationHub");

            hubConnection.Start().Wait();

            var data = dbcontext.HistoryGps.GroupBy(v => v.VehicleNo).Select(grp => grp.ToList()).ToList();
            var dataTruck = dbcontext.DataTruck.ToList();
            foreach (var item in data)
            {
                foreach (var setting in dbcontext.SettingGeneral.Where(d => d.status).ToList())
                {
                    DateTime endDate = item.Max(d => d.CreatedDate);
                    DateTime strDate = DateTime.Now;
                    if (setting.overSatuan == "Menit")
                    {
                        strDate = endDate.AddMinutes(setting.over * -1);
                    }
                    else if (setting.overSatuan == "Jam")
                    {
                        strDate = endDate.AddHours(setting.over * -1);
                    }
                    else if (setting.overSatuan == "Hari")
                    {
                        strDate = endDate.AddDays(setting.over * -1);
                    }

                    var dataSpeed = item.Where(d => d.CreatedDate >= strDate && d.CreatedDate <= endDate);
                    var truk = dataTruck.Where(d => d.VehicleNo == dataSpeed.FirstOrDefault().VehicleNo).FirstOrDefault();
                    var avgSpeed = dataSpeed.Average(k => k.Speed);

                    if (setting.idProses == "Speed Alert")
                    {
                        if ((avgSpeed > truk.MaxSpeed) && (truk.MaxSpeed != 0))
                        {
                            foreach (var user in setting.idUserAlert.Split(','))
                            {
                                if(setting.AlertPopup)
                                    alphaProxy.Invoke("SendNotifications", "Kecepatan " + truk.VehicleNo + " " + avgSpeed + "KM/Jam ( Limit " + truk.MaxSpeed + " KM/Jam).", user).Wait();
                                if (setting.AlertEmail)
                                { 
                                    string BodyEmail = string.Format(
                                    "<p>Kecepatan " + truk.VehicleNo + " " + avgSpeed + "KM/Jam ( Limit " + truk.MaxSpeed + " KM/Jam)..</p>" +
                                    "<BR/><BR/><BR/> Regards,<BR/><BR/> TMS MKA Team");
                                    int IdUser = int.Parse(user);
                                    var users = dbcontext.User.Find(IdUser);
                                    alphaProxy.Invoke("SendNotificationEmail", users.Email, "Speed Alert TMS MKA", BodyEmail);
                                }
                                if (setting.AlertSound)
                                    alphaProxy.Invoke("SendNotificationSound", user);
                            }
                        }
                    }
                    if (setting.idProses == "Parking Alert")
                    {
                        //if (avgSpeed == 0 && item.FirstOrDefault().MonitoringDetailSoId != 0)
                        //{
                        //    if ((avgSpeed > decimal.Parse(truk.MaxSpeed.ToString())) && (truk.MaxSpeed != 0))
                        //    {
                        //        foreach (var user in setting.idUserAlert.Split(','))
                        //        {
                        //            if (setting.AlertPopup)
                        //                alphaProxy.Invoke("SendNotifications", "Truk dengan Nopol " + truk.VehicleNo + " melebihi batas kecepatan.", user).Wait();
                        //            else if (setting.AlertEmail)
                        //            { }
                        //            else if (setting.AlertSound)
                        //            { }
                        //        }
                        //    }
                        //}
                    }
                }
            }
            dbcontext.Dispose();
        }

        private void ProsesTimeAlertNotif()
        {
            ContextModel dbcontext = new ContextModel();
            string baseUrl = System.Configuration.ConfigurationManager.AppSettings["baseUrl"];
            var hubConnection = new HubConnection(baseUrl + "signalr", useDefaultUrl: false);
            IHubProxy alphaProxy = hubConnection.CreateHubProxy("NotificationHub");

            hubConnection.Start().Wait();

            var dataNotif = dbcontext.ListNotif.Where(d => !d.IsSend).ToList();

            foreach (var item in dataNotif)
            {
                List<TimeAlert> dataAlert = dbcontext.TimeAlert.Where(d => d.idProses == item.Type && d.status).ToList();
                foreach (var itemAlert in dataAlert)
                {
                    DateTime currDateNotif = DateTime.Now;
                    if (itemAlert.overSatuan == "Menit")
                    {
                        currDateNotif = item.CreateDate.AddMinutes(itemAlert.over);
                    }
                    else if (itemAlert.overSatuan == "Jam")
                    {
                        currDateNotif = item.CreateDate.AddHours(itemAlert.over);
                    }
                    else if (itemAlert.overSatuan == "Hari")
                    {
                        currDateNotif = item.CreateDate.AddDays(itemAlert.over);
                    }

                    if (currDateNotif <= DateTime.Now)
                    {
                        foreach (var user in itemAlert.idUserAlert.Split(','))
                        {
                            if (itemAlert.AlertPopup)
                                alphaProxy.Invoke("SendNotifications", item.PopupMsg, user);
                            if (itemAlert.AlertEmail)
                            {
                                int idUser = int.Parse(user);
                                var users = dbcontext.User.Find(idUser);
                                if (users.Email != null && users.Email != "")
                                    alphaProxy.Invoke("SendNotificationEmail", users.Email, "Alert TMS MKA", item.EmailMsg);
                            }
                            if (itemAlert.AlertSound)
                                alphaProxy.Invoke("SendNotificationSound", user);
                        }

                        item.IsSend = true;
                        dbcontext.ListNotif.Attach(item);
                        var entry = dbcontext.Entry(item);
                        entry.State = EntityState.Modified;
                        dbcontext.SaveChanges();
                    }
                }
            }
            dbcontext.Dispose();
        }
    }
}