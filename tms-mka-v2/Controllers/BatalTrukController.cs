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
    public class BatalTrukController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        private IBatalOrderRepo RepoBatalOrder;
        private ISettlementBatalRepo RepoSettBatal;
        private IAtmRepo Repoatm;
        private IDataBoronganRepo RepoBor;
        private IAdminUangJalanRepo RepoAdminUangJalan;
        private Iglt_detRepo Repoglt_det;
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private IAuditrailRepo RepoAuditrail;
        private Ipbyd_detRepo Repopbyd_det;

        public BatalTrukController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IBatalOrderRepo repoBatalOrder,
            ISettlementBatalRepo repoSettBatal, IAtmRepo repoatm, IDataBoronganRepo repoBor, IAdminUangJalanRepo repoAdminUangJalan, Iglt_detRepo repoglt_det,
            IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr, IAuditrailRepo repoAuditrail, Ipbyd_detRepo repopbyd_det,
            ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoBatalOrder = repoBatalOrder;
            RepoSettBatal = repoSettBatal;
            Repoatm = repoatm;
            RepoBor = repoBor;
            RepoAdminUangJalan = repoAdminUangJalan;
            Repoglt_det = repoglt_det;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            RepoAuditrail = repoAuditrail;
            Repopbyd_det = repopbyd_det;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
        }

        [MyAuthorize(Menu = "Batal Truck", Action="create")]
        public ActionResult Edit(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            AdminUangJalan model = new AdminUangJalan();
            if (dbitem.AdminUangJalanId.HasValue)
                model = new AdminUangJalan(dbitem.AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            ViewBag.Status = dbitem.Status;
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
                ViewBag.dbsoPerDriverCount = 1;
                ViewBag.Title = "Batal Truk " + dbitem.SalesOrderOncall.SONumber;
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
                ViewBag.dbsoPerDriverCount = 1;
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
            else if (dbitem.SalesOrderKontrakId.HasValue)
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
                ViewBag.dbsoPerDriverCount = 1;
                return View("Form", model);
            }
            
            return View();            
        }

        [HttpPost]
        public ActionResult Edit(AdminUangJalan model, string Keterangan, string IsTransfer)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.AdminUangJalan dummyAdminUangJalan = dbitem.AdminUangJalan;
            if (dummyAdminUangJalan != null)
            {
                dummyAdminUangJalan.Status = "Batal";
                dummyAdminUangJalan.StatusSblmBatal = dbitem.Status;
            }
            Context.BatalOrder batalOrder = new Context.BatalOrder();
            string code =  "BT-" + (dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.SONumber : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.SONumber : dbitem.SalesOrderPickup.SONumber);
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == code).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == code).Max(d => d.UrutanBatal) + 1;
            int? vehicleId =  dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.IdDataTruck : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.IdDataTruck : dbitem.SalesOrderPickup.IdDataTruck;
            int? DriverId =  dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.Driver1Id : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.Driver1Id : dbitem.SalesOrderPickup.Driver1Id;

            //cek status na
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            if (dbitem.Status == "dispatched")
            {
                Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalAlokasi - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                //ambil data 
                dbsettlement.IdSalesOrder = model.IdSalesOrder;
                if(dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai" ))
                {
                    dbsettlement.KasDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").FirstOrDefault().JumlahTransfer;
                }
                if(dummyAdminUangJalan.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer")))
                {
                    dbsettlement.TransferDiterima = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer")).Sum(t => t.JumlahTransfer);
                }
                dbsettlement.SolarDiterima = dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(s => s.Value);
                dbsettlement.KapalDiterima = dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(s => s.Value);
                dbsettlement.JenisBatal = "Batal Truk";
                dbsettlement.IdAdminUangJalan = dummyAdminUangJalan.Id;
                dbsettlement.IdDriver = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.Driver1Id : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.Driver1Id : dbitem.SalesOrderPickup.Driver1Id;
                dbsettlement.IdDataTruck = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.IdDataTruck : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.IdDataTruck : dbitem.SalesOrderPickup.IdDataTruck;
                RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Truk");
                decimal? TotalBiaya = (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                decimal TotalUtangDriver = (dbsettlement.SolarDiterima == null ? 0 : dbsettlement.SolarDiterima.Value) + (dbsettlement.KapalDiterima == null ? 0 : dbsettlement.KapalDiterima.Value) + (dbsettlement.TransferDiterima == null ? 0 : dbsettlement.TransferDiterima.Value) + (dbsettlement.KasDiterima == null ? 0 : dbsettlement.KasDiterima.Value);
                string codeBT = code + "-" + urutanBatal;
                Repoglt_det.saveFromAc(1, codeBT, TotalBiaya, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);
                Repoglt_det.saveFromAc(2, codeBT, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem);
                Repoglt_det.saveFromAc(3, codeBT, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem);
                Repoglt_det.saveFromAc(4, codeBT, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem);
                Repoglt_det.saveFromAc(5, codeBT, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem);
                Repoglt_det.saveFromAc(6, codeBT, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem);
                Repoglt_det.saveFromAc(7, codeBT, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem);
                Repoglt_det.saveFromAc(8, codeBT, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem);
                Repoglt_det.saveFromAc(9, codeBT, 0, dummyAdminUangJalan.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                Repoglt_det.saveFromAc(10, codeBT, 0, dummyAdminUangJalan.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                Repoglt_det.saveFromAc(11, codeBT, 0, dummyAdminUangJalan.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem);
                Repoglt_det.saveFromAc(12, codeBT, 0, dummyAdminUangJalan.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem);
            }
            else if (dbitem.Status == "admin uang jalan"){
                //lebah dieu sync ERPna
                string codeBT = code + "-" + urutanBatal;
                decimal? TotalBiaya = (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalAlokasi - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                var glt_oid = Guid.NewGuid().ToString();
                string codePby = "PK-BT-" + dbitem.SONumber + "-" + urutanBatal;
                if (dummyAdminUangJalan.KasbonDriver1 != null)
                {
                    Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + dbitem.SONumber, dummyAdminUangJalan.IdDriver1.Value + 7000000);
                    Repopbyd_det.save(
                        glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + dbitem.SONumber, dummyAdminUangJalan.IdDriver1.Value + 7000000, erpConfig.IdAUJCredit.Value,
                        Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, dummyAdminUangJalan.KasbonDriver1.Value
                    );
                    Repoglt_det.saveFromAc(0, code, dummyAdminUangJalan.KasbonDriver1.Value, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, null, null, null, null, null, dummyAdminUangJalan.IdDriver1);//PIUTANG DRIVER BATAL JALAN
                    TotalBiaya += dummyAdminUangJalan.KasbonDriver1.Value;
                }
                Repoglt_det.saveFromAc(1, codeBT, TotalBiaya, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);
                Repoglt_det.saveFromAc(2, codeBT, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem);
                Repoglt_det.saveFromAc(3, codeBT, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem);
                Repoglt_det.saveFromAc(4, codeBT, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem);
                Repoglt_det.saveFromAc(5, codeBT, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem);
                Repoglt_det.saveFromAc(6, codeBT, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem);
                Repoglt_det.saveFromAc(7, codeBT, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem);
                Repoglt_det.saveFromAc(8, codeBT, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem);
                Repoglt_det.saveFromAc(9, codeBT, 0, dummyAdminUangJalan.PotonganB, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                Repoglt_det.saveFromAc(10, codeBT, 0, dummyAdminUangJalan.PotonganP, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverPribadi), dbitem);//PIUTANG DRIVER BATAL JALAN / SEMENTARA TUNAI
                Repoglt_det.saveFromAc(11, codeBT, 0, dummyAdminUangJalan.PotonganK, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPengembalianPiutangDriver), dbitem);
                Repoglt_det.saveFromAc(12, codeBT, 0, dummyAdminUangJalan.PotonganT, Repoac_mstr.FindByPk(erpConfig.IdTabunganDriver), dbitem);
            }

            if (dbitem.Status != "batal order"){
                dbitem.Status = "save";
                dbitem.IsBatalTruk = true;
                if (dbitem.SalesOrderOncallId.HasValue){
                    dbitem.KeteranganBatal = "Ganti truck dari " + dbitem.SalesOrderOncall.DataTruck.VehicleNo + " " + Keterangan;//Ganti truck dari <nopol sebelumnya > ke <nopol yang dipilih> [isi dari keterangan batal truck ]
                    dbitem.SalesOrderOncall.Keterangan = "Ganti truck dari " + dbitem.SalesOrderOncall.DataTruck.VehicleNo + " " + Keterangan;//Ganti truck dari <nopol sebelumnya > ke <nopol yang dipilih> [isi dari keterangan batal truck ]
                    RepoAuditrail.SetAuditTrail(
                        "UPDATE dbo.\"SalesOrderOncall\" SET \"Driver1Id\" = NULL , Driver2Id = NULL, IdDataTruck = NULL WHERE \"SalesOrderOnCallId\" = " + dbitem.SalesOrderOncallId, "List Order", "Batal Truk", UserPrincipal.id
                    );
                }
                else if (dbitem.SalesOrderPickupId.HasValue){
                    dbitem.KeteranganBatal = "Ganti truck dari " + dbitem.SalesOrderPickup.DataTruck.VehicleNo + " " + Keterangan;//Ganti truck dari <nopol sebelumnya > ke <nopol yang dipilih> [isi dari keterangan batal truck ]
                    dbitem.SalesOrderPickup.Keterangan = "Ganti truck dari " + dbitem.SalesOrderPickup.DataTruck.VehicleNo + " " + Keterangan;//Ganti truck dari <nopol sebelumnya > ke <nopol yang dipilih> [isi dari keterangan batal truck ]
                }
                else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue){
                    dbitem.KeteranganBatal = "Ganti truck dari " + dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo + " " + Keterangan;//Ganti truck dari <nopol sebelumnya > ke <nopol yang dipilih> [isi dari keterangan batal truck ]
                    dbitem.SalesOrderProsesKonsolidasi.Keterangan = "Ganti truck dari " + dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo + " " + Keterangan;//Ganti truck dari <nopol sebelumnya > ke <nopol yang dipilih> [isi dari keterangan batal truck ]
                }
                batalOrder.IdSalesOrder = dbitem.Id;
                batalOrder.Keterangan = Keterangan;
                batalOrder.ModifiedDate = DateTime.Now;
                batalOrder.TanggalBatal = dbitem.SalesOrderOncallId.HasValue ? dbitem.SalesOrderOncall.TanggalMuat : dbitem.SalesOrderProsesKonsolidasiId.HasValue ? dbitem.SalesOrderProsesKonsolidasi.TanggalMuat : dbitem.SalesOrderPickupId.HasValue ?
                        dbitem.SalesOrderPickup.TanggalPickup : dbitem.SalesOrderPickup.TanggalPickup;
                batalOrder.IsBatalTruk = true;
                batalOrder.Code = code;
                batalOrder.UrutanBatal = urutanBatal;
                batalOrder.IsTransfer = IsTransfer == "1";
                dbitem.AdminUangJalanId = null;
                dbitem.StatusBatal = "Batal";
                dbitem.UpdatedBy = UserPrincipal.id;
                RepoSalesOrder.save(dbitem, 9);
                RepoBatalOrder.save(batalOrder, UserPrincipal.id);

                return RedirectToAction("index","ListOrder");
            }
            ViewBag.status = "batal order";
            return View("Form", model);
        }

        [MyAuthorize(Menu = "Batal Truck Kontrak", Action = "create")]
        public ActionResult EditKontrak(int id, int idsokontrak)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            Context.SalesOrderKontrakListSo soKontrak = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == idsokontrak).FirstOrDefault();
            //ambil jumlah so yang sama admin uang jalan nya
            int jumSo = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == soKontrak.IdAdminUangJalan).Count();
            ViewBag.dbsoPerDriverCount = 1;
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo = new List<Context.SalesOrderKontrakListSo>();
            dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Add(soKontrak);
            AdminUangJalan model = new AdminUangJalan();
            if (soKontrak.IdAdminUangJalan.HasValue)
                model = new AdminUangJalan(soKontrak.AdminUangJalan);

            model.IdSalesOrder = dbitem.Id;
            model.ListIdSo = idsokontrak.ToString(); 
            ViewBag.Status = soKontrak.Status;
            model.ModelKontrak = new SalesOrderKontrak(dbitem);
            if (soKontrak.Status != null && soKontrak.Status.ToLower().Contains("konfirmasi"))
                model.StatusSo = "Konfirmasi";
            if (model.IdDriver1.HasValue)
            {
                model.IdDriver1 = model.IdDriver1;
                model.NamaDriver1 = model.NamaDriver1;
                model.KeteranganGanti1 = model.KeteranganGanti1;
            }
            else
            {
                model.IdDriver1 = soKontrak.Driver1Id;
                model.NamaDriver1 = soKontrak.Driver1.KodeDriver + " - " + soKontrak.Driver1.NamaDriver;
            }
            if (model.IdDriver2.HasValue)
            {
                model.IdDriver2 = model.IdDriver2;
                model.NamaDriver2 = model.NamaDriver2;
                model.KeteranganGanti2 = model.KeteranganGanti2;
            }
            else
            {
                if (soKontrak.Driver2Id.HasValue)
                {
                    model.IdDriver2 = soKontrak.Driver2Id;
                    model.NamaDriver2 = soKontrak.Driver2.KodeDriver + " - " + soKontrak.Driver2.NamaDriver;
                }
            }
            ViewBag.Title = "Batal Truk " + dbitem.SalesOrderKontrak.SONumber;
            return View("Form", model);
        }

        public void IkutanBatal(Context.SalesOrderKontrakListSo sokontrak)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(sokontrak.SalesKontrakId.Value);
            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            if (sokontrak.Status == "admin uang jalan")
            {
                Context.AdminUangJalan dummyAdminUangJalan = sokontrak.AdminUangJalan;
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalAlokasi - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                string code = "BT-" + sokontrak.NoSo;
                int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == code).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == code).Max(d => d.UrutanBatal) + 1;
                string btCode = code + "-" + urutanBatal;
                Repoglt_det.saveFromAc(8, code, dummyAdminUangJalan.TotalBorongan, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem);
                if (DateTime.Now.Month.ToString("00").PadLeft(2, '0') == sokontrak.MuatDate.Month.ToString("00").PadLeft(2, '0'))
                {
                    Repoglt_det.saveFromAc(1, code, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem);
                    Repoglt_det.saveFromAc(2, code, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem);
                    Repoglt_det.saveFromAc(3, code, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem);
                    Repoglt_det.saveFromAc(4, code, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem);
                    Repoglt_det.saveFromAc(5, code, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem);
                    Repoglt_det.saveFromAc(6, code, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem);
                    Repoglt_det.saveFromAc(7, code, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem);
                }
                else
                    Repoglt_det.saveFromAc(2, code, 0, dummyAdminUangJalan.TotalBorongan, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPosSementara), dbitem);
                foreach (var sokon in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Urutan == sokontrak.Urutan && d.IdDataTruck == sokontrak.IdDataTruck).ToList())
                {
                    if (sokon.MuatDate >= sokontrak.MuatDate)
                    {
                        sokon.IsProses = false;
                        sokon.Urutan = 0;
                        sokon.IsBatalTruck = true;
                        sokon.IdDataTruck = null;
                        sokon.Driver1Id = null;
                        sokon.Driver2Id = null;
                        sokon.Status = "draft planning";
                        sokon.StatusFlow = "PLANNING";
                    }
                    else
                        sokon.IsKenaImbasBatalTruck = true;
                }
            }
            else if (sokontrak.Status == "dispatched")
            {
                Context.AdminUangJalan dbauj = sokontrak.AdminUangJalan;
                Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                //ambil data 
                dbsettlement.IdSalesOrder = dbitem.Id;
                List<Context.SalesOrderKontrakListSo> listSokls = RepoSalesOrderKontrakListSo.FindAllByUrutan(sokontrak.Urutan);
                dbsettlement.IdSoKontrak = string.Join(",", listSokls.Select(d => d.Id));
                decimal? KasDiterima = 0;
                decimal? TransferDiterima = 0;
                if (dbauj.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai" && d.isTf == true))
                    KasDiterima = dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai" && d.isTf == true).FirstOrDefault().JumlahTransfer;// -boronganPerHari * jumSoJalan;
                dbsettlement.KasDiterima = KasDiterima;
                if (dbauj.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer") && d.isTf == true))
                    TransferDiterima = dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer") && d.isTf == true).Sum(t => t.JumlahTransfer);// -boronganPerHari * jumSoJalan;
                dbsettlement.TransferDiterima = TransferDiterima;
                dbsettlement.SolarDiterima = dbauj.AdminUangJalanVoucherSpbu.Sum(s => s.Value);// -boronganPerHari * jumSoJalan;
                dbsettlement.KapalDiterima = dbauj.AdminUangJalanVoucherKapal.Sum(s => s.Value);// -boronganPerHari * jumSoJalan;
                dbsettlement.JenisBatal = "Batal Truk";
                dbsettlement.IdDriver = sokontrak.Driver1Id;
                dbsettlement.jumSo = listSokls.Count();
                dbsettlement.jumSoBatal = listSokls.Count();
                dbsettlement.jumSoJalan = 0;
                string code = "BTB-" + sokontrak.NoSo;
                decimal? TotalBiaya = (dbauj.KasbonDriver1 ?? 0) + (dbauj.KlaimDriver1 ?? 0) + dbauj.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                    dbauj.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dbauj.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                    dbauj.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                decimal? tambahanRute = dbauj.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dbauj.TotalBorongan - dbauj.Kawalan - dbauj.Timbangan - dbauj.Karantina - dbauj.SPSI - dbauj.Multidrop - tambahanRute - dbauj.AdminUangJalanTambahanLain.Sum(s => s.Values);
                dbsettlement.KasDiakui = 0;// dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.TransferDiakui = 0;//dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan != "Tunai").Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.SolarDiakui = 0;// dbauj.AdminUangJalanVoucherSpbu.Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.KapalDiakui = 0;// dbauj.AdminUangJalanVoucherKapal.Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.KasKembali = KasDiterima - dbsettlement.KasDiakui;
                dbsettlement.TransferKembali = TransferDiterima - dbsettlement.TransferDiakui;
                dbsettlement.SolarKembali = dbsettlement.SolarDiterima - dbsettlement.SolarDiakui;
                dbsettlement.KapalKembali = dbsettlement.KapalDiterima - dbsettlement.KapalDiakui;
                decimal? sementaraTunai = dbsettlement.KasKembali + dbsettlement.TransferKembali + dbsettlement.KapalKembali;

                //hutang uang jalan pada biaya borongan, jurnal balik
                int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == code).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == code).Max(d => d.UrutanBatal) + 1;
                string codeJB = "BJ-" + sokontrak.NoSo + "-" + urutanBatal;
                Repoglt_det.saveFromAc(1, codeJB, TotalBiaya, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(2, codeJB, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(3, codeJB, 0, dbauj.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(4, codeJB, 0, dbauj.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(5, codeJB, 0, dbauj.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(6, codeJB, 0, dbauj.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(7, codeJB, 0, dbauj.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(8, codeJB, 0, tambahanRute + dbauj.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, null, sokontrak);

                //biaya borongan pada utang uang jalan driver sebanyak biaya yg terpakai
                string codeBTB = code + "-" + urutanBatal;
                decimal? kasbon = 0;
                if (dbauj.KasbonDriver1 > 0)
                    kasbon = dbauj.KasbonDriver1;
                Repoglt_det.saveFromAc(16, codeBTB, sementaraTunai, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, null, sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(17, codeBTB, dbsettlement.SolarKembali, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalanSementaraSolar), dbitem, null, sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(18, codeBTB, kasbon, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, "Kasbon", sokontrak);//PIUTANG DRIVER BATAL JALAN
                dbsettlement.BatalTruckCode = sokontrak.NoSo + "-" + urutanBatal;
                RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Truk");
            }
        }

        [HttpPost]
        public ActionResult EditKontrak(AdminUangJalan model, string Keterangan)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            Context.SalesOrderKontrakListSo sokontrak = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == int.Parse(model.ListIdSo)).FirstOrDefault();
            int jumSo = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == sokontrak.IdAdminUangJalan && d.IdDataTruck == sokontrak.IdDataTruck).Count();
            int jumSoBatal = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == sokontrak.IdAdminUangJalan && d.MuatDate >= sokontrak.MuatDate && d.IdDataTruck == sokontrak.IdDataTruck).Count();
            int jumSoJalan = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == sokontrak.IdAdminUangJalan && d.MuatDate < sokontrak.MuatDate && d.IdDataTruck == sokontrak.IdDataTruck).Count();

            Context.BatalOrder batalOrder = new Context.BatalOrder();
            Context.AdminUangJalan dbauj = sokontrak.AdminUangJalan;
            if (dbauj == null)
            {
                dbauj = RepoAdminUangJalan.FindAll().Where(d => d.SONumber != null && d.SONumber.Contains(sokontrak.NoSo) && d.Status != "Batal").FirstOrDefault();
                if (dbauj == null)
                {
                    jumSo = 0;
                    jumSoBatal = 0;
                    jumSoJalan = 0;
                }
                else
                {
                    jumSo = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => dbauj.SONumber.Contains(d.NoSo) && d.IdDataTruck == sokontrak.IdDataTruck).Count();
                    jumSoBatal = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => dbauj.SONumber.Contains(d.NoSo) && d.MuatDate >= sokontrak.MuatDate && d.IdDataTruck == sokontrak.IdDataTruck).Count();
                    jumSoJalan = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => dbauj.SONumber.Contains(d.NoSo) && d.MuatDate < sokontrak.MuatDate && d.IdDataTruck == sokontrak.IdDataTruck).Count();
                }
            }
            Context.AdminUangJalan dummyAdminUangJalan = dbauj;
            if (dummyAdminUangJalan != null)
            {
                dummyAdminUangJalan.Status = "Batal";
                queryManual("UPDATE dbo.\"AdminUangJalan\" SET \"Status\"='Batal' WHERE \"Id\"=" + dummyAdminUangJalan.Id);
            }

            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            string code = "BTB-" + sokontrak.NoSo;
            int? urutanBatal = Repopbyd_det.getUrutan(sokontrak.NoSo) + 1;
            //cek status na
            if (sokontrak.Status == "dispatched")
            {
                Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                //ambil data 
                dbsettlement.IdSalesOrder = model.IdSalesOrder;
                dbsettlement.IdSoKontrak = model.ListIdSo;
                decimal? KasDiterima = 0;
                decimal? TransferDiterima = 0;
                if (dbauj.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai" && d.isTf == true))
                    KasDiterima = dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai" && d.isTf == true).FirstOrDefault().JumlahTransfer;// -boronganPerHari * jumSoJalan;
                dbsettlement.KasDiterima = KasDiterima;
                if (dbauj.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer") && d.isTf == true))
                    TransferDiterima = dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer") && d.isTf == true).Sum(t => t.JumlahTransfer);// -boronganPerHari * jumSoJalan;
                dbsettlement.TransferDiterima = TransferDiterima;
                dbsettlement.SolarDiterima = dbauj.AdminUangJalanVoucherSpbu.Sum(s => s.Value);// -boronganPerHari * jumSoJalan;
                dbsettlement.KapalDiterima = dbauj.AdminUangJalanVoucherKapal.Sum(s => s.Value);// -boronganPerHari * jumSoJalan;
                dbsettlement.JenisBatal = "Batal Truk";
                dbsettlement.IdDriver = sokontrak.Driver1Id;
                dbsettlement.jumSo = jumSo;
                dbsettlement.jumSoBatal = jumSoBatal;
                dbsettlement.jumSoJalan = jumSoJalan;
                dbsettlement.IdAdminUangJalan = dummyAdminUangJalan.Id;
                decimal? TotalBiaya = (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalBorongan - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                dbsettlement.KasDiakui = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.TransferDiakui = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan != "Tunai").Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.SolarDiakui = dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(e => e.Value) * jumSoJalan / jumSo;
                dbsettlement.KapalDiakui = dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(e => e.Value) * jumSoJalan / jumSo;
                dbsettlement.KasKembali = KasDiterima - dbsettlement.KasDiakui;
                dbsettlement.TransferKembali = TransferDiterima - dbsettlement.TransferDiakui;
                dbsettlement.SolarKembali = dbsettlement.SolarDiterima - dbsettlement.SolarDiakui;
                dbsettlement.KapalKembali = dbsettlement.KapalDiterima - dbsettlement.KapalDiakui;
                dbsettlement.SPBUKembali = "SO: " + String.Join(", ", dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == sokontrak.IdAdminUangJalan && d.IdDataTruck == sokontrak.IdDataTruck).Select(e => e.NoSo)) + "AUJID: " + sokontrak.IdAdminUangJalan;
                decimal? sementaraTunai = 0;
                if (dbsettlement.KasKembali != null)
                    sementaraTunai = sementaraTunai + dbsettlement.KasKembali;
                if (dbsettlement.TransferKembali != null)
                    sementaraTunai = sementaraTunai + dbsettlement.TransferKembali;
                if (dbsettlement.KapalKembali != null)
                    sementaraTunai = sementaraTunai + dbsettlement.KapalKembali;

                //hutang uang jalan pada biaya borongan, jurnal balik
                string codeJB = "BJ-" + sokontrak.NoSo + "-" + urutanBatal;
                Repoglt_det.saveFromAc(1, codeJB, TotalBiaya, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(2, codeJB, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(3, codeJB, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(4, codeJB, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(5, codeJB, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(6, codeJB, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(7, codeJB, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(8, codeJB, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, null, sokontrak);

                //biaya borongan pada utang uang jalan driver sebanyak biaya yg terpakai
                string codeBTB = code + "-" + urutanBatal;
                decimal? kasbon = 0;
                decimal TotalUtangDriver = (dbsettlement.SolarKembali == null ? 0 : dbsettlement.SolarKembali.Value) + (dbsettlement.KapalKembali == null ? 0 : dbsettlement.KapalKembali.Value) + (dbsettlement.TransferKembali == null ? 0 : dbsettlement.TransferKembali.Value) + (dbsettlement.KasKembali == null ? 0 : dbsettlement.KasKembali.Value);
                var glt_oid = Guid.NewGuid().ToString();
                if (dummyAdminUangJalan.KasbonDriver1 > 0)
                {
                    string codePby = "PK-" + codeBTB;
                    kasbon = dummyAdminUangJalan.KasbonDriver1 / jumSo * jumSoBatal;
                    Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + code, dbsettlement.IdDriver.Value + 7000000);
                    Repopbyd_det.save(
                        glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + code, dbsettlement.IdDriver.Value + 7000000, erpConfig.IdAUJCredit.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, kasbon.Value
                    );
                }
                Repoglt_det.saveFromAc(9, codeBTB, boronganDasar / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(10, codeBTB, dummyAdminUangJalan.Kawalan / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(11, codeBTB, dummyAdminUangJalan.Timbangan / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(12, codeBTB, dummyAdminUangJalan.Karantina / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(13, codeBTB, dummyAdminUangJalan.SPSI / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(14, codeBTB, dummyAdminUangJalan.Multidrop / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(15, codeBTB, (tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values)) / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(16, codeBTB, sementaraTunai, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, null, sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(17, codeBTB, dbsettlement.SolarKembali, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalanSementaraSolar), dbitem, null, sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(18, codeBTB, kasbon, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, "Kasbon", sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(19, codeBTB, 0, TotalBiaya / jumSo * jumSoJalan + sementaraTunai + dbsettlement.SolarKembali + kasbon, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, sokontrak);
                dbsettlement.BatalTruckCode = sokontrak.NoSo + "-" + urutanBatal;
                RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Truk");
                glt_oid = Guid.NewGuid().ToString();
                Repopbyd_det.saveMstr(glt_oid, codeBTB, erpConfig.IdAUJCredit.Value, "Batal Truk " + code, dbsettlement.IdDriver.Value + 7000000);
                Repopbyd_det.save(
                    glt_oid, codeBTB, erpConfig.IdAUJCredit.Value, "Batal Truk " + code, dbsettlement.IdDriver.Value + 7000000, erpConfig.IdAUJCredit.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, TotalUtangDriver
                );
            }
            else if (sokontrak.Status == "admin uang jalan")
            {
                string codeBTB = code + "-" + urutanBatal;
                var glt_oid = Guid.NewGuid().ToString();
                if (dummyAdminUangJalan.KasbonDriver1 > 0)
                {
                    string codePby = "PK-" + codeBTB;
                    Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + code, dummyAdminUangJalan.IdDriver1.Value + 7000000);
                    Repopbyd_det.save(
                        glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + code, dummyAdminUangJalan.IdDriver1.Value + 7000000, erpConfig.IdAUJCredit.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, dummyAdminUangJalan.KasbonDriver1.Value
                    );
                }
                foreach (Context.AdminUangJalanUangTf dbUang in dbauj.AdminUangJalanUangTf)
                {
                    dbUang.Value = dbUang.Value / jumSo * (jumSo-1);
                    RepoSalesOrder.saveUangTf(dbUang);
                }
                //lebah dieu sync ERPna
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalAlokasi - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                string btCode = code + "-" + urutanBatal;
                Repoglt_det.saveFromAc(8, code, dummyAdminUangJalan.TotalBorongan, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, sokontrak);
                if (DateTime.Now.Month.ToString("00").PadLeft(2, '0') == sokontrak.MuatDate.Month.ToString("00").PadLeft(2, '0'))
                {
                    Repoglt_det.saveFromAc(1, code, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, sokontrak);
                    Repoglt_det.saveFromAc(2, code, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, sokontrak);
                    Repoglt_det.saveFromAc(3, code, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, sokontrak);
                    Repoglt_det.saveFromAc(4, code, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, sokontrak);
                    Repoglt_det.saveFromAc(5, code, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, sokontrak);
                    Repoglt_det.saveFromAc(6, code, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, sokontrak);
                    Repoglt_det.saveFromAc(7, code, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, null, sokontrak);
                }
                else
                    Repoglt_det.saveFromAc(2, code, 0, dummyAdminUangJalan.TotalBorongan, Repoac_mstr.FindByPk(erpConfig.IdPendapatanPosSementara), dbitem, null, sokontrak);
            }
            bool menginjakAkhirBulan = false;
            Context.SalesOrderKontrakListSo nextMonthKontrak = RepoSalesOrderKontrakListSo.FindSONextMonth(sokontrak);
            if (menginjakAkhirBulan && nextMonthKontrak != null)
                IkutanBatal(nextMonthKontrak);
            List<Context.SalesOrderKontrakListSo> sokontrakYangbatal = null;
            if (sokontrak.Status == "admin uang jalan" || sokontrak.Status == "dispatched")
                sokontrakYangbatal = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == sokontrak.IdAdminUangJalan && d.IdDataTruck == sokontrak.IdDataTruck).ToList();
            else
                sokontrakYangbatal = dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Urutan == sokontrak.Urutan && d.IdDataTruck == sokontrak.IdDataTruck).ToList();
            foreach (var sokon in sokontrakYangbatal)
            {
                if (sokon.MuatDate >= sokontrak.MuatDate)
                {
                    sokon.IsProses = false;
                    sokon.Urutan = 0;
                    sokon.IsBatalTruck = true;
                    sokon.Status = "draft planning";
                    sokon.StatusFlow = "PLANNING";
                    sokon.IdAdminUangJalan = null;
                    if (sokon.MuatDate.AddDays(1).Month > sokon.MuatDate.Month)
                        menginjakAkhirBulan = true;
                }
                else
                    sokon.IsKenaImbasBatalTruck = true;
            }
            batalOrder.IdSalesOrder = dbitem.Id;
            batalOrder.IdSoKontrak = model.ListIdSo;
            batalOrder.Keterangan = Keterangan;
            batalOrder.ModifiedDate = DateTime.Now;
            batalOrder.TanggalBatal = sokontrak.MuatDate;
            batalOrder.IsBatalTruk = true;
            batalOrder.UrutanBatal = urutanBatal;
            batalOrder.Code = code;
            RepoSalesOrder.save(dbitem, 9, String.Join(",", dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Urutan == sokontrak.Urutan && d.IdDataTruck == sokontrak.IdDataTruck && d.MuatDate >= sokontrak.MuatDate).Select(e => e.Id)));
            RepoBatalOrder.save(batalOrder, UserPrincipal.id);

            ViewBag.status = "batal order";
            return RedirectToAction("IndexKontrak", "ListOrder");
        }

        public ActionResult NyusulinKPD(int AdminUangJalanId, int soklsId)
        {
            Context.AdminUangJalan dbauj = RepoAdminUangJalan.FindByPK(AdminUangJalanId);
            Context.SalesOrderKontrakListSo sokontrak = RepoSalesOrderKontrakListSo.FindByPK(soklsId);
            int jumSo = 2;
            int jumSoBatal = 2;
            int jumSoJalan = 0;

            Context.BatalOrder batalOrder = new Context.BatalOrder();
            Context.AdminUangJalan dummyAdminUangJalan = dbauj;
            if (dummyAdminUangJalan != null)
                dummyAdminUangJalan.Status = "Batal";

            Context.ERPConfig erpConfig = RepoERPConfig.FindByFrist();
            string code = "BTB-" + sokontrak.NoSo;
            int? urutanBatal = RepoBatalOrder.FindAll().Where(d => d.Code == code).Count() == 0 ? 1 : RepoBatalOrder.FindAll().Where(d => d.Code == code).Max(d => d.UrutanBatal) + 1;
            //cek status na
            Context.SalesOrder dbitem = RepoSalesOrder.FindByKontrak(sokontrak.SalesKontrakId.Value);
            if (sokontrak.Status == "dispatched")
            {
                Context.SettlementBatal dbsettlement = new Context.SettlementBatal();
                dbsettlement.IdSalesOrder = dbitem.Id;
                dbsettlement.IdSoKontrak = sokontrak.Id.ToString();
                decimal? KasDiterima = 0;
                decimal? TransferDiterima = 0;
                if (dbauj.AdminUangJalanUangTf.Any(d => d.Keterangan == "Tunai" && d.isTf == true))
                    KasDiterima = dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai" && d.isTf == true).FirstOrDefault().JumlahTransfer;// -boronganPerHari * jumSoJalan;
                dbsettlement.KasDiterima = KasDiterima;
                if (dbauj.AdminUangJalanUangTf.Any(d => d.Keterangan.Contains("Transfer") && d.isTf == true))
                    TransferDiterima = dbauj.AdminUangJalanUangTf.Where(d => d.Keterangan.Contains("Transfer") && d.isTf == true).Sum(t => t.JumlahTransfer);// -boronganPerHari * jumSoJalan;
                dbsettlement.TransferDiterima = TransferDiterima;
                dbsettlement.SolarDiterima = dbauj.AdminUangJalanVoucherSpbu.Sum(s => s.Value);// -boronganPerHari * jumSoJalan;
                dbsettlement.KapalDiterima = dbauj.AdminUangJalanVoucherKapal.Sum(s => s.Value);// -boronganPerHari * jumSoJalan;
                dbsettlement.JenisBatal = "Batal Truk";
                dbsettlement.IdDriver = sokontrak.Driver1Id;
                dbsettlement.jumSo = jumSo;
                dbsettlement.jumSoBatal = jumSoBatal;
                dbsettlement.jumSoJalan = jumSoJalan;
                dbsettlement.IdAdminUangJalan = dummyAdminUangJalan.Id;
                decimal? TotalBiaya = (dummyAdminUangJalan.KasbonDriver1 ?? 0) + (dummyAdminUangJalan.KlaimDriver1 ?? 0) + dummyAdminUangJalan.AdminUangJalanPotonganDriver.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanVoucherKapal.Select(d => d.Value).Sum() + dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Select(d => d.Value).Sum() +
                    dummyAdminUangJalan.AdminUangJalanUangTf.Select(d => d.Value).Sum();
                decimal? tambahanRute = dummyAdminUangJalan.AdminUangJalanTambahanRute.Sum(s => s.values);
                decimal? boronganDasar = dummyAdminUangJalan.TotalBorongan - dummyAdminUangJalan.Kawalan - dummyAdminUangJalan.Timbangan - dummyAdminUangJalan.Karantina - dummyAdminUangJalan.SPSI - dummyAdminUangJalan.Multidrop - tambahanRute - dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values);
                dbsettlement.KasDiakui = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan == "Tunai").Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.TransferDiakui = dummyAdminUangJalan.AdminUangJalanUangTf.Where(d => d.Keterangan != "Tunai").Sum(e => e.Value) / jumSo * jumSoJalan;
                dbsettlement.SolarDiakui = dummyAdminUangJalan.AdminUangJalanVoucherSpbu.Sum(e => e.Value) * jumSoJalan / jumSo;
                dbsettlement.KapalDiakui = dummyAdminUangJalan.AdminUangJalanVoucherKapal.Sum(e => e.Value) * jumSoJalan / jumSo;
                dbsettlement.KasKembali = KasDiterima - dbsettlement.KasDiakui;
                dbsettlement.TransferKembali = TransferDiterima - dbsettlement.TransferDiakui;
                dbsettlement.SolarKembali = dbsettlement.SolarDiterima - dbsettlement.SolarDiakui;
                dbsettlement.KapalKembali = dbsettlement.KapalDiterima - dbsettlement.KapalDiakui;
                dbsettlement.SPBUKembali = "SO: " + String.Join(", ", dbitem.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == sokontrak.IdAdminUangJalan && d.IdDataTruck == sokontrak.IdDataTruck).Select(e => e.NoSo)) + "AUJID: " + sokontrak.IdAdminUangJalan;
                decimal? sementaraTunai = 0;
                if (dbsettlement.KasKembali != null)
                    sementaraTunai = sementaraTunai + dbsettlement.KasKembali;
                if (dbsettlement.TransferKembali != null)
                    sementaraTunai = sementaraTunai + dbsettlement.TransferKembali;
                if (dbsettlement.KapalKembali != null)
                    sementaraTunai = sementaraTunai + dbsettlement.KapalKembali;

                //hutang uang jalan pada biaya borongan, jurnal balik
                string codeJB = "BJ-" + sokontrak.NoSo + "-" + urutanBatal;
                Repoglt_det.saveFromAc(1, codeJB, TotalBiaya, 0, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(2, codeJB, 0, boronganDasar, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(3, codeJB, 0, dummyAdminUangJalan.Kawalan, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(4, codeJB, 0, dummyAdminUangJalan.Timbangan, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(5, codeJB, 0, dummyAdminUangJalan.Karantina, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(6, codeJB, 0, dummyAdminUangJalan.SPSI, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(7, codeJB, 0, dummyAdminUangJalan.Multidrop, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(8, codeJB, 0, tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values), Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, null, sokontrak);

                //biaya borongan pada utang uang jalan driver sebanyak biaya yg terpakai
                string codeBTB = code + "-" + urutanBatal;
                decimal? kasbon = 0;
                decimal TotalUtangDriver = (dbsettlement.SolarKembali == null ? 0 : dbsettlement.SolarKembali.Value) + (dbsettlement.KapalKembali == null ? 0 : dbsettlement.KapalKembali.Value) + (dbsettlement.TransferKembali == null ? 0 : dbsettlement.TransferKembali.Value) + (dbsettlement.KasKembali == null ? 0 : dbsettlement.KasKembali.Value);
                var glt_oid = Guid.NewGuid().ToString();
                if (dummyAdminUangJalan.KasbonDriver1 > 0)
                {
                    string codePby = "PK-" + codeBTB;
                    kasbon = dummyAdminUangJalan.KasbonDriver1 / jumSo * jumSoBatal;
                    Repopbyd_det.saveMstr(glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + code, dbsettlement.IdDriver.Value + 7000000);
                    Repopbyd_det.save(
                        glt_oid, codePby, erpConfig.IdAUJCredit.Value, "Pembatalan Kasbon " + code, dbsettlement.IdDriver.Value + 7000000, erpConfig.IdAUJCredit.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, kasbon.Value
                    );
                }
                Repoglt_det.saveFromAc(9, codeBTB, boronganDasar / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdBoronganDasar), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(10, codeBTB, dummyAdminUangJalan.Kawalan / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKawalan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(11, codeBTB, dummyAdminUangJalan.Timbangan / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdTimbangan), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(12, codeBTB, dummyAdminUangJalan.Karantina / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdKarantina), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(13, codeBTB, dummyAdminUangJalan.SPSI / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdSPSI), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(14, codeBTB, dummyAdminUangJalan.Multidrop / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdMultidrop), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(15, codeBTB, (tambahanRute + dummyAdminUangJalan.AdminUangJalanTambahanLain.Sum(s => s.Values)) / jumSo * jumSoJalan, 0, Repoac_mstr.FindByPk(erpConfig.IdTambahanRuteMuat), dbitem, null, sokontrak);
                Repoglt_det.saveFromAc(16, codeBTB, sementaraTunai, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, null, sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(17, codeBTB, dbsettlement.SolarKembali, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalanSementaraSolar), dbitem, null, sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(18, codeBTB, kasbon, 0, Repoac_mstr.FindByPk(erpConfig.IdPiutangDriverBatalJalan), dbitem, "Kasbon", sokontrak);//PIUTANG DRIVER BATAL JALAN
                Repoglt_det.saveFromAc(19, codeBTB, 0, TotalBiaya / jumSo * jumSoJalan + sementaraTunai + dbsettlement.SolarKembali + kasbon, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit), dbitem, null, sokontrak);
                dbsettlement.BatalTruckCode = sokontrak.NoSo + "-" + urutanBatal;
                RepoSettBatal.save(dbsettlement, UserPrincipal.id, "Batal Truk");
                glt_oid = Guid.NewGuid().ToString();
                Repopbyd_det.saveMstr(glt_oid, codeBTB, erpConfig.IdAUJCredit.Value, "Batal Truk " + code, dbsettlement.IdDriver.Value + 7000000);
                Repopbyd_det.save(
                    glt_oid, codeBTB, erpConfig.IdAUJCredit.Value, "Batal Truk " + code, dbsettlement.IdDriver.Value + 7000000, erpConfig.IdAUJCredit.Value, Repoac_mstr.FindByPk(erpConfig.IdAUJCredit).ac_name, TotalUtangDriver
                );
            }

            ViewBag.status = "batal order";
            return RedirectToAction("IndexKontrak", "ListOrder");
        }
    }
}