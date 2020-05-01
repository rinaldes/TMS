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

namespace tms_mka_v2.Controllers
{
    public class UnitListController : BaseController
    {
        private IDataTruckRepo RepoDataTruck;
        private IDataBoxRepo RepoDataBox;
        private IDataGPSRepo RepoDataGPS;
        private IDataPendinginRepo RepoDataPendingin;
        public UnitListController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, IDataTruckRepo repoDataTruck,
            IDataBoxRepo repoDataBox, IDataGPSRepo repoDataGPS, IDataPendinginRepo repoDataPendingin)
            : base(repoBase, repoLookup)
        {
            RepoDataTruck = repoDataTruck;
            RepoDataBox = repoDataBox;
            RepoDataGPS = repoDataGPS;
            RepoDataPendingin = repoDataPendingin;
        }
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "UnitList").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            //List<Context.DataTruck> items = RepoDataTruck.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);
            List<Context.DataTruck> items = RepoDataTruck.FindAll();

            List<UnitList> ListModel = new List<UnitList>();
            foreach (Context.DataTruck item in items)
            {
                ListModel.Add(new UnitList(item));
            }

            int total = RepoDataTruck.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingDataPendingin(int id)
        {
            Context.DataTruck dbtruck = RepoDataTruck.FindByPK(id);

            List<DataPendingin> ListModel = new List<DataPendingin>();
            foreach (Context.DataTruckPendinginHistory item in dbtruck.DataTruckPendinginHistory.OrderByDescending(d => d.Id))
            {
                if (ListModel.Count == 0)
                {
                    ListModel.Add(new DataPendingin(item));
                }
                else
                {
                    if (ListModel.Last().NoPendingin == item.NoPendingin)
                    {
                        var data = ListModel.Last();
                        data = new DataPendingin(item);
                    }
                    else
                    {
                        ListModel.Add(new DataPendingin(item));
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }
        public string BindingDataBox(int id)
        {
            Context.DataTruck dbtruck = RepoDataTruck.FindByPK(id);

            List<DataBox> ListModel = new List<DataBox>();
            foreach (Context.DataTruckBoxHistory item in dbtruck.DataTruckBoxHistory.OrderByDescending(d => d.Id))
            {
                if (ListModel.Count == 0)
                {
                    ListModel.Add(new DataBox(item));
                }
                else
                {
                    if (ListModel.Last().NoBox == item.NoBox)
                    {
                        var data = ListModel.Last();
                        data = new DataBox(item);
                    }
                    else
                    {
                        ListModel.Add(new DataBox(item));
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }
        public string BindingDataGps(int id)
        {
            Context.DataTruck dbtruck = RepoDataTruck.FindByPK(id);

            List<DataGPS> ListModel = new List<DataGPS>();
            foreach (Context.DataTruckGPSHistory item in dbtruck.DataTruckGPSHistory.OrderByDescending(d => d.Id))
            {
                if (ListModel.Count == 0)
                {
                    ListModel.Add(new DataGPS(item));
                }
                else
                {
                    if (ListModel.Last().NoGPS == item.NoGPS)
                    {
                        var data = ListModel.Last();
                        data = new DataGPS(item);
                    }
                    else
                    {
                        ListModel.Add(new DataGPS(item));
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(new { data = ListModel });
        }
        public ActionResult Detail(int id)
        {
            Context.DataTruck dbitem = RepoDataTruck.FindByPK(id);
            UnitList model = new UnitList(dbitem);

            return View("Form", model);
        }
    }
}