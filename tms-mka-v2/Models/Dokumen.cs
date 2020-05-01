using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Dokumen
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        public SalesOrderKontrak ModelKontrak { get; set; }
        public string ListIdSo { get; set; }
        public bool IsComplete { get; set; }
        public string SONumber { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime ModifiedDate { get; set; }
        public System.DateTime? ReceivedDate { get; set; }
        public string strDokumen { get; set; }
        public List<DokumenItem> ListDokumen { get; set; }
        public List<DokumenItem> ListHistory { get; set; }
        public Dokumen()
        {

        }
        public Dokumen(Context.Dokumen dbitem)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSO;
            if (dbitem.SalesOrder != null)
            {
                if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
                {
                    ModelOncall = new SalesOrderOncall(dbitem.SalesOrder);
                }
                else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
                {
                    ModelPickup = new SalesOrderPickup(dbitem.SalesOrder);
                }
                else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem.SalesOrder);
                }
                else if (dbitem.SalesOrder.SalesOrderKontrakId.HasValue)
                {
                    List<int> ListIdDumy = dbitem.ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                    List<Context.SalesOrderKontrakListSo> dbsoDummy = dbitem.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                    dbitem.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;

                    ModelKontrak = new SalesOrderKontrak(dbitem.SalesOrder);

                    ModelKontrak.ListValueModelSOKontrak = ModelKontrak.ListModelSOKontrak;
                }
                else
                {
                    SONumber = dbitem.SalesOrder.SONumber;
                }
            }
            else if (dbitem.ListIdSo != "" && dbitem.ListIdSo != null)
            {
                List<int> ListIdDumy = dbitem.ListIdSo.Split(new string[] { "." }, StringSplitOptions.None).ToList().Select(int.Parse).ToList();
                List<Context.SalesOrderKontrakListSo> dbsoDummy = context.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).ToList();
                dbitem.SalesOrder = context.SalesOrder.Where(d => d.SalesOrderKontrakId == dbsoDummy.FirstOrDefault().SalesKontrakId).FirstOrDefault();
                dbitem.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo = dbsoDummy;

                ModelKontrak = new SalesOrderKontrak(dbitem.SalesOrder);

                ModelKontrak.ListValueModelSOKontrak = ModelKontrak.ListModelSOKontrak;
            }
            ListDokumen = new List<DokumenItem>();
            foreach (var item in dbitem.DokumenItem)
            {
                ListDokumen.Add(new DokumenItem(item));
            }
            ListHistory = new List<DokumenItem>();
            foreach (var item in dbitem.DokumenItemHistory)
            {
                ListHistory.Add(new DokumenItem(item));
            }
            
            ListIdSo = dbitem.ListIdSo;
            IsComplete = dbitem.IsComplete;
            ModifiedDate = dbitem.ModifiedDate;
            ReceivedDate = dbitem.ReceivedDate;
            IsAdmin = dbitem.IsAdmin;
            SONumber = dbitem.SONumber;
        }
        public void setDb(Context.Dokumen dbitem)
        {
            dbitem.Id = Id;
            //dbitem.NoSo = NoSo;
            //dbitem.VehicleNo = VehicleNo;
            //dbitem.Customer = Customer;
        }
    }
    
    public class DokumenItem
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public int IdBilling { get; set; }
        public string Nama { get; set; }
        public int Jml { get; set; }
        public string Warna { get; set; }
        public bool Stempel { get; set; }
        public bool Lengkap { get; set; }
        public string KeteranganAdmin { get; set; }
        public string KeteranganBilling { get; set; }
        public bool IsEdit { get; set; }
        public string User { get; set; }
        public DateTime Tanggal { get; set; }
        public DokumenItem()
        {

        }
        public DokumenItem(Context.DokumenItem dbitem)
        {
            Id = dbitem.Id;
            IdCustomer = dbitem.CustomerId;
            IdBilling = dbitem.IdBilling;
            if (dbitem.CustomerBilling != null)
            {
                Nama = dbitem.CustomerBilling.DocumentName;
                Jml = dbitem.CustomerBilling.Lembar;
                Warna = dbitem.CustomerBilling.Warna;
                Stempel = dbitem.CustomerBilling.Stempel;
            }
            Lengkap = dbitem.IsLengkap;
            KeteranganAdmin = dbitem.KeteranganAdmin;
            KeteranganBilling = dbitem.KeteranganBilling;
            Tanggal = dbitem.ModifiedDate;
        }
        public DokumenItem(Context.DokumenItemHistory dbitem)
        {
            Id = dbitem.Id;
            Nama = dbitem.Nama;
            Jml = dbitem.Jml;
            Warna = dbitem.Warna;
            Stempel = dbitem.Stempel;
            Lengkap = dbitem.Lengkap;
            KeteranganAdmin = dbitem.KeteranganAdmin;
            KeteranganBilling = dbitem.KeteranganBilling;
            Tanggal = dbitem.ModifiedDate;
            User = context.User.Where(d => d.Id == dbitem.UserId).FirstOrDefault() == null ? "" : context.User.Where(d => d.Id == dbitem.UserId).FirstOrDefault().Username;
        }
        public void SetDb(Context.DokumenItem dbitem)
        {
            dbitem.IdBilling = IdBilling;
            dbitem.CustomerId = IdCustomer;
            dbitem.KeteranganAdmin = KeteranganAdmin;
            dbitem.KeteranganBilling = KeteranganBilling;
            dbitem.IsLengkap = Lengkap;
            dbitem.ModifiedDate = DateTime.Now;
        }
        public Context.DokumenItemHistory SetDbHistory(Context.DokumenItemHistory dbitem)
        {
            dbitem.Nama = Nama;
            dbitem.Jml = Jml;
            dbitem.Warna = Warna;
            dbitem.Stempel = Stempel;
            dbitem.Lengkap = Lengkap;
            dbitem.KeteranganAdmin = KeteranganAdmin;
            dbitem.KeteranganBilling = KeteranganBilling;
            dbitem.ModifiedDate = DateTime.Now;

            return dbitem;
        }
    }
}