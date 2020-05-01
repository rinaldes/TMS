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
    public class RevisiKeteranganController : BaseController
    {
        private ISalesOrderRepo RepoSalesOrder;
        private IRevisiTanggalRepo RepoRevisiTanggal;
        private ISettlementBatalRepo RepoSettBatal;
        private IBatalOrderRepo RepoBatalOrder;
        private IAuditrailRepo RepoAuditrail;

        public RevisiKeteranganController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, IRevisiTanggalRepo repoRevisiTanggal, ISettlementBatalRepo repoSettBatal, IBatalOrderRepo repoBatalOrder, IAuditrailRepo repoAuditrail)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoRevisiTanggal = repoRevisiTanggal;
            RepoSettBatal = repoSettBatal;
            RepoBatalOrder = repoBatalOrder;
            RepoAuditrail = repoAuditrail;
        }

        public ActionResult Edit(int idSo)
        {
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(idSo);

            RevisiKeterangan model = new RevisiKeterangan(dbso);

            return View("Form", model);
        }
        [HttpPost]
        public ActionResult Edit(RevisiKeterangan model)
        {
            var query = "";
            Context.SalesOrder dbso = RepoSalesOrder.FindByPK(model.IdSalesOrder.Value);
            if (ModelState.IsValid)
            {
                if (dbso.SalesOrderOncallId.HasValue)
                {
                    dbso.SalesOrderOncall.Keterangan = model.KeteranganRevisi;
                }
                else if (dbso.SalesOrderPickupId.HasValue)
                {
                    dbso.SalesOrderPickup.Keterangan = model.KeteranganRevisi;
                    query += "UPDATE dbo.\"SalesOrderPickup\" SET \"Keterangan\" = " + model.KeteranganRevisi + " WHERE \"SalesOrderPickupId\" = " + dbso.SalesOrderPickupId + ";";
                }
                else if (dbso.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    dbso.SalesOrderProsesKonsolidasi.Keterangan = model.KeteranganRevisi;
                    query += "UPDATE dbo.\"SalesOrderProsesKonsolidasi\" SET \"Keterangan\" = " + model.KeteranganRevisi + " WHERE \"SalesOrderProsesKonsolidasiId\" = " + dbso.SalesOrderProsesKonsolidasiId + ";";
                }

                RepoSalesOrder.save(dbso);
                RepoAuditrail.SetAuditTrail(query, "List Order", "Revisi Keterangan", UserPrincipal.id);

                return RedirectToAction("Index", "ListOrder");
            }

            return View("Form", model);
        }
    }
}