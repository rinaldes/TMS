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
    public class ListOrderController : BaseController
    {
        #region initial condition

        private ISalesOrderRepo RepoSalesOrder;
        private ISalesOrderKontrakListSoRepo RepoSalesOrderKontrakListSo;
        public ListOrderController(IUserReferenceRepo repoBase, ILookupCodeRepo repoLookup, ISalesOrderRepo repoSalesOrder, ISalesOrderKontrakListSoRepo repoSalesOrderKontrakListSo)
            : base(repoBase, repoLookup)
        {
            RepoSalesOrder = repoSalesOrder;
            RepoSalesOrderKontrakListSo = repoSalesOrderKontrakListSo;
        }
        [MyAuthorize(Menu = "List Order", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "ListOrder").ToList();
            return View();
        }
        [MyAuthorize(Menu = "List Order", Action = "read")]
        public ActionResult IndexKontrak()
        {
            ViewBag.listKolom = ListKolom.Where(d => d.Action == "Index" && d.Controller == "ListOrder").ToList();
            return View();
        }

        #endregion initial condition

        #region initial value for grid main

        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrder> items = RepoSalesOrder.FindAllListOrder(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<ListOrder> ListModel = new List<ListOrder>();
            foreach (Context.SalesOrder item in items)
            {
                ListOrder a = new ListOrder(item);
                a.StatusBatal = item.StatusBatal;
                ListModel.Add(a);
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrder.CountListOrder(param.Filters), data = ListModel });
        }

        public string BindingKontrak()
        {
            GridRequestParameters param = GridRequestParameters.Current;
            List<Context.SalesOrderKontrakListSo> items = RepoSalesOrder.FindAllKontrakListSo(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();

            List<ListOrder> ListModel = new List<ListOrder>();
            foreach (var itemKontrakPerOrder in items.OrderBy(t => t.MuatDate).ToList())
            {
                ListOrder a = new ListOrder(itemKontrakPerOrder);
                a.Id = RepoSalesOrder.FindByKontrak(itemKontrakPerOrder.SalesKontrakId.Value).Id;
                a.IdSoKontrak = itemKontrakPerOrder.Id;
                a.StatusBatal = itemKontrakPerOrder.StatusBatal;
                ListModel.Add(a);
            }

            return new JavaScriptSerializer().Serialize(new { total = RepoSalesOrderKontrakListSo.CountKontrakListSo(param.Filters), data = ListModel });
        }

        public FileContentResult ExportKontrak(string NoSo)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            ExcelPackage pck = new ExcelPackage();
            List<Context.SalesOrderKontrakListSo> dbitems = RepoSalesOrder.FindAllKontrakListSo(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");
            ws.Cells[1, 1].Value = "DN";
            ws.Cells[1, 2].Value = "No SO";
            ws.Cells[1, 3].Value = "Tgl Muat";
            ws.Cells[1, 4].Value = "Jam Muat";
            ws.Cells[1, 5].Value = "Kode Nama";
            ws.Cells[1, 6].Value = "Customer";
            ws.Cells[1, 7].Value = "Jenis Barang";
            ws.Cells[1, 8].Value = "Target Suhu";
            ws.Cells[1, 9].Value = "Rute";
            ws.Cells[1, 10].Value = "Multidrop";
            ws.Cells[1, 11].Value = "Jenis Truck";
            ws.Cells[1, 12].Value = "Status Flow";
            ws.Cells[1, 13].Value = "Flow Date";
            ws.Cells[1, 14].Value = "Status Batal";
            ws.Cells[1, 15].Value = "Status Dokumen";
            ws.Cells[1, 16].Value = "Penangan Khusus";
            ws.Cells[1, 17].Value = "Vehicle No";
            ws.Cells[1, 18].Value = "Driver";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ListOrder item = new ListOrder(dbitems[i]);
                item.Id = RepoSalesOrder.FindByKontrak(dbitems[i].SalesKontrakId.Value).Id;
                ws.Cells[i + 2, 1].Value = item.DN;
                ws.Cells[i + 2, 2].Value = item.SONumber;
                ws.Cells[i + 2, 3].Value = item.TanggalMuat;
                ws.Cells[i + 2, 4].Value = item.JamMuat;
                ws.Cells[i + 2, 5].Value = item.KodeNama;
                ws.Cells[i + 2, 6].Value = item.NamaCustomer;
                ws.Cells[i + 2, 7].Value = item.StrProduct;
                ws.Cells[i + 2, 8].Value = item.Suhu;
                ws.Cells[i + 2, 9].Value = item.Rute;
                ws.Cells[i + 2, 10].Value = item.StrMultidrop;
                ws.Cells[i + 2, 11].Value = item.StrJenisTruck;
                ws.Cells[i + 2, 12].Value = item.StatusFlow;
                ws.Cells[i + 2, 13].Value = item.FlowDate;
                ws.Cells[i + 2, 14].Value = item.StatusBatal;
                ws.Cells[i + 2, 15].Value = item.StatusDokumen;
                ws.Cells[i + 2, 16].Value = item.PenanganKhusus;
                ws.Cells[i + 2, 17].Value = item.VehicleNo;
                ws.Cells[i + 2, 18].Value = item.Driver1;
            }
            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Order Kontrak.xls";
            return fsr;
        }

        public FileContentResult Export(string NoSo)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            ExcelPackage pck = new ExcelPackage();
            List<Context.SalesOrder> dbitems = RepoSalesOrder.FindAllListOrder(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters).ToList();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");
            ws.Cells[1, 1].Value = "DN";
            ws.Cells[1, 2].Value = "No SO";
            ws.Cells[1, 3].Value = "Tgl Muat";
            ws.Cells[1, 4].Value = "Jam Muat";
            ws.Cells[1, 5].Value = "Kode Nama";
            ws.Cells[1, 6].Value = "Customer";
            ws.Cells[1, 7].Value = "Jenis Barang";
            ws.Cells[1, 8].Value = "Target Suhu";
            ws.Cells[1, 9].Value = "Rute";
            ws.Cells[1, 10].Value = "Multidrop";
            ws.Cells[1, 11].Value = "Jenis Truck Awal";
            ws.Cells[1, 12].Value = "Jenis Truck Real";
            ws.Cells[1, 13].Value = "Status Flow";
            ws.Cells[1, 14].Value = "Flow Date";
            ws.Cells[1, 15].Value = "Status Batal";
            ws.Cells[1, 16].Value = "Status Dokumen";
            ws.Cells[1, 17].Value = "Penangan Khusus";
            ws.Cells[1, 18].Value = "Vehicle No";
            ws.Cells[1, 19].Value = "Driver";

            // Inserts Data
            for (int i = 0; i < dbitems.Count(); i++)
            {
                ListOrder item = new ListOrder(dbitems[i]);
                ws.Cells[i + 2, 1].Value = item.DN;
                ws.Cells[i + 2, 2].Value = item.SONumber;
                ws.Cells[i + 2, 3].Value = item.TanggalMuat;
                ws.Cells[i + 2, 4].Value = "'"+item.JamMuat;
                ws.Cells[i + 2, 5].Value = item.KodeNama;
                ws.Cells[i + 2, 6].Value = item.NamaCustomer;
                ws.Cells[i + 2, 7].Value = item.StrProduct;
                ws.Cells[i + 2, 8].Value = item.Suhu;
                ws.Cells[i + 2, 9].Value = item.Rute;
                ws.Cells[i + 2, 10].Value = item.StrMultidrop;
                ws.Cells[i + 2, 11].Value = item.MktJenisTruck;
                ws.Cells[i + 2, 12].Value = item.StrJenisTruck;
                ws.Cells[i + 2, 13].Value = item.StatusFlow;
                ws.Cells[i + 2, 14].Value = "'" + item.FlowDate;
                ws.Cells[i + 2, 15].Value = item.StatusBatal;
                ws.Cells[i + 2, 16].Value = item.StatusDokumen;
                ws.Cells[i + 2, 17].Value = item.PenanganKhusus;
                ws.Cells[i + 2, 18].Value = item.VehicleNo;
                ws.Cells[i + 2, 19].Value = item.Driver1;
            }
            var fsr = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fsr.FileDownloadName = "Order.xls";
            return fsr;
        }

        #endregion initial value for grid main
        public string generateStatusFlow()
        {
            foreach(Context.SalesOrderKontrakListSo sokls in RepoSalesOrderKontrakListSo.FindAll().Where(d => d.StatusFlow == null)){
                RepoSalesOrderKontrakListSo.save(sokls);
            }
            return "aa";
        }
        #region partial views

        [MyAuthorize(Menu = "List Order", Action="update")]
        public ActionResult InputDp(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            if (dbitem.SalesOrderOncallId != null)
            {
                return RedirectToAction("InputDp", "SalesOrderOncall", new { id = dbitem.Id, status = dbitem.Status });
            }
            else if (dbitem.SalesOrderKontrakId != null)
            {
                return RedirectToAction("InputDp", "SalesOrderKontrak", new { id = dbitem.Id, status = dbitem.Status });
            }
            else if (dbitem.SalesOrderPickupId != null)
            {
                return RedirectToAction("InputDp", "SalesOrderPickup", new { id = dbitem.Id, status = dbitem.Status });
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId != null)
            {
                return RedirectToAction("InputDp", "SalesOrderProsesKonsolidasi", new { id = dbitem.Id, status = dbitem.Status });
            }
            else
                return RedirectToAction("", "");
        }

        [MyAuthorize(Menu = "List Order", Action="update")]
        public ActionResult BatalOrder(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            if (dbitem.SalesOrderOncallId != null)
            {
                return RedirectToAction("BatalOrderOnCall", "BatalOrder", new { id = dbitem.Id });
            }
            else if (dbitem.SalesOrderPickupId != null)
            {
                return RedirectToAction("BatalOrderPickup", "BatalOrder", new { id = dbitem.SalesOrderPickupId });
            }
            else
                return RedirectToAction("", "");
        }

        [MyAuthorize(Menu = "List Order", Action="update")]
        public ActionResult RevisiTanggal(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            if (dbitem.SalesOrderOncallId != null)
            {
                return RedirectToAction("RevisiTanggalOnCall", "RevisiTanggal", new { id = dbitem.Id });
            }
            else if (dbitem.SalesOrderPickupId != null)
            {
                return RedirectToAction("RevisiTanggalPickup", "RevisiTanggal", new { id = dbitem.Id });
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId != null)
            {
                return RedirectToAction("RevisiTanggalKonsolidasi", "RevisiTanggal", new { id = dbitem.Id });
            }
            //else if (dbitem.SalesOrderProsesKonsolidasiId != null)
            //{
            //    return RedirectToAction("InputDp", "SalesOrderProsesKonsolidasi", new { id = dbitem.SalesOrderProsesKonsolidasiId });
            //}
            else
                return RedirectToAction("", "");
        }

        [MyAuthorize(Menu = "List Order", Action="update")]
        public ActionResult RevisiJenisTruk(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            if (dbitem.SalesOrderOncallId != null)
            {
                return RedirectToAction("RevisiKapasitasOnCall", "RevisiJenisTruk", new { id = dbitem.Id });
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId != null)
            {
                return RedirectToAction("RevisiKapasitasKonsolidasi", "RevisiJenisTruk", new { id = dbitem.Id });
            }
            else if (dbitem.SalesOrderPickupId != null)
            {
                return RedirectToAction("RevisiKapasitasPickup", "RevisiJenisTruk", new { id = dbitem.Id });
            }
            //else if (dbitem.SalesOrderProsesKonsolidasiId != null)
            //{
            //    return RedirectToAction("InputDp", "SalesOrderProsesKonsolidasi", new { id = dbitem.SalesOrderProsesKonsolidasiId });
            //}
            else
                return RedirectToAction("", "");
        }
        
        [MyAuthorize(Menu = "List Order", Action="update")]
        public ActionResult RevisiRute(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);
            if (dbitem.SalesOrderOncallId != null)
            {
                return RedirectToAction("RevisiRuteOnCall", "RevisiRute", new { id = dbitem.Id });
            }
            else if (dbitem.SalesOrderPickupId != null)
            {
                return RedirectToAction("RevisiRutePickup", "RevisiRute", new { id = dbitem.Id });
            }
            else if (dbitem.SalesOrderKonsolidasiId != null)
            {
                return RedirectToAction("RevisiRutekonsolidasi", "RevisiRute", new { id = dbitem.Id });
            }
            //else if (dbitem.SalesOrderProsesKonsolidasiId != null)
            //{
            //    return RedirectToAction("InputDp", "SalesOrderProsesKonsolidasi", new { id = dbitem.SalesOrderProsesKonsolidasiId });
            //}
            else
                return RedirectToAction("", "");
        }

        [MyAuthorize(Menu = "List Order", Action="read")]
        public ActionResult View(int id)
        {
            Context.SalesOrder dbitem = RepoSalesOrder.FindByPK(id);

            if (dbitem.SalesOrderOncallId != null){
                SalesOrderOncall model = new SalesOrderOncall(dbitem);
                ViewBag.name = model.SONumber;
                return View("SalesOrderOncall/FormReadOnly", model);
            }
            else if (dbitem.SalesOrderPickupId != null){
                SalesOrderPickup model = new SalesOrderPickup(dbitem);
                ViewBag.name = model.SONumber;
                return View("SalesOrderPickup/FormReadOnly", model);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId != null){
                SalesOrderProsesKonsolidasi model = new SalesOrderProsesKonsolidasi(dbitem);
                ViewBag.name = model.SONumber;
                return View("SalesOrderProsesKonsolidasi/FormReadOnly", model);
            }
            else if (dbitem.SalesOrderKontrakId != null){
                SalesOrderKontrak model = new SalesOrderKontrak(dbitem);
                ViewBag.name = model.SONumber;
                return View("SalesOrderKontrak/FormReadOnly", model);
            }
            return RedirectToAction("", "");
        }
        #endregion partial views
    }
}