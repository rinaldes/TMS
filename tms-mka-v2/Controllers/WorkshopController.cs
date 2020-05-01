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
    public class WorkshopController : BaseController
    {
        private IWorkshopRepo RepoWorkshop;
        private ISpkRepo RepoSpk;
        private IMekanikRepo RepoMecha;
        private IAuditrailRepo RepoAuditrail;
        private IMonitoringVehicleRepo RepoMonitoringVehicle;
        private IDataTruckRepo RepoDataTruck;
        public WorkshopController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IWorkshopRepo repoWorkshop, ISpkRepo repoSpk, IMekanikRepo repoMecha,
            IAuditrailRepo repoAuditrail, IMonitoringVehicleRepo repoMonitoringVehicle, IDataTruckRepo repoDataTruck)
            : base(repoBase, repoLookup)
        {
            RepoWorkshop = repoWorkshop;
            RepoSpk = repoSpk;
            RepoMecha = repoMecha;
            RepoAuditrail = repoAuditrail;
            RepoMonitoringVehicle = repoMonitoringVehicle;
            RepoDataTruck = repoDataTruck;
        }
        [MyAuthorize(Menu = "Perbaikan Kendaraan", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "Workshop").ToList();
            return View();
        }
        public string GetMecha()
        {
            List<Context.Mekanik> dbitems = new List<Context.Mekanik>();
            dbitems = RepoMecha.FindAll().ToList();
            return new JavaScriptSerializer().Serialize(dbitems);
        }
        public string FindById(int id)
        {
            Context.Workshop item = RepoWorkshop.FindByPK(id);
            Workshop data = new Workshop(item);
            return new JavaScriptSerializer().Serialize(data);
        }

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Spk> items = RepoSpk.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<Workshop> ListModel = new List<Workshop>();
            foreach (Context.Spk spk in items)
            {
                if (spk.NoSpk == null && spk.ServiceIn != null && spk.TglSPK == null)
                {
                    spk.TglSPK = spk.ServiceIn;
                    spk.Urutan = RepoSpk.getUrutan(spk.ServiceIn.Value);
                    spk.NoSpk = RepoSpk.generateCode(spk.ServiceIn.Value, RepoSpk.getUrutan(spk.ServiceIn.Value) == null ? 1 : RepoSpk.getUrutan(spk.ServiceIn.Value).Value);
                    RepoSpk.save(spk, UserPrincipal.id);
                }
                Context.Workshop item = spk.Workshop;
                if (UserPrincipal.HasMenuAccess("Pre-PPK") && item.Status == "Pre-PPK" || item.Status == "PPK" || UserPrincipal.HasMenuAccess("PPK-In") && item.Status == "PPK-IN" ||
                    UserPrincipal.HasMenuAccess("SPK") && (item.Status == "SPK" || item.Status == "SPK-O" || item.Status == "SPK-P" || item.Status == "SPK-C" || item.Status == "SPK-W"))
                {
                    if (spk.Jenis == "Truck")
                        ListModel.Add(new Workshop(item, "Truck"));
                    if (spk.Jenis == "AC")
                        ListModel.Add(new Workshop(item, "AC"));
                    if (spk.Jenis == "Ban")
                        ListModel.Add(new Workshop(item, "Ban"));
                    if (spk.Jenis == "Box")
                        ListModel.Add(new Workshop(item, "Box"));
                    if (spk.Jenis == "GPS")
                        ListModel.Add(new Workshop(item, "GPS"));
                }
            }

            int total = RepoSpk.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string gNoSpk(string StartTime, string EndTime)
        {
            DateTime start = DateTime.Parse(StartTime + "00:00:00");
            DateTime end = DateTime.Parse(EndTime + "00:00:00");
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Spk> items = RepoSpk.FindAll().Where(d => d.Workshop.TglPre >= start && d.Workshop.TglPre <= end && d.Workshop.Status.Contains("SPK")).ToList();
            List<Workshop> ListModel = new List<Workshop>();
            int i = 1;
            foreach (Context.Spk spk in items)
            {
                spk.TglSPK = spk.Workshop.TglPre;
                spk.Urutan = i;
                spk.NoSpk = RepoSpk.generateCode(spk.Workshop.TglPre, i);
                RepoSpk.save(spk, UserPrincipal.id);
                i++;
            }
            int total = items.Count();
            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }

        public string BindingKet(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.Spk> items = RepoSpk.FindByWorkshop(id);
            List<Spk> ListModel = new List<Spk>();
            foreach (Context.Spk item in items)
            {
                ListModel.Add(new Spk(item));
            }

            int total = RepoSpk.Count(param.Filters);
            return new JavaScriptSerializer().Serialize(new { total = items.Count(), data = ListModel });
        }
        public string BindingKet2(int id)
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.SpkHistory> items = RepoSpk.FindByWorkshopForHistory(id);
            List<SpkHistory> ListModel = new List<SpkHistory>();
            foreach (Context.SpkHistory item in items)
            {
                ListModel.Add(new SpkHistory(item));
            }

            int total = RepoSpk.Count(param.Filters);
            return new JavaScriptSerializer().Serialize(new { total = items.Count(), data = ListModel });
        }

        [MyAuthorize(Menu = "Perbaikan Kendaraan", Action="create")]
        public ActionResult Add()
        {
            Workshop model = new Workshop();
            model.Urutan = RepoWorkshop.getUrutan() + 1;
            model.Status = "PrePPK";
            model.TglPre = DateTime.Now;
            model.TglPPK = DateTime.Now;
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Add(Workshop model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsAc == true)
                {
                    Context.Workshop dbitem = new Context.Workshop();
                    model.setDb(dbitem);
                    dbitem.IsBan = false;
                    dbitem.IsBox = false;
                    dbitem.IsGps = false;
                    dbitem.IsTruck = false;
                    if (model.IsPool != true)
                    {
                        dbitem.NoPrePPK = RepoWorkshop.generateCodePrePPK(RepoWorkshop.getUrutan());
                        dbitem.Urutan = RepoWorkshop.getUrutan();
                    }
                    string noPPK = RepoWorkshop.generateCodePPK(dbitem.TglPre, RepoWorkshop.getUrutanPPK(dbitem.TglPre));
                    dbitem.TglPPK = dbitem.TglPre;
                    dbitem.NoPPK = noPPK;
                    dbitem.UrutanPPK = RepoWorkshop.getUrutanPPK(dbitem.TglPre);
                    dbitem.Status = model.IsPool == true && model.Status != "PPK-IN" ? "PPK" : "Pre-PPK";

                    RepoWorkshop.save(dbitem, UserPrincipal.id);
                    Context.Workshop dbitemAfterSave = RepoWorkshop.FindByNoPPK(noPPK);
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitemAfterSave.Id, "AC");
                    if (spk_dbitem == null)
                    {
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "AC";
                        model2.Permintaan = model.KetAc;
                        model2.Keterangan = model.KetKerjaAc;
                        model2.Workshop_id = dbitemAfterSave.Id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else
                    {
                        spk_dbitem.Permintaan = model.KetAc;
                        spk_dbitem.Keterangan = model.KetKerjaAc;
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsBan == true)
                {
                    Context.Workshop dbitem = new Context.Workshop();
                    model.setDb(dbitem);
                    dbitem.IsAc = false;
                    dbitem.IsBox = false;
                    dbitem.IsGps = false;
                    dbitem.IsTruck = false;
                    if (model.IsPool != true)
                    {
                        dbitem.NoPrePPK = RepoWorkshop.generateCodePrePPK(RepoWorkshop.getUrutan());
                        dbitem.Urutan = RepoWorkshop.getUrutan();
                    }
                    string noPPK = RepoWorkshop.generateCodePPK(dbitem.TglPre, RepoWorkshop.getUrutanPPK(dbitem.TglPre));
                    dbitem.TglPPK = dbitem.TglPre;
                    dbitem.NoPPK = noPPK;
                    dbitem.UrutanPPK = RepoWorkshop.getUrutanPPK(dbitem.TglPre);
                    dbitem.Status = model.IsPool == true && model.Status != "PPK-IN" ? "PPK" : "Pre-PPK";

                    RepoWorkshop.save(dbitem, UserPrincipal.id);
                    Context.Workshop dbitemAfterSave = RepoWorkshop.FindByNoPPK(noPPK);
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitemAfterSave.Id, "Ban");
                    if (spk_dbitem == null)
                    {
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "Ban";
                        model2.Permintaan = model.KetBan;
                        model2.Keterangan = model.KetKerjaBan;
                        model2.Workshop_id = dbitemAfterSave.Id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else
                    {
                        spk_dbitem.Permintaan = model.KetBan;
                        spk_dbitem.Keterangan = model.KetKerjaBan;
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsBox == true)
                {
                    Context.Workshop dbitem = new Context.Workshop();
                    model.setDb(dbitem);
                    dbitem.IsBan = false;
                    dbitem.IsAc = false;
                    dbitem.IsGps = false;
                    dbitem.IsTruck = false;
                    if (model.IsPool != true)
                    {
                        dbitem.NoPrePPK = RepoWorkshop.generateCodePrePPK(RepoWorkshop.getUrutan());
                        dbitem.Urutan = RepoWorkshop.getUrutan();
                    }
                    string noPPK = RepoWorkshop.generateCodePPK(dbitem.TglPre, RepoWorkshop.getUrutanPPK(dbitem.TglPre));
                    dbitem.TglPPK = dbitem.TglPre;
                    dbitem.NoPPK = noPPK;
                    dbitem.UrutanPPK = RepoWorkshop.getUrutanPPK(dbitem.TglPre);
                    dbitem.Status = model.IsPool == true && model.Status != "PPK-IN" ? "PPK" : "Pre-PPK";

                    RepoWorkshop.save(dbitem, UserPrincipal.id);
                    Context.Workshop dbitemAfterSave = RepoWorkshop.FindByNoPPK(noPPK);
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitemAfterSave.Id, "Box");
                    if (spk_dbitem == null)
                    {
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "Box";
                        model2.Permintaan = model.KetBox;
                        model2.Keterangan = model.KetKerjaBox;
                        model2.Workshop_id = dbitemAfterSave.Id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else
                    {
                        spk_dbitem.Permintaan = model.KetBox;
                        spk_dbitem.Keterangan = model.KetKerjaBox;
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsGps == true)
                {
                    Context.Workshop dbitem = new Context.Workshop();
                    model.setDb(dbitem);
                    dbitem.IsBan = false;
                    dbitem.IsBox = false;
                    dbitem.IsAc = false;
                    dbitem.IsTruck = false;
                    if (model.IsPool != true)
                    {
                        dbitem.NoPrePPK = RepoWorkshop.generateCodePrePPK(RepoWorkshop.getUrutan());
                        dbitem.Urutan = RepoWorkshop.getUrutan();
                    }
                    string noPPK = RepoWorkshop.generateCodePPK(dbitem.TglPre, RepoWorkshop.getUrutanPPK(dbitem.TglPre));
                    dbitem.TglPPK = dbitem.TglPre;
                    dbitem.NoPPK = noPPK;
                    dbitem.UrutanPPK = RepoWorkshop.getUrutanPPK(dbitem.TglPre);
                    dbitem.Status = model.IsPool == true && model.Status != "PPK-IN" ? "PPK" : "Pre-PPK";

                    RepoWorkshop.save(dbitem, UserPrincipal.id);
                    Context.Workshop dbitemAfterSave = RepoWorkshop.FindByNoPPK(noPPK);
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitemAfterSave.Id, "GPS");
                    if (spk_dbitem == null)
                    {
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "GPS";
                        model2.Permintaan = model.KetGps;
                        model2.Keterangan = model.KetKerjaGps;
                        model2.Workshop_id = dbitemAfterSave.Id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else
                    {
                        spk_dbitem.Permintaan = model.KetGps;
                        spk_dbitem.Keterangan = model.KetKerjaGps;
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsTruck == true)
                {
                    Context.Workshop dbitem = new Context.Workshop();
                    model.setDb(dbitem);
                    dbitem.IsBan = false;
                    dbitem.IsBox = false;
                    dbitem.IsGps = false;
                    dbitem.IsAc = false;
                    if (model.IsPool != true)
                    {
                        dbitem.NoPrePPK = RepoWorkshop.generateCodePrePPK(RepoWorkshop.getUrutan());
                        dbitem.Urutan = RepoWorkshop.getUrutan();
                    }
                    string noPPK = RepoWorkshop.generateCodePPK(dbitem.TglPre, RepoWorkshop.getUrutanPPK(dbitem.TglPre));
                    dbitem.TglPPK = dbitem.TglPre;
                    dbitem.NoPPK = noPPK;
                    dbitem.UrutanPPK = RepoWorkshop.getUrutanPPK(dbitem.TglPre);
                    dbitem.Status = model.IsPool == true && model.Status != "PPK-IN" ? "PPK" : "Pre-PPK";

                    RepoWorkshop.save(dbitem, UserPrincipal.id);
                    Context.Workshop dbitemAfterSave = RepoWorkshop.FindByNoPPK(noPPK);
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitemAfterSave.Id, "Truck");
                    if (spk_dbitem == null)
                    {
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "Truck";
                        model2.Permintaan = model.KetTruck;
                        model2.Keterangan = model.KetKerjaTruck;
                        model2.Workshop_id = dbitemAfterSave.Id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else
                    {
                        spk_dbitem.Permintaan = model.KetTruck;
                        spk_dbitem.Keterangan = model.KetKerjaTruck;
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public ActionResult UpdateSpk(Spk model)
        {
            int w_id = 0;
            //if (ModelState.IsValid)
            //{
                w_id = model.Workshop_id ?? default(int);
                Context.Spk dbitem = new Context.Spk();
                model.setDb(dbitem);

                RepoSpk.save(dbitem, UserPrincipal.id);
            //}
            Context.Workshop dbitem2 = RepoWorkshop.FindByPK(w_id);
            Workshop model2 = new Workshop(dbitem2);
            RepoWorkshop.save(dbitem2, UserPrincipal.id);
            return RedirectToAction("Spk", new { Id = w_id, idnya = w_id });
        }
        public ActionResult Spk(int id)
        {
            Context.Workshop dbitem2 = RepoWorkshop.FindByPK(id);
            Context.DataTruck truck = RepoDataTruck.FindByPK(dbitem2.IdVehicle);
            Context.Spk dbitem = new Context.Spk();
            dbitem.Workshop_id = dbitem2.Id;
            dbitem.Workshop = dbitem2;
            Workshop model2 = new Workshop(dbitem2);
            Spk model = new Spk(dbitem);
            model.Workshop = model2;
            ViewBag.name = model.Workshop.NoPPK;
            ViewBag.MonVech = RepoMonitoringVehicle.FindByVehicleNo(truck.VehicleNo);
            return View("Spk", model);
        }

        [HttpPost]
        public ActionResult Spk(Workshop model)
        {
            if (ModelState.IsValid)
            {
                Context.Workshop dbitem = RepoWorkshop.FindByPK(model.id);
                model.setDbSpk(dbitem);
                RepoWorkshop.save(dbitem, UserPrincipal.id);
            }
            return RedirectToAction("Index");
        }

        [MyAuthorize(Menu = "Perbaikan Kendaraan", Action="update")]
        public ActionResult PpkIn(int id)
        {
            Context.Workshop dbitem = RepoWorkshop.FindByPK(id);
            Workshop model = new Workshop(dbitem);
            Context.DataTruck truck = RepoDataTruck.FindByPK(dbitem.IdVehicle);
            ViewBag.MonVech = RepoMonitoringVehicle.FindByVehicleNo(truck.VehicleNo);

            ViewBag.name = model.NoPPK;
            return View("PpkIn", model);
        }
        [HttpPost]
        public ActionResult PpkIn(Workshop model)
        {
            if (ModelState.IsValid)
            {
                Context.Workshop dbitem = RepoWorkshop.FindByPK(model.id);
                model.setDbPpkIn(dbitem);
                RepoWorkshop.save(dbitem, UserPrincipal.id);
                dbitem.Spk.Clear();
                RepoAuditrail.saveDelAllSpkQuery(dbitem, UserPrincipal.id);
                if (model.IsAc == true)
                {
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitem.Id, "AC");
                    if (spk_dbitem == null){
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "AC";
                        model2.Permintaan = model.KetAc;
                        model2.Keterangan = model.KetKerjaAc;
                        model2.Workshop_id = model.id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else{
                        spk_dbitem.Permintaan = model.KetAc;
                        spk_dbitem.Keterangan = model.KetKerjaAc;
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        spk_dbitem.ServiceIn = DateTime.Now;
                        spk_dbitem.JamServiceIn = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsBan == true)
                {
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitem.Id, "Ban");
                    if (spk_dbitem == null){
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "Ban";
                        model2.Permintaan = model.KetBan;
                        model2.Keterangan = model.KetKerjaBan;
                        model2.Workshop_id = model.id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else{
                        spk_dbitem.Permintaan = model.KetBan;
                        spk_dbitem.Keterangan = model.KetKerjaBan;
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        spk_dbitem.ServiceIn = DateTime.Now;
                        spk_dbitem.JamServiceIn = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsBox == true)
                {
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitem.Id, "Box");
                    if (spk_dbitem == null){
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "Box";
                        model2.Permintaan = model.KetBox;
                        model2.Keterangan = model.KetKerjaBox;
                        model2.Workshop_id = model.id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else{
                        spk_dbitem.Permintaan = model.KetBox;
                        spk_dbitem.Keterangan = model.KetKerjaBox;
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        spk_dbitem.ServiceIn = DateTime.Now;
                        spk_dbitem.JamServiceIn = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsGps == true)
                {
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitem.Id, "GPS");
                    if (spk_dbitem == null){
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "GPS";
                        model2.Permintaan = model.KetGps;
                        model2.Keterangan = model.KetKerjaGps;
                        model2.Workshop_id = model.id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else{
                        spk_dbitem.Permintaan = model.KetGps;
                        spk_dbitem.Keterangan = model.KetKerjaGps;
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        spk_dbitem.ServiceIn = DateTime.Now;
                        spk_dbitem.JamServiceIn = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                if (model.IsTruck == true)
                {
                    Context.Spk spk_dbitem = RepoSpk.FindByWorkshopAndType(dbitem.Id, "Truck");
                    if (spk_dbitem == null){
                        spk_dbitem = new Context.Spk();
                        spk_dbitem.Id = spk_dbitem.Id;
                        Spk model2 = new Spk();
                        model2.Jenis = "Truck";
                        model2.Permintaan = model.KetTruck;
                        model2.Keterangan = model.KetKerjaTruck;
                        model2.Workshop_id = model.id;
                        model2.RevEstimasi = 0;
                        model2.setDb(spk_dbitem);
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                    else{
                        spk_dbitem.Permintaan = model.KetTruck;
                        spk_dbitem.Keterangan = model.KetKerjaTruck;
                        spk_dbitem.TglSPK = DateTime.Now;
                        spk_dbitem.Urutan = RepoSpk.getUrutan(spk_dbitem.TglSPK.Value.AddHours(7));
                        spk_dbitem.NoSpk = RepoSpk.generateCode(spk_dbitem.TglSPK.Value.AddHours(7), spk_dbitem.Urutan.Value);
                        spk_dbitem.ServiceIn = DateTime.Now;
                        spk_dbitem.JamServiceIn = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        RepoSpk.save(spk_dbitem, UserPrincipal.id);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("PpkIn", model);
        }
        [MyAuthorize(Menu = "Perbaikan Kendaraan", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.Workshop dbitem = RepoWorkshop.FindByPK(id);
            Workshop model = new Workshop(dbitem);

            ViewBag.name = model.NoPPK;
            if(dbitem.Status == "PPK-IN")
                return View("PpkIn", model);
            else if(dbitem.Status == "SPK")
                return View("SPK", model);
            else
                return View("Form", model);
        }   
        [HttpPost]
        public ActionResult Edit(Workshop model, string btnsave)
        {
            if (ModelState.IsValid)
            {
                Context.Workshop dbitem = RepoWorkshop.FindByPK(model.id);
                model.setDb(dbitem);

                if (model.Status != "PPK-IN" && model.Status != "SPK")
                    dbitem.Status = model.IsPool == true ? "PPK" : "Pre-PPK";
                RepoWorkshop.save(dbitem, UserPrincipal.id);
                return RedirectToAction("Index");
            }
            return View("Form", model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Workshop dbItem = RepoWorkshop.FindByPK(id);

            RepoWorkshop.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }
        [HttpPost]
        public JsonResult Submit(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.Workshop dbItem = RepoWorkshop.FindByPK(id);
            dbItem.Status = "save";
            RepoWorkshop.save(dbItem, UserPrincipal.id);

            return Json(response);
        }
    }
}