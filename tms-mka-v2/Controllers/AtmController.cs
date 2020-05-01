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
    public class AtmController : BaseController 
    {
        private IAtmRepo RepoAtm;
        private IDriverRepo RepoDriver;
        public AtmController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup,
            IAtmRepo repoAtm, IDriverRepo repoDriver)
            : base(repoBase, repoLookup)
        {   
            RepoAtm = repoAtm;
            RepoDriver = repoDriver;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "Atm").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Atm> items = RepoAtm.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Atm> ListModel = new List<Atm>();
            foreach (Context.Atm item in items)
            {
                ListModel.Add(new Atm(item));
            }

            int total = RepoAtm.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public ActionResult Add()
        {
            Atm model = new Atm();
            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(Atm model)
        {
            if (ModelState.IsValid)
            {
                Context.Atm dbitem = new Context.Atm();
                model.setDb(dbitem);
                RepoAtm.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.Atm dbitem = RepoAtm.FindByPK(id);
            Atm model = new Atm(dbitem);
            ViewBag.name = model.NamaBank;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(Atm model)
        {
            if (ModelState.IsValid)
            {
                Context.Atm dbitem = RepoAtm.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoAtm.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Atm dbItem = RepoAtm.FindByPK(id);

            RepoAtm.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        public string FindByDriver(int id)
        {
            Context.Atm items = RepoAtm.FindByDriver(id);
            Atm model = new Atm();
            if (items != null)
                model = new Atm(items);

            return new JavaScriptSerializer().Serialize(model);
        }
        #region upload
        public string UploadATM(IEnumerable<HttpPostedFileBase> filesAtm)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesAtm != null)
            {
                foreach (var file in filesAtm)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && 
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null && 
                                    workSheet.Cells[rowIterator, 5].Value != null)
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 6].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    //cara gancang ngarah teu kudu aya pengecekan tiap field
                                    Context.Atm dbitem = new Context.Atm();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoAtm.FindByPK(id);
                                        }
                                        dbitem.NoKartu = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbitem.IdBank = RepoLookup.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        dbitem.NoRekening = workSheet.Cells[rowIterator, 3].Value.ToString();
                                        dbitem.AtasNama = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        dbitem.IdDriver = RepoDriver.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;

                                        RepoAtm.save(dbitem, UserPrincipal.id);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                        response.Success = true;
                    }
                    catch (Exception e)
                    {
                        response.Success = false;
                        response.Message = e.Message.ToString();
                    }

                }
            }

            return new JavaScriptSerializer().Serialize(new { Response = response });
        }
        public FileContentResult Export()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Atm> dbitems = RepoAtm.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "No Kartu";
            ws.Cells[1, 2].Value = "Nama Bank";
            ws.Cells[1, 3].Value = "No Rekening";
            ws.Cells[1, 4].Value = "Atas Nama";
            ws.Cells[1, 5].Value = "Driver";
            ws.Cells[1, 6].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].NoKartu;
                ws.Cells[i + 2, 2].Value = dbitems[i].LookupCodeBank.Nama;
                ws.Cells[i + 2, 3].Value = dbitems[i].NoRekening;
                ws.Cells[i + 2, 4].Value = dbitems[i].AtasNama;
                ws.Cells[i + 2, 5].Value = dbitems[i].Driver.KodeDriver;
                ws.Cells[i + 2, 6].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Atm.xls";

            return fsr;
        }
        #endregion
    }
}