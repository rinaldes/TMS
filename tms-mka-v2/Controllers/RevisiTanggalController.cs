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
    public class RevisiTanggalController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IRevisiTanggalRepo RepoRevisiTanggal;
        private ISettlementBatalRepo RepoSettBatal;
        private IBatalOrderRepo RepoBatalOrder;
        private IAuditrailRepo RepoAuditrail;
        private Iso_mstrRepo Reposo_mstr;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;

        public RevisiTanggalController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IRevisiTanggalRepo repoRevisiTanggal, ISettlementBatalRepo repoSettBatal,
            IBatalOrderRepo repoBatalOrder, IAuditrailRepo repoAuditrail, Iso_mstrRepo reposo_mstr, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoRevisiTanggal = repoRevisiTanggal;
            RepoSettBatal = repoSettBatal;
            RepoBatalOrder = repoBatalOrder;
            RepoAuditrail = repoAuditrail;
            Reposo_mstr = reposo_mstr;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repoglt_det = repoglt_det;
        }

        [MyAuthorize(Menu = "Revisi Tanggal", Action="create")]
        public ActionResult Edit(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);
            Context.RevisiTanggal dbitem = RepoRevisiTanggal.FindBySo(idSo);
            
            RevisiTanggal model = new RevisiTanggal(dbso);

            if (RepoRevisiTanggal.FindBySo(idSo) != null)
            {
                model = new RevisiTanggal(dbitem);
            }

            return View("Form", model);
        }

        [MyAuthorize(Menu = "Revisi Tanggal BP", Action = "create")]
        public ActionResult EditBP(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);
            Context.RevisiTanggal dbitem = RepoRevisiTanggal.FindBySo(idSo);

            RevisiTanggal model = new RevisiTanggal(dbso);

            if (RepoRevisiTanggal.FindBySo(idSo) != null)
            {
                model = new RevisiTanggal(dbitem);
            }

            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(RevisiTanggal model)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.RevisiTanggal revisiTgl = new Context.RevisiTanggal();

            if (ModelState.IsValid)
            {
                if (dbso.Status == "dispatched")
                {
                    //batalkeun so na
                    Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                    Context.AdminUangJalan dummyAdminUangJalan = dbso.AdminUangJalan;
                    dummyAdminUangJalan.Status = "Batal";
                    Context.BatalOrder batalOrder = new Context.BatalOrder();
                    //batal
                    dbso.Status = "batal order";
                    batalOrder.IdSalesOrder = dbso.Id;
                    batalOrder.Keterangan = "Revisi Tanggal";
                    batalOrder.ModifiedDate = DateTime.Now;
                    batalOrder.TanggalBatal = dbso.SalesOrderOncallId.HasValue ? dbso.SalesOrderOncall.TanggalMuat : dbso.SalesOrderProsesKonsolidasiId.HasValue ? dbso.SalesOrderProsesKonsolidasi.TanggalMuat : dbso.SalesOrderPickupId.HasValue ?
                            dbso.SalesOrderPickup.TanggalPickup : dbso.SalesOrderPickup.TanggalPickup;
                    dbso.KeteranganBatal = "Revisi tanggal dari " + (dbso.SalesOrderOncallId.HasValue ? (dbso.SalesOrderOncall.TanggalMuat.ToString()).Split(' ')[0] : dbso.SalesOrderPickupId.HasValue ? dbso.SalesOrderPickup.TanggalPickup.ToString().Split(' ')[0] : dbso.SalesOrderProsesKonsolidasi.TanggalMuat.ToString().Split(' ')[0]) + " ke " + model.TanggalBaru.ToString().Split(' ')[0] + " - " + model.KeteranganRevisi;
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dbso, 11);
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
                    RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Revisi Tanggal");
                    //create so baru
                    Context.SalesOrder dummySo = new Context.SalesOrder();
                    dummySo.isReturn = true;
                    dummySo.DateRevised = true;
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
                        dboncall.TanggalMuat = model.TanggalBaru;
                        dboncall.JamMuat = model.JamBaru.Value;
                        dboncall.Urutan = RepoSalesOrder.getUrutanOnCAll(modelOncall.TanggalMuat.Value) + 1;
                        dboncall.SONumber = RepoSalesOrder.generateCodeOnCall(modelOncall.TanggalMuat.Value, dboncall.Urutan);
                        dboncall.DN = "DN" + dboncall.SONumber;
                        dboncall.Keterangan = "Revisi tanggal dari " + (dbso.SalesOrderOncallId.HasValue ? (dbso.SalesOrderOncall.TanggalMuat.ToString()).Split(' ')[0] : dbso.SalesOrderPickupId.HasValue ? dbso.SalesOrderPickup.TanggalPickup.ToString().Split(' ')[0] : dbso.SalesOrderProsesKonsolidasi.TanggalMuat.ToString().Split(' ')[0]) + " ke " + model.TanggalBaru.ToString().Split(' ')[0] + " - " + model.KeteranganRevisi + " - " + dbso.SalesOrderOncall.Keterangan;
                        revisiTgl.JamMuat = dbso.SalesOrderOncall.JamMuat;

                        dummySo.SalesOrderOncall = dboncall;
                        RepoAuditrail.SetAuditTrail("INSERT INTO dbo.\"SalesOrderOncall\" (\"SONumber\", \"Urutan\", \"TanggalOrder\", \"JamOrder\", \"CustomerId\", \"PrioritasId\", \"JenisTruckId\", \"ProductId\", " +
                            "\"TanggalMuat\", \"JamMuat\", \"Keterangan\", \"KeteranganLoading\", \"KeteranganUnloading\", \"IdDaftarHargaItem\", \"StrDaftarHargaItem\", \"StrMultidrop\", \"IdDataTruck\", \"Driver1Id\", " +
                            "\"KeteranganDriver1\", \"Driver2Id\", \"KeteranganDriver2\", \"IsCash\", \"KeteranganRek\", \"IdDriverTitip\", \"DN\", \"KeteranganDataTruck\", \"AtmId\") VALUES (" + dboncall.SONumber + ", "
                            + dboncall.Urutan + ", " + dboncall.TanggalOrder + ", " + dboncall.JamOrder + ", " + dboncall.CustomerId + ", " + dboncall.PrioritasId + ", " + dboncall.JenisTruckId + ", " + dboncall.ProductId +
                            ", " + dboncall.TanggalMuat + ", " + dboncall.JamMuat + ", " + dboncall.Keterangan + ", " + dboncall.KeteranganLoading + ", " + dboncall.KeteranganUnloading + ", " + dboncall.IdDaftarHargaItem +
                            "," + dboncall.StrDaftarHargaItem + ", " + dboncall.StrMultidrop + ", " + dboncall.IdDataTruck + ", " + dboncall.Driver1Id + ", " + dboncall.KeteranganDriver1 + ", " + dboncall.Driver2Id + ", " + 
                            dboncall.KeteranganDriver2 + ", " + dboncall.IsCash + ", " + dboncall.KeteranganRek + ", " + dboncall.IdDriverTitip + ", " + dboncall.DN + ", " + dboncall.KeteranganDataTruck + ", " +
                            dboncall.AtmId + ");", "List Order", "Revisi Tanggal", UserPrincipal.id);
                    }
                    else if (dbso.SalesOrderPickupId.HasValue)
                    {
                        Context.SalesOrderPickup dbpickup = new Context.SalesOrderPickup();
                        SalesOrderPickup modelPickup = new SalesOrderPickup(dbso);
                        modelPickup.setDb(dbpickup);
                        revisiTgl.JamMuat = dbso.SalesOrderPickup.JamPickup;
                        dbpickup.SalesOrderPickupId = 0;
                        dbpickup.TanggalPickup = model.TanggalBaru.Value;
                        dbpickup.JamPickup = model.JamBaru.Value;
                        dbpickup.Urutan = RepoSalesOrder.getUrutanPickup(modelPickup.TanggalPickup.Value) + 1;
                        dbpickup.SONumber = RepoSalesOrder.generatePickup(modelPickup.TanggalPickup.Value, dbpickup.Urutan);
                        dbpickup.Keterangan = "Revisi tanggal dari " + (dbso.SalesOrderOncallId.HasValue ? (dbso.SalesOrderOncall.TanggalMuat.ToString()).Split(' ')[0] : dbso.SalesOrderPickupId.HasValue ? dbso.SalesOrderPickup.TanggalPickup.ToString().Split(' ')[0] : dbso.SalesOrderProsesKonsolidasi.TanggalMuat.ToString().Split(' ')[0]) + " ke " + model.TanggalBaru.ToString().Split(' ')[0] + " - " + model.KeteranganRevisi + " - " + dbso.SalesOrderPickup.Keterangan;

                        dummySo.SalesOrderPickup = dbpickup;
                    }
                    else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        Context.SalesOrderProsesKonsolidasi dbkonsolidasi = new Context.SalesOrderProsesKonsolidasi();
                        SalesOrderProsesKonsolidasi modelKonsolidasi = new SalesOrderProsesKonsolidasi(dbso);
                        modelKonsolidasi.setDb(dbkonsolidasi);
                        revisiTgl.JamMuat = dbso.SalesOrderProsesKonsolidasi.JamMuat;
                        dbkonsolidasi.SalesOrderProsesKonsolidasiId = 0;
                        dbkonsolidasi.TanggalMuat = model.TanggalBaru.Value;
                        dbkonsolidasi.JamMuat = model.JamBaru.Value;
                        dbkonsolidasi.Urutan = RepoSalesOrder.getUrutanProsesKonsolidasi(modelKonsolidasi.TanggalMuat.Value) + 1;
                        dbkonsolidasi.SONumber = RepoSalesOrder.generateProsesKonsolidasi(modelKonsolidasi.TanggalMuat.Value, dbkonsolidasi.Urutan);
                        dbkonsolidasi.DN = "DN" + dbkonsolidasi.SONumber;
                        dbkonsolidasi.Keterangan = "Revisi tanggal dari " + (dbso.SalesOrderOncallId.HasValue ? (dbso.SalesOrderOncall.TanggalMuat.ToString()).Split(' ')[0] : dbso.SalesOrderPickupId.HasValue ? dbso.SalesOrderPickup.TanggalPickup.ToString().Split(' ')[0] : dbso.SalesOrderProsesKonsolidasi.TanggalMuat.ToString().Split(' ')[0]) + " ke " + model.TanggalBaru.ToString().Split(' ')[0] + " - " + model.KeteranganRevisi + " - " + dbso.SalesOrderProsesKonsolidasi.Keterangan;

                        dummySo.SalesOrderProsesKonsolidasi = dbkonsolidasi;
                        RepoAuditrail.saveSalesOrderProsesKonsolidasiQuery(dummySo.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                    }
                    dummySo.Id = 0;
                    dummySo.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dummySo);
                    Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                    string code = "RT-" + dummySo.SONumber;
                    decimal? TotalBiaya = (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                        dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                        dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                    string sod_guid = Guid.NewGuid().ToString();
                    dummySo.oidErp = sod_guid;
                    decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                    decimal? boronganDasar = dummyAdminUangJalan.TotalBorongan - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI -
                        dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                    Repoglt_det.saveFromAc(1, code, TotalBiaya, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso);
                    Repoglt_det.saveFromAc(2, code, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbso);
                    Repoglt_det.saveFromAc(3, code, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbso);
                    Repoglt_det.saveFromAc(4, code, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbso);
                    Repoglt_det.saveFromAc(5, code, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbso);
                    Repoglt_det.saveFromAc(6, code, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbso);
                    Repoglt_det.saveFromAc(7, code, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbso);
                    Repoglt_det.saveFromAc(8, code, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbso);
                    Repoglt_det.saveFromAc(9, code, RepoSalesOrder.Harga(dbso), 0, Repoac_mstr.FindByPk(erpConfig.IdPendapatanUsahaBlmInv), dbso);
                    Repoglt_det.saveFromAc(10, code, 0, RepoSalesOrder.Harga(dbso), Repoac_mstr.FindByPk(erpConfig.IdPiutangDagang), dbso);
                }
                else 
                {
                    if (dbso.SalesOrderOncallId.HasValue)
                    {
                        revisiTgl.JamMuat = dbso.SalesOrderOncall.JamMuat;
                        dbso.SalesOrderOncall.TanggalMuat = model.TanggalBaru;
                        dbso.SalesOrderOncall.JamMuat = model.JamBaru.Value;
                        dbso.SalesOrderOncall.Keterangan = dbso.SalesOrderOncall.Keterangan + ". Revisi tanggal dari " + (dbso.SalesOrderOncallId.HasValue ? (dbso.SalesOrderOncall.TanggalMuat.ToString()).Split(' ')[0] : dbso.SalesOrderPickupId.HasValue ? dbso.SalesOrderPickup.TanggalPickup.ToString().Split(' ')[0] : dbso.SalesOrderProsesKonsolidasi.TanggalMuat.ToString().Split(' ')[0]) + " ke " + model.TanggalBaru.ToString().Split(' ')[0] + " - " + model.KeteranganRevisi + " - " + dbso.SalesOrderOncall.Keterangan;
                        RepoAuditrail.SetAuditTrail(
                            "UPDATE dbo.\"SalesOrderOncall\" SET \"TanggalMuat\" = " + model.TanggalBaru + ", \"JamMuat\" = " + model.JamBaru.Value + " WHERE \"SalesOrderOnCallId\" = " + dbso.SalesOrderOncallId + ";",
                            "List Order", "Revisi Tanggal", UserPrincipal.id
                        );
                    }
                    else if (dbso.SalesOrderPickupId.HasValue)
                    {
                        revisiTgl.JamMuat = dbso.SalesOrderPickup.JamPickup;
                        dbso.SalesOrderPickup.TanggalPickup = model.TanggalBaru.Value;
                        dbso.SalesOrderPickup.JamPickup = model.JamBaru.Value;
                        dbso.SalesOrderPickup.Keterangan = dbso.SalesOrderPickup.Keterangan + ". Revisi tanggal dari " + dbso.OrderTanggalMuat.ToString().Split(' ')[0] + " ke " + model.TanggalBaru.ToString().Split(' ')[0] + " - " + model.KeteranganRevisi;
                    }
                    else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        revisiTgl.JamMuat = dbso.SalesOrderProsesKonsolidasi.JamMuat;
                        dbso.SalesOrderProsesKonsolidasi.TanggalMuat = model.TanggalBaru.Value;
                        dbso.SalesOrderProsesKonsolidasi.JamMuat = model.JamBaru.Value;
                        dbso.SalesOrderProsesKonsolidasi.Keterangan = dbso.SalesOrderProsesKonsolidasi.Keterangan + ". Revisi tanggal dari " + (dbso.SalesOrderOncallId.HasValue ? (dbso.SalesOrderOncall.TanggalMuat.ToString()).Split(' ')[0] : dbso.SalesOrderPickupId.HasValue ? dbso.SalesOrderPickup.TanggalPickup.ToString().Split(' ')[0] : dbso.SalesOrderProsesKonsolidasi.TanggalMuat.ToString().Split(' ')[0]) + " ke " + model.TanggalBaru.ToString().Split(' ')[0] + " - " + model.KeteranganRevisi;
                        RepoAuditrail.saveUpdSalesOrderProsesKonsolidasiQuery(dbso.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                    }
                    revisiTgl.IdSalesOrder = model.IdSalesOrder.Value;
                    revisiTgl.Keterangan = model.KeteranganRevisi;
                    revisiTgl.TanggalMuat = model.TanggalBaru;
                    revisiTgl.ModifiedDate = DateTime.Now;
                    dbso.DateRevised = true;
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dbso, 11);
                    RepoRevisiTanggal.save(revisiTgl);
                }

                return RedirectToAction("Index", "ListOrder");
            }

            return View("Form", model);
        }

        [HttpPost]
        public ActionResult EditBP(RevisiTanggal model)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.RevisiTanggal revisiTgl = new Context.RevisiTanggal();

            if (ModelState.IsValid)
            {
                if (dbso.SalesOrderOncallId.HasValue)
                {
                    dbso.SalesOrderOncall.TanggalMuat = model.TanggalBaru;
                    dbso.SalesOrderOncall.JamMuat = model.JamBaru.Value;
                    revisiTgl.JamMuat = dbso.SalesOrderOncall.JamMuat;
                    RepoAuditrail.SetAuditTrail(
                        "UPDATE dbo.\"SalesOrderOncall\" SET \"TanggalMuat\" = " + model.TanggalBaru + ", \"JamMuat\" = " + model.JamBaru.Value + " WHERE \"SalesOrderOnCallId\" = " + dbso.SalesOrderOncallId + ";",
                        "List Order", "Revisi Tanggal BP", UserPrincipal.id
                    );
                }
                else if (dbso.SalesOrderPickupId.HasValue)
                {
                    revisiTgl.JamMuat = dbso.SalesOrderPickup.JamPickup;
                    dbso.SalesOrderPickup.TanggalPickup = model.TanggalBaru.Value;
                    dbso.SalesOrderPickup.JamPickup = model.JamBaru.Value;
                }
                else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    revisiTgl.JamMuat = dbso.SalesOrderProsesKonsolidasi.JamMuat;
                    dbso.SalesOrderProsesKonsolidasi.TanggalMuat = model.TanggalBaru.Value;
                    dbso.SalesOrderProsesKonsolidasi.JamMuat = model.JamBaru.Value;
                    RepoAuditrail.saveUpdSalesOrderProsesKonsolidasiQuery(dbso.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                }
                revisiTgl.IdSalesOrder = model.IdSalesOrder.Value;
                revisiTgl.Keterangan = model.KeteranganRevisi;
                revisiTgl.TanggalMuat = model.TanggalBaru;
                revisiTgl.ModifiedDate = DateTime.Now;
                dbso.DateRevised = true;
                dbso.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbso, 14);
                RepoRevisiTanggal.save(revisiTgl);

                return RedirectToAction("Index", "ListOrder");
            }

            return View("Form", model);
        }
    }
}