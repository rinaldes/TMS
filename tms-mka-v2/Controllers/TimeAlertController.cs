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
    public class TimeAlertController : BaseController
    {

        private ITimeAlertRepo RepoTimeAlert;

        public TimeAlertController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            ITimeAlertRepo repoTimeAlert)
            : base(repoBase, repoLookup)
        {
            RepoTimeAlert = repoTimeAlert;
        }

        // GET: TimeAlert
        [MyAuthorize(Menu = "Time Alert", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "TimeAlert").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.TimeAlert> items = RepoTimeAlert.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<TimeAlert> ListModel = new List<TimeAlert>();
            foreach (Context.TimeAlert item in items)
            {
                ListModel.Add(new TimeAlert(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = items.Count, data = ListModel });
        }

        [HttpPost]
        public JsonResult SaveTimeAlert(TimeAlert model)
        {
            Context.TimeAlert dbitem = new Context.TimeAlert();
            if(model.Id != 0)
            {
                dbitem = RepoTimeAlert.FindByPK(model.Id);
            }
            model.setDb(dbitem);
            RepoTimeAlert.save(dbitem, UserPrincipal.id);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        [HttpPost]
        [MyAuthorize(Menu = "Time Alert", Action="delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.TimeAlert dbItem = RepoTimeAlert.FindByPK(id);

            RepoTimeAlert.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
    }
}