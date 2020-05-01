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
    public class RevisiRuteController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IRevisiRuteRepo RepoRevisiRute;
        private ISettlementBatalRepo RepoSettBatal;
        private IBatalOrderRepo RepoBatalOrder;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private IAuditrailRepo RepoAuditrail;
        private Iso_mstrRepo Reposo_mstr;
        private Ipbyd_detRepo Repopbyd_det;
        public RevisiRuteController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IRevisiRuteRepo repoRevisiRute,
            ISettlementBatalRepo repoSettBatal, IBatalOrderRepo repoBatalOrder, Iglt_detRepo repoglt_det, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr,
            Ibk_mstrRepo repobk_mstr, IAuditrailRepo repoAuditrail, Iso_mstrRepo reposo_mstr, Ipbyd_detRepo repopbyd_det)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoRevisiRute = repoRevisiRute;
            RepoSettBatal = repoSettBatal;
            RepoBatalOrder = repoBatalOrder;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            RepoAuditrail = repoAuditrail;
            Reposo_mstr = reposo_mstr;
            Repopbyd_det = repopbyd_det;
        }

        [MyAuthorize(Menu = "Revisi Rute", Action="create")]
        public ActionResult Edit(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);

            RevisiRute model = new RevisiRute(dbso);
            ViewBag.kondisi = "revisiRute";

            if(dbso.SalesOrderOncallId.HasValue)
                return View("FormOncall", model);
            else if (dbso.SalesOrderPickupId.HasValue)
                return View("FormPickup", model);
            else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                return View("FormKonsolidasi", model);

            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Edit(RevisiRute model)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.RevisiRute revisiRute = new Context.RevisiRute();
            revisiRute.Code = "RR-" + (dbso.SalesOrderOncallId.HasValue ? dbso.SONumber : dbso.SalesOrderProsesKonsolidasiId.HasValue ? dbso.SONumber : dbso.SalesOrderPickupId.HasValue ? dbso.SalesOrderPickup.SONumber : dbso.SalesOrderKontrak.SONumber);
            Context.SalesOrder dummySo = new Context.SalesOrder();
            
            if (ModelState.IsValid)
            {
                if (dbso.Status == "dispatched" || dbso.Status == "admin uang jalan" || dbso.Status == "billing" || dbso.Status == "surat jalan")
                {
                    Context.AdminUangJalan dummyAdminUangJalan = dbso.AdminUangJalan;
                    dummyAdminUangJalan.Status = "Batal";
                    dummyAdminUangJalan.StatusSblmBatal = dbso.Status;
                    Context.BatalOrder batalOrder = new Context.BatalOrder();
                    //batal
                    string batal_status = dbso.Status;
                    dbso.Status = "batal order";
                    dbso.UpdatedBy = UserPrincipal.id;
                    dbso.KeteranganBatal = "Revisi rute dari " + model.RuteLama + " ke " + model.RuteBaru;//"Revisi RUte" dari <rute sebelumnya> ke <rute yang dipilih > [isi dari keterangan revisi rute ]
                    batalOrder.IdSalesOrder = dbso.Id;
                    batalOrder.Keterangan = "Revisi Rute";
                    batalOrder.ModifiedDate = DateTime.Now;
                    batalOrder.TanggalBatal = dbso.SalesOrderOncallId.HasValue ? dbso.SalesOrderOncall.TanggalMuat : dbso.SalesOrderProsesKonsolidasiId.HasValue ? dbso.SalesOrderProsesKonsolidasi.TanggalMuat : dbso.SalesOrderPickupId.HasValue ?
                            dbso.SalesOrderPickup.TanggalPickup : dbso.SalesOrderPickup.TanggalPickup;
                    RepoSalesOrder.save(dbso, 13);
                    RepoSalesOrder.save(dbso, 8);
                    RepoBatalOrder.save(batalOrder, UserPrincipal.id);
                    Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
                    if (batal_status == "dispatched" || batal_status == "billing" || batal_status == "surat jalan")
                    {
                        //batalkeun so na
                        Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                        //settlement batal
                        dbsettlement.IdDriver = dummyAdminUangJalan.IdDriver1;
                        dbsettlement.IdSalesOrder = dbso.Id;
                        dbsettlement.IdAdminUangJalan = dbso.AdminUangJalanId;
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
                        string code = revisiRute.Code;
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
                        RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Order");
                    }
                    else{
                        //jurnal balik cynt
                        decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                        decimal? boronganDasar = dummyAdminUangJalan.TotalAlokasi - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                        Repoglt_det.saveFromAc(1, revisiRute.Code, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbso);
                        Repoglt_det.saveFromAc(2, revisiRute.Code, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbso);
                        Repoglt_det.saveFromAc(3, revisiRute.Code, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbso);
                        Repoglt_det.saveFromAc(4, revisiRute.Code, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbso);
                        Repoglt_det.saveFromAc(5, revisiRute.Code, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbso);
                        Repoglt_det.saveFromAc(6, revisiRute.Code, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbso);
                        Repoglt_det.saveFromAc(7, revisiRute.Code, 0, tambahanRute, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbso);
                        Repoglt_det.saveFromAc(8, revisiRute.Code, 0, dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteLain), dbso);
                        Repoglt_det.saveFromAc(9, revisiRute.Code, dummyAdminUangJalan.KasbonDriver1, 0, Repoac_mstr.FindByPk(erpConfig.IdKasbonAuj), dbso);
                        Repoglt_det.saveFromAc(10, revisiRute.Code, dummyAdminUangJalan.KlaimDriver1, 0, Repoac_mstr.FindByPk(erpConfig.IdKlaimAuj), dbso);
                        decimal? utangDriver = dummyAdminUangJalan.AdminUangJalanPotonganDriver.Sum(s => s.Value) + dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(s => s.Value) + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(s => s.Value) + dummyAdminUangJalan.AdminUangJalanUangTf.Sum(s => s.Value);
                        Repoglt_det.saveFromAc(11, revisiRute.Code, dummyAdminUangJalan.AdminUangJalanUangTf.Sum(s => s.Value), 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbso);

                        if (dummyAdminUangJalan.KasbonDriver1 != null)
                        {
                            string code = "BO-" + dbso.SONumber;
                            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == code).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == code).Max(d => d.UrutanBatal) + 1;
                            var glt_oid = Guid.NewGuid().ToString();
                            string codePby = "PK-RR-" + dbso.SONumber + "-" + urutanBatal;
                            Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + dbso.SONumber, dummyAdminUangJalan.IdDriver1.Value + 7000000);
                            Repopbyd_det.save(
                                glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + dbso.SONumber, dummyAdminUangJalan.IdDriver1.Value + 7000000, erpConfig.IdAUJCredit.Value,
                                Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, dummyAdminUangJalan.KasbonDriver1.Value
                            );
                            Repoglt_det.saveFromAc(0, code, dummyAdminUangJalan.KasbonDriver1.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbso, null, null, null, null, null, dummyAdminUangJalan.IdDriver1);//PIUTANG DRIVER BATAL JALAN
                        }
                    }
                    //create so baru
                    dummySo.isReturn = true;
                    dummySo.Status = "Draft";
                    dummySo.AdminUangJalanId = null;
                    dummySo.AdminUangJalan = null;
                    dummySo.DateStatus = DateTime.Now;

                    try{
                        SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrLoad);
                        model.ListLoad = resultLoad.ToList();
                        SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrUnload);
                        model.ListUnload = resultUnload.ToList();
                    }
                    catch (Exception)
                    {
                    }

                    if (dbso.SalesOrderOncallId.HasValue)
                    {
                        //urus anak na
                        Context.SalesOrderOncall dboncall = new Context.SalesOrderOncall();
                        SalesOrderOncall modelOncall = new SalesOrderOncall(dbso);
                        modelOncall.setDb(dboncall);
                        dboncall.SalesOrderOnCallId = 0;
                        dboncall.Urutan = RepoSalesOrder.getUrutanOnCAll(modelOncall.TanggalMuat.Value) + 1;
                        dboncall.SONumber = RepoSalesOrder.generateCodeOnCall(modelOncall.TanggalMuat.Value, dboncall.Urutan);
                        dboncall.DN = "DN" + dboncall.SONumber;
                        dboncall.IdDaftarHargaItem = model.IdRute;
                        dboncall.StrDaftarHargaItem = model.RuteBaru;
                        dboncall.StrMultidrop = model.MultidropBaru;
                        dboncall.SalesOrderOnCallLoadingAdd.Clear();
                        dboncall.Keterangan = "Revisi rute dari " + model.RuteLama + " ke " + model.RuteBaru + " - " + dbso.SalesOrderOncall.Keterangan;
                        foreach (SalesOrderLoadUnload item in model.ListLoad)
                        {
                            dboncall.SalesOrderOnCallLoadingAdd.Add(new Context.SalesOrderOnCallLoadingAdd()
                            {
                                CustomerId = dboncall.CustomerId,
                                CustomerLoadingAddressId = item.Id,
                                urutan = item.urutan,
                                IsSelect = item.IsSelect
                            });
                        }

                        dboncall.SalesOrderOnCallUnLoadingAdd.Clear();
                        foreach (SalesOrderLoadUnload item in model.ListUnload)
                        {
                            dboncall.SalesOrderOnCallUnLoadingAdd.Add(new Context.SalesOrderOnCallUnLoadingAdd()
                            {
                                CustomerId = dboncall.CustomerId,
                                CustomerUnloadingAddressId = item.Id,
                                urutan = item.urutan,
                                IsSelect = item.IsSelect
                            });
                        }

                        dummySo.SalesOrderOncall = dboncall;
                        RepoAuditrail.SetAuditTrail("INSERT INTO dbo.\"SalesOrderOncall\" (\"SONumber\", \"Urutan\", \"TanggalOrder\", \"JamOrder\", \"CustomerId\", \"PrioritasId\", \"JenisTruckId\", \"ProductId\", " +
                            "\"TanggalMuat\", \"JamMuat\", \"Keterangan\", \"KeteranganLoading\", \"KeteranganUnloading\", \"IdDaftarHargaItem\", \"StrDaftarHargaItem\", \"StrMultidrop\", \"IdDataTruck\", \"Driver1Id\", " +
                            "\"KeteranganDriver1\", \"Driver2Id\", \"KeteranganDriver2\", \"IsCash\", \"KeteranganRek\", \"IdDriverTitip\", \"DN\", \"KeteranganDataTruck\", \"AtmId\") VALUES (" + dboncall.SONumber + ", "
                            + dboncall.Urutan + ", " + dboncall.TanggalOrder + ", " + dboncall.JamOrder + ", " + dboncall.CustomerId + ", " + dboncall.PrioritasId + ", " + dboncall.JenisTruckId + ", " + dboncall.ProductId +
                            ", " + dboncall.TanggalMuat + ", " + dboncall.JamMuat + ", " + dboncall.Keterangan + ", " + dboncall.KeteranganLoading + ", " + dboncall.KeteranganUnloading + ", " + dboncall.IdDaftarHargaItem +
                            "," + dboncall.StrDaftarHargaItem + ", " + dboncall.StrMultidrop + ", " + dboncall.IdDataTruck + ", " + dboncall.Driver1Id + ", " + dboncall.KeteranganDriver1 + ", " + dboncall.Driver2Id + ", " + 
                            dboncall.KeteranganDriver2 + ", " + dboncall.IsCash + ", " + dboncall.KeteranganRek + ", " + dboncall.IdDriverTitip + ", " + dboncall.DN + ", " + dboncall.KeteranganDataTruck + ", " +
                            dboncall.AtmId + ");", "Revisi Rute", "Edit", UserPrincipal.id);
                    }
                    else if (dbso.SalesOrderPickupId.HasValue)
                    {
                        Context.SalesOrderPickup dbpickup = new Context.SalesOrderPickup();
                        SalesOrderPickup modelPickup = new SalesOrderPickup(dbso);
                        modelPickup.setDb(dbpickup);

                        dbpickup.SalesOrderPickupId = 0;
                        dbpickup.Urutan = RepoSalesOrder.getUrutanPickup(modelPickup.TanggalPickup.Value) + 1;
                        dbpickup.SONumber = RepoSalesOrder.generatePickup(modelPickup.TanggalPickup.Value, dbpickup.Urutan);
                        dbpickup.Keterangan = "Revisi rute dari " + model.RuteLama + " ke " + model.RuteBaru + " - " + dbso.SalesOrderPickup.Keterangan;

                        dbpickup.SalesOrderPickupLoadingAdd.Clear();
                        foreach (SalesOrderLoadUnload item in model.ListLoad)
                        {
                            dbpickup.SalesOrderPickupLoadingAdd.Add(new Context.SalesOrderPickupLoadingAdd()
                            {
                                CustomerId = dbpickup.CustomerId,
                                CustomerLoadingAddressId = item.Id,
                                urutan = item.urutan,
                                IsSelect = item.IsSelect
                            });
                        }

                        dbpickup.SalesOrderPickupUnLoadingAdd.Clear();
                        RepoAuditrail.saveDelAllSalesOrderPickupUnLoadingAddQuery(dbpickup, UserPrincipal.id);
                        foreach (SalesOrderLoadUnload item in model.ListUnload)
                        {
                            dbpickup.SalesOrderPickupUnLoadingAdd.Add(new Context.SalesOrderPickupUnLoadingAdd()
                            {
                                CustomerId = dbpickup.CustomerId,
                                CustomerUnloadingAddressId = item.Id,
                                urutan = item.urutan,
                                IsSelect = item.IsSelect
                            });
                        }

                        dummySo.SalesOrderPickup = dbpickup;
                    }
                    else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        Context.SalesOrderProsesKonsolidasi dbkonsolidasi = new Context.SalesOrderProsesKonsolidasi();
                        SalesOrderProsesKonsolidasi modelKonsolidasi = new SalesOrderProsesKonsolidasi(dbso);
                        modelKonsolidasi.setDb(dbkonsolidasi);

                        dbkonsolidasi.SalesOrderProsesKonsolidasiId = 0;
                        try{
                            dbkonsolidasi.Urutan = RepoSalesOrder.getUrutanProsesKonsolidasi(modelKonsolidasi.TanggalMuat.Value) + 1;
                        }
                        catch (Exception)
                        {
                            dbkonsolidasi.Urutan = 0;
                        }
                        dbkonsolidasi.SONumber = RepoSalesOrder.generateProsesKonsolidasi(modelKonsolidasi.TanggalMuat.Value, dbkonsolidasi.Urutan);
                        dbkonsolidasi.DN = "DN" + dbkonsolidasi.SONumber;
                        dbkonsolidasi.IdDaftarHargaItem = model.IdRute;
                        dbkonsolidasi.StrDaftarHargaItem = model.RuteBaru;
                        dbkonsolidasi.Multidrop = model.MultidropBaru;
                        dbkonsolidasi.Keterangan = "Revisi rute dari " + model.RuteLama + " ke " + model.RuteBaru + " - " + dbso.SalesOrderProsesKonsolidasi.Keterangan;

                        dummySo.SalesOrderProsesKonsolidasi = dbkonsolidasi;
                        RepoAuditrail.saveSalesOrderProsesKonsolidasiQuery(dummySo.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                    }

                    dummySo.Id = 0;
                    dummySo.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dummySo);
                    if (dbso.SalesOrderPickupId.HasValue){
                        foreach (Context.SalesOrderPickupUnLoadingAdd sopula in dbso.SalesOrderPickup.SalesOrderPickupUnLoadingAdd){
                            RepoAuditrail.saveSalesOrderPickupUnLoadingAddQuery(sopula, UserPrincipal.id);
                        }
                    }
                }
                else
                {
                    if (dbso.SalesOrderOncallId.HasValue)
                    {
                        dbso.SalesOrderOncall.IdDaftarHargaItem = model.IdRute;
                        dbso.SalesOrderOncall.StrDaftarHargaItem = model.RuteBaru;
                        dbso.SalesOrderOncall.StrMultidrop = model.MultidropBaru;
                        RepoAuditrail.SetAuditTrail(
                            "UPDATE dbo.\"SalesOrderOncall\" SET \"IdDaftarHargaItem\" = " + model.IdRute + ", \"StrDaftarHargaItem\" = " + model.RuteBaru + ", \"StrMultidrop\" = " + model.MultidropBaru + 
                            " WHERE \"SalesOrderOnCallId\" = " + dbso.SalesOrderOncallId + ";",
                            "List Order", "Revisi Rute", UserPrincipal.id
                        );
                    }
                    else if (dbso.SalesOrderPickupId.HasValue)
                    {
                        dbso.SalesOrderPickup.RuteId = model.IdRute;
                    }
                    else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        dbso.SalesOrderProsesKonsolidasi.IdDaftarHargaItem = model.IdRute;
                        dbso.SalesOrderProsesKonsolidasi.StrDaftarHargaItem = model.RuteBaru;
                        dbso.SalesOrderProsesKonsolidasi.Multidrop = model.MultidropBaru;
                        RepoAuditrail.saveUpdSalesOrderProsesKonsolidasiQuery(dbso.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                    }
                    revisiRute.IdSalesOrder = model.IdSalesOrder.Value;
                    dbso.RuteRevised = true;
                    dbso.AdminUangJalanId = null;
                    dbso.AdminUangJalan = null;
                    dbso.UpdatedBy = UserPrincipal.id;
                    RepoSalesOrder.save(dbso, 13);
                    string sod_guid = Guid.NewGuid().ToString();
                    dbso.oidErp = sod_guid;
                    RepoRevisiRute.save(revisiRute);
                }

                return RedirectToAction("Index", "ListOrder");
            }

            model = new RevisiRute(dbso);

            if (dbso.SalesOrderOncallId.HasValue)
                return View("FormOncall", model);
            else if (dbso.SalesOrderPickupId.HasValue)
                return View("FormPickup", model);
            else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                return View("FormKonsolidasi", model);

            return View("");
        }
    }
}