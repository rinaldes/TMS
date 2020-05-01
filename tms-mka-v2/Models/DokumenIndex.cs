using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DokumenIndex
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public string Status { get; set; }
        public string NoSo { get; set; }
        public string IKSNo { get; set; }
        public decimal? Tonase { get; set; }
        public string VehicleNo { get; set; }
        public string JnsTruck { get; set; }
        public string Customer { get; set; }
        public string Rute { get; set; }
        public string SONumber { get; set; }
        public DateTime? TanggalMuat { get; set; }
        public System.DateTime? ReceivedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Delay { get; set; }
        public string Lengkap { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsReturn { get; set; }
        public bool IsLengkap { get; set; }
        public List<DokumenItem> ListDokumen { get; set; }
        public string KodeNama { get; set; }
        public string NamaDriver { get; set; }
        public string Keterangan { get; set; }

        public DokumenIndex() { }
        public DokumenIndex(Context.Dokumen dbitem)
        {
            Id = dbitem.Id; 
            IsAdmin = dbitem.IsAdmin;
            Status = dbitem.IsAdmin && dbitem.Status == "Save" ? "Save" : dbitem.IsComplete ? "Close" : "Open";
            if (dbitem.SalesOrder != null)
            {
                if (dbitem.SalesOrder.Customer != null)
                    KodeNama = dbitem.SalesOrder.Customer.CustomerCodeOld;
                if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderOncall.SONumber;
                    VehicleNo = dbitem.SalesOrder.SalesOrderOncall.DataTruck == null ? "" : dbitem.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo;
                    JnsTruck = dbitem.SalesOrder.SalesOrderOncall.DataTruck == null ? "" : dbitem.SalesOrder.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck;
                    Rute = dbitem.SalesOrder.SalesOrderOncall.StrDaftarHargaItem;
                    TanggalMuat = dbitem.SalesOrder.SalesOrderOncall.TanggalMuat;
                    NamaDriver = dbitem.SalesOrder.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrder.SalesOrderOncall.Driver1.NamaDriver;
                    Keterangan = dbitem.SalesOrder.SalesOrderOncall.Keterangan;
                }
                else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderPickup.SONumber;
                    VehicleNo = dbitem.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo;
                    JnsTruck = dbitem.SalesOrder.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck;
                    Rute = dbitem.SalesOrder.SalesOrderPickup.Rute.Nama;
                    TanggalMuat = dbitem.SalesOrder.SalesOrderPickup.TanggalPickup;
                    NamaDriver = dbitem.SalesOrder.SalesOrderPickup.Driver1.NamaDriver;
                    Keterangan = dbitem.SalesOrder.SalesOrderPickup.Keterangan;
                }
                else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                    VehicleNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                    JnsTruck = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck;
                    Rute = dbitem.RuteSo;
                    TanggalMuat = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.TanggalMuat;
                    NamaDriver = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                    Keterangan = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.Keterangan;
                }
                else if (dbitem.SalesOrder.SalesOrderKonsolidasiId.HasValue)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderKonsolidasi.SONumber;
                    Context.SalesOrderProsesKonsolidasiItem sopki = context.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == dbitem.SalesOrder.SalesOrderKonsolidasiId).FirstOrDefault();
                    if (sopki != null)
                    {
                        Context.SalesOrderProsesKonsolidasi sopk = sopki.SalesOrderProsesKonsolidasi;
                        if (sopk.DataTruck != null)
                        {
                            VehicleNo = sopk.DataTruck.VehicleNo;
                            if (sopk.DataTruck.JenisTrucks != null)
                                JnsTruck = sopk.DataTruck.JenisTrucks.StrJenisTruck;
                        }
                        IKSNo = sopk.SONumber;
                        TanggalMuat = sopk.TanggalMuat;
                        if (sopk.Driver1 != null)
                            NamaDriver = sopk.Driver1.NamaDriver;
                        Keterangan = sopk.Keterangan;
                    }
                    Rute = dbitem.SalesOrder.SalesOrderKonsolidasi.StrDaftarHargaItem;
                    Tonase = dbitem.SalesOrder.SalesOrderKonsolidasi.Tonase;
                }
                else if (dbitem.ListIdSo != null && dbitem.ListIdSo != "")
                {
                    List<int> ListIdDumy = dbitem.ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                    List<Context.SalesOrderKontrakListSo> dbsoDummy = context.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                    NoSo = dbitem.SONumber;
                    if (dbsoDummy.FirstOrDefault() != null && dbsoDummy.FirstOrDefault().DataTruck != null)
                    {
                        VehicleNo = dbsoDummy.FirstOrDefault().DataTruck.VehicleNo;
                        JnsTruck = dbsoDummy.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck;
                        TanggalMuat = dbsoDummy.FirstOrDefault().MuatDate;
                    }
                    if (dbsoDummy.FirstOrDefault() != null && dbsoDummy.FirstOrDefault().Driver1 != null)
                        NamaDriver = dbsoDummy.FirstOrDefault().Driver1.NamaDriver;
                    if (dbsoDummy.FirstOrDefault() != null && dbsoDummy.FirstOrDefault().SalesOrderKontrak.Customer != null)
                    {
                        Customer = dbsoDummy.FirstOrDefault().SalesOrderKontrak.Customer.CustomerNama;
                        KodeNama = dbsoDummy.FirstOrDefault().SalesOrderKontrak.Customer.CustomerCodeOld;
                    }
                }
            }
            else if (dbitem.ListIdSo != null && dbitem.ListIdSo != "")
            {
                List<int> ListIdDumy = dbitem.ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = context.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                NoSo = dbitem.SONumber;
                if (dbsoDummy.FirstOrDefault() != null && dbsoDummy.FirstOrDefault().DataTruck != null)
                {
                    VehicleNo = dbsoDummy.FirstOrDefault().DataTruck.VehicleNo;
                    JnsTruck = dbsoDummy.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck;
                    TanggalMuat = dbsoDummy.FirstOrDefault().MuatDate;
                    NamaDriver = dbsoDummy.FirstOrDefault().Driver1.NamaDriver;
                }
                Customer = dbsoDummy.FirstOrDefault().SalesOrderKontrak.Customer.CustomerNama;
                KodeNama = dbsoDummy.FirstOrDefault().SalesOrderKontrak.Customer.CustomerCodeOld;
            }

            Customer = dbitem.Customer.CustomerNama;
            KodeNama = dbitem.Customer.CustomerCodeOld;
            Delay = dbitem.DokumenItem.Count() == 0 ? 0 : dbitem.DokumenItem.Where(d => !d.IsLengkap).Count();
            Lengkap = dbitem.DokumenItem.Count() == 0 ? "Ya" : dbitem.DokumenItem.Any(d => !d.IsLengkap) ? "Tidak" : "Ya";
            IsLengkap = dbitem.IsLengkap;
            LastUpdate = dbitem.ModifiedDate;
            ReceivedDate = dbitem.ReceivedDate;

            ListDokumen = new List<DokumenItem>();
            foreach (var item in dbitem.DokumenItem.Where(d => d.IsLengkap == true))
            {
                ListDokumen.Add(new DokumenItem(item));
            }

            IsReturn = dbitem.IsReturn;
        }
    }
}