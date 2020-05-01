using Npgsql;
using System.Data;
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
    public class DokumenController : BaseController
    {
        private IDokumenRepo RepoDokumen;
        private ICustomerRepo RepoCustomer;
        private ISalesOrderRepo RepoSalesOrder;
        private Iso_mstrRepo Reposo_mstr;
        private IERPConfigRepo RepoERPConfig;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private IDaftarHargaKontrakRepo RepoDHKontrak;
        private IAdminUangJalanRepo RepoAUJ;
        public DokumenController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDokumenRepo repoDokumen, ICustomerRepo repoCustomer, ISalesOrderRepo repoSalesOrder, Iso_mstrRepo reposo_mstr,
            IERPConfigRepo repoERPConfig, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo, IDaftarHargaKontrakRepo repoDHKontrak, IAdminUangJalanRepo repoAUJ)
            : base(repoBase, repoLookup)
        {
            RepoDokumen = repoDokumen;
            RepoCustomer = repoCustomer;
            RepoSalesOrder = repoSalesOrder;
            RepoERPConfig = repoERPConfig;
            Reposo_mstr = reposo_mstr;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
            RepoDHKontrak = repoDHKontrak;
            RepoAUJ = repoAUJ;
        }
        [MyAuthorize(Menu = "Dokumen Surat Jalan", Action = "read")]
        public ActionResult Index(string caller)
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Dokumen").ToList();
            ViewBag.caller = caller;
            if (caller == "admin")
                ViewBag.Title = "Dokumen Admin Surat Jalan";
            else
                ViewBag.Title = "Dokumen Billing";

            return View();
        }
        [MyAuthorize(Menu = "Dokumen Billing", Action = "read")]
        public ActionResult Billing(string caller)
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Dokumen").ToList();
            ViewBag.caller = caller;
            ViewBag.Title = "Dokumen Billing";
            return View();
        }

        public string generateIKSNo()
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKonsolidasi().Where(d => d.SalesOrderKonsolidasi.IsSelect && d.SalesOrderKonsolidasi.IKSNo == null).ToList();
            foreach (var item in items)
            {
                RepoSalesOrder.save(item);
            }
            return "aa";
        }

        public string generateCustomerId()
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d => d.CustomerId == null || d.DriverId == null || d.DataTruckId == null || d.OrderTanggalMuat == null).ToList();
            foreach (var item in items)
            {
                RepoSalesOrder.save(item);
            }
            return "aa";
        }
        public string generateProsesKonsolidasiId()
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAll().Where(d => d.SalesOrderProsesKonsolidasiId != null && d.OrderTanggalMuat == null).ToList();
            foreach (var item in items)
            {
                RepoSalesOrder.save(item);
            }
            return "aa";
        }
        public string generateIsLengkap()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.Dokumen> items = RepoDokumen.FindAll().ToList();

            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            foreach (var item in items.Where(d => d.IsAdmin))
            {

                RepoDokumen.save(item, UserPrincipal.id);
            }
            return "aa";
        }

        public string generateDokumenKSO()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.Dokumen> items = RepoDokumen.FindAll().Where(d => d.SalesOrder.SalesOrderProsesKonsolidasiId != null).ToList();

            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            foreach (var item in items.Where(d => d.IsAdmin))
            {
                item.IdSO = RepoDokumen.FindBySalesOrderProsesKonsolidasiIdAndCust(item.IdSO.Value, item.IdCustomer.Value).Id;
                RepoDokumen.save(item, UserPrincipal.id);
            }
            return "aa";
        }

        public string generateDokumenKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.Dokumen> items = RepoDokumen.FindAll().Where(d => d.ListIdSo != null && d.ListIdSo != "").ToList();
            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            foreach (var item in items)
            {
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(item.ListIdSo.Split(',')[0]));
                if (sokls != null && sokls.DokumenId == null)
                {
                    RepoSalesOrderKontrakListSo.save(sokls);
                }
            }
            return "aa";
        }

        public string generateDokumenKontrakAgain()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrderKontrakListSo> soklss = RepoSalesOrderKontrakListSo.FindAll().Where(d => d.DokumenId == null && d.Status == "dispatched").ToList();
            foreach (var item in soklss)
            {
                RepoSalesOrderKontrakListSo.save(item);
            }
            return "aa";
        }

        public ActionResult IndexKontrak(string caller)
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Dokumen").ToList();
            ViewBag.caller = caller;
            if (caller == "admin")
                ViewBag.Title = "Dokumen Admin Surat Jalan Kontrak";
            else
                ViewBag.Title = "Dokumen Billing";
            List<Context.SalesOrderKontrakListSo> soklss = RepoSalesOrderKontrakListSo.FindAll().Where(d => d.DokumenId == null && d.Status == "dispatched").ToList();
            return View("IndexKontrak");
        }

        public ActionResult GenerateDokKontrak(string caller)
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Dokumen").ToList();
            ViewBag.caller = caller;
            if (caller == "admin")
                ViewBag.Title = "Dokumen Admin Surat Jalan Kontrak";
            else
                ViewBag.Title = "Dokumen Billing";
            List<Context.SalesOrderKontrakListSo> soklss = RepoSalesOrderKontrakListSo.FindAll().Where(d => d.DokumenId == null && d.Status == "dispatched").ToList();
            foreach (var item in soklss)
            {
                RepoSalesOrderKontrakListSo.save(item);
            }
            return View("IndexKontrak");
        }

        public ActionResult IndexKonsolidasi(string caller)
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Dokumen").ToList();
            ViewBag.caller = caller;
            if (caller == "admin")
                ViewBag.Title = "Dokumen Admin Surat Jalan Konsolidasi";
            else
                ViewBag.Title = "Dokumen Billing";

            return View();
        }

        public string Binding(string caller)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.Dokumen> items = RepoDokumen.FindAllOnCallSJ(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            if (caller == "admin")
            {
                foreach (var item in items.Where(d => d.IsAdmin))
                {
                    ListModel.Add(new DokumenIndex(item));
                    item.IsLengkap = !item.DokumenItem.Any(d => !d.IsLengkap);
                    RepoDokumen.save(item, UserPrincipal.id);
                }
                return new JavaScriptSerializer().Serialize(new { total = RepoDokumen.Count(param.Filters), data = ListModel });
            }
            else
            {
                items = RepoDokumen.FindAllBilling(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
                foreach (var item in items)
                {
                    if (item.ListIdSo != "" && item.ListIdSo != null)
                    {
                        DokumenIndex di = new DokumenIndex(item);
                        Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(item.ListIdSo));
                        di.NoSo = sokls.NoSo;
                        di.TanggalMuat = sokls.MuatDate;
                        if (sokls.DataTruck != null)
                        {
                            di.VehicleNo = sokls.DataTruck.VehicleNo;
                            di.JnsTruck = sokls.DataTruck.JenisTrucks.StrJenisTruck;
                        }
                        if (sokls.Driver1 != null)
                            di.NamaDriver = sokls.Driver1.NamaDriver;
                        ListModel.Add(di);

                    }
                    else
                        ListModel.Add(new DokumenIndex(item));
                }
                return new JavaScriptSerializer().Serialize(new { total = RepoDokumen.CountBilling(param.Filters), data = ListModel });
            }
        }

        public string BindingBilling(string caller)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            List<Context.Dokumen> items = RepoDokumen.FindAllBilling().ToList();
            foreach (var item in items)
            {
                ListModel.Add(new DokumenIndex(item));
            }
            return new JavaScriptSerializer().Serialize(new { total = RepoDokumen.CountBilling(), data = ListModel });
        }

        public string BindingKonsolidasi(string caller)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.Dokumen> items = RepoDokumen.FindAllKonsolidasiSJ(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            if (caller == "admin")
            {
                foreach (var item in items.Where(d => d.IsAdmin))
                {
                    ListModel.Add(new DokumenIndex(item));
                    item.IsLengkap = !item.DokumenItem.Any(d => !d.IsLengkap);
                    RepoDokumen.save(item, UserPrincipal.id);
                    Context.SalesOrder so = RepoSalesOrder.FindByPK(item.IdSO.Value);
                    so.SONumber = so.SalesOrderKonsolidasi.SONumber;
                    RepoSalesOrder.save(so);
                }
                return new JavaScriptSerializer().Serialize(new { total = RepoDokumen.CountKonsolidasiSJ(param.Filters), data = ListModel });
            }
            else
            {
                items = RepoDokumen.FindAllBilling(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
                foreach (var item in items)
                {
                    ListModel.Add(new DokumenIndex(item));
                }
                return new JavaScriptSerializer().Serialize(new { total = RepoDokumen.CountBilling(param.Filters), data = ListModel });
            }
        }

        public string BindingKontrak(string caller)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrderKontrakListSo> items = RepoDokumen.FindAllKontrakSJ(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<DokumenIndex> ListModel = new List<DokumenIndex>();
            foreach (var item in items.Where(d => d.Dokumen.IsAdmin))
            {
                DokumenIndex di = new DokumenIndex(item.Dokumen);
                di.NoSo = item.NoSo;
                di.TanggalMuat = item.MuatDate;
                if (item.DataTruck != null)
                {
                    di.VehicleNo = item.DataTruck.VehicleNo;
                    di.JnsTruck = item.DataTruck.JenisTrucks.StrJenisTruck;
                }
                else
                {
                    di.VehicleNo = "bete";
                    di.JnsTruck = "bete1";
                }
                if (item.Driver1 != null)
                    di.NamaDriver = item.Driver1.NamaDriver;
                ListModel.Add(di);
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoDokumen.CountKontrak(param.Filters), data = ListModel });
        }

        [MyAuthorize(Menu = "Dokumen Surat Jalan", Action = "update")]
        public ActionResult Edit(int id, string caller = "billing")
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(id);
            List<int> ListIdDokumen = dbitem.DokumenItem.Select(b => b.IdBilling).ToList();
            string strQuery = "";
            //cek apakah di billing customer ada penambahan data
            //jika ada maka update data jika data dokumen belum close
            if (!dbitem.IsComplete)
            {
                Context.Customer dbcust = dbitem.Customer;
                foreach (var itemBilling in dbitem.Customer.CustomerBilling.Where(i => !ListIdDokumen.Contains(i.Id) && i.IsActive).ToList())
                {
                    dbitem.DokumenItem.Add(new Context.DokumenItem()
                    {
                        IdBilling = itemBilling.Id,
                        CustomerId = itemBilling.CustomerId,
                        ModifiedDate = DateTime.Now,
                    });
                    strQuery += "INSERT INTO dbo.\"DokumenItem\" (\"IdBilling\", \"CustomerId\", \"ModifiedDate\") VALUES (" + itemBilling.Id + ", " + itemBilling.CustomerId + ", " + DateTime.Now + ");";
                }
                RepoDokumen.save(dbitem, UserPrincipal.id, strQuery);
            }
            Dokumen model = new Dokumen(dbitem);
            ViewBag.caller = caller;
            try {
                ViewBag.KeteranganGanti = RepoAUJ.FindAll().Where(d => d.SONumber.Contains(dbitem.SONumber) && d.Status != "Batal").FirstOrDefault();
            }
            catch (Exception) { }
            if (caller == "admin")
            {
                ViewBag.Title = "Dokumen Admin Surat Jalan";
                if (dbitem.ListIdSo != "" && dbitem.ListIdSo != null)
                    ViewBag.backlink = Url.Action("IndexKontrak", new { caller = ViewBag.caller });
                else
                    ViewBag.backlink = Url.Action("Index", new { caller = ViewBag.caller });
            }
            else
                ViewBag.Title = "Dokumen Billing";
            if (dbitem.SalesOrder != null && dbitem.SalesOrder.SalesOrderKonsolidasiId.HasValue)
            {
                ViewBag.SO = RepoSalesOrder.FindByKonsolidasiId(dbitem.SalesOrder.SalesOrderKonsolidasiId.Value).SalesOrderProsesKonsolidasi;
                ViewBag.SOBarang = dbitem.SalesOrder.SalesOrderKonsolidasi;
                ViewBag.Supplier = RepoCustomer.FindByPK(dbitem.SalesOrder.SalesOrderKonsolidasi.SupplierId.Value);
                ViewBag.Customer = RepoLookup.FindByPK(dbitem.SalesOrder.SalesOrderKonsolidasi.CustomerId);
                ViewBag.Tagihan = RepoCustomer.FindByPK(dbitem.SalesOrder.SalesOrderKonsolidasi.NamaTagihanId.Value);
                return View("FormKonsolidasi", model);
            }
            else
                return View("Form", model);
        }

        [MyAuthorize(Menu = "Dokumen Billing", Action = "update")]
        public ActionResult EditBilling(int id, string caller = "billing")
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(id);
            List<int> ListIdDokumen = dbitem.DokumenItem.Select(b => b.IdBilling).ToList();
            string strQuery = "";
            //cek apakah di billing customer ada penambahan data
            //jika ada maka update data jika data dokumen belum close
            if (!dbitem.IsComplete)
            {
                Context.Customer dbcust = dbitem.Customer;
                foreach (var itemBilling in dbitem.Customer.CustomerBilling.Where(i => !ListIdDokumen.Contains(i.Id) && i.IsActive).ToList())
                {
                    dbitem.DokumenItem.Add(new Context.DokumenItem()
                    {
                        IdBilling = itemBilling.Id,
                        CustomerId = itemBilling.CustomerId,
                        ModifiedDate = DateTime.Now,
                    });
                    strQuery += "INSERT INTO dbo.\"DokumenItem\" (\"IdBilling\", \"CustomerId\", \"ModifiedDate\") VALUES (" + itemBilling.Id + ", " + itemBilling.CustomerId + ", " + DateTime.Now + ");";
                }
                RepoDokumen.save(dbitem, UserPrincipal.id, strQuery);
            }
            Dokumen model = new Dokumen(dbitem);
            ViewBag.caller = caller;
            ViewBag.Title = "Dokumen Billing";
            ViewBag.backlink = Url.Action("Billing", new { caller = ViewBag.caller });
            if (dbitem.SalesOrder != null && dbitem.SalesOrder.SalesOrderKonsolidasiId.HasValue)
            {
                ViewBag.SO = RepoSalesOrder.FindByKonsolidasiId(dbitem.SalesOrder.SalesOrderKonsolidasiId.Value).SalesOrderProsesKonsolidasi;
                ViewBag.SOBarang = dbitem.SalesOrder.SalesOrderKonsolidasi;
                ViewBag.Supplier = RepoCustomer.FindByPK(dbitem.SalesOrder.SalesOrderKonsolidasi.SupplierId.Value);
                ViewBag.Customer = RepoLookup.FindByPK(dbitem.SalesOrder.SalesOrderKonsolidasi.CustomerId);
                ViewBag.Tagihan = RepoCustomer.FindByPK(dbitem.SalesOrder.SalesOrderKonsolidasi.NamaTagihanId.Value);
                return View("FormKonsolidasi", model);
            }
            else
                return View("Form", model);
        }
        public string reloadSalesOrderKontrak()
        {
            foreach (Context.Dokumen dokumen in RepoDokumen.FindAll().Where(d => d.IdSO == null))
            {
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dokumen.ListIdSo.ToString()));
                if (sokls != null && sokls.SalesKontrakId != null)
                {
                    if (RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value) != null)
                    {
                        dokumen.IdSO = RepoSalesOrder.FindByKontrak(sokls.SalesKontrakId.Value).Id;
                        RepoDokumen.save(dokumen, UserPrincipal.id);
                    }
                }
            }
            return "aa";
        }

        [HttpPost]
        public ActionResult Edit(Dokumen model, string btnSubmit, string caller)
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(model.Id);
            DokumenItem[] resDok = JsonConvert.DeserializeObject<DokumenItem[]>(model.strDokumen);
            string strQuery = "";
            foreach (var item in resDok.Where(d => d.IsEdit))
            {
                item.SetDb(dbitem.DokumenItem.Where(d => d.Id == item.Id).FirstOrDefault());
                Context.DokumenItemHistory dih = new Context.DokumenItemHistory();
                dih.UserId = UserPrincipal.id;
                dbitem.DokumenItemHistory.Add(item.SetDbHistory(dih));
                strQuery += "INSERT INTO dbo.\"DokumenItemHistory\" (\"IdDok\", \"Nama\", \"Jml\", \"Warna\", \"Stempel\", \"Lengkap\", \"KeteranganAdmin\", \"KeteranganBilling\", \"ModifiedDate\") VALUES (" + dbitem.Id +
                    ", " + item.Nama + ", " + item.Jml + ", " + item.Warna + ", " + item.Stempel + ", " + item.Lengkap + ", " + item.KeteranganAdmin + ", " + item.KeteranganBilling + ", " + dbitem.ModifiedDate + ");";
            }
            if (dbitem.ReceivedDate == null)
            {
                dbitem.ReceivedDate = DateTime.Now;
                dbitem.Kelengkapan = dbitem.DokumenItem.Count() == dbitem.DokumenItem.Where(d => d.IsLengkap == true).Count() ? "Lengkap" : dbitem.DokumenItem.Where(d => d.IsLengkap == true).Count() == 0 ? "Tidak Ada" : "Tidak Lengkap";
            }
            Context.SalesOrderKontrakListSo sokls = null;
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(dbitem.IdSO.Value);
            if (dbso.SalesOrderKontrakId.HasValue)
                sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo));
            dbitem.SONumber = sokls == null ? dbso.SONumber : sokls.NoSo;
            dbitem.DataTruckId = sokls == null ? dbso.DataTruckId : sokls.IdDataTruck;
            dbitem.DriverId = sokls == null ? dbso.DriverId : sokls.Driver1Id;
            dbitem.TanggalMuat = sokls == null ? dbso.OrderTanggalMuat : sokls.MuatDate;
            dbso.UpdatedBy = UserPrincipal.id;

            if (btnSubmit == "Kirim"){
                dbitem.IsAdmin = false;
                dbso.Status = "billing";
                RepoSalesOrder.save(dbso, 16, dbitem.ListIdSo);
            }
            else if (btnSubmit == "Submit")
            {
                dbitem.IsAdmin = true;
                dbitem.Status = "Save";
                dbso.Status = "surat jalan";
                RepoSalesOrder.save(dbso, 17, dbitem.ListIdSo);
            }
            else if (btnSubmit == "Terima")
            {
                //create soship
                string sod_oid = Guid.NewGuid().ToString();
                string code = "";
                if (dbso.SalesOrderKontrakId.HasValue)
                {
                    sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo));
                    code = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo)).NoSo;
                }
                else
                    code = dbso.SONumber;
                string ship_guid = Guid.NewGuid().ToString();
                string guid;
                string sod_guid;
                dbitem.IsComplete = true;
                if (dbso.SalesOrderKontrakId == null || dbso.SalesOrderKontrakId != null && RepoDHKontrak.FindItemByPK(sokls.AdminUangJalan.DaftarHargaKontrakId.Value).DaftarHargaKontrak.LookUpTypeKontrak.Nama != "TYPE 2"){
                    SyncToERP(dbso, sod_oid, dbitem);
                    guid = Reposo_mstr.FindByPK(code).so_oid;
                    sod_guid = Reposo_mstr.FindSoDet(code).so_oid;
                    Reposo_mstr.saveSoShipMstr(dbso, UserPrincipal.username, guid, ship_guid, sokls);
                    Reposo_mstr.saveSoShipDet(dbso, UserPrincipal.username, ship_guid, sod_oid, sokls);
                }
                dbso.Status = "completed";
                RepoSalesOrder.save(dbso, 17, dbitem.ListIdSo);
            }

            RepoDokumen.save(dbitem, UserPrincipal.id, strQuery);
            if (dbitem.SalesOrder.SalesOrderKonsolidasiId.HasValue)
                return RedirectToAction("IndexKonsolidasi", new { caller = caller });
            else if (dbitem.ListIdSo != null && dbitem.ListIdSo != "" && caller != "billing")
                return RedirectToAction("IndexKontrak", new { caller = caller });
            else if (btnSubmit == "Terima")
                return RedirectToAction("Billing", new { caller = caller });
            else
                return RedirectToAction("Index", new { caller = caller });
        }

        [HttpPost]
        public ActionResult EditBilling(Dokumen model, string btnSubmit, string caller)
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(model.Id);
            DokumenItem[] resDok = JsonConvert.DeserializeObject<DokumenItem[]>(model.strDokumen);
            string strQuery = "";
            foreach (var item in resDok.Where(d => d.IsEdit))
            {
                item.SetDb(dbitem.DokumenItem.Where(d => d.Id == item.Id).FirstOrDefault());
                Context.DokumenItemHistory dih = new Context.DokumenItemHistory();
                dih.UserId = UserPrincipal.id;
                dbitem.DokumenItemHistory.Add(item.SetDbHistory(dih));
                strQuery += "INSERT INTO dbo.\"DokumenItemHistory\" (\"IdDok\", \"Nama\", \"Jml\", \"Warna\", \"Stempel\", \"Lengkap\", \"KeteranganAdmin\", \"KeteranganBilling\", \"ModifiedDate\") VALUES (" + dbitem.Id +
                    ", " + item.Nama + ", " + item.Jml + ", " + item.Warna + ", " + item.Stempel + ", " + item.Lengkap + ", " + item.KeteranganAdmin + ", " + item.KeteranganBilling + ", " + dbitem.ModifiedDate + ");";
            }
            if (dbitem.ReceivedDate == null)
            {
                dbitem.ReceivedDate = DateTime.Now;
                dbitem.Kelengkapan = dbitem.DokumenItem.Count() == dbitem.DokumenItem.Where(d => d.IsLengkap == true).Count() ? "Lengkap" : dbitem.DokumenItem.Where(d => d.IsLengkap == true).Count() == 0 ? "Tidak Ada" : "Tidak Lengkap";
            }
            Context.SalesOrderKontrakListSo sokls = null;
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(dbitem.IdSO.Value);
            if (dbso.SalesOrderKontrakId.HasValue)
                sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo));
            dbitem.SONumber = sokls == null ? dbso.SONumber : sokls.NoSo;
            dbitem.DataTruckId = sokls == null ? dbso.DataTruckId : sokls.IdDataTruck;
            dbitem.DriverId = sokls == null ? dbso.DriverId : sokls.Driver1Id;
            dbitem.TanggalMuat = sokls == null ? dbso.OrderTanggalMuat : sokls.MuatDate;
            dbso.UpdatedBy = UserPrincipal.id;
            if (btnSubmit == "Retur")
            {
                dbitem.IsAdmin = true;
                dbitem.IsReturn = true;
                dbso.Status = "surat jalan";
                RepoSalesOrder.save(dbso, 17, dbitem.ListIdSo);
            }
            else if (btnSubmit == "Submit")
            {
                dbitem.IsAdmin = false;
                dbso.Status = "billing";
                RepoSalesOrder.save(dbso, 17, dbitem.ListIdSo);
            }
            else if (btnSubmit == "Terima")
            {
                dbso.Status = "completed";
                string sod_oid = Guid.NewGuid().ToString();
                string code = "";
                if (dbso.SalesOrderKontrakId.HasValue)
                {
                    sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo));
                    code = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo)).NoSo;
                }
                else
                    code = dbso.SONumber;
                string ship_guid = Guid.NewGuid().ToString();
                string guid;
                string sod_guid;
                dbitem.IsComplete = true;
                Context.AdminUangJalan auj = null;
                if (sokls != null)
                    auj = sokls.AdminUangJalan == null ? RepoAUJ.FindAll().Where(d => d.SONumber != null && d.SONumber.Contains(sokls.NoSo) && d.Status != "Batal").FirstOrDefault() : sokls.AdminUangJalan;
                if (dbso.SalesOrderKontrakId == null || dbso.SalesOrderKontrakId != null && RepoDHKontrak.FindItemByPK(auj.DaftarHargaKontrakId.Value).DaftarHargaKontrak.LookUpTypeKontrak.Nama != "TYPE 2")
                {
                    SyncToERP(dbso, sod_oid, dbitem);
                    guid = Reposo_mstr.FindByPK(code).so_oid;
                    sod_guid = Reposo_mstr.FindSoDet(code).so_oid;
                    Reposo_mstr.saveSoShipMstr(dbso, UserPrincipal.username, guid, ship_guid, sokls);
                    Reposo_mstr.saveSoShipDet(dbso, UserPrincipal.username, ship_guid, sod_oid, sokls);
                }
                RepoSalesOrder.save(dbso, 17, dbitem.ListIdSo);
            }
            RepoDokumen.save(dbitem, UserPrincipal.id, strQuery);
            return RedirectToAction("Billing", new { caller = caller });
        }
        public void removeUnusedKontrak(string NoSo)
        {
            queryManual("DELETE FROM dbo.\"Dokumen\" WHERE \"SONumber\"='" + NoSo + "' AND \"Id\" NOT IN (SELECT \"DokumenId\" FROM dbo.\"SalesOrderKontrakListSo\" WHERE \"NoSo\"='" + NoSo + "')");
        }
        public void SyncToERP(Context.SalesOrder dbso, string sod_guid, Context.Dokumen dokumen)
        {
            string guid = Guid.NewGuid().ToString();
            if (dbso.SalesOrderKonsolidasiId != null)
            {
                Context.SalesOrderKonsolidasi sok = dbso.SalesOrderKonsolidasi;
                Context.SalesOrder so = RepoSalesOrder.FindByKonsolidasi(sok.SalesOrderKonsolidasiId);
                Reposo_mstr.saveSoMstr(so, UserPrincipal.username, guid, sok.NamaTagihanId.Value, sok.TotalHarga == null ? 0 : sok.TotalHarga.Value);
                Reposo_mstr.saveSoDet(so, UserPrincipal.username, guid, sod_guid, null, sok.TotalHarga == null ? 0 : sok.TotalHarga.Value);
            }
            else if (dbso.SalesOrderKontrakId == null)
            {
                decimal harga = RepoSalesOrder.Harga(dbso);
                Context.SalesOrderOncall dbitem = dbso.SalesOrderOncall;
                Reposo_mstr.saveSoMstr(dbso, UserPrincipal.username, guid, dbitem.CustomerId.Value, harga);
                Reposo_mstr.saveSoDet(dbso, UserPrincipal.username, guid, sod_guid);
            }
            else
            {
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dokumen.ListIdSo));
                decimal harga = RepoSalesOrder.Harga(dbso, sokls);
                Reposo_mstr.saveSoMstr(dbso, UserPrincipal.username, guid, dbso.SalesOrderKontrak.CustomerId.Value, harga, sokls);
                Reposo_mstr.saveSoDet(dbso, UserPrincipal.username, guid, sod_guid, sokls);
            }
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
            con.Open();
            using (DataTable dt = new DataTable())
            {
                var query = "UPDATE glt_det SET glt_ref_oid='" + guid + "' WHERE glt_code LIKE '%" + dbso.SONumber + "%' AND glt_daybook='TMS-SO'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Dispose();
                con.Close();
            }
        }

        public void generateStatusSuratJalanInSo()
        {
            foreach (Context.Dokumen dokumen in RepoDokumen.FindAll().Where(d => d.IsAdmin && d.SONumber.Contains("OC")))
            {
//                Context.SalesOrder
            }
        }

        public string generateSONumber() {
            foreach (Context.Dokumen dbitem in RepoDokumen.FindAll().Where(d => d.SONumber == null || d.DataTruckId == null || d.DriverId == null || d.TanggalMuat == null))
            {
                Context.SalesOrderKontrakListSo sokls = null;
                Context.SalesOrder dbso = dbitem.SalesOrder;
                if (dbitem.ListIdSo != null && dbitem.ListIdSo != "")
                    sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo));
                if (dbso != null)
                {
                    dbitem.SONumber = sokls == null ? dbso.SONumber : sokls.NoSo;
                    dbitem.DataTruckId = sokls == null ? dbso.DataTruckId : sokls.IdDataTruck;
                    dbitem.DriverId = sokls == null ? dbso.DriverId : sokls.Driver1Id;
                    dbitem.TanggalMuat = sokls == null ? dbso.OrderTanggalMuat : sokls.MuatDate;
                    RepoDokumen.save(dbitem, UserPrincipal.id);
                }
            }
            return "aa";
        }

        public ActionResult View(int id, string caller)
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(id);
            Dokumen model = new Dokumen(dbitem);
            ViewBag.caller = caller;
            if (caller == "admin")
                ViewBag.Title = "Dokumen Admin Surat Jalan";
            else
                ViewBag.Title = "Dokumen Billing";

            return View("View", model);
        }

        public ActionResult ShowPrint(int id)
        {
            Context.Dokumen dbitem = RepoDokumen.FindByPK(id);
            DokumenIndex model = new DokumenIndex(dbitem);
            if (dbitem.ListIdSo != "" && dbitem.ListIdSo != null)
            {
                Context.SalesOrderKontrakListSo sokls = RepoSalesOrderKontrakListSo.FindByPK(int.Parse(dbitem.ListIdSo));
                model.NoSo = sokls.NoSo;
                model.TanggalMuat = sokls.MuatDate;
                if (sokls.DataTruck != null)
                {
                    model.VehicleNo = sokls.DataTruck.VehicleNo;
                    model.JnsTruck = sokls.DataTruck.JenisTrucks.StrJenisTruck;
                }
                if (sokls.Driver1 != null)
                    model.NamaDriver = sokls.Driver1.NamaDriver;
            }
            return View("Print", model);
        }

        public FileContentResult ExportBilling(string NoSo)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Dokumen> dbitems = RepoDokumen.FindAllBilling(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            //bikin worksheet worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Status";
            ws.Cells[1, 2].Value = "SO No";
            ws.Cells[1, 3].Value = "Tanggal Terima";
            ws.Cells[1, 4].Value = "Vehicle No";
            ws.Cells[1, 5].Value = "Jenis Truk";
            ws.Cells[1, 6].Value = "Customer";
            ws.Cells[1, 7].Value = "Rute";
            ws.Cells[1, 8].Value = "Nama Driver";
            ws.Cells[1, 9].Value = "Tanggal Muat";
            ws.Cells[1, 10].Value = "Delay";
            ws.Cells[1, 11].Value = "Lengkap?";
            ws.Cells[1, 12].Value = "Last Update";
            ws.Cells[1, 13].Value = "Keterangan";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                DokumenIndex item = new DokumenIndex(dbitems[i]);
                ws.Cells[i + 2, 1].Value = item.Status;
                ws.Cells[i + 2, 2].Value = item.NoSo;
                ws.Cells[i + 2, 3].Value = item.ReceivedDate.ToString();
                ws.Cells[i + 2, 4].Value = item.VehicleNo;
                ws.Cells[i + 2, 5].Value = item.JnsTruck;
                ws.Cells[i + 2, 6].Value = item.Customer;
                ws.Cells[i + 2, 7].Value = item.Rute;
                ws.Cells[i + 2, 8].Value = item.NamaDriver;
                ws.Cells[i + 2, 9].Value = item.TanggalMuat.ToString();
                ws.Cells[i + 2, 10].Value = item.Delay;
                ws.Cells[i + 2, 11].Value = item.Lengkap;
                ws.Cells[i + 2, 12].Value = item.LastUpdate.ToString();
                ws.Cells[i + 2, 13].Value = item.Keterangan;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Billing.xls";

            return fsr;
        }

        public FileContentResult Export(string NoSo)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Dokumen> dbitems = RepoDokumen.FindAllOnCallSJ(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            //bikin worksheet worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Status";
            ws.Cells[1, 2].Value = "SO No";
            ws.Cells[1, 3].Value = "Tanggal Terima";
            ws.Cells[1, 4].Value = "Vehicle No";
            ws.Cells[1, 5].Value = "Jenis Truk";
            ws.Cells[1, 6].Value = "Customer";
            ws.Cells[1, 7].Value = "Rute";
            ws.Cells[1, 8].Value = "Nama Driver";
            ws.Cells[1, 9].Value = "Tanggal Muat";
            ws.Cells[1, 10].Value = "Delay";
            ws.Cells[1, 11].Value = "Lengkap?";
            ws.Cells[1, 12].Value = "Last Update";
            ws.Cells[1, 13].Value = "Keterangan";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                DokumenIndex item = new DokumenIndex(dbitems[i]);
                ws.Cells[i + 2, 1].Value = item.Status;
                ws.Cells[i + 2, 2].Value = item.NoSo;
                ws.Cells[i + 2, 3].Value = item.ReceivedDate.ToString();
                ws.Cells[i + 2, 4].Value = item.VehicleNo;
                ws.Cells[i + 2, 5].Value = item.JnsTruck;
                ws.Cells[i + 2, 6].Value = item.Customer;
                ws.Cells[i + 2, 7].Value = item.Rute;
                ws.Cells[i + 2, 8].Value = item.NamaDriver;
                ws.Cells[i + 2, 9].Value = item.TanggalMuat.ToString();
                ws.Cells[i + 2, 10].Value = item.Delay;
                ws.Cells[i + 2, 11].Value = item.Lengkap;
                ws.Cells[i + 2, 12].Value = item.LastUpdate.ToString();
                ws.Cells[i + 2, 13].Value = item.Keterangan;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Surat Jalan.xls";

            return fsr;
        }

        public FileContentResult ExportKonsolidasi(string NoSo)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Dokumen> dbitems = RepoDokumen.FindAllKonsolidasiSJ(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            //bikin worksheet worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Status";
            ws.Cells[1, 2].Value = "SO No";
            ws.Cells[1, 3].Value = "Tanggal Terima";
            ws.Cells[1, 4].Value = "Vehicle No";
            ws.Cells[1, 5].Value = "Jenis Truk";
            ws.Cells[1, 6].Value = "Customer";
            ws.Cells[1, 7].Value = "Rute";
            ws.Cells[1, 8].Value = "Nama Driver";
            ws.Cells[1, 9].Value = "Tanggal Muat";
            ws.Cells[1, 10].Value = "Delay";
            ws.Cells[1, 11].Value = "Lengkap?";
            ws.Cells[1, 12].Value = "Last Update";
            ws.Cells[1, 13].Value = "Keterangan";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                DokumenIndex item = new DokumenIndex(dbitems[i]);
                ws.Cells[i + 2, 1].Value = item.Status;
                ws.Cells[i + 2, 2].Value = item.NoSo;
                ws.Cells[i + 2, 3].Value = item.ReceivedDate.ToString();
                ws.Cells[i + 2, 4].Value = item.VehicleNo;
                ws.Cells[i + 2, 5].Value = item.JnsTruck;
                ws.Cells[i + 2, 6].Value = item.Customer;
                ws.Cells[i + 2, 7].Value = item.Rute;
                ws.Cells[i + 2, 8].Value = item.NamaDriver;
                ws.Cells[i + 2, 9].Value = item.TanggalMuat.ToString();
                ws.Cells[i + 2, 10].Value = item.Delay;
                ws.Cells[i + 2, 11].Value = item.Lengkap;
                ws.Cells[i + 2, 12].Value = item.LastUpdate.ToString();
                ws.Cells[i + 2, 13].Value = item.Keterangan;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Surat Jalan.xls";

            return fsr;
        }

        public FileContentResult ExportKontrak(string NoSo)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            //            List<Context.Dokumen> dbitems = RepoDokumen.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).Where(d => d.IsComplete != true).ToList();
            List<Context.SalesOrderKontrakListSo> dbitems = RepoDokumen.FindAllKontrakSJ(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            //bikin worksheet worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Status";
            ws.Cells[1, 2].Value = "SO No";
            ws.Cells[1, 3].Value = "Vehicle No";
            ws.Cells[1, 4].Value = "Customer";
            ws.Cells[1, 5].Value = "Rute";
            ws.Cells[1, 6].Value = "Nama Driver";
            ws.Cells[1, 7].Value = "Tanggal Muat";
            ws.Cells[1, 8].Value = "Delay";
            ws.Cells[1, 9].Value = "Lengkap?";
            ws.Cells[1, 10].Value = "Last Update";
            ws.Cells[1, 11].Value = "Keterangan";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                DokumenIndex item = new DokumenIndex(dbitems[i].Dokumen);
                ws.Cells[i + 2, 1].Value = item.Status;
                ws.Cells[i + 2, 2].Value = dbitems[i].NoSo;
                ws.Cells[i + 2, 3].Value = dbitems[i].DataTruck == null ? "" : dbitems[i].DataTruck.VehicleNo;
                ws.Cells[i + 2, 4].Value = item.Customer;
                ws.Cells[i + 2, 5].Value = item.Rute;
                ws.Cells[i + 2, 6].Value = dbitems[i].Driver1 == null ? "" : dbitems[i].Driver1.NamaDriver;
                ws.Cells[i + 2, 7].Value = dbitems[i].MuatDate.ToString();
                ws.Cells[i + 2, 8].Value = item.Delay;
                ws.Cells[i + 2, 9].Value = item.Lengkap;
                ws.Cells[i + 2, 10].Value = item.LastUpdate.ToString();
                ws.Cells[i + 2, 11].Value = item.Keterangan;
            }
            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Surat Jalan.xls";
            return fsr;
        }
    }
}