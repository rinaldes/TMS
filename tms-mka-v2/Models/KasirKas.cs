using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Kasirkas
    {
        public int Id { get; set; }
        public string KodeDriverOld { get; set; }
        public string Status { get; set; }
        public int? IdSalesOrder { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public int? IdRemoval { get; set; }
        public int? IdSettlement { get; set; }
        public string DnNo { get; set; }
        public string SoNo { get; set; }
        public int IdChild { get; set; }
        public string IdDriver { get; set; }
        public string Driver { get; set; }
        public string VehicleNo { get; set; }
        public string KodeNama { get; set; }
        public string Customer { get; set; }
        public DateTime? TanggalJalan { get; set; }
        public string Keterangan { get; set; }
        public DateTime? Tanggal { get; set; }
        public decimal? Jumlah { get; set; }
        public decimal? Realisasi { get; set; }
        public DateTime? Waktu { get; set; }
        public string Penerima { get; set; }
        public string strGrid { get; set; }
        public string JenisOrder { get; set; }
        public string ListIdSo { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string JadwalTransfer { get; set; }
        public Kasirkas()
        {

        }
        public Kasirkas(Context.SalesOrder dbitem)
        {
            if (dbitem.AdminUangJalanId.HasValue){
                Context.AdminUangJalanUangTf dbkas = dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(n => n.Keterangan == "Tunai").FirstOrDefault();
                List<Context.AdminUangJalanUangTf> dbkases = dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(n => n.Keterangan == "Tunai").ToList();
                if (dbkas != null)
                {
                    IdSalesOrder = dbitem.Id;
                    if (dbitem.Status == "settlement")
                        Status = "Close";
                    else
                        Status = dbkas.isTf ? "Sudah" : "Belum";
                    Jumlah = dbkas.Value;
                    Realisasi = dbkas.JumlahTransfer;
                    Waktu = dbkas.TanggalAktual + dbkas.JamAktual;
                    Tanggal = dbkas.Tanggal;
                    Penerima = dbkas.IdDriverPenerima.HasValue ? dbkas.Driver.NamaDriver : "";
                    ModifiedDate = dbitem.DateStatus;
                    if (dbitem.SalesOrderOncallId.HasValue)
                    {
                        DnNo = dbitem.SalesOrderOncall.DN;
                        SoNo = dbitem.SalesOrderOncall.SONumber;
                        IdChild = dbitem.SalesOrderOncallId.Value;
                        if (dbitem.SalesOrderOncall.Driver1 != null)
                        {
                            IdDriver = dbitem.SalesOrderOncall.Driver1.KodeDriver;
                            Driver = dbitem.SalesOrderOncall.Driver1.NamaDriver;
                        }
                        if (dbitem.SalesOrderOncall.DataTruck != null)
                        VehicleNo = dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                        KodeNama = dbitem.SalesOrderOncall.Customer.CustomerCodeOld;
                        Customer = dbitem.SalesOrderOncall.Customer.CustomerNama;
                        TanggalJalan = dbitem.SalesOrderOncall.TanggalMuat;
                        KodeDriverOld = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.KodeDriverOld;
                    }
                    else if (dbitem.SalesOrderPickupId.HasValue)
                    {
                        DnNo = dbitem.SalesOrderPickup.SONumber;
                        IdChild = dbitem.SalesOrderPickupId.Value;
                        IdDriver = dbitem.SalesOrderPickup.Driver1.KodeDriver;
                        Driver = dbitem.SalesOrderPickup.Driver1.NamaDriver;
                        VehicleNo = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
                        KodeNama = dbitem.SalesOrderPickup.Customer.CustomerCodeOld;
                        Customer = dbitem.SalesOrderPickup.Customer.CustomerNama;
                        TanggalJalan = dbitem.SalesOrderPickup.TanggalOrder;
                        KodeDriverOld = dbitem.SalesOrderPickup.Driver1 == null ? "" : dbitem.SalesOrderPickup.Driver1.KodeDriverOld;
                    }
                    else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
                    {
                        DnNo = dbitem.SalesOrderProsesKonsolidasi.DN;
                        SoNo = dbitem.SalesOrderProsesKonsolidasi.SONumber;
                        IdChild = dbitem.SalesOrderProsesKonsolidasiId.Value;
                        IdDriver = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver;
                        Driver = dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                        VehicleNo = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                        TanggalJalan = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat;
                        KodeDriverOld = dbitem.SalesOrderProsesKonsolidasi.Driver1 == null ? "" : dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriverOld;
                    }
                }
            }
        }

        public Kasirkas(Context.SalesOrder dbitem, Context.SettlementReguler sr)
        {
            DnNo = dbitem.DN;
            SoNo = dbitem.SONumber;
            if (dbitem.Driver != null)
            {
                IdDriver = dbitem.Driver.KodeDriver;
                Driver = dbitem.Driver.NamaDriver;
                KodeDriverOld = dbitem.Driver.KodeDriverOld;
            }
            if (dbitem.DataTruck != null)
                VehicleNo = dbitem.DataTruck.VehicleNo;
            if (dbitem.Customer != null)
            {
                KodeNama = dbitem.Customer.CustomerCodeOld;
                Customer = dbitem.Customer.CustomerNama;
            }
            TanggalJalan = dbitem.OrderTanggalMuat;
        }

        
        public Kasirkas(Context.AdminUangJalan auj)
        {
            Context.AdminUangJalanUangTf dbkas = auj.AdminUangJalanUangTf.Where(n => n.Keterangan == "Tunai").FirstOrDefault();
            List<Context.AdminUangJalanUangTf> dbkases = auj.AdminUangJalanUangTf.Where(n => n.Keterangan == "Tunai").ToList();
            IdSalesOrder = auj.SalesOrderId;
            IdAdminUangJalan = auj.Id;
            Status = "Sudah";
            Jumlah = dbkas.Value;
            Realisasi = dbkas.JumlahTransfer;
            Waktu = dbkas.TanggalAktual + dbkas.JamAktual;
            Tanggal = dbkas.Tanggal;
            Penerima = dbkas.IdDriverPenerima.HasValue ? dbkas.Driver.NamaDriver : "";
            DnNo = "DN-"+auj.SONumber;
            SoNo = auj.SONumber;
            IdDriver = auj.Driver1.KodeDriver;
            Driver = auj.Driver1.NamaDriver;
            VehicleNo = auj.DataTruck.VehicleNo;
            if (auj.Customer != null)
            {
                KodeNama = auj.Customer.CustomerCodeOld;
                Customer = auj.Customer.CustomerNama;
            }
            TanggalJalan = auj.AUJTanggalMuat;
            KodeDriverOld = auj.Driver1.KodeDriverOld;
        }
        public Kasirkas(Context.SalesOrder dbso, List<Context.SalesOrderKontrakListSo> dbitem)
        {
            Context.AdminUangJalanUangTf dbkas = dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(n => n.Keterangan == "Tunai").FirstOrDefault();
            IdSalesOrder = dbso.Id;
            if (dbitem.FirstOrDefault().Status == "settlement")
                Status = "Close";
            else
                Status = dbkas.isTf ? "Sudah" : "Belum";
            Jumlah = dbkas.Value;
            Realisasi = dbkas.JumlahTransfer;
            Waktu = dbkas.TanggalAktual + dbkas.JamAktual;
            Tanggal = dbkas.Tanggal;
            Penerima = dbkas.IdDriverPenerima.HasValue ? dbkas.Driver.NamaDriver : "";

            DnNo = "";
            SoNo = string.Join(", ", dbitem.Select(s => s.NoSo).ToList());
            IdDriver = dbitem.FirstOrDefault().Driver1.KodeDriver;
            KodeDriverOld = dbitem.FirstOrDefault().Driver1.KodeDriverOld;
            Driver = dbitem.FirstOrDefault().Driver1.NamaDriver;
            VehicleNo = dbitem.FirstOrDefault().DataTruck.VehicleNo;
            KodeNama = dbso.SalesOrderKontrak.Customer.CustomerCodeOld;
            Customer = dbso.SalesOrderKontrak.Customer.CustomerNama;
            //TanggalJalan = dbitem.SalesOrderOncall.TanggalMuat;
            JenisOrder = "Kontrak";
            ListIdSo = string.Join(".", dbitem.Select(d => d.Id.ToString()).ToList());
            ModifiedDate = dbso.DateStatus;
            JadwalTransfer = String.Join(",", dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(d => !d.isTf && d.Value > 0 && d.Keterangan == "Tunai").Select(d => d.Tanggal.Value.ToString("dd-MM-yyyy")));
        }
        public Kasirkas(Context.Removal dbitem)
        {
            Context.RemovalUangTf dbkas = dbitem.RemovalUangTf.Where(n => n.Keterangan == "Tunai").FirstOrDefault();
            if (dbkas != null)
            {
                IdRemoval = dbitem.Id;
                IdSalesOrder = dbitem.IdSO;
                if (dbitem.Status == "settlement")
                    Status = "Close";
                else
                    Status = dbkas.isTf ? "Sudah" : "Belum";
                Jumlah = dbkas.Value;
                Realisasi = dbkas.JumlahTransfer;
                Waktu = dbkas.TanggalAktual + dbkas.JamAktual;
                Tanggal = dbkas.Tanggal;
                Penerima = dbkas.IdDriverPenerima.HasValue ? dbkas.Driver.NamaDriver : "";
                ModifiedDate = dbitem.ModifiedDate;
                if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
                {
                    DnNo = dbitem.SalesOrder.SalesOrderOncall.DN;
                    SoNo = dbitem.SalesOrder.SalesOrderOncall.SONumber;
                    IdChild = dbitem.SalesOrder.SalesOrderOncallId.Value;
                    IdDriver = dbitem.SalesOrder.SalesOrderOncall.Driver1.KodeDriver;
                    Driver = dbitem.SalesOrder.SalesOrderOncall.Driver1.NamaDriver;
                    VehicleNo = dbitem.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo;
                    Customer = dbitem.SalesOrder.SalesOrderOncall.Customer.CustomerNama;
                    TanggalJalan = dbitem.SalesOrder.SalesOrderOncall.TanggalMuat;
                }
                else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
                {
                    DnNo = dbitem.SalesOrder.SalesOrderPickup.SONumber;
                    IdChild = dbitem.SalesOrder.SalesOrderPickupId.Value;
                    IdDriver = dbitem.SalesOrder.SalesOrderPickup.Driver1.KodeDriver;
                    Driver = dbitem.SalesOrder.SalesOrderPickup.Driver1.NamaDriver;
                    VehicleNo = dbitem.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo;
                    Customer = dbitem.SalesOrder.SalesOrderPickup.Customer.CustomerNama;
                    TanggalJalan = dbitem.SalesOrder.SalesOrderPickup.TanggalOrder;
                }
                else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    DnNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DN;
                    SoNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                    IdChild = dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.Value;
                    IdDriver = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.Driver1.KodeDriver;
                    Driver = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                    VehicleNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                    TanggalJalan = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.TanggalMuat;
                }
                JadwalTransfer = String.Join(",", dbkas.Tanggal.Value.ToString("dd-MM-yyyy"));
            }
        }
    }
}