using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tms_mka_v2.Business_Logic.Abstract;
using System.Web.Script.Serialization;
using tms_mka_v2.Models;

namespace tms_mka_v2.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IJenisTruckRepo RepoJenisTruck;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private IWorkshopRepo RepoWorkshop;
        private IDataTruckRepo RepoDataTruck;
        private IPenetapanaDriverRepo RepoPenetapanDriver;
        private IDriverRepo RepoDriver;
        private ITrainingSettingRepo RepoTrainingSetting;
        private IPelaksanaanTrainingRepo RepoPelaksanaanTraining;
        private IMonitoringVehicleRepo RepoMonitoringVehicle;
        private IDataGPSRepo RepoDataGps;
        public HomeController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IJenisTruckRepo repoJenisTruck, IDataTruckRepo repoDataTruck,
            ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo, IWorkshopRepo repoWorkshop, IPenetapanaDriverRepo repoPenetapanDriver, IDriverRepo repoDriver,
                ITrainingSettingRepo repoTrainingSetting, IPelaksanaanTrainingRepo repoPelaksanaanTraining, IMonitoringVehicleRepo repoMonitoringVehicle, IDataGPSRepo repoDataGps)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoJenisTruck = repoJenisTruck;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
            RepoWorkshop = repoWorkshop;
            RepoDataTruck = repoDataTruck;
            RepoPenetapanDriver = repoPenetapanDriver;
            RepoDriver = repoDriver;
            RepoTrainingSetting = repoTrainingSetting;
            RepoPelaksanaanTraining = repoPelaksanaanTraining;
            RepoMonitoringVehicle = repoMonitoringVehicle;
            RepoDataGps = repoDataGps;
        }
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.MarketingApprovalNeed = RepoSalesOrder.FindAllOnCall().Where(d => d.Status == "draft").Count();
            ViewBag.MarketingApprovalNeedColour = RepoSalesOrder.FindAllOnCall().Where(d => d.Status == "draft" && d.SalesOrderOncall.TanggalMuat < DateTime.Now.AddDays(1)).Count() > 0 ? "red" : "yellow";
            ViewBag.PlanningApprovalNeed = RepoSalesOrder.FindAllOnCall().Where(d => d.Status == "save" || d.Status == "draft planning").Count();
            ViewBag.PlanningApprovalNeedColour = RepoSalesOrder.FindAllOnCall().Where(d => (d.Status == "save" || d.Status == "draft planning") && d.SalesOrderOncall.TanggalMuat < DateTime.Now.AddDays(1))
                .Count() > 0 ? "red" : "yellow";
            ViewBag.KonfirmasiApprovalNeed = RepoSalesOrder.FindAllOnCall().Where(d => d.Status == "save planning" || d.Status == "draft konfirmasi").Count();
            ViewBag.KonfirmasiApprovalNeedColour = RepoSalesOrder.FindAllOnCall()
                .Where(d => (d.Status == "save planning" || d.Status == "draft konfirmasi") && d.SalesOrderOncall.TanggalMuat < DateTime.Now.AddDays(1)).Count() > 0 ? "red" : "yellow";
            ViewBag.AUJApprovalNeed = RepoSalesOrder.FindAllOnCall().Where(d => d.Status == "save konfirmasi").Count();
            ViewBag.AUJApprovalNeedColour = RepoSalesOrder.FindAllOnCall().Where(d => (d.Status == "save konfirmasi") && d.SalesOrderOncall.TanggalMuat < DateTime.Now.AddDays(1))
                .Count() > 0 ? "red" : "yellow";
            ViewBag.KasirApprovalNeed = RepoSalesOrder.FindAllOnCall().Where(d => d.Status == "admin uang jalan").Count();
            ViewBag.KasirApprovalNeedColour = RepoSalesOrder.FindAllOnCall().Where(d => (d.Status == "admin uang jalan") && d.SalesOrderOncall.TanggalMuat < DateTime.Now.AddDays(1))
                .Count() > 0 ? "red" : "yellow";
            return View();
        }

        public String BindingSalesBulanBerjalan()
        {
            List<Context.JenisTrucks> items = RepoJenisTruck.FindAll();
            return new JavaScriptSerializer().Serialize(items.Select(d => new
            {
                JenisTruck = d.StrJenisTruck,
                Nominal = RepoSalesOrder.FindAll().Where(f => f.Status == "settlement" && f.SalesOrderOncall.JenisTrucks.StrJenisTruck == d.StrJenisTruck).Sum(e => RepoSalesOrder.Harga(e)),
                Percentage = RepoSalesOrder.FindAll().Where(f => f.Status == "settlement" && f.SalesOrderOncall.JenisTrucks.StrJenisTruck == d.StrJenisTruck)
                    .Sum(e => RepoSalesOrder.Harga(e))/(RepoSalesOrder.FindAll().Where(f => f.Status == "settlement")
                    .Sum(e => RepoSalesOrder.Harga(e)) == 0 ? 1 : RepoSalesOrder.FindAll().Where(f => f.Status == "settlement").Sum(e => RepoSalesOrder.Harga(e)))*100,
                Total = RepoSalesOrder.FindAll().Where(f => f.Status == "settlement").Sum(e => RepoSalesOrder.Harga(e))
            }));
        }

        public String GetSales()
        {
            List<Dashboard> ListModel = new List<Dashboard>();
            double month_1 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-01-01"));
            double month_2 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-02-01"));
            double month_3 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-03-01"));
            double month_4 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-04-01"));
            double month_5 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-05-01"));
            double month_6 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-06-01"));
            double month_7 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-07-01"));
            double month_8 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-08-01"));
            double month_9 = RepoSalesOrder.GroupPerMonth(DateTime.Parse("2017-09-01"));
            ListModel.Add(new Dashboard("Jan", month_1, "3"));
            ListModel.Add(new Dashboard("Feb", month_2, "3"));
            ListModel.Add(new Dashboard("Mar", month_3, "3"));
            ListModel.Add(new Dashboard("Apr", month_4, "3"));
            ListModel.Add(new Dashboard("Mei", month_5, "3"));
            ListModel.Add(new Dashboard("Jun", month_6, "3"));
            ListModel.Add(new Dashboard("Jul", month_7, "3"));
            ListModel.Add(new Dashboard("Agt", month_8, "3"));
            ListModel.Add(new Dashboard("Sep", month_9, "3"));
            ListModel.Add(new Dashboard("AVG", (month_7+month_8)/2, "3"));

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }

        public String BindingPlanningKontrak(){
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKontrak().Where(d => d.Status == "save" || d.Status == "draft planning").ToList();

            return new JavaScriptSerializer().Serialize(items.Select(e => new{
                Tanggal = DateTime.Parse(e.SalesOrderKontrak.PeriodStr.ToString()).ToString("yyyy-MM-dd"),
                OrderCount = RepoSalesOrder.FindAll().Where(d =>
                    (d.SalesOrderKontrakId != null) && (d.Status == "save" || d.Status == "draft planning") && 
                    DateTime.Parse(d.SalesOrderKontrak.PeriodStr.ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(e.SalesOrderKontrak.PeriodStr.ToString()).ToString("yyyy-MM-dd")
                ).Count()
            }));
        }

        public String BindingPlanningOncall(){
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "save" || d.Status == "draft planning")).GroupBy(
                d => DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
            ).Select(d => d.First()).ToList();
            return new JavaScriptSerializer().Serialize(items.Select(e => new{
                Tanggal = DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd"),
                OrderCount = RepoSalesOrder.FindAll().Where(d =>
                    (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "save" || d.Status == "draft planning") && 
                    DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
                ).Count()
            }));
        }

        public String BindingKonfirmasiKontrak(){
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKontrak().Where(d => d.Status == "save planning" || d.Status == "draft konfirmasi").ToList();

            return new JavaScriptSerializer().Serialize(items.Select(e => new{
                Tanggal = DateTime.Parse(e.SalesOrderKontrak.PeriodStr.ToString()).ToString("yyyy-MM-dd"),
                OrderCount = RepoSalesOrder.FindAll().Where(d =>
                    (d.SalesOrderKontrakId != null) && (d.Status == "save planning" || d.Status == "draft konfirmasi") && 
                    DateTime.Parse(d.SalesOrderKontrak.PeriodStr.ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(e.SalesOrderKontrak.PeriodStr.ToString()).ToString("yyyy-MM-dd")
                ).Count()
            }));
        }

        public String BindingKonfirmasiOncall(){
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "save planning" || d.Status == "draft konfirmasi")).GroupBy(
                d => DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
            ).Select(d => d.First()).ToList();
            return new JavaScriptSerializer().Serialize(items.Select(e => new{
                Tanggal = DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd"),
                OrderCount = RepoSalesOrder.FindAll().Where(d =>
                    (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "save planning" || d.Status == "draft konfirmasi") && 
                    DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
                ).Count()
            }));
        }

        public String BindingAUJOncall(){
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "save konfirmasi")).GroupBy(
                d => DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
            ).Select(d => d.First()).ToList();
            return new JavaScriptSerializer().Serialize(items.Select(e => new{
                Tanggal = DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd"),
                OrderCount = RepoSalesOrder.FindAll().Where(d =>
                    (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "save konfirmasi") && 
                    DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
                ).Count()
            }));
        }

        public String BindingKasirTransfer(){
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "dispatched")).GroupBy(
                d => DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
            ).Select(d => d.First()).ToList();
            return new JavaScriptSerializer().Serialize(items.Select(e => new{
                Tanggal = DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd"),
                OrderCount = RepoSalesOrder.FindAll().Where(d =>
                    (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) && (d.Status == "dispatched") && 
                    DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(e.SalesOrderOncall.TanggalMuat.ToString()).ToString("yyyy-MM-dd")
                ).Count()
            }));
        }

        public String BindingPlanningWaktuProses(){
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string dfltTgl = "01/01/0001 00.00.00";
            List<Context.OrderHistory> items = RepoSalesOrder.FindAllPlanningHistory().Where(d => 
                RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat >= firstDayOfMonth && RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat <= lastDayOfMonth
            ).ToList();
            List<WaktuProses> ListModel = new List<WaktuProses>();
            ListModel.Add(new WaktuProses("< 1 jam",
                items.Where(d => RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null && d.ProcessedAt.ToString() != dfltTgl
                    && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count()));
            ListModel.Add(new WaktuProses("< 4 jam", items.Where(d => RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null &&
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count()));
            ListModel.Add(new WaktuProses("< 8 jam", items.Where(d => RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null &&
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count()));
            ListModel.Add(new WaktuProses("> 8 jam",
                items.Where(d => RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null && d.ProcessedAt.ToString() != dfltTgl && 
                    (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count()
            ));

            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingKonfirmasiWaktuProses(){
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string dfltTgl = "01/01/0001 00.00.00";
            List<Context.OrderHistory> items = RepoSalesOrder.FindAllKonfirmasiHistory().Where(d => 
                RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat >= firstDayOfMonth && RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat <= lastDayOfMonth
            ).ToList();
            List<WaktuProses> ListModel = new List<WaktuProses>();
            ListModel.Add(new WaktuProses("< 1 jam",
                items.Where(d => 
                    RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && d.ProcessedAt.ToString() != dfltTgl &&
                    (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1
                ).Count()));
            ListModel.Add(new WaktuProses("< 4 jam", items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count()));
            ListModel.Add(new WaktuProses("< 8 jam", items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count()));
            ListModel.Add(new WaktuProses("> 8 jam",
                items.Where(d => 
                    RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && d.ProcessedAt.ToString() != dfltTgl && 
                    (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8
                ).Count()));

            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingAUJWaktuProses(){
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string dfltTgl = "01/01/0001 00.00.00";
            List<Context.OrderHistory> items = RepoSalesOrder.FindAllAUJHistory().Where(d => 
                RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat >= firstDayOfMonth && RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat <= lastDayOfMonth
            ).ToList();
            List<WaktuProses> ListModel = new List<WaktuProses>();
            ListModel.Add(new WaktuProses("< 1 jam",
                items.Where(d =>  RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null && d.ProcessedAt.ToString() != dfltTgl &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count()));
            ListModel.Add(new WaktuProses("< 4 jam", items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count()));
            ListModel.Add(new WaktuProses("< 8 jam", items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count()));
            ListModel.Add(new WaktuProses("> 8 jam",
                items.Where(d => 
                    RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null && d.ProcessedAt.ToString() != dfltTgl &&
                    (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8
                ).Count()));

            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingKasirWaktuProses(){
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string dfltTgl = "01/01/0001 00.00.00";
            List<Context.OrderHistory> items = RepoSalesOrder.FindAllAUJHistory().Where(d => 
                RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat >= firstDayOfMonth && RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat <= lastDayOfMonth
            ).ToList();
            List<WaktuProses> ListModel = new List<WaktuProses>();
            ListModel.Add(new WaktuProses("< 1 jam",
                items.Where(d =>  d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count()));
            ListModel.Add(new WaktuProses("< 4 jam", items.Where(d => 
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count()));
            ListModel.Add(new WaktuProses("< 8 jam", items.Where(d => 
                d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count()));
            ListModel.Add(new WaktuProses("> 8 jam",
                items.Where(d => d.ProcessedAt.ToString() != dfltTgl && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count()));

            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingRepairMaintenance(){
            List<RepairMaintenance> ListModel = new List<RepairMaintenance>();
            ListModel.Add(new RepairMaintenance(
                "PPK", RepoWorkshop.CountTruck("PPK"), RepoWorkshop.CountPendingin("PPK"), RepoWorkshop.CountBox("PPK"), RepoWorkshop.CountBan("PPK"), RepoWorkshop.CountGPS("PPK")
            ));
            ListModel.Add(new RepairMaintenance(
                "PPK-In", RepoWorkshop.CountTruck("PPK-In"), RepoWorkshop.CountPendingin("PPK-In"), RepoWorkshop.CountBox("PPK-In"), RepoWorkshop.CountBan("PPK-In"), RepoWorkshop.CountGPS("PPK-In")
            ));
            ListModel.Add(new RepairMaintenance(
                "SPK-O", RepoWorkshop.CountTruckSPKO(), RepoWorkshop.CountPendinginSPKO(), RepoWorkshop.CountBoxSPKO(), RepoWorkshop.CountBanSPKO(), RepoWorkshop.CountGPSSPKO()
            ));
            ListModel.Add(new RepairMaintenance(
                "SPK-P", RepoWorkshop.CountTruckSPKP(), RepoWorkshop.CountPendinginSPKP(), RepoWorkshop.CountBoxSPKP(), RepoWorkshop.CountBanSPKP(), RepoWorkshop.CountGPSSPKP()
            ));
            ListModel.Add(new RepairMaintenance(
                "SPK-C", RepoWorkshop.CountTruckSPKC(), RepoWorkshop.CountPendinginSPKC(), RepoWorkshop.CountBoxSPKC(), RepoWorkshop.CountBanSPKC(), RepoWorkshop.CountGPSSPKC()
            ));
            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingRepairMaintenanceTime(){
            List<RepairMaintenance> ListModel = new List<RepairMaintenance>();
            ListModel.Add(new RepairMaintenance(
                "> 7 hari", RepoWorkshop.CountTruck("PPK"), RepoWorkshop.CountPendingin("PPK"), RepoWorkshop.CountBox("PPK"), RepoWorkshop.CountBan("PPK"), RepoWorkshop.CountGPS("PPK")
            ));
            ListModel.Add(new RepairMaintenance(
                "> 3 hari", RepoWorkshop.CountTruck("PPK-In"), RepoWorkshop.CountPendingin("PPK-In"), RepoWorkshop.CountBox("PPK-In"), RepoWorkshop.CountBan("PPK-In"), RepoWorkshop.CountGPS("PPK-In")
            ));
            ListModel.Add(new RepairMaintenance(
                "> 1 hari", RepoWorkshop.CountTruckSPKO(), RepoWorkshop.CountPendinginSPKO(), RepoWorkshop.CountBoxSPKO(), RepoWorkshop.CountBanSPKO(), RepoWorkshop.CountGPSSPKO()
            ));
            ListModel.Add(new RepairMaintenance(
                "> 4 jam", RepoWorkshop.CountTruckSPKP(), RepoWorkshop.CountPendinginSPKP(), RepoWorkshop.CountBoxSPKP(), RepoWorkshop.CountBanSPKP(), RepoWorkshop.CountGPSSPKP()
            ));
            ListModel.Add(new RepairMaintenance(
                "< 4 jam", RepoWorkshop.CountTruckSPKC(), RepoWorkshop.CountPendinginSPKC(), RepoWorkshop.CountBoxSPKC(), RepoWorkshop.CountBanSPKC(), RepoWorkshop.CountGPSSPKC()
            ));
            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingDocumentExpired(){
            List<DocumentExpired> ListModel = new List<DocumentExpired>();
            ListModel.Add(new DocumentExpired(
                "> 2 Minggu", RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(14)).Count(), RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(14)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(14)).Count(), RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(14)).Count()
            ));
            ListModel.Add(new DocumentExpired(
                "> 1 minggu", RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(7) && d.KIR >= DateTime.Now.AddDays(14)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(7) && d.KIU >= DateTime.Now.AddDays(14)).Count(),
                RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(7) && d.IBM >= DateTime.Now.AddDays(14)).Count()
            ));
            ListModel.Add(new DocumentExpired(
                "> 1 hari", RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(1) && d.STNK >= DateTime.Now.AddDays(7)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(1) && d.KIR >= DateTime.Now.AddDays(7)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(1) && d.KIU >= DateTime.Now.AddDays(7)).Count(),
                RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(1) && d.IBM >= DateTime.Now.AddDays(7)).Count()
            ));
            ListModel.Add(new DocumentExpired(
                "< 1 minggu", RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(-7)).Count(), RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(-7)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(-7)).Count(), RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(-7)).Count()
            ));
            ListModel.Add(new DocumentExpired(
                "< 2 minggu", RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(-14) && d.STNK >= DateTime.Now.AddDays(-7)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(-14) && d.KIR >= DateTime.Now.AddDays(-7)).Count(),
                RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(-14) && d.KIU >= DateTime.Now.AddDays(-7)).Count(),
                RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(-14) && d.IBM >= DateTime.Now.AddDays(-7)).Count()
            ));
            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingPlanningChart(){
            List<Month> ListModel = new List<Month>();

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string month = "yyyy-MM";
            List<Context.OrderHistory> items = RepoSalesOrder.FindAllPlanningHistory().Where(d => 
                RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat >= firstDayOfMonth && RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat <= lastDayOfMonth
            ).ToList();

            List<int> Valuess = new List<int>();
            Valuess.Add(items.Where(d =>  
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d =>  
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d =>  
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d =>  
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d =>  
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d =>  
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d =>  
                RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null && DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && 
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count()
            );
            ListModel.Add(new Month("< 1 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d =>
                RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null && DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            ListModel.Add(new Month("< 4 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d =>
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d =>
                RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null && DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && 
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            ListModel.Add(new Month("< 8 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => 
                RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value) != null && DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && 
                (d.ProcessedAt - RepoSalesOrder.FindMarketingHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            ListModel.Add(new Month("> 8 jam", Valuess));

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }

        public String BindingKonfirmasiChart(){
            List<Month> ListModel = new List<Month>();

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string month = "yyyy-MM";
            List<Context.OrderHistory> items = RepoSalesOrder.FindAllKonfirmasiHistory().Where(d => 
                RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat >= firstDayOfMonth && RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat <= lastDayOfMonth
            ).ToList();

            List<int> Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&  
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            ListModel.Add(new Month("< 1 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            ListModel.Add(new Month("< 4 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            ListModel.Add(new Month("< 8 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value) != null && 
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindPlanningHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            ListModel.Add(new Month("> 8 jam", Valuess));

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }

        public String BindingAUJChart(){
            List<Month> ListModel = new List<Month>();

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string month = "yyyy-MM";
            List<Context.OrderHistory> items = RepoSalesOrder.FindAllAUJHistory().Where(d => 
                RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat >= firstDayOfMonth && RepoSalesOrder.FindByPK(d.SalesOrderId.Value).SalesOrderOncall.TanggalMuat <= lastDayOfMonth
            ).ToList();

            List<int> Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 1).Count());
            ListModel.Add(new Month("< 1 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 1 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 4
            ).Count());
            ListModel.Add(new Month("< 4 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours >= 4 &&
                (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours < 8
            ).Count());
            ListModel.Add(new Month("< 8 jam", Valuess));

            Valuess = new List<int>();
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-01" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-02" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-03" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-04" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-05" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-06" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-07" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            Valuess.Add(items.Where(d => RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value) != null &&
                DateTime.Parse(d.ProcessedAt.ToString()).ToString(month) == "2017-08" && (d.ProcessedAt - RepoSalesOrder.FindKonfirmasiHistory(d.SalesOrderId.Value).ProcessedAt).TotalHours > 8).Count());
            ListModel.Add(new Month("> 8 jam", Valuess));

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }

        public String BindingDocumentExpiredChart(){
            List<Month> ListModel = new List<Month>();

            List<int> Valuess = new List<int>();
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(14)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(14)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(14)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(14)).Count());
            ListModel.Add(new Month("> 2 Minggu", Valuess));

            Valuess = new List<int>();
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(7) && d.KIR >= DateTime.Now.AddDays(14)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(7) && d.KIU >= DateTime.Now.AddDays(14)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(7) && d.IBM >= DateTime.Now.AddDays(14)).Count());
            ListModel.Add(new Month("> 1 Minggu", Valuess));

            Valuess = new List<int>();
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(1) && d.STNK >= DateTime.Now.AddDays(7)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(1) && d.KIR >= DateTime.Now.AddDays(7)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(1) && d.KIU >= DateTime.Now.AddDays(7)).Count());
            Valuess.Add(RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(1) && d.IBM >= DateTime.Now.AddDays(7)).Count());
            ListModel.Add(new Month("> 1 Hari", Valuess));

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }

        public String BindingKekuranganDriver(){
            List<KekuranganDriver> ListModel = new List<KekuranganDriver>();
            List<Context.JenisTrucks> items = RepoJenisTruck.FindAll();
            int totaljmlkrg = 0;
            foreach (Context.JenisTrucks d in items){
                int jmlkrg = 0;
                decimal prcnt = 0;
                if (RepoDataTruck.FindAll().Where(e => e.IdJenisTruck == d.Id).Count() > 0){
                    jmlkrg = RepoDataTruck.FindAll().Where(e => e.IdJenisTruck == d.Id).Count()-RepoPenetapanDriver.FindAll().Where(f => f.DataTruck.IdJenisTruck == d.Id && f.IdDriver1 != null).Count();
                    prcnt = decimal.Parse(jmlkrg.ToString())/decimal.Parse(RepoDataTruck.FindAll().Where(e => e.IdJenisTruck == d.Id).Count().ToString())*100;
                }
                if (RepoPenetapanDriver.FindLastDriverHistory(d.StrJenisTruck) != null){
                    ListModel.Add(
                        new KekuranganDriver(
                            d.StrJenisTruck, jmlkrg, Math.Round(prcnt, 2), Math.Round((DateTime.Now - RepoPenetapanDriver.FindLastDriverHistory(d.StrJenisTruck).Tanggal).TotalDays, 0).ToString() + " Hari"
                        )
                    );
                }
                else{
                    ListModel.Add(
                        new KekuranganDriver(d.StrJenisTruck, jmlkrg, Math.Round(prcnt, 2), jmlkrg == 0 ? "" : Math.Round((DateTime.Now - DateTime.Parse("2017-07-07")).TotalDays, 0).ToString() + " Hari")
                    );
                }
                totaljmlkrg += jmlkrg;
            }
            ListModel.Add(
                new KekuranganDriver(
                    "T O T A L", totaljmlkrg, Math.Round(decimal.Parse(totaljmlkrg.ToString())/decimal.Parse(RepoDataTruck.FindAll().Count().ToString())*100, 2),
                    "dari " + RepoDriver.FindAll().Where(d => d.IdStatus == 66 && !RepoPenetapanDriver.FindAll().Any(e => e.IdDriver1 == d.Id)).Count() + " driver aktif yang belum ditempatkan"
                )
            );
            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingTrainingPertama(){
            return new JavaScriptSerializer().Serialize(RepoTrainingSetting.FindAll().Select(d => new{
                NamaTraining = d.Nama,
                KekuranganPeserta = RepoDriver.FindAll().Count() - RepoPelaksanaanTraining.FindAllDetail().Where(e => e.PelaksanaanTraining != null && e.PelaksanaanTraining.IdTrainingSetting == d.Id)
                .Count()
            }));
        }

        public String BindingDeliveryQuanlity(){
            List<DeliveryQuality> ListModel = new List<DeliveryQuality>();
            ListModel.Add(new DeliveryQuality("No Data", "50 delivery (10%)", "50 delivery (10%)", "50 delivery (10%)"));
            ListModel.Add(new DeliveryQuality("Off Target", "50 delivery (10%)", "50 delivery (10%)", "50 delivery (10%)"));
            ListModel.Add(new DeliveryQuality("Tolerance", "50 delivery (10%)", "50 delivery (10%)", "50 delivery (10%)"));
            ListModel.Add(new DeliveryQuality("On Target", "50 delivery (10%)", "50 delivery (10%)", "50 delivery (10%)"));
            ListModel.Add(new DeliveryQuality("T O T A L", "50 delivery (10%)", "50 delivery (10%)", "50 delivery (10%)"));
            return new JavaScriptSerializer().Serialize(ListModel);
        }

        public String BindingKekuranganDriverChart(){
            List<Month> ListModel = new List<Month>();

            List<Context.JenisTrucks> items = RepoJenisTruck.FindAll();
            List<int> Valuess = new List<int>();
            foreach (Context.JenisTrucks e in items)
            {
                Valuess = new List<int>();
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIR < DateTime.Now.AddDays(7) && d.KIR >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.STNK < DateTime.Now.AddDays(7) && d.STNK >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.KIU < DateTime.Now.AddDays(7) && d.KIU >= DateTime.Now.AddDays(14)).Count());
                Valuess.Add(RepoDataTruck.FindAll().Where(d => d.IBM < DateTime.Now.AddDays(7) && d.IBM >= DateTime.Now.AddDays(14)).Count());
                ListModel.Add(new Month(e.StrJenisTruck, Valuess));
            }

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }

        public String BindingGPSOff(){
            List<GPSDashboard> ListModel = new List<GPSDashboard>();
            ListModel.Add(new GPSDashboard("> 1 bulan",
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 31 && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 16
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 31 && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 17
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 31 && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 18
                ).ToList()
            ));
            ListModel.Add(new GPSDashboard("> 14 hari",
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 31 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 15 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 16
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 31 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 15 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 17
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 31 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 15 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 18
                ).ToList()
            ));
            ListModel.Add(new GPSDashboard("> 7 hari",
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 15 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 8 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 16
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 15 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 8 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 17
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 15 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 8 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 18
                ).ToList()
            ));
            ListModel.Add(new GPSDashboard("> 1 hari",
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 8 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 2 &&
                    RepoDataTruck.FindByName(d.VehicleNo) != null && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id) != null &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 16
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 8 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 2 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 17
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays <= 8 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 2 &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 18
                ).ToList()
            ));
            ListModel.Add(new GPSDashboard("< 6 jam",
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalHours < 30 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 1 &&
                    RepoDataTruck.FindByName(d.VehicleNo) != null &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id) != null && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 16
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalHours < 30 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 1 &&
                    RepoDataTruck.FindByName(d.VehicleNo) != null && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id) != null && 
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 17
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalHours < 30 && (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 1 &&
                    RepoDataTruck.FindByName(d.VehicleNo) != null && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id) != null &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 18
                ).ToList()
            ));
            ListModel.Add(new GPSDashboard("T O T A L",
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 1 && RepoDataTruck.FindByName(d.VehicleNo) != null &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id) != null && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 16
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 1 && RepoDataTruck.FindByName(d.VehicleNo) != null &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id) != null && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 17
                ).ToList(),
                RepoMonitoringVehicle.FindAll().Where(
                    d => (DateTime.Now-DateTime.Parse(d.LastUpdate.ToString())).TotalDays > 1 && RepoDataTruck.FindByName(d.VehicleNo) != null &&
                    RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id) != null && RepoDataGps.FindByTruck(RepoDataTruck.FindByName(d.VehicleNo).Id).IdVendor == 18
                ).ToList()
            ));
            return new JavaScriptSerializer().Serialize(ListModel);
        }
    }
}