using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
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
    public class DataPendinginController : BaseController
    {
        private IDataPendinginRepo RepoPendingin;
        private IDataTruckRepo RepoDataTruck;
        private ILookupCodeRepo LookupCode;

        public DataPendinginController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDataPendinginRepo repoPendingin, IDataTruckRepo repoDataTruck, ILookupCodeRepo lookupCode)
            : base(repoBase, repoLookup)
        {
            RepoPendingin = repoPendingin;
            RepoDataTruck = repoDataTruck;
            LookupCode = lookupCode;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DataPendingin").ToList();

            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DataPendingin> items = RepoPendingin.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<DataPendingin> ListModel = new List<DataPendingin>();
            foreach (Context.DataPendingin item in items)
            {
                ListModel.Add(new DataPendingin(item));
            }

            int total = RepoPendingin.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new
            {
                total = total,
                data = ListModel
            });
        }
        public string BindingHistory(int IdPendingin)
        {
            Context.DataPendingin items = RepoPendingin.FindByPK(IdPendingin);

            List<DataPendingin> listmodel = new List<DataPendingin>();
            foreach (Context.DataPendinginHistory item in items.ListHistoryPendingin)
            {
                listmodel.Add(new DataPendingin(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = listmodel });
        }
        public ActionResult Add()
        {
            DataPendingin model = new DataPendingin();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DataPendingin model)
        {
            if (ModelState.IsValid)
            {
                if (RepoPendingin.IsBoxExist(model.IdDataTruk.Value))
                {
                    ModelState.AddModelError("IdDataTruk", "Truck sudah tepasang pendingin, harap ganti dengan truck yang lain");
                    return View("Form", model);
                }
                Context.DataPendingin dbitem = new Context.DataPendingin();
                model.setDb(dbitem);

                //generate code
                dbitem.Urutan = RepoPendingin.getUrutan() + 1;
                dbitem.NoPendingin = RepoPendingin.generateCode(dbitem.Urutan);

                Context.DataPendinginHistory dbitemhistory = new Context.DataPendinginHistory();
                model.setDbHistory(dbitemhistory, UserPrincipal.firstname + ' ' + UserPrincipal.lastname);
                dbitemhistory.NoPendingin = dbitem.NoPendingin;
                dbitemhistory.strDataTruk = RepoDataTruck.FindByPK(dbitem.IdDataTruk).VehicleNo;
                dbitemhistory.strJenisPendingin = dbitem.IdJenisPendingin.HasValue ? RepoLookup.FindByPK(dbitem.IdJenisPendingin).Nama : "";
                dbitem.ListHistoryPendingin.Add(dbitemhistory);
                Context.DataTruckPendinginHistory dbtruckhistory = new Context.DataTruckPendinginHistory();
                model.setDbTruckHistory(dbtruckhistory, UserPrincipal.firstname + ' ' + UserPrincipal.lastname);
                RepoPendingin.save(dbitem, UserPrincipal.id, dbitemhistory, dbtruckhistory);

                Context.DataTruck dbtruck = RepoDataTruck.FindByPK(model.IdDataTruk.Value);
                dbtruckhistory.NoPendingin = dbitem.NoPendingin;
                dbtruckhistory.strDataTruk = RepoDataTruck.FindByPK(dbitem.IdDataTruk).VehicleNo;
                dbtruckhistory.strJenisPendingin = dbitem.IdJenisPendingin.HasValue ? RepoLookup.FindByPK(dbitem.IdJenisPendingin).Nama : "";
                dbtruck.DataTruckPendinginHistory.Add(dbtruckhistory);
                RepoDataTruck.save(dbtruck, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            DataPendingin model = new DataPendingin(RepoPendingin.FindByPK(id));
            ViewBag.name = model.NoPendingin;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(DataPendingin model)
        {
            if (ModelState.IsValid)
            {
                if (RepoPendingin.IsBoxExist(model.IdDataTruk.Value, model.Id))
                {
                    ModelState.AddModelError("IdDataTruk", "Truck sudah tepasang pendingin, harap ganti dengan truck yang lain");
                    return View("Form", model);
                }
                Context.DataPendingin dbitem = RepoPendingin.FindByPK(model.Id);
                model.setDb(dbitem);
                Context.DataPendinginHistory dbitemhistory = new Context.DataPendinginHistory();
                model.setDbHistory(dbitemhistory, UserPrincipal.firstname + ' ' + UserPrincipal.lastname);
                dbitem.ListHistoryPendingin.Add(dbitemhistory);
                Context.DataTruckPendinginHistory dbtruckhistory = new Context.DataTruckPendinginHistory();
                model.setDbTruckHistory(dbtruckhistory, UserPrincipal.firstname + ' ' + UserPrincipal.lastname);
                RepoPendingin.save(dbitem, UserPrincipal.id, dbitemhistory, dbtruckhistory);

                Context.DataTruck dbtruck = RepoDataTruck.FindByPK(model.IdDataTruk.Value);
                dbtruck.DataTruckPendinginHistory.Add(dbtruckhistory);
                RepoDataTruck.save(dbtruck, UserPrincipal.id);
                
                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.DataPendingin dbItem = RepoPendingin.FindByPK(id);

            RepoPendingin.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        #region option

        #endregion

        #region upload
        public string UploadDataPendingin(IEnumerable<HttpPostedFileBase> filesDataPendingin)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesDataPendingin != null)
            {
                foreach (var file in filesDataPendingin)
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
                                if (workSheet.Cells[rowIterator, 1].Value != null /*&& workSheet.Cells[rowIterator, 2].Value != null &&
                                    workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null &&
                                    workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null &&
                                    workSheet.Cells[rowIterator, 9].Value != null*/)
                                {
                                    int id = 0;

                                    int resId;
                                    if (workSheet.Cells[rowIterator, 10].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 10].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    Context.DataPendingin dbitem = new Context.DataPendingin();
                                    try
                                    {
                                        if (id != 0)
                                        {
                                            dbitem = RepoPendingin.FindByPK(id);
                                            if (RepoPendingin.IsBoxExist(RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id, id))
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (RepoPendingin.IsBoxExist(RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id))
                                            {
                                                continue;
                                            }
                                            dbitem.Urutan = RepoPendingin.getUrutan() + 1;
                                            dbitem.NoPendingin = RepoPendingin.generateCode(dbitem.Urutan);
                                        }

                                        dbitem.IdDataTruk = RepoDataTruck.FindByName(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        dbitem.Merk = workSheet.Cells[rowIterator, 2].Value == null ? null : workSheet.Cells[rowIterator, 2].Value.ToString();
                                        dbitem.Model = workSheet.Cells[rowIterator, 3].Value == null ? null : workSheet.Cells[rowIterator, 3].Value.ToString();
                                        dbitem.HmLimit = workSheet.Cells[rowIterator, 4].Value == null ? null : workSheet.Cells[rowIterator, 4].Value.ToString();
                                        dbitem.Tahun = workSheet.Cells[rowIterator, 5].Value == null ? (int?)null : int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        dbitem.IdJenisPendingin = workSheet.Cells[rowIterator, 6].Value == null ? (int?)null : LookupCode.FindByName(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                                        dbitem.NoMesin = workSheet.Cells[rowIterator, 7].Value == null ? null : workSheet.Cells[rowIterator, 7].Value.ToString();
                                        dbitem.NoKompresor = workSheet.Cells[rowIterator, 8].Value == null ? null : workSheet.Cells[rowIterator, 8].Value.ToString();

                                        //history
                                        Context.DataPendinginHistory dbhistory = new Context.DataPendinginHistory();
                                        dbhistory.Id =  dbitem.Id;
                                        dbhistory.Tanggal = DateTime.Now;
                                        dbhistory.user = UserPrincipal.firstname + " " + UserPrincipal.lastname;
                                        dbhistory.strDataTruk = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        dbhistory.Merk = dbitem.Merk;
                                        dbhistory.Model = dbitem.Model;
                                        dbhistory.HmLimit = dbitem.HmLimit;
                                        dbhistory.Tahun = Convert.ToInt32(dbitem.Tahun);
                                        dbhistory.strJenisPendingin = workSheet.Cells[rowIterator, 6].Value == null ? null : workSheet.Cells[rowIterator, 6].Value.ToString();
                                        dbhistory.NoMesin = dbitem.NoMesin;
                                        dbhistory.NoKompresor = dbitem.NoKompresor;
                                        dbhistory.tglPasang = Convert.ToDateTime(dbitem.tglPasang);
                                        dbitem.ListHistoryPendingin.Add(dbhistory);

                                        RepoPendingin.save(dbitem, UserPrincipal.id, dbhistory);
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

        public FileContentResult ExportDataPendingin()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.DataPendingin> dbitems = RepoPendingin.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Vehicle No";
            ws.Cells[1, 2].Value = "Merk";
            ws.Cells[1, 3].Value = "Model";
            ws.Cells[1, 4].Value = "HM Limit";
            ws.Cells[1, 5].Value = "Tahun";
            ws.Cells[1, 6].Value = "Jenis";
            ws.Cells[1, 7].Value = "No Mesin";
            ws.Cells[1, 8].Value = "No Compressor";
            ws.Cells[1, 9].Value = "Tanggal Pasang";
            ws.Cells[1, 10].Value = "Id Database";


            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].DataTruk.VehicleNo;
                ws.Cells[i + 2, 2].Value = dbitems[i].Merk;
                ws.Cells[i + 2, 3].Value = dbitems[i].Model;
                ws.Cells[i + 2, 4].Value = dbitems[i].HmLimit;
                ws.Cells[i + 2, 5].Value = dbitems[i].Tahun;
                ws.Cells[i + 2, 6].Value = dbitems[i].LookupCodeJenis == null ? "" : dbitems[i].LookupCodeJenis.Nama;
                ws.Cells[i + 2, 7].Value = dbitems[i].NoMesin;
                ws.Cells[i + 2, 8].Value = dbitems[i].NoKompresor;
                ws.Cells[i + 2, 9].Value = Convert.ToDateTime(dbitems[i].tglPasang).ToString("dd/MM/yyyy");
                ws.Cells[i + 2, 10].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Data_Pendingin.xls";

            return fsr;
        }
        #endregion
    }



}