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
    public class SalesOrderPickupController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IAuditrailRepo RepoAuditrail;
        public SalesOrderPickupController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IAuditrailRepo repoAuditrail)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoAuditrail = repoAuditrail;
        }
        [MyAuthorize(Menu = "Sales Order Konsolidasi Pickup", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SalesOrderPickup").ToList();
            return View();
        }

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllPickUp().Where(d => d.Status.ToLower() == "draft").ToList();
            List<SalesOrderPickup> ListModel = new List<SalesOrderPickup>();
            foreach (Context.SalesOrder item in items)
            {
                ListModel.Add(new SalesOrderPickup(item));
            }

            int total = RepoSalesOrder.CountOncall(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingComment(int id)
        {
            Context.SalesOrder items = RepoSalesOrder.FindByPickup(id);

            return new JavaScriptSerializer().Serialize(new
            {
                data = items.SalesOrderPickup.SalesOrderPickupComment.ToList().Select(d => new
                {
                    Id = d.Id,
                    Tanggal = d.Tanggal,
                    CommentUser = d.CommentUser,
                    Username = d.Username,
                    Action = d.Action
                })
            });
        }
        [MyAuthorize(Menu = "Sales Order Konsolidasi Pickup", Action="create")]
        public ActionResult Add()
        {
            SalesOrderPickup model = new SalesOrderPickup();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(SalesOrderPickup model, string btnsave)
        {
            SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrLoad);
            model.ListLoad = resultLoad.ToList();
            SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrUnload);
            model.ListUnload = resultUnload.ToList();
            if (ModelState.IsValid)
            {
                Context.SalesOrder dbso = new Context.SalesOrder();
                Context.SalesOrderPickup dbitem = new Context.SalesOrderPickup();
                model.setDb(dbitem);
                dbitem.Urutan = RepoSalesOrder.getUrutanPickup(model.TanggalPickup.Value) + 1;
                dbitem.SONumber = RepoSalesOrder.generatePickup(model.TanggalPickup.Value, dbitem.Urutan);
                dbso.SalesOrderPickup = dbitem;

                if (btnsave == "save")
                    dbso.Status = "draft planning";
                else if (btnsave == "draft")
                    dbso.Status = "draft";
                else
                    dbso.Status = model.Status;
                dbso.CreatedBy = UserPrincipal.id;
                dbso.UpdatedBy = UserPrincipal.id;

                RepoSalesOrder.save(dbso);
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }

        [MyAuthorize(Menu = "Sales Order Konsolidasi Pickup", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderPickup model = new SalesOrderPickup(dbitem);

            ViewBag.name = model.SONumber;
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Edit(SalesOrderPickup model, string btnsave)
        {
            SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrLoad);
            model.ListLoad = resultLoad.ToList();
            SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrUnload);
            model.ListUnload = resultUnload.ToList();
            if (ModelState.IsValid)
            {
                Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.SalesOrderId.Value);

                model.setDb(dbitem.SalesOrderPickup);
                RepoAuditrail.saveDelAllSalesOrderPickupUnLoadingAddQuery(dbitem.SalesOrderPickup, UserPrincipal.id);
                foreach (Context.SalesOrderPickupUnLoadingAdd sopula in dbitem.SalesOrderPickup.SalesOrderPickupUnLoadingAdd){
                    RepoAuditrail.saveSalesOrderPickupUnLoadingAddQuery(sopula, UserPrincipal.id);
                }
                if (btnsave == "save")
                    dbitem.Status = "save";
                else if (btnsave == "draft")
                    dbitem.Status = "draft";
                else
                    dbitem.Status = model.Status;

                RepoSalesOrder.save(dbitem);


                return RedirectToAction("Index");
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

        #region options
        //public string getExistSo(string currList)
        public string getExistSo()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            //List<int> listid = new List<int>();
            //if (currList != "")
            //{
            //    foreach (string item in currList.Split(new string[] { ", " }, StringSplitOptions.None))
            //    {
            //        listid.Add(int.Parse(item));
            //    }
            //}
            List<Context.SalesOrder> db = RepoSalesOrder.FindAllPickUp(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).Where(d => d.SalesOrderPickup.IsSelect == false).ToList();

            //if (listid.Count() > 0)
            //    db = db.Where(d => listid.Contains(d.SalesOrderPickupId.Value)).ToList();

            return new JavaScriptSerializer().Serialize(db.
                Select(d => new
                {
                    Id = d.SalesOrderPickupId,
                    NoSo = d.SalesOrderPickup.SONumber
                })
            );
        }

        [HttpPost]
        public JsonResult Submit(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SalesOrder dbItem = RepoSalesOrder.FindByPK(id);
            dbItem.isReturn = false;
            dbItem.Status = "save";
            RepoSalesOrder.save(dbItem);

            return Json(response);
        }
        #endregion

        #region inputDp
        public string BindingDp(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.SalesOrder items = RepoSalesOrder.FindByPickup(id);

            return new JavaScriptSerializer().Serialize(new
            {
                data = items.SalesOrderPickup.SalesOrderPickupDp.ToList().Select(d => new {
                    Id = d.Id,
                    Tanggal = d.Tanggal,
                    Penerima = d.Penerima,
                    IdRekening = d.RekeningId,
                    NoRekening = d.Rekenings.NoRekening,
                    Jenis = d.Jenis,
                    Jumlah = d.Jumlah
                })
            });
        }

        public ActionResult InputDp(int id, string status)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderPickup model = new SalesOrderPickup(dbitem);

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
            return View("SalesOrderPickup/FormReadOnly", model);

        }

        [HttpPost]
        public JsonResult SaveDp(Context.SalesOrderPickupDp model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPickup(model.SalesOrderPickupId);
            if (model.Id == 0){
                dbitem.SalesOrderPickup.SalesOrderPickupDp.Add(model);
                var query = "INSERT INTO \"dbo\".\"SalesOrderPickupDp\" (\"SalesOrderPickupId\", \"Tanggal\", \"Penerima\", \"Jenis\", \"RekeningId\", \"Jumlah\") VALUES (" + model.SalesOrderPickupId + 
                    ", " + model.Tanggal + ", " + model.Penerima + ", " + model.Jenis + ", " + model.RekeningId + ", " + model.Jumlah + ");";
                RepoAuditrail.SetAuditTrail(query, "Input DP", "List Order", UserPrincipal.id);
            }
            else
            {
                Context.SalesOrderPickupDp dbdp = dbitem.SalesOrderPickup.SalesOrderPickupDp.Where(d => d.Id == model.Id).FirstOrDefault();
                dbdp.Tanggal = model.Tanggal;
                dbdp.Penerima = model.Penerima;
                dbdp.RekeningId = model.RekeningId;
                dbdp.Jenis = model.Jenis;
                dbdp.Jumlah = model.Jumlah;
                var query = "UPDATE \"dbo\".\"SalesOrderPickupDp\" SET \"Tanggal\" = " + dbdp.Tanggal + ", \"Penerima\" = " + dbdp.Penerima + ", \"Jenis\" = \" = " + dbdp.Jenis +
                    ", \"RekeningId\" = " + dbdp.RekeningId + "\"Jumlah\" = " + dbdp.Jumlah + " WHERE \"Id\" = " + dbdp.Id + ";";
                RepoAuditrail.SetAuditTrail(query, "Input DP", "List Order", UserPrincipal.id);
            }

            RepoSalesOrder.save(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        public JsonResult DeleteDp(int IdSo, int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPickup(IdSo);
            Context.SalesOrderPickupDp dbdp = dbitem.SalesOrderPickup.SalesOrderPickupDp.Where(d => d.Id == id).FirstOrDefault();
            dbitem.SalesOrderPickup.SalesOrderPickupDp.Remove(dbdp);
            ResponeModel response = new ResponeModel(true);

            var query = "DELETE FROM \"dbo\".\"SalesOrderPickupDp\" WHERE \"Id\"= " + id + ";";
            RepoSalesOrder.save(dbitem);
            RepoAuditrail.SetAuditTrail(query, "Input DP", "Delete SO Pickup", UserPrincipal.id);

            return Json(response);
        }
        #endregion inputDp
    }
}