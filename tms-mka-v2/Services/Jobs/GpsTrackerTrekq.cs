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
    public class GpsTrackerTrekq : IJob
    {
        private string DateTimeNow = DateTime.Today.ToString("yyyy-MM-dd");
        string DateTimeYesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");

        public void Execute(IJobExecutionContext context)
        {
            process_ApiTrekq();
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
        
        #region Process Api Trekq
        public void process_ApiTrekq()
        {
            //* # Alur Proses # 
            //*
            //* 1. Mendapatkan data langsung ke API Trekq dengan range tanggal
            //* 2. Mengecek apakah tanggal jam sudah tersedia, jika sudah lewati jika belum simpan ke database
            //* 3. Untuk pengecekan awal akan di list data dari jam 17:00:00 hingga 16:59:59, selanjutnya di list hanya dari jam terakhir pada database hingga 23:59:59
            //*    Jam menggunakan GMT+7
            //* Username mkajumbo pswd mka2015

            #region Api
            string urlApi = System.Configuration.ConfigurationManager.AppSettings["urlApiTrekq"];
            //string urlApiId = urlApi + "/kiatananda/tarikdata.php?data=KIATANANDA,mkatrekq,ALL," + DateTimeNow + "%2000:00:00," + DateTimeNow + "%2023:59:59";
            string urlApiId = urlApi + "/kiatananda/tarikdata.php?data=KIATANANDA,mkatrekq,ALL," + DateTimeYesterday + "%2017:00:00," + DateTimeNow + "%2016:59:59";
            string devicesId, vehicleno, longitude, latitude, kecepatan, suhu, mesin, ac;
            string createddate = "";
            string provinsi, kabupaten, alamat;

            //dynamic getDeviceListId = getDataApi(urlApiId);

            string data = getDataApiString(urlApiId);

            string[] lines = Regex.Split(data, "<br>");

            foreach (string line in lines)
            {
                string[] words = line.Split(',');

                if (words[0] == "")
                {
                    break;
                }

                devicesId = words[0];
                vehicleno = words[1];
                string DateTimeNowApi = DateTimeApi(vehicleno);
                DateTimeNowApi = DateTimeNowApi.Replace("%20", " ");

                DateTime dateApiDb = Convert.ToDateTime(DateTimeNowApi);
                dateApiDb = dateApiDb.AddHours(-7);
                DateTime dateApiNow = Convert.ToDateTime(words[9]);
                dateApiNow = dateApiNow.AddHours(7);

                if (dateApiNow > dateApiDb)
                {
                    provinsi = "";
                    kabupaten = "";
                    alamat = "";

                    //string reverseGeolocation = "http://nominatim.openstreetmap.org/search.php?q=" + words[3] + "%2C+" + words[4] + "&accept-language=id";
                    //string htmlText = GetWebText(reverseGeolocation);

                    //string toFind1 = "langaddress\": \"";
                    //string toFind2 = "\",\n";
                    //int start = htmlText.IndexOf(toFind1) + toFind1.Length;
                    //int end = htmlText.IndexOf(toFind2, start);
                    //myCapturedAlamat = htmlText.Substring(start, end - start);

                    //string urlApireverseGeolocation = "http://nominatim.openstreetmap.org/reverse?format=json&lat=" + words[3] + "&lon=" + words[4] + "&zoom=18&addressdetails=1&accept-language=id";
                    string urlApireverseGeolocation = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + words[3] + "," + words[4] + "&sensor=false&language=id&region=id";
                    dynamic getreverseGeolocation = getDataApi(urlApireverseGeolocation);

                    int contData = getreverseGeolocation["results"][0]["address_components"].Count;
                    for (int j = 0; j <= (contData - 1); j++)
                    {

                        if (getreverseGeolocation["results"][0]["address_components"][j]["types"][0] == "administrative_area_level_3")
                        {
                            alamat = getreverseGeolocation["results"][0]["address_components"][j]["long_name"];
                            alamat = alamat.Replace("'", "''");
                        }

                        if (getreverseGeolocation["results"][0]["address_components"][j]["types"][0] == "administrative_area_level_2")
                        {
                            kabupaten = getreverseGeolocation["results"][0]["address_components"][j]["long_name"];
                            kabupaten = kabupaten.Replace("'", "''");
                        }

                        if (getreverseGeolocation["results"][0]["address_components"][j]["types"][0] == "administrative_area_level_1")
                        {
                            provinsi = getreverseGeolocation["results"][0]["address_components"][j]["long_name"];
                            provinsi = provinsi.Replace("'", "''");
                        }

                    }

                    if (provinsi != "" && kabupaten != "" && alamat != "")
                    {
                        longitude = words[4];
                        latitude = words[3];

                        kecepatan = words[8];
                        suhu = words[5];
                        mesin = words[6];
                        ac = words[7];

                        createddate = dateApiNow.ToString("yyyy-MM-dd H:mm:ss");

//                        InsertApi(vehicleno, latitude, longitude, kecepatan, suhu, mesin, ac, provinsi, kabupaten, alamat, DateTime.Parse(createddate), "", "trekQ");
                    }

                }

            }
            #endregion Api
        }
        #endregion Process Api Intellitrac
    }
}