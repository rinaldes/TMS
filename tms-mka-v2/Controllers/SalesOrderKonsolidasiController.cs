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
    public class SalesOrderKonsolidasiController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder; private IDaftarHargaKonsolidasiRepo RepoDHKonsolidasi; private ICustomerRepo RepoCustomer; private ILookupCodeRepo RepoLookupCode;
        public SalesOrderKonsolidasiController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IDaftarHargaKonsolidasiRepo repoDHKonsolidasi,
            ICustomerRepo repoCustomer, ILookupCodeRepo repoLookupCode)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoDHKonsolidasi = repoDHKonsolidasi;
            RepoCustomer = repoCustomer;
            RepoLookupCode = repoLookupCode;
        }
        [MyAuthorize(Menu = "Sales Order Konsolidasi Daftar Barang", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "SalesOrderKonsolidasi").ToList();
            return View();
        }

        public string FindById(int id)
        {
            Context.SalesOrder item = RepoSalesOrder.FindByKonsolidasi(id);
            SalesOrderKonsolidasi data = new SalesOrderKonsolidasi(item);
            data.KodeNama = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.SupplierId.Value).CustomerCodeOld;
            data.SupplierName = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.SupplierId.Value).CustomerNama;
            data.NamaCustomer = RepoLookup.FindByPK(data.CustomerId).Nama;
            data.KodeCustomerTagihan = RepoCustomer.FindByPK(data.CustomerTagihanId.Value).CustomerCodeOld;
            data.IsOld = true;
            return new JavaScriptSerializer().Serialize(data);
        }

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKonsolidasi();
            List<SalesOrderKonsolidasi> ListModel = new List<SalesOrderKonsolidasi>();
            foreach (Context.SalesOrder item in items)
            {
                SalesOrderKonsolidasi newItem = (new SalesOrderKonsolidasi(item));
                newItem.NamaCustomer = RepoLookupCode.FindByPK(newItem.CustomerId).Nama;
                if (RepoCustomer.FindByPK(newItem.SupplierId.Value) != null)
                {
                    newItem.KodeNama = RepoCustomer.FindByPK(newItem.SupplierId.Value).CustomerCodeOld;
                    newItem.SupplierName = RepoCustomer.FindByPK(newItem.SupplierId.Value).CustomerNama;
                }
                ListModel.Add(newItem);
            }

            int total = RepoSalesOrder.CountKonsolidasi(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        [MyAuthorize(Menu = "Sales Order Konsolidasi Daftar Barang", Action="create")]
        public ActionResult Add()
        {
            SalesOrderKonsolidasi model = new SalesOrderKonsolidasi();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(SalesOrderKonsolidasi model, string btnsave)
        {
//            if (ModelState.IsValid)
  //          {
                //cek min max
                bool isPalid = true;
                Context.DaftarHargaKonsolidasi dbDh = RepoDHKonsolidasi.FindByItemId(model.RuteId.Value);
                Context.DaftarHargaKonsolidasiItem dbDhItem = dbDh.DaftarHargaKonsolidasiItem.Where(d => d.Id == model.RuteId.Value).FirstOrDefault();
                if(!isPalid)
                    return View("Form", model);

                Context.SalesOrder dbso = new Context.SalesOrder();
                Context.SalesOrderKonsolidasi dbitem = new Context.SalesOrderKonsolidasi();
                model.setDb(dbitem);
                dbitem.Urutan = RepoSalesOrder.getUrutanKonsolidasi(model.TanggalMasuk.Value) + 1;
                dbitem.SONumber = RepoSalesOrder.generateKonsolidasi(model.TanggalMasuk.Value, dbitem.Urutan);
                dbitem.DN = "DN" + dbitem.SONumber;
                dbso.SalesOrderKonsolidasi = dbitem;

                if (btnsave == "save")
                    dbso.Status = "draft planning";
                else if (btnsave == "draft")
                    dbso.Status = "draft";
                else
                    dbso.Status = model.Status;

                RepoSalesOrder.save(dbso);

                return RedirectToAction("Index");
    //        }
      //      return View("Form", model);
        }
        [MyAuthorize(Menu = "Sales Order Konsolidasi Daftar Barang", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            SalesOrderKonsolidasi model = new SalesOrderKonsolidasi(dbitem);
            model.NamaCustomer = RepoLookupCode.FindByPK(model.CustomerId).Nama;
            model.KodeNama = RepoCustomer.FindByPK(model.SupplierId.Value).CustomerCodeOld;
            model.SupplierName = RepoCustomer.FindByPK(model.SupplierId.Value).CustomerNama;
            Context.DaftarHargaKonsolidasiItem dhoitem = RepoDHKonsolidasi.FindItemByPK(model.RuteId.Value);
            ViewBag.ListNamaRute = dhoitem.ListNamaRute;

            ViewBag.name = model.SONumber;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(SalesOrderKonsolidasi model, string btnsave)
        {
    //        if (ModelState.IsValid)
      //      {
                //cek min max
            /*    bool isPalid = true;
                Context.DaftarHargaKonsolidasi dbDh = RepoDHKonsolidasi.FindByItemId(model.RuteId.Value);
                Context.DaftarHargaKonsolidasiItem dbDhItem = dbDh.DaftarHargaKonsolidasiItem.Where(d => d.Id == model.RuteId.Value).FirstOrDefault();
                if (
                    (dbDhItem.LookupCodeSatuan.Nama == model.PerhitunganDasar) || (model.PerhitunganDasar == "Tonase" && (dbDhItem.LookupCodeSatuan.Nama == "Ton" || dbDhItem.LookupCodeSatuan.Nama == "Kg"))
                )
                {
                    if (model.PerhitunganDasar == "Tonase")
                    {
                        if (!((model.Tonase >= dbDhItem.MinKg) && (model.Tonase <= dbDhItem.MaxKg)))
                        {
                            isPalid = false;
                            ModelState.AddModelError("Tonase", "Nilai harus lebih besar dari " + dbDhItem.MinKg.ToString() + " dan lebih kecil dari " + dbDhItem.MaxKg.ToString());
                        }
                    }
                    else if (model.PerhitunganDasar == "Karton")
                    {
                        if (!((model.karton >= dbDhItem.MinKg) && (model.karton <= dbDhItem.MaxKg)))
                        {
                            isPalid = false;
                            ModelState.AddModelError("karton", "Nilai harus lebih besar dari " + dbDhItem.MinKg.ToString() + " dan lebih kecil dari " + dbDhItem.MaxKg.ToString());
                        }
                    }
                    else if (model.PerhitunganDasar == "Pallet")
                    {
                        if (!((model.Pallet >= dbDhItem.MinKg) && (model.Pallet <= dbDhItem.MaxKg)))
                        {
                            isPalid = false;
                            ModelState.AddModelError("Pallet", "Nilai harus lebih besar dari " + dbDhItem.MinKg.ToString() + " dan lebih kecil dari " + dbDhItem.MaxKg.ToString());
                        }
                    }
                    else if (model.PerhitunganDasar == "Container")
                    {
                        if (!((model.Container >= dbDhItem.MinKg) && (model.Container <= dbDhItem.MaxKg)))
                        {
                            isPalid = false;
                            ModelState.AddModelError("Container", "Nilai harus lebih besar dari " + dbDhItem.MinKg.ToString() + " dan lebih kecil dari " + dbDhItem.MaxKg.ToString());
                        }
                    }
                    else if (model.PerhitunganDasar == "m3")
                    {
                        if (!((model.m3 >= dbDhItem.MinKg) && (model.m3 <= dbDhItem.MaxKg)))
                        {
                            isPalid = false;
                            ModelState.AddModelError("m3", "Nilai harus lebih besar dari " + dbDhItem.MinKg.ToString() + " dan lebih kecil dari " + dbDhItem.MaxKg.ToString());
                        }
                    }
                }
                if (!isPalid)
                    return View("Form", model);
                */
                Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.SalesOrderId.Value);

                model.setDb(dbitem.SalesOrderKonsolidasi);

                if (btnsave == "save")
                    dbitem.Status = "save";
                else if (btnsave == "draft")
                    dbitem.Status = "draft";
                else
                    dbitem.Status = model.Status;

                RepoSalesOrder.save(dbitem);


                return RedirectToAction("Index");
 //           }
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
            dbItem.Status = "save";
            RepoSalesOrder.save(dbItem);

            return Json(response);
        }
        public string GetDataForProses()
        {
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllKonsolidasi();
            List<SalesOrderKonsolidasi> ListModel = new List<SalesOrderKonsolidasi>();
            foreach (Context.SalesOrder item in items.Where(d => d.SalesOrderKonsolidasi.IsSelect == false && d.Status == "save"))
            {
                SalesOrderKonsolidasi model = new SalesOrderKonsolidasi(item, RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.SupplierId.Value));
                model.KodeNama = RepoCustomer.FindByPK(item.SalesOrderKonsolidasi.SupplierId.Value).CustomerCodeOld;
                model.NamaCustomer = RepoLookupCode.FindByPK(model.CustomerId).Nama;
                model.KodeCustomerTagihan = RepoCustomer.FindByPK(model.CustomerTagihanId.Value).CustomerCodeOld;
                model.SONumber = item.SONumber;
                model.IsOld = false;
                ListModel.Add(model);
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }

        #region inputDp
        public string BindingDp(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.SalesOrder items = RepoSalesOrder.FindByKonsolidasi(id);

            return new JavaScriptSerializer().Serialize(new
            {
                data = items.SalesOrderKonsolidasi.SalesOrderKonsolidasiDp.ToList().Select(d => new
                {
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
            SalesOrderKonsolidasi model = new SalesOrderKonsolidasi(dbitem);

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
            //    return View("SalesOrderKonsolidasi/FormReadOnly", model);
            //}
            TempData.Remove("errorMsgListOrder");
            return View("SalesOrderKonsolidasi/FormReadOnly", model);

        }

        [HttpPost]
        public JsonResult SaveDp(Context.SalesOrderKonsolidasiDp model)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKonsolidasi(model.SalesOrderKonsolidasiId);
            if (model.Id == 0)
                dbitem.SalesOrderKonsolidasi.SalesOrderKonsolidasiDp.Add(model);
            else
            {
                Context.SalesOrderKonsolidasiDp dbdp = dbitem.SalesOrderKonsolidasi.SalesOrderKonsolidasiDp.Where(d => d.Id == model.Id).FirstOrDefault();
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
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKonsolidasi(IdSo);
            Context.SalesOrderKonsolidasiDp dbdp = dbitem.SalesOrderKonsolidasi.SalesOrderKonsolidasiDp.Where(d => d.Id == id).FirstOrDefault();
            dbitem.SalesOrderKonsolidasi.SalesOrderKonsolidasiDp.Remove(dbdp);
            ResponeModel response = new ResponeModel(true);

            RepoSalesOrder.save(dbitem);

            return Json(response);
        }
       #endregion inputDp
    }
}