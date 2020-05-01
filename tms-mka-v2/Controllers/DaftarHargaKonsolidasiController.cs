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
    public class DaftarHargaKonsolidasiController : BaseController
    {
        private IDaftarHargaKonsolidasiRepo RepoDHKonsolidasi;
        private ISalesOrderRepo RepoSalesOrder;
        public DaftarHargaKonsolidasiController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDaftarHargaKonsolidasiRepo repoDHKonsolidasi, ISalesOrderRepo repoSalesOrder)
            : base(repoBase, repoLookup)
        {
            RepoDHKonsolidasi = repoDHKonsolidasi;
            RepoSalesOrder = repoSalesOrder;
        }
        [MyAuthorize(Menu = "Daftar Harga Konsolidasi", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DaftarHargaKonsolidasi").ToList();
            return View();
        }

        public string Harga(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            List<decimal> harga = new List<decimal>();
            foreach (var item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
            {
                harga.Add(RepoSalesOrder.HargaKonsolidasiPerItem(item) / RepoSalesOrder.Harga(dbitem) * 100);
            }
            return new JavaScriptSerializer().Serialize(new
            {
                data = harga
            });
        }

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DaftarHargaKonsolidasi> items = RepoDHKonsolidasi.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<DaftarHargaKonsolidasi> ListModel = new List<DaftarHargaKonsolidasi>();
            foreach (Context.DaftarHargaKonsolidasi item in items)
            {
                ListModel.Add(new DaftarHargaKonsolidasi(item));
            }

            int total = RepoDHKonsolidasi.Count(param.Filters);

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
                    listItem = i.listItem,
                    listAtt = i.listAtt,
                    listKondisi = i.listKondisi
                })
            });
        }
        [MyAuthorize(Menu = "Daftar Harga Konsolidasi", Action="create")]
        public ActionResult Add()
        {
            DaftarHargaKonsolidasi model = new DaftarHargaKonsolidasi();
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DaftarHargaKonsolidasi model)
        {
            //validasi kondisi
            DaftarHargaKonsolidasiItem[] result = JsonConvert.DeserializeObject<DaftarHargaKonsolidasiItem[]>(model.StrItem);
            model.listItem = result.ToList();

            DaftarHargaKonsolidasiAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaKonsolidasiAttachment[]>(model.StrAttachment);
            model.listAtt = resultAtt.ToList();

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
                if (RepoDHKonsolidasi.IsPeriodValid(model.PeriodStart.Value,model.PeriodEnd.Value, model.IdCust.Value))
                {
                    ModelState.AddModelError("PeriodStart", "Periode awal tidak boleh overlaping.");
                    ModelState.AddModelError("PeriodEnd", "Periode akhir tidak boleh overlaping.");
                    palid = false;
                }
                if(!palid)
                    return View("Form", model);
                
                Context.DaftarHargaKonsolidasi dbitem = new Context.DaftarHargaKonsolidasi();
                model.setDb(dbitem);
                RepoDHKonsolidasi.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }


            return View("Form", model);

        }
        [MyAuthorize(Menu = "Daftar Harga Konsolidasi", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.DaftarHargaKonsolidasi dbitem = RepoDHKonsolidasi.FindByPK(id);
            DaftarHargaKonsolidasi model = new DaftarHargaKonsolidasi(dbitem);
            ViewBag.name = model.NamaCustomer;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(DaftarHargaKonsolidasi model)
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

            if (RepoDHKonsolidasi.IsPeriodValid(model.PeriodStart.Value, model.PeriodEnd.Value, model.IdCust.Value, model.Id))
            {
                ModelState.AddModelError("PeriodStart", "Periode awal tidak boleh overlaping.");
                ModelState.AddModelError("PeriodEnd", "Periode akhir tidak boleh overlaping.");
            }

            if (ModelState.IsValid)
            {
                Context.DaftarHargaKonsolidasi dbitem = RepoDHKonsolidasi.FindByPK(model.Id);
                model.setDb(dbitem);
                DaftarHargaKonsolidasiItem[] results = JsonConvert.DeserializeObject<DaftarHargaKonsolidasiItem[]>(model.StrItem);
                List<Context.DaftarHargaKonsolidasiItem> DummyItems = dbitem.DaftarHargaKonsolidasiItem.ToList();
                List<int> ListAnuTeuDiHapus = new List<int>();
                var query = "";
                foreach (DaftarHargaKonsolidasiItem item in results){
                    if (item.Id != 0)
                    {
                        query += "UPDATE dbo.\"DaftarHargaKonsolidasiItem\" SET \"NamaDaftarHargaRute\" = " + item.NamaRuteDaftarHarga + ", \"ListIdRute\" = " + item.ListIdRute + ", \"ListNamaRute\" = " +
                            item.ListNamaRute + ", \"IdJenisKendaraan\" = " + item.IdJenisKendaraan + ", \"MinKg\" = " + item.MinKg + ", \"MaxKg\" = " + item.MaxKg + ", \"Harga\" = " + item.Harga +
                            ", \"IsAsuransi\" = " + item.IsAsuransi + ", \"Premi\" = " + item.Premi + ", \"NilaiTanggungan\" = " + item.NilaiTanggungan + ", \"Keterangan\" = " + item.Keterangan + 
                            ", \"PihakPenanggung\" = " + item.PihakPenanggung + ", \"TipeNilaiTanggungan\" = " + item.TipeNilaiTanggungan + ", \"IdSatuanHarga\" = " + item.IdSatuanHarga +
                            " WHERE \"Id\" = " + item.Id + ";";
                        ListAnuTeuDiHapus.Add(item.Id);
                    }
                    else
                    {
                        query += "INSERT INTO dbo.\"DaftarHargaKonsolidasiItem\" (\"IdDaftarHargaKonsolidasi\", \"NamaDaftarHargaRute\", \"ListIdRute\", \"ListNamaRute\", \"IdJenisKendaraan\", " +
                            "\"MinKg\", \"MaxKg\", \"Harga\", \"IsAsuransi\", \"Premi\", \"NilaiTanggungan\", \"Keterangan\", \"PihakPenanggung\", \"TipeNilaiTanggungan\", \"IdSatuanHarga\") VALUES ( " +
                            dbitem.Id + ", " + item.NamaRuteDaftarHarga + ", " + item.ListIdRute + ", " + item.ListNamaRute + ", " + item.IdJenisKendaraan + ", " + item.MinKg + ", " + item.MaxKg + ", " +
                            item.Harga + ", " + item.IsAsuransi + ", " + item.Premi + ", " + item.NilaiTanggungan + ", " + item.Keterangan + ", " + item.PihakPenanggung + ", " + item.TipeNilaiTanggungan +
                            ", " + item.IdSatuanHarga + ");";
                    }
                }
                foreach (Context.DaftarHargaKonsolidasiItem dbhapus in DummyItems)
                {
                    if (!ListAnuTeuDiHapus.Any(d => d == dbhapus.Id))
                    {
                        query += "DELETE FROM dbo.\"DaftarHargaKonsolidasiItem\" WHERE \"IdDaftarHargaKonsolidasi\" = " + dbitem.Id + ";";
                    }
                }
                RepoDHKonsolidasi.save(dbitem, UserPrincipal.id, query);

                return RedirectToAction("Index");
            }
            ViewBag.name = model.NamaCustomer;

            DaftarHargaKonsolidasiItem[] result = JsonConvert.DeserializeObject<DaftarHargaKonsolidasiItem[]>(model.StrItem);
            model.listItem = result.ToList();

            DaftarHargaKonsolidasiAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaKonsolidasiAttachment[]>(model.StrAttachment);
            model.listAtt = resultAtt.ToList();

            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isExistData = false;

            ResponeModel response = new ResponeModel(true);
            Context.DaftarHargaKonsolidasi dbItem = RepoDHKonsolidasi.FindByPK(id);
            List<int> listId = dbItem.DaftarHargaKonsolidasiItem.Where(i => i.IdDaftarHargaKonsolidasi == dbItem.Id).Select(i => i.Id).ToList();
            isExistData = RepoSalesOrder.FindAllKonsolidasi().Where(a => listId.Contains(a.SalesOrderKonsolidasi.IdDaftarHargaItem.Value)).Count() > 0;

            if (!isExistData)
                RepoDHKonsolidasi.delete(dbItem, UserPrincipal.id);
            else {
                response.Success = false;
                response.Message = "Data sudah digunakan, Penghapusan gagal";
            }
                
            return Json(response);
        }

        #region option

        public string GetRuteByCustomer(int idCust, DateTime TanggalMasuk)
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
            List<Context.DaftarHargaKonsolidasi> dho = RepoDHKonsolidasi.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<DaftarHargaKonsolidasiItem> model = new List<DaftarHargaKonsolidasiItem>();
            Context.DaftarHargaKonsolidasi dhoitem = dho.Where(d => TanggalMasuk >= d.PeriodStart && TanggalMasuk <= d.PeriodEnd).FirstOrDefault();

            foreach (Context.DaftarHargaKonsolidasiItem item in dhoitem.DaftarHargaKonsolidasiItem.ToList())
            {
                model.Add(new DaftarHargaKonsolidasiItem(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        //[HttpPost]
        public JsonResult GetPeriodeByCust(int id)
        {
            List<Context.DaftarHargaKonsolidasi> dhk = RepoDHKonsolidasi.FindAll().Where(d => d.IdCust == id && d.DaftarHargaKonsolidasiItem.Count > 0).ToList();
            var listPeriode = dhk.Select(c => new
            {
                Id = c.Id,
                PeriodeHarga = c.PeriodStart.ToString("dd/MM/yyyy") + " - " + c.PeriodEnd.ToString("dd/MM/yyyy")

            });

            return this.Json(listPeriode, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        //public JsonResult GetItem(int id, string date)
        public String GetItem(int id, string date)
        {
            DateTime dtrDate = DateTime.Parse(date.Split(new string[] { " - " }, StringSplitOptions.None)[0]);
            DateTime dtrDate2 = DateTime.Parse(date.Split(new string[] { " - " }, StringSplitOptions.None)[1]);

            List<DaftarHargaKonsolidasiItem> listItem = new List<DaftarHargaKonsolidasiItem>();
            Context.DaftarHargaKonsolidasi DHK = RepoDHKonsolidasi.FindAll().Where(d => d.Id == id && d.PeriodStart == dtrDate && d.PeriodEnd == dtrDate2).FirstOrDefault();
            if (DHK != null)
            {
                foreach (Context.DaftarHargaKonsolidasiItem item in DHK.DaftarHargaKonsolidasiItem)
                {
                    listItem.Add(new DaftarHargaKonsolidasiItem(item));
                }
            }

            return new JavaScriptSerializer().Serialize(
                listItem.Select(i => new
                {
                    Id = i.Id,
                    NamaRuteDaftarHarga = i.NamaRuteDaftarHarga,
                    ListIdRute = i.ListIdRute,
                    ListNamaRute = i.ListNamaRute,
                    IdJenisKendaraan = i.IdJenisKendaraan,
                    NamaJenisKendaraan = i.NamaJenisKendaraan,
                    MinKg = i.MinKg,
                    MaxKg = i.MaxKg,
                    Harga = i.Harga,
                    IdSatuanHarga = i.IdSatuanHarga,
                    SatuanHarga = i.SatuanHarga,
                    IsAsuransi = i.IsAsuransi,
                    Premi = i.Premi,
                    TipeNilaiTanggungan = i.TipeNilaiTanggungan,
                    NilaiTanggungan = i.NilaiTanggungan,
                    PihakPenanggung = i.PihakPenanggung,
                    Keterangan = i.Keterangan
                }));
        }

        public String GetListRute(int idItem)
        {
            Context.DaftarHargaKonsolidasi DHK = RepoDHKonsolidasi.FindAll().Where(d=>d.DaftarHargaKonsolidasiItem.Any(i=>i.Id == idItem)).FirstOrDefault();

            return new JavaScriptSerializer().Serialize(
                DHK.DaftarHargaKonsolidasiItem.Where(d => d.Id == idItem).Select(i => i.ListIdRute).FirstOrDefault());
        }
        public JsonResult IsUsed(int id, bool isImported)
        {
            bool isExistData = false;            
            // fitur Import Daftar Harga
            if (isImported)
            {
                Context.DaftarHargaKonsolidasi dbItem = RepoDHKonsolidasi.FindByPK(id);
                List<int> listId = dbItem.DaftarHargaKonsolidasiItem.Where(i => i.IdDaftarHargaKonsolidasi == dbItem.Id).Select(i => i.Id).ToList();
                isExistData = RepoSalesOrder.FindAllKonsolidasi().Where(a => listId.Contains(a.SalesOrderKonsolidasi.IdDaftarHargaItem.Value)).Count() > 0;

            }
            // edit & delete grid item Daftar Harga
            else
            {
                isExistData = RepoSalesOrder.FindAllKonsolidasi().Where(a => a.SalesOrderKonsolidasi.IdDaftarHargaItem == id).Count() > 0;
            }

            return this.Json(isExistData, JsonRequestBehavior.AllowGet);
        }

        public bool isValidMinMax(int id, int val, string satuan)
        {
            Context.DaftarHargaKonsolidasi dbDh = RepoDHKonsolidasi.FindByItemId(id);
            Context.DaftarHargaKonsolidasiItem dbItem = dbDh.DaftarHargaKonsolidasiItem.Where(d => d.Id == id).FirstOrDefault();
            if (dbItem.LookupCodeSatuan.Nama == satuan)
                return (val >= dbItem.MinKg) && (val <= dbItem.MaxKg);
            else
                return true;
        }
        #endregion
    }

}