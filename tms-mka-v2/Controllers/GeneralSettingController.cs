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
    public class GeneralSettingController : BaseController
    {
        private IGeneralSettingRepo RepoGeneralSetting;

        public GeneralSettingController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IGeneralSettingRepo repoGeneralSetting)
            : base(repoBase, repoLookup)
        {
            RepoGeneralSetting = repoGeneralSetting;
        }

        // GET: GeneralSetting
        [MyAuthorize(Menu = "General Setting", Action = "read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "GeneralSetting").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SettingGeneral> items = RepoGeneralSetting.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<SettingGeneral> ListModel = new List<SettingGeneral>();
            foreach (Context.SettingGeneral item in items)
            {
                ListModel.Add(new SettingGeneral(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = items.Count, data = ListModel });
        }

        [HttpPost]
        public JsonResult SaveGeneralSetting(SettingGeneral model)
        {
            Context.SettingGeneral dbitem = new Context.SettingGeneral();
            if (model.Id != 0)
            {
                dbitem = RepoGeneralSetting.FindByPK(model.Id);
            }
            dbitem.idProses = model.idProses;
            dbitem.keteranganBagian = model.keteranganBagian;
            dbitem.status = model.status;
            dbitem.idUserAlert = model.idUserAlert;
            dbitem.over = model.over;
            dbitem.overSatuan = model.overSatuan;
            dbitem.AlertPopup = model.AlertPopup;
            dbitem.AlertSound = model.AlertSound;
            dbitem.AlertEmail = model.AlertEmail;
            dbitem.rowColor = model.rowColor;
            RepoGeneralSetting.save(dbitem, UserPrincipal.id);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        [HttpPost]
        [MyAuthorize(Menu = "General Setting", Action = "delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.SettingGeneral dbItem = RepoGeneralSetting.FindByPK(id);

            RepoGeneralSetting.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }


    }
}