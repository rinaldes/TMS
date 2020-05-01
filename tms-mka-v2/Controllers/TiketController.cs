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
    public class TiketController : BaseController 
    {
        private ITiketRepo RepoTiket;
        private ICustomerRepo RepoCustomer;
        private IUserRepo RepoCS;
        private ISalesOrderRepo RepoSalesOrder;
        private IAtmRepo RepoAtm;
        private IDataBoronganRepo RepoBor;
        public TiketController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            ITiketRepo repoTiket, ICustomerRepo repoCustomer, IUserRepo repoCS, ISalesOrderRepo repoSalesOrder, IAtmRepo repoAtm, IDataBoronganRepo repoBor)
            : base(repoBase, repoLookup)
        {   
            RepoTiket = repoTiket;
            RepoCustomer = repoCustomer;
            RepoCS = repoCS;
            RepoSalesOrder = repoSalesOrder;
            RepoAtm = repoAtm;
            RepoBor = repoBor;
        }
        [MyAuthorize(Menu = "Tiket", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "Tiket").ToList();
            return View();
        }
        public string GetCS()
        {
            List<Context.User> dbitems = new List<Context.User>();
            dbitems = RepoCS.FindAll().ToList();
            return new JavaScriptSerializer().Serialize(dbitems.Select(x => new
            {
                Id = x.Id,
                Nama = x.Nik + " | " + x.Fristname + " " + x.Lastname
            }));
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Tiket> items = RepoTiket.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            if (!UserPrincipal.IsSuperadmin())
                items = items.Where(d => d.IdCS == UserPrincipal.id || d.DitujukanKe.Split(',').Contains(UserPrincipal.id.ToString())).ToList();

            List<Tiket> ListModel = new List<Tiket>();
            foreach (Context.Tiket item in items)
            {
                ListModel.Add(new Tiket(item));
            }

            int total = RepoTiket.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [MyAuthorize(Menu = "Tiket", Action="read")]
        public ActionResult Add()
        {
            Tiket model = new Tiket();
            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(Tiket model)
        {
            if (ModelState.IsValid)
            {
                Context.Tiket dbitem = new Context.Tiket();
                model.setDb(dbitem);
                dbitem.IdCS = UserPrincipal.id;
                dbitem.Urutan = RepoTiket.getUrutan() + 1;
                dbitem.NoTiket = RepoTiket.generateCodePPK(model.Urutan);
                dbitem.CreatedBy = UserPrincipal.id;
                RepoTiket.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Tiket", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.Tiket dbitem = RepoTiket.FindByPK(id);

            Tiket model = new Tiket(dbitem);
            ViewBag.name = model.NoTiket;
            ViewBag.Creator = dbitem.CS.Fristname + " " + dbitem.CS.Lastname;
            if(dbitem.DitujukanKe.Split(',').Contains(UserPrincipal.id.ToString()) || UserPrincipal.id == dbitem.CreatedBy){
                ViewBag.Responses = dbitem.TiketResponse;
                ViewBag.setReadonly = dbitem.DitujukanKe.Split(',').Contains(UserPrincipal.id.ToString()) ? "true" : "false";
                ViewBag.SO = dbitem.SalesOrder.SalesOrderOncall;
                ViewBag.TanggalTiba = RepoSalesOrder.TanggalTiba(dbitem.SalesOrder.SalesOrderOncall);
                ViewBag.CreatedBy = dbitem.CreatedBy;
                return View("FormComment", model);
            }
            else
                return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(Tiket model)
        {
            if (ModelState.IsValid)
            {
                Context.Tiket dbitem = RepoTiket.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoTiket.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Tiket", Action="delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Tiket dbItem = RepoTiket.FindByPK(id);

            RepoTiket.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        [HttpGet]
        public PartialViewResult GetPartialSo(int idSo, string ListIdSo)
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
            else if (dbitem.SalesOrderKontrakId.HasValue)
            {
                List<int> ListIdDumy = ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;
                AdminUangJalan model = new AdminUangJalan(dbsoDummy.FirstOrDefault().AdminUangJalan);
                model.ModelKontrak = new SalesOrderKontrak(dbitem);
                return PartialView("SalesOrderKontrak/_PartialFormReadOnly", model);
            }
            return PartialView("");
        }
    }
}