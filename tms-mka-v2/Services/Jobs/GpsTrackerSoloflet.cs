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
    public class GpsTrackerSoloflet : IJob
    {
        private string DateTimeNow = DateTime.Today.ToString("yyyy-MM-dd");
        string DateTimeYesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");

        public void Execute(IJobExecutionContext context)
        {
            process_ApiSolofleet();
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
        
        #region Process Api Solofleet
        public void process_ApiSolofleet()
        {
            ContextModel context = new ContextModel();
            //* # Alur Proses # 
            //*
            //* 1. Membuat list ID Devices GPS yang digunakan oleh MKA pada vendor Solofleet, yang mana digunakan untuk looping permintaan data
            //* 2. Mengecek apakah tanggal jam sudah tersedia, jika sudah lewati jika belum simpan ke database
            //* 3. Untuk pengecekan awal akan di list data dari jam 00:00:00 hingga 23:59:59, selanjutnya di list hanya dari jam terakhir pada database hingga 23:59:59
            #region Api
            string urlApi = System.Configuration.ConfigurationManager.AppSettings["urlApiSolofleet"];
            string idApi = System.Configuration.ConfigurationManager.AppSettings["idApiSolofleet"];
            int RealId = int.Parse(idApi);
            var dataTruck = context.DataGPS.Where(d => d.IdVendor == RealId).ToList();
            
            foreach (var truk in dataTruck)
            {
                try
                {
                    string devicesId = truk.NoDevice;
                    string vehicleno = truk.DataTruck.VehicleNo;
                    string DateTimeNowApi = DateTimeApi(vehicleno);
                    DateTimeNowApi = DateTimeNowApi.Replace("%20", "T");
                    string urlApiIdData = urlApi + "/api/jsondailyreport?username=pasolo277role1082&password=kacs&gprsid=" + devicesId + "&filterStartDate=" + DateTimeNowApi + ".070Z&filterEndDate=" + DateTimeNow + "T23:59:59.070Z&type=json";
                    dynamic getDeviceData = getDataApiArray(urlApiIdData);
                    int contData = getDeviceData.Count;
                    if (contData >= 1)
                    {
                        for (int j = 0; j <= (contData - 1); j++)
                        {
                            string longitude = getDeviceData[j]["longtitude"];
                            string latitude = getDeviceData[j]["latitude"];

                            string provinsi = getDeviceData[j]["province"] != "" ? getDeviceData[j]["province"] + ", " : "";
                            provinsi = provinsi.Replace("'", "''");

                            string kabupaten = getDeviceData[j]["city"] != "" ? getDeviceData[j]["city"] + ", " : "";
                            kabupaten = kabupaten.Replace("'", "''");

                            string alamat = getDeviceData[j]["district"] != "" ? getDeviceData[j]["district"] + ", " : "";
                            alamat = alamat.Replace("'", "''");

                            string kecepatan = getDeviceData[j]["speed"];
                            string suhu = getDeviceData[j]["temperature1"];
                            string mesin = getDeviceData[j]["engineStatus"];
                            string ac = getDeviceData[j]["iP4Compressor"];

                            DateTime dtenow = getDeviceData[j]["gpstime"];
                            string createddate = dtenow.ToString("yyyy-MM-dd H:mm:ss");

//                            InsertApi(vehicleno, latitude, longitude, kecepatan, suhu, mesin, ac, provinsi, kabupaten, alamat, DateTime.Parse(createddate), "" , "Soloflet");
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            #endregion Api
            context.Dispose();
        }
        #endregion Process Api Solofleet
    }
}