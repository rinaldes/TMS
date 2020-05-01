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
    public class RemovalController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IRemovalRepo RepoRemoval;
        private ISettlementBatalRepo RepoSettBatal;
        private IBatalOrderRepo RepoBatalOrder;
        private IAuditrailRepo RepoAuditrail;
        public RemovalController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IRemovalRepo repoRemoval, ISettlementBatalRepo repoSettBatal,
            IBatalOrderRepo repoBatalOrder, IAuditrailRepo repoAuditrail)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoRemoval = repoRemoval;
            RepoSettBatal = repoSettBatal;
            RepoBatalOrder = repoBatalOrder;
            RepoAuditrail = repoAuditrail;
        }

        [MyAuthorize(Menu = "Removal", Action="create")]
        public ActionResult Edit(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);

            Removal model = new Removal(dbso);

            if(dbso.SalesOrderOncallId.HasValue)
                return View("FormOncall", model);
            else if (dbso.SalesOrderPickupId.HasValue)
                return View("FormPickup", model);
            else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                return View("FormKonsolidasi", model);

            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(Removal model)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSo.Value);
            SalesOrderLoadUnload[] resultUnload = new SalesOrderLoadUnload[0];
            if (model.StrUnload != null && model.StrUnload != "") {
                resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(model.StrUnload);
                model.ListUnload = resultUnload.ToList();            
            }

            if (ModelState.IsValid)
            {
                //rubah data so na
                if (dbso.SalesOrderOncallId.HasValue)
                {
                    dbso.SalesOrderOncall.IdDaftarHargaItem = model.IdRute;
                    dbso.SalesOrderOncall.StrDaftarHargaItem = model.StrRute;
                    dbso.SalesOrderOncall.SalesOrderOnCallUnLoadingAdd.Clear();
                    foreach (var item in resultUnload)
                    {
                        dbso.SalesOrderOncall.SalesOrderOnCallUnLoadingAdd.Add(new Context.SalesOrderOnCallUnLoadingAdd()
                        {
                            CustomerId = dbso.SalesOrderOncall.CustomerId,
                            CustomerUnloadingAddressId = item.Id,
                            urutan = item.urutan,
                            IsSelect = item.IsSelect
                        });    
                    }
                }
                else if (dbso.SalesOrderPickupId.HasValue)
                {
                    dbso.SalesOrderPickup.RuteId = model.IdRute;
                    dbso.SalesOrderPickup.SalesOrderPickupUnLoadingAdd.Clear();
                    RepoAuditrail.saveDelAllSalesOrderPickupUnLoadingAddQuery(dbso.SalesOrderPickup, UserPrincipal.id);
                    foreach (var item in resultUnload)
                    {
                        dbso.SalesOrderOncall.SalesOrderOnCallUnLoadingAdd.Add(new Context.SalesOrderOnCallUnLoadingAdd()
                        {
                            CustomerId = dbso.SalesOrderOncall.CustomerId,
                            CustomerUnloadingAddressId = item.Id,
                            urutan = item.urutan,
                            IsSelect = item.IsSelect
                        });    
                    }
                }
                else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    dbso.SalesOrderProsesKonsolidasi.IdDaftarHargaItem = model.IdRute;
                    dbso.SalesOrderProsesKonsolidasi.StrDaftarHargaItem = model.StrRute;
                }

                dbso.AdminUangJalan.Removal.Add(new Context.Removal()
                {
                    StatusTagihan = model.StatusTagihan,
                    TanggalRemoval = model.TanggalRemoval,
                    JamRemoval = model.JamRemoval,
                    KeteranganRemoval = model.Keterangan,
                    IdDriver1 = dbso.AdminUangJalan.IdDriver1,
                    IdDriver2 = dbso.AdminUangJalan.IdDriver2,
                    IdSO = dbso.Id
                });
                RepoSalesOrder.save(dbso);
                RepoAuditrail.saveUpdSalesOrderProsesKonsolidasiQuery(dbso.SalesOrderProsesKonsolidasi, UserPrincipal.id);
                return RedirectToAction("Index", "ListOrder");
            }

            if (dbso.SalesOrderOncallId.HasValue){
                model.ModelOncall = new SalesOrderOncall(dbso);
                return View("FormOncall", model);
            }
            else if (dbso.SalesOrderPickupId.HasValue){
                model.ModelPickup = new SalesOrderPickup(dbso);
                return View("FormPickup", model);
            }
            else if (dbso.SalesOrderProsesKonsolidasiId.HasValue){
                model.ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbso);
                return View("FormKonsolidasi", model);
            }

            return View("");
        }
    }

}