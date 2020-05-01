using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Linq;


namespace tms_mka_v2.Models
{
    public class KasirTf
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public int IdAdminUangJalan { get; set; }
        public string Status { get; set; }
        public int? IdSalesOrder { get; set; }
        public int? IdSettlement { get; set; }
        public int? IdRemoval { get; set; }
        public string DnNo {get;set;}
        public string SoNo {get;set;}
        public int IdChild { get; set; }
        public string IdDriver { get; set; }
        public string KodeDriverOld { get; set; }
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
        public string strGrid { get; set; }
        public string JenisOrder { get; set; }
        public string ListIdSo { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string JadwalTransfer { get; set; }
        public KasirTf()
        {

        }
        public KasirTf(Context.SalesOrder dbitem, Context.AdminUangJalan auj=null)
        {
            IdSettlement = null;
            if (dbitem == null)
                dbitem = context.SalesOrder.Where(d => d.Id == auj.SalesOrderId).FirstOrDefault();
            IdSalesOrder = dbitem.Id;
            Jumlah = 0; Realisasi = 0;
            if (auj != null)
            {
                dbitem.AdminUangJalan = auj;
            }
            if (dbitem.Status == "settlement")
                Status = "Close";
            else if (dbitem.AdminUangJalan != null)
            {

                if ((dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai" && s.Value > 0).Any(n => n.isTf == false)) && (dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == true)))
                    Status =  "On Progress";
                else if (dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai" && s.Value > 0).Any(n => n.isTf == false))
                    Status =  "Belum";
                else
                    Status =  "Sudah";
            }
            else
            {
                Status = "Sudah";
            }
            if (dbitem.AdminUangJalan != null)
            {
                Waktu = dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(d => d.TanggalAktual != null).OrderByDescending(t => t.TanggalAktual).Select(t => t.TanggalAktual).FirstOrDefault();
                Tanggal = dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(d => d.TanggalAktual != null).OrderByDescending(t => t.TanggalAktual).Select(t => t.Tanggal).FirstOrDefault();
                IdAdminUangJalan = dbitem.AdminUangJalan.Id;
                JadwalTransfer = String.Join(",", dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(d => !d.isTf && d.Value > 0 && d.Keterangan != "Tunai").Select(d => d.Tanggal.Value.ToString("dd-MM-yyyy")));
            }
            ModifiedDate = dbitem.DateStatus;
            if (dbitem.SalesOrderOncallId.HasValue && dbitem.AdminUangJalanId.HasValue)
            {
                Id = auj == null ? dbitem.AdminUangJalanId.Value : auj.Id;
                DnNo = dbitem.SalesOrderOncall.DN;
                SoNo = dbitem.SalesOrderOncall.SONumber;
                IdChild = dbitem.SalesOrderOncallId.Value;
                IdDriver = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.KodeDriver;
                Driver = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.NamaDriver;
                KodeDriverOld = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.KodeDriverOld;
                VehicleNo = dbitem.SalesOrderOncall.DataTruck == null ? "" : dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                KodeNama = dbitem.SalesOrderOncall.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrderOncall.Customer.CustomerNama;
                if (dbitem.AdminUangJalan != null)
                {
                    foreach (var item in dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(n => n.Keterangan != "Tunai"))
                    {
                        Jumlah = Jumlah + (item.Value > 0 ? item.Value : 0);
                        Realisasi = Realisasi + item.JumlahTransfer;
                    }
                }
                else
                {
                    foreach (var item in auj.AdminUangJalanUangTf.Where(n => n.Keterangan != "Tunai"))
                    {
                        Jumlah = Jumlah + (item.Value > 0 ? item.Value : 0);
                        Realisasi = Realisasi + item.JumlahTransfer;
                    }
                }
                TanggalJalan = dbitem.SalesOrderOncall.TanggalMuat;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                DnNo = dbitem.SalesOrderPickup.SONumber;
                IdChild = dbitem.SalesOrderPickupId.Value;
                IdDriver = dbitem.SalesOrderPickup.Driver1.KodeDriver;
                Driver = dbitem.SalesOrderPickup.Driver1.NamaDriver;
                KodeDriverOld = dbitem.SalesOrderPickup.Driver1.KodeDriverOld;
                VehicleNo = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
                KodeNama = dbitem.SalesOrderPickup.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrderPickup.Customer.CustomerNama;
                foreach (var item in dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(n => n.Keterangan != "Tunai"))
                {
                    Jumlah = Jumlah + item.Value;
                    Realisasi = Realisasi + item.JumlahTransfer;
                }
                TanggalJalan = dbitem.SalesOrderPickup.TanggalPickup;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                DnNo = dbitem.SalesOrderProsesKonsolidasi.DN;
                SoNo = dbitem.SalesOrderProsesKonsolidasi.SONumber;
                IdChild = dbitem.SalesOrderProsesKonsolidasiId.Value;
                IdDriver = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver;
                Driver = dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                KodeDriverOld = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriverOld;
                VehicleNo = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                foreach (var item in dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(n => n.Keterangan != "Tunai"))
                {
                    Jumlah = Jumlah + item.Value;
                    Realisasi = Realisasi + item.JumlahTransfer;
                }
                TanggalJalan = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat;
            }
        }
        public KasirTf(Context.AdminUangJalan auj = null)
        {
            SoNo = auj.SONumber;
            IdSalesOrder = auj.SalesOrderId;
            if (auj.DaftarHargaKontrakId != null)
            {
                Context.SalesOrderKontrakListSo sokls = context.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == auj.Id).FirstOrDefault();
                if (sokls != null)
                    IdSalesOrder = context.SalesOrder.Where(d => d.SalesOrderKontrakId == sokls.SalesKontrakId.Value).FirstOrDefault().Id;
            }
            DnNo = "DN" + SoNo;
            Status = auj.Status;
            if (auj.Driver1 != null)
            {
                IdDriver = auj.Driver1.KodeDriver;
                Driver = auj.Driver1.NamaDriver;
            }
            IdAdminUangJalan = auj.Id;
            if (auj.DataTruck != null)
                VehicleNo = auj.DataTruck.VehicleNo;
            if (auj.Customer != null)
            {
                KodeNama = auj.Customer.CustomerCodeOld;
                Customer = auj.Customer.CustomerNama;
            }
            TanggalJalan = auj.AUJTanggalMuat;
            Jumlah = 0;
            Realisasi = 0;
            foreach (var item in auj.AdminUangJalanUangTf.Where(n => n.Keterangan != "Tunai"))
            {
                Jumlah = Jumlah + (item.Value > 0 ? item.Value : 0);
                Realisasi = Realisasi + item.JumlahTransfer;
            }
            if ((auj.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai" && s.Value > 0).Any(n => n.isTf == false)) && (auj.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == true)))
                Status = "On Progress";
            else if (auj.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai" && s.Value > 0).Any(n => n.isTf == false))
                Status = "Belum";
            else
                Status = "Sudah";
        }
        public KasirTf(Context.SalesOrder dbso, List<Context.SalesOrderKontrakListSo> dbitem)
        {
            IdSalesOrder = dbso.Id;
            Jumlah = 0; Realisasi = 0;
            if (dbitem.FirstOrDefault().Status == "settlement")
                Status = "Close";
            else if ((dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == false)) && (dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == true)))
                Status = "On Progress";
            else if (dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == false))
                Status = "Belum";
            else
                Status = "Sudah";
            JenisOrder = "Kontrak";
            Waktu = dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(d => d.TanggalAktual != null).OrderByDescending(t => t.TanggalAktual).Select(t => t.TanggalAktual).FirstOrDefault();
            Tanggal = dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(d => d.TanggalAktual != null).OrderByDescending(t => t.TanggalAktual).Select(t => t.Tanggal).FirstOrDefault();
            JadwalTransfer = String.Join(",", dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(d => !d.isTf && d.Value > 0 && d.Keterangan != "Tunai").Select(d => d.Tanggal.Value.ToString("dd-MM-yyyy")));

            SoNo = string.Join(", ", dbitem.Select(s => s.NoSo).ToList());
            IdDriver = dbitem.FirstOrDefault().AdminUangJalan.Driver1.KodeDriver;
            Driver = dbitem.FirstOrDefault().AdminUangJalan.Driver1.NamaDriver;
            KodeDriverOld = dbitem.FirstOrDefault().AdminUangJalan.Driver1.KodeDriverOld;
            VehicleNo = dbitem.FirstOrDefault().DataTruck.VehicleNo;
            Customer = dbso.SalesOrderKontrak.Customer.CustomerNama;
            foreach (var item in dbitem.FirstOrDefault().AdminUangJalan.AdminUangJalanUangTf.Where(n => n.Keterangan != "Tunai"))
            {
                Jumlah = Jumlah + item.Value;
                Realisasi = Realisasi + item.JumlahTransfer;
            }
            ListIdSo = string.Join(".", dbitem.Select(d => d.Id.ToString()).ToList());
            ModifiedDate = dbso.DateStatus;
        }
        public KasirTf(Context.Removal dbitem)
        {
            IdSalesOrder = dbitem.IdSO;
            IdRemoval = dbitem.Id;
            Jumlah = 0; Realisasi = 0;
            if (dbitem.Status == "settlement")
                Status = "Close";
            else if ((dbitem.RemovalUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == false)) && (dbitem.RemovalUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == true)))
                Status = "On Progress";
            else if (dbitem.RemovalUangTf.Where(s => s.Keterangan != "Tunai").Any(n => n.isTf == false))
                Status = "Belum";
            else
                Status = "Sudah";

            Waktu = dbitem.RemovalUangTf.Where(d => d.TanggalAktual != null).OrderByDescending(t => t.TanggalAktual).Select(t => t.TanggalAktual).FirstOrDefault();
            Tanggal = dbitem.RemovalUangTf.Where(d => d.TanggalAktual != null).OrderByDescending(t => t.TanggalAktual).Select(t => t.Tanggal).FirstOrDefault();
            JadwalTransfer = String.Join(",", dbitem.RemovalUangTf.Where(d => !d.isTf && d.JumlahTransfer > 0 && d.Keterangan != "Tunai").Select(d => d.Tanggal.Value.ToString("dd-MM-yyyy")));
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
                foreach (var item in dbitem.RemovalUangTf.Where(n => n.Keterangan != "Tunai"))
                {
                    Jumlah = Jumlah + item.Value;
                    Realisasi = Realisasi + item.JumlahTransfer;
                }
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
                foreach (var item in dbitem.RemovalUangTf.Where(n => n.Keterangan != "Tunai"))
                {
                    Jumlah = Jumlah + item.Value;
                    Realisasi = Realisasi + item.JumlahTransfer;
                }
                TanggalJalan = dbitem.SalesOrder.SalesOrderPickup.TanggalPickup;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                DnNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DN;
                SoNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                IdChild = dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.Value;
                IdDriver = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.Driver1.KodeDriver;
                Driver = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                VehicleNo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                foreach (var item in dbitem.RemovalUangTf.Where(n => n.Keterangan != "Tunai"))
                {
                    Jumlah = Jumlah + item.Value;
                    Realisasi = Realisasi + item.JumlahTransfer;
                }
                TanggalJalan = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.TanggalMuat;
            }
        }
    }
}