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
    public class DataBoronganController : BaseController
    {
        #region initial repo
        private IDataBoronganRepo RepoDataBorongan;
        private IMasterPoolRepo RepoPool;
        private IJenisTruckRepo RepoJenisTruck;
        private IMasterSolarRepo RepoSolar;
        private IFaktorBoronganRepo RepoFaktorBorongan;
        private IRuteRepo RepoRute;
        private IRuteTolRepo RepoRuteTol;
        private ISalesOrderRepo RepoSalesOrder;
        private IDaftarHargaOnCallRepo RepoDaftarHargaOncall;
        private IDaftarHargaKonsolidasiRepo RepoDaftarHargaKonsolidasi;
        private IDaftarHargaKontrakRepo RepoDaftarHargaKontrak;
        #endregion
        
        public DataBoronganController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDataBoronganRepo repoDataBorongan, IMasterPoolRepo repoPool,
            IJenisTruckRepo repoJenisTruck, IMasterSolarRepo repoSolar, IFaktorBoronganRepo repoFaktorBorongan, IRuteRepo repoRute, IRuteTolRepo repoRuteTol,
            ISalesOrderRepo repoSalesOrder, IDaftarHargaOnCallRepo repoDaftarHargaOncall, IDaftarHargaKonsolidasiRepo repoDaftarHargaKonsolidasi, IDaftarHargaKontrakRepo repoDaftarHargaKontrak)
            : base(repoBase, repoLookup)
        {
            RepoDataBorongan = repoDataBorongan;
            RepoPool = repoPool;
            RepoJenisTruck = repoJenisTruck;
            RepoSolar = repoSolar;
            RepoFaktorBorongan = repoFaktorBorongan;
            RepoRute = repoRute;
            RepoRuteTol = repoRuteTol;
            RepoSalesOrder = repoSalesOrder;
            RepoDaftarHargaOncall = repoDaftarHargaOncall;
            RepoDaftarHargaKonsolidasi = repoDaftarHargaKonsolidasi;
            RepoDaftarHargaKontrak = repoDaftarHargaKontrak;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "DataBorongan").ToList();
            return View();
        }

        public void GenerateAlokasi(DataBorongan model)
        {
            model.listSpbuBorongan = new List<DataBoronganSPBU>();
            for (int i = 0; i < 10; i++)
            {
                model.listSpbuBorongan.Add(new DataBoronganSPBU());
            }
            model.listKapalBorongan = new List<DataBoronganKapal>();
            for (int i = 0; i < 10; i++)
            {
                model.listKapalBorongan.Add(new DataBoronganKapal());
            }
            model.listTfBorongan = new List<DataBoronganTf>();
            for (int i = 0; i < 10; i++)
            {
                model.listTfBorongan.Add(new DataBoronganTf());
            }
        }

        public void SetAlokasi(DataBorongan model, Context.DataBorongan dbitem)
        {
            int idx = 0;

            if (dbitem.DataBoronganSPBU != null)
            {
                foreach (Context.DataBoronganSPBU item in dbitem.DataBoronganSPBU)
                {
                    model.listSpbuBorongan[idx].IdSPBU = item.IdLookupCodeSpbu;
                    model.listSpbuBorongan[idx].NamaSpbu = item.LookupCodeSpbu.Nama;
                    model.listSpbuBorongan[idx].value = item.value;
                    idx++;
                }
            }

            idx = 0;

            if (dbitem.DataBoronganKapal != null)
            {
                foreach (Context.DataBoronganKapal item in dbitem.DataBoronganKapal)
                {
                    model.listKapalBorongan[idx].IdKapal = item.IdLookupCodeKapal;
                    model.listKapalBorongan[idx].NamaPenyebrangan = item.LookupCodeKapal.Nama;
                    model.listKapalBorongan[idx].value = item.value;
                    idx++;
                }
            }

            idx = 0;

            if (dbitem.DataBoronganTf != null)
            {
                foreach (Context.DataBoronganTf item in dbitem.DataBoronganTf.OrderBy(d => d.Id))
                {
                    model.listTfBorongan[idx].value = item.value;
                    model.listTfBorongan[idx].Nama = "Transfer " + (idx + 1).ToString();
                    model.listTfBorongan[idx].LeadTime = item.LeadTime;
                    idx++;
                }
            }
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.DataBorongan> items = RepoDataBorongan.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<DataBorongan> ListModel = new List<DataBorongan>();
            foreach (Context.DataBorongan item in items)
            {
                ListModel.Add(new DataBorongan(item));
            }

            int total = RepoDataBorongan.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string BindingKontrak(int idtruck,int IdCustomer)
        {
            List<Context.DataBorongan> items = RepoDataBorongan.FindAll().Where(d => !d.IsTambahan && d.IdJenisTruck == idtruck && 
                ( d.CustomerId == IdCustomer || !d.CustomerId.HasValue)).ToList();

            List<DataBorongan> ListModel = new List<DataBorongan>();
            foreach (Context.DataBorongan item in items)
            {
                ListModel.Add(new DataBorongan(item));
            }

            return new JavaScriptSerializer().Serialize(new { total = ListModel.Count(), data = ListModel });
        }

        public string BindingHistory(int id)
        {
            Context.DataBorongan db = RepoDataBorongan.FindByPK(id);
            List<DataBorongan> listmodel = new List<DataBorongan>();
            foreach (Context.DataBoronganHistory item in db.DataBoronganHistory.ToList())
            {
                listmodel.Add(new DataBorongan(item));
            }
            return new JavaScriptSerializer().Serialize(listmodel);
        }
        public ActionResult Add()
        {
            DataBorongan model = new DataBorongan();
            GenerateAlokasi(model);
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(DataBorongan model)
        {
            if (ModelState.IsValid)
            {
                bool isPalid = true;
                if(model.IdMasterPool.HasValue && model.IdJenisTruck.HasValue)
                {
                    if (RepoDataBorongan.IsExist(model.IdMasterPool.Value, model.IdJenisTruck.Value, model.NamaBorongan, 0, model.CustomerId == null ? 0 : model.CustomerId.Value))
                    {
                        ModelState.AddModelError("IdMasterPool","Alokasi pool, truck dan nama borongan sudah terdaftar didatabase.");
                        ModelState.AddModelError("IdJenisTruck", "Alokasi pool, truck dan nama borongan sudah terdaftar didatabase.");
                        ModelState.AddModelError("NamaBorongan", "Alokasi pool, truck dan nama borongan sudah terdaftar didatabase.");
                        isPalid = false;
                    }
                }
                if(!model.IsTambahan)
                {
                    if(!model.IdJenisTruck.HasValue)
                    {
                        ModelState.AddModelError("IdJenisTruck","Jenis truck harus diisi.");
                        isPalid = false;
                    }
                    if(!model.IdMasterPool.HasValue)
                    {
                        ModelState.AddModelError("IdMasterPool","Master pool harus diisi.");
                        isPalid = false;
                    }
                    //if(model.CustomerId == null)
                    //{
                    //    ModelState.AddModelError("Customer","Customer harus diisi.");
                    //    isPalid = false;
                    //}
                    if(!model.Jarak.HasValue)
                    {
                        ModelState.AddModelError("Jarak","Jarak harus diisi.");
                        isPalid = false;
                    }
                    if(model.Rasio == "")
                    {
                        ModelState.AddModelError("Rasio","Rasio harus diisi.");
                        isPalid = false;
                    }
                    if(!model.WaktuHariKerja.HasValue)
                    {
                        ModelState.AddModelError("WaktuHariKerja","Waktu hari kerja harus diisi.");
                        isPalid = false;
                    }
                    if(!model.JumlahMakan.HasValue)
                    {
                        ModelState.AddModelError("JumlahMakan","Jumlah makan harus diisi.");
                        isPalid = false;
                    }
                    if(model.AreaUangMakan == "")
                    {
                        ModelState.AddModelError("AreaUangMakan","Area uang makan harus diisi.");
                        isPalid = false;
                    }
                    if(!model.BobotTipsParkir.HasValue)
                    {
                        ModelState.AddModelError("BobotTipsParkir","Bobot tips parkir harus diisi.");
                        isPalid = false;
                    }
                    if(!model.BobotGaji1.HasValue)
                    {
                        ModelState.AddModelError("BobotGaji1","Bobot gaji 1 harus diisi.");
                        isPalid = false;
                    }
                    if(!model.BobotGaji2.HasValue)
                    {
                        ModelState.AddModelError("BobotGaji2","Bobot gaji 1 harus diisi.");
                        isPalid = false;
                    }   
                }

                if(!isPalid)
                {
                    GenerateAlokasi(model);
                    return View("Form", model);
                }

                Context.DataBorongan dbitem = new Context.DataBorongan();
                model.SetDb(dbitem);
                Context.DataBoronganHistory dbhistory = new Context.DataBoronganHistory();
                model.SetDbHistory(dbhistory);
                dbitem.DataBoronganHistory.Add(dbhistory);
                
                RepoDataBorongan.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            GenerateAlokasi(model);
            return View("Form", model);
        }
        public ActionResult Edit(int id)
        {
            Context.DataBorongan dbitem = RepoDataBorongan.FindByPK(id);
            DataBorongan model = new DataBorongan(dbitem);
            GenerateAlokasi(model);
            SetAlokasi(model, dbitem);
            ViewBag.name = model.NamaBorongan;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(DataBorongan model)
        {
            Context.DataBorongan dbitem = RepoDataBorongan.FindByPK(model.Id);
            if (ModelState.IsValid)
            {
                bool isPalid = true;
                if (model.IdMasterPool.HasValue && model.IdJenisTruck.HasValue)
                {
                    if (RepoDataBorongan.IsExist(model.IdMasterPool.Value, model.IdJenisTruck.Value, model.NamaBorongan, model.Id))
                    {
                        ModelState.AddModelError("IdMasterPool", "Alokasi pool, truck dan nama borongan sudah terdaftar didatabase.");
                        ModelState.AddModelError("IdJenisTruck", "Alokasi pool, truck dan nama borongan sudah terdaftar didatabase.");
                        ModelState.AddModelError("NamaBorongan", "Alokasi pool, truck dan nama borongan sudah terdaftar didatabase.");
                        isPalid = false;
                    }
                }
                if (!model.IsTambahan)
                {
                    if (!model.IdJenisTruck.HasValue)
                    {
                        ModelState.AddModelError("IdJenisTruck", "Jenis truck harus diisi.");
                        isPalid = false;
                    }
                    if (!model.IdMasterPool.HasValue)
                    {
                        ModelState.AddModelError("IdMasterPool", "Master pool harus diisi.");
                        isPalid = false;
                    }
                    //if (model.CustomerId == null)
                    //{
                    //    ModelState.AddModelError("Customer", "Customer harus diisi.");
                    //    isPalid = false;
                    //}
                    if (!model.Jarak.HasValue)
                    {
                        ModelState.AddModelError("Jarak", "Jarak harus diisi.");
                        isPalid = false;
                    }
                    if (model.Rasio == "")
                    {
                        ModelState.AddModelError("Rasio", "Rasio harus diisi.");
                        isPalid = false;
                    }
                    if (!model.WaktuHariKerja.HasValue)
                    {
                        ModelState.AddModelError("WaktuHariKerja", "Waktu hari kerja harus diisi.");
                        isPalid = false;
                    }
                    if (!model.JumlahMakan.HasValue)
                    {
                        ModelState.AddModelError("JumlahMakan", "Jumlah makan harus diisi.");
                        isPalid = false;
                    }
                    if (model.AreaUangMakan == "")
                    {
                        ModelState.AddModelError("AreaUangMakan", "Area uang makan harus diisi.");
                        isPalid = false;
                    }
                    if (!model.BobotTipsParkir.HasValue)
                    {
                        ModelState.AddModelError("BobotTipsParkir", "Bobot tips parkir harus diisi.");
                        isPalid = false;
                    }
                    if (!model.BobotGaji1.HasValue)
                    {
                        ModelState.AddModelError("BobotGaji1", "Bobot gaji 1 harus diisi.");
                        isPalid = false;
                    }
                    if (!model.BobotGaji2.HasValue)
                    {
                        ModelState.AddModelError("BobotGaji2", "Bobot gaji 1 harus diisi.");
                        isPalid = false;
                    }
                }

                if (!isPalid)
                {
                    GenerateAlokasi(model);
                    return View("Form", model);
                }

                model.SetDb(dbitem);
                Context.DataBoronganHistory dbhistory = new Context.DataBoronganHistory();
                model.SetDbHistory(dbhistory);
                dbitem.DataBoronganHistory.Add(dbhistory);

                RepoDataBorongan.save(dbitem, UserPrincipal.id);

                return RedirectToAction("Index");
            }
            GenerateAlokasi(model);
            SetAlokasi(model, dbitem);
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.DataBorongan dbItem = RepoDataBorongan.FindByPK(id);

            RepoDataBorongan.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        #region options
        public JsonResult GetTotalTol(List<int> ListRute)
        {
            ResponeModel response = new ResponeModel();
            List<Context.RuteTol> dbrutetol = RepoRuteTol.FindAll();
            List<int> ListIdTolBrkt = new List<int>(); 
            List<int> ListIdTolPulang = new List<int>(); 

            int gol1 = 0, gol2 = 0, gol3 = 0, gol4 = 0;
            foreach (int id in ListRute)
            {
                Context.RuteTol dummy = dbrutetol.Where(d => d.IdRute == id).FirstOrDefault();
                if(dummy != null)
                {
                    foreach(Context.TolBerangkat item in dummy.ListTolBerangkat)
                    {
                        gol1 += int.Parse(item.JnsTol.GolonganTol1.ToString());
                        gol2 += int.Parse(item.JnsTol.GolonganTol2.ToString());
                        gol3 += int.Parse(item.JnsTol.GolonganTol3.ToString());
                        gol4 += int.Parse(item.JnsTol.GolonganTol4.ToString());

                        ListIdTolBrkt.Add(item.IdTol.Value);
                    }

                    foreach (Context.TolPulang item in dummy.ListTolPulang)
                    {
                        gol1 += int.Parse(item.JnsTol.GolonganTol1.ToString());
                        gol2 += int.Parse(item.JnsTol.GolonganTol2.ToString());
                        gol3 += int.Parse(item.JnsTol.GolonganTol3.ToString());
                        gol4 += int.Parse(item.JnsTol.GolonganTol4.ToString());

                        ListIdTolPulang.Add(item.IdTol.Value);
                    }
                }
                
            }
            response.Success = true;
            response.Data = new JavaScriptSerializer().Serialize(new { gol1 = gol1, gol2 = gol2, gol3 = gol3, gol4 = gol4 });

            return Json(response);
        }
        public string GetDataForDispatch(int idSo, string listIdso = "")
        {
            int IdCustomer, IdJenisTruck;
            string IdRute;
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);
            if (dbso.SalesOrderOncallId != null)
            {
                IdCustomer = dbso.SalesOrderOncall.CustomerId.Value;

                RepoDaftarHargaOncall.FindRuteTruk(dbso.SalesOrderOncall.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);

                //aya anu salah jadi diisi deui jenistruk na
                if (dbso.SalesOrderOncall.IdDataTruck.HasValue)
                {
                    IdJenisTruck = dbso.SalesOrderOncall.DataTruck.IdJenisTruck.Value;
                }
                else
                {
                    IdJenisTruck = dbso.SalesOrderOncall.JenisTruckId.Value;
                }

                List<Context.DataBorongan> dbborongan = RepoDataBorongan.FindAll()
                    .Where(d => d.IsTambahan == false && (!d.CustomerId.HasValue || d.CustomerId == IdCustomer) && d.IdJenisTruck == IdJenisTruck &&
                        d.DataBoronganRute.Any(r => IdRute.Split(',').Any(e => int.Parse(e) == r.IdRute))
                    ).OrderByDescending(x => x.DataBoronganRute.Where(r => IdRute.Split(',').Any(e => int.Parse(e) == r.IdRute)).FirstOrDefault().Id).ToList();

                //Context.Rute dbrute = RepoRute.FindByPK(_idRute);

                List<DataBorongan> ListModel = new List<DataBorongan>();
                foreach (Context.DataBorongan item in dbborongan)
                {
                    DataBorongan _model = new DataBorongan(item);
                    GenerateAlokasi(_model);
                    SetAlokasi(_model, item);
                    _model.listSpbuBorongan = _model.listSpbuBorongan.Where(d => d.IdSPBU.HasValue).ToList();
                    _model.listKapalBorongan = _model.listKapalBorongan.Where(d => d.IdKapal.HasValue).ToList();
                    _model.listTfBorongan = _model.listTfBorongan.Where(d => d.value >= 0).ToList();
                    ListModel.Add(_model);
                }

                return new JavaScriptSerializer().Serialize(ListModel);
            }
            else if (dbso.SalesOrderPickupId != null)
            {
                IdCustomer = dbso.SalesOrderPickup.CustomerId.Value;

                List<Context.DataBorongan> dbborongan = RepoDataBorongan.FindAll()
                    .Where(d => d.IsTambahan == false &&
                        (!d.CustomerId.HasValue || d.CustomerId == IdCustomer) &&
                        d.IdJenisTruck == dbso.SalesOrderPickup.DataTruck.IdJenisTruck &&
                        d.DataBoronganRute.Any(r => r.IdRute == dbso.SalesOrderPickup.RuteId)).ToList();

                //Context.Rute dbrute = RepoRute.FindByPK(_idRute);

                List<DataBorongan> ListModel = new List<DataBorongan>();
                foreach (Context.DataBorongan item in dbborongan)
                {
                    DataBorongan _model = new DataBorongan(item);
                    GenerateAlokasi(_model);
                    SetAlokasi(_model, item);
                    _model.listSpbuBorongan = _model.listSpbuBorongan.Where(d => d.IdSPBU.HasValue).ToList();
                    _model.listKapalBorongan = _model.listKapalBorongan.Where(d => d.IdKapal.HasValue).ToList();
                    _model.listTfBorongan = _model.listTfBorongan.Where(d => d.value >= 0).ToList();
                    ListModel.Add(_model);
                }

                return new JavaScriptSerializer().Serialize(ListModel);
            }
            else if (dbso.SalesOrderProsesKonsolidasiId != null)
            {
                //RepoDaftarHargaKonsolidasi.FindRuteTruk(dbso.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);

                //int _idRute = int.Parse(IdRute.Split(',').FirstOrDefault());

                //tambahan gara" aya anu salah, lieur anjing ah
                if (dbso.SalesOrderProsesKonsolidasi.IdDataTruck.HasValue)
                {
                    IdJenisTruck = dbso.SalesOrderProsesKonsolidasi.DataTruck.IdJenisTruck.Value;
                }
                else
                {
                    IdJenisTruck = dbso.SalesOrderProsesKonsolidasi.JenisTruckId.Value;
                }

                List<Context.DataBorongan> dbborongan = RepoDataBorongan.FindAll()
                    .Where(d => d.IsTambahan == false && d.IdJenisTruck == IdJenisTruck && d.DataBoronganRute.Any(r => r.IdRute == dbso.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value)).ToList();

                //Context.Rute dbrute = RepoRute.FindByPK(_idRute);

                List<DataBorongan> ListModel = new List<DataBorongan>();
                foreach (Context.DataBorongan item in dbborongan)
                {
                    DataBorongan _model = new DataBorongan(item);
                    GenerateAlokasi(_model);
                    SetAlokasi(_model, item);
                    _model.listSpbuBorongan = _model.listSpbuBorongan.Where(d => d.IdSPBU.HasValue).ToList();
                    _model.listKapalBorongan = _model.listKapalBorongan.Where(d => d.IdKapal.HasValue).ToList();
                    _model.listTfBorongan = _model.listTfBorongan.Where(d => d.value >= 0).ToList();
                    ListModel.Add(_model);
                }

                return new JavaScriptSerializer().Serialize(ListModel);
            }
            else if (dbso.SalesOrderKontrakId != null)
            {
                //List<string> listSo = listIdso.Split( new string[]{ "||" },StringSplitOptions.None).ToList();
                //foreach (var item in listSo)
                //{

                //}
                //RepoDaftarHargaKontrak.FindRuteTruk(dbso.SalesOrderProsesKonsolidasi.IdDaftarHargaItem.Value, out IdRute, out IdJenisTruck);

                //int _idRute = int.Parse(IdRute.Split(',').FirstOrDefault());

                //List<Context.DataBorongan> dbborongan = RepoDataBorongan.FindAll()
                //    .Where(d => d.IsTambahan == false && !d.CustomerId.HasValue &&
                //        d.IdJenisTruck == dbso.SalesOrderProsesKonsolidasi.DataTruck.IdJenisTruck &&
                //        d.DataBoronganRute.Any(r => r.IdRute == dbso.SalesOrderProsesKonsolidasi.IdDaftarHargaItem)).ToList();

                ////Context.Rute dbrute = RepoRute.FindByPK(_idRute);

                //List<DataBorongan> ListModel = new List<DataBorongan>();
                //foreach (Context.DataBorongan item in dbborongan)
                //{
                //    DataBorongan _model = new DataBorongan(item);
                //    GenerateAlokasi(_model);
                //    SetAlokasi(_model, item);
                //    _model.listSpbuBorongan = _model.listSpbuBorongan.Where(d => d.IdSPBU.HasValue).ToList();
                //    _model.listKapalBorongan = _model.listKapalBorongan.Where(d => d.IdKapal.HasValue).ToList();
                //    _model.listTfBorongan = _model.listTfBorongan.Where(d => d.value > 0).ToList();
                //    ListModel.Add(_model);
                //}

                //return new JavaScriptSerializer().Serialize(ListModel);
            }
            return "Invalid Sales Order";
        }
        public string GetBoronganTambahan(int idTruck)
        {
            List<Context.DataBorongan> dbborongan = RepoDataBorongan.FindAll().Where(d => d.IsTambahan == true && d.IdJenisTruck == idTruck).ToList();

            List<DataBorongan> ListModel = new List<DataBorongan>();
            foreach (Context.DataBorongan item in dbborongan)
            {
                DataBorongan _model = new DataBorongan(item);
                GenerateAlokasi(_model);
                SetAlokasi(_model, item);
                _model.listSpbuBorongan = _model.listSpbuBorongan.Where(d => d.IdSPBU.HasValue).ToList();
                _model.listKapalBorongan = _model.listKapalBorongan.Where(d => d.IdKapal.HasValue).ToList();
                _model.listTfBorongan = _model.listTfBorongan.Where(d => d.value > 0).ToList();
                ListModel.Add(_model);
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public string FindById(int id)
        {
            Context.DataBorongan db = RepoDataBorongan.FindByPK(id);

            return new JavaScriptSerializer().Serialize(new DataBorongan(db));
        }
        public string FindList(string ListId)
        {
            List<DataBorongan> ListModel = new List<DataBorongan>();
            foreach (string id in ListId.Split('.'))
            {
                Context.DataBorongan db = RepoDataBorongan.FindByPK(int.Parse(id));
                DataBorongan _model = new DataBorongan(db);
                GenerateAlokasi(_model);
                SetAlokasi(_model, db);
                _model.listSpbuBorongan = _model.listSpbuBorongan.Where(d => d.IdSPBU.HasValue).ToList();
                _model.listKapalBorongan = _model.listKapalBorongan.Where(d => d.IdKapal.HasValue).ToList();
                _model.listTfBorongan = _model.listTfBorongan.Where(d => d.value >= 0).ToList();
                ListModel.Add(_model);
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        #endregion

        #region fungsi faktor borongan
        private Decimal GetRasioSolar(Context.FaktorBorongan dbitem, string rasio)
        {
            if (rasio == "KOTA1")
                return dbitem.RasioDlmKota;
            else if (rasio == "KOTA2")
                return dbitem.RasioDlmKota2;
            else if (rasio == "JAWABALI")
                return dbitem.RasioJawaBali;
            else if (rasio == "SUMATRA")
                return dbitem.RasioSumatra;
            else if (rasio == "KOSONG")
                return dbitem.RasioKosong;
            else
                return 0;
        }

        private int GetRasioByText(string rasio)
        {
            if (rasio == "KOTA1")
                return 1;
            else if (rasio == "KOTA2")
                return 2;
            else if (rasio == "JAWABALI")
                return 3;
            else if (rasio == "SUMATRA")
                return 4;
            else if (rasio == "KOSONG")
                return 4;
            else
                return 0;
        }

        private Decimal GetUangMakan(Context.FaktorBorongan dbitem, string areaMakan)
        {
            if (areaMakan == "JAWABALI")
                return dbitem.UangMakanJawaBali;
            else if (areaMakan == "SUMATRA")
                return dbitem.UangMakanSumatra;
            else
                return 0;
        }

        private int GetAreaMakanByText(string areaMakan)
        {
            if (areaMakan == "JAWABALI")
                return 1;
            else if (areaMakan == "SUMATRA")
                return 2;
            else
                return 0;
        }

        private Decimal GetBiayaKapal(Context.FaktorBorongan dbitem, string kapal)
        {
            if (kapal == "BALI")
                return dbitem.BiayaKapalBali;
            else if (kapal == "BALI_NTB")
                return dbitem.BiayaKapalBaliNTB;
            else if (kapal == "SUMATRA")
                return dbitem.BiayaKapalSumatra;
            else if (kapal == "KALIMANTAN")
                return dbitem.BiayaKapalKalimantan;
            else if (kapal == "SULAWESI")
                return dbitem.BiayaKapalSulawesi;
            else
                return 0;
        }

        private int GetKapalByText(string kapal)
        {
            if (kapal == "BALI")
                return 1;
            else if (kapal == "BALI_NTB")
                return 2;
            else if (kapal == "SUMATRA")
                return 3;
            else if (kapal == "KALIMANTAN")
                return 4;
            else if (kapal == "SULAWESI")
                return 5;
            else
                return 0;
        }

        #endregion

        #region export import

        public string Upload(IEnumerable<HttpPostedFileBase> files)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (files != null)
            {
                foreach (var file in files)
                {
/*                    try
                    {*/
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfRow = workSheet.Dimension.End.Row;

                            DateTime currDate = DateTime.Now.Date;
                            Decimal? HaragaSolar = RepoSolar.FindAll().Where(d => currDate >= d.Start && currDate <= d.End).FirstOrDefault().Harga;
                            HaragaSolar = !HaragaSolar.HasValue ? 0 : HaragaSolar;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 3].Value != null)
                                {
                                    int id = 0;
                                    int resId;
                                    if (workSheet.Cells[rowIterator, 21].Value != null)
                                    {
                                        if (int.TryParse(workSheet.Cells[rowIterator, 21].Value.ToString(), out resId))
                                            id = resId;
                                    }

                                    Context.DataBorongan dbitem = new Context.DataBorongan();
                       /*             try
                                    {*/
                                        if (workSheet.Cells[rowIterator, 1].Value != null)
                                        {
                                            if (RepoPool.FindByNamePool(workSheet.Cells[rowIterator, 1].Value.ToString()) != null)
                                                dbitem.IdMasterPool = RepoPool.FindByNamePool(workSheet.Cells[rowIterator, 1].Value.ToString()).Id;
                                        }
                                        if (workSheet.Cells[rowIterator, 2].Value != null)
                                        {
                                            if (RepoJenisTruck.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()) != null)
                                                dbitem.IdJenisTruck = RepoJenisTruck.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                                        }

                                        //faktor pengali kosong
                                        Context.FaktorBorongan dbfaktor = new Context.FaktorBorongan();
                                        if (dbitem.IdMasterPool != null && dbitem.IdJenisTruck != null)
                                        {
                                            if (id != 0)
                                            {
                                                if (RepoDataBorongan.IsExist(RepoPool.FindByNamePool(workSheet.Cells[rowIterator, 1].Value.ToString()).Id,
                                                    RepoJenisTruck.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id, workSheet.Cells[rowIterator, 2].Value.ToString(), id))
                                                {
                                                    continue;
                                                }
                                                dbitem = RepoDataBorongan.FindByPK(id);
                                            }
                                            else
                                            {
                                                if (RepoDataBorongan.IsExist(RepoPool.FindByNamePool(workSheet.Cells[rowIterator, 1].Value.ToString()).Id,
                                                    RepoJenisTruck.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id, workSheet.Cells[rowIterator, 2].Value.ToString()))
                                                {
                                                    continue;
                                                }
                                            }
                                            dbfaktor = RepoFaktorBorongan.FindAll().Where(d => d.IdMasterPool == dbitem.IdMasterPool && d.IdJenisTruck == dbitem.IdJenisTruck).FirstOrDefault();
                                        }
                                        
                                        if (dbfaktor == null)
                                        {
                                            dbfaktor.BiayaKapalBali = 0;
                                            dbfaktor.BiayaKapalBaliNTB = 0;
                                            dbfaktor.BiayaKapalKalimantan = 0;
                                            dbfaktor.BiayaKapalSulawesi = 0;
                                            dbfaktor.BiayaKapalSumatra = 0;
                                            dbfaktor.FaktorPengaliGaji = 0;
                                            dbfaktor.FaktorPengaliTips = 0;
                                            dbfaktor.PotonganDriver1 = 0;
                                            dbfaktor.PotonganDriver2 = 0;
                                            dbfaktor.RasioDlmKota = 0;
                                            dbfaktor.RasioDlmKota2 = 0;
                                            dbfaktor.RasioJawaBali = 0;
                                            dbfaktor.RasioKosong = 0;
                                            dbfaktor.RasioSumatra = 0;
                                            dbfaktor.UangMakanJawaBali = 0;
                                            dbfaktor.UangMakanSumatra = 0;
                                        }
                                            //continue;
                                        dbitem.NamaBorongan = workSheet.Cells[rowIterator, 3].Value.ToString();
                                        //dbitem.Customer = workSheet.Cells[rowIterator, 4].Value.ToString() == "CPI" ? "CPI" : workSheet.Cells[rowIterator, 4].Value.ToString() == "Umum" ? "UMUM" : "";
                                        //if (dbitem.Customer == "") continue;
                                        dbitem.Jarak = workSheet.Cells[rowIterator, 5].Value == null ? 0 : int.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        if (workSheet.Cells[rowIterator, 6].Value != null)
                                        {
                                            dbitem.Rasio = workSheet.Cells[rowIterator, 6].Value.ToString() == "RASIO DALAM KOTA" ? "KOTA1" :
									        workSheet.Cells[rowIterator, 6].Value.ToString() == "RASIO DALAM KOTA 2" ? "KOTA2" :
									        workSheet.Cells[rowIterator, 6].Value.ToString() == "RASIO JAWA BALI" ? "JAWABALI" :
									        workSheet.Cells[rowIterator, 6].Value.ToString() == "RASIO SUMATERA" ? "SUMATRA" :
									        workSheet.Cells[rowIterator, 6].Value.ToString() == "RASIO KOSONG" ? "KOSONG" : "";
                                            if (dbitem.Rasio == "")
                                            { }  //continue;
                                            else
                                            {
                                                Decimal rasio = GetRasioSolar(dbfaktor, dbitem.Rasio);
                                                dbitem.LiterSolar = rasio > 0 ? (dbitem.Jarak / rasio) : 0;
                                                dbitem.HargaSolar = rasio > 0 ? (dbitem.Jarak / rasio) * HaragaSolar.Value : 0;
                                                dbitem.TotalBorongan += dbitem.HargaSolar;
                                            }
                                        }

                                        dbitem.WaktuHariKerja = workSheet.Cells[rowIterator, 7].Value == null ? 0 : decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString());
                                        dbitem.JumlahMakan = workSheet.Cells[rowIterator, 8].Value == null ? 0 : decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                                        dbitem.AlokasiCash = workSheet.Cells[rowIterator, 81].Value == null ? 0 : decimal.Parse(workSheet.Cells[rowIterator, 81].Value.ToString());
                                        if (workSheet.Cells[rowIterator, 9].Value != null)
                                        {
                                            dbitem.AreaUangMakan = workSheet.Cells[rowIterator, 9].Value.ToString() == "Jawa Bali" ? "JAWABALI" :
                                                workSheet.Cells[rowIterator, 9].Value.ToString() == "Sumatera" ? "SUMATRA" : "";
                                        }
                                        
                                        if (dbitem.AreaUangMakan == "")
                                        { }   //continue;
                                        else
                                        {
                                            Decimal UangMakan = GetUangMakan(dbfaktor, dbitem.AreaUangMakan);
                                            dbitem.UangMakan = dbitem.JumlahMakan * UangMakan;
                                            dbitem.TotalBorongan += dbitem.UangMakan;
                                        }
                                        dbitem.BiayaTol = workSheet.Cells[rowIterator, 10].Value == null ? 0 : Decimal.Parse(workSheet.Cells[rowIterator, 10].Value.ToString());
                                        dbitem.TotalBorongan += dbitem.BiayaTol;
                                        dbitem.BobotTipsParkir = workSheet.Cells[rowIterator, 11].Value == null ? 0 : int.Parse(workSheet.Cells[rowIterator, 11].Value.ToString());
                                        dbitem.TipsParkir = dbitem.BobotTipsParkir * dbfaktor.FaktorPengaliTips;
                                        dbitem.TotalBorongan += dbitem.TipsParkir;
                                        dbitem.BobotGaji1 = workSheet.Cells[rowIterator, 12].Value == null ? 0 : decimal.Parse(workSheet.Cells[rowIterator, 12].Value.ToString());
                                        dbitem.gaji1 = dbitem.BobotGaji1 * dbfaktor.FaktorPengaliGaji;
                                        dbitem.BobotGaji2 = workSheet.Cells[rowIterator, 13].Value == null ? 0 : decimal.Parse(workSheet.Cells[rowIterator, 13].Value.ToString());
                                        dbitem.gaji2 = dbitem.BobotGaji2 * dbfaktor.FaktorPengaliGaji;
                                        dbitem.TotalGaji = dbitem.gaji1 + dbitem.gaji2;
                                        dbitem.TotalBorongan += dbitem.TotalGaji;
                                        if (workSheet.Cells[rowIterator, 14].Value != null)
                                        {
                                            dbitem.Kapal = workSheet.Cells[rowIterator, 14].Value.ToString() == "Bali" ? "BALI" :
                                                workSheet.Cells[rowIterator, 14].Value.ToString() == "Bali NTB" ? "BALI_NTB" :
                                                workSheet.Cells[rowIterator, 14].Value.ToString() == "Sumatera" ? "SUMATRA" :
                                                workSheet.Cells[rowIterator, 14].Value.ToString() == "Kalimantan" ? "KALIMANTAN" :
                                                workSheet.Cells[rowIterator, 14].Value.ToString() == "Sulawesi" ? "SULAWESI" : "";
                                            if (dbitem.Kapal == "")
                                            {}//continue;
                                            else
                                            {
                                                dbitem.BiayaKapal = GetBiayaKapal(dbfaktor, dbitem.Kapal);
                                                dbitem.TotalBorongan += dbitem.BiayaKapal;
                                            }
                                        }
                                        dbitem.BoronganDasar = dbitem.TotalBorongan;
                                        if (workSheet.Cells[rowIterator, 15].Value != null)
                                        {
                                            dbitem.Kawalan = Decimal.Parse(workSheet.Cells[rowIterator, 15].Value.ToString());
                                            dbitem.TotalBorongan += dbitem.Kawalan.Value;
                                        }
                                        if (workSheet.Cells[rowIterator, 16].Value != null)
                                        {
                                            dbitem.Timbangan = Decimal.Parse(workSheet.Cells[rowIterator, 16].Value.ToString());
                                            dbitem.TotalBorongan += dbitem.Timbangan.Value;
                                        }
                                        if (workSheet.Cells[rowIterator, 17].Value != null)
                                        {
                                            dbitem.Karantina = Decimal.Parse(workSheet.Cells[rowIterator, 17].Value.ToString());
                                            dbitem.TotalBorongan += dbitem.Karantina.Value;
                                        }
                                        if (workSheet.Cells[rowIterator, 18].Value != null)
                                        {
                                            dbitem.SPSI = Decimal.Parse(workSheet.Cells[rowIterator, 18].Value.ToString());
                                            dbitem.TotalBorongan += dbitem.SPSI.Value;
                                        }
                                        if (workSheet.Cells[rowIterator, 19].Value != null)
                                        {
                                            dbitem.MultiDrop = Decimal.Parse(workSheet.Cells[rowIterator, 19].Value.ToString());
                                            dbitem.TotalBorongan += dbitem.MultiDrop.Value;
                                        }

                                        //history
                                        Context.DataBoronganHistory dbhistory = new Context.DataBoronganHistory();
                                        dbhistory.IsTambahan = dbitem.IsTambahan;
                                        dbhistory.Tanggal = DateTime.Now;
                                        dbhistory.NamaBorongan = dbitem.NamaBorongan;
                                        dbhistory.Jarak = dbitem.Jarak;
                                        dbhistory.Customer = dbitem.CustomerId == null ? "" : dbitem.Customer.CustomerNama;
                                        dbhistory.Rasio = dbitem.Rasio;
                                        dbhistory.LiterSolar = dbitem.LiterSolar;
                                        dbhistory.HargaSolar = dbitem.HargaSolar;
                                        dbhistory.WaktuHariKerja = dbitem.WaktuHariKerja;
                                        dbhistory.JumlahMakan = dbitem.JumlahMakan;
                                        dbhistory.AreaUangMakan = dbitem.AreaUangMakan;
                                        dbhistory.UangMakan = dbitem.UangMakan;
                                        dbhistory.BiayaTol = dbitem.BiayaTol;
                                        dbhistory.BobotTipsParkir = dbitem.BobotTipsParkir;
                                        dbhistory.TipsParkir = dbitem.TipsParkir;
                                        dbhistory.BobotGaji1 = dbitem.BobotGaji1;
                                        dbhistory.gaji1 = dbitem.gaji1;
                                        dbhistory.BobotGaji2 = dbitem.BobotGaji2;
                                        dbhistory.gaji2 = dbitem.gaji2;
                                        dbhistory.TotalGaji = dbitem.TotalGaji;
                                        dbhistory.Kapal = dbitem.Kapal;
                                        dbhistory.BiayaKapal = dbitem.BiayaKapal;
                                        dbhistory.BoronganDasar = dbitem.BoronganDasar;
                                        dbhistory.Kawalan = dbitem.Kawalan;
                                        dbhistory.Timbangan = dbitem.Timbangan;
                                        dbhistory.Karantina = dbitem.Karantina;
                                        dbhistory.SPSI = dbitem.SPSI;
                                        dbhistory.MultiDrop = dbitem.MultiDrop;
                                        dbhistory.TotalBorongan = dbitem.TotalBorongan;
                                        dbhistory.Pembulatan = dbitem.Pembulatan;
                                        dbitem.DataBoronganHistory.Add(dbhistory);

                                        response.Message = response.Message + Environment.NewLine + rowIterator; 

                                        //rute
                                        int idy = 0;
                                        for (idy = rowIterator; idy <= noOfRow; idy++)
                                        {
                                            if (workSheet.Cells[idy, 1].Value == null || idy == rowIterator)
                                            {
                                                if (workSheet.Cells[idy, 20].Value != null)
                                                {
                                                    Context.DataBoronganRute item = new Context.DataBoronganRute();
                                                    item.IdRute = RepoRute.FindByKode(workSheet.Cells[idy, 20].Value.ToString()).Id;
                                                    dbitem.DataBoronganRute.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        if (idy != 0)
                                            rowIterator = idy - 1;

                                        //SPBU
                                        int idx = 0;
                                        for (idx = 0; idx < 10; idx=idx+2)
                                        {
                                            if (workSheet.Cells[rowIterator, 21+idx].Value != null)
                                            {
                                                Context.DataBoronganSPBU dbSPBU = new Context.DataBoronganSPBU();
		                                        dbSPBU.IdLookupCodeSpbu = RepoLookup.FindByName(workSheet.Cells[rowIterator, 21+idx].Value.ToString()).Id; //workSheet.Cells[rowIterator, 21].Value.ToString();
		                                        dbSPBU.value = Convert.ToDecimal(workSheet.Cells[rowIterator, 22+idx].Value.ToString());
                                                dbitem.DataBoronganSPBU.Add(dbSPBU);
                                            }
                                        }
                                        //SPBU
                                        idx = 0;
                                        for (idx = 0; idx < 10; idx=idx+2)
                                        {
                                            if (workSheet.Cells[rowIterator, 41+idx].Value != null)
                                            {
                                                Context.DataBoronganKapal dbKapal = new Context.DataBoronganKapal();
		                                        dbKapal.IdLookupCodeKapal = RepoLookup.FindByName(workSheet.Cells[rowIterator, 41+idx].Value.ToString()).Id; //workSheet.Cells[rowIterator, 21].Value.ToString();
		                                        dbKapal.value = Convert.ToDecimal(workSheet.Cells[rowIterator, 42+idx].Value.ToString());
                                                dbitem.DataBoronganKapal.Add(dbKapal);
                                            }
                                        }
                                        //Tf
                                        idx = 0;
                                        for (idx = 0; idx < 10; idx=idx+2)
                                        {
                                            if (workSheet.Cells[rowIterator, 61+idx].Value != null)
                                            {
                                                Context.DataBoronganTf dbTf = new Context.DataBoronganTf();
		                                        dbTf.value = Convert.ToDecimal(workSheet.Cells[rowIterator, 61+idx].Value.ToString());
                                                if (workSheet.Cells[rowIterator, 62+idx].Value != null)
    		                                        dbTf.LeadTime = int.Parse(workSheet.Cells[rowIterator, 62+idx].Value.ToString());
                                                dbitem.DataBoronganTf.Add(dbTf);
                                            }
                                        }

                                        RepoDataBorongan.save(dbitem, UserPrincipal.id);
                                    /*}
                                    catch (Exception e)
                                    {
                                        response.Message = response.Message + Environment.NewLine + " Baris : " + rowIterator + " " + e.StackTrace;
                                    }*/
                                }
                            }
                        }
                        response.Success = true;
/*                    }
                    catch (Exception e)
                    {
                        response.Success = false;
                        response.Message = e.Message.ToString();
                    }*/

                }
            }

            return new JavaScriptSerializer().Serialize(new { Response = response });
        }

        public FileContentResult Export()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.DataBorongan> dbitems = RepoDataBorongan.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Alokasi Pool";
            ws.Cells[1, 2].Value = "Jenis Truck";
            ws.Cells[1, 3].Value = "Nama Borongan";
            ws.Cells[1, 4].Value = "Customer";
            ws.Cells[1, 5].Value = "Jarak";
            ws.Cells[1, 6].Value = "Rasio";
            ws.Cells[1, 7].Value = "Waktu Hari kerja";
            ws.Cells[1, 8].Value = "Jumlah Makan";
            ws.Cells[1, 9].Value = "Area Makan";
            ws.Cells[1, 10].Value = "Biaya Tol";
            ws.Cells[1, 11].Value = "Bobot Tips parkir";
            ws.Cells[1, 12].Value = "Bobot Gaji 1";
            ws.Cells[1, 13].Value = "Bobot Gaji 2";
            ws.Cells[1, 14].Value = "Kapal";
            ws.Cells[1, 15].Value = "Kawalan";
            ws.Cells[1, 16].Value = "Timbangan";
            ws.Cells[1, 17].Value = "karantina";
            ws.Cells[1, 18].Value = "SPSI";
            ws.Cells[1, 19].Value = "Multidrop";
            ws.Cells[1, 20].Value = "Rute";
            ws.Cells[1, 21].Value = "Id Database";

            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[idx, 1].Value = dbitems[i].MasterPool == null ? "" : dbitems[i].MasterPool.NamePool;
                ws.Cells[idx, 2].Value = dbitems[i].JenisTrucks == null ? "" : dbitems[i].JenisTrucks.StrJenisTruck;
                ws.Cells[idx, 3].Value = dbitems[i].NamaBorongan;
                ws.Cells[idx, 4].Value = dbitems[i].CustomerId == null ? "" : dbitems[i].Customer.CustomerCode;
                ws.Cells[idx, 5].Value = dbitems[i].Jarak;
                ws.Cells[idx, 6].Value = GetRasioByText(dbitems[i].Rasio);
                ws.Cells[idx, 7].Value = dbitems[i].Rasio;
                ws.Cells[idx, 8].Value = dbitems[i].JumlahMakan;
                ws.Cells[idx, 9].Value = GetAreaMakanByText(dbitems[i].AreaUangMakan);
                ws.Cells[idx, 10].Value = dbitems[i].BiayaTol;
                ws.Cells[idx, 11].Value = dbitems[i].BobotTipsParkir;
                ws.Cells[idx, 12].Value = dbitems[i].BobotGaji1;
                ws.Cells[idx, 13].Value = dbitems[i].BobotGaji2;
                ws.Cells[idx, 14].Value = GetKapalByText(dbitems[i].Kapal);
                ws.Cells[idx, 15].Value = dbitems[i].Kawalan;
                ws.Cells[idx, 16].Value = dbitems[i].Timbangan;
                ws.Cells[idx, 17].Value = dbitems[i].Karantina;
                ws.Cells[idx, 18].Value = dbitems[i].SPSI;
                ws.Cells[idx, 19].Value = dbitems[i].MultiDrop;
                ws.Cells[idx, 21].Value = dbitems[i].Id;

                foreach (Context.DataBoronganRute item in dbitems[i].DataBoronganRute)
                {
                    ws.Cells[idx, 20].Value = item.Rute.Kode;
                    idx++;
                }
            }

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Data Borongan.xls";

            return fsr;
        }

        #endregion
    }
}