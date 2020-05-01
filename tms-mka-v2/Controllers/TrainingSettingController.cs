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
    public class TrainingSettingController : BaseController 
    {
        private ITrainingSettingRepo RepoTrainingSetting;
        public TrainingSettingController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            ITrainingSettingRepo repoTrainingSetting)
            : base(repoBase, repoLookup)
        {   
            RepoTrainingSetting = repoTrainingSetting;
        }
        [MyAuthorize(Menu = "Training Setting", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "TrainingSetting").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.TrainingSetting> items = RepoTrainingSetting.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<TrainingSetting> ListModel = new List<TrainingSetting>();
            foreach (Context.TrainingSetting item in items)
            {
                ListModel.Add(new TrainingSetting(item));
            }

            int total = RepoTrainingSetting.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [MyAuthorize(Menu = "Training Setting", Action="create")]
        public ActionResult Add()
        {
            TrainingSetting model = new TrainingSetting();
            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(TrainingSetting model)
        {
            if (ModelState.IsValid)
            {
                Context.TrainingSetting dbitem = new Context.TrainingSetting();
                model.setDb(dbitem);
                RepoTrainingSetting.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Training Setting", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.TrainingSetting dbitem = RepoTrainingSetting.FindByPK(id);
            TrainingSetting model = new TrainingSetting(dbitem);
            ViewBag.name = model.Nama;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(TrainingSetting model)
        {
            if (ModelState.IsValid)
            {
                Context.TrainingSetting dbitem = RepoTrainingSetting.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoTrainingSetting.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.TrainingSetting dbItem = RepoTrainingSetting.FindByPK(id);

            RepoTrainingSetting.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

    }
}