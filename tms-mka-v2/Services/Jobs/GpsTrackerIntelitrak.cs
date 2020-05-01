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

namespace tms_mka_v2.Services.Jobs
{
    public class GpsTrackerIntelitrak : IJob
    {
        private string DateTimeNow = DateTime.Today.ToString("yyyy-MM-dd");
        string DateTimeYesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");

        public void Execute(IJobExecutionContext context)
        {
            //process_ApiIntellitrac();
        }

        private string DateTimeApi(string id)
        {
            ContextModel context = new ContextModel();
            string DateTimeNowApi = "";
            if (context.HistoryGps.Where(d => d.VehicleNo == id).Count() > 0)
            {
                var data = context.HistoryGps.Where(d => d.VehicleNo == id).Max(d => d.CreatedDate);

                TimeSpan start = TimeSpan.Parse(data.ToString("HH:mm:ss"));
                TimeSpan end = TimeSpan.Parse("23:59:59");

                if (start < end)
                {
                    data = data.AddSeconds(1);
                }

                DateTimeNowApi = data.ToString("yyyy-MM-dd'%'20HH:mm:ss");
            }
            else
            {
                DateTimeNowApi = DateTime.Today.ToString("yyyy-MM-dd'%'2000:00:00");            
            }

            context.Dispose();
            return DateTimeNowApi;
        }
        private dynamic getDataApi(string urlApi)
        {
            MyWebRequest myRequest = new MyWebRequest(urlApi);
            var data = myRequest.GetResponse();

            dynamic dataApi = JObject.Parse(data);

            return dataApi;
        }
        private dynamic getDataApiArray(string urlApi)
        {
            MyWebRequest myRequest = new MyWebRequest(urlApi);
            var data = myRequest.GetResponse();

            dynamic dataApi = JArray.Parse(data);

            return dataApi;
        }
        private string getDataApiString(string urlApi)
        {
            MyWebRequest myRequest = new MyWebRequest(urlApi);
            string data = myRequest.GetResponse();

            return data;
        }

        private class MyWebRequest
        {
            private WebRequest request;
            private Stream dataStream;

            private string status;

            public String Status
            {
                get
                {
                    return status;
                }
                set
                {
                    status = value;
                }
            }

            public MyWebRequest(string url)
            {
                // Create a request using a URL that can receive a post.

                request = WebRequest.Create(url);
            }

            public string GetResponse()
            {
                // Get the original response.
                request.Method = "GET";

                WebResponse response = request.GetResponse();

                this.Status = ((HttpWebResponse)response).StatusDescription;

                // Get the stream containing all content returned by the requested server.
                dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content fully up to the end.
                string responseFromServer = reader.ReadToEnd();

                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer;
            }

        }
        private void InsertApi(string VehicleNo, string Lat, string Long, string Kecepatan, string Suhu, string Mesin, string AC, string Provinsi, string Kabupaten, string Alamat, DateTime CreatedDate, string geofence, string provider)
        {
            ContextModel context = new ContextModel();
            if (context.HistoryGps.Where(d => d.VehicleNo == VehicleNo && d.Lat == Lat && d.Long == Long && d.CreatedDate == CreatedDate).Count() == 0)
            {
                VehicleNo = VehicleNo.TrimStart();
                VehicleNo = VehicleNo.TrimEnd();
                Suhu = Suhu == null || Suhu == "" ? Suhu = "0" : Suhu;
                HistoryGps dbitem = new HistoryGps() {
                    VehicleNo = VehicleNo,
                    Lat = Lat,
                    Long = Long,
                    Speed = double.Parse(Kecepatan),
                    Temp = float.Parse(Suhu),
                    Mesin = Mesin,
                    Ac = AC,
                    Provinsi = Provinsi,
                    Kabupaten = Kabupaten,
                    Alamat = Alamat,
                    CreatedDate = CreatedDate,
                    Geofence = geofence,
                    Provider = provider
                };
                
                context.HistoryGps.Add(dbitem);
                context.SaveChanges();
            }
            context.Dispose();
        }
        
        #region Process Api Intellitrac
        private void process_ApiIntellitrac()
        {
            ContextModel context = new ContextModel();
            try
            {
                //var hubConnection = new HubConnection("http://localhost:15401/signalr", useDefaultUrl: false);
                //IHubProxy alphaProxy = hubConnection.CreateHubProxy("NotificationHub");

                //hubConnection.Start().Wait();
                // Invoke method on hub

                //* # Alur Proses # 
                //*
                //* 1. Membuat list ID Devices GPS yang digunakan oleh MKA pada vendor Intellitrac, yang mana digunakan untuk looping permintaan data
                //* 2. Mengecek apakah tanggal jam sudah tersedia, jika sudah lewati jika belum simpan ke database
                //* 3. Untuk pengecekan awal akan di list data dari jam 00:00:00 hingga 23:59:59, selanjutnya di list hanya dari jam terakhir pada database hingga 23:59:59

                #region Api
                string urlApi = System.Configuration.ConfigurationManager.AppSettings["urlApiIntellitrac"];
                string idApi = System.Configuration.ConfigurationManager.AppSettings["idApiIntellitrac"];
                int RealId = int.Parse(idApi);
                var dataTruck = context.DataGPS.Where(d => d.IdVendor == RealId).ToList();
                foreach (var truck in dataTruck)
                {
                    string DateTimeNowApi = DateTimeApi(truck.DataTruck.VehicleNo);
                    string urlApiIdData = urlApi + "/history.php?username=mka_api&password=m4ngg4l4API&start_datetime=" + DateTimeNowApi + "& end_datetime=" + DateTimeNow + "%2023:59:59&devices=" + truck.NoDevice + "& filter=ignition_status;speed;temperature_2;location;longitude;latitude;geofences;input;temperature";
                    dynamic getDeviceData = getDataApi(urlApiIdData);
                    if (getDeviceData["status"] == "OK")
                    {
                        if (getDeviceData["data"][truck.NoDevice]["history"] != null)
                        {
                            int contData = getDeviceData["data"][truck.NoDevice]["history"].Count;
                            string longitude, latitude, location, provinsi, kabupaten, alamat, kecepatan, suhu, mesin, ac, createddate, geofence;
                            for (int j = 0; j <= (contData - 1); j++)
                            {
                                longitude = getDeviceData["data"][truck.NoDevice]["history"][j]["longitude"];
                                latitude = getDeviceData["data"][truck.NoDevice]["history"][j]["latitude"];
                                location = getDeviceData["data"][truck.NoDevice]["history"][j]["location"];
                                location = location.Replace("'", "''");

                                string[] words = location.Split(',');

                                provinsi = words[2];
                                kabupaten = words[1];
                                alamat = words[0];

                                kecepatan = getDeviceData["data"][truck.NoDevice]["history"][j]["speed"];
                                suhu = getDeviceData["data"][truck.NoDevice]["history"][j]["temperature_2"];
                                mesin = getDeviceData["data"][truck.NoDevice]["history"][j]["ignition_status"];
                                ac = getDeviceData["data"][truck.NoDevice]["history"][j]["input"]["02"] == 1 ? "ON" : "OFF";
                                createddate = getDeviceData["data"][truck.NoDevice]["history"][j]["local_datetime"];
                                geofence = getDeviceData["data"][truck.NoDevice]["history"][j]["geofence"];
//                                InsertApi(truck.DataTruck.VehicleNo, latitude, longitude, kecepatan, suhu, mesin, ac, provinsi, kabupaten, alamat, DateTime.Parse(createddate), geofence, "Intellitrac");
                            }
                        }
                    }
                }
                #endregion Api
            }
            catch (Exception)
            {
                
            }
            context.Dispose();
        }
        #endregion Process Api Intellitrac
    }
}