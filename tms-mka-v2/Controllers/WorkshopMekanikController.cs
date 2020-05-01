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
    public class WorkshopMekanikController : BaseController 
    {
        private IWorkshopMekanikRepo RepoWorkshopMekanik;
        private ISpkRepo RepoSpk;
        private IMekanikRepo RepoMekanik;
        public WorkshopMekanikController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IWorkshopMekanikRepo repoWorkshopMekanik, ISpkRepo repoSpk, IMekanikRepo repoMekanik)
            : base(repoBase, repoLookup)
        {
            RepoWorkshopMekanik = repoWorkshopMekanik;
            RepoMekanik = repoMekanik;
            RepoSpk = repoSpk;
        }
        [MyAuthorize(Menu = "Mekanik", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "WorkshopMekanik").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Spk> items = RepoSpk.FindAllOnProgress(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<WorkshopMekanik> ListModel = new List<WorkshopMekanik>();
            foreach (Context.Spk spk in items)
            {
                ListModel.Add(new WorkshopMekanik(spk));
            }

            int total = RepoSpk.CountOnProgress(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
    }
}