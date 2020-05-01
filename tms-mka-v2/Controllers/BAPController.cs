using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using System.Web.Script.Serialization;
using tms_mka_v2.Infrastructure;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using OfficeOpenXml;
using System.Globalization;

namespace tms_mka_v2.Controllers
{
    public class BAPController : BaseController
    {
        private IBAPRepo RepoBAP;
        private ISalesOrderRepo RepoSalesOrder;
        private IAtmRepo RepoAtm;
        private IDataBoronganRepo RepoBor;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        public BAPController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookUp, IBAPRepo repoBAP, ISalesOrderRepo repoSalesOrder,
            IAtmRepo repoAtm, IDataBoronganRepo repoBor, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo)
            : base(repoBase, repoLookUp)
        {
            RepoBAP = repoBAP;
            RepoSalesOrder = repoSalesOrder;
            RepoAtm = repoAtm;
            RepoBor = repoBor;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "BAP").ToList();

            return View();
        }
        public string BindingBAPForKlaim()
        {
            List<Context.BAP> items = RepoBAP.FindAllAll();

            List<BAP> ListModel = new List<BAP>();
            foreach (Context.BAP item in items)
            {
                ListModel.Add(new BAP(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = items.Count(), data = ListModel });
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.BAP> items = RepoBAP.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<BAP> ListModel = new List<BAP>();
            foreach (Context.BAP item in items)
            {
                ListModel.Add(new BAP(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoBAP.Count(param.Filters), data = ListModel });
        }
        public string BindingBySo(int idSo, int? idSoKontrak)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.BAP> items = RepoBAP.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            if(idSoKontrak.HasValue)
                items = items.Where(d => d.SalesOrderId == idSo && d.SalesOrderKontrakId == idSoKontrak && d.Status == "Open").ToList();
            else
                items = items.Where(d => d.SalesOrderId == idSo && d.Status == "Open").ToList();
            List<BAP> ListModel = new List<BAP>();
            foreach (Context.BAP item in items)
            {
                ListModel.Add(new BAP(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count, data = ListModel });
        }

        public string BindingSalesOrder()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKlaim(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<ListOrder> ListModel = new List<ListOrder>();
            foreach (Context.SalesOrder item in items)
            {
                ListModel.Add(new ListOrder(item));
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountSoKlaim(param.Filters), data = ListModel });
        }

        public string BindingSalesOrderKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrderKontrakListSo> items = RepoSalesOrder.FindAllKlaimKontrak(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<ListOrder> ListModel = new List<ListOrder>();
            foreach (Context.SalesOrderKontrakListSo item in items)
            {
                ListModel.Add(new ListOrder(item));
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountSoKlaimKontrak(param.Filters), data = ListModel });
        }

        public ActionResult Add()
        {
            BAP model = new BAP();

            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(BAP model)
        {
            if (ModelState.IsValid)
            {
                int seq = RepoBAP.getUrutanOnCAll(model.TanggalKejadian.Value)+1;
                model.NoBAP = RepoBAP.GenerateCode(model.TanggalKejadian.Value, seq);
                Context.BAP dbitem = new Context.BAP();
                model.setDb(dbitem);
                if (model.IdDataTruck != null)
                    dbitem.IdDataTruck = model.IdDataTruck;
                RepoBAP.save(dbitem, UserPrincipal.id);
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            BAP model = new BAP(RepoBAP.FindByPK(id));
            ViewBag.name = model.NoBAP;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(BAP model)
        {
            if (ModelState.IsValid)
            {
                Context.BAP dbitem = RepoBAP.FindByPK(model.Id);
                bool Isexist = RepoBAP.IsExist(model.SOBapId, model.SOBapKontrakId, model.Driver1Id, model.IdDataTruck, model.KategoriId, model.Id);

                if (Isexist)
                {
                    ModelState.AddModelError("NoBAP", "BAP telah dipakai.");
                    return View("Form", model);
                }
                model.setDb(dbitem);
                RepoBAP.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.BAP dbItem = RepoBAP.FindByPK(id);

            RepoBAP.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        [HttpGet]
        public PartialViewResult GetPartialSo(int idSo, int IdSoKontrak = 0)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                AdminUangJalan model = new AdminUangJalan(dbitem.AdminUangJalan);
                model.ModelOncall = new SalesOrderOncall(dbitem);
                return PartialView("SalesOrderOncall/_PartialFormReadOnly", model.ModelOncall);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                AdminUangJalan model = new AdminUangJalan(dbitem.AdminUangJalan);
                model.ModelPickup = new SalesOrderPickup(dbitem);
                return PartialView("SalesOrderPickup/_PartialFormReadOnly", model.ModelPickup);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                AdminUangJalan model = new AdminUangJalan(dbitem.AdminUangJalan);
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                return PartialView("SalesOrderProsesKonsolidasi/_PartialFormReadOnly", model.ModelKonsolidasi);
            }
            else if (IdSoKontrak != 0)
            {
                //List<int> ListIdDumy = ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                Context.SalesOrderKontrakListSo dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == IdSoKontrak).FirstOrDefault();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Clear();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Add(dbsoDummy);
                AdminUangJalan model = new AdminUangJalan(dbsoDummy.AdminUangJalan);
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
                return PartialView("SalesOrderKontrak/_PartialFormReadOnly", model);
            }
            return PartialView("");
        }

        [HttpGet]
        public PartialViewResult GetPartialSoKontrak(int idSo)
        {
            Context.SalesOrderKontrakListSo dbsoDummy = RepoSalesOrderKontrakListSo.FindAll().Where(d => d.Id == idSo).FirstOrDefault();
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(dbsoDummy.SalesKontrakId.Value);
            AdminUangJalan model = new AdminUangJalan(dbsoDummy.AdminUangJalan);
            model.ModelKontrak = new SalesOrderKontrak(dbitem);
            ViewBag.NoSo = dbsoDummy.NoSo;
            return PartialView("SalesOrderKontrak/_PartialFormReadOnly", model);
        }

        public string GetDetailSo(int idSo, int IdSoKontrak = 0)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(idSo);

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                return new JavaScriptSerializer().Serialize(
                    new
                    {
                        driverId1 = dbitem.SalesOrderOncall.Driver1Id,
                        kodeDriver1 = dbitem.SalesOrderOncall.Driver1.KodeDriver,
                        namaDriver1 = dbitem.SalesOrderOncall.Driver1.NamaDriver,
                        driverId2 = dbitem.SalesOrderOncall.Driver2Id,
                        kodeDriver2 = dbitem.SalesOrderOncall.Driver2Id.HasValue ? dbitem.SalesOrderOncall.Driver1.KodeDriver : "",
                        namaDriver2 = dbitem.SalesOrderOncall.Driver2Id.HasValue ? dbitem.SalesOrderOncall.Driver1.NamaDriver : "",
                        dataTruckId = dbitem.SalesOrderOncall.IdDataTruck,
                        vehicleNo = dbitem.SalesOrderOncall.DataTruck.VehicleNo,
                        jenisTruck = dbitem.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck,
                        jenisPendingin = dbitem.SalesOrderOncall.DataTruck.DataPendingin.Count > 0 ? dbitem.SalesOrderOncall.DataTruck.DataPendingin.OrderBy(p => p.Id).Last().Model : "",
                    });
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                return new JavaScriptSerializer().Serialize(
                    new
                    {
                        driverId1 = dbitem.SalesOrderPickup.Driver1Id,
                        kodeDriver1 = dbitem.SalesOrderPickup.Driver1.KodeDriver,
                        namaDriver1 = dbitem.SalesOrderPickup.Driver1.NamaDriver,
                        driverId2 = dbitem.SalesOrderPickup.Driver2Id,
                        kodeDriver2 = dbitem.SalesOrderPickup.Driver2Id.HasValue ? dbitem.SalesOrderPickup.Driver1.KodeDriver : "",
                        namaDriver2 = dbitem.SalesOrderPickup.Driver2Id.HasValue ? dbitem.SalesOrderPickup.Driver1.NamaDriver : "",
                        dataTruckId = dbitem.SalesOrderPickup.IdDataTruck,
                        vehicleNo = dbitem.SalesOrderPickup.DataTruck.VehicleNo,
                        jenisTruck = dbitem.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck,
                        jenisPendingin = dbitem.SalesOrderPickup.DataTruck.DataPendingin.Count > 0 ? dbitem.SalesOrderPickup.DataTruck.DataPendingin.OrderBy(p => p.Id).Last().Model : "",
                    });
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                return new JavaScriptSerializer().Serialize(
                    new
                    {
                        driverId1 = dbitem.SalesOrderProsesKonsolidasi.Driver1Id,
                        kodeDriver1 = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver,
                        namaDriver1 = dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver,
                        driverId2 = dbitem.SalesOrderProsesKonsolidasi.Driver2Id,
                        kodeDriver2 = dbitem.SalesOrderProsesKonsolidasi.Driver2Id.HasValue ? dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver : "",
                        namaDriver2 = dbitem.SalesOrderProsesKonsolidasi.Driver2Id.HasValue ? dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver : "",
                        dataTruckId = dbitem.SalesOrderProsesKonsolidasi.IdDataTruck,
                        vehicleNo = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo,
                        jenisTruck = dbitem.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck,
                        jenisPendingin = dbitem.SalesOrderProsesKonsolidasi.DataTruck.DataPendingin.Count > 0 ? dbitem.SalesOrderProsesKonsolidasi.DataTruck.DataPendingin.OrderBy(p => p.Id).Last().Model : "",
                    });
            }
            return "";
        }
        public string GetDetailSoKontrak(int idSo)
        {
            Context.SalesOrderKontrakListSo dbsoDummy = RepoSalesOrderKontrakListSo.FindAll().Where(d => d.Id == idSo).FirstOrDefault();
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(dbsoDummy.SalesKontrakId.Value);
            return new JavaScriptSerializer().Serialize(
                new
                {
                    driverId1 = dbsoDummy.Driver1Id, kodeDriver1 = dbsoDummy.Driver1.KodeDriver, namaDriver1 = dbsoDummy.Driver1.NamaDriver, driverId2 = dbsoDummy.Driver2Id,
                    kodeDriver2 = dbsoDummy.Driver2Id.HasValue ? dbsoDummy.Driver1.KodeDriver : "",
                    namaDriver2 = dbsoDummy.Driver2Id.HasValue ? dbsoDummy.Driver1.NamaDriver : "", dataTruckId = dbsoDummy.IdDataTruck,
                    vehicleNo = dbsoDummy.DataTruck.VehicleNo, jenisTruck = dbsoDummy.DataTruck.JenisTrucks.StrJenisTruck,
                    jenisPendingin = dbsoDummy.DataTruck.DataPendingin.Count > 0 ? dbsoDummy.DataTruck.DataPendingin.OrderBy(p => p.Id).Last().Model : "",
                }
            );
        }
        public string GetBAP()
        {
            List<Context.BAP> dbitems = RepoBAP.FindAll();
            return new JavaScriptSerializer().Serialize(dbitems.Select(d => new { Id = d.Id, Nama = d.NoBAP }));
        }
    }
}