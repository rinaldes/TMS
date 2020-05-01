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
    public class SalesOrderKontrakController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IDaftarHargaKontrakRepo RepoDaftarHarga;
        private IJenisTruckRepo RepoJenisTruck;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private IAuditrailRepo RepoAuditrail;
        private IDataTruckRepo RepoDataTruck;
        public SalesOrderKontrakController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IDaftarHargaKontrakRepo repoDaftarHarga,
            IJenisTruckRepo repoJenisTruck, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo, IDataTruckRepo repoDataTruck, IAuditrailRepo repoAuditrail)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoDaftarHarga = repoDaftarHarga;
            RepoJenisTruck = repoJenisTruck;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
            RepoDataTruck = repoDataTruck;
            RepoAuditrail = repoAuditrail;
        }
        [MyAuthorize(Menu = "Sales Order Kontrak", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SalesOrderKontrak").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            //List<Context.SalesOrder> items = RepoSalesOrder.FindAllKontrak(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKontrak().Where(d => d.Status == "draft").ToList();
            List<SalesOrderKontrak> ListModel = new List<SalesOrderKontrak>();
            foreach (Context.SalesOrder item in items)
            {
                ListModel.Add(new SalesOrderKontrak(item));
            }

            int total = RepoSalesOrder.CountKontrak(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string generateDokumenId()
        {
            List<Context.SalesOrderKontrakListSo> items = RepoSalesOrderKontrakListSo.FindAll().ToList();

            
            foreach (var item in items)
            {
                RepoSalesOrderKontrakListSo.save(item);
            }
            return "aa";
        }

        public string GetItemTruck(int idSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(idSo);
            List<SalesOrderKontrakItemTruck> model = new List<SalesOrderKontrakItemTruck>();

            foreach (Context.SalesOrderKontrakTruck item in dbitem.SalesOrderKontrak.SalesOrderKontrakTruck)
            {
                model.Add(new SalesOrderKontrakItemTruck(item));
            }

            return new JavaScriptSerializer().Serialize(model);
        }

        public string GetTruckImport(int idSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(idSo);
            List<DataTruckDetail> model = new List<DataTruckDetail>();

            foreach (Context.SalesOrderKontrakTruck item in dbitem.SalesOrderKontrak.SalesOrderKontrakTruck)
            {
                model.Add(new DataTruckDetail(item.DataTruck));
            }

            return new JavaScriptSerializer().Serialize(model);
        }

        public string GetItemTruckKonfirmasi(int idSo)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(idSo);
            List<SalesOrderKontrakItemTruck> model = new List<SalesOrderKontrakItemTruck>();
            List<Context.SalesOrder> dbso = RepoSalesOrder.FindAll().Where(s => s.Id != idSo).ToList();

            foreach (Context.DataTruck item in RepoDataTruck.FindAll().Where(d => d.IdJenisTruck == dbitem.SalesOrderKontrak.JenisTruckId).ToList())
            {
                SalesOrderKontrakItemTruck sokit = new SalesOrderKontrakItemTruck(item, dbso);
                model.Add(sokit);
            }

            return new JavaScriptSerializer().Serialize(new { total = model.Count(), data = model });
        }

        [MyAuthorize(Menu = "Sales Order Kontrak", Action="create")]
        public ActionResult Add()
        {
            SalesOrderKontrak model = new SalesOrderKontrak();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(SalesOrderKontrak model, string btnsave)
        {
            bool palid = true;//validasi tambahan
            List<Context.DaftarHargaKontrak> dummy = RepoDaftarHarga.FindAll().Where(d =>
                d.IdCust == model.CustomerId &&
                d.DaftarHargaKontrakItem.Any(i => i.IdJenisTruck == model.JenisTruckId)).ToList();
            if (dummy.Count() > 0 && dummy.FirstOrDefault().LookUpTypeKontrak.Nama == "TYPE 1" && (model.Kerja < 1))
            {
                palid = false;
                ViewBag.errorMsg = "Hari kerja harus lebih dari atau sama dengan satu untuk jenis kontrak tipe 1";
            }
            if (btnsave == null && model.Status == "save")
            {
                if (dummy.Count() < 1)
                {
                    palid = false;
                    ViewBag.errorMsg = "Tidak terdapat daftar harga untuk jenis truck " + RepoJenisTruck.FindByPK(model.JenisTruckId.Value).StrJenisTruck;
                }
                else if (dummy.FirstOrDefault().LookUpTypeKontrak.Nama == "TYPE 1" && (model.Kerja < 1))
                {
                    palid = false;
                    ViewBag.errorMsg = "Hari kerja harus lebih dari atau sama dengan satu untuk jenis kontrak tipe 1";
                }
                else
                {
                    if (model.JsonDateMuat != null && model.JsonDateMuat != "")
                    {
                        List<string> wadah = new List<string>();
                        foreach (string item in model.JsonDateMuat.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            DateTime valDate = DateTime.Parse(item);

                            if (!dummy.Any(d => valDate >= d.PeriodStart && valDate <= d.PeriodEnd))
                            {
                                wadah.Add(item);
                            }
                        }
                        if (wadah.Count() > 0)
                        {
                            palid = false;
                            ViewBag.errorMsg = "Tidak terdapat daftar harga untuk jenis truck " + RepoJenisTruck.FindByPK(model.JenisTruckId.Value).StrJenisTruck +
                                    " pada tanggal " + string.Join(", ", wadah);
                        }
                    }
                    else
                    {
                        palid = false;
                        ViewBag.errorMsg = "Harap pilih tanggal muat.";
                    }
                }
            }
            else
            {
                if (model.JsonDateMuat == null || model.JsonDateMuat == "")
                {
                    palid = false;
                    ViewBag.errorMsg = "Harap pilih tanggal muat.";
                }
            }
            if (palid)
            {
                if (ModelState.IsValid)
                {
                    if (model.existingMuatDate != "" && model.existingMuatDate != null)
                    {
                        model.existingMuatDate = RepoSalesOrder.IsMuatDateExist(model.JsonDateMuat.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList(), model.CustomerId.Value);
                        if (model.existingMuatDate != "")
                            return View("Form", model);
                    }
                    Context.SalesOrder dbso = new Context.SalesOrder();
                    Context.SalesOrderKontrak dbitem = new Context.SalesOrderKontrak();
                    model.setDb(dbitem);
                    dbitem.DocDate = DateTime.Now;
                    dbitem.Urutan = RepoSalesOrder.getUrutanKontrak(dbitem.PeriodStr.AddDays(1)) + 1;
                    dbitem.SONumber = RepoSalesOrder.generateCodeKontrak(dbitem.Urutan, dbitem.PeriodStr);
                    dbitem.DN = "DN" + dbitem.SONumber;
                    dbso.SalesOrderKontrak = dbitem;
                    dbso.CreatedBy = UserPrincipal.id;
                    dbso.UpdatedBy = UserPrincipal.id;
                    if (btnsave == "save")
                        dbso.Status = "save";
                    else if (btnsave == "draft")
                        dbso.Status = "draft";
                    else
                        dbso.Status = model.Status;
                    if (RepoSalesOrder.FindByKontrakCode(dbitem.SONumber) == null)
                    {
                        RepoSalesOrder.save(dbso);
                        Context.SalesOrder so = RepoSalesOrder.FindByKontrakCode(dbitem.SONumber);
                        RepoAuditrail.saveOrderHistory(so);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.errorMsg = "SO number " + dbitem.SONumber + " is exist. Please try again";
                        return View("Form", model);
                    }
                }
            }
            return View("Form", model);
        }

        [MyAuthorize(Menu = "Sales Order Kontrak", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderKontrak model = new SalesOrderKontrak(dbitem);

            ViewBag.name = model.SONumber;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(SalesOrderKontrak model, string btnsave)
        {
            //validasi tambahan
            bool palid = true;
            if (btnsave == null && model.Status == "save")
            {
                List<Context.DaftarHargaKontrak> dummy = RepoDaftarHarga.FindAll().Where(d =>
                    d.IdCust == model.CustomerId &&
                    d.DaftarHargaKontrakItem.Any(i => i.IdJenisTruck == model.JenisTruckId)).ToList();

                if (dummy.Count() < 1)
                {
                    palid = false;
                    ViewBag.errorMsg = "Tidak terdapat daftar harga untuk jenis truck " + RepoJenisTruck.FindByPK(model.JenisTruckId.Value).StrJenisTruck;
                }
                else
                {
                    if (model.JsonDateMuat != null && model.JsonDateMuat != "")
                    {
                        List<string> wadah = new List<string>();
                        foreach (string item in model.JsonDateMuat.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            DateTime valDate = DateTime.Parse(item);

                            if (!dummy.Any(d => valDate >= d.PeriodStart && valDate <= d.PeriodEnd))
                            {
                                wadah.Add(item);
                            }
                        }
                        if (wadah.Count() > 0)
                        {
                            palid = false;
                            ViewBag.errorMsg = "Tidak terdapat daftar harga untuk jenis truck " + RepoJenisTruck.FindByPK(model.JenisTruckId.Value).StrJenisTruck +
                                    " pada tanggal " + string.Join(", ", wadah);
                        }
                    }
                    else
                    {
                        palid = false;
                        ViewBag.errorMsg = "Harap pilih tanggal muat.";
                    }
                }
            }
            else
            {
                if (model.JsonDateMuat == null || model.JsonDateMuat == "")
                {
                    palid = false;
                    ViewBag.errorMsg = "Harap pilih tanggal muat.";
                }
            }

            if (palid)
            {
                if (ModelState.IsValid)
                {
                    if (model.existingMuatDate != "" && model.existingMuatDate != null)
                    {
                        model.existingMuatDate = RepoSalesOrder.IsMuatDateExist(model.JsonDateMuat.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList(), model.CustomerId.Value);
                        if (model.existingMuatDate != "")
                        {
                            return View("Form", model);
                        }
                    }

                    Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.SalesOrderId.Value);
                    
                    model.setDb(dbitem.SalesOrderKontrak);

                    if (btnsave == "save")
                        dbitem.Status = "save";
                    else if (btnsave == "draft")
                        dbitem.Status = "draft";
                    else
                        dbitem.Status = model.Status;
                    dbitem.isReturn = false;
                    RepoSalesOrder.save(dbitem);

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

            //RepoSalesOrderKontrakListSo.deleteAdd(dbItem.SalesOrderKontrakId);
            RepoSalesOrder.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        [HttpPost]
        public JsonResult Submit(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SalesOrder dbItem = RepoSalesOrder.FindByPK(id);

            List<Context.DaftarHargaKontrak> dummy = RepoDaftarHarga.FindAll().Where(d =>
                    d.IdCust == dbItem.SalesOrderKontrak.CustomerId &&
                    d.DaftarHargaKontrakItem.Any(i => i.IdJenisTruck == dbItem.SalesOrderKontrak.JenisTruckId)).ToList();

            if (dummy.Count() < 1)
            {
                response.Success = false;
                response.Message = "Tidak terdapat daftar harga untuk jenis truck " + dbItem.SalesOrderKontrak.JenisTrucks.StrJenisTruck;
            }
            else
            {
                List<string> wadah = new List<string>();
                foreach (Context.SalesOrderKontrakDetail item in dbItem.SalesOrderKontrak.SalesOrderKontrakDetail)
                {
                    if (!dummy.Any(d => item.MuatDate >= d.PeriodStart && item.MuatDate <= d.PeriodEnd))
                    {
                        wadah.Add(item.MuatDate.ToShortDateString());
                    }
                }
                if (wadah.Count() > 0)
                {
                    response.Success = false;
                    response.Message = "Tidak terdapat daftar harga untuk jenis truck " + dbItem.SalesOrderKontrak.JenisTrucks.StrJenisTruck +
                            " pada tanggal " + string.Join(", ", wadah);
                }
                else
                {
                    dbItem.Status = "save";
                    dbItem.isReturn = false;
                    RepoSalesOrder.save(dbItem);
                }
            }

            return Json(response);
        }

        public string getDetailByRange(int Id, string StrDate, string EndDate)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(Id);
            DateTime strDate = DateTime.Parse(StrDate);
            DateTime endDate = DateTime.Parse(EndDate);

            List<Context.SalesOrderKontrakListSo> listDetail = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IsProses == false && (d.MuatDate >= strDate && d.MuatDate <= endDate) && d.IdDataTruck != null && (d.Status == "draft konfirmasi" || d.Status == null)).ToList();
            listDetail = listDetail.OrderBy(d => d.MuatDate).ToList();
 
            return new JavaScriptSerializer().Serialize(listDetail.Select(d => new
            {
                Id = d.Id,
                NoSo = d.NoSo,
                TaggalMuat = d.MuatDate.ToShortDateString(),
                IdTruk = d.IdDataTruck,
                VehicleNumber = d.DataTruck == null ? "" : d.DataTruck.VehicleNo,
                JenisTruck = d.DataTruck == null ? "" : d.DataTruck.JenisTrucks.StrJenisTruck,
                IdDriver1 = d.Driver1Id,
                KodeDriver1 = d.Driver1Id.HasValue ? d.Driver1.KodeDriver : "",
                NamaDriver1 = d.Driver1Id.HasValue ? d.Driver1.NamaDriver : "",
                IdDriver2 = d.Driver2Id,
                KodeDriver2 = d.Driver2Id.HasValue ? d.Driver2.KodeDriver : "",
                NamaDriver2 = d.Driver2Id.HasValue ? d.Driver2.NamaDriver : "",
            }));
        }

        public string Import(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(id);

            List<SalesOrderKontrakItemTruck> model = new List<SalesOrderKontrakItemTruck>();

            foreach (Context.SalesOrderKontrakTruck item in dbitem.SalesOrderKontrak.SalesOrderKontrakTruck)
            {
                if (item.StatusTruk == "Available")
                    model.Add(new SalesOrderKontrakItemTruck(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = model.Count(), data = model });
        }
        #region inputDp
        public string BindingDp(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.SalesOrder items = RepoSalesOrder.FindByKontrak(id);

            return new JavaScriptSerializer().Serialize(new
            {
                data = items.SalesOrderKontrak.SalesOrderKontrakDp.ToList().Select(d => new {
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
            SalesOrderKontrak model = new SalesOrderKontrak(dbitem);

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
            //    return View("SalesOrderKontrak/FormReadOnly", model);
            //}
            TempData.Remove("errorMsgListOrder");
            return View("SalesOrderKontrak/FormReadOnly", model);

        }

        [HttpPost]
        public JsonResult SaveDp(Context.SalesOrderKontrakDp model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(model.SalesOrderKontrakId);
            if (model.Id == 0)
                dbitem.SalesOrderKontrak.SalesOrderKontrakDp.Add(model);
            else
            {
                Context.SalesOrderKontrakDp dbdp = dbitem.SalesOrderKontrak.SalesOrderKontrakDp.Where(d => d.Id == model.Id).FirstOrDefault();
                dbdp.Tanggal = model.Tanggal;
                dbdp.Penerima = model.Penerima;
                dbdp.RekeningId = model.RekeningId;
                dbdp.Jenis = model.Jenis;
                dbdp.Jumlah = model.Jumlah;
            }

            RepoSalesOrder.save(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        public JsonResult DeleteDp(int IdSo, int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(IdSo);
            Context.SalesOrderKontrakDp dbdp = dbitem.SalesOrderKontrak.SalesOrderKontrakDp.Where(d => d.Id == id).FirstOrDefault();
            dbitem.SalesOrderKontrak.SalesOrderKontrakDp.Remove(dbdp);
            ResponeModel response = new ResponeModel(true);

            RepoSalesOrder.save(dbitem);

            return Json(response);
        }
        #endregion inputDp

    }
}