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
using MoreLinq;

namespace tms_mka_v2.Controllers
{
    public class DaftarHargaOnCallController : BaseController
    {
        private IDaftarHargaOnCallRepo RepoDHO;
        private IRuteRepo RepoRute;
        private ISalesOrderRepo RepoSO;
        private ICustomerRepo RepoCustomer;
        private IJenisTruckRepo RepoJenisTruck;
        public DaftarHargaOnCallController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDaftarHargaOnCallRepo repoDHO, IRuteRepo repoRute, ISalesOrderRepo repoSO,
            ICustomerRepo repoCustomer, IJenisTruckRepo repoJenisTruck)
            : base(repoBase, repoLookup)
        {
            RepoDHO = repoDHO;
            RepoRute = repoRute;
            RepoSO = repoSO;
            RepoCustomer = repoCustomer;
            RepoJenisTruck = repoJenisTruck;
        }

        public string Upload(IEnumerable<HttpPostedFileBase> files)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (files != null)
            {
                foreach (var file in files)
                {
//                    try
  //                  {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null)
                                {
                                    int id = 0;
                                    int resId;

                                    if (workSheet.Cells[rowIterator, 3].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 3].Value.ToString(), out resId))
                                        {
                                            id = resId;
                                        }
                                    }
                                    Context.Customer dbCust = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                                    int dbCustId = dbCust.Id;
                                    DateTime start = DateTime.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                    DateTime end = DateTime.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                    Context.DaftarHargaOnCall dbitem = RepoDHO.FindByPeriodAndCust(DateTime.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()), DateTime.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()), dbCust.Id);
                                    int dbitem1 = dbitem.Id;
                                    int dbitem2 = dbitem.Id;
        //                            try
          //                          {
                                        List<string> IdRute = new List<string>();
                                        List<string> NamaRute = new List<string>();
                                        foreach (var item in workSheet.Cells[rowIterator, 7].Value.ToString().Split(','))
                                        {
                                            IdRute.Add(RepoRute.FindByKode(item).Id.ToString());
                                            NamaRute.Add(RepoRute.FindByKode(item).Nama);
                                        }
                                        dbitem.DaftarHargaOnCallItem.Add(new Context.DaftarHargaOnCallItem()
                                        {
                                            NamaRuteDaftarHarga = workSheet.Cells[rowIterator, 6].Value.ToString(),
                                            ListIdRute = string.Join(", ", IdRute),
                                            ListNamaRute = string.Join(", ", NamaRute),
                                            IdJenisTruck = RepoJenisTruck.FindByName(workSheet.Cells[rowIterator, 8].Value.ToString()).Id,
                                            MinKg = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString()),
                                            Harga = decimal.Parse(workSheet.Cells[rowIterator, 10].Value.ToString()),
                                            IdSatuanHarga = RepoLookup.FindByName(workSheet.Cells[rowIterator, 11].Value.ToString()).Id,
                                            IsAsuransi = bool.Parse(workSheet.Cells[rowIterator, 13].Value.ToString()),
                                            IsAdHoc = bool.Parse(workSheet.Cells[rowIterator, 12].Value.ToString()),
                                            PihakPenanggung = workSheet.Cells[rowIterator, 14].Value == null ? null : workSheet.Cells[rowIterator, 14].Value.ToString(),
                                            TipeNilaiTanggungan = workSheet.Cells[rowIterator, 16].Value.ToString(),
                                            NilaiTanggungan = workSheet.Cells[rowIterator, 17].Value == null ? 0 : decimal.Parse(workSheet.Cells[rowIterator, 17].Value.ToString()),
                                            Premi = workSheet.Cells[rowIterator, 15].Value == null ? 0 : decimal.Parse(workSheet.Cells[rowIterator, 15].Value.ToString()),
                                            Keterangan = workSheet.Cells[rowIterator, 18].Value == null ? null : workSheet.Cells[rowIterator, 18].Value.ToString()
                                        });
                                        RepoDHO.save(dbitem, UserPrincipal.id);
            /*                        }
                                    catch (Exception)
                                    {

                                    }
              */                  }
                            }
                        }
                        response.Success = true;
    /*                }
                    catch (Exception e)
                    {
                        response.Success = false;
                        response.Message = e.Message.ToString();
                    }
      */          }
            }

            return new JavaScriptSerializer().Serialize(new { Response = response });
        }


        [MyAuthorize(Menu = "Daftar Harga Oncall", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DaftarHargaOnCall").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DaftarHargaOnCall> items = RepoDHO.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<DaftarHargaOnCall> ListModel = new List<DaftarHargaOnCall>();
            foreach (Context.DaftarHargaOnCall item in items)
            {
                ListModel.Add(new DaftarHargaOnCall(item));
            }

            int total = RepoDHO.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        [MyAuthorize(Menu = "Daftar Harga Oncall", Action="create")]
        public ActionResult Add()
        {
            DaftarHargaOnCall model = new DaftarHargaOnCall();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DaftarHargaOnCall model)
        {
            //validasi kondisi
            int idx = 0;
            foreach (DaftarHargaKondisi item in model.listKondisi.Where(d => d.IsDelete == false))
            {
                if (item.kondisi != "Biaya Multidrop" && item.IsDefault == true && item.IsInclude == true && item.IsBill == false && item.value == null)
                {
                    ModelState.AddModelError("listKondisi[" + idx + "].value", "Nilai harus diisi.");
                }
                if (item.kondisi == "Biaya Multidrop" && item.IsKota == true && item.ValKota == null)
                {
                    ModelState.AddModelError("listKondisi[" + idx + "].ValKota", "Nilai harus diisi.");
                }
                if (item.kondisi == "Biaya Multidrop" && item.IsTitik == true && item.ValTitik == null)
                {
                    ModelState.AddModelError("listKondisi[" + idx + "].ValTitik", "Nilai harus diisi.");
                }
                if (item.IsDefault == false)
                {
                    if (item.kondisi == "" || item.kondisi == null)
                        ModelState.AddModelError("listKondisi[" + idx + "].kondisi", "Nama kondisi harus diisi.");
                    if (item.IsInclude == true && item.IsBill == false && item.value == null)
                        ModelState.AddModelError("listKondisi[" + idx + "].value", "Nilai harus diisi.");
                }

                idx++;
            }

            DaftarHargaOnCallItem[] result = JsonConvert.DeserializeObject<DaftarHargaOnCallItem[]>(model.StrItem);
            model.listItem = result.ToList();

            DaftarHargaOnCallAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaOnCallAttachment[]>(model.StrAttachment);
            model.listAtt = resultAtt.ToList();

            if (ModelState.IsValid)
            {
                bool palid = true;
                if (RepoDHO.IsPeriodValid(model.PeriodStart.Value, model.PeriodEnd.Value, model.IdCust.Value))
                {
                    ModelState.AddModelError("PeriodStart", "Periode awal tidak boleh overlaping.");
                    ModelState.AddModelError("PeriodEnd", "Periode akhir tidak boleh overlaping.");
                    palid = false;
                }
                if (!palid)
                    return View("Form", model);

                Context.DaftarHargaOnCall dbitem = new Context.DaftarHargaOnCall();
                model.setDb(dbitem);
                RepoDHO.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }

            return View("Form", model);
        }
        [MyAuthorize(Menu = "Daftar Harga Oncall", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.DaftarHargaOnCall dbitem = RepoDHO.FindByPK(id);
            DaftarHargaOnCall model = new DaftarHargaOnCall(dbitem);
            ViewBag.name = model.NamaCustomer;
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Daftar Harga Oncall", Action = "read")]
        public ActionResult Detail(int id)
        {
            Context.DaftarHargaOnCall dbitem = RepoDHO.FindByPK(id);
            DaftarHargaOnCall model = new DaftarHargaOnCall(dbitem);
            ViewBag.name = model.NamaCustomer;
            return View("Show", model);
        }
        [HttpPost]
        public ActionResult Edit(DaftarHargaOnCall model)
        {
            //validasi kondisi
            int idx = 0;
            foreach (DaftarHargaKondisi item in model.listKondisi.Where(d => d.IsDelete == false))
            {
                if (item.kondisi != "Biaya Multidrop" && item.IsDefault == true && item.IsInclude == true && item.IsBill == false && item.value == null)
                {
                    ModelState.AddModelError("listKondisi[" + idx + "].value", "Nilai harus diisi.");
                }
                if (item.kondisi == "Biaya Multidrop" && item.IsKota == true && item.ValKota == null)
                {
                    ModelState.AddModelError("listKondisi[" + idx + "].ValKota", "Nilai harus diisi.");
                }
                if (item.kondisi == "Biaya Multidrop" && item.IsTitik == true && item.ValTitik == null)
                {
                    ModelState.AddModelError("listKondisi[" + idx + "].ValTitik", "Nilai harus diisi.");
                }
                if (item.IsDefault == false)
                {
                    if (item.kondisi == "" || item.kondisi == null)
                        ModelState.AddModelError("listKondisi[" + idx + "].kondisi", "Nama kondisi harus diisi.");
                    if (item.IsInclude == true && item.IsBill == false && item.value == null)
                        ModelState.AddModelError("listKondisi[" + idx + "].value", "Nilai harus diisi.");
                }

                idx++;
            }

            if (RepoDHO.IsPeriodValid(model.PeriodStart.Value, model.PeriodEnd.Value, model.IdCust.Value, model.Id))
            {
                ModelState.AddModelError("PeriodStart", "Periode awal tidak boleh overlaping.");
                ModelState.AddModelError("PeriodEnd", "Periode akhir tidak boleh overlaping.");
            }

            if (ModelState.IsValid)
            {
                Context.DaftarHargaOnCall dbitem = RepoDHO.FindByPK(model.Id);
                model.setDb(dbitem);

                var query = "";
                DaftarHargaOnCallItem[] results = JsonConvert.DeserializeObject<DaftarHargaOnCallItem[]>(model.StrItem);
                List<Context.DaftarHargaOnCallItem> DummyItems = dbitem.DaftarHargaOnCallItem.ToList();
                List<int> ListAnuTeuDiHapus = new List<int>();
                foreach (DaftarHargaOnCallItem item in results)
                {
                    if (item.Id != 0)
                    {
                        query += "UPDATE dbo.\"DaftarHargaOnCallItem\" SET \"NamaDaftarHargaRute\" = " + item.NamaRuteDaftarHarga + ", \"ListIdRute\" = " + item.ListIdRute + ", \"ListNamaRute\" = " +
                            item.ListNamaRute + ", \"IdJenisKendaraan\" = " + item.IdJenisTruck + ", \"MinKg\" = " + item.MinKg + ", \"Harga\" = " + item.Harga +
                            ", \"IsAsuransi\" = " + item.IsAsuransi + ", \"Premi\" = " + item.Premi + ", \"NilaiTanggungan\" = " + item.NilaiTanggungan + ", \"Keterangan\" = " + item.Keterangan +
                            ", \"PihakPenanggung\" = " + item.PihakPenanggung + ", \"TipeNilaiTanggungan\" = " + item.TipeNilaiTanggungan + ", \"IdSatuanHarga\" = " + item.IdSatuanHarga +
                            " WHERE \"Id\" = " + item.Id + ";";
                        ListAnuTeuDiHapus.Add(item.Id);
                    }
                    else
                    {
                        query += "INSERT INTO dbo.\"DaftarHargaKonsolidasiItem\" (\"IdDaftarHargaKonsolidasi\", \"NamaDaftarHargaRute\", \"ListIdRute\", \"ListNamaRute\", \"IdJenisKendaraan\", " +
                            "\"MinKg\", \"MaxKg\", \"Harga\", \"IsAsuransi\", \"Premi\", \"NilaiTanggungan\", \"Keterangan\", \"PihakPenanggung\", \"TipeNilaiTanggungan\", \"IdSatuanHarga\") VALUES ( " +
                            dbitem.Id + ", " + item.NamaRuteDaftarHarga + ", " + item.ListIdRute + ", " + item.ListNamaRute + ", " + item.IdJenisTruck + ", " +
                            item.Harga + ", " + item.IsAsuransi + ", " + item.Premi + ", " + item.NilaiTanggungan + ", " + item.Keterangan + ", " + item.PihakPenanggung + ", " + item.TipeNilaiTanggungan +
                            ", " + item.IdSatuanHarga + ");";
                    }
                }
                foreach (Context.DaftarHargaOnCallItem dbhapus in DummyItems)
                {
                    if (!ListAnuTeuDiHapus.Any(d => d == dbhapus.Id))
                    {
                        query += "DELETE FROM dbo.\"DaftarHargaOnCallItem\" WHERE \"IdDaftarHargaOnCall\" = " + dbitem.Id + ";";
                    }
                }
                RepoDHO.save(dbitem, UserPrincipal.id, query);
                return RedirectToAction("Index");
            }
            ViewBag.name = model.NamaCustomer;

            DaftarHargaOnCallItem[] result = JsonConvert.DeserializeObject<DaftarHargaOnCallItem[]>(model.StrItem);
            model.listItem = result.ToList();

            DaftarHargaOnCallAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaOnCallAttachment[]>(model.StrAttachment);
            model.listAtt = resultAtt.ToList();

            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isExistData = false;
            ResponeModel response = new ResponeModel(true);
            Context.DaftarHargaOnCall dbItem = RepoDHO.FindByPK(id);
            List<int> listId = dbItem.DaftarHargaOnCallItem.Where(i => i.IdDaftarHargaOnCall == dbItem.Id).Select(i => i.Id).ToList();
            isExistData = RepoSO.FindAllOnCall().Where(a => listId.Contains(a.SalesOrderOncall.IdDaftarHargaItem.Value)).Count() > 0;

            if (!isExistData)
                RepoDHO.delete(dbItem);
            else
            {
                response.Success = false;
                response.Message = "Data sudah digunakan, Penghapusan gagal";
            }

            return Json(response);
        }

        #region option

        public string GetRuteByCustomer(int idCust, DateTime tanggalMuat)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            if (param.Filters.Filters == null)
                param.Filters.Filters = new List<tms_mka_v2.Infrastructure.FilterInfo>();
            param.Filters.Filters.Add(new tms_mka_v2.Infrastructure.FilterInfo
            {
                Field = "IdCust",
                Operator = "eq",
                Value = idCust.ToString(),
            });

            List<Rute> listRute = new List<Rute>();
            List<Context.DaftarHargaOnCall> dho = RepoDHO.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<DaftarHargaOnCallItem> model = new List<DaftarHargaOnCallItem>();
            Context.DaftarHargaOnCall dhoitem = dho.Where(d => tanggalMuat >= d.PeriodStart && tanggalMuat <= d.PeriodEnd).FirstOrDefault();

            if (dho != null)
            {
                foreach (Context.DaftarHargaOnCallItem item in dhoitem.DaftarHargaOnCallItem.ToList())
                {
                    model.Add(new DaftarHargaOnCallItem(item));
                }
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        //[HttpPost]
        public JsonResult GetPeriodeByCust(int id)
        {
            List<Context.DaftarHargaOnCall> dho = RepoDHO.FindAll().Where(d => d.IdCust == id && d.DaftarHargaOnCallItem.Count > 0).ToList();
            var listPeriode = dho.Select(c => new
            {
                Id = c.Id,
                PeriodeHarga = c.PeriodStart.ToString("dd/MM/yyyy") + " - " + c.PeriodEnd.ToString("dd/MM/yyyy")

            });

            return this.Json(listPeriode, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public String GetItem(int id, string date)
        {
            DateTime dtrDate = DateTime.Parse(date.Split(new string[] { " - " }, StringSplitOptions.None)[0]);
            DateTime dtrDate2 = DateTime.Parse(date.Split(new string[] { " - " }, StringSplitOptions.None)[1]);

            List<DaftarHargaOnCallItem> listItem = new List<DaftarHargaOnCallItem>();
            Context.DaftarHargaOnCall DHO = RepoDHO.FindAll().Where(d => d.Id == id && d.PeriodStart == dtrDate && d.PeriodEnd == dtrDate2).FirstOrDefault();
            if (DHO != null)
            {
                foreach (Context.DaftarHargaOnCallItem item in DHO.DaftarHargaOnCallItem)
                {
                    listItem.Add(new DaftarHargaOnCallItem(item));
                }
            }

            return new JavaScriptSerializer().Serialize(
                listItem.Select(i => new
                {
                    Id = i.Id,
                    NamaRuteDaftarHarga = i.NamaRuteDaftarHarga,
                    ListIdRute = i.ListIdRute,
                    ListNamaRute = i.ListNamaRute,
                    IdJenisTruck = i.IdJenisTruck,
                    NamaJenisTruck = i.NamaJenisTruck,
                    MinKg = i.MinKg,
                    Harga = i.Harga,
                    IdSatuanHarga = i.IdSatuanHarga,
                    SatuanHarga = i.SatuanHarga,
                    IsAsuransi = i.IsAsuransi,
                    IsAdhoc = i.IsAdHoc,
                    PihakPenanggung = i.PihakPenanggung,
                    TipeNilaiTanggungan = i.TipeNilaiTanggungan,
                    Premi = i.Premi,
                    NilaiTanggungan = i.NilaiTanggungan,
                    Keterangan = i.Keterangan
                }));
        }

        public JsonResult IsUsed(int id, bool isImported)
        {
            bool isExistData = false;

            // fitur Import Daftar Harga
            if (isImported)
            {
                Context.DaftarHargaOnCall dbItem = RepoDHO.FindByPK(id);
                List<int> listId = dbItem.DaftarHargaOnCallItem.Where(i => i.IdDaftarHargaOnCall == dbItem.Id).Select(i => i.Id).ToList();
                isExistData = RepoSO.FindAllOnCall().Where(a => listId.Contains(a.SalesOrderOncall.IdDaftarHargaItem.Value)).Count() > 0;
            }
            // edit & delete grid item Daftar Harga
            else
            {
                isExistData = RepoSO.FindAllOnCall().Where(a => a.SalesOrderOncall.IdDaftarHargaItem == id).Count() > 0;
            }

            return this.Json(isExistData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}