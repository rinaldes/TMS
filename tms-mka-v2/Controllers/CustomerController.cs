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
    public class CustomerController : BaseController
    {
        private ICustomerRepo RepoCustomer;
        private IProductRepo RepoProduct;
        private IRekeningRepo RepoRekening;
        private IJenisTruckRepo RepoTruck;
        private ILocationRepo RepoLocation;
        private IRuteRepo RepoRute;
        private IDokumenRepo RepoDokumen;
        private Iptnr_mstrRepo Repoptnr_mstr;
        private Iptnra_addrRepo Repoptnra_addr;
        private IAuditrailRepo RepoAuditrail;

        public CustomerController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IProductRepo repoProduct, ICustomerRepo repoCustomer, IRekeningRepo repoRekening, IJenisTruckRepo repoTruck,
            ILocationRepo repoLocation, IRuteRepo repoRute, IDokumenRepo repoDokumen, Iptnr_mstrRepo repoptnr_mstr, Iptnra_addrRepo repoptnra_addr, IAuditrailRepo repoAuditrail)
            : base(repoBase, repoLookup)
        {
            RepoCustomer = repoCustomer;
            RepoLocation = repoLocation;
            RepoProduct = repoProduct;
            RepoRekening = repoRekening;
            RepoTruck = repoTruck;
            RepoRute = repoRute;
            RepoDokumen = repoDokumen;
            Repoptnr_mstr = repoptnr_mstr;
            Repoptnra_addr = repoptnra_addr;
            RepoAuditrail = repoAuditrail;
        }
        [MyAuthorize(Menu = "Customer", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Customer").ToList();

            return View();
        }
 
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Customer> items = RepoCustomer.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<Customer> ListModel = new List<Customer>();
            foreach (Context.Customer item in items)
            {
                ListModel.Add(new Customer(item));
            }

            int total = RepoCustomer.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new
            {
                total = total,
                data = ListModel.Select(d => new
                {
                    Id = d.Id,
                    CustomerCode = d.CustomerCode,
                    CustomerCodeOld = d.CustomerCodeOld,
                    CustomerNama = d.CustomerNama,
                    StrPrioritas = d.StrPrioritas,
                    WajibPO = d.WajibPO,
                    WajibGPS = d.WajibGPS,
                    CustomerFullName = d.CustomerCodeOld + " - " + d.CustomerNama,
                    SpecialTreatment = d.SpecialTreatment,
                    Keterangan = d.Keterangan
                })
            });
        }
        public string GetVendor()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Customer> items = RepoCustomer.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).Where(d => d.IsVendor == true).ToList();

            List<Customer> ListModel = new List<Customer>();
            foreach (Context.Customer item in items)
            {
                ListModel.Add(new Customer(item));
            }

            int total = RepoCustomer.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(ListModel.Select(d => new
            {
                Id = d.Id,
                CustomerCode = d.CustomerCode,
                CustomerCodeOld = d.CustomerCodeOld,
                CustomerNama = d.CustomerNama,
                StrPrioritas = d.StrPrioritas,
                WajibPO = d.WajibPO,
                WajibGPS = d.WajibGPS,
                SpecialTreatment = d.SpecialTreatment
            })
            );
        }
        public string BindingPic(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);

            List<CustPIC> model = new List<CustPIC>();

            foreach (Context.CustomerPic item in dbitem.CustomerPic)
            {
                model.Add(new CustPIC(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }

        public string BindingAddress(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);

            List<CustAddress> model = new List<CustAddress>();

            foreach (Context.CustomerAddress item in dbitem.CustomerAddress)
            {
                model.Add(new CustAddress(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        public string BindingProduct(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);
            List<CustProduct> model = new List<CustProduct>();

            foreach (Context.CustomerProductType item in dbitem.CustomerProductType)
            {
                model.Add(new CustProduct(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        public string BindingLoadingAddress(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);
            List<CustLoadUnload> model = new List<CustLoadUnload>();

            foreach (Context.CustomerLoadingAddress item in dbitem.CustomerLoadingAddress)
            {
                model.Add(new CustLoadUnload(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        public string BindingUnLoadingAddress(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);
            List<CustLoadUnload> model = new List<CustLoadUnload>();

            foreach (Context.CustomerUnloadingAddress item in dbitem.CustomerUnloadingAddress)
            {
                model.Add(new CustLoadUnload(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        public string BindingSupplier(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);
            List<CustSupp> model = new List<CustSupp>();

            foreach (Context.CustomerSupplier item in dbitem.CustomerSupplier)
            {
                model.Add(new CustSupp(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        public string BindingNotif(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);
            List<CustNotif> model = new List<CustNotif>();

            foreach (Context.CustomerNotification item in dbitem.CustomerNotification)
            {
                Context.CustomerPic dbpic = dbitem.CustomerPic.Where(d => d.Id == item.IdPic).FirstOrDefault();
                model.Add(new CustNotif(item, dbpic));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        public string BindingBilling(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);
            List<CustBilling> model = new List<CustBilling>();

            foreach (Context.CustomerBilling item in dbitem.CustomerBilling)
            {
                model.Add(new CustBilling(item));
            }

            return new JavaScriptSerializer().Serialize(new { data = model });
        }
        public string bindingAttachment(int id)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(id);
            List<Context.CustomerAttachment> listData = dbitem.CustomerAttachment.ToList();

            List<UserAttachment> models = new List<UserAttachment>();
            foreach (Context.CustomerAttachment item in listData)
            {
                models.Add(new UserAttachment()
                {
                    id = item.Id,
                    CustId = item.CustomerId,
                    realfname = item.realfname,
                    url = item.url,
                    filename = item.filename
                });
            }

            return new JavaScriptSerializer().Serialize(new { total = models.Count(), data = models });
        }
        public string BindingHisCs(int idCust)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            Context.Customer dbitem = RepoCustomer.FindByPK(idCust);

            Context.CustomerCreditStatus data = dbitem.CustomerCreditStatus.FirstOrDefault();

            return new JavaScriptSerializer().Serialize(new { data = data.CustomerCreditStatusHistory.Select(d => new { 
                    tanggal = d.Tanggal,
                    awal = d.StatAwal,
                    akhir = d.StatAkhir,
                    user = d.Username,
                    keterangan = d.Keterangan
                }) 
            });
        }

        [MyAuthorize(Menu = "Customer", Action="create")]
        public ActionResult Add()
        {
            Customer model = new Customer();
            ViewBag.activeTab = "BasicData";
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "create")]
        public ActionResult Add(Customer model, string submitBasicData)
        {
            if (ModelState.IsValid)
            {
                //more validation
                bool IsExist = RepoCustomer.IsExist(model.CustomerCode);

                if (IsExist)
                {
                    ModelState.AddModelError("CustomerCode", "Kode Customer sudah terdaftar");
                    return View("Form", model);
                }
                Context.Customer dbitem = new Context.Customer();
                model.setDb(dbitem);
                //generate code
                dbitem.urutan = RepoCustomer.getUrutan() + 1;
                dbitem.CustomerCode = RepoCustomer.generateCode(dbitem.urutan);

                RepoCustomer.save(dbitem);
                RepoAuditrail.saveCustomerQuery(dbitem, UserPrincipal.id, "Add");
                if (Repoptnr_mstr.FindByPK(dbitem.Id) == null)
                {
                    Repoptnr_mstr.save(dbitem);
                }
                else
                {
                    Context.ptnr_mstr dbptnr = Repoptnr_mstr.FindByPK(dbitem.Id);
                    dbptnr.ptnr_name = dbitem.CustomerNama;
                    Repoptnr_mstr.updateCustomer(dbptnr);
                }

                return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = submitBasicData == "Create" ? "BasicData" : "Pic" }));
            }
            ViewBag.activeTab = "BasicData";
            return View("Form", model);
        }
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public ActionResult Edit(int id, string activeTab, string stat)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(id);
            if (dbitem != null)
            {
                Customer Viewmodel = new Customer(dbitem);
                Viewmodel.ListTreatment = GetListProductTreatment();
                Viewmodel.ListWarna = GeListtWarna();
                Viewmodel.ListTruck = GetTruck();
                Viewmodel.ListCustTruckType = new List<CustTruckType>();
                foreach (Context.JenisTrucks item in Viewmodel.ListTruck)
                {
                    Viewmodel.ListCustTruckType.Add(new CustTruckType()
                    {
                        TruckId = item.Id,
                        Kode = item.StrJenisTruck,
                        Alias = item.Alias
                    });
                }
                foreach (Context.CustomerTypeTrucks item in dbitem.CustomerTypeTrucks)
                {
                    CustTruckType custtruck = Viewmodel.ListCustTruckType.Where(d => d.TruckId == item.JenisTruck.Value).First();
                    if (item.Type == "KODE")
                    {
                        custtruck.IsKode = true;
                    }
                    else
                    {
                        custtruck.IsAlias = true;
                        custtruck.Alias = item.Value;
                    }
                }
                ViewBag.name = Viewmodel.CustomerCode;
                ViewBag.activeTab = activeTab;
                ViewBag.status = stat;
                return View("Form", Viewmodel);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public ActionResult Edit(Customer model, string submitBasicData)
        {
            if (ModelState.IsValid)
            {
                //more validation
                bool IsExist = RepoCustomer.IsExist(model.CustomerCode, model.Id);

                if (IsExist)
                {
                    Context.Customer dbitemOld = RepoCustomer.FindByPK(model.Id);
                    model = new Customer(dbitemOld);
                    ModelState.AddModelError("CustomerCode", "Kode Customer sudah terdaftar");
                    return View("Form", model);
                }

                Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
                model.setDb(dbitem);
                dbitem.IsVendor = bool.Parse(model.IsVendor.ToString());
                RepoCustomer.save(dbitem);
                RepoAuditrail.saveCustomerQuery(dbitem, UserPrincipal.id);
                if (Repoptnr_mstr.FindByPK(dbitem.Id) == null)
                {
                    Repoptnr_mstr.save(dbitem);
                    foreach (Context.CustomerPPN item in dbitem.CustomerPPN)
                    {
                        //Repoptnr_mstr.save(dbitem);
                    }
                }
                else{
                    Context.ptnr_mstr dbptnr = Repoptnr_mstr.FindByPK(dbitem.Id);
                    dbptnr.ptnr_code = dbitem.CustomerCodeOld;
                    dbptnr.ptnr_remarks = dbitem.CustomerCode;
                    dbptnr.ptnr_taxable = "Y";
                    dbptnr.ptnr_name = dbitem.CustomerNama;
                    dbptnr.ptnr_ppn_type = dbitem.CustomerPPN != null && dbitem.CustomerPPN.FirstOrDefault() != null && dbitem.CustomerPPN.FirstOrDefault().PPN == true ? "Y" : "N";
                    if (dbitem.CustomerPPN != null && dbitem.CustomerPPN.FirstOrDefault() != null){
                        dbptnr.ptnr_npwp = dbitem.CustomerPPN.FirstOrDefault().NomorNPWP;
                        dbptnr.ptnr_contact_tax = dbitem.CustomerPPN.FirstOrDefault().NamaNPWP;
                        dbptnr.ptnr_address_tax = dbitem.CustomerPPN.FirstOrDefault().AddressNPWP;
                    }
                    Repoptnr_mstr.updateCustomer(dbptnr);
                }
                foreach (Context.CustomerAddress item in dbitem.CustomerAddress)
                {
                    if (Repoptnra_addr.FindByPK(item.Id) == null)
                    {
                        Repoptnra_addr.save(item);
                    }
                    else{
                        Context.ptnra_addr ptnra_addr = Repoptnra_addr.FindByPK(item.Id);
                        ptnra_addr.ptnra_line_1 = item.Alamat;
                        ptnra_addr.ptnra_line_2 = item.LocKabKota.Nama;
                        ptnra_addr.ptnra_phone_1 = item.Telp;
                        ptnra_addr.ptnra_fax_1 = item.Fax;
                        ptnra_addr.ptnra_id = item.Id;
                        Repoptnra_addr.updateCustomerAddress(ptnra_addr);
                    }
                }

                return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = submitBasicData == "Update" ? "BasicData" : "Pic", stat = "success" }));
            }
            Context.Customer db = RepoCustomer.FindByPK(model.Id);
            model.ListTreatment = GetListProductTreatment();
            model.ListWarna = GeListtWarna();
            model.ListTruck = GetTruck();
            model.ListCustTruckType = new List<CustTruckType>();
            foreach (Context.JenisTrucks item in model.ListTruck)
            {
                model.ListCustTruckType.Add(new CustTruckType()
                {
                    TruckId = item.Id,
                    Kode = item.StrJenisTruck,
                    Alias = item.Alias
                });
            }
            foreach (Context.CustomerTypeTrucks item in db.CustomerTypeTrucks)
            {
                CustTruckType custtruck = model.ListCustTruckType.Where(d => d.TruckId == item.JenisTruck.Value).First();
                if (item.Type == "KODE")
                {
                    custtruck.IsKode = true;
                }
                else
                {
                    custtruck.IsAlias = true;
                    custtruck.Alias = item.Value;
                }
            }

            ViewBag.name = model.CustomerCode;
            ViewBag.activeTab = "BasicData";
            //ViewBag.status = stat;
            return View("Form", model);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSavePIC(CustPIC model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerPic dbpic;
            if (model.Id == 0)
            {
                int Urutan = dbitem.CustomerPic.Count() == 0 ? 1 : dbitem.CustomerPic.Max(d => d.Urutan) + 1;
                dbpic = new Context.CustomerPic()
                {
                    Name = model.Nama,
                    //auto generate
                    //generate code
                    Urutan = Urutan,
                    Code = "CP-" + (Urutan).ToString().PadLeft(4, '0'),
                    DepartemenId = model.DepartemenId,
                    JabatanId = model.JabatanId,
                    EmailAdd = model.EmailAdd,
                    Mobile = model.Mobile,
                };
                dbitem.CustomerPic.Add(dbpic);
                RepoAuditrail.saveUpdCustomerPICQuery(dbpic, UserPrincipal.id);
            }
            else
            {
                dbpic = dbitem.CustomerPic.Where(d => d.Id == model.Id).FirstOrDefault();
                dbpic.Name = model.Nama;
                dbpic.DepartemenId = model.DepartemenId;
                dbpic.JabatanId = model.JabatanId;
                dbpic.EmailAdd = model.EmailAdd;
                dbpic.Mobile = model.Mobile;
                RepoAuditrail.saveCustomerPICQuery(dbpic, UserPrincipal.id);
            }
            RepoCustomer.save(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }

        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSaveAddress(CustAddress model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerAddress dbaddress;
            if (model.Id == 0)
            {
                int Urutan = dbitem.CustomerAddress.Count() == 0 ? 1 : dbitem.CustomerAddress.Max(d => d.urutan) + 1;
                dbaddress = new Context.CustomerAddress()
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    //generate code
                    urutan = Urutan,
                    Code = "CA-" + (Urutan).ToString().PadLeft(4, '0'),
                    Alamat = model.Alamat,
                    IdProvinsi = model.IdProvinsi,
                    IdKabKota = model.IdKabKota,
                    IdKec = model.IdKec,
                    IdKel = model.IdKel,
                    Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.',','),
                    Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ','),
                    Radius = model.Radius,
                    Zona = model.Zona,
                    OfficeTypeId = model.OfficeTypeId,
                    Telp = model.Telp,
                    Fax = model.Fax,
                };
                dbitem.CustomerAddress.Add(dbaddress);
                RepoAuditrail.saveCustomerAddressQuery(dbaddress, UserPrincipal.id, "add");
            }
            else
            {
                dbaddress = dbitem.CustomerAddress.Where(d => d.Id == model.Id).FirstOrDefault();
                dbaddress.Id = model.Id;
                dbaddress.CustomerId = model.CustomerId;
                dbaddress.Code = model.Code;
                dbaddress.Alamat = model.Alamat;
                dbaddress.IdProvinsi = model.IdProvinsi;
                dbaddress.IdKabKota = model.IdKabKota == -1 ? dbaddress.IdKabKota : model.IdKabKota;
                dbaddress.IdKec = model.IdKec == -1 ? dbaddress.IdKec : model.IdKec;
                dbaddress.IdKel = model.IdKel == -1 ? dbaddress.IdKel : model.IdKel;
                dbaddress.Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.', ',');
                dbaddress.Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ',');
                dbaddress.Radius = model.Radius;
                dbaddress.Zona = model.Zona;
                dbaddress.OfficeTypeId = model.OfficeTypeId;
                dbaddress.Telp = model.Telp;
                dbaddress.Fax = model.Fax;
                RepoAuditrail.saveCustomerAddressQuery(dbaddress, UserPrincipal.id);
            }
            RepoCustomer.save(dbitem);
            foreach (Context.CustomerAddress item in dbitem.CustomerAddress)
            {
//                Repoptnra_addr.save(item);
            }
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSaveProduct(CustProduct model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerProductType dbproduct;
            if (model.Id == 0)
            {
                dbproduct = new Context.CustomerProductType()
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    idProduk = model.idProduk,
                    PenangananKhusus = model.PenangananKhusus,
                    Keterangan = model.Keterangan,
                };
                dbitem.CustomerProductType.Add(dbproduct);
            }
            else
            {
                dbproduct = dbitem.CustomerProductType.Where(d => d.Id == model.Id).FirstOrDefault();
                dbproduct.Id = model.Id;
                dbproduct.CustomerId = model.CustomerId;
                dbproduct.idProduk = model.idProduk;
                dbproduct.PenangananKhusus = model.PenangananKhusus;
                dbproduct.Keterangan = model.Keterangan;
            }
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveCustomerProductTypeQuery(dbproduct, UserPrincipal.id);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSaveLoadingAddress(CustLoadUnload model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerLoadingAddress dbloadingadd;
            if (model.Id == 0)
            {
                int Urutan = dbitem.CustomerLoadingAddress.Count() == 0 ? 1 : dbitem.CustomerLoadingAddress.Max(d => d.urutan) + 1;
                dbloadingadd = new Context.CustomerLoadingAddress()
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    //generate code
                    urutan = Urutan,
                    Code = "CLA-" + (Urutan).ToString().PadLeft(4, '0'),
                    Alamat = model.Alamat,
                    IdProvinsi = model.IdProvinsi,
                    IdKabKota = model.IdKabKota,
                    IdKec = model.IdKec,
                    IdKel = model.IdKel,
                    Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.', ','),
                    Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ','),
                    Radius = model.Radius,
                    Zona = model.Zona,
                    Telp = model.Telp,
                    Fax = model.Fax,
                };
                dbitem.CustomerLoadingAddress.Add(dbloadingadd);
                RepoAuditrail.saveCustomerLoadingAddressQuery(dbloadingadd, UserPrincipal.id, "add");
            }
            else
            {
                dbloadingadd = dbitem.CustomerLoadingAddress.Where(d => d.Id == model.Id).FirstOrDefault();
                dbloadingadd.Id = model.Id;
                dbloadingadd.CustomerId = model.CustomerId;
                dbloadingadd.Code = model.Code;
                dbloadingadd.Alamat = model.Alamat;
                dbloadingadd.IdProvinsi = model.IdProvinsi;
                dbloadingadd.IdKabKota = model.IdKabKota == -1 ? dbloadingadd.IdKabKota : model.IdKabKota;
                dbloadingadd.IdKec = model.IdKec == -1 ? dbloadingadd.IdKec : model.IdKec;
                dbloadingadd.IdKel = model.IdKel == -1 ? dbloadingadd.IdKel : model.IdKel;
                dbloadingadd.Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.', ',');
                dbloadingadd.Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ',');
                dbloadingadd.Radius = model.Radius;
                dbloadingadd.Zona = model.Zona;
                dbloadingadd.Telp = model.Telp;
                dbloadingadd.Fax = model.Fax;
                RepoAuditrail.saveCustomerLoadingAddressQuery(dbloadingadd, UserPrincipal.id);
            }
            RepoCustomer.save(dbitem);
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSaveUnLoadingAddress(CustLoadUnload model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerUnloadingAddress dbloadingadd;
            if (model.Id == 0)
            {
                int Urutan = dbitem.CustomerUnloadingAddress.Count() == 0 ? 1 : dbitem.CustomerUnloadingAddress.Max(d => d.urutan) + 1;
                dbloadingadd = new Context.CustomerUnloadingAddress()
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    //generate code
                    urutan = Urutan,
                    Code = "CUA-" + (Urutan).ToString().PadLeft(4, '0'),
                    Alamat = model.Alamat,
                    IdProvinsi = model.IdProvinsi,
                    IdKabKota = model.IdKabKota,
                    IdKec = model.IdKec,
                    IdKel = model.IdKel,
                    Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.', ','),
                    Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ','),
                    Radius = model.Radius,
                    Zona = model.Zona,
                    Telp = model.Telp,
                    Fax = model.Fax,
                };
                dbitem.CustomerUnloadingAddress.Add(dbloadingadd);
            }
            else
            {
                dbloadingadd = dbitem.CustomerUnloadingAddress.Where(d => d.Id == model.Id).FirstOrDefault();
                dbloadingadd.Id = model.Id;
                dbloadingadd.CustomerId = model.CustomerId;
                dbloadingadd.Code = model.Code;
                dbloadingadd.Alamat = model.Alamat;
                dbloadingadd.IdProvinsi = model.IdProvinsi;
                dbloadingadd.IdKabKota = model.IdKabKota == -1 ? dbloadingadd.IdKabKota : dbloadingadd.IdKabKota;
                dbloadingadd.IdKec = model.IdKec == -1 ? dbloadingadd.IdKec : model.IdKec;
                dbloadingadd.IdKel = model.IdKel == -1 ? dbloadingadd.IdKel : model.IdKel;
                dbloadingadd.Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.', ',');
                dbloadingadd.Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ',');
                dbloadingadd.Radius = model.Radius;
                dbloadingadd.Zona = model.Zona;
                dbloadingadd.Telp = model.Telp;
                dbloadingadd.Fax = model.Fax;
            }
            RepoCustomer.save(dbitem);
            if (model.Id == 0)
            {
                RepoAuditrail.SetAuditTrail(
                    "INSERT INTO dbo.\"CustomerUnloadingAddress\" (\"CustomerId\", \"Code\", \"Alamat\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\", \"Longitude\", " + "\"Latitude\", " +
                    "\"Radius\", \"Zona\", \"Telp\", \"Fax\", urutan) VALUES (" + dbitem.Id + ", " + dbloadingadd.Code + ", " + dbloadingadd.Alamat + ", " + dbloadingadd.IdProvinsi + ", " +
                    dbloadingadd.IdKabKota + ", " + dbloadingadd.IdKec + ", " + dbloadingadd.IdKel + ", " + dbloadingadd.Longitude + ", " + dbloadingadd.Latitude + ", " + dbloadingadd.Radius + ", " +
                    dbloadingadd.Zona + ", " + dbloadingadd.Telp + ", " + dbloadingadd.Fax + ", " + dbloadingadd.urutan + ");", "Customer Unloading Address", "Add", UserPrincipal.id);
            }
            else{
                RepoAuditrail.SetAuditTrail(
                    "UPDATE dbo.\"CustomerUnloadingAddress\" \"Alamat\" = " + dbloadingadd.Alamat + ", \"IdProvinsi\" = " + dbloadingadd.IdProvinsi + ", \"IdKabKota\" = " + dbloadingadd.IdKabKota +
                        ", \"IdKec\" = " + dbloadingadd.IdKec + ", \"IdKel\" = " + dbloadingadd.IdKel + ", \"Longitude\" = " + dbloadingadd.Longitude + ", \"Latitude\" = " + dbloadingadd.Latitude + 
                        ", \"Radius\" = " + dbloadingadd.Radius + ", \"Zona\" = " + dbloadingadd.Zona + ", \"Telp\" = " + dbloadingadd.Telp + ", \"Fax\" = " + dbloadingadd.Fax + ", urutan = " +
                        dbloadingadd.urutan + " WHERE \"CustomerId\" = " + dbitem.Id + ";", "Customer Unloading Address", "Edit", UserPrincipal.id);
            }
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSaveSupplier(CustSupp model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerSupplier dbsupp;
            if (model.Id == 0)
            {
                int Urutan = dbitem.CustomerSupplier.Count() == 0 ? 1 : dbitem.CustomerSupplier.Max(d => d.urutan) + 1;
                dbsupp = new Context.CustomerSupplier()
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    //generate code
                    urutan = Urutan,
                    Code = "CSA-" + (Urutan).ToString().PadLeft(4, '0'),
                    Alamat = model.Alamat,
                    Nama = model.Nama,
                    IdProvinsi = model.IdProvinsi,
                    IdKabKota = model.IdKabKota,
                    IdKec = model.IdKec,
                    IdKel = model.IdKel,
                    Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.', ','),
                    Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ','),
                    Pic = model.Pic,
                    Radius = model.Radius,
                    Zona = model.Zona,
                    Telp = model.Telp,
                    Fax = model.Fax,
                };
                dbitem.CustomerSupplier.Add(dbsupp);
                RepoAuditrail.saveCustomerSupplierQuery(dbsupp, UserPrincipal.id);
            }
            else
            {
                dbsupp = dbitem.CustomerSupplier.Where(d => d.Id == model.Id).FirstOrDefault();
                dbsupp.Id = model.Id;
                dbsupp.CustomerId = model.CustomerId;
                dbsupp.Code = model.Code;
                dbsupp.Nama = model.Nama;
                dbsupp.Alamat = model.Alamat;
                dbsupp.IdProvinsi = model.IdProvinsi;
                dbsupp.IdKabKota = model.IdKabKota == -1 ? dbsupp.IdKabKota : dbsupp.IdKabKota;
                dbsupp.IdKec = model.IdKec == -1 ? dbsupp.IdKec : model.IdKec;
                dbsupp.IdKel = model.IdKel == -1 ? dbsupp.IdKel : model.IdKel;
                dbsupp.Longitude = model.Longitude == null ? "" : model.Longitude.Replace('.', ',');
                dbsupp.Latitude = model.Latitude == null ? "" : model.Latitude.Replace('.', ',');
                dbsupp.Radius = model.Radius;
                dbsupp.Zona = model.Zona;
                dbsupp.Pic = model.Pic;
                dbsupp.Telp = model.Telp;
                dbsupp.Fax = model.Fax;
                RepoAuditrail.saveUpdCustomerSupplierQuery(dbsupp, UserPrincipal.id);
            }
            RepoCustomer.save(dbitem);
            
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSaveNotif(CustNotif model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerNotification dbnotif;
            if (model.Id == 0)
            {
                dbnotif = new Context.CustomerNotification()
                {
                    Id = model.Id,
                    IsActive = model.IsActive,
                    IdPic = model.IdPic,
                    NotifType = model.NotifType,
                };
                foreach (string item in model.strIdRute.Split(','))
                {
                    dbnotif.CustomerNotifRute.Add(new Context.CustomerNotifRute()
                    {
                        IdRute = int.Parse(item)
                    });
                }
                foreach (string item in model.strIdTruck.Split(','))
                {
                    dbnotif.CustomerNotifTruck.Add(new Context.CustomerNotifTruck()
                    {
                        IdTruck = int.Parse(item)
                    });
                }

                dbitem.CustomerNotification.Add(dbnotif);
            }
            else
            {
                dbnotif = dbitem.CustomerNotification.Where(d => d.Id == model.Id).FirstOrDefault();
                dbnotif.Id = model.Id;
                dbnotif.IsActive = model.IsActive;
                dbnotif.IdPic = model.IdPic;
                dbnotif.NotifType = model.NotifType;
                dbnotif.CustomerNotifRute.Clear();
                foreach (string item in model.strIdRute.Split(','))
                {
                    dbnotif.CustomerNotifRute.Add(new Context.CustomerNotifRute()
                    {
                        IdRute = int.Parse(item)
                    });
                }
                dbnotif.CustomerNotifTruck.Clear();
                foreach (string item in model.strIdTruck.Split(','))
                {
                    dbnotif.CustomerNotifTruck.Add(new Context.CustomerNotifTruck()
                    {
                        IdTruck = int.Parse(item)
                    });
                }
                RepoAuditrail.saveDelAllCustomerNotifRuteQuery(dbnotif, UserPrincipal.id);
                RepoAuditrail.saveDelAllCustomerNotifTruckQuery(dbnotif, UserPrincipal.id);
            }
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveCustomerNotificationQuery(dbnotif, UserPrincipal.id);
            foreach(Context.CustomerNotifRute cnr in dbnotif.CustomerNotifRute){
                RepoAuditrail.saveCustomerNotifRuteQuery(cnr, UserPrincipal.id);
            }
            foreach(Context.CustomerNotifTruck cnt in dbnotif.CustomerNotifTruck){
                RepoAuditrail.saveCustomerNotifTruckQuery(cnt, UserPrincipal.id);
            }
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult CustomerSaveBilling(CustBilling model)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustomerId);
            Context.CustomerBilling dbBilling;
            if (model.Id == 0)
            {
                dbBilling = new Context.CustomerBilling()
                {
                    DocumentName = model.DocumentName,
                    Lembar = model.Lembar,
                    Warna = model.Warna,
                    Stempel = model.Stempel,
                    UrlAtt = model.UrlAtt,
                    FileName = model.FileName,
                    IsFax = model.IsFax,
                    Fax = model.Fax,
                    IsEmail = model.IsEmail,
                    Email = model.Email,
                    IsTukarFaktur = model.IsTukarFaktur,
                    IsJasaPengiriman = model.IsJasaPengiriman,
                    IsActive = model.IsActive
                };

                dbitem.CustomerBilling.Add(dbBilling);
                RepoAuditrail.saveCustomerBillingQuery(dbBilling, UserPrincipal.id);
            }
            else
            {
                dbBilling = dbitem.CustomerBilling.Where(d => d.Id == model.Id).FirstOrDefault();
                //hapus file sebelumnya jika tidak sama dengan yang baru
                if (dbBilling.FileName != model.FileName)
                {
                    List<string> listfile = new List<string>();
                    listfile.Add(dbBilling.FileName);
                    var res = new FileManagementController().Delete(listfile.ToArray(), Server.MapPath("~/Uploads"));
                }

                dbBilling.DocumentName = model.DocumentName;
                dbBilling.Lembar = model.Lembar;
                dbBilling.Warna = model.Warna;
                dbBilling.Stempel = model.Stempel;
                dbBilling.UrlAtt = model.UrlAtt;
                dbBilling.FileName = model.FileName;
                dbBilling.IsFax = model.IsFax;
                dbBilling.Fax = model.Fax;
                dbBilling.IsEmail = model.IsEmail;
                dbBilling.Email = model.Email;
                dbBilling.IsTukarFaktur = model.IsTukarFaktur;
                dbBilling.IsJasaPengiriman = model.IsJasaPengiriman;
                dbBilling.IsActive = model.IsActive;

                dbBilling.CustomerJadwalBilling.Clear();

                RepoAuditrail.saveDelCustomerJadwalBillingQuery(dbBilling, UserPrincipal.id);
                RepoAuditrail.saveCustomerBillingQuery(dbBilling, UserPrincipal.id);
            }

            ObjDataJadwal[] result = JsonConvert.DeserializeObject<ObjDataJadwal[]>(model.srtDataJadwal);

            foreach (ObjDataJadwal datJ in result)
            {
                dbBilling.CustomerJadwalBilling.Add(new Context.CustomerJadwalBilling()
                {
                    Hari = datJ.Hari,
                    Jam = datJ.Jam,
                    Catatan = datJ.Catatan,
                    PIC = datJ.Email
                });
                
            }

            RepoCustomer.save(dbitem);
            foreach (Context.CustomerJadwalBilling cjb in dbitem.CustomerBilling.FirstOrDefault().CustomerJadwalBilling)
            {
                RepoAuditrail.saveCustomerJadwalBillingQuery(cjb, UserPrincipal.id);
            }
            ResponeModel response = new ResponeModel(true);
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult AddAttachment(UserAttachment model, int id)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustId);
            dbitem.CustomerAttachment.Add(new Context.CustomerAttachment()
            {
                filename = model.filename,
                url = model.url,
                realfname = model.realfname,
            });

            ResponeModel response = new ResponeModel(true);

            RepoCustomer.save(dbitem);
            RepoAuditrail.saveCustomerAttachmentQuery(dbitem.CustomerAttachment.FirstOrDefault(), UserPrincipal.id);

            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public JsonResult DeleteAttachment(int custid, int id)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(custid);
            Context.CustomerAttachment dbupdate = dbitem.CustomerAttachment.Where(d => d.Id == id).First();
            dbitem.CustomerAttachment.Remove(dbupdate);

            ResponeModel response = new ResponeModel(true);

            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelCustomerAttachmentQuery(dbitem.CustomerAttachment.FirstOrDefault(), UserPrincipal.id);

            return Json(response);
        }

        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Customer dbItem = RepoCustomer.FindByPK(id);

            RepoCustomer.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeletePIC(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerPic dbpic = dbitem.CustomerPic.Where(d => d.Id == id).FirstOrDefault();

            if (dbitem.CustomerNotification.Any(d => d.IdPic == id))
            {
                ResponeModel responseFalse = new ResponeModel(false);
                responseFalse.SetFail("Pic sudah digunakan dicustomer notifikasi.");
                return Json(responseFalse);
            }

            dbitem.CustomerPic.Remove(dbpic);
            RepoAuditrail.saveDelCustomerPICQuery(dbpic, UserPrincipal.id);
            RepoCustomer.save(dbitem);
            response.Success = true;
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeleteAddress(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerAddress dbaddress = dbitem.CustomerAddress.Where(d => d.Id == id).FirstOrDefault();

            dbitem.CustomerAddress.Remove(dbaddress);
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelCustomerAddressQuery(dbaddress, UserPrincipal.id);
            response.Success = true;
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeleteProduct(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerProductType dbproduct = dbitem.CustomerProductType.Where(d => d.Id == id).FirstOrDefault();

            dbitem.CustomerProductType.Remove(dbproduct);
            RepoAuditrail.saveDelCustomerProductTypeQuery(dbproduct, UserPrincipal.id);
            RepoCustomer.save(dbitem);
            response.Success = true;
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeleteLoadingAdd(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerLoadingAddress dbdelete = dbitem.CustomerLoadingAddress.Where(d => d.Id == id).FirstOrDefault();

            dbitem.CustomerLoadingAddress.Remove(dbdelete);
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelCustomerLoadingAddressQuery(dbdelete, UserPrincipal.id);
            response.Success = true;
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeleteUnLoadingAdd(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerUnloadingAddress dbdelete = dbitem.CustomerUnloadingAddress.Where(d => d.Id == id).FirstOrDefault();

            dbitem.CustomerUnloadingAddress.Remove(dbdelete);
            RepoCustomer.save(dbitem);
            RepoAuditrail.SetAuditTrail("DELETE FROM dbo.\"CustomerUnloadingAddress\" WHERE \"Id\"= " + dbitem.Id + ";", "Customer Unloading Address", "Delete", UserPrincipal.id);
            response.Success = true;
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeleteSupp(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerSupplier dbdelete = dbitem.CustomerSupplier.Where(d => d.Id == id).FirstOrDefault();

            dbitem.CustomerSupplier.Remove(dbdelete);
            RepoAuditrail.saveDelCustomerSupplierQuery(dbdelete, UserPrincipal.id);
            RepoCustomer.save(dbitem);
            response.Success = true;
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeleteNotif(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerNotification dbdelete = dbitem.CustomerNotification.Where(d => d.Id == id).FirstOrDefault();

            dbitem.CustomerNotification.Remove(dbdelete);
            RepoAuditrail.saveDelCustomerNotificationQuery(dbdelete, UserPrincipal.id);
            RepoCustomer.save(dbitem);
            response.Success = true;
            return Json(response);
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "delete")]
        public JsonResult CustomerDeleteBilling(int CustId, int id)
        {
            ResponeModel response = new ResponeModel();
            Context.Customer dbitem = RepoCustomer.FindByPK(CustId);
            Context.CustomerBilling dbdelete = dbitem.CustomerBilling.Where(d => d.Id == id).FirstOrDefault();
            dbitem.CustomerBilling.Remove(dbdelete);
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelCustomerBillingQuery(dbdelete, UserPrincipal.id);
            response.Success = true;
            return Json(response);
        }

        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public ActionResult UpdatePPN(Customer model, string submitPPN)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerPPN.Clear();
            dbitem.CustomerPPN.Add(new Context.CustomerPPN()
            {
                PPN = model.IsPPn,
                IdRekening = model.IdRekening,
                NomorNPWP = model.NoNpwp,
                NamaNPWP = model.NamaNpwp,
                AddressNPWP = model.AddressNpwp
            });
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelCustomerPPNQuery(dbitem.CustomerPPN.FirstOrDefault(), UserPrincipal.id);
            RepoAuditrail.saveCustomerPPNQuery(dbitem.CustomerPPN.FirstOrDefault(), UserPrincipal.id);

            Context.ptnr_mstr ptnra_addr = Repoptnr_mstr.FindByPK(dbitem.Id);
            ptnra_addr.ptnr_ppn_type = dbitem.CustomerPPN != null && dbitem.CustomerPPN.FirstOrDefault() != null && dbitem.CustomerPPN.FirstOrDefault().PPN == true ? "Y" : "N";
            Repoptnr_mstr.updateCustomer(ptnra_addr);

            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = submitPPN == "Update" ? "PPN" : "cs", stat = "success" }));
        }
        [HttpPost]
        [MyAuthorize(Menu = "Customer", Action = "update")]
        public ActionResult UpdateCS(Customer model, string submitCS)
        {
            bool isnew = false;
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            Context.CustomerCreditStatus dbcs = dbitem.CustomerCreditStatus.Where(d=>d.Id == model.IdCreditStatus).FirstOrDefault();
            if (dbcs == null)
            {
                dbcs = new Context.CustomerCreditStatus();
                isnew = true;
            }
            Context.CustomerCreditStatusHistory dbcshistory = new Context.CustomerCreditStatusHistory();

            string statawal = dbcs.StatusOveride != null ? dbcs.StatusOveride : "";
            //dbcs.StatusSystem = model.StatusSystem;
            dbcs.StatusOveride = model.StatusOveride;
            dbcs.Keterangan = model.KeteranganCS;
            dbcs.Condition = model.ConditionCS;
            dbcs.MinTOPOverdue1 = model.MinTOPOverdue1;
            dbcs.MaxTOPOverdue1 = model.MaxTOPOverdue1;
            dbcs.ValueOverdue2 = model.ValueOverdue2;
            dbcs.TOPOverdue2 = model.TOPOverdue2;
            dbcs.ShipmentDay1 = model.ShipmentDay1;
            dbcs.ShipmentDay2 = model.ShipmentDay2;
            dbcs.CustomerCreditStatusHistory.Add(new Context.CustomerCreditStatusHistory()
            {
                StatusSystem = model.StatusSystem,
                StatusOveride = model.StatusOveride,
                Keterangan = model.KeteranganCS,
                Condition = model.ConditionCS,
                MinTOPOverdue1 = model.MinTOPOverdue1,
                MaxTOPOverdue1 = model.MaxTOPOverdue1,
                ValueOverdue2 = model.ValueOverdue2,
                TOPOverdue2 = model.TOPOverdue2,
                ShipmentDay1 = model.ShipmentDay1,
                ShipmentDay2 = model.ShipmentDay2,
                Tanggal = DateTime.Now,
                //Username = "By user",
                Username = UserPrincipal.firstname + " " + UserPrincipal.lastname,
                StatAwal = statawal,
                StatAkhir = model.StatusOveride
            });

            if(isnew){
                dbitem.CustomerCreditStatus.Add(dbcs);
                RepoAuditrail.saveCustomerCreditStatusQuery(dbcs, UserPrincipal.id);
            }
            else
                RepoAuditrail.saveUpdCustomerCreditStatusQuery(dbcs, UserPrincipal.id);
    
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveCustomerCreditStatusHistoryQuery(dbcs.CustomerCreditStatusHistory.Last(), UserPrincipal.id);
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = submitCS == "Update" ? "cs" : "notif", stat = "success" }));
        }

        [HttpPost]
        public ActionResult UpdateTruckType(Customer model, string submittruckType)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerTypeTrucks.Clear();
            foreach (CustTruckType item in model.ListCustTruckType)
            {
                Context.CustomerTypeTrucks dbtruck = new Context.CustomerTypeTrucks();
                if (item.IsKode == true)
                {
                    dbtruck.JenisTruck = item.TruckId;
                    dbtruck.Type = "KODE";
                    dbitem.CustomerTypeTrucks.Add(dbtruck);
                }
                else if (item.IsAlias == true)
                {
                    dbtruck.JenisTruck = item.TruckId;
                    dbtruck.Type = "ALIAS";
                    dbtruck.Value = item.Alias;
                    dbitem.CustomerTypeTrucks.Add(dbtruck);
                }
            }
            RepoCustomer.save(dbitem);
            var query = "DELETE FROM dbo.\"CustomerTypeTrucks\" WHERE \"CustomerId\" = " + dbitem.Id + ";";
            foreach (Context.CustomerTypeTrucks item in dbitem.CustomerTypeTrucks)
            {
                query += "INSERT INTO dbo.\"CustomerTypeTrucks\" (\"CustomerId\", \"JenisTruck\", \"Type\", \"Value\") VALUES (" + dbitem.Id + ", " + item.JenisTruck+", "+item.Type+", "+item.Value + ");";
            }
            RepoAuditrail.SetAuditTrail(query, "Customer Truck Type", "Edit", UserPrincipal.id);
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = submittruckType == "Update" ? "TypeTrucks" : "BasicData", stat = "success" }));
        }
        [HttpPost]
        public JsonResult UpdateAttachment(UserAttachment model, int id)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.CustId);
            Context.CustomerAttachment dbupdate = dbitem.CustomerAttachment.Where(d => d.Id == model.id).First();
            dbitem.CustomerAttachment.Remove(dbupdate);
            dbitem.CustomerAttachment.Add(new Context.CustomerAttachment()
            {
                filename = model.filename,
                url = model.url,
                realfname = model.realfname
            });

            ResponeModel response = new ResponeModel(true);

            RepoCustomer.save(dbitem);
            RepoAuditrail.saveCustomerAttachmentQuery(dbitem.CustomerAttachment.FirstOrDefault(), UserPrincipal.id);
            RepoAuditrail.saveDelCustomerAttachmentQuery(dbitem.CustomerAttachment.FirstOrDefault(), UserPrincipal.id);

            return Json(response);
        }

        #region old function
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustAdd(Customer model, string[] listAddress, string updateAddress)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);

            dbitem.CustomerAddress.Clear();

            if (listAddress != null)
            {
                foreach (string item in listAddress)
                {
                    string[] data = item.Split(';');
                    dbitem.CustomerAddress.Add(new Context.CustomerAddress()
                    {
                        Code = data[0],
                        Alamat = data[1],
                        Zona = data[2],
                        IdProvinsi = int.Parse(data[3]),
                        IdKabKota = int.Parse(data[4]),
                        IdKec = int.Parse(data[5]),
                        IdKel = int.Parse(data[6]),
                        Longitude = data[7],
                        Latitude = data[8],
                        Radius = int.Parse(data[9]),
                        Telp = data[10],
                        Fax = data[11],
                        OfficeTypeId = int.Parse(data[12])
                    });
                }
            }
            RepoCustomer.save(dbitem);
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updateAddress == "Update" ? "Address" : "ProductType", stat = "success" }));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustProdType(Customer model, string[] ProdTypeData, string updateProdType)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerProductType.Clear();

            if (ProdTypeData != null)
            {
                foreach (string item in ProdTypeData)
                {
                    string[] data = item.Split(';');

                    dbitem.CustomerProductType.Add(new Context.CustomerProductType()
                    {
                        idProduk = Convert.ToInt32(data[1]),
                        Keterangan = data[2],
                        PenangananKhusus = data[3]
                    });
                }
            }
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelAllCustomerProductTypeQuery(dbitem, UserPrincipal.id);
            foreach(Context.CustomerProductType cpt in dbitem.CustomerProductType){
                RepoAuditrail.saveCustomerProductTypeQuery(cpt, UserPrincipal.id);
            }
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updateProdType == "Update" ? "ProductType" : "LoadingAddress", stat = "success" }));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustPic(Customer model, string[] listPic, string updatePic)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);

            //dbitem.CustomerPic.Clear();

            if (listPic != null)
            {
                List<int> listIdData = new List<int>();
                foreach (string pic in listPic)
                {
                    string[] data = pic.Split(';');
                    int picId = Convert.ToInt32(data[0]);

                    Context.CustomerPic dbpic;
                    if (dbitem.CustomerPic.Any(d => d.Id == picId) && picId != 0)
                    {
                        //edit
                        listIdData.Add(picId);
                        dbpic = dbitem.CustomerPic.Where(d => d.Id == int.Parse(data[0])).FirstOrDefault();
                        dbpic.Name = data[1];
                        dbpic.DepartemenId = int.Parse(data[6]);
                        dbpic.JabatanId = int.Parse(data[7]);
                        dbpic.EmailAdd = data[4].ToString();
                        dbpic.Mobile = data[5].ToString();
                        RepoAuditrail.saveUpdCustomerPICQuery(dbpic, UserPrincipal.id);
                    }
                    else
                    {
                        //add
                        dbpic = new Context.CustomerPic() {Name = data[1], DepartemenId = int.Parse(data[6]), JabatanId = int.Parse(data[7]), EmailAdd = data[4].ToString(), Mobile = data[5].ToString()};
                        dbitem.CustomerPic.Add(dbpic);
                        RepoAuditrail.saveCustomerPICQuery(dbpic, UserPrincipal.id);
                    }
                }
                //hapus data berdasarkan yang tidak terdapat didalam list Id
                List<Context.CustomerPic> dbremove = dbitem.CustomerPic.Where(d => !listIdData.Contains(d.Id) && d.Id != 0).ToList();
                foreach (Context.CustomerPic item in dbremove)
                {
                    dbitem.CustomerPic.Remove(item);
                    RepoAuditrail.saveDelCustomerPICQuery(item, UserPrincipal.id);
                }
            }
            else
            {
                dbitem.CustomerPic.Clear();
                RepoAuditrail.saveDelAllCustomerPICQuery(dbitem, UserPrincipal.id);
            }
            RepoCustomer.save(dbitem);
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updatePic == "Update" ? "Pic" : "Address", stat = "success" }));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustLoadingAdd(Customer model, string[] listLoadingAddress, string updateLoadingAddress)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerLoadingAddress.Clear();

            if (listLoadingAddress != null)
            {
                foreach (string item in listLoadingAddress)
                {
                    string[] data = item.Split(';');
                    dbitem.CustomerLoadingAddress.Add(new Context.CustomerLoadingAddress()
                    {
                        Code = data[0],
                        Alamat = data[1],
                        Zona = data[2],
                        IdProvinsi = int.Parse(data[3]),
                        IdKabKota = int.Parse(data[4]),
                        IdKec = int.Parse(data[5]),
                        IdKel = int.Parse(data[6]),
                        Longitude = data[7],
                        Latitude = data[8],
                        Radius = int.Parse(data[9]),
                        Telp = data[10],
                        Fax = data[11]
                    });
                }
            }
            RepoAuditrail.saveDelAllCustomerLoadingAddressQuery(dbitem, UserPrincipal.id);
            RepoCustomer.save(dbitem);
            foreach (Context.CustomerLoadingAddress item in dbitem.CustomerLoadingAddress)
            {
                RepoAuditrail.saveCustomerLoadingAddressQuery(item, UserPrincipal.id);
            }
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updateLoadingAddress == "Update" ? "LoadingAddress" : "UnloadingAddress", stat = "success" }));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustUnloadingAdd(Customer model, string[] listUnloadingAddress, string updateUnloadAddress)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerUnloadingAddress.Clear();

            if (listUnloadingAddress != null)
            {
                foreach (string item in listUnloadingAddress)
                {
                    string[] data = item.Split(';');
                    dbitem.CustomerUnloadingAddress.Add(new Context.CustomerUnloadingAddress()
                    {
                        Code = data[0],
                        Alamat = data[1],
                        Zona = data[2],
                        IdProvinsi = int.Parse(data[3]),
                        IdKabKota = int.Parse(data[4]),
                        IdKec = int.Parse(data[5]),
                        IdKel = int.Parse(data[6]),
                        Longitude = data[7],
                        Latitude = data[8],
                        Radius = int.Parse(data[9]),
                        Telp = data[10],
                        Fax = data[11]
                    });
                }
            }
            RepoCustomer.save(dbitem);
            var query = "DELETE FROM dbo.\"CustomerUnloadingAddress\" WHERE \"CustomerId\"= " + dbitem.Id + ";";
            foreach (Context.CustomerUnloadingAddress item in dbitem.CustomerUnloadingAddress)
            {
                query += "INSERT INTO dbo.\"CustomerUnloadingAddress\" (\"CustomerId\", \"Code\", \"Alamat\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\", \"Longitude\", " + "\"Latitude\", " +
                    "\"Radius\", \"Zona\", \"Telp\", \"Fax\", urutan) VALUES (" + dbitem.Id + ", " + item.Code + ", " + item.Alamat + ", " + item.IdProvinsi + ", " + item.IdKabKota + ", " + item.IdKec +
                    ", " + item.IdKel + ", " + item.Longitude + ", " + item.Latitude + ", " + item.Radius + ", " + item.Zona + ", " + item.Telp + ", " + item.Fax + ", " + item.urutan + ");";
            }
            RepoAuditrail.SetAuditTrail(query, "Customer Unloading Address", "Delete", UserPrincipal.id);
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updateUnloadAddress == "Update" ? "UnloadingAddress" : "Supplier", stat = "success" }));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustSupplier(Customer model, string[] listSupplier, string updateSupplier)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerSupplier.Clear();

            if (listSupplier != null)
            {
                foreach (string item in listSupplier)
                {
                    string[] data = item.Split(';');
                    dbitem.CustomerSupplier.Add(new Context.CustomerSupplier()
                    {
                        Code = data[0],
                        Alamat = data[1],
                        Zona = data[2],
                        IdProvinsi = int.Parse(data[3]),
                        IdKabKota = int.Parse(data[4]),
                        IdKec = int.Parse(data[5]),
                        IdKel = int.Parse(data[6]),
                        Longitude = data[7],
                        Latitude = data[8],
                        Radius = int.Parse(data[9]),
                        Telp = data[10],
                        Fax = data[11],
                        Pic = data[12],
                        Nama = data[13],
                    });
                }
            }
            RepoCustomer.save(dbitem);
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updateSupplier == "Update" ? "Supplier" : "Billing", stat = "success" }));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustNotif(Customer model, string[] listNotif, string updateNotif)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerNotification.Clear();

            if (listNotif != null)
            {
                foreach (string item in listNotif)
                {
                    string[] data = item.Split(';');
                    Context.CustomerNotification dbNotif = new Context.CustomerNotification()
                    {
                        IsActive = bool.Parse(data[0]),
                        IdPic = int.Parse(data[1]),
                        NotifType = data[2],
                    };
                    foreach (string idrute in data[3].Split(','))
                    {
                        dbNotif.CustomerNotifRute.Add(new Context.CustomerNotifRute()
                        {
                            IdRute = int.Parse(idrute)
                        });
                    }
                    foreach (string idtruck in data[4].Split(','))
                    {
                        dbNotif.CustomerNotifTruck.Add(new Context.CustomerNotifTruck()
                        {
                            IdTruck = int.Parse(idtruck)
                        });
                    }

                    dbitem.CustomerNotification.Add(dbNotif);
                }
            }
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelAllCustomerNotificationQuery(dbitem, UserPrincipal.id);
            RepoAuditrail.saveDelAllCustomerNotifRuteQuery(dbitem.CustomerNotification.FirstOrDefault(), UserPrincipal.id);
            RepoAuditrail.saveDelAllCustomerNotifTruckQuery(dbitem.CustomerNotification.FirstOrDefault(), UserPrincipal.id);
            foreach(Context.CustomerNotification cn in dbitem.CustomerNotification){
                RepoAuditrail.saveCustomerNotificationQuery(cn, UserPrincipal.id);
            }
            foreach(Context.CustomerNotification cn in dbitem.CustomerNotification){
                RepoAuditrail.saveCustomerNotificationQuery(cn, UserPrincipal.id);
            }
            foreach(Context.CustomerNotifTruck cnt in dbitem.CustomerNotification.FirstOrDefault().CustomerNotifTruck){
                RepoAuditrail.saveCustomerNotifTruckQuery(cnt, UserPrincipal.id);
            }
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updateNotif == "Update" ? "notif" : "attc", stat = "success" }));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCustBilling(Customer model, string[] listBilling, string updateBilling)
        {
            Context.Customer dbitem = RepoCustomer.FindByPK(model.Id);
            dbitem.CustomerBilling.Clear();

            if (listBilling != null)
            {
                foreach (string item in listBilling)
                {
                    string[] data = item.Split(';');
                    Context.CustomerBilling dbbilling = new Context.CustomerBilling()
                    {
                        DocumentName = data[0],
                        Lembar = int.Parse(data[1]),
                        Warna = data[2],
                        Stempel = bool.Parse(data[3]),
                        UrlAtt = data[4],
                        FileName = data[5],
                        IsFax = bool.Parse(data[6]),
                        Fax = data[7],
                        IsEmail = bool.Parse(data[8]),
                        Email = data[9],
                        IsTukarFaktur = bool.Parse(data[10]),
                        IsJasaPengiriman = bool.Parse(data[12])
                    };

                    ObjDataJadwal[] result = JsonConvert.DeserializeObject<ObjDataJadwal[]>(data[11]);

                    foreach (ObjDataJadwal datJ in result)
                    {
                        dbbilling.CustomerJadwalBilling.Add(new Context.CustomerJadwalBilling()
                        {
                            Hari = datJ.Hari,
                            Jam = datJ.Jam,
                            Catatan = datJ.Catatan,
                            PIC = datJ.Email
                        });
                    }

                    dbitem.CustomerBilling.Add(dbbilling);
                    RepoAuditrail.saveCustomerBillingQuery(dbbilling, UserPrincipal.id);
                }
            }
            RepoCustomer.save(dbitem);
            RepoAuditrail.saveDelAllCustomerBillingQuery(dbitem, UserPrincipal.id);
            foreach (Context.CustomerJadwalBilling cjb in dbitem.CustomerBilling.FirstOrDefault().CustomerJadwalBilling)
            {
                RepoAuditrail.saveCustomerJadwalBillingQuery(cjb, UserPrincipal.id);
            }
            ViewBag.status = "success";
            return RedirectToAction("Edit", new RouteValueDictionary(new { id = dbitem.Id, activeTab = updateBilling == "Update" ? "Billing" : "PPN", stat = "success" }));
        }
        #endregion

        #region import
        public string UploadDetail(IEnumerable<HttpPostedFileBase> filesDataCustomerDetail)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesDataCustomerDetail != null)
            {
                foreach (var file in filesDataCustomerDetail)
                {
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            UploadPic(package);
                            UploadAddress(package);
                            UploadLoadingAddress(package);
                            UploadUnloadingAddress(package);
                            UploadSupplier(package);
                            UploadProductType(package);
                            UploadBilling(package);
                            //UploadNotif(package);
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

        public string UploadBasicData(IEnumerable<HttpPostedFileBase> filesDataCustomer)
        {
            ResponeModel response = new ResponeModel();
            //algoritma
            if (filesDataCustomer != null)
            {
                foreach (var file in filesDataCustomer)
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
    //                            try
      //                          {
                                    //cek mandatory utama
                                    if (workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null &&
                                        workSheet.Cells[rowIterator, 6].Value != null && workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 1].Value != null)
                                    {
                                        Context.Customer dbitem = new Context.Customer();
                                        Context.CustomerPPN dbppn = new Context.CustomerPPN();
                                        //cek mandatory ppn
                                        bool restBool;
                                        if (!bool.TryParse(workSheet.Cells[rowIterator, 8].Value.ToString(), out restBool))
                                        {
                                            continue;
                                        }

                                        if (restBool)
                                        {
                                            if (workSheet.Cells[rowIterator, 9].Value != null && workSheet.Cells[rowIterator, 10].Value != null &&
                                                workSheet.Cells[rowIterator, 11].Value != null && workSheet.Cells[rowIterator, 12].Value != null)
                                            {
                                                dbppn.PPN = bool.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                                                dbppn.IdRekening = RepoRekening.FindByName(workSheet.Cells[rowIterator, 9].Value.ToString()).Id;
                                                dbppn.NomorNPWP = workSheet.Cells[rowIterator, 10].Value.ToString();
                                                dbppn.NamaNPWP = workSheet.Cells[rowIterator, 11].Value.ToString();
                                                dbppn.AddressNPWP = workSheet.Cells[rowIterator, 12].Value.ToString();
                                            }
                                        }
                                        //end cek mandatory ppn

                                        int id = 0;

                                        int resId;
                                        if (workSheet.Cells[rowIterator, 14].Value != null)
                                        {
                                            if (int.TryParse(workSheet.Cells[rowIterator, 14].Value.ToString(), out resId))
                                                id = resId;
                                        }

                                        if (id != 0)
                                        {
                                            dbitem = RepoCustomer.FindByPK(id);
                                        }

                                        dbitem.CustomerCodeOld = workSheet.Cells[rowIterator, 1].Value != null ? workSheet.Cells[rowIterator, 1].Value.ToString() : "";
                                        dbitem.urutan = RepoCustomer.getUrutan() + 1;
                                        dbitem.CustomerCode = RepoCustomer.generateCode(dbitem.urutan);
                                        dbitem.CustomerNama = workSheet.Cells[rowIterator, 3].Value.ToString();
                                        dbitem.PrioritasId = RepoLookup.FindByName(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                                        dbitem.WajibPO = bool.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                        dbitem.WajibGPS = bool.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                                        dbitem.IsVendor = bool.Parse(workSheet.Cells[rowIterator, 14].Value.ToString());
                                        dbitem.SpecialTreatment = workSheet.Cells[rowIterator, 7].Value != null ? workSheet.Cells[rowIterator, 7].Value.ToString() : "";
                                        
                                        if (dbitem.CustomerPPN.Count() > 0)
                                        {
                                            var dummyPPN = dbitem.CustomerPPN.FirstOrDefault();
                                            dummyPPN.PPN = dbppn.PPN;
                                            if(dummyPPN.PPN)
                                            {
                                                dbppn.IdRekening = RepoRekening.FindByName(workSheet.Cells[rowIterator, 9].Value.ToString()).Id;
                                                dbppn.NomorNPWP = workSheet.Cells[rowIterator, 10].Value.ToString();
                                                dbppn.NamaNPWP = workSheet.Cells[rowIterator, 11].Value.ToString();
                                                dbppn.AddressNPWP = workSheet.Cells[rowIterator, 12].Value.ToString();
                                            }
                                            RepoAuditrail.saveCustomerPPNQuery(dbitem.CustomerPPN.FirstOrDefault(), UserPrincipal.id);
                                        }
                                        else
                                        {
                                            dbitem.CustomerPPN.Add(dbppn);
                                            RepoAuditrail.saveUpdCustomerPPNQuery(dbitem.CustomerPPN.FirstOrDefault(), UserPrincipal.id);
                                        }
                                        RepoCustomer.save(dbitem);
                                    }
        /*                        }
                                catch (Exception)
                                {
                                    continue;
                                }*/
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

        public void UploadPic(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 1).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null)
                {
                    if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 7].Value != null)
                    {
                        //edit
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerPic dbpic = db.CustomerPic.Where(d => d.Id == int.Parse(workSheet.Cells[rowIterator, 7].Value.ToString())).FirstOrDefault();

                            dbpic.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbpic.DepartemenId = RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbpic.JabatanId = RepoLookup.FindByName(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbpic.EmailAdd = workSheet.Cells[rowIterator, 5].Value.ToString();
                            dbpic.Mobile = workSheet.Cells[rowIterator, 6].Value.ToString();

                            RepoCustomer.save(db);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        //add
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerPic dbpic = new Context.CustomerPic();
                            int Urutan = db.CustomerPic.Count() == 0 ? 1 : db.CustomerPic.Max(d => d.Urutan) + 1;
                            dbpic.Urutan = Urutan;
                            dbpic.Code = "CP-" + (Urutan).ToString().PadLeft(4, '0');
                            dbpic.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbpic.DepartemenId = RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbpic.JabatanId = RepoLookup.FindByName(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbpic.EmailAdd = workSheet.Cells[rowIterator, 5].Value.ToString();
                            dbpic.Mobile = workSheet.Cells[rowIterator, 6].Value.ToString();
                            db.CustomerPic.Add(dbpic);
                            RepoCustomer.save(db);
                            RepoAuditrail.saveCustomerPICQuery(dbpic, UserPrincipal.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void UploadAddress(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 2).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 9].Value != null &&
                    workSheet.Cells[rowIterator, 10].Value != null && workSheet.Cells[rowIterator, 11].Value != null && workSheet.Cells[rowIterator, 12].Value != null &&
                    workSheet.Cells[rowIterator, 13].Value != null)
                {
                    if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 14].Value != null)
                    {
                        //edit
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerAddress dbitem = db.CustomerAddress.Where(d => d.Id == int.Parse(workSheet.Cells[rowIterator, 14].Value.ToString())).FirstOrDefault();

                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            dbitem.OfficeTypeId = RepoLookup.FindByName(workSheet.Cells[rowIterator, 11].Value.ToString()).Id;
                            dbitem.Telp = workSheet.Cells[rowIterator, 12].Value.ToString();
                            dbitem.Fax = workSheet.Cells[rowIterator, 13].Value.ToString();

                            RepoCustomer.save(db);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        //add
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerAddress dbitem = new Context.CustomerAddress();

                            int Urutan = db.CustomerAddress.Count() == 0 ? 1 : db.CustomerAddress.Max(d => d.urutan) + 1;
                            dbitem.urutan = Urutan;
                            dbitem.Code = "CP-" + (Urutan).ToString().PadLeft(4, '0');
                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            dbitem.OfficeTypeId = RepoLookup.FindByName(workSheet.Cells[rowIterator, 11].Value.ToString()).Id;
                            dbitem.Telp = workSheet.Cells[rowIterator, 12].Value.ToString();
                            dbitem.Fax = workSheet.Cells[rowIterator, 13].Value.ToString();

                            db.CustomerAddress.Add(dbitem);
                            RepoCustomer.save(db);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void UploadLoadingAddress(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 4).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 9].Value != null &&
                    workSheet.Cells[rowIterator, 10].Value != null && workSheet.Cells[rowIterator, 11].Value != null && workSheet.Cells[rowIterator, 12].Value != null)
                {
                    if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 13].Value != null)
                    {
                        //edit
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerLoadingAddress dbitem = db.CustomerLoadingAddress.Where(d => d.Id == int.Parse(workSheet.Cells[rowIterator, 13].Value.ToString())).FirstOrDefault();

                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            dbitem.Telp = workSheet.Cells[rowIterator, 11].Value.ToString();
                            dbitem.Fax = workSheet.Cells[rowIterator, 12].Value.ToString();

                            RepoCustomer.save(db);
                            RepoAuditrail.saveCustomerLoadingAddressQuery(dbitem, UserPrincipal.id);

                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        //add
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerLoadingAddress dbitem = new Context.CustomerLoadingAddress();

                            int Urutan = db.CustomerLoadingAddress.Count() == 0 ? 1 : db.CustomerLoadingAddress.Max(d => d.urutan) + 1;
                            dbitem.urutan = Urutan;
                            dbitem.Code = "CP-" + (Urutan).ToString().PadLeft(4, '0');
                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            dbitem.Telp = workSheet.Cells[rowIterator, 11].Value.ToString();
                            dbitem.Fax = workSheet.Cells[rowIterator, 12].Value.ToString();

                            db.CustomerLoadingAddress.Add(dbitem);
                            RepoCustomer.save(db);
                            RepoAuditrail.saveCustomerLoadingAddressQuery(dbitem, UserPrincipal.id, "add");
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void UploadUnloadingAddress(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 5).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 9].Value != null &&
                    workSheet.Cells[rowIterator, 10].Value != null)
                {
                    if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 13].Value != null)
                    {
                        //edit
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerUnloadingAddress dbitem = db.CustomerUnloadingAddress.Where(d => d.Id == int.Parse(workSheet.Cells[rowIterator, 13].Value.ToString())).FirstOrDefault();

                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            if (workSheet.Cells[rowIterator, 4].Value != null)
                                dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            if (workSheet.Cells[rowIterator, 5].Value != null)
                                dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            if (workSheet.Cells[rowIterator, 6] != null)
                                dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            dbitem.Telp = workSheet.Cells[rowIterator, 11].Value.ToString();
                            dbitem.Fax = workSheet.Cells[rowIterator, 12].Value.ToString();

                            RepoCustomer.save(db);
                            RepoAuditrail.SetAuditTrail("UPDATE dbo.\"CustomerUnloadingAddress\" \"Alamat\" = " + dbitem.Alamat + ", \"IdProvinsi\" = " + dbitem.IdProvinsi + ", \"IdKabKota\" = " +
                                dbitem.IdKabKota + ", \"IdKec\" = " + dbitem.IdKec + ", \"IdKel\" = " + dbitem.IdKel + ", \"Longitude\" = " + dbitem.Longitude + ", \"Latitude\" = " + dbitem.Latitude +
                                ", \"Radius\" = " + dbitem.Radius + ", \"Zona\" = " + dbitem.Zona + ", \"Telp\" = " + dbitem.Telp + ", \"Fax\" = " + dbitem.Fax + ", urutan = " + dbitem.urutan +
                                " WHERE \"CustomerId\" = " + db.Id + ";", "Customer Unloading Address", "Upload", UserPrincipal.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        //add
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerUnloadingAddress dbitem = new Context.CustomerUnloadingAddress();

                            int Urutan = db.CustomerUnloadingAddress.Count() == 0 ? 1 : db.CustomerUnloadingAddress.Max(d => d.urutan) + 1;
                            dbitem.urutan = Urutan;
                            dbitem.Code = "CUA-" + (Urutan).ToString().PadLeft(4, '0');
                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            if (workSheet.Cells[rowIterator, 4].Value != null)
                                dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            if (workSheet.Cells[rowIterator, 5].Value != null)
                                dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            if (workSheet.Cells[rowIterator, 6].Value != null)
                                dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            if (workSheet.Cells[rowIterator, 11].Value != null)
                                dbitem.Telp = workSheet.Cells[rowIterator, 11].Value.ToString();
                            if (workSheet.Cells[rowIterator, 12].Value != null)
                                dbitem.Fax = workSheet.Cells[rowIterator, 12].Value.ToString();

                            db.CustomerUnloadingAddress.Add(dbitem);
                            RepoCustomer.save(db);
                            RepoAuditrail.SetAuditTrail("INSERT INTO dbo.\"CustomerUnloadingAddress\" (\"CustomerId\", \"Code\", \"Alamat\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\"," +
                                " \"Longitude\", \"Latitude\", \"Radius\", \"Zona\", \"Telp\", \"Fax\", urutan) VALUES (" + db.Id + ", " + dbitem.Code + ", " + dbitem.Alamat + ", " +
                                dbitem.IdProvinsi + ", " + dbitem.IdKabKota + ", " + dbitem.IdKec + ", " + dbitem.IdKel + ", " + dbitem.Longitude + ", " + dbitem.Latitude + ", " + dbitem.Radius + ", " +
                                dbitem.Zona + ", " + dbitem.Telp + ", " + dbitem.Fax + ", " + dbitem.urutan + ");", "Customer Unloading Address", "Upload", UserPrincipal.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void UploadSupplier(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 6).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                    workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 9].Value != null &&
                    workSheet.Cells[rowIterator, 10].Value != null && workSheet.Cells[rowIterator, 11].Value != null && workSheet.Cells[rowIterator, 12].Value != null &&
                    workSheet.Cells[rowIterator, 13].Value != null)
                {
                    if (workSheet.Cells[rowIterator, 14].Value != null && workSheet.Cells[rowIterator, 15].Value != null)
                    {
                        //edit
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 14].Value.ToString());
                            Context.CustomerSupplier dbitem = db.CustomerSupplier.Where(d => d.Id == int.Parse(workSheet.Cells[rowIterator, 15].Value.ToString())).FirstOrDefault();

                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            dbitem.Pic = workSheet.Cells[rowIterator, 11].Value.ToString();
                            dbitem.Telp = workSheet.Cells[rowIterator, 12].Value.ToString();
                            dbitem.Fax = workSheet.Cells[rowIterator, 13].Value.ToString();

                            RepoCustomer.save(db);
                            RepoAuditrail.saveCustomerSupplierQuery(dbitem, UserPrincipal.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        //add
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerSupplier dbitem = new Context.CustomerSupplier();

                            int Urutan = db.CustomerSupplier.Count() == 0 ? 1 : db.CustomerSupplier.Max(d => d.urutan) + 1;
                            dbitem.urutan = Urutan;
                            dbitem.Code = "CP-" + (Urutan).ToString().PadLeft(4, '0');
                            dbitem.Alamat = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dbitem.IdProvinsi = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 3].Value.ToString()).Id;
                            dbitem.IdKabKota = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 4].Value.ToString()).Id;
                            dbitem.IdKec = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 5].Value.ToString()).Id;
                            dbitem.IdKel = RepoLocation.FindByCode(workSheet.Cells[rowIterator, 6].Value.ToString()).Id;
                            dbitem.Longitude = workSheet.Cells[rowIterator, 7].Value.ToString();
                            dbitem.Latitude = workSheet.Cells[rowIterator, 8].Value.ToString();
                            dbitem.Radius = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                            dbitem.Zona = workSheet.Cells[rowIterator, 10].Value.ToString();
                            dbitem.Pic = workSheet.Cells[rowIterator, 11].Value.ToString();
                            dbitem.Telp = workSheet.Cells[rowIterator, 12].Value.ToString();
                            dbitem.Fax = workSheet.Cells[rowIterator, 13].Value.ToString();

                            db.CustomerSupplier.Add(dbitem);
                            RepoCustomer.save(db);
                            RepoAuditrail.saveUpdCustomerSupplierQuery(dbitem, UserPrincipal.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void UploadProductType(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 3).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null)
                {
                    if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 4].Value != null)
                    {
                        //edit
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerProductType dbitem = db.CustomerProductType.Where(d => d.Id == int.Parse(workSheet.Cells[rowIterator, 4].Value.ToString())).FirstOrDefault();

                            dbitem.idProduk = RepoProduct.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                            List<string> listTreatment = new List<string>();

                            if (RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()) != null)
                            {
                                listTreatment.Add(workSheet.Cells[rowIterator, 3].ToString());
                            }

                            for (int idx = rowIterator + 1; idx <= noOfRow; idx++)
                            {
                                if (workSheet.Cells[idx, 1].Value == null)
                                {
                                    if (RepoLookup.FindByName(workSheet.Cells[idx, 3].Value.ToString()) != null)
                                    {
                                        listTreatment.Add(workSheet.Cells[idx, 3].ToString());
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            dbitem.PenangananKhusus = string.Join(", ", listTreatment);

                            RepoCustomer.save(db);
                            RepoAuditrail.saveUpdCustomerProductTypeQuery(dbitem, UserPrincipal.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        //add
                        try
                        {
                            Context.Customer db = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            Context.CustomerProductType dbitem = new Context.CustomerProductType();
                            dbitem.idProduk = RepoProduct.FindByName(workSheet.Cells[rowIterator, 2].Value.ToString()).Id;
                            List<string> listTreatment = new List<string>();

                            if (RepoLookup.FindByName(workSheet.Cells[rowIterator, 3].Value.ToString()) != null)
                            {
                                listTreatment.Add(workSheet.Cells[rowIterator, 3].ToString());
                            }

                            for (int idx = rowIterator + 1; idx <= noOfRow; idx++)
                            {
                                if (workSheet.Cells[idx, 1].Value == null)
                                {
                                    if (RepoLookup.FindByName(workSheet.Cells[idx, 3].Value.ToString()) != null)
                                    {
                                        listTreatment.Add(workSheet.Cells[idx, 3].ToString());
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            dbitem.PenangananKhusus = string.Join(", ", listTreatment);

                            db.CustomerProductType.Add(dbitem);
                            RepoCustomer.save(db);
                            RepoAuditrail.saveCustomerProductTypeQuery(dbitem, UserPrincipal.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void UploadBilling(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 7).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                try
                {
                    if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                    workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null &&
                    workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 10].Value != null && workSheet.Cells[rowIterator, 14].Value != null)
                    {
                        int id = 0;
                        int resId;
                        Context.Customer dbCust = new Context.Customer();
                        Context.CustomerBilling dbitem = new Context.CustomerBilling();

                        if (workSheet.Cells[rowIterator, 16].Value != null)
                        {
                            if (int.TryParse(workSheet.Cells[rowIterator, 16].Value.ToString(), out resId))
                                id = resId;
                            dbCust = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            dbitem = dbCust.CustomerBilling.Where(d => d.Id == id).FirstOrDefault();
                        }
                        else if (workSheet.Cells[rowIterator, 1].Value != null)
                        {
                            dbCust = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                        }

                        //dbCust.CustomerBilling.Clear();
                        dbitem.DocumentName = workSheet.Cells[rowIterator, 2].Value.ToString();
                        dbitem.Lembar = int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                        dbitem.Stempel = bool.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                        dbitem.IsFax = bool.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                        if (dbitem.IsFax)
                        {
                            if (workSheet.Cells[rowIterator, 7].Value == null)
                                continue;
                            else
                            {
                                dbitem.Fax = workSheet.Cells[rowIterator, 7].Value.ToString();
                            }
                        }
                        dbitem.IsEmail = bool.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                        if (dbitem.IsEmail)
                        {
                            if (workSheet.Cells[rowIterator, 9].Value == null)
                                continue;
                            else
                            {
                                dbitem.Email = workSheet.Cells[rowIterator, 9].Value.ToString();
                            }
                        }
                        dbitem.IsTukarFaktur = bool.Parse(workSheet.Cells[rowIterator, 10].Value.ToString());
                        dbitem.IsJasaPengiriman = bool.Parse(workSheet.Cells[rowIterator, 15].Value.ToString());

                        //warna
                        int idxWarna = 0;
                        List<string> lisrWarna = new List<string>();
                        for (idxWarna = rowIterator; idxWarna <= noOfRow; idxWarna++)
                        {
                            if (RepoLookup.FindByName(workSheet.Cells[idxWarna, 4].Value.ToString()) != null)
                            {
                                lisrWarna.Add(workSheet.Cells[idxWarna, 4].Value.ToString());
                            }
                        }
                        dbitem.Warna = string.Join(", ", lisrWarna);

                        //item faktur
                        int idFaktur = 0;
                        if (dbitem.IsTukarFaktur)
                        {
                            for (idFaktur = rowIterator; idFaktur <= noOfRow; idFaktur++)
                            {
                                if (workSheet.Cells[idFaktur, 11].Value == null && workSheet.Cells[idFaktur, 12].Value == null &&
                                workSheet.Cells[idFaktur, 13].Value == null && workSheet.Cells[idFaktur, 14].Value == null)
                                    continue;
                                else
                                {
                                    Context.CustomerJadwalBilling dbJadwal = new Context.CustomerJadwalBilling();
                                    dbJadwal.Hari = workSheet.Cells[idFaktur, 11].Value.ToString();
                                    dbJadwal.Jam = workSheet.Cells[idFaktur, 12].Value.ToString();
                                    dbJadwal.Catatan = workSheet.Cells[idFaktur, 13].Value.ToString();
                                    dbJadwal.PIC = workSheet.Cells[idFaktur, 14].Value.ToString();
                                    dbitem.CustomerJadwalBilling.Add(dbJadwal);
                                    RepoAuditrail.saveCustomerJadwalBillingQuery(dbJadwal, UserPrincipal.id);
                                }
                            }
                        }
                        if (idxWarna > idFaktur)
                            rowIterator = idxWarna;
                        else
                            rowIterator = idFaktur;

                        dbCust.CustomerBilling.Add(dbitem);
                        RepoCustomer.save(dbCust);
                        RepoAuditrail.saveCustomerBillingQuery(dbitem, UserPrincipal.id);
                    }
                    
                }
                catch (Exception)
                {

                }
            }
        }

        public void UploadNotif(ExcelPackage package)
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.Where(d => d.Index == 8).FirstOrDefault();
            var noOfRow = workSheet.Dimension.End.Row;

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                try
                {
                    if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null &&
                        workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 6].Value != null)
                    {
                        int id = 0;
                        int resId;
                        Context.Customer dbCust = new Context.Customer();
                        Context.CustomerNotification dbitem = new Context.CustomerNotification();
                        if (workSheet.Cells[rowIterator, 7].Value != null)
                        {
                            if (int.TryParse(workSheet.Cells[rowIterator, 7].Value.ToString(), out resId))
                                id = resId;
                            dbCust = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                            dbitem = dbCust.CustomerNotification.Where(d => d.Id == id).FirstOrDefault();
                        }
                        else if (workSheet.Cells[rowIterator, 1].Value != null)
                        {
                            dbCust = RepoCustomer.FindByCode(workSheet.Cells[rowIterator, 1].Value.ToString());
                        }

                        //dbCust.CustomerNotification.Clear();
                        dbitem.IsActive = bool.Parse(workSheet.Cells[rowIterator, 2].Value.ToString());
                        Context.CustomerPic dbpic = dbCust.CustomerPic.Where(d => d.Code == workSheet.Cells[rowIterator, 3].Value.ToString()).FirstOrDefault();
                        dbitem.IdPic = dbpic.Id;
                        if (workSheet.Cells[rowIterator, 4].Value.ToString() == "SMS")
                            dbitem.NotifType = "SMS";
                        else if (workSheet.Cells[rowIterator, 4].Value.ToString() == "EMAIL")
                            dbitem.NotifType = "EMAIL";
                        //rute
                        int idxRute = 0;
                        for (idxRute = rowIterator; idxRute <= noOfRow; idxRute++)
                        {
                            if (RepoRute.FindByKode(workSheet.Cells[idxRute, 5].Value.ToString()) != null)
                            {
                                Context.CustomerNotifRute dbrute = new Context.CustomerNotifRute();
                                dbrute.IdRute = RepoRute.FindByKode(workSheet.Cells[idxRute, 5].Value.ToString()).Id;
                                dbitem.CustomerNotifRute.Add(dbrute);
                            }
                        }
                        
                        //truck
                        int idxTruck = 0;
                        for (idxTruck = rowIterator; idxTruck <= noOfRow; idxTruck++)
                        {
                            if (RepoRute.FindByKode(workSheet.Cells[idxTruck, 6].Value.ToString()) != null)
                            {
                                Context.CustomerNotifTruck dbtruck = new Context.CustomerNotifTruck();
                                dbtruck.IdTruck = RepoTruck.FindByName(workSheet.Cells[idxTruck, 6].Value.ToString()).Id;
                                dbitem.CustomerNotifTruck.Add(dbtruck);
                            }
                        }

                        if (idxRute > idxTruck)
                            rowIterator = idxRute;
                        else
                            rowIterator = idxTruck;

                        dbCust.CustomerNotification.Add(dbitem);
                        RepoCustomer.save(dbCust);
                        RepoAuditrail.saveCustomerNotificationQuery(dbitem, UserPrincipal.id);
                        foreach(Context.CustomerNotifRute cnr in dbitem.CustomerNotifRute){
                            RepoAuditrail.saveCustomerNotifRuteQuery(cnr, UserPrincipal.id);
                        }
                        foreach(Context.CustomerNotifTruck cnt in dbitem.CustomerNotifTruck){
                            RepoAuditrail.saveCustomerNotifTruckQuery(cnt, UserPrincipal.id);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        #endregion

        #region export

        public FileContentResult ExportBasicData()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Kode Lama";
            ws.Cells[1, 2].Value = "Kode Customer";
            ws.Cells[1, 3].Value = "Nama Customer";
            ws.Cells[1, 4].Value = "Prioritas";
            ws.Cells[1, 5].Value = "Wajib PO";
            ws.Cells[1, 6].Value = "Wajib GPS";
            ws.Cells[1, 7].Value = "Penanganan Khusus";
            ws.Cells[1, 8].Value = "Is PPN";
            ws.Cells[1, 9].Value = "Rekening";
            ws.Cells[1, 10].Value = "No NPWP";
            ws.Cells[1, 11].Value = "Nama NPWP";
            ws.Cells[1, 12].Value = "Alamat NPWP";
            ws.Cells[1, 13].Value = "Keterangan";
            ws.Cells[1, 14].Value = "Id Database";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = dbitems[i].CustomerCodeOld;
                ws.Cells[i + 2, 2].Value = dbitems[i].CustomerCode;
                ws.Cells[i + 2, 3].Value = dbitems[i].CustomerNama;
                ws.Cells[i + 2, 4].Value = dbitems[i].LookupCode.Nama;
                ws.Cells[i + 2, 5].Value = dbitems[i].WajibPO;
                ws.Cells[i + 2, 6].Value = dbitems[i].WajibGPS;
                ws.Cells[i + 2, 7].Value = dbitems[i].SpecialTreatment;

                if (dbitems[i].CustomerPPN.Count() > 0)
                {
                    ws.Cells[i + 2, 8].Value = dbitems[i].CustomerPPN.FirstOrDefault().PPN;
                    if (Convert.ToBoolean(dbitems[i].CustomerPPN.FirstOrDefault().PPN))
                    {
                        ws.Cells[i + 2, 9].Value = dbitems[i].CustomerPPN.FirstOrDefault().Rekenings == null ? null : dbitems[i].CustomerPPN.FirstOrDefault().Rekenings.NamaRekening;
                        ws.Cells[i + 2, 10].Value = dbitems[i].CustomerPPN.FirstOrDefault().NomorNPWP;
                        ws.Cells[i + 2, 11].Value = dbitems[i].CustomerPPN.FirstOrDefault().NamaNPWP;
                        ws.Cells[i + 2, 12].Value = dbitems[i].CustomerPPN.FirstOrDefault().AddressNPWP;
                    }
                }
                else
                {
                    ws.Cells[i + 2, 8].Value = "TRUE";
                }

                ws.Cells[i + 2, 13].Value = dbitems[i].Keterangan;
                ws.Cells[i + 2, 14].Value = dbitems[i].Id;
            }


            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Customer_Basic_Data.xls";

            return fsr;
        }

        [MyAuthorize(Menu = "Customer", Action="read")]
        public FileContentResult ExportDetail()
        {
            //bikin file baru
            ExcelPackage pck = new ExcelPackage();
            ExportPic(pck);
            ExportAddress(pck);
            ExportProductType(pck);
            ExportLoadingAddress(pck);
            ExportUnloadingAddress(pck);
            ExportSupplier(pck);
            ExportBilling(pck);

            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Customer_Detail.xls";

            return fsr;
        }

        public void ExportPic(ExcelPackage pck)
        {
            //bikin file baru
            //ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Pic");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Nama";
            ws.Cells[1, 3].Value = "Departement";
            ws.Cells[1, 4].Value = "Jabatan";
            ws.Cells[1, 5].Value = "Email";
            ws.Cells[1, 6].Value = "Handphone";
            ws.Cells[1, 7].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerPic.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerPic.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerPic.ToList()[j].Name;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerPic.ToList()[j].LookUpCodesDept.Nama;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerPic.ToList()[j].LookUpCodesJabatan.Nama;
                    ws.Cells[idx, 5].Value = dbitems[i].CustomerPic.ToList()[j].EmailAdd;
                    ws.Cells[idx, 6].Value = dbitems[i].CustomerPic.ToList()[j].Mobile;
                    ws.Cells[idx, 7].Value = dbitems[i].CustomerPic.ToList()[j].Id;

                    idx++;
                }
            }
            //var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //fsr.FileDownloadName = "Customer_Detail.xls";
        }

        public void ExportAddress(ExcelPackage pck)
        {
            //bikin file baru
            //ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Address");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Alamat";
            ws.Cells[1, 3].Value = "Provinsi";
            ws.Cells[1, 4].Value = "Kabupatan/Kota";
            ws.Cells[1, 5].Value = "Kecamatan";
            ws.Cells[1, 6].Value = "Kelurahan/Desa";
            ws.Cells[1, 7].Value = "Longitude";
            ws.Cells[1, 8].Value = "Latitude";
            ws.Cells[1, 9].Value = "Radius";
            ws.Cells[1, 10].Value = "Zona";
            ws.Cells[1, 11].Value = "Office Type";
            ws.Cells[1, 12].Value = "Telephone";
            ws.Cells[1, 13].Value = "Fax";
            ws.Cells[1, 14].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerAddress.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerAddress.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerAddress.ToList()[j].Alamat;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerAddress.ToList()[j].LocProvinsi == null ? "" : dbitems[i].CustomerAddress.ToList()[j].LocProvinsi.Code;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerAddress.ToList()[j].LocKabKota == null ? "" : dbitems[i].CustomerAddress.ToList()[j].LocKabKota.Code;
                    ws.Cells[idx, 5].Value = dbitems[i].CustomerAddress.ToList()[j].LocKecamatan == null ? "" : dbitems[i].CustomerAddress.ToList()[j].LocKecamatan.Code;
                    ws.Cells[idx, 6].Value = dbitems[i].CustomerAddress.ToList()[j].LocKelurahan == null ? "" : dbitems[i].CustomerAddress.ToList()[j].LocKelurahan.Code;
                    ws.Cells[idx, 7].Value = dbitems[i].CustomerAddress.ToList()[j].Longitude;
                    ws.Cells[idx, 8].Value = dbitems[i].CustomerAddress.ToList()[j].Latitude;
                    ws.Cells[idx, 9].Value = dbitems[i].CustomerAddress.ToList()[j].Radius;
                    ws.Cells[idx, 10].Value = dbitems[i].CustomerAddress.ToList()[j].Zona;
                    ws.Cells[idx, 11].Value = dbitems[i].CustomerAddress.ToList()[j].LookUpCodesOffice == null ? null : dbitems[i].CustomerAddress.ToList()[j].LookUpCodesOffice.Nama;
                    ws.Cells[idx, 12].Value = dbitems[i].CustomerAddress.ToList()[j].Telp;
                    ws.Cells[idx, 13].Value = dbitems[i].CustomerAddress.ToList()[j].Fax;
                    ws.Cells[idx, 14].Value = dbitems[i].CustomerAddress.ToList()[j].Id;


                    idx++;
                }
            }
            //var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //fsr.FileDownloadName = "Customer_Detail.xls";

            //return fsr;
        }

        public void ExportLoadingAddress(ExcelPackage pck)
        {
            //bikin file baru
            //ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("LoadingAddress");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Alamat";
            ws.Cells[1, 3].Value = "Provinsi";
            ws.Cells[1, 4].Value = "Kabupatan/Kota";
            ws.Cells[1, 5].Value = "Kecamatan";
            ws.Cells[1, 6].Value = "Kelurahan/Desa";
            ws.Cells[1, 7].Value = "Longitude";
            ws.Cells[1, 8].Value = "Latitude";
            ws.Cells[1, 9].Value = "Radius";
            ws.Cells[1, 10].Value = "Zona";
            ws.Cells[1, 11].Value = "Telephone";
            ws.Cells[1, 12].Value = "Fax";
            ws.Cells[1, 13].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerLoadingAddress.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Alamat;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].LocProvinsi == null ? null : dbitems[i].CustomerLoadingAddress.ToList()[j].LocProvinsi.Code;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].LocKabKota == null ? null : dbitems[i].CustomerLoadingAddress.ToList()[j].LocKabKota.Code;
                    ws.Cells[idx, 5].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].LocKecamatan == null ? null : dbitems[i].CustomerLoadingAddress.ToList()[j].LocKecamatan.Code;
                    ws.Cells[idx, 6].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].LocKelurahan == null ? null : dbitems[i].CustomerLoadingAddress.ToList()[j].LocKelurahan.Code;
                    ws.Cells[idx, 7].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Longitude;
                    ws.Cells[idx, 8].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Latitude;
                    ws.Cells[idx, 9].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Radius;
                    ws.Cells[idx, 10].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Zona;
                    ws.Cells[idx, 11].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Telp;
                    ws.Cells[idx, 12].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Fax;
                    ws.Cells[idx, 13].Value = dbitems[i].CustomerLoadingAddress.ToList()[j].Id;


                    idx++;
                }
            }
            //var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //fsr.FileDownloadName = "Customer_Detail.xls";

            //return fsr;
        }

        public void ExportUnloadingAddress(ExcelPackage pck)
        {
            //bikin file baru
            //ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("UnloadingAddress");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Alamat";
            ws.Cells[1, 3].Value = "Provinsi";
            ws.Cells[1, 4].Value = "Kabupatan/Kota";
            ws.Cells[1, 5].Value = "Kecamatan";
            ws.Cells[1, 6].Value = "Kelurahan/Desa";
            ws.Cells[1, 7].Value = "Longitude";
            ws.Cells[1, 8].Value = "Latitude";
            ws.Cells[1, 9].Value = "Radius";
            ws.Cells[1, 10].Value = "Zona";
            ws.Cells[1, 11].Value = "Telephone";
            ws.Cells[1, 12].Value = "Fax";
            ws.Cells[1, 13].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerUnloadingAddress.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Alamat;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].LocProvinsi == null ? null : dbitems[i].CustomerUnloadingAddress.ToList()[j].LocProvinsi.Code;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].LocKabKota == null ? null : dbitems[i].CustomerUnloadingAddress.ToList()[j].LocKabKota.Code;
                    ws.Cells[idx, 5].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].LocKecamatan == null ? null : dbitems[i].CustomerUnloadingAddress.ToList()[j].LocKecamatan.Code;
                    ws.Cells[idx, 6].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].LocKelurahan == null ? null : dbitems[i].CustomerUnloadingAddress.ToList()[j].LocKelurahan.Code;
                    ws.Cells[idx, 7].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Longitude;
                    ws.Cells[idx, 8].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Latitude;
                    ws.Cells[idx, 9].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Radius;
                    ws.Cells[idx, 10].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Zona;
                    ws.Cells[idx, 11].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Telp;
                    ws.Cells[idx, 12].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Fax;
                    ws.Cells[idx, 13].Value = dbitems[i].CustomerUnloadingAddress.ToList()[j].Id;


                    idx++;
                }
            }
            //var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //fsr.FileDownloadName = "Customer_Detail.xls";

            //return fsr;
        }

        public void ExportSupplier(ExcelPackage pck)
        {
            //bikin file baru
            //ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Supplier");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Alamat";
            ws.Cells[1, 3].Value = "Provinsi";
            ws.Cells[1, 4].Value = "Kabupatan/Kota";
            ws.Cells[1, 5].Value = "Kecamatan";
            ws.Cells[1, 6].Value = "Kelurahan/Desa";
            ws.Cells[1, 7].Value = "Longitude";
            ws.Cells[1, 8].Value = "Latitude";
            ws.Cells[1, 9].Value = "Radius";
            ws.Cells[1, 10].Value = "Zona";
            ws.Cells[1, 11].Value = "PIC";
            ws.Cells[1, 12].Value = "Telephone";
            ws.Cells[1, 13].Value = "Fax";
            ws.Cells[1, 14].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerSupplier.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerSupplier.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerSupplier.ToList()[j].Alamat;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerSupplier.ToList()[j].LocProvinsi == null ? null : dbitems[i].CustomerSupplier.ToList()[j].LocProvinsi.Code;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerSupplier.ToList()[j].LocKabKota == null ? null : dbitems[i].CustomerSupplier.ToList()[j].LocKabKota.Code;
                    ws.Cells[idx, 5].Value = dbitems[i].CustomerSupplier.ToList()[j].LocKecamatan == null ? null : dbitems[i].CustomerSupplier.ToList()[j].LocKecamatan.Code;
                    ws.Cells[idx, 6].Value = dbitems[i].CustomerSupplier.ToList()[j].LocKelurahan == null ? null : dbitems[i].CustomerSupplier.ToList()[j].LocKelurahan.Code;
                    ws.Cells[idx, 7].Value = dbitems[i].CustomerSupplier.ToList()[j].Longitude;
                    ws.Cells[idx, 8].Value = dbitems[i].CustomerSupplier.ToList()[j].Latitude;
                    ws.Cells[idx, 9].Value = dbitems[i].CustomerSupplier.ToList()[j].Radius;
                    ws.Cells[idx, 10].Value = dbitems[i].CustomerSupplier.ToList()[j].Zona;
                    ws.Cells[idx, 11].Value = dbitems[i].CustomerSupplier.ToList()[j].Pic;
                    ws.Cells[idx, 12].Value = dbitems[i].CustomerSupplier.ToList()[j].Telp;
                    ws.Cells[idx, 13].Value = dbitems[i].CustomerSupplier.ToList()[j].Fax;
                    ws.Cells[idx, 14].Value = dbitems[i].CustomerSupplier.ToList()[j].Id;


                    idx++;
                }
            }
            //var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //fsr.FileDownloadName = "Customer_Detail.xls";

            //return fsr;
        }

        public void ExportProductType(ExcelPackage pck)
        {
            //bikin file baru
            //ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ProductType");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Nama Product";
            ws.Cells[1, 3].Value = "Penanganan Khusus";
            ws.Cells[1, 4].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerProductType.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerProductType.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerProductType.ToList()[j].MasterProduct.NamaProduk;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerProductType.ToList()[j].PenangananKhusus;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerProductType.ToList()[j].Id;

                    idx++;
                }
            }
            //var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //fsr.FileDownloadName = "Customer_Detail.xls";

            //return fsr;
        }

        public void ExportBilling(ExcelPackage pck)
        {
            //bikin file baru
            //ExcelPackage pck = new ExcelPackage();
            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Billing");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Nama Dokumen";
            ws.Cells[1, 3].Value = "Lembar";
            ws.Cells[1, 4].Value = "Warna";
            ws.Cells[1, 5].Value = "Stample";
            ws.Cells[1, 6].Value = "Info Fax";
            ws.Cells[1, 7].Value = "Fax";
            ws.Cells[1, 8].Value = "Info Email";
            ws.Cells[1, 9].Value = "Email";
            ws.Cells[1, 10].Value = "Info Tukar Faktur";
            ws.Cells[1, 11].Value = "Hari Tukar Faktur";
            ws.Cells[1, 12].Value = "Jam";
            ws.Cells[1, 13].Value = "Catatan";
            ws.Cells[1, 14].Value = "PIC";
            ws.Cells[1, 15].Value = "Info Jasa Pengiriman";
            ws.Cells[1, 16].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerBilling.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerBilling.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerBilling.ToList()[j].DocumentName;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerBilling.ToList()[j].Lembar;
                    ws.Cells[idx, 5].Value = dbitems[i].CustomerBilling.ToList()[j].Stempel;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerBilling.ToList()[j].Warna;
                    ws.Cells[idx, 6].Value = dbitems[i].CustomerBilling.ToList()[j].IsFax;
                    ws.Cells[idx, 7].Value = dbitems[i].CustomerBilling.ToList()[j].Fax;
                    ws.Cells[idx, 8].Value = dbitems[i].CustomerBilling.ToList()[j].IsEmail;
                    ws.Cells[idx, 9].Value = dbitems[i].CustomerBilling.ToList()[j].Email;
                    ws.Cells[idx, 10].Value = dbitems[i].CustomerBilling.ToList()[j].IsTukarFaktur;
                    ws.Cells[idx, 15].Value = dbitems[i].CustomerBilling.ToList()[j].IsJasaPengiriman;
                    ws.Cells[idx, 16].Value = dbitems[i].CustomerBilling.ToList()[j].Id;


                    int i1 = idx;
                    foreach (Context.CustomerJadwalBilling item in dbitems[i].CustomerBilling.ToList()[j].CustomerJadwalBilling)
                    {
                        ws.Cells[i1, 11].Value = item.Hari;
                        ws.Cells[i1, 12].Value = item.Jam;
                        ws.Cells[i1, 13].Value = item.Catatan;
                        ws.Cells[i1, 14].Value = item.PIC;
                        i1++;
                    }

                    idx++;
                }
            }
        }

        public void ExportNotification(ExcelPackage pck)
        {

            //sumber data
            List<Context.Customer> dbitems = RepoCustomer.FindAll();
            //bikin worksheet worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ProductType");

            //bikin header cell[baris,kolom] , nama kolom sesuaikan dengan template
            ws.Cells[1, 1].Value = "Customer Code";
            ws.Cells[1, 2].Value = "Status";
            ws.Cells[1, 3].Value = "Kode PIC";
            ws.Cells[1, 4].Value = "Type";
            ws.Cells[1, 5].Value = "Kode Rute";
            ws.Cells[1, 6].Value = "Truk";
            ws.Cells[1, 7].Value = "Id Database";


            // Inserts Data
            int idx = 2;
            for (int i = 0; i < dbitems.Count(); i++)
            {
                for (int j = 0; j < dbitems[i].CustomerNotification.Count(); j++)
                {
                    ws.Cells[idx, 1].Value = dbitems[i].CustomerNotification.ToList()[j].Customer.CustomerCode;
                    ws.Cells[idx, 2].Value = dbitems[i].CustomerNotification.ToList()[j].IsActive;
                    ws.Cells[idx, 3].Value = dbitems[i].CustomerNotification.ToList()[j].IdPic;
                    ws.Cells[idx, 4].Value = dbitems[i].CustomerNotification.ToList()[j].NotifType;
                    ws.Cells[idx, 7].Value = dbitems[i].CustomerNotification.ToList()[j].Id;

                    int i1 = idx;
                    foreach (Context.CustomerNotifRute item in dbitems[i].CustomerNotification.ToList()[j].CustomerNotifRute)
                    {
                        ws.Cells[i1, 5].Value = item.Rute.Kode;
                        i1++;
                    }

                    int i2 = idx;
                    foreach (Context.CustomerNotifTruck item in dbitems[i].CustomerNotification.ToList()[j].CustomerNotifTruck)
                    {
                        ws.Cells[i2, 6].Value = item.JenisTrucks.StrJenisTruck;
                        i1++;
                    }
                    idx++;
                }
            }
        }

        #endregion
        #region option
        public string GetProvinsi()
        {
            return new JavaScriptSerializer().Serialize(RepoLocation.FindAll().Where(d => d.Type == "Provinsi").ToList());
        }
        public string GetChildLoc(int idParent)
        {
            return new JavaScriptSerializer().Serialize(RepoLocation.FindAll().Where(d => d.ParentId == idParent).ToList());
        }
        public string GetProduct()
        {
            List<Context.MasterProduct> dbitems = RepoProduct.FindAll();

            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string GetProductById(int id)
        {
            Context.MasterProduct dbitem = RepoProduct.FindByPK(id);

            return new JavaScriptSerializer().Serialize(dbitem);
        }
        public string GetRekening(string stat)
        {
            List<Context.Rekenings> dbitem = RepoRekening.FindAll().Where(d => d.Type == stat).ToList();

            return new JavaScriptSerializer().Serialize(dbitem);
        }
        public string GetCustRekening(string stat)
        {
            List<Context.Rekenings> dbitem = RepoRekening.FindAll().Where(d => d.Type == stat).ToList();
            List<Rekening> ListModel = new List<Rekening>();
            foreach (Context.Rekenings item in dbitem)
            {
                ListModel.Add(new Rekening(item));
            }

            return new JavaScriptSerializer().Serialize(ListModel);
        }
        public List<Context.JenisTrucks> GetTruck()
        {
            return RepoTruck.FindAll().ToList();
        }
        public string GetDataForSO(int id)
        {
            Context.Customer dbcust = RepoCustomer.FindByPK(id);
            List<CustLoadUnload> dataLoad = new List<CustLoadUnload>();
            foreach (Context.CustomerLoadingAddress item in dbcust.CustomerLoadingAddress.ToList())
            {
                dataLoad.Add(new CustLoadUnload(item));
            }
            List<CustLoadUnload> dataUnload = new List<CustLoadUnload>();
            foreach (Context.CustomerUnloadingAddress item in dbcust.CustomerUnloadingAddress.ToList())
            {
                dataUnload.Add(new CustLoadUnload(item));
            }

            return new JavaScriptSerializer().Serialize(new
            {
                Kode = dbcust.CustomerCode,
                KodeNama = dbcust.CustomerCodeOld,
                Nama = dbcust.CustomerNama,
                StatusKredit = "",
                PenangananKhusus = dbcust.SpecialTreatment,
                dataLoad = dataLoad,
                dataUnload = dataUnload,
            });
        }
        public string getTreatmentProduct(int id, int idCust)
        {
            Context.Customer dbcust = RepoCustomer.FindByPK(idCust);
            Context.CustomerProductType dbpro = dbcust.CustomerProductType.Where(d => d.idProduk == id).FirstOrDefault();
            string treatment = dbpro.PenangananKhusus;
            int suhu = int.Parse(dbpro.MasterProduct.TargetSuhu.ToString());
            return new JavaScriptSerializer().Serialize(new { treatment = treatment, suhu = suhu });
        }
        public JsonResult GetCustomer()
        {
            return Json(RepoCustomer.FindAll().Select(c => new { c.Id, c.CustomerCode, c.CustomerNama}), JsonRequestBehavior.AllowGet);
        }

        public string GetSpecLocation(int id, List<int> idLoad, List<int> idUnload, List<string> ListMultidrop)
        {
            Context.Customer dbcust = RepoCustomer.FindByPK(id);
            List<Context.CustomerLoadingAddress> listLoad = dbcust.CustomerLoadingAddress.ToList();
            List<Context.CustomerLoadingAddress> dummyLoad = new List<Context.CustomerLoadingAddress>();
            List<Context.CustomerUnloadingAddress> listUnload = dbcust.CustomerUnloadingAddress.ToList();
            List<Context.CustomerUnloadingAddress> dummyUnload = new List<Context.CustomerUnloadingAddress>();

            if (idLoad != null)
            {
                foreach (int item in idLoad)
                {
                    if (isJKTS(item))
                    {
                        foreach (Context.CustomerLoadingAddress custLoad in listLoad)
                        {
                            if (custLoad.IdProvinsi.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdProvinsi.Value))
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKabKota.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdKabKota.Value))
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKec.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdKec.Value))
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKel.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdKel.Value))
                                    dummyLoad.Add(custLoad);
                            }
                        }
                    }
                    else
                    {
                        int idProvinsi = getParentIsProvinsi(item);

                        foreach (Context.CustomerLoadingAddress custLoad in listLoad)
                        {
                            if (custLoad.IdProvinsi.HasValue)
                            {
                                if (custLoad.IdProvinsi.Value == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKabKota.HasValue)
                            {
                                if (getParentIsProvinsi(custLoad.IdKabKota.Value) == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKec.HasValue)
                            {
                                if (getParentIsProvinsi(custLoad.IdKec.Value) == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKel.HasValue)
                            {
                                if (getParentIsProvinsi(custLoad.IdKel.Value) == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                        }
                    }
                }            
            }

            if (idLoad != null)
            {
                foreach (int item in idUnload)
                {
                    if (IsJabodetabek(item))
                    {
                        foreach (Context.CustomerUnloadingAddress custUnload in listUnload)
                        {
                            if (custUnload.IdProvinsi.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdProvinsi.Value))
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKabKota.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdKabKota.Value))
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKec.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdKec.Value))
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKel.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdKel.Value))
                                    dummyUnload.Add(custUnload);
                            }
                        }
                    }
                    else
                    {
                        int idProvinsi = getParentIsProvinsi(item);
                        foreach (Context.CustomerUnloadingAddress custUnload in listUnload)
                        {
                            if (custUnload.IdProvinsi.HasValue)
                            {
                                if (custUnload.IdProvinsi.Value == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKabKota.HasValue)
                            {
                                if (getParentIsProvinsi(custUnload.IdKabKota.Value) == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKec.HasValue)
                            {
                                if (getParentIsProvinsi(custUnload.IdKec.Value) == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKel.HasValue)
                            {
                                if (getParentIsProvinsi(custUnload.IdKel.Value) == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else { dummyUnload.Add(custUnload); }
                        }
                    }
                }
            }

            if (ListMultidrop != null)
            {
                List<int> idRuteMulti = new List<int>();
                foreach (var item in ListMultidrop)
                {
                    List<string> naonwelah = item.Split(new[]{" - "} ,StringSplitOptions.None).ToList();
                    foreach (string s in naonwelah)
                    {
                        if (RepoLocation.FindByNama(s) != null)
                        {
                            int idRt = RepoLocation.FindByNama(s).Id;
                            idRuteMulti.Add(idRt);
                        }
                    }
                }
                //load
                foreach (int item in idRuteMulti)
                {
                    if (isJKTS(item))
                    {
                        foreach (Context.CustomerLoadingAddress custLoad in listLoad)
                        {
                            if (custLoad.IdProvinsi.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdProvinsi.Value))
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKabKota.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdKabKota.Value))
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKec.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdKec.Value))
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKel.HasValue)
                            {
                                if (IsJabodetabek(custLoad.IdKel.Value))
                                    dummyLoad.Add(custLoad);
                            }
                        }
                    }
                    else
                    {
                        int idProvinsi = getParentIsProvinsi(item);

                        foreach (Context.CustomerLoadingAddress custLoad in listLoad)
                        {
                            if (custLoad.IdProvinsi.HasValue)
                            {
                                if (custLoad.IdProvinsi.Value == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKabKota.HasValue)
                            {
                                if (getParentIsProvinsi(custLoad.IdKabKota.Value) == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKec.HasValue)
                            {
                                if (getParentIsProvinsi(custLoad.IdKec.Value) == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                            else if (custLoad.IdKel.HasValue)
                            {
                                if (getParentIsProvinsi(custLoad.IdKel.Value) == idProvinsi)
                                    dummyLoad.Add(custLoad);
                            }
                        }
                    }
                }
                //unload
                foreach (int item in idRuteMulti)
                {
                    if (isJKTS(item))
                    {
                        foreach (Context.CustomerUnloadingAddress custUnload in listUnload)
                        {
                            if (custUnload.IdProvinsi.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdProvinsi.Value))
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKabKota.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdKabKota.Value))
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKec.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdKec.Value))
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKel.HasValue)
                            {
                                if (IsJabodetabek(custUnload.IdKel.Value))
                                    dummyUnload.Add(custUnload);
                            }
                        }
                    }
                    else
                    {
                        int idProvinsi = getParentIsProvinsi(item);
                        foreach (Context.CustomerUnloadingAddress custUnload in listUnload)
                        {
                            if (custUnload.IdProvinsi.HasValue)
                            {
                                if (custUnload.IdProvinsi.Value == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKabKota.HasValue)
                            {
                                if (getParentIsProvinsi(custUnload.IdKabKota.Value) == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKec.HasValue)
                            {
                                if (getParentIsProvinsi(custUnload.IdKec.Value) == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else if (custUnload.IdKel.HasValue)
                            {
                                if (getParentIsProvinsi(custUnload.IdKel.Value) == idProvinsi)
                                    dummyUnload.Add(custUnload);
                            }
                            else { dummyUnload.Add(custUnload); }
                        }
                    }
                }
            }

            List<CustLoadUnload> dataLoad = new List<CustLoadUnload>();
            foreach (Context.CustomerLoadingAddress item in dummyLoad.Distinct())
            {
                dataLoad.Add(new CustLoadUnload(item));
            }

            List<CustLoadUnload> dataUnload = new List<CustLoadUnload>();
            foreach (Context.CustomerUnloadingAddress item in dummyUnload.Distinct())
            {
                dataUnload.Add(new CustLoadUnload(item));
            }

            return new JavaScriptSerializer().Serialize(new { dataLoad = dataLoad, dataUnload = dataUnload });
        }

        private int getParentIsProvinsi(int id)
        {
            Context.Location dbLocation = RepoLocation.FindByPK(id);
            if (dbLocation.Type != "Provinsi")
            {
                if (dbLocation.ParentId.HasValue)
                    return getParentIsProvinsi(dbLocation.ParentId.Value);
                else
                    return 0;
            }
            else
            {
                return dbLocation.Id;
            }
        }

        private bool IsJabodetabek(int id)
        {
            Context.Location dbLocation = RepoLocation.FindByPK(id);
            if (dbLocation.Type == "Provinsi")
            {
                string prov = dbLocation.Nama.ToLower();
                if (prov.Contains("jakarta") || prov.Contains("jawa barat") || prov.Contains("banten"))
                    return true;
            }
            else if (dbLocation.Type == "Kab/Kota")
            { 
                string prov = dbLocation.LocationParent.Nama.ToLower();
                if (prov.Contains("jakarta") || prov.Contains("jawa barat") || prov.Contains("banten"))
                    return true;
            }
            else if (dbLocation.Type == "Kecamatan")
            {
                string prov = dbLocation.LocationParent.LocationParent.Nama.ToLower();
                if (prov.Contains("jakarta") || prov.Contains("jawa barat") || prov.Contains("banten"))
                    return true;
            }
            else if (dbLocation.Type == "Kelurahan")
            {
                string prov = dbLocation.LocationParent.LocationParent.LocationParent.Nama.ToLower();
                if (prov.Contains("jakarta") || prov.Contains("jawa barat") || prov.Contains("banten"))
                    return true;
            }

            return false;
        }

        private bool isJKTS(int id)
        {
            Context.Location dbrute = RepoLocation.FindByPK(id);
            if (dbrute.Nama.ToLower().Contains("jakarta"))
                return true;
            else
                return false;
        }
        #endregion
    }
}