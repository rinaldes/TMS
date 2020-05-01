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

namespace tms_mka_v2.Controllers
{
    public class AuthorizationRuleController : BaseController
    {

        private IAuthorizationRuleRepo RepoAuthorizationRule;

        public AuthorizationRuleController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IAuthorizationRuleRepo repoAuthorizationRule)
            : base(repoBase, repoLookup)
        {
            RepoAuthorizationRule = repoAuthorizationRule;
        }

        // GET: AuthorizationRule
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "AuthorizationRule").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.AuthorizationRule> items = RepoAuthorizationRule.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<AuthorizationRule> ListModel = new List<AuthorizationRule>();
            foreach (Context.AuthorizationRule item in items)
            {
                ListModel.Add(new AuthorizationRule(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = items.Count, data = ListModel });
        }

        [HttpPost]
        public JsonResult SaveAuthorizationRule(AuthorizationRule model)
        {
            Context.AuthorizationRule dbitem = RepoAuthorizationRule.FindByPK(model.Id);
            dbitem.idProses = model.idProses;
            dbitem.statusAktif = model.statusAktif;
            dbitem.idUserOtoritas1 = model.idUserOtoritas1;
            dbitem.idUserOtoritas2 = model.idUserOtoritas2;
            dbitem.frekuensi = model.frekuensi;
            dbitem.frekuensiSatuan = model.frekuensiSatuan;
            dbitem.AlertPopup = model.AlertPopup;
            dbitem.AlertFingerPrint = model.AlertFingerPrint;
            dbitem.AlertEmail = model.AlertEmail;
            dbitem.AlertPassword = model.AlertPassword;

            RepoAuthorizationRule.save(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.AuthorizationRule dbItem = RepoAuthorizationRule.FindByPK(id);

            RepoAuthorizationRule.delete(dbItem);

            return Json(response);
        }


    }
}