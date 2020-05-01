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
    public class RoleController : BaseController
    {
        private IRoleRepo RepoRole;
        public RoleController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IRoleRepo repoRole)
            : base(repoBase, repoLookup)
        {
            RepoRole = repoRole;
        }
        [MyAuthorize(Menu = "Role", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Role").ToList();
            return View();
        }
        public string Binding()
        {
            List<Context.Role> items = RepoRole.FindAll();

            List<Role> ListModel = new List<Role>();
            foreach (Context.Role item in items)
            {
                ListModel.Add(new Role(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel, data = ListModel });
        }
        [MyAuthorize(Menu = "Role", Action="create")]
        public ActionResult Add()
        {
            Role model = new Role();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(Role model)
        {
            RoleMenu[] result = JsonConvert.DeserializeObject<RoleMenu[]>(model.StrMenu);
            model.ListMenu = result.ToList();
            if (ModelState.IsValid)
            {
                Context.Role dbitem = new Context.Role();
                model.setDb(dbitem);

                RepoRole.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Role", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.Role dbitem = RepoRole.FindByPK(id);
            Role model = new Role(dbitem);
            ViewBag.name = model.name;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(Role model)
        {
            RoleMenu[] result = JsonConvert.DeserializeObject<RoleMenu[]>(model.StrMenu);
            model.ListMenu = result.ToList();
            if (ModelState.IsValid)
            {
                Context.Role dbitem = RepoRole.FindByPK(model.id);
                model.setDb(dbitem);
                RepoRole.save(dbitem);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Role", Action="delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Role dbItem = RepoRole.FindByPK(id);

            RepoRole.delete(dbItem);

            return Json(response);
        }
    }
}