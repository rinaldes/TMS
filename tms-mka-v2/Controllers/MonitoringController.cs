using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Models;

namespace tms_mka_v2.Controllers
{
    public class MonitoringController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private IDaftarHargaOnCallRepo RepoDaftarHarga;
        private IJenisTruckRepo RepoJnsTruck;
        private IDataTruckRepo RepoDataTruck;
        private IMonitoringVehicleRepo RepoMonitoringVehicle;
        public MonitoringController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IDaftarHargaOnCallRepo repoDaftarHarga, IJenisTruckRepo repoJnsTruck,
            IDataTruckRepo repoDataTruck, IMonitoringVehicleRepo repoMonitoringVehicle, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoDaftarHarga = repoDaftarHarga;
            RepoJnsTruck = repoJnsTruck;
            RepoDataTruck = repoDataTruck;
            RepoMonitoringVehicle = repoMonitoringVehicle;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
        }

        // GET: Monitoring
        public ActionResult Index()
        {
            List<MonitoringAll> model = new List<MonitoringAll>();
            RepoMonitoringVehicle.FindAllTruck(model);
            model = model.Where(d => (d.Lat != null && d.Lat != "") && (d.Long != null && d.Long != "")).ToList();
            ViewBag.GPSRealtimeAll = model;
            return View();
        }

        public string BindingGridAllTruck()
        {
            List<MonitoringAll> ListModel = new List<MonitoringAll>();

            RepoMonitoringVehicle.FindAllTruck(ListModel);

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }

        public string BindingGridByStat(string stat)
        {
            List<MonitoringAll> ListModel = new List<MonitoringAll>();

            RepoMonitoringVehicle.FindAllTruck(ListModel);

            ListModel = ListModel.Where(d => d.Status == stat).ToList();

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }

        public string BindingGridService()
        {
            List<MonitoringService> ListModel = new List<MonitoringService>();

            RepoMonitoringVehicle.FindService(ListModel);

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }

        public string BindingGridOntime()
        {
            List<MonitoringOntime> ListModel = new List<MonitoringOntime>();

            RepoMonitoringVehicle.FindOnTime(ListModel);

            ListModel = ListModel.Where(d => (d.Lat != null && d.Lat != "") && (d.Long != null && d.Long != "")).ToList();

            return new JavaScriptSerializer().Serialize(new { total = ListModel, data = ListModel });
        }

        public string BindingGridOntemp()
        {
            List<MonitoringOntemp> ListModel = new List<MonitoringOntemp>();

            RepoMonitoringVehicle.FindOnTemp(ListModel);

            ListModel = ListModel.Where(d => (d.Lat != null && d.Lat != "") && (d.Long != null && d.Long != "")).ToList();

            return new JavaScriptSerializer().Serialize(new { total = ListModel, data = ListModel });
        }

        public string BindingGridOnduty()
        {
            List<MonitoringOnduty> ListModel = new List<MonitoringOnduty>();

            RepoMonitoringVehicle.FindOnDuty(ListModel);

            ListModel = ListModel.Where(d => (d.Lat != null && d.Lat != "") && (d.Long != null && d.Long != "")).ToList();

            return new JavaScriptSerializer().Serialize(new { total = ListModel, data = ListModel });
        }

        [HttpPost]
        public JsonResult ReportGpsError(GpsErrorReport model)
        {
            Context.GpsErrorReport dbitem = new Context.GpsErrorReport()
            {
                Id = model.Id, GpsOnOff = model.GpsOnOff, KetGpsOnOff = model.KetGpsOnOff, LocationError = model.LocationError, KetLocationError = model.KetLocationError,
                TemperatureSensorError = model.TemperatureSensorError, KetTemperatureSensorError = model.KetTemperatureSensorError, TidakAdaGrafik = model.TidakAdaGrafik,
                KetTidakAdaGrafik = model.KetTidakAdaGrafik, GrafikRata = model.GrafikRata, KetGrafikRata = model.KetGrafikRata, GrafikPatah = model.GrafikPatah,
                KetGrafikPatah = model.KetGrafikPatah, PerbedaanSuhu = model.PerbedaanSuhu, KetPerbedaanSuhu = model.KetPerbedaanSuhu, EngineOnOff = model.EngineOnOff,
                KetEngineOnOff = model.KetEngineOnOff, AcOnOff = model.AcOnOff, KetAcOnOff = model.KetAcOnOff
            };
            RepoMonitoringVehicle.saveReportGpsError(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        [HttpPost]
        public JsonResult updateLocation(MonitoringAll model)
        {
            Context.MonitoringVehicle dbitem = RepoMonitoringVehicle.FindByVehicleNo(model.Vehicle);
            dbitem.Provinsi = model.Provinsi;
            dbitem.Kabupaten = model.KotaKab;
            dbitem.Alamat = model.Lokasi;
            RepoMonitoringVehicle.updateLocation(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        [HttpPost]
        public JsonResult saveAnalisaOnTemp(AnalisaOnTemp model)
        {
            Context.AnalisaOnTemp dbitem = new Context.AnalisaOnTemp()
            {
                Id = model.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Kondisi = model.Kondisi, Status = model.Status, Keterangan = model.Keterangan,
                VehicleNo = model.VehicleNo,
            };
            if (model.Id > 0)
            {
                dbitem = new Context.AnalisaOnTemp()
                {
                    Id = model.Id, UpdatedAt = DateTime.Now, Kondisi = model.Kondisi, Status = model.Status, Keterangan = model.Keterangan, VehicleNo = model.VehicleNo,
                };
            }
            RepoMonitoringVehicle.saveAnalisaOnTemp(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        public static DataTable GetDataTruck()
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsDefault"].ConnectionString);
            con.Open();
            using (DataTable dt = new DataTable())
            {

                var query = " SELECT \"VehicleNo\", \"Type\", \"Status\", \"LastSo\", \"Gps\", \"Engine\", \"Speed\", \"Ac\", \"Suhu\", \"Km\", \"Hm\", \"LatNew\", \"LongNew\", \"LatOld\", \"LongOld\", ";
                query += " \"Zone\", \"Provinsi\", \"Kabupaten\", \"Alamat\", \"LastUpdate\" FROM dbo.\"MonitoringVehicle\" WHERE \"LatNew\" is not null and \"LongNew\" is not null and ";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);


                cmd.Dispose();
                con.Close();

                return dt;
            }
        }

        public static DataTable GetDataTruckAll(string state=null)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsDefault"].ConnectionString);
            con.Open();
            using (DataTable dt = new DataTable())
            {
                var query = " SELECT \"VehicleNo\", \"Type\", \"Status\", \"LastSo\", \"Gps\", \"Engine\", \"Speed\", \"Ac\", \"Suhu\", \"Km\", \"Hm\", \"LatNew\", \"LongNew\", \"LatOld\", \"LongOld\", ";
                query += "\"Zone\", \"Provinsi\", \"Kabupaten\", \"Alamat\", \"LastUpdate\" FROM  dbo.\"MonitoringVehicle\" WHERE \"LatNew\" is not null and \"LongNew\" is not null ";
                if (state == "ready"){
                    query += "AND \"VehicleNo\" IN (SELECT \"VehicleNo\" FROM \"dbo\".\"DataTruck\" WHERE \"DataTruck\".\"Id\" IN (SELECT \"IdDataTruck\" FROM \"dbo\".\"SalesOrderOncall\" ";
                    query += "WHERE \"SalesOrderOnCallId\" IN (SELECT \"SalesOrderOncallId\" FROM \"dbo\".\"SalesOrder\" WHERE \"SalesOrderOncallId\" IS NOT NULL AND \"Status\" = 'save konfirmasi')))";
                }
                else if (state == "available"){
                    query += "AND \"VehicleNo\" IN (SELECT \"VehicleNo\" FROM \"dbo\".\"DataTruck\" WHERE \"DataTruck\".\"Id\" NOT IN (SELECT \"IdDataTruck\" FROM \"dbo\".\"SalesOrderOncall\"  WHERE ";
                    query += "\"SalesOrderOnCallId\" IN (SELECT \"SalesOrderOncallId\" FROM \"dbo\".\"SalesOrder\" WHERE \"SalesOrderOncallId\" IS NOT NULL AND \"Status\" IN ";
                    query += "('draft planning', 'save planning', 'draft konfirmasi', 'save konfirmasi', 'admin uang jalan', 'dispatched'))))";
                }
                query += " LIMIT 350";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Dispose();
                con.Close();
                return dt;
            }
        }

        public static DataTable GetDataTruckOnDuty()
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsDefault"].ConnectionString);
            con.Open();
            using (DataTable dt = new DataTable())
            {
                var query = " SELECT \"MonitoringVehicle\".\"VehicleNo\", \"Type\", \"MonitoringVehicle\".\"Status\", \"MonitoringVehicle\".\"LastSo\", \"MonitoringVehicle\".\"Gps\", \"NoSo\", ";
                query += " \"MonitoringVehicle\".\"Engine\", \"MonitoringVehicle\".\"Speed\", \"MonitoringVehicle\".\"Ac\", \"Suhu\", \"Km\", \"Hm\", \"LatNew\", \"LongNew\", \"LatOld\", \"LongOld\", ";
                query += "\"Zone\", \"Provinsi\", \"Kabupaten\", \"Alamat\", \"LastUpdate\", \"CustomerNama\", \"Dari\", \"Tujuan\", \"TglMuat\", \"TglBerangkat\", \"TargetTiba\", \"RangeSuhu\", ";
                query += "\"TglTiba\",\"SuhuAvg\" FROM dbo.\"MonitoringDetailSo\" INNER JOIN dbo.\"MonitoringVehicle\" ON \"MonitoringDetailSo\".\"VehicleNo\" = \"MonitoringVehicle\".\"VehicleNo\" ";
                query += "WHERE \"LatNew\" is not null and \"LongNew\" is not null AND LOWER(\"Ac\") = 'on'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Dispose();
                con.Close();
                return dt;
            }
        }

        public static DataTable GetDataTruckService(string state=null)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsDefault"].ConnectionString);
            con.Open();
            using (DataTable dt = new DataTable())
            {
                var query = "SELECT \"Workshop\".\"Status\", \"DataTruck\".\"VehicleNo\", \"Type\", \"LastSo\", \"Gps\", \"Engine\", \"Speed\", \"Ac\", \"Suhu\", \"Km\", \"Hm\", \"LatNew\",";
                query += " \"LongNew\", \"LatOld\", \"LongOld\", ";
                query += "\"Zone\", \"Provinsi\", \"Kabupaten\", \"Alamat\", \"LastUpdate\" FROM  dbo.\"MonitoringVehicle\" ";
                query += "INNER JOIN dbo.\"DataTruck\" ON \"DataTruck\".\"VehicleNo\" = \"MonitoringVehicle\".\"VehicleNo\" INNER JOIN dbo.\"Workshop\" ON \"Workshop\".\"IdVehicle\"=\"DataTruck\".\"Id\"";
                query += "WHERE \"LatNew\" is not null and \"LongNew\" is not null AND \"Workshop\".\"Status\" != 'SPK' GROUP BY \"MonitoringVehicle\".\"VehicleNo\", \"Workshop\".\"Status\", \"DataTruck\".\"VehicleNo\"";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Dispose();
                con.Close();
                return dt;
            }
        }

        public string BindingGridAvailable()
        {
            DataTable dt = GetDataTruckAll("available");
            List<MonitoringPosition> ListModel = dt.DataTableToList<MonitoringPosition>();

            return new JavaScriptSerializer().Serialize(new { total = dt.Rows.Count, data = ListModel });
        }

        public string BindingGridHistroyTruckDriver()
        {
            //DataTable dt = GetDataHistoryTruckDriver(TruckDriver, Search);
            //DataTable dt = GetDataHistoryTruckDriver();
            //List<HistoryTruckDriver> ListModel = dt.DataTableToList<HistoryTruckDriver>();

            //return new JavaScriptSerializer().Serialize(new { total = dt.Rows.Count, data = ListModel });
            return "";
        }

        public List<MonitoringPosition> GPSRealtimeAll()
        {
            List<MonitoringPosition> gisModels = new List<MonitoringPosition>();
            DataTable dt = GetDataTruckAll();

            gisModels = (from DataRow dr in dt.Rows
                         select new MonitoringPosition()
                         {
                             VehicleNo = dr["VehicleNo"].ToString(),
                             Type = dr["Type"].ToString(),
                             Status = dr["Status"].ToString(),
                             //LastSo = dr["LastSo"].ToString(),
                             Gps = dr["Gps"].ToString(),
                             Engine = dr["Engine"].ToString(),
                             Speed = dr["Speed"].ToString(),
                             Ac = dr["Ac"].ToString(),
                             Suhu = dr["Suhu"].ToString(),
                             Km = dr["Km"].ToString(),
                             Hm = dr["Hm"].ToString(),
                             LatNew = dr["LatNew"].ToString(),
                             LongNew = dr["LongNew"].ToString(),
                             LatOld = dr["LatOld"].ToString() == "" ? dr["LatNew"].ToString() : dr["LatOld"].ToString(),
                             LongOld = dr["LongOld"].ToString() == "" ? dr["LongNew"].ToString() : dr["LongOld"].ToString(),
                             Zone = dr["Zone"].ToString(),
                             Provinsi = dr["Provinsi"].ToString(),
                             Kabupaten = dr["Kabupaten"].ToString(),
                             Alamat = dr["Alamat"].ToString(),
                             LastUpdate = dr["LastUpdate"].ToString()
                         }).ToList();

            return gisModels;
        }

        public string getCoordinate()
        {
            //List<MonitoringOnduty> gisModels = new List<MonitoringOnduty>();
            //DataTable dt = GetDataTruck();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    var newGISModels = new MonitoringOnduty();

            //    newGISModels.VehicleNo = dt.Rows[i]["VehicleNo"].ToString();
            //    newGISModels.Kecepatan = dt.Rows[i]["Kecepatan"].ToString();
            //    newGISModels.Alamat = dt.Rows[i]["Alamat"].ToString();
            //    newGISModels.Long = dt.Rows[i]["Long"].ToString();
            //    newGISModels.Lat = dt.Rows[i]["Lat"].ToString();
            //    newGISModels.LongFirst = dt.Rows[i]["LongFirst"].ToString();
            //    newGISModels.LatFisrt = dt.Rows[i]["LatFisrt"].ToString();
            //    newGISModels.StatusOrder = dt.Rows[i]["StatusOrder"].ToString();
            //    newGISModels.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
            //    newGISModels.Status = dt.Rows[i]["Status"].ToString();
            //    newGISModels.Suhu = dt.Rows[i]["Suhu"].ToString();

            //    gisModels.Add(newGISModels);
            //}

            //return new JavaScriptSerializer().Serialize(gisModels.Select(d => new
            //{
            //    VehicleNo = d.VehicleNo,
            //    Kecepatan = d.Kecepatan,
            //    Alamat = d.Alamat,
            //    Long = d.Long,
            //    Lat = d.Lat,
            //    LongFirst = d.LongFirst,
            //    LatFisrt = d.LatFisrt,
            //    StatusOrder = d.StatusOrder,
            //    CreatedDate = d.CreatedDate,
            //    Status = d.Status,
            //    Suhu = d.Suhu,
            //}));

            return "";
        }

        public string OnDutyDetail(string VehicleNo, string NoSo){
            Context.DataTruck item = RepoDataTruck.FindByName(VehicleNo);
            UnitList unit = new UnitList(item);
            Context.MonitoringDetailSo mon_so = RepoMonitoringVehicle.FindMonitoringDetailSo(NoSo);

            return new JavaScriptSerializer().Serialize(new {unit = unit, Delay = mon_so.TglTiba - mon_so.TargetTiba});
        }

        public string getCoordinateAll()
        {
            List<MonitoringOnduty> gisModels = new List<MonitoringOnduty>();
            DataTable dt = GetDataTruckAll();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var newGISModels = new MonitoringOnduty();
                gisModels.Add(newGISModels);
            }
            return new JavaScriptSerializer().Serialize(gisModels.Select(d => new {}));
        }

        public ActionResult Detail(string VehicleNo)
        {
            ViewBag.MonitoringPosition = RepoMonitoringVehicle.FindByVehicleNo(VehicleNo);
            Context.DataTruck truck = RepoDataTruck.FindByName(VehicleNo);
            ViewBag.Speed = RepoMonitoringVehicle.FindByVehicleNo(VehicleNo).Kecepatan > 0 ? RepoMonitoringVehicle.FindByVehicleNo(VehicleNo).Kecepatan : 0;
            if (RepoMonitoringVehicle.FindByVehicleNo(VehicleNo) != null)
            {
                ViewBag.Pendingin = truck.DataPendingin.FirstOrDefault().Model;
            }
            ViewBag.MonitoringSo = RepoMonitoringVehicle.FindMonitoringDetailSoByVehicleNo(VehicleNo);
            ViewBag.DataUnit = RepoDataTruck.FindByName(VehicleNo);
            return View("Detail");
        }

        public String BindingSO(string VehicleNo)
        {
            List<MonitoringDetailSo> ListModel = new List<MonitoringDetailSo>();
            List<Context.MonitoringDetailSo> items = RepoMonitoringVehicle.FindAllMonitoringDetailSoByVehicleNo(VehicleNo);
            foreach (Context.MonitoringDetailSo item in items)
            {
                MonitoringDetailSo mso = new MonitoringDetailSo(item);
                Context.SalesOrder so = RepoSalesOrder.FindByCode(item.NoSo);
                if (so != null)
                {
                    if (so.SalesOrderOncallId.HasValue)
                    {
                        mso.JenisProduk = so.SalesOrderOncall.MasterProduct.NamaProduk;
                    }
                    else if (so.SalesOrderPickupId.HasValue)
                    {
                        mso.JenisProduk = so.SalesOrderPickup.MasterProduct.NamaProduk;
                    }
                }
                else
                {
                    Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByNoSo(item.NoSo);
                    so = RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value);
                    mso.JenisProduk = so.SalesOrderKontrak.MasterProduct.NamaProduk;
                }
                ListModel.Add(mso);
            }
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
/*
        public String getTemp(string VehicleNo, string StartTime, string EndTime)
        {
            List<HistoryGps> ListModel = new List<HistoryGps>();
            List<Context.HistoryGps> items = RepoMonitoringVehicle.FindAllHistory1Gps(VehicleNo, StartTime, EndTime);
            return new JavaScriptSerializer().Serialize(new
            {
                Avg = items.Select(d => d.Temp).Average(), Min = items.Select(d => d.Temp).Min(), Max = items.Select(d => d.Temp).Max() 
            });
        }

        public String BindingHistory(string VehicleNo, string StartTime, string EndTime)
        {
            List<HistoryGps> ListModel = new List<HistoryGps>();
            List<Context.HistoryGps> items = RepoMonitoringVehicle.FindAllHistory1Gps(VehicleNo, StartTime, EndTime);
            foreach (Context.HistoryGps item in items)
            {
                HistoryGps mso = new HistoryGps(item);
                ListModel.Add(mso);
            }
            return new JavaScriptSerializer().Serialize(new { total = 1, data = ListModel });
        }

        public String BindingTempAlert(string VehicleNo, string StartTime, string EndTime)
        {
            List<HistoryGps> ListModel = new List<HistoryGps>();
            List<Context.HistoryGps> items = RepoMonitoringVehicle.FindAllHistory1Gps(VehicleNo, StartTime, EndTime);
            float PrevTemp = 0;
            DateTime PrevCreatedAt = DateTime.Now;
            DateTime PrevUntil = DateTime.Now;
            DateTime? PresentStart = DateTime.Now;
            foreach (Context.HistoryGps item in items)
            {
                HistoryGps mso = new HistoryGps(item);
                Context.MonitoringDetailSo mdso = RepoMonitoringVehicle.FindMonitoringDetailSoByPK(item.MonitoringDetailSoId);
                mso.PrevTemp = PrevTemp;
                if (mdso != null && mdso.RangeSuhu != "-")
                {
                    mso.MaxTemp = float.Parse(mdso.RangeSuhu.Split(' ')[2].Replace(".", ","));
                    mso.MinTemp = float.Parse(mdso.RangeSuhu.Split(' ')[0].Replace(".", ","));
                    mso.OutOfTemp = mso.Temp > mso.MaxTemp || mso.Temp < mso.MinTemp;
                    if (mso.OutOfTemp)
                    {
                        Context.HistoryGps nextHst = RepoMonitoringVehicle.FindNextHistory1GpsByPK(item.Id, VehicleNo);
                        float NextMaxTemp = float.Parse(mdso.RangeSuhu.Split(' ')[2].Replace(".", ","));
                        float NextMinTemp = float.Parse(mdso.RangeSuhu.Split(' ')[0].Replace(".", ","));
                        bool NextOutOfTemp = mso.PrevTemp > NextMaxTemp || mso.PrevTemp < NextMinTemp;
                        if (nextHst != null)
                            mso.OutOfTempUntil = nextHst.CreatedDate;
                        mso.Durasi = mso.OutOfTempUntil - mso.CreatedDate;
                        mso.StrDurasi = (mso.Durasi.Hours > 0 ? mso.Durasi.Hours + " Hour " : "") + mso.Durasi.Minutes + " Minutes";
                        mso.TargetSuhu = mdso.RangeSuhu;
                        ListModel.Add(mso);
                    }
                    PrevTemp = mso.Temp;
                }
            }
            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
*/
        public String BindingOnTemp(string VehicleNo, string StartTime, string EndTime)
        {
            List<AnalisaOnTemp> ListModel = new List<AnalisaOnTemp>();
            List<Context.AnalisaOnTemp> items = RepoMonitoringVehicle.FindAllAnalisaOnTemp(VehicleNo, StartTime, EndTime);
            foreach (Context.AnalisaOnTemp item in items)
            {
                AnalisaOnTemp mso = new AnalisaOnTemp(item);
                ListModel.Add(mso);
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoMonitoringVehicle.CountAllAnalisaOnTemp(VehicleNo, StartTime, EndTime), data = ListModel });
        }
        
        public String BindingOntime(string VehicleNo)
        {
            List<MonitoringDetailSo> ListModel = new List<MonitoringDetailSo>();
            List<Context.MonitoringDetailSo> items = RepoMonitoringVehicle.FindAllMonitoringDetailSoByVehicleNo(VehicleNo);
            foreach (Context.MonitoringDetailSo item in items)
            {
                MonitoringDetailSo mso = new MonitoringDetailSo(item);
                Context.SalesOrder so = RepoSalesOrder.FindByOnCallCode(item.NoSo);
                mso.JenisProduk = so.SalesOrderOncall.MasterProduct.NamaProduk;
                ListModel.Add(mso);
            }
            return new JavaScriptSerializer().Serialize(new { total = 1, data = ListModel });
        }
    }
}