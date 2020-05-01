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

/*
Berhubung Driver & Customer sama2 Sync ke ptnr_mstr, jadi utk kolom ptnr_idnya memang ambil dari Id masing2 tabel, tapi:
Driver:
 - ptnr_id = Id + 7000000
 - Tambah kolom oid supaya benar2 terhubung

Customer: +0
*/

namespace tms_mka_v2.Controllers
{
    public class SyncController : BaseController 
    {
        private IDriverRepo RepoDriver;
        private IUserRepo RepoUser;
        private Iptnr_mstrRepo Repoptnr_mstr;
        private ICustomerRepo RepoCustomer;
        private IDataTruckRepo RepoDataTruck;
        private Icode_mstrRepo Repocode_mstr;
        private ISalesOrderRepo RepoSalesOrder;

        public SyncController(IUserReferenceRepo repoBase, IDriverRepo repoDriver, IUserRepo repoUser, Iptnr_mstrRepo repoptnr_mstr, ILookupCodeRepo repoLookup, ICustomerRepo repoCustomer, IDataTruckRepo repoDataTruck,
            Icode_mstrRepo repocode_mstr, ISalesOrderRepo repoSalesOrder)
            : base(repoBase, repoLookup)
        {   
            RepoDriver = repoDriver;
            RepoUser = repoUser;
            Repoptnr_mstr = repoptnr_mstr;
            RepoCustomer = repoCustomer;
            RepoDataTruck = repoDataTruck;
            Repocode_mstr = repocode_mstr;
            RepoSalesOrder = repoSalesOrder;
        }

        public ActionResult sync_driver()
        {
            foreach (Context.Driver dbitem in RepoDriver.FindAll())
            {
                if (Repoptnr_mstr.FindByPK(dbitem.Id+7000000) == null)
                {
                    Repoptnr_mstr.saveDriver(dbitem);
                }
                else
                {
                    Context.ptnr_mstr dbptnr = Repoptnr_mstr.FindByPK(dbitem.Id + 7000000);
                    dbptnr.ptnr_name = dbitem.NamaDriver + " - " + dbitem.KodeDriver;
                    Repoptnr_mstr.updateCustomer(dbptnr);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult sync_vehicle()
        {
            foreach (Context.DataTruck dbitem in RepoDataTruck.FindAll())
            {
                if (Repocode_mstr.FindByCodeName(dbitem.VehicleNo) == null){
                    try
                    {
                        Repocode_mstr.saveVehicle(dbitem);
                    }
                    catch (Exception) { }
                }
                else
                {
                    Context.code_mstr vech = Repocode_mstr.FindByCodeCode(dbitem.VehicleNo);
                    vech.code_name = dbitem.VehicleNo;
                    vech.code_desc = dbitem.JenisTrucks.StrJenisTruck;
                    Repocode_mstr.updateVehicle(vech);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult sync_customer()
        {
            foreach (Context.Customer dbitem in RepoCustomer.FindAll())
            {
                if (Repoptnr_mstr.FindByPK(dbitem.Id) == null && Repoptnr_mstr.FindByCode(dbitem.CustomerCodeOld) == null)
                {
                    Repoptnr_mstr.save(dbitem);
                }
            }
            return RedirectToAction("Index");
        }

        public void generate_ptnr_id_to_glt_det()
        {
            foreach (Context.SalesOrder so in RepoSalesOrder.FindAll().Where(d => d.SONumber.Contains("1810")).ToList()){
                queryManual("UPDATE glt_det SET glt_ptnr_id=" + so.CustomerId + " WHERE glt_ptnr_id IS NULL glt_code LIKE " + so.SONumber);
            }
        }
    }
}