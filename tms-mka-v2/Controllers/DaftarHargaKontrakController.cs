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
    public class DaftarHargaKontrakController : BaseController
    {
        private IDaftarHargaKontrakRepo RepoDHK;
        private ISalesOrderRepo RepoSO;
        public DaftarHargaKontrakController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, 
            IDaftarHargaKontrakRepo repoDHK, ISalesOrderRepo repoSO)
            : base(repoBase, repoLookup)
        {
            RepoDHK = repoDHK;
            RepoSO = repoSO;
        }
        [MyAuthorize(Menu = "Daftar Harga Kontrak", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DaftarHargaKontrak").ToList();
            return View();
        }

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
            List<Context.DaftarHargaKontrak> dho = RepoDHK.FindAll();
            List<DaftarHargaKontrakItem> model = new List<DaftarHargaKontrakItem>();
            Context.DaftarHargaKontrak dhoitem = dho.Where(d => tanggalMuat >= d.PeriodStart && tanggalMuat <= d.PeriodEnd && d.IdCust == idCust).FirstOrDefault();

            if (dho != null)
            {
                foreach (Context.DaftarHargaKontrakItem item in dhoitem.DaftarHargaKontrakItem.ToList())
                {
                    model.Add(new DaftarHargaKontrakItem(item));
                }
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DaftarHargaKontrak> items = RepoDHK.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<DaftarHargaKontrak> ListModel = new List<DaftarHargaKontrak>();
            foreach (Context.DaftarHargaKontrak item in items)
            {
                ListModel.Add(new DaftarHargaKontrak(item));
            }

            int total = RepoDHK.Count(param.Filters);
            return new JavaScriptSerializer().Serialize(new
            {
                total = total,
                data = ListModel.Select(i => new
                {
                    Id = i.Id,
                    IdCust = i.IdCust,
                    KodeCustomer = i.KodeCustomer,
                    KodeNama = i.KodeNama,
                    NamaCustomer = i.NamaCustomer,
                    AlamatCustomer = i.AlamatCustomer,
                    TelpCustomer = i.TelpCustomer,
                    FaxCustomer = i.FaxCustomer,
                    ContactCustomer = i.ContactCustomer,
                    HpCustomer = i.HpCustomer,
                    PeriodStart = i.PeriodStart,
                    PeriodEnd = i.PeriodEnd,
                    IdTypeKontrak = i.IdTypeKontrak,
                    listItem = i.listItem,
                    listAtt = i.listAtt,
                    listKondisi = i.listKondisi
                })
            });
        }
        [MyAuthorize(Menu = "Daftar Harga Kontrak", Action="create")]
        public ActionResult Add()
        {
            DaftarHargaKontrak model = new DaftarHargaKontrak();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DaftarHargaKontrak model)
        {
            DaftarHargaKontrakItem[] result = JsonConvert.DeserializeObject<DaftarHargaKontrakItem[]>(model.StrItem);
            model.listItem = result.ToList();

            DaftarHargaKontrakAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaKontrakAttachment[]>(model.StrAttachment);
            model.listAtt = resultAtt.ToList();
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
            if (ModelState.IsValid)
            {
                bool palid = true;
                if (RepoDHK.IsPeriodValid(model.PeriodStart.Value, model.PeriodEnd.Value, model.IdCust.Value))
                {
                    ModelState.AddModelError("PeriodStart", "Periode awal tidak boleh overlaping.");
                    ModelState.AddModelError("PeriodEnd", "Periode akhir tidak boleh overlaping.");
                    palid = false;
                }

                if (!palid)
                    return View("Form", model);

                Context.DaftarHargaKontrak dbitem = new Context.DaftarHargaKontrak();
                model.setDb(dbitem);
                RepoDHK.save(dbitem);

                return RedirectToAction("Index");
            }

            return View("Form", model);

        }
        [MyAuthorize(Menu = "Daftar Harga Kontrak", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.DaftarHargaKontrak dbitem = RepoDHK.FindByPK(id);
            DaftarHargaKontrak model = new DaftarHargaKontrak(dbitem);
            ViewBag.name = model.NamaCustomer;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(DaftarHargaKontrak model)
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

            if (RepoDHK.IsPeriodValid(model.PeriodStart.Value, model.PeriodEnd.Value, model.IdCust.Value, model.Id))
            {
                ModelState.AddModelError("PeriodStart", "Periode awal tidak boleh overlaping.");
                ModelState.AddModelError("PeriodEnd", "Periode akhir tidak boleh overlaping.");
            }

            if (ModelState.IsValid)
            {
                Context.DaftarHargaKontrak dbitem = RepoDHK.FindByPK(model.Id);
                model.setDb(dbitem);
                RepoDHK.save(dbitem);

                return RedirectToAction("Index");
            }
            ViewBag.name = model.NamaCustomer;

            DaftarHargaKontrakItem[] result = JsonConvert.DeserializeObject<DaftarHargaKontrakItem[]>(model.StrItem);
            model.listItem = result.ToList();

            DaftarHargaKontrakAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaKontrakAttachment[]>(model.StrAttachment);
            model.listAtt = resultAtt.ToList();

            return View("Form", model);
        }
        [MyAuthorize(Menu = "Daftar Harga Kontrak", Action="delete")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.DaftarHargaKontrak dbItem = RepoDHK.FindByPK(id);

            RepoDHK.delete(dbItem);

            return Json(response);
        }

        #region option

        //[HttpPost]
        public JsonResult GetPeriodeByCust(int id)
        {
            List<Context.DaftarHargaKontrak> dhk = RepoDHK.FindAll().Where(d => d.IdCust == id && d.DaftarHargaKontrakItem.Count > 0).ToList();
            var listPeriode = dhk.Select(c => new
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

            List<DaftarHargaKontrakItem> listItem = new List<DaftarHargaKontrakItem>();
            Context.DaftarHargaKontrak DHK = RepoDHK.FindAll().Where(d => d.Id == id && d.PeriodStart == dtrDate && d.PeriodEnd == dtrDate2).FirstOrDefault();
            if (DHK != null)
            {
                foreach (Context.DaftarHargaKontrakItem item in DHK.DaftarHargaKontrakItem)
                {
                    listItem.Add(new DaftarHargaKontrakItem(item));
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
                    BeratMinimum = i.BeratMinimum,
                    Harga = i.Harga,
                    IdSatuanHarga = i.IdSatuanHarga,
                    SatuanHarga = i.SatuanHarga,
                    HargaRit2 = i.HargaRit2,
                    Overtime = i.Overtime,
                    RitaseBulan = i.RitaseBulan,
                    IsAsuransi = i.IsAsuransi,
                    PihakPenanggung = i.PihakPenanggung,
                    TipeNilaiPenanggung = i.TipeNilaiTanggungan,
                    Premi = i.Premi,
                    NilaiTanggungan = i.NilaiTanggungan,
                    Keterangan = i.Keterangan
                }));
        }
        #endregion
    }

}