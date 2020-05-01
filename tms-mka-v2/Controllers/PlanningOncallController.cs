using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using Newtonsoft.Json;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using tms_mka_v2.Security;
using tms_mka_v2.Infrastructure;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace tms_mka_v2.Controllers
{
    public class PlanningOncallController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IDriverRepo RepoDriver;
        private ISettlementBatalRepo RepoSettlementBatal;
        private IAuditrailRepo RepoAuditrail;
        private Iso_mstrRepo Reposo_mstr;
        private Icode_mstrRepo Repocode_mstr;
        private IDataTruckRepo RepoDataTruck;
        public PlanningOncallController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IDriverRepo repoDriver, ISettlementBatalRepo repoSettlementBatal,
            IAuditrailRepo repoAuditrail, Iso_mstrRepo reposo_mstr, Icode_mstrRepo repocode_mstr, IDataTruckRepo repoDataTruck)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoDriver = repoDriver;
            RepoSettlementBatal = repoSettlementBatal;
            RepoAuditrail = repoAuditrail;
            Reposo_mstr = reposo_mstr;
            Repocode_mstr = repocode_mstr;
            RepoDataTruck = repoDataTruck;
        }
        [MyAuthorize(Menu = "Planning Oncall", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "PlanningOncall").ToList();
            return View();
        }
        public string Binding()
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d =>
                (d.SalesOrderOncallId != null || d.SalesOrderProsesKonsolidasiId != null || d.SalesOrderPickupId != null) &&
                (d.Status == "save" || d.Status == "draft planning")).ToList();
            List<ListOrder> ListModel = new List<ListOrder>();
            foreach (Context.SalesOrder item in items)
            {
                if ((item.SONumber == null || item.SONumber == "") && item.SalesOrderPickupId != null)
                {
                    item.SONumber = item.SalesOrderPickup.SONumber;
                    RepoSalesOrder.save(item);
                }
                else if ((item.SONumber == null || item.SONumber == "") && item.SalesOrderOncallId != null)
                {
                    item.SONumber = item.SalesOrderOncall.SONumber;
                    RepoSalesOrder.save(item);
                }
                ListModel.Add(new ListOrder(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
        [MyAuthorize(Menu = "Planning Oncall", Action = "update")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                SalesOrderOncall model = new SalesOrderOncall(dbitem);
                ViewBag.kondisi = "planning";
                ViewBag.name = model.SONumber;
                ViewBag.area = RepoSalesOrder.FindArea(dbitem.SalesOrderOncall.IdDaftarHargaItem.Value);
                ViewBag.is_area_pulang = RepoSalesOrder.FindRute(dbitem.SalesOrderOncall.IdDaftarHargaItem.Value).IsAreaPulang == true ? "Yes" : "No";
                ViewBag.TanggalPulang = model.TanggalMuat.Value.AddDays(RepoSalesOrder.FindRute(dbitem.SalesOrderOncall.IdDaftarHargaItem.Value).WaktuKerja);
                ViewBag.Title = "Planning Sales Order Oncall " + model.SONumber;
                ViewBag.PostData = "EditOncall";
                return View("SalesOrderOncall/FormReadOnly", model);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                SalesOrderProsesKonsolidasi model = new SalesOrderProsesKonsolidasi(dbitem);
                ViewBag.kondisi = "planning";
                ViewBag.name = model.SONumber;
                ViewBag.Title = "Planning Sales Order Proses Konsolidasi " + model.SONumber;
                ViewBag.PostData = "EditProsesKonsolidasi";
                return View("SalesOrderProsesKonsolidasi/FormReadOnly", model);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                SalesOrderPickup model = new SalesOrderPickup(dbitem);
                ViewBag.kondisi = "planning";
                ViewBag.name = model.SONumber;
                ViewBag.Title = "Planning Sales Order Pickup " + model.SONumber;
                ViewBag.PostData = "EditPickup";
                return View("SalesOrderPickup/FormReadOnly", model);
            }
            else
            {
                return View("");
            }
        }

        [HttpPost]
        public ActionResult EditOncall(SalesOrderOncall model, string btnsave)
        {
            //validasi manual
            bool palid = true;
            int urutan = 1;
            if (btnsave != "draft" && model.Status != "draft")
            {
                if (!model.IdDataTruck.HasValue)
                {
                    ModelState.AddModelError("IdDataTruck", "Truk harus diisi.");
                    palid = false;
                }
                if (!model.Driver1Id.HasValue || model.Driver1Id.Value == 0)
                {
                    ModelState.AddModelError("Driver1Id", "Driver harus diisi.");
                    palid = false;
                }/*
                if (model.Driver1Id != null){
                    Context.Driver item = RepoDriver.FindAll().Where(d => d.Id == model.Driver1Id.Value).FirstOrDefault();
                List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().ToList();
                List<Context.SettlementBatal> dbsb = RepoSettlementBatal.FindAll().Where(s => s.IsProses == false).ToList();
                Driver driver = new Driver(item, dbso, dbsb);
                Context.SalesOrder dbsoDriver = RepoSalesOrder.FindAll().Where(d => 
                    (d.Status == "save" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                        (d.SalesOrderOncallId.HasValue && (d.SalesOrderOncall.Driver1Id == model.Driver1Id || d.SalesOrderOncall.Driver2Id == model.Driver1Id)) ||
                        (d.SalesOrderPickupId.HasValue && (d.SalesOrderPickup.Driver1Id == model.Driver1Id || d.SalesOrderPickup.Driver2Id == model.Driver1Id)) ||
                        (d.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrderProsesKonsolidasi.Driver1Id == model.Driver1Id || d.SalesOrderProsesKonsolidasi.Driver2Id == model.Driver1Id))
                    )
                ).FirstOrDefault();
                if (driver.StatusSo != "Available" && dbsoDriver != null)
                {
                    List<Context.SalesOrder> dbsoDriverList = RepoSalesOrder.FindAll().Where(d => 
                        (d.Status == "save" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                            (d.SalesOrderOncallId.HasValue && (d.SalesOrderOncall.Driver1Id == model.Driver1Id || d.SalesOrderOncall.Driver2Id == model.Driver1Id)) ||
                            (d.SalesOrderPickupId.HasValue && (d.SalesOrderPickup.Driver1Id == model.Driver1Id || d.SalesOrderPickup.Driver2Id == model.Driver1Id)) ||
                            (d.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrderProsesKonsolidasi.Driver1Id == model.Driver1Id || d.SalesOrderProsesKonsolidasi.Driver2Id == model.Driver1Id))
                        )
                    ).ToList();
                    urutan = dbsoDriver.urutan + 1;

                    //SO yg sdh jalan
                    Context.Rute rute = RepoSalesOrder.FindRute(dbsoDriver.SalesOrderOncall.IdDaftarHargaItem.Value);
                    int waktuKerja = int.Parse(rute.WaktuKerja.ToString());

                    //SO yg sedang diplanning
                    Context.SalesOrder so = RepoSalesOrder.FindByOnCall(model.SalesOrderId.Value);
                    Context.Rute rutePlanning = null;
                    if (so != null && so.SalesOrderOncall != null && so.SalesOrderOncall.IdDaftarHargaItem != null){                        
                        rutePlanning = RepoSalesOrder.FindRute(so.SalesOrderOncall.IdDaftarHargaItem.Value);
                        int waktuKerjaPlanning = int.Parse(rutePlanning.WaktuKerja.ToString());
                        bool used_at_that_date = dbsoDriverList.Any(d => 
                            DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()).AddDays(waktuKerja) >= model.TanggalMuat || 
                            DateTime.Parse(d.SalesOrderOncall.TanggalMuat.ToString()) <= DateTime.Parse(model.TanggalMuat.ToString()).AddDays(waktuKerja)
                        );
                        if (used_at_that_date){
//                            ModelState.AddModelError("Driver1Id", "Driver tidak tersedia, harap pilih driver lain.");
  //                          palid = false;
                        }
                        else if (urutan > 2 && rutePlanning.IsAreaPulang != true){
    //                        ModelState.AddModelError("Driver1Id", "Driver tidak tersedia, harap pilih driver lain.");
      //                      palid = false;
                        }
                    }
                }
                }*/
            }
            if (model.Status == "draft")
            {
                if (model.CommentUser == "" || model.CommentUser == null)
                {
                    ModelState.AddModelError("CommentUser", "Comment harus diisi.");
                    palid = false;
                }
            }
            if (palid)
            {
                Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.SalesOrderId.Value);
                dbitem.isReturn = false;
                dbitem.UpdatedBy = UserPrincipal.id;
                dbitem.urutan = urutan;
                string act = "";
                if (btnsave != null && btnsave != "")
                {
                    act = "Draft";
                    dbitem.Status = btnsave;
                }
                else
                {
                    if (model.Status.ToLower() == "save")
                    {
                        act = "Return";
                        dbitem.isReturn = true;
                    }
                    else
                    {
                        act = "Submit";
                        dbitem.isReturn = false;
                    }

                    dbitem.Status = model.Status;
                }

                model.setDbOperasional(dbitem.SalesOrderOncall, act, "Operational");
                RepoSalesOrder.save(dbitem);
                RepoAuditrail.savePlanningHistory(dbitem);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.kondisi = "planning";
                ViewBag.name = model.SONumber;
                ViewBag.Title = "Planning Sales Order Oncall " + model.SONumber;
                ViewBag.PostData = "EditOncall";
                return View("SalesOrderOncall/FormReadOnly", model);
            }
        }

        [HttpPost]
        public ActionResult EditProsesKonsolidasi(SalesOrderProsesKonsolidasi model, string btnsave)
        {
            //validasi manual
            bool palid = true;
            if (btnsave != "draft")
            {
                if (!model.IdDataTruck.HasValue)
                {
                    ModelState.AddModelError("IdDataTruck", "Truk harus diisi.");
                    palid = false;
                }
                if (!model.Driver1Id.HasValue || model.Driver1Id.Value == 0)
                {
                    ModelState.AddModelError("Driver1Id", "Driver harus diisi.");
                    palid = false;
                }
                Context.Driver item = RepoDriver.FindAll().Where(d => d.Id == model.Driver1Id.Value).FirstOrDefault();
                List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().ToList();
                List<Context.SettlementBatal> dbsb = RepoSettlementBatal.FindAll().Where(s => s.IsProses == false).ToList();
                Driver driver = new Driver(item, dbso, dbsb);
                if (driver.StatusSo != "Available")
                {
//                    ModelState.AddModelError("Driver1Id", "Driver tidak tersedia, harap pilih driver lain.");
  //                  palid = false;
                }
            }
            if (model.Status == "draft")
            {
                if (model.CommentUser == "" || model.CommentUser == null)
                {
                    ModelState.AddModelError("CommentUser", "Comment harus diisi.");
                    palid = false;
                }
            }
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.SalesOrderId.Value);
            if (palid)
            {
                dbitem.isReturn = false;
                string act = "";
                if (btnsave != null && btnsave != "")
                {
                    act = "Draft";
                    dbitem.Status = btnsave;
                }
                else
                {
                    if (model.Status.ToLower() == "save")
                    {
                        act = "Return";
                        dbitem.isReturn = true;
                    }
                    else
                    {
                        act = "Submit";
                        dbitem.isReturn = false;
                    }

                    dbitem.Status = model.Status;
                }

                model.setDbOperasional(dbitem.SalesOrderProsesKonsolidasi, act, "Operational");
                dbitem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbitem);
                RepoAuditrail.savePlanningHistory(dbitem);
                RepoAuditrail.saveUpdSalesOrderProsesKonsolidasiQuery(dbitem.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                RepoAuditrail.saveSalesOrderProsesKonsolidasiCommentQuery(dbitem.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.kondisi = "planning";
                ViewBag.name = model.SONumber;
                ViewBag.Title = "Planning Sales Order Proses Konsolidasi " + model.SONumber;
                ViewBag.PostData = "EditProsesKonsolidasi";
                SalesOrderProsesKonsolidasi modelsopk = new SalesOrderProsesKonsolidasi(dbitem);
                ViewBag.StrListSo = modelsopk.StrListSo;
                return View("SalesOrderProsesKonsolidasi/FormReadOnly", model);
            }
        }
        [HttpPost]
        public ActionResult EditPickup(SalesOrderPickup model, string btnsave)
        {
            //validasi manual
            bool palid = true;
            if (btnsave != "draft")
            {
                if (!model.IdDataTruck.HasValue)
                {
                    ModelState.AddModelError("IdDataTruck", "Truk harus diisi.");
                    palid = false;
                }
                if (!model.Driver1Id.HasValue || model.Driver1Id.Value == 0)
                {
                    ModelState.AddModelError("Driver1Id", "Driver harus diisi.");
                    palid = false;
                }
                Context.Driver item = RepoDriver.FindAll().Where(d => d.Id == model.Driver1Id.Value).FirstOrDefault();
                List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().ToList();
                List<Context.SettlementBatal> dbsb = RepoSettlementBatal.FindAll().Where(s => s.IsProses == false).ToList();
                Driver driver = new Driver(item, dbso, dbsb);
                if (driver.StatusSo != "Available")
                {
//                    ModelState.AddModelError("Driver1Id", "Driver tidak tersedia, harap pilih driver lain.");
  //                  palid = false;
                }
            }
            if (model.Status == "draft")
            {
                if (model.CommentUser == "" || model.CommentUser == null)
                {
                    ModelState.AddModelError("CommentUser", "Comment harus diisi.");
                    palid = false;
                }
            }

            if (palid)
            {
                Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.SalesOrderId.Value);
                dbitem.isReturn = false;
                string act = "";
                if (btnsave != null && btnsave != "")
                {
                    act = "Draft";
                    dbitem.Status = btnsave;
                }
                else
                {
                    if (model.Status.ToLower() == "save")
                    {
                        act = "Return";
                        dbitem.isReturn = true;
                    }
                    else
                    {
                        dbitem.isReturn = false;
                        act = "Submit";
                    }

                    dbitem.Status = model.Status;
                }

                model.setDbOperasional(dbitem.SalesOrderPickup, act, "Operational");
                RepoSalesOrder.save(dbitem);
                RepoAuditrail.savePlanningHistory(dbitem);
                var query = "UPDATE dbo.\"SalesOrderPickup\" SET \"SONumber\" = " + dbitem.SalesOrderPickup.SONumber + ", \"Urutan\" = " + dbitem.SalesOrderPickup.Urutan + ", TanggalOrder\" = " +
                    dbitem.SalesOrderPickup.TanggalOrder + ", \"CustomerId\" = " + dbitem.SalesOrderPickup.CustomerId + ", \"JenisTruckId\" = " + dbitem.SalesOrderPickup.JenisTruckId + ", \"ProductId\" = " +
                     dbitem.SalesOrderPickup.ProductId + ", \"TanggalPickup\" = " + dbitem.SalesOrderPickup.TanggalPickup + ", \"JamPickup\" = " + dbitem.SalesOrderPickup.JamPickup + ", \"IsSelect\" = " +
                      dbitem.SalesOrderPickup.IsSelect + ", \"Keterangan\" = " + dbitem.SalesOrderPickup.Keterangan + ", \"KeteranganLoading\" = " + dbitem.SalesOrderPickup.KeteranganLoading + ", \"KeteranganUnloading\" = " +
                       dbitem.SalesOrderPickup.KeteranganUnloading + ", \"JamOrder\" = " + dbitem.SalesOrderPickup.JamOrder + ", \"StrMultidrop\" = " + dbitem.SalesOrderPickup.StrMultidrop + ", \"RuteId\" = " +
                       dbitem.SalesOrderPickup.RuteId + ", \"IdDataTruck\" = " + dbitem.SalesOrderPickup.IdDataTruck + ", \"KeteranganDataTruck\" = " + dbitem.SalesOrderPickup.KeteranganDataTruck + ", \"Driver1Id\" = " +
                       dbitem.SalesOrderPickup.Driver1Id + ", \"KeteranganDriver1\" = " + dbitem.SalesOrderPickup.KeteranganDriver1 + ", \"Driver2Id\" = " + dbitem.SalesOrderPickup.Driver2Id + ", \"KeteranganDriver2\" = " +
                        dbitem.SalesOrderPickup.KeteranganDriver2 + ", \"IsCash\" = " + dbitem.SalesOrderPickup.IsCash + ", \"IdDriverTitip\" = " + dbitem.SalesOrderPickup.IdDriverTitip + ", \"KeteranganRek\" = " +
                         dbitem.SalesOrderPickup.KeteranganRek + ", \"AtmId\" = " + dbitem.SalesOrderPickup.AtmId + " WHERE \"SalesOrderPickupId\" = " + dbitem.SalesOrderPickupId +
                         ";INSERT INTO dbo.\"SalesOrderPickupComment\" (\"SalesOrderPickupId\", \"Tanggal\", \"CommentUser\", \"Action\", \"Username\") VALUES (" + dbitem.SalesOrderPickupId + ", " + DateTime.Now + ", " +
                    model.CommentUser + ", " + act + ", " + UserPrincipal.username + ");";
                RepoAuditrail.SetAuditTrail(query, "Planning Pickup", "Edit", UserPrincipal.id);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.kondisi = "planning";
                ViewBag.name = model.SONumber;
                ViewBag.Title = "Planning Sales Order Pickup " + model.SONumber;
                ViewBag.PostData = "EditPickup";
                return View("SalesOrderPickup/FormReadOnly", model);
            }
        }
        [MyAuthorize(Menu = "Planning Oncall", Action = "update")]
        [HttpPost]
        public JsonResult Submit(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SalesOrder dbItem = RepoSalesOrder.FindByPK(id);
            dbItem.isReturn = false;
            List<string> listError = new List<string>();

            if ((dbItem.SalesOrderOncallId.HasValue ? !dbItem.SalesOrderOncall.IdDataTruck.HasValue : true) &&
                (dbItem.SalesOrderProsesKonsolidasiId.HasValue ? !dbItem.SalesOrderProsesKonsolidasi.IdDataTruck.HasValue : true) &&
                (dbItem.SalesOrderPickupId.HasValue ? !dbItem.SalesOrderPickup.IdDataTruck.HasValue : true))
            {
                response.Success = false;
                listError.Add("Truk"); 
            }
            if ((dbItem.SalesOrderOncallId.HasValue ? !dbItem.SalesOrderOncall.Driver1Id.HasValue : true) &&
                (dbItem.SalesOrderProsesKonsolidasiId.HasValue ? !dbItem.SalesOrderProsesKonsolidasi.Driver1Id.HasValue : true) &&
                (dbItem.SalesOrderPickupId.HasValue ? !dbItem.SalesOrderPickup.Driver1Id.HasValue : true))
            {
                response.Success = false;
                listError.Add("Driver 1");
            }

            if (response.Success) {
                dbItem.Status = "draft konfirmasi";
                dbItem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbItem);
                RepoAuditrail.savePlanningHistory(dbItem);
            }
            else {
                response.Message = string.Join(", ", listError) + " belum dipilih.";
            }

            return Json(response);
        }

        public String CheckAvailability(string SONumber, int DriverId)
        {
            Context.SalesOrder currentSO = RepoSalesOrder.FindByCode(SONumber);
            Context.SalesOrder dbso = RepoSalesOrder.FindYgBentrok(SONumber, DriverId, currentSO.OrderTanggalMuat.Value.AddDays(7));
            ResponeModel response = new ResponeModel(true);
            response.Message = dbso == null ? "YES" : "NO";
            return new JavaScriptSerializer().Serialize(new { Message = response.Message, currentSo = dbso == null ? null : dbso.SONumber });
        }

        public String CheckLeadTime(int id){
            Context.SalesOrder dbso = RepoSalesOrder.FindAll().Where(d => 
                (d.Status == "save planning" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                    (d.SalesOrderOncallId.HasValue && d.SalesOrderOncall.IdDataTruck == id) || (d.SalesOrderPickupId.HasValue && d.SalesOrderPickup.IdDataTruck == id) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue && d.SalesOrderProsesKonsolidasi.IdDataTruck == id) ||
                    (d.SalesOrderKontrakId.HasValue && d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.DataTruckId == id))
                )
            ).FirstOrDefault();
            if (dbso != null && dbso.SalesOrderOncallId.HasValue){
                Context.Rute rute = RepoSalesOrder.FindRute(dbso.SalesOrderOncall.IdDaftarHargaItem.Value);
                double wktkrj = rute.WaktuKerja;
                int waktuKerja = int.Parse(rute.WaktuKerja.ToString());
                return new JavaScriptSerializer().Serialize(new {
                    waktupulang = DateTime.Parse(dbso.SalesOrderOncall.TanggalMuat.ToString()).AddDays(waktuKerja).ToString(), waktuMuat = DateTime.Parse(dbso.SalesOrderOncall.TanggalMuat.ToString()).ToString()
                });
            }
            return null;
        }

        public String CheckLeadTimeDriver(int id){
            Context.SalesOrder dbso = RepoSalesOrder.FindAll().Where(d => 
                (d.Status == "save planning" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                    (d.SalesOrderOncallId.HasValue && (d.SalesOrderOncall.Driver1Id == id || d.SalesOrderOncall.Driver2Id == id)) ||
                    (d.SalesOrderPickupId.HasValue && (d.SalesOrderPickup.Driver1Id == id || d.SalesOrderPickup.Driver2Id == id)) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrderProsesKonsolidasi.Driver1Id == id || d.SalesOrderProsesKonsolidasi.Driver2Id == id)) ||
                    (d.SalesOrderKontrakId.HasValue && (d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.IdDriver1 == id || k.IdDriver2 == id)))
                )
            ).FirstOrDefault();
            List<Context.SalesOrder> soList = RepoSalesOrder.FindAll().Where(d => 
                (d.Status == "save planning" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                    (d.SalesOrderOncallId.HasValue && (d.SalesOrderOncall.Driver1Id == id || d.SalesOrderOncall.Driver2Id == id)) ||
                    (d.SalesOrderPickupId.HasValue && (d.SalesOrderPickup.Driver1Id == id || d.SalesOrderPickup.Driver2Id == id)) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrderProsesKonsolidasi.Driver1Id == id || d.SalesOrderProsesKonsolidasi.Driver2Id == id)) ||
                    (d.SalesOrderKontrakId.HasValue && (d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.IdDriver1 == id || k.IdDriver2 == id)))
                )
            ).ToList();
            if (dbso != null && dbso.SalesOrderOncallId.HasValue){
                int maxUrutan = soList.Count() > 0 ? RepoSalesOrder.FindAll().Where(d => 
                    (d.Status == "save planning" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                        (d.SalesOrderOncallId.HasValue && (d.SalesOrderOncall.Driver1Id == id || d.SalesOrderOncall.Driver2Id == id)) ||
                        (d.SalesOrderPickupId.HasValue && (d.SalesOrderPickup.Driver1Id == id || d.SalesOrderPickup.Driver2Id == id)) ||
                        (d.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrderProsesKonsolidasi.Driver1Id == id || d.SalesOrderProsesKonsolidasi.Driver2Id == id)) ||
                        (d.SalesOrderKontrakId.HasValue && (d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.IdDriver1 == id || k.IdDriver2 == id)))
                    )
                ).Max(d => d.urutan) : 0;
                Context.Rute rute = RepoSalesOrder.FindRute(dbso.SalesOrderOncall.IdDaftarHargaItem.Value);
                double wktkrj = rute.WaktuKerja;
                int waktuKerja = int.Parse(rute.WaktuKerja.ToString());
                return new JavaScriptSerializer().Serialize(new {
                    waktupulang = DateTime.Parse(dbso.SalesOrderOncall.TanggalMuat.ToString()).AddDays(waktuKerja).ToString(), orderCount = maxUrutan
                });
            }
            else if (dbso != null && dbso.SalesOrderKontrakId.HasValue){
                int maxUrutan = soList.Count() > 0 ? RepoSalesOrder.FindAll().Where(d => 
                    (d.Status == "save planning" || d.Status == "draft planning" || d.Status == "draft konfirmasi" || d.Status == "save konfirmasi" || d.Status == "dispatched") && (
                        (d.SalesOrderOncallId.HasValue && (d.SalesOrderOncall.Driver1Id == id || d.SalesOrderOncall.Driver2Id == id)) ||
                        (d.SalesOrderPickupId.HasValue && (d.SalesOrderPickup.Driver1Id == id || d.SalesOrderPickup.Driver2Id == id)) ||
                        (d.SalesOrderProsesKonsolidasiId.HasValue && (d.SalesOrderProsesKonsolidasi.Driver1Id == id || d.SalesOrderProsesKonsolidasi.Driver2Id == id)) ||
                        (d.SalesOrderKontrakId.HasValue && (d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.IdDriver1 == id || k.IdDriver2 == id)))
                    )
                ).Max(d => d.urutan) : 0;
                return new JavaScriptSerializer().Serialize(new {
                    waktupulang = dbso.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Driver1Id == id).Select(d => d.MuatDate).Max().ToString(),
                    waktuMuat = dbso.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Driver1Id == id).Select(d => d.MuatDate).Min().ToString(), orderCount = 0,
                });
            }
            return new JavaScriptSerializer().Serialize(new {success = true});
        }

        [MyAuthorize(Menu = "Planning Oncall", Action = "update")]
        [HttpPost]
        public JsonResult Return(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SalesOrder dbItem = RepoSalesOrder.FindByPK(id);
            dbItem.isReturn = true;
            dbItem.Status = "draft";

            RepoSalesOrder.save(dbItem);

            return Json(response);
        }
    }
}