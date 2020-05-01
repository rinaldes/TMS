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
    public class SalesOrderOncallController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IDaftarHargaOnCallRepo RepoDaftarHarga;
        private IJenisTruckRepo RepoJnsTruck;
        private Iptnr_mstrRepo Repoptnr_mstr;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private IAuditrailRepo RepoAuditrail;
        private IlistNotifRepo RepoListNotif;

        public SalesOrderOncallController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IDaftarHargaOnCallRepo repoDaftarHarga, IJenisTruckRepo repoJnsTruck,
            Iptnr_mstrRepo repoptnr_mstr, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr, IAuditrailRepo repoAuditrail, IlistNotifRepo repoListNotif)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoDaftarHarga = repoDaftarHarga;
            RepoJnsTruck = repoJnsTruck;
            Repoptnr_mstr = repoptnr_mstr;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            RepoAuditrail = repoAuditrail;
            RepoListNotif = repoListNotif;
        }
        [MyAuthorize(Menu = "Sales Order Oncall", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SalesOrderOnCall").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            

            //List<Context.SalesOrder> items = RepoSalesOrder.FindAllOnCall(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllOnCall();
            items = items.Where(d => d.Status.ToLower() == "draft").ToList();
            List<SalesOrderOncall> ListModel = new List<SalesOrderOncall>();
            foreach (Context.SalesOrder item in items)
            {
                var _model = new SalesOrderOncall(item);
                _model.ListLoad.Clear();
                _model.ListUnload.Clear();
                ListModel.Add(_model);
            }

            int total = RepoSalesOrder.CountOncall(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }
        public string BindingDp(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.SalesOrder items = RepoSalesOrder.FindByOnCall(id);

            return new JavaScriptSerializer().Serialize(new { data = items.SalesOrderOncall.SalesOrderOncallDp.ToList().Select(d => new {
                Id = d.Id,
                Tanggal = d.Tanggal,
                Penerima = d.Penerima,
                IdRekening = d.RekeningId,
                NoRekening = d.Rekenings.NoRekening,
                Jenis = d.Jenis,
                Jumlah = d.Jumlah
            })});
        }
        public string BindingComment(int id)
        {
            Context.SalesOrder items = RepoSalesOrder.FindByOnCall(id);

            return new JavaScriptSerializer().Serialize(new
            {
                data = items.SalesOrderOncall.SalesOrderOnCallComment.ToList().Select(d => new
                {
                    Id = d.Id,
                    Tanggal = d.Tanggal,
                    CommentUser = d.CommentUser,
                    Username = d.Username,
                    Action = d.Action
                })
            });
        }
        [MyAuthorize(Menu = "Sales Order Oncall", Action="create")]
        public ActionResult Add()
        {
            SalesOrderOncall model = new SalesOrderOncall();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(SalesOrderOncall model, string btnsave)
        {
            bool palid = true;
            if (ModelState.IsValid)
            {
                if (btnsave == null && model.Status == "save")
                {
                    List<Context.DaftarHargaOnCall> dummy = RepoDaftarHarga.FindAll().Where(d => d.IdCust == model.CustomerId && (model.TanggalMuat >= d.PeriodStart && model.TanggalMuat <= d.PeriodEnd)).ToList();

                    if (dummy.Where(d => d.DaftarHargaOnCallItem.Any(i => i.IdJenisTruck == model.JenisTruckId)).Count() < 1)
                    {
                        palid = false;
                        ViewBag.errorMsg = "Tidak terdapat daftar harga untuk jenis truck " + RepoJnsTruck.FindByPK(model.JenisTruckId.Value).StrJenisTruck;
                    }
                }
            }

            SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrLoad);
            model.ListLoad = resultLoad.ToList();
            SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrUnload);
            model.ListUnload = resultUnload.ToList();

            if (palid)
            {
                if (ModelState.IsValid)
                {
                    Context.SalesOrder dbso = new Context.SalesOrder();
                    Context.SalesOrderOncall dbitem = new Context.SalesOrderOncall();
                    dbitem.Urutan = RepoSalesOrder.getUrutanOnCAll(model.TanggalMuat.Value) + 1;
                    dbitem.SONumber = RepoSalesOrder.generateCodeOnCall(model.TanggalMuat.Value, dbitem.Urutan);
                    dbso.SONumber = RepoSalesOrder.generateCodeOnCall(model.TanggalMuat.Value, dbitem.Urutan);
                    dbitem.DN = "DN" + dbitem.SONumber;
                    if (btnsave == "save")
                        dbso.Status = "draft planning";
                    else if (btnsave == "draft")
                        dbso.Status = "draft";
                    else
                        dbso.Status = model.Status;

                    if (model.Status == "save")
                    {
                        //{ Nama: "Sales Order", Id: "S" },
                        //{ Nama: "Planning Order", Id: "P" },
                        //{ Nama: "Konfirmasi Planning", Id: "KP" },
                        //{ Nama: "Admin Uang Jalan", Id: "A" },
                        //{ Nama: "Kasir", Id: "K" }
                        RepoListNotif.save("P",
                        "PLANNING SO : " + model.SONumber + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " belum diproses.",
                        "PLANNING SO : " + model.SONumber + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " belum diproses.");
                    }

                    model.setDb(dbitem, model.Status == "save" ? "Submit" : model.Status == "draft" ? "Draft" : "", "Marketing");
                    dbso.SalesOrderOncall = dbitem;
                    dbso.isReturn = false;
                    dbso.CreatedBy = UserPrincipal.id;
                    dbso.UpdatedBy = UserPrincipal.id;
                    string sod_guid = Guid.NewGuid().ToString();
                    dbso.oidErp = sod_guid;
                    dbso.NoShipment = model.NoShipment;
                    if (RepoSalesOrder.FindByOnCallCode(dbso.SONumber) == null)
                    {
                        RepoSalesOrder.save(dbso);
                        Context.SalesOrder so = RepoSalesOrder.FindByOnCallCode(dbitem.SONumber);
                        RepoAuditrail.saveOrderHistory(so);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.errorMsg = "SO number "+dbso.SONumber+" is exist. Please try again";
                        return View("Form", model);
                    }
                }
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Sales Order Oncall", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderOncall model = new SalesOrderOncall(dbitem);

            ViewBag.name = model.SONumber;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(SalesOrderOncall model, string btnsave)
        {
            //validasi tambahan
            bool palid = true;
            if (ModelState.IsValid)
            {
                if (btnsave == null && model.Status == "save")
                {
                    List<Context.DaftarHargaOnCall> dummy = RepoDaftarHarga.FindAll().Where(d => d.IdCust == model.CustomerId && (model.TanggalMuat >= d.PeriodStart && model.TanggalMuat <= d.PeriodEnd)).ToList();

                    if (dummy.Where(d => d.DaftarHargaOnCallItem.Any(i => i.IdJenisTruck == model.JenisTruckId)).Count() < 1)
                    {
                        palid = false;
                        ViewBag.errorMsg = "Tidak terdapat daftar harga untuk jenis truck " + RepoJnsTruck.FindByPK(model.JenisTruckId.Value).StrJenisTruck;
                    }
                }
            }

            SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrLoad);
            model.ListLoad = resultLoad.ToList();
            SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrUnload);
            model.ListUnload = resultUnload.ToList();
            //if (!model.ListLoad.Any(d => d.IsSelect == true))
            //{
            //    ModelState.AddModelError("StrLoad", "Alamat muat harus dipilih.");
            //}
            //if (!model.ListLoad.Any(d => d.IsSelect == true))
            //{
            //    ModelState.AddModelError("StrUnload", "Alamat bongkar harus dipilih.");
            //}

            if (palid)
            {
                if (ModelState.IsValid)
                {
                    Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.SalesOrderId.Value);

                    if (btnsave == "save")
                        dbitem.Status = "save";
                    else if (btnsave == "draft")
                        dbitem.Status = "draft";
                    else
                        dbitem.Status = model.Status;


                    if (model.Status == "save")
                    {
                        //{ Nama: "Sales Order", Id: "S" },
                        //{ Nama: "Planning Order", Id: "P" },
                        //{ Nama: "Konfirmasi Planning", Id: "KP" },
                        //{ Nama: "Admin Uang Jalan", Id: "A" },
                        //{ Nama: "Kasir", Id: "K" }
                        RepoListNotif.save("P",
                        "PLANNING SO : " + model.SONumber + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " belum diproses.",
                        "PLANNING SO : " + model.SONumber + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " belum diproses.");
                    }

                    model.setDb(dbitem.SalesOrderOncall, model.Status == "save" ? "Submit" : model.Status == "draft" ? "Draft" : "", "Marketing");
                    dbitem.isReturn = false;
                    dbitem.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dbitem);
                    //RepoAuditrail.saveOrderHistory(dbitem);
                    RepoAuditrail.SetAuditTrail(
                        "UPDATE dbo.\"SalesOrderOncall\" SET \"TanggalOrder\" = " + model.TanggalOrder + ", \"JamOrder\" = " + model.JamOrder + ", \"CustomerId\" = " + model.CustomerId + ", \"PrioritasId\" = " +
                            model.PrioritasId + ", \"JenisTruckId\" = " + model.JenisTruckId + ", \"ProductId\" = " + model.ProductId + ", \"TanggalMuat\" = " + model.TanggalMuat + ", \"JamMuat\" = " + model.JamMuat +
                            ", \"Keterangan\" = " + model.Keterangan + ", \"KeteranganLoading\" = " + model.KeteranganLoading + ", \"KeteranganUnloading\" = " + model.KeteranganUnloading + ", \"IdDaftarHargaItem\" = " +
                            dbitem.SalesOrderOncall.IdDaftarHargaItem + ", \"StrDaftarHargaItem\" = " + dbitem.SalesOrderOncall.StrDaftarHargaItem + ", \"StrMultidrop\" = " + model.StrMultidrop + ", \"IdDataTruck\" = " + model.IdDataTruck +
                            ", \"Driver1Id\" = " + model.Driver1Id + ", \"KeteranganDriver1\" = " + model.KeteranganDriver1 + ", \"Driver2Id\" = " + model.Driver2Id + ", \"KeteranganDriver2\" = " + model.KeteranganDriver2 +
                            ", \"IsCash\" = " + model.IsCash + ", \"KeteranganRek\" = " + model.KeteranganRek + ", \"IdDriverTitip\" = " + model.IdDriverTitip + ", \"DN\" = " + model.DN + ", \"KeteranganDataTruck\" = " +
                            model.KeteranganDataTruck + ", \"AtmId\" = " + model.AtmId + " WHERE \"SalesOrderOnCallId\" = " + model.SalesOrderOnCallId + ";",
                        "List Order", "Revisi Rute", UserPrincipal.id
                    );

                    return RedirectToAction("Index");
                }
            }

            
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SalesOrder dbItem = RepoSalesOrder.FindByPK(id);

            RepoSalesOrder.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        [HttpPost]
        public JsonResult Submit(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SalesOrder dbItem = RepoSalesOrder.FindByPK(id);

            List<Context.DaftarHargaOnCall> dummy = RepoDaftarHarga.FindAll().Where(d => d.IdCust == dbItem.SalesOrderOncall.CustomerId && (dbItem.SalesOrderOncall.TanggalMuat >= d.PeriodStart && dbItem.SalesOrderOncall.TanggalMuat <= d.PeriodEnd)).ToList();

            if (dummy.Where(d => d.DaftarHargaOnCallItem.Any(i => i.IdJenisTruck == dbItem.SalesOrderOncall.JenisTruckId)).Count() < 1)
            {
                response.Success = false;
                response.Message = "Tidak terdapat daftar harga untuk jenis truck " + dbItem.SalesOrderOncall.JenisTrucks.StrJenisTruck;
            }
            else
            {
                dbItem.Status = "save";
                dbItem.isReturn = false;
                dbItem.UpdatedBy = UserPrincipal.id;
                RepoAuditrail.saveOrderHistory(dbItem);
                RepoSalesOrder.save(dbItem);

                //{ Nama: "Sales Order", Id: "S" },
                //{ Nama: "Planning Order", Id: "P" },
                //{ Nama: "Konfirmasi Planning", Id: "KP" },
                //{ Nama: "Admin Uang Jalan", Id: "A" },
                //{ Nama: "Kasir", Id: "K" }
                RepoListNotif.save("P",
                    "PLANNING SO : " + dbItem.SalesOrderOncall.SONumber + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " belum diproses.",
                    "PLANNING SO : " + dbItem.SalesOrderOncall.SONumber + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " belum diproses.");
            }

            return Json(response);
        }
        [MyAuthorize(Menu = "Input DP", Action="create")]
        public ActionResult InputDp(int id, string status)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderOncall model = new SalesOrderOncall(dbitem);

            ViewBag.name = model.SONumber;
            ViewBag.kondisi = "inputdp";

            //if (status != "settlement" && status != "dispatched")
            //{
            //    TempData["errorMsgListOrder"] = "Maaf, Status SO belum Settlement ataupun Dispatched";
            //    return RedirectToAction("Index", "ListOrder");
            //}
            //else
            //{
            //    TempData.Remove("errorMsgListOrder");
            //    return View("SalesOrderOncall/FormReadOnly", model);
            //}
            TempData.Remove("errorMsgListOrder");
            return View("SalesOrderOncall/FormReadOnly", model);
        }
        [HttpPost]
        public JsonResult SaveDp(Context.SalesOrderOncallDp model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByOnCall(model.SalesOrderOnCallId);
            if (model.Id == 0){
                dbitem.SalesOrderOncall.SalesOrderOncallDp.Add(model);
                var query = "INSERT INTO \"dbo\".\"SalesOrderOncallDp\" (\"SalesOrderOnCallId\", \"Tanggal\", \"Penerima\", \"Jenis\", \"RekeningId\", \"Jumlah\") VALUES (" + model.SalesOrderOnCallId +
                    ", " + model.Tanggal + ", " + model.Penerima + ", " + model.Jenis + ", " + model.RekeningId + ", " + model.Jumlah + ");";
                RepoAuditrail.SetAuditTrail(query, "Input DP", "List Order", UserPrincipal.id);
            }
            else
            {
                Context.SalesOrderOncallDp dbdp = dbitem.SalesOrderOncall.SalesOrderOncallDp.Where(d => d.Id == model.Id).FirstOrDefault();
                dbdp.Tanggal = model.Tanggal;
                dbdp.Penerima = model.Penerima;
                dbdp.RekeningId = model.RekeningId;
                dbdp.Jenis = model.Jenis;
                dbdp.Jumlah = model.Jumlah;
                var query = "UPDATE \"dbo\".\"SalesOrderOncallDp\" SET \"Tanggal\" = " + dbdp.Tanggal + ", \"Penerima\" = " + dbdp.Penerima + ", \"Jenis\" = \" = " + dbdp.Jenis +
                    ", \"RekeningId\" = " + dbdp.RekeningId + "\"Jumlah\" = " + dbdp.Jumlah + " WHERE \"Id\" = " + dbdp.Id + ";";
                RepoAuditrail.SetAuditTrail(query, "Input DP", "List Order", UserPrincipal.id);
            }

            RepoSalesOrder.save(dbitem);
            //lebah dieu sync ERPna
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            string code =  "DP-" + (dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderPickupId.HasValue ? dbitem.SalesOrderPickup.SONumber : dbitem.SalesOrderKontrak.SONumber);
            //Jurnal
            Repoglt_det.saveFromAc(1, code, model.Jumlah, 0, Repoac_mstr.FindByPk(erpConfig.IdCashCredit), dbitem); //Cash
            Repoglt_det.saveFromAc(2, code, 0, model.Jumlah, Repoac_mstr.FindByPk(erpConfig.IdDP), dbitem); //Uang Di Muka
            //Tambah Saldo Customer
            Context.ptnr_mstr dbptnr = Repoptnr_mstr.FindByPK(dbitem.Id);

            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        public JsonResult DeleteDp(int IdSo, int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByOnCall(IdSo);
            Context.SalesOrderOncallDp dbdp = dbitem.SalesOrderOncall.SalesOrderOncallDp.Where(d => d.Id == id).FirstOrDefault();
            dbitem.SalesOrderOncall.SalesOrderOncallDp.Remove(dbdp);
            ResponeModel response = new ResponeModel(true);

            RepoSalesOrder.save(dbitem);

            return Json(response);
        }
    }
}