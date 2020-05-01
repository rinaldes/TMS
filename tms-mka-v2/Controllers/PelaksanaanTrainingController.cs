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
    public class PelaksanaanTrainingController : BaseController 
    {
        private IPelaksanaanTrainingRepo RepoPelaksanaanTraining;
        private IMasterPoolRepo RepoLocation;
        private ITrainingSettingRepo RepoTraining;
        private ITrainingSettingDetailRepo RepoTrainingDetail;
        public PelaksanaanTrainingController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IMasterPoolRepo repoLocation, ITrainingSettingRepo repoTraining, ITrainingSettingDetailRepo repoTrainingDetail, IPelaksanaanTrainingRepo repoPelaksanaanTraining)
            : base(repoBase, repoLookup)
        {   
            RepoPelaksanaanTraining = repoPelaksanaanTraining;
            RepoLocation = repoLocation;
            RepoTraining = repoTraining;
            RepoTrainingDetail = repoTrainingDetail;
        }
        [MyAuthorize(Menu = "Pelaksanaan Training", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "PelaksanaanTraining").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.PelaksanaanTraining> items = RepoPelaksanaanTraining.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<PelaksanaanTraining> ListModel = new List<PelaksanaanTraining>();
            foreach (Context.PelaksanaanTraining item in items)
            {
                ListModel.Add(new PelaksanaanTraining(item));
            }

            int total = RepoPelaksanaanTraining.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingItem(int id)
        {
            PelaksanaanTraining model = new PelaksanaanTraining(RepoPelaksanaanTraining.FindByPK(id));

            return new JavaScriptSerializer().Serialize(new { total = model.listPelaksanaanTraining.Count() , data = model.listPelaksanaanTraining });
        }
        [MyAuthorize(Menu = "Pelaksanaan Training", Action="create")]
        public ActionResult Add()
        {
            PelaksanaanTraining model = new PelaksanaanTraining();
            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(PelaksanaanTraining model)
        {
            PelaksanaanTrainingDetail[] result = JsonConvert.DeserializeObject<PelaksanaanTrainingDetail[]>(model.strPelaksanaanTrainingDetail);
            model.listPelaksanaanTraining = result.ToList();
            if (ModelState.IsValid)
            {
                Context.PelaksanaanTraining dbitem = new Context.PelaksanaanTraining();
                model.setDb(dbitem);
                RepoPelaksanaanTraining.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Pelaksanaan Training", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.PelaksanaanTraining dbitem = RepoPelaksanaanTraining.FindByPK(id);
            PelaksanaanTraining model = new PelaksanaanTraining(dbitem);
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(PelaksanaanTraining model)
        {
            PelaksanaanTrainingDetail[] resultPelaksanaanTrainingDetail = JsonConvert.DeserializeObject<PelaksanaanTrainingDetail[]>(model.strPelaksanaanTrainingDetail);
            model.listPelaksanaanTraining = resultPelaksanaanTrainingDetail.ToList();
            if (ModelState.IsValid)
            {
                Context.PelaksanaanTraining dbitem = RepoPelaksanaanTraining.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoPelaksanaanTraining.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.PelaksanaanTraining dbItem = RepoPelaksanaanTraining.FindByPK(id);
            dbItem.PelaksanaanTrainingDetail.Clear();
            RepoPelaksanaanTraining.delete(dbItem);

            return Json(response);
        }

        public string GetLocation()
        {
            List<Context.MasterPool> dbitems = new List<Context.MasterPool>();
            dbitems = RepoLocation.FindAll().ToList();
            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string GetTraining()
        {
            List<Context.TrainingSetting> dbitems = new List<Context.TrainingSetting>();
            dbitems = RepoTraining.FindAll().ToList();
            return new JavaScriptSerializer().Serialize(dbitems.Select(x => new
            {
                Id = x.Id,
                Name = x.Nama
            }));
        }
        public string GetMateri(int idTraining)
        {
           List<Context.TrainingSetting> dbitems = new List<Context.TrainingSetting>();
           dbitems = RepoTraining.FindAll().ToList();

           return new JavaScriptSerializer().Serialize(dbitems.Where(x => x.Id == idTraining).SelectMany(x => x.TrainingSettingDetail).ToList().Select(x => new
           {
               Id = x.Id,
               Name = x.Materi,
               NilaiMinimum = x.NilaiMinimum
           }));
        }
        public int GetNilaiMinimum(int idMateri)
        {
            Context.TrainingSettingDetail dbitems = new Context.TrainingSettingDetail();
            dbitems = RepoTrainingDetail.FindByPK(idMateri);

            return dbitems.NilaiMinimum;
        }

    }
}