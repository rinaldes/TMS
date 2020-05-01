using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using System.Web.Script.Serialization;
using tms_mka_v2.Infrastructure;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace tms_mka_v2.Controllers
{
    public class JnsTolController : BaseController
    {
        private IJnsTolRepo RepoJnsTol;
        public JnsTolController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookUp, IJnsTolRepo repoJnsTol)
            : base(repoBase, repoLookUp)
        {
            RepoJnsTol = repoJnsTol;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "JnsTol").ToList();

            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.JnsTols> items = RepoJnsTol.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<JnsTol> ListModel = new List<JnsTol>();
            foreach (Context.JnsTols item in items)
            {
                ListModel.Add(new JnsTol(item));
            }

            int total = RepoJnsTol.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel.Select(d => new {
                Id = d.Id,
                NamaTol = d.NamaTol,
                gol1 = d.GolonganTol1,
                gol2 = d.GolonganTol2,
                gol3 = d.GolonganTol3,
                gol4 = d.GolonganTol4,
                Keterangan = d.Keterangan
            }) });
        }
        public string BindingHistory(int IdTol)
        {
            Context.JnsTols items = RepoJnsTol.FindByPK(IdTol);

            return new JavaScriptSerializer().Serialize(new
            {
                data = items.ListHistoryJnsTols.OrderByDescending(d=> d.Id).Select(d => new
                {
                    Tanggal = d.Tanggal.ToLongDateString(),
                    NamaTol = d.NamaTol,
                    GolonganTol1 = d.GolonganTol1,
                    GolonganTol2 = d.GolonganTol2,
                    GolonganTol3 = d.GolonganTol3,
                    GolonganTol4 = d.GolonganTol4,
                    Keterangan = d.Keterangan,
                    User = d.ForUser.Username
                })
            });
        }
        public ActionResult Add()
        {
            JnsTol model = new JnsTol();

            return View("Form",model);
        }
        [HttpPost]
        public ActionResult Add(JnsTol model)
        {
            if (ModelState.IsValid)
            {
                bool Isexist = RepoJnsTol.IsExist(model.NamaTol);

                if (Isexist)
                {
                    ModelState.AddModelError("NamaTol", "Nama Tol telah dipakai.");
                    return View("Form", model);
                }

                Context.JnsTols dbitem = new Context.JnsTols();
                model.setDb(dbitem);
                Context.HistoryJnsTols dbitemhistory = new Context.HistoryJnsTols();
                model.setDbHistory(dbitemhistory, UserPrincipal.id);
                dbitem.ListHistoryJnsTols.Add(dbitemhistory);
                RepoJnsTol.save(dbitem, UserPrincipal.id, dbitemhistory);
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            JnsTol model = new JnsTol(RepoJnsTol.FindByPK(id));
            ViewBag.name = model.NamaTol;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(JnsTol model)
        {
            if (ModelState.IsValid)
            {
                bool Isexist = RepoJnsTol.IsExist(model.NamaTol, model.Id);

                if (Isexist)
                {
                    ModelState.AddModelError("NamaTol", "Nama Tol telah dipakai.");
                    return View("Form", model);
                }

                Context.JnsTols dbitem = RepoJnsTol.FindByPK(model.Id);
                model.setDb(dbitem);
                Context.HistoryJnsTols dbitemhistory = new Context.HistoryJnsTols();
                model.setDbHistory(dbitemhistory, UserPrincipal.id);
                dbitem.ListHistoryJnsTols.Add(dbitemhistory);
                RepoJnsTol.save(dbitem, UserPrincipal.id, dbitemhistory);

                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.JnsTols dbItem = RepoJnsTol.FindByPK(id);

            RepoJnsTol.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        public string GetJnsTol()
        {
            List<Context.JnsTols> dbitems = RepoJnsTol.FindAll();

            return new JavaScriptSerializer().Serialize(dbitems.Select(d => new { Id = d.Id, Nama = d.NamaTol}));
        }
        #region upload
        public string UploadTol(IEnumerable<HttpPostedFileBase> filesTol)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesTol != null)
            {
                foreach (var file in filesTol)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null &&
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null
                                    && workSheet.Cells[rowIterator, 5].Value != null)
                                {
                                    try
                                    {
                                        int id = 0;

                                        int resId;
                                        if (workSheet.Cells[rowIterator, 7].Value != null)
                                        {
                                            if (int.TryParse(workSheet.Cells[rowIterator, 7].Value.ToString(), out resId))
                                                id = resId;
                                        }
                                        if (id == 0)
                                        {
                                            bool Isexist = RepoJnsTol.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString());

                                            if (Isexist)
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            bool Isexist = RepoJnsTol.IsExist(workSheet.Cells[rowIterator, 1].Value.ToString(), id);

                                            if (Isexist)
                                            {
                                                continue;
                                            }
                                        }
                                        Context.JnsTols dbitem = new Context.JnsTols();
                                        if (id != 0)
                                        {
                                            dbitem = RepoJnsTol.FindByPK(id);
                                        }
                                        
                                        dbitem.NamaTol = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbitem.GolonganTol1 = decimal.Parse(workSheet.Cells[rowIterator, 2].Value.ToString());
                                        dbitem.GolonganTol2 = decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                        dbitem.GolonganTol3 = decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                        dbitem.GolonganTol4 = decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        dbitem.Keterangan = workSheet.Cells[rowIterator, 6].Value == null ? "" : workSheet.Cells[rowIterator, 6].Value.ToString();

                                        RepoJnsTol.save(dbitem, UserPrincipal.id, new Context.HistoryJnsTols());
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
            List<Context.JnsTols> dbitems = RepoJnsTol.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Nama Area";
            ws.Cells[1, 2].Value = "Golongan 1";
            ws.Cells[1, 3].Value = "Golongan 2";
            ws.Cells[1, 4].Value = "Golongan 3";
            ws.Cells[1, 5].Value = "Golongan 4";
            ws.Cells[1, 6].Value = "Keterangan";
            ws.Cells[1, 7].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].NamaTol;
                ws.Cells[i + 2, 2].Value = dbitems[i].GolonganTol1;
                ws.Cells[i + 2, 3].Value = dbitems[i].GolonganTol2;
                ws.Cells[i + 2, 4].Value = dbitems[i].GolonganTol3;
                ws.Cells[i + 2, 5].Value = dbitems[i].GolonganTol4;
                ws.Cells[i + 2, 6].Value = dbitems[i].Keterangan;
                ws.Cells[i + 2, 7].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Jenis Tol.xls";

            return fsr;
        }

        #endregion

    }
}