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
    public class ERPConfigController : BaseController
    {
        private IERPConfigRepo RepoERPConfig;
        private Iac_mstrRepo Repoac_mstr;
        private Ibk_mstrRepo Repobk_mstr;
        private ILookupCodeRepo RepoLookupCode;
        private IMasterPoolRepo RepoMasterPool;
        private IUserRepo RepoUser;
        public ERPConfigController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IERPConfigRepo repoERPConfig, Iac_mstrRepo repoac_mstr, Ibk_mstrRepo repobk_mstr,
            ILookupCodeRepo repoLookupCode, IMasterPoolRepo repoMasterPool, IUserRepo repoUser)
            : base(repoBase, repoLookup)
        {
            RepoUser = repoUser;
            RepoERPConfig = repoERPConfig;
            Repoac_mstr = repoac_mstr;
            Repobk_mstr = repobk_mstr;
            RepoLookupCode = repoLookupCode;
            RepoMasterPool = repoMasterPool;
        }

        public void save(int id, int ac_id, string type){
            Context.ERPDynamicConfig eRPDynamicConfig = RepoERPConfig.FindByIdAndType(id, type);
            RepoERPConfig.saveDynamic(id, ac_id, type);
        }

        public string select(int id, int ac_id, string type){
            Context.ERPDynamicConfig eRPDynamicConfig = RepoERPConfig.FindByIdAndType(id, type);
            ac_mstr ac = Repoac_mstr.FindByPk(ac_id);
            return new JavaScriptSerializer().Serialize(new { id = id, ac_id = ac.id, ac_code = ac.ac_code, ac_name = ac.ac_name });
        }

        public ActionResult Dynamic()
        {
            ViewBag.LookupCodesSPBU = RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_SPBU").ToList();
            ViewBag.LookupCodesKapal = RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_penyebrangan").ToList();
            ViewBag.Pool = RepoMasterPool.FindAll();
            ViewBag.BiayaTambahan = RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_biaya_tambahan_settlement_reguler").ToList();

            return View("Dynamic");
        }

        public string BindingAcc()
        {
            List<ac_mstr> items = Repoac_mstr.FindAll();

            return new JavaScriptSerializer().Serialize(new { total = items.Count, data = items });
        }
        public string BindingBk()
        {
            List<bk_mstr> items = Repobk_mstr.FindAll();

            return new JavaScriptSerializer().Serialize(new { total = items.Count, data = items });
        }
        [MyAuthorize(Menu = "ERP Config", Action="update")]
        public ActionResult Edit()
        {
            ERPConfig model = new ERPConfig();
            Context.ERPConfig dbitem = RepoERPConfig.FindByFrist();
            if (dbitem != null)
            {
                if (dbitem.IdDP.HasValue)
                {
                    ac_mstr ac_mstr_DP = Repoac_mstr.FindByPk(dbitem.IdDP);
                    model.IdDP = ac_mstr_DP.id;
                    model.CodeDP = ac_mstr_DP.ac_code;
                    model.NamaDP = ac_mstr_DP.ac_name;
                }

                if (dbitem.IdBiayaPerjalanan.HasValue)
                {
                    ac_mstr ac_mstr_BiayaPerjalanan = Repoac_mstr.FindByPk(dbitem.IdBiayaPerjalanan);
                    model.IdBiayaPerjalanan = ac_mstr_BiayaPerjalanan.id;
                    model.CodeBiayaPerjalanan = ac_mstr_BiayaPerjalanan.ac_code;
                    model.NamaBiayaPerjalanan = ac_mstr_BiayaPerjalanan.ac_name;
                }

                if (dbitem.IdBiayaInap.HasValue)
                {
                    ac_mstr ac_mstr_BiayaInap = Repoac_mstr.FindByPk(dbitem.IdBiayaInap);
                    model.IdBiayaInap = ac_mstr_BiayaInap.id;
                    model.CodeBiayaInap = ac_mstr_BiayaInap.ac_code;
                    model.NamaBiayaInap = ac_mstr_BiayaInap.ac_name;
                }

                if (dbitem.IdKasbonDriver.HasValue)
                {
                    ac_mstr ac_mstr_KasbonDriver = Repoac_mstr.FindByPk(dbitem.IdKasbonDriver);
                    model.IdKasbonDriver = ac_mstr_KasbonDriver.id;
                    model.CodeKasbonDriver = ac_mstr_KasbonDriver.ac_code;
                    model.NamaKasbonDriver = ac_mstr_KasbonDriver.ac_name;
                }

                if (dbitem.IdVoucherKapal.HasValue)
                {
                    ac_mstr ac_mstr_VoucherKapal = Repoac_mstr.FindByPk(dbitem.IdVoucherKapal);
                    model.IdVoucherKapal = ac_mstr_VoucherKapal.id;
                    model.CodeVoucherKapal = ac_mstr_VoucherKapal.ac_code;
                    model.NamaVoucherKapal = ac_mstr_VoucherKapal.ac_name;
                }

                if (dbitem.IdVoucherSolar.HasValue)
                {
                    ac_mstr ac_mstr_VoucherSolar = Repoac_mstr.FindByPk(dbitem.IdVoucherSolar);
                    model.IdVoucherSolar = ac_mstr_VoucherSolar.id;
                    model.CodeVoucherSolar = ac_mstr_VoucherSolar.ac_code;
                    model.NamaVoucherSolar = ac_mstr_VoucherSolar.ac_name;
                }

                if (dbitem.IdBoronganDasar.HasValue)
                {
                    ac_mstr ac_mstr_BoronganDasar = Repoac_mstr.FindByPk(dbitem.IdBoronganDasar);
                    model.IdBoronganDasar = ac_mstr_BoronganDasar.id;
                    model.CodeBoronganDasar = ac_mstr_BoronganDasar.ac_code;
                    model.NamaBoronganDasar = ac_mstr_BoronganDasar.ac_name;
                }

                if (dbitem.IdKawalan.HasValue)
                {
                    ac_mstr ac_mstr_kawalan = Repoac_mstr.FindByPk(dbitem.IdKawalan);
                    model.IdKawalan = ac_mstr_kawalan.id;
                    model.CodeKawalan = ac_mstr_kawalan.ac_code;
                    model.NamaKawalan = ac_mstr_kawalan.ac_name;
                }

                if (dbitem.IdTimbangan.HasValue)
                {
                    ac_mstr ac_mstr_timbangan = Repoac_mstr.FindByPk(dbitem.IdTimbangan);
                    model.IdTimbangan = ac_mstr_timbangan.id;
                    model.CodeTimbangan = ac_mstr_timbangan.ac_code;
                    model.NamaTimbangan = ac_mstr_timbangan.ac_name;
                }

                if (dbitem.IdKarantina.HasValue)
                {
                    ac_mstr ac_mstr_karantina = Repoac_mstr.FindByPk(dbitem.IdKarantina);
                    model.IdKaarantina = ac_mstr_karantina.id;
                    model.CodeKaarantina = ac_mstr_karantina.ac_code;
                    model.NamaKaarantina = ac_mstr_karantina.ac_name;
                }

                if (dbitem.IdSPSI.HasValue)
                {
                    ac_mstr ac_mstr_spsi = Repoac_mstr.FindByPk(dbitem.IdSPSI);
                    model.IdSPSI = ac_mstr_spsi.id;
                    model.CodeSPSI = ac_mstr_spsi.ac_code;
                    model.NamaSPSI = ac_mstr_spsi.ac_name;
                }

                if (dbitem.IdMultidrop.HasValue)
                {
                    ac_mstr ac_mstr_multidrop = Repoac_mstr.FindByPk(dbitem.IdMultidrop);
                    model.IdMultidrop = ac_mstr_multidrop.id;
                    model.CodeMultidrop = ac_mstr_multidrop.ac_code;
                    model.NamaMultidrop = ac_mstr_multidrop.ac_name;
                }

                if (dbitem.IdTambahanRuteMuat.HasValue)
                {
                    ac_mstr ac_mstr_tambahan_rute_muat = Repoac_mstr.FindByPk(dbitem.IdTambahanRuteMuat);
                    model.IdRutemuat = ac_mstr_tambahan_rute_muat.id;
                    model.CodeRutemuat = ac_mstr_tambahan_rute_muat.ac_code;
                    model.NamaRutemuat = ac_mstr_tambahan_rute_muat.ac_name;
                }

                if (dbitem.IdTambahanRuteLain.HasValue)
                {
                    ac_mstr ac_mstr_tambahan_rute_lain = Repoac_mstr.FindByPk(dbitem.IdTambahanRuteLain);
                    model.IdRuteLain = ac_mstr_tambahan_rute_lain.id;
                    model.CodeRuteLain = ac_mstr_tambahan_rute_lain.ac_code;
                    model.NamaRuteLain = ac_mstr_tambahan_rute_lain.ac_name;
                }

                if (dbitem.IdKasirCash.HasValue)
                {
                    ac_mstr ac_mstr_kasir_cash = Repoac_mstr.FindByPk(dbitem.IdKasirCash);
                    model.IdOprCash = ac_mstr_kasir_cash.id;
                    model.CodeOprCash = ac_mstr_kasir_cash.ac_code;
                    model.NamaOprCash = ac_mstr_kasir_cash.ac_name;
                }
                
                if (dbitem.IdKasirTf.HasValue)
                {
                    ac_mstr ac_mstr_kasir_tf = Repoac_mstr.FindByPk(dbitem.IdKasirTf);
                    model.IdOprTf = ac_mstr_kasir_tf.id;
                    model.CodeOprTf = ac_mstr_kasir_tf.ac_code;
                    model.NamaOprTf = ac_mstr_kasir_tf.ac_name;
                }

                //credit
                if (dbitem.IdAUJCredit.HasValue)
                {
                    ac_mstr ac_credit_auj = Repoac_mstr.FindByPk(dbitem.IdAUJCredit);
                    model.IdCreditAuj = ac_credit_auj.id;
                    model.CodeCreditAuj = ac_credit_auj.ac_code;
                    model.NamaCreditAuj = ac_credit_auj.ac_name;
                }
                if (dbitem.IdCashCredit.HasValue)
                {
                    ac_mstr bk_credit_cash = Repoac_mstr.FindByPk(dbitem.IdCashCredit);
                    model.IdCreditCash = bk_credit_cash.id;
                    model.CodeCreditCash = bk_credit_cash.ac_code;
                    model.NamaCreditCash = bk_credit_cash.ac_name;
                }
                if (dbitem.IdTfCredit.HasValue)
                {
                    ac_mstr bk_credit_tf = Repoac_mstr.FindByPk(dbitem.IdTfCredit);
                    model.IdCreditTf = bk_credit_tf.id;
                    model.CodeCreditTf = bk_credit_tf.ac_code;
                    model.NamaCreditTf = bk_credit_tf.ac_name;
                }
                if (dbitem.IdKasbonAuj.HasValue)
                {
                    ac_mstr kasbon_auj = Repoac_mstr.FindByPk(dbitem.IdKasbonAuj);
                    model.IdKasbonAuj = kasbon_auj.id;
                    model.CodeKasbonAuj = kasbon_auj.ac_code;
                    model.NamaKasbonAuj = kasbon_auj.ac_name;
                }
                if (dbitem.IdKlaimAuj.HasValue)
                {
                    ac_mstr klaim_auj = Repoac_mstr.FindByPk(dbitem.IdKlaimAuj);
                    model.IdKlaimAuj = klaim_auj.id;
                    model.CodeKlaimAuj = klaim_auj.ac_code;
                    model.NamaKlaimAuj = klaim_auj.ac_name;
                }
                if (dbitem.IdPotonganLainAuj.HasValue)
                {
                    ac_mstr potongan_lain_auj = Repoac_mstr.FindByPk(dbitem.IdPotonganLainAuj);
                    model.IdPotonganLainAuj = potongan_lain_auj.id;
                    model.CodePotonganLainAuj = potongan_lain_auj.ac_code;
                    model.NamaPotonganLainAuj = potongan_lain_auj.ac_name;
                }
                if (dbitem.IdPiutangCustomer.HasValue)
                {
                    ac_mstr piutangCustomer = Repoac_mstr.FindByPk(dbitem.IdPiutangCustomer);
                    model.IdPiutangCustomer = piutangCustomer.id;
                    model.CodePiutangCustomer = piutangCustomer.ac_code;
                    model.NamaPiutangCustomer = piutangCustomer.ac_name;
                }
                if (dbitem.IdBiayaKlaim.HasValue)
                {
                    ac_mstr biayaKlaim = Repoac_mstr.FindByPk(dbitem.IdBiayaKlaim);
                    model.IdBiayaKlaim = biayaKlaim.id;
                    model.CodeBiayaKlaim = biayaKlaim.ac_code;
                    model.NamaBiayaKlaim = biayaKlaim.ac_name;
                }
                if (dbitem.IdKreditKlaim.HasValue)
                {
                    ac_mstr kreditKlaim = Repoac_mstr.FindByPk(dbitem.IdKreditKlaim);
                    model.IdKreditKlaim = kreditKlaim.id;
                    model.CodeKreditKlaim = kreditKlaim.ac_code;
                    model.NamaKreditKlaim = kreditKlaim.ac_name;
                }
                if (dbitem.IdBiayaKlaim2.HasValue)
                {
                    ac_mstr biayaKlaim2 = Repoac_mstr.FindByPk(dbitem.IdBiayaKlaim2);
                    model.IdBiayaKlaim2 = biayaKlaim2.id;
                    model.CodeBiayaKlaim2 = biayaKlaim2.ac_code;
                    model.NamaBiayaKlaim2 = biayaKlaim2.ac_name;
                }
                if (dbitem.IdKreditKlaim2.HasValue)
                {
                    ac_mstr kreditKlaim2 = Repoac_mstr.FindByPk(dbitem.IdKreditKlaim2);
                    model.IdKreditKlaim2 = kreditKlaim2.id;
                    model.CodeKreditKlaim2 = kreditKlaim2.ac_code;
                    model.NamaKreditKlaim2 = kreditKlaim2.ac_name;
                }
                if (dbitem.IdPiutangDagang.HasValue)
                {
                    ac_mstr piutangBlmInv = Repoac_mstr.FindByPk(dbitem.IdPiutangDagang);
                    model.IdPiutangBlmInv = piutangBlmInv.id;
                    model.CodePiutangBlmInv = piutangBlmInv.ac_code;
                    model.NamaPiutangBlmInv = piutangBlmInv.ac_name;
                }
                if (dbitem.IdPendapatanUsahaBlmInv.HasValue)
                {
                    ac_mstr penjualanBlmInv = Repoac_mstr.FindByPk(dbitem.IdPendapatanUsahaBlmInv);
                    model.IdPenjualanBlmInv = penjualanBlmInv.id;
                    model.CodePenjualanBlmInv = penjualanBlmInv.ac_code;
                    model.NamaPenjualanBlmInv = penjualanBlmInv.ac_name;
                }
                if (dbitem.IdPiutangDailySupir.HasValue)
                {
                    ac_mstr piutangDailySupir = Repoac_mstr.FindByPk(dbitem.IdPiutangDailySupir);
                    model.IdPiutangDailySupir = piutangDailySupir.id;
                    model.CodePiutangDailySupir = piutangDailySupir.ac_code;
                    model.NamaPiutangDailySupir = piutangDailySupir.ac_name;
                }
                if (dbitem.IdKlaimSupir.HasValue)
                {
                    ac_mstr klaimSupir = Repoac_mstr.FindByPk(dbitem.IdKlaimSupir);
                    model.IdKlaimSupir = klaimSupir.id;
                    model.CodeKlaimSupir = klaimSupir.ac_code;
                    model.NamaKlaimSupir = klaimSupir.ac_name;
                }
            }

            return View("Form", model);
        }


/*        [HttpPost]
        public ActionResult Dynamic(string params)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            return View("Dynamic");
        }
*/
        [HttpPost]
        [MyAuthorize(Menu = "ERP Config", Action="update")]
        public ActionResult Dynamic(ERPConfig model)
        {
            return RedirectToAction("Edit");
        }


        [HttpPost]
        [MyAuthorize(Menu = "ERP Config", Action="update")]
        public ActionResult Edit(ERPConfig model)
        {
            Context.ERPConfig dbitem = RepoERPConfig.FindByFrist();
            if (dbitem == null)
                dbitem = new Context.ERPConfig();

            dbitem.IdKawalan = model.IdKawalan;
            dbitem.IdTimbangan = model.IdTimbangan;
            dbitem.IdKarantina = model.IdKaarantina;
            dbitem.IdSPSI = model.IdSPSI;
            dbitem.IdMultidrop = model.IdMultidrop;
            //dbitem.IdSolar = model.IdSolar;
            //dbitem.IdKapal = model.IdKapal;
            dbitem.IdTambahanRuteMuat = model.IdRutemuat;
            dbitem.IdTambahanRuteLain = model.IdRuteLain;
            dbitem.IdAUJCredit = model.IdCreditAuj;
            dbitem.IdKasirCash = model.IdOprCash;
            dbitem.IdCashCredit = model.IdCreditCash;
            dbitem.IdKasirTf = model.IdOprTf;
            dbitem.IdTfCredit = model.IdCreditTf;
            dbitem.IdKasbonAuj = model.IdKasbonAuj;
            dbitem.IdPotonganLainAuj = model.IdPotonganLainAuj;
            dbitem.IdKlaimAuj = model.IdKlaimAuj;
            dbitem.IdBoronganDasar = model.IdBoronganDasar;
            dbitem.IdVoucherSolar = model.IdVoucherSolar;
            dbitem.IdVoucherKapal = model.IdVoucherKapal;
            dbitem.IdKasbonDriver = model.IdKasbonDriver;
            dbitem.IdBiayaPerjalanan = model.IdBiayaPerjalanan;
            dbitem.IdBiayaInap = model.IdBiayaInap;
            dbitem.IdBiayaKlaim = model.IdBiayaKlaim;
            dbitem.IdKreditKlaim = model.IdKreditKlaim;
            dbitem.IdBiayaKlaim2 = model.IdBiayaKlaim2;
            dbitem.IdKreditKlaim2 = model.IdKreditKlaim2;
            dbitem.IdPiutangCustomer = model.IdPiutangCustomer;
            dbitem.IdDP = model.IdDP;
            dbitem.IdPiutangDagang = model.IdPiutangBlmInv;
            dbitem.IdPendapatanUsahaBlmInv = model.IdPenjualanBlmInv;
            dbitem.IdPiutangDailySupir = model.IdPiutangDailySupir;
            dbitem.IdKlaimSupir = model.IdKlaimSupir;

            RepoERPConfig.save(dbitem);
            return RedirectToAction("Edit");
        }
    }
}