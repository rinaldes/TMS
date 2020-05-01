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
    public class RevisiJenisTrukController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IRevisiJenisTrukRepo RepoRevisiJenisTruk;
        private ISettlementBatalRepo RepoSettBatal;
        private IBatalOrderRepo RepoBatalOrder;
        private IAuditrailRepo RepoAuditrail;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        public RevisiJenisTrukController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IRevisiJenisTrukRepo repoRevisiJenisTruk,
            ISettlementBatalRepo repoSettBatal, IBatalOrderRepo repoBatalOrder, IAuditrailRepo repoAuditrail, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoSettBatal = repoSettBatal;
            RepoBatalOrder = repoBatalOrder;
            RepoRevisiJenisTruk = repoRevisiJenisTruk;
            RepoAuditrail = repoAuditrail;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repoglt_det = repoglt_det;
        }

        [MyAuthorize(Menu = "Revisi Jenis Truck", Action="create")]
        public ActionResult Edit(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);
            Context.RevisiJenisTruk dbitem = RepoRevisiJenisTruk.FindBySo(idSo);

            RevisiJenisTruk model = new RevisiJenisTruk(dbso);

            if (RepoRevisiJenisTruk.FindBySo(idSo) != null)
            {
                model = new RevisiJenisTruk(dbitem);
            }

            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(RevisiJenisTruk model)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.RevisiJenisTruk revisiTgl = new Context.RevisiJenisTruk();

            if (ModelState.IsValid)
            {
                if (dbso.Status == "dispatched" || dbso.Status == "admin uang jalan")
                {
                    //batalkeun so na
                    Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                    Context.AdminUangJalan dummyAdminUangJalan = dbso.AdminUangJalan;
                    dummyAdminUangJalan.Status = "Batal";
                    Context.BatalOrder batalOrder = new Context.BatalOrder();
                    //batal
                    dbso.Status = "batal order";
                    batalOrder.IdSalesOrder = dbso.Id;
                    batalOrder.Keterangan = "Revisi JenisTruk";
                    batalOrder.ModifiedDate = DateTime.Now;
                    batalOrder.TanggalBatal = dbso.SalesOrderOncallId.HasValue ? dbso.SalesOrderOncall.TanggalMuat : dbso.SalesOrderProsesKonsolidasiId.HasValue ? dbso.SalesOrderProsesKonsolidasi.TanggalMuat : dbso.SalesOrderPickupId.HasValue ?
                            dbso.SalesOrderPickup.TanggalPickup : dbso.SalesOrderPickup.TanggalPickup;
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dbso, 12);
                    RepoSalesOrder.save(dbso, 8);
                    RepoBatalOrder.save(batalOrder, UserPrincipal.id);
                    //settlement batal
                    dbsettlement.IdDriver = dummyAdminUangJalan.IdDriver1;
                    dbsettlement.IdSalesOrder = dbso.Id;
                    if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai"))
                    {
                        dbsettlement.KasDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault().JumlahTransfer;
                    }
                    if (dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer")))
                    {
                        dbsettlement.TransferDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer")).Sum(t => t.JumlahTransfer);
                    }
                    dbsettlement.SolarDiterima = dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(s => s.Value);
                    dbsettlement.KapalDiterima = dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(s => s.Value);
                    dbsettlement.JenisBatal = "Batal Order";
                    dbsettlement.IdDriver = dbso.SalesOrderOncallId.HasValue ? dbso.SalesOrderOncall.Driver1Id : dbso.SalesOrderProsesKonsolidasiId.HasValue ? dbso.SalesOrderProsesKonsolidasi.Driver1Id : dbso.SalesOrderPickup.Driver1Id;
                    RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Revisi Jenis Truk");
                    //create so baru
                    Context.SalesOrder dummySo = new Context.SalesOrder();
                    dummySo.isReturn = true;
                    dummySo.Status = "Draft";
                    dummySo.AdminUangJalanId = null;
                    dummySo.AdminUangJalan = null;
                    dummySo.DateStatus = DateTime.Now;

                    if (dbso.SalesOrderOncallId.HasValue)
                    {
                        //urus anak na
                        Context.SalesOrderOncall dboncall = new Context.SalesOrderOncall();
                        SalesOrderOncall modelOncall = new SalesOrderOncall(dbso);
                        modelOncall.setDb(dboncall);
                        dboncall.SalesOrderOnCallId = 0;
                        dboncall.JenisTruckId = model.JenisTruckIdBaru;
                        dboncall.Urutan = RepoSalesOrder.getUrutanOnCAll(modelOncall.TanggalMuat.Value) + 1;
                        dboncall.SONumber = RepoSalesOrder.generateCodeOnCall(modelOncall.TanggalMuat.Value, dboncall.Urutan);
                        dboncall.DN = "DN" + dboncall.SONumber;

                        dummySo.SalesOrderOncall = dboncall;
                        RepoAuditrail.SetAuditTrail("INSERT INTO dbo.\"SalesOrderOncall\" (\"SONumber\", \"Urutan\", \"TanggalOrder\", \"JamOrder\", \"CustomerId\", \"PrioritasId\", \"JenisTruckId\", \"ProductId\", " +
                            "\"TanggalMuat\", \"JamMuat\", \"Keterangan\", \"KeteranganLoading\", \"KeteranganUnloading\", \"IdDaftarHargaItem\", \"StrDaftarHargaItem\", \"StrMultidrop\", \"IdDataTruck\", \"Driver1Id\", " +
                            "\"KeteranganDriver1\", \"Driver2Id\", \"KeteranganDriver2\", \"IsCash\", \"KeteranganRek\", \"IdDriverTitip\", \"DN\", \"KeteranganDataTruck\", \"AtmId\") VALUES (" + dboncall.SONumber + ", "
                            + dboncall.Urutan + ", " + dboncall.TanggalOrder + ", " + dboncall.JamOrder + ", " + dboncall.CustomerId + ", " + dboncall.PrioritasId + ", " + dboncall.JenisTruckId + ", " + dboncall.ProductId +
                            ", " + dboncall.TanggalMuat + ", " + dboncall.JamMuat + ", " + dboncall.Keterangan + ", " + dboncall.KeteranganLoading + ", " + dboncall.KeteranganUnloading + ", " + dboncall.IdDaftarHargaItem +
                            "," + dboncall.StrDaftarHargaItem + ", " + dboncall.StrMultidrop + ", " + dboncall.IdDataTruck + ", " + dboncall.Driver1Id + ", " + dboncall.KeteranganDriver1 + ", " + dboncall.Driver2Id + ", " + 
                            dboncall.KeteranganDriver2 + ", " + dboncall.IsCash + ", " + dboncall.KeteranganRek + ", " + dboncall.IdDriverTitip + ", " + dboncall.DN + ", " + dboncall.KeteranganDataTruck + ", " +
                            dboncall.AtmId + ");", "List Order", "Revisi Jenis Truk", UserPrincipal.id);
                    }
                    else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        Context.SalesOrderProsesKonsolidasi dbkonsolidasi = new Context.SalesOrderProsesKonsolidasi();
                        SalesOrderProsesKonsolidasi modelKonsolidasi = new SalesOrderProsesKonsolidasi(dbso);
                        modelKonsolidasi.setDb(dbkonsolidasi);
                        dbkonsolidasi.SalesOrderProsesKonsolidasiId = 0;
                        dbkonsolidasi.JenisTruckId = model.JenisTruckIdBaru;
                        dbkonsolidasi.Urutan = RepoSalesOrder.getUrutanProsesKonsolidasi(modelKonsolidasi.TanggalMuat.Value) + 1;
                        dbkonsolidasi.SONumber = RepoSalesOrder.generateProsesKonsolidasi(modelKonsolidasi.TanggalMuat.Value, dbkonsolidasi.Urutan);
                        dbkonsolidasi.DN = "DN" + dbkonsolidasi.SONumber;
                        dummySo.SalesOrderProsesKonsolidasi = dbkonsolidasi;
                        RepoAuditrail.saveSalesOrderProsesKonsolidasiQuery(dbso.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                    }
                    dummySo.Id = 0;
                    dummySo.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dummySo);
                    Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                    string code = "RK-" + dbso.SONumber;
                    decimal? TotalBiaya = (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                        dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                        dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                    int id = dbso.Id;
                    Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
                    decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                    decimal? boronganDasar = dummyAdminUangJalan.TotalBorongan - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI -
                        dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
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
                else 
                {
                    if (dbso.SalesOrderOncallId.HasValue)
                    {
                        dbso.SalesOrderOncall.JenisTruckId = model.JenisTruckIdBaru;
                        RepoAuditrail.SetAuditTrail(
                            "UPDATE dbo.\"SalesOrderOncall\" SET \"JenisTruckId\" = " + model.JenisTruckIdBaru + " WHERE \"SalesOrderOnCallId\" = " + dbso.SalesOrderOncallId + ";",
                            "List Order", "Revisi Jenis Truk", UserPrincipal.id
                        );
                    }
                    else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        dbso.SalesOrderProsesKonsolidasi.JenisTruckId = model.JenisTruckIdBaru;
                    }
                    revisiTgl.IdSalesOrder = model.IdSalesOrder.Value;
                    revisiTgl.Keterangan = model.KeteranganRevisi;
                    revisiTgl.JenisTruckIdLama = model.JenisTruckIdLama;
                    revisiTgl.JenisTruckIdBaru = model.JenisTruckIdBaru;
                    revisiTgl.ModifiedDate = DateTime.Now;
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoRevisiJenisTruk.save(revisiTgl);
                    RepoSalesOrder.save(dbso, 12);
                    RepoAuditrail.saveUpdSalesOrderProsesKonsolidasiQuery(dbso.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                }

                return RedirectToAction("Index", "ListOrder");
            }

            return View("Form", model);
        }
    }
}