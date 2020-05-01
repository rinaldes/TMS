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
    public class BatalOrderController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IBatalOrderRepo RepoBatalOrder;
        private ISettlementBatalRepo RepoSettBatal;
        private IAtmRepo Repoatm;
        private IDataBoronganRepo RepoBor;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private IAuditrailRepo RepoAuditrail;
        private Ipbyd_detRepo Repopbyd_det;

        public BatalOrderController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IBatalOrderRepo repoBatalOrder, ISettlementBatalRepo repoSettBatal,
            IAtmRepo repoatm, IDataBoronganRepo repoBor, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr, IAuditrailRepo repoAuditrail, Ipbyd_detRepo repopbyd_det)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoBatalOrder = repoBatalOrder;
            RepoSettBatal = repoSettBatal;
            Repoatm = repoatm;
            RepoBor = repoBor;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            RepoAuditrail = repoAuditrail;
            Repopbyd_det = repopbyd_det;
        }

        [MyAuthorize(Menu = "Batal Order", Action = "create")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            AdminUangJalan model = new AdminUangJalan();
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                model.ModelOncall = new SalesOrderOncall(dbitem);
                model.StatusSo = model.ModelOncall.Status;
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelOncall.Driver1Id;
                    model.NamaDriver1 = model.ModelOncall.KodeDriver1 + " - " + model.ModelOncall.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelOncall.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelOncall.Driver2Id;
                    model.NamaDriver2 = model.ModelOncall.KodeDriver2 + " - " + model.ModelOncall.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelOncall.KeteranganDriver2;
                }
                ViewBag.Title = "Batal Order " + dbitem.SalesOrderOncall.SONumber;
                //ViewBag.postData = "EditOncall";
                return View("Form", model);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                model.ModelPickup = new SalesOrderPickup(dbitem);
                model.StatusSo = model.ModelPickup.Status;
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelPickup.Driver1Id;
                    model.NamaDriver1 = model.ModelPickup.KodeDriver1 + " - " + model.ModelPickup.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelPickup.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelPickup.Driver2Id;
                    model.NamaDriver2 = model.ModelPickup.KodeDriver2 + " - " + model.ModelPickup.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelPickup.KeteranganDriver2;
                }
                ViewBag.Title = "Batal Order " + dbitem.SalesOrderPickup.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("Form", model);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                model.StatusSo = model.ModelKonsolidasi.Status;
                if (model.IdDriver1.HasValue)
                {
                    model.IdDriver1 = model.IdDriver1;
                    model.NamaDriver1 = model.NamaDriver1;
                    model.KeteranganGanti1 = model.KeteranganGanti1;
                }
                else
                {
                    model.IdDriver1 = model.ModelKonsolidasi.Driver1Id;
                    model.NamaDriver1 = model.ModelKonsolidasi.KodeDriver1 + " - " + model.ModelKonsolidasi.NamaDriver1;
                    model.KeteranganGanti1 = model.ModelKonsolidasi.KeteranganDriver1;
                }
                if (model.IdDriver2.HasValue)
                {
                    model.IdDriver2 = model.IdDriver2;
                    model.NamaDriver2 = model.NamaDriver2;
                    model.KeteranganGanti2 = model.KeteranganGanti2;
                }
                else
                {
                    model.IdDriver2 = model.ModelKonsolidasi.Driver2Id;
                    model.NamaDriver2 = model.ModelKonsolidasi.KodeDriver2 + " - " + model.ModelKonsolidasi.NamaDriver2;
                    model.KeteranganGanti2 = model.ModelKonsolidasi.KeteranganDriver2;
                }
                ViewBag.Title = "Batal Order " + dbitem.SalesOrderProsesKonsolidasi.SONumber;
                //ViewBag.postData = "EditPickup";
                return View("Form", model);
            }

            return View();
        }
        [HttpPost]
        public ActionResult Edit(AdminUangJalan model, string Keterangan, string KeteranganBatal)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.AdminUangJalan dummyAdminUangJalan = dbitem.AdminUangJalan;
            Context.BatalOrder batalOrder = new Context.BatalOrder();
            if (dummyAdminUangJalan != null)
            {
                dummyAdminUangJalan.Status = "Batal";
                dummyAdminUangJalan.StatusSblmBatal = dbitem.Status;
            }
            string code = "BO-" + dbitem.SONumber;

            //lebah dieu sync ERPna
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            //cek status na
            if (dbitem.Status == "dispatched")
            {
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalBorongan - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI -
                    dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                //ambil data 
                dbsettlement.IdSalesOrder = model.IdSalesOrder;
                if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai"))
                {
                    dbsettlement.KasDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault().JumlahTransfer;
                }
                if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer")))
                {
                    dbsettlement.TransferDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer")).Sum(t => t.JumlahTransfer);
                }
                dbsettlement.IdAdminUangJalan = dummyAdminUangJalan.Id;
                dbsettlement.SolarDiterima = dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(s => s.Value);
                dbsettlement.KapalDiterima = dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(s => s.Value);
                dbsettlement.JenisBatal = "Batal Order";
                dbsettlement.IdDriver = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.Driver1Id : dbitem.SalesOrderProsesKonsolidasiId.HasValue ?
                    dbitem.SalesOrderProsesKonsolidasi.Driver1Id : dbitem.SalesOrderPickup.Driver1Id;
                RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Order");
                decimal? TotalBiaya = (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                decimal TotalUtangDriver = (dbsettlement.SolarDiterima == null ? 0 : dbsettlement.SolarDiterima.Value) + (dbsettlement.KapalDiterima == null ? 0 : dbsettlement.KapalDiterima.Value) +
                    (dbsettlement.TransferDiterima == null ? 0 : dbsettlement.TransferDiterima.Value) + (dbsettlement.KasDiterima == null ? 0 : dbsettlement.KasDiterima.Value) +
                    (dummyAdminUangJalan.PotonganB == null ? 0 : dummyAdminUangJalan.PotonganB.Value) + (dummyAdminUangJalan.PotonganP == null ? 0 : dummyAdminUangJalan.PotonganP.Value) +
                    (dummyAdminUangJalan.PotonganK == null ? 0 : dummyAdminUangJalan.PotonganK.Value) + (dummyAdminUangJalan.PotonganT == null ? 0 : dummyAdminUangJalan.PotonganT.Value);
                Repoglt_det.saveFromAc(1, code, TotalBiaya, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);
                Repoglt_det.saveFromAc(2, code, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem);
                Repoglt_det.saveFromAc(3, code, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem);
                Repoglt_det.saveFromAc(4, code, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem);
                Repoglt_det.saveFromAc(5, code, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem);
                Repoglt_det.saveFromAc(6, code, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem);
                Repoglt_det.saveFromAc(7, code, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem);
                Repoglt_det.saveFromAc(8, code, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem);
                Repoglt_det.saveFromAc(9, code, RepoSalesOrder.Harga(dbitem), 0, Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbitem);
                Repoglt_det.saveFromAc(10, code, 0, RepoSalesOrder.Harga(dbitem), Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbitem);
            }
            else if (dbitem.Status == "admin uang jalan")
            {
                int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == code).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == code).Max(d => d.UrutanBatal) + 1;
                string codePby = "PK-BO-" + dbitem.SONumber + "-" + urutanBatal;
                var glt_oid = Guid.NewGuid().ToString();
                if (dummyAdminUangJalan.KasbonDriver1 != null)
                {
                    Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + dbitem.SONumber, dummyAdminUangJalan.IdDriver1.Value + 7000000);
                    Repopbyd_det.save(
                        glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + dbitem.SONumber, dummyAdminUangJalan.IdDriver1.Value + 7000000, erpConfig.IdAUJCredit.Value,
                        Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, dummyAdminUangJalan.KasbonDriver1.Value
                    );
                    Repoglt_det.saveFromAc(0, code, dummyAdminUangJalan.KasbonDriver1.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, null, null, null, null, null, dummyAdminUangJalan.IdDriver1);//PIUTANG DRIVER BATAL JALAN
                }
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalBorongan - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                Repoglt_det.saveFromAc(
                    1, code,
                    (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum(), 0,
                    Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem
                );
                Repoglt_det.saveFromAc(2, code, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem);
                Repoglt_det.saveFromAc(3, code, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem);
                Repoglt_det.saveFromAc(4, code, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem);
                Repoglt_det.saveFromAc(5, code, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem);
                Repoglt_det.saveFromAc(6, code, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem);
                Repoglt_det.saveFromAc(7, code, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem);
                Repoglt_det.saveFromAc(8, code, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem);
            }

            dbitem.Status = "batal order";
            dbitem.UpdatedBy = UserPrincipal.id;
            batalOrder.IdSalesOrder = dbitem.Id;
            batalOrder.Keterangan = Keterangan;
            dbitem.KeteranganBatal = KeteranganBatal;
            batalOrder.ModifiedDate = DateTime.Now;
            batalOrder.TanggalBatal = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.TanggalMuat : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.TanggalMuat : dbitem.SalesOrderPickupId.HasValue ?
                    dbitem.SalesOrderPickup.TanggalPickup : dbitem.SalesOrderPickup.TanggalPickup;
            batalOrder.Code = code;
            RepoSalesOrder.save(dbitem);
            RepoAuditrail.saveBOHistory(dbitem);

            return RedirectToAction("index", "ListOrder");
        }
    }
}