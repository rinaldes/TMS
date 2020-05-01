using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class AdminUangJalanIndex
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public int IdChild { get; set; }
        public string Status { get; set; }
        public string JenisOrder { get; set; }
        public string NoSo { get; set; }
        public string KodeNama { get; set; }
        public string SONumber { get; set; }
        public string Customer { get; set; }
        public string VehicleNo { get; set; }
        public string JenisTruk { get; set; }
        public string IDDriver { get; set; }
        public string IDDriverOld { get; set; }
        public string Driver { get; set; }
        public string Rute { get; set; }
        public DateTime? TanggalMuat { get; set; }
        public TimeSpan JamMuat { get; set; }
        public int? JumlahRit { get; set; }
        public string KeteranganPenggatian { get; set; }
        public string KeteranganAdmin { get; set; }
        public string Keterangan { get; set; }
        public string ListIdSo { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool RuteRevised { get; set; }
        public decimal? Nominal { get; set; }
        public bool TfExecuted { get; set; }
        public bool CashExecuted { get; set; }

        public AdminUangJalanIndex()
        {

        }
        public AdminUangJalanIndex(Context.SalesOrder dbitem)
        {
            IdSalesOrder = dbitem.Id;
            Status = dbitem.Status == "dispatched" || dbitem.Status == "settlement" || dbitem.Status == "admin uang jalan" ? "Sudah" : "Belum";
            Context.AdminUangJalan dbauj = dbitem.AdminUangJalan;
            if (dbitem.AdminUangJalanId.HasValue)
            {
                if(dbitem.AdminUangJalan.Removal.Count > 0)
                    Status = "Removal";            
            }
            ModifiedDate = dbitem.DateStatus;
            RuteRevised = dbitem.RuteRevised;
            KeteranganAdmin = dbauj == null ? "" : dbauj.KeteranganAdmin;
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                JenisOrder = "Oncall";
                IdChild = dbitem.SalesOrderOncallId.Value;
                NoSo = dbitem.SalesOrderOncall.SONumber;
                KodeNama = dbitem.SalesOrderOncall.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrderOncall.Customer.CustomerNama;
                VehicleNo = dbitem.SalesOrderOncall.DataTruck == null ? "" : dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                if (dbitem.SalesOrderOncall.DataTruck != null && dbitem.SalesOrderOncall.DataTruck.JenisTrucks != null)
                    JenisTruk = dbitem.SalesOrderOncall.DataTruck == null ? "" : dbitem.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck;
                IDDriver = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.KodeDriver;
                IDDriverOld = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.KodeDriverOld;
                Driver = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.NamaDriver;
                Rute = dbitem.SalesOrderOncall.StrDaftarHargaItem;
                TanggalMuat = dbitem.SalesOrderOncall.TanggalMuat;
                JamMuat = dbitem.SalesOrderOncall.JamMuat;
                JumlahRit = 0;
                KeteranganPenggatian = dbitem.SalesOrderOncall.KeteranganDriver1;
                Keterangan = dbitem.SalesOrderOncall.Keterangan;
            }

            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                JenisOrder = "Pickup";
                IdChild = dbitem.SalesOrderPickupId.Value;
                NoSo = dbitem.SalesOrderPickup.SONumber;
                KodeNama = dbitem.SalesOrderPickup.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrderPickup.Customer.CustomerNama;
                VehicleNo = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
                JenisTruk = dbitem.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck;
                IDDriver = dbitem.SalesOrderPickup.Driver1.KodeDriver;
                IDDriverOld = dbitem.SalesOrderPickup.Driver1.KodeDriverOld;
                Driver = dbitem.SalesOrderPickup.Driver1.NamaDriver;
                Rute = dbitem.SalesOrderPickup.Rute.Nama;
                TanggalMuat = dbitem.SalesOrderPickup.TanggalPickup;
                JamMuat = dbitem.SalesOrderPickup.JamPickup;
                Keterangan = dbitem.SalesOrderPickup.Keterangan;
                JumlahRit = 0;
                KeteranganPenggatian = dbitem.SalesOrderPickup.KeteranganDriver1;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                JenisOrder = "Konsolidasi";
                IdChild = dbitem.SalesOrderProsesKonsolidasiId.Value;
                NoSo = dbitem.SalesOrderProsesKonsolidasi.SONumber;
                Customer = "";
                VehicleNo = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                JenisTruk = dbitem.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck;
                IDDriver = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver;
                IDDriverOld = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriverOld;
                Driver = dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
                Rute = dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                TanggalMuat = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat;
                JamMuat = dbitem.SalesOrderProsesKonsolidasi.JamMuat;
                Keterangan = dbitem.SalesOrderProsesKonsolidasi.Keterangan;
                JumlahRit = 0;
                KeteranganPenggatian = dbitem.SalesOrderProsesKonsolidasi.KeteranganDriver1;
            }
        }
        public AdminUangJalanIndex(Context.SalesOrder dbSo, List<Context.SalesOrderKontrakListSo> dbitem, string penghibur)
        {
            IdSalesOrder = dbitem.FirstOrDefault().SalesKontrakId;
            Status = dbitem.FirstOrDefault().Status == "admin uang jalan" || dbitem.FirstOrDefault().Status == "dispatched" || dbitem.FirstOrDefault().Status == "settlement" ? "Sudah" : "Belum";
            JenisOrder = "Kontrak";
            IdChild = dbitem.FirstOrDefault().SalesKontrakId.Value;
            NoSo = string.Join(", ", dbitem.Select(s => s.NoSo).ToList());
            KodeNama = dbitem.FirstOrDefault().SalesOrderKontrak.Customer.CustomerCodeOld;
            Customer = dbitem.FirstOrDefault().SalesOrderKontrak.Customer.CustomerNama;
            VehicleNo = dbitem.FirstOrDefault().DataTruck.VehicleNo;
            JenisTruk = dbitem.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck;
            IDDriver = dbitem.FirstOrDefault().Driver1.KodeDriver;
            IDDriverOld = dbitem.FirstOrDefault().Driver1.KodeDriverOld;
            Driver = dbitem.FirstOrDefault().Driver1.NamaDriver;
            TanggalMuat = dbitem.FirstOrDefault().MuatDate;
            JamMuat = dbitem.FirstOrDefault().SalesOrderKontrak.JamMuat;
            JumlahRit = 0;
            ListIdSo = string.Join(".", dbitem.Select(d => d.Id.ToString()).ToList());
            //ModifiedDate = dbSo.DateStatus;
            Rute = "";
        }
        public AdminUangJalanIndex(Context.AdminUangJalan auj, List<Context.SalesOrderKontrakListSo> dbitem, string penghibur)
        {
            Id = auj.Id;
            NoSo = auj.SONumber;
            SONumber = auj.SONumber;
            Status = auj.Status;
            JenisOrder = "Kontrak";
            if (auj.DataTruck != null)
            {
                VehicleNo = auj.DataTruck.VehicleNo;
                JenisTruk = auj.DataTruck.JenisTrucks.StrJenisTruck;
            }
            else
            {
                VehicleNo = dbitem.FirstOrDefault().DataTruck.VehicleNo;
                JenisTruk = dbitem.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck;
            }
            if (auj.Driver1 != null)
            {
                IDDriver = auj.Driver1.KodeDriver;
                IDDriverOld = auj.Driver1.KodeDriverOld;
                Driver = auj.Driver1.NamaDriver;
            }
            ListIdSo = string.Join(".", dbitem.Select(d => d.Id.ToString()).ToList());
            JumlahRit = 0;
            if (auj.Customer != null)
            {
                KodeNama = auj.Customer.CustomerCodeOld;
                Customer = auj.Customer.CustomerNama;
            }
            TanggalMuat = auj.AUJTanggalMuat;
            if (dbitem.FirstOrDefault() != null)
            {
                IdSalesOrder = dbitem.FirstOrDefault().SalesKontrakId;
                IdChild = dbitem.FirstOrDefault().SalesKontrakId.Value;
                JamMuat = dbitem.FirstOrDefault().SalesOrderKontrak.JamMuat;
            }
            else
            {
                Context.SalesOrderKontrakListSo sokls = context.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == auj.Id).FirstOrDefault();
                if (sokls != null)
                {
                    IdSalesOrder = sokls.SalesKontrakId;
                    IdChild = sokls.SalesKontrakId.Value;
                    JamMuat = sokls.SalesOrderKontrak.JamMuat;
                }
            }
        }

        public AdminUangJalanIndex(Context.SalesOrder dbSo, List<Context.SalesOrderKontrakListSo> dbitem)
        {
            IdSalesOrder = dbSo.Id;
            Status = dbitem.FirstOrDefault().Status == "dispatched" || dbitem.FirstOrDefault().Status == "settlement" ? "Sudah" : "Belum";
            JenisOrder = "Kontrak";
            IdChild = dbitem.FirstOrDefault().SalesKontrakId.Value;
            NoSo = string.Join(", ", dbitem.Select(s => s.NoSo).ToList());
            KodeNama = dbitem.FirstOrDefault().SalesOrderKontrak.Customer.CustomerCodeOld;
            Customer = dbitem.FirstOrDefault().SalesOrderKontrak.Customer.CustomerNama;
            VehicleNo = dbitem.FirstOrDefault().DataTruck.VehicleNo;
            JenisTruk = dbitem.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck;
            IDDriver = dbitem.FirstOrDefault().Driver1.KodeDriver;
            IDDriverOld = dbitem.FirstOrDefault().Driver1.KodeDriverOld;
            Driver = dbitem.FirstOrDefault().Driver1.NamaDriver;
            TanggalMuat = dbitem.FirstOrDefault().MuatDate;
            JamMuat = dbitem.FirstOrDefault().SalesOrderKontrak.JamMuat;
            JumlahRit = 0;
            ListIdSo = string.Join(".", dbitem.Select(d => d.Id.ToString()).ToList());
            Rute = "";
        }

        public AdminUangJalanIndex(Context.SalesOrderKontrakListSo dbitem)
        {
            Context.SalesOrder dbSo = context.SalesOrder.Where(d => d.SalesOrderKontrakId == dbitem.SalesKontrakId).FirstOrDefault();
            IdSalesOrder = dbSo.Id;
            Status = dbitem.Status == "dispatched" || dbitem.Status == "settlement" ? "Sudah" : "Belum";
            JenisOrder = "Kontrak";
            IdChild = dbitem.SalesKontrakId.Value;
            NoSo = dbitem.NoSo;
            KodeNama = dbitem.SalesOrderKontrak.Customer.CustomerCodeOld;
            Customer = dbitem.SalesOrderKontrak.Customer.CustomerNama;
            VehicleNo = dbitem.DataTruck.VehicleNo;
            JenisTruk = dbitem.DataTruck.JenisTrucks.StrJenisTruck;
            IDDriver = dbitem.Driver1.KodeDriver;
            IDDriverOld = dbitem.Driver1.KodeDriverOld;
            Driver = dbitem.Driver1.NamaDriver;
            TanggalMuat = dbitem.MuatDate;
            JamMuat = dbitem.SalesOrderKontrak.JamMuat;
            JumlahRit = 0;
            ListIdSo = dbitem.Id.ToString();
            Rute = "";
        }

        public AdminUangJalanIndex(Context.SalesOrder dbSo, List<Context.SalesOrderKontrakListSo> dbitem, Context.AdminUangJalan dbauj)
        {
            IdSalesOrder = dbSo.Id;
            Status = dbitem.FirstOrDefault().Status == "dispatched" || dbitem.FirstOrDefault().Status == "settlement" ? "Sudah" : "Belum";
            JenisOrder = "Kontrak";
            IdChild = dbSo.SalesOrderKontrakId.Value;
            NoSo = string.Join(", ", dbitem.Select(s => s.NoSo).ToList());
            KodeNama = dbSo.SalesOrderKontrak.Customer.CustomerCodeOld;
            Customer = dbSo.SalesOrderKontrak.Customer.CustomerNama;
            VehicleNo = dbitem.FirstOrDefault().DataTruck.VehicleNo;
            JenisTruk = dbitem.FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck;
            IDDriver = dbauj.Driver1.KodeDriver;
            IDDriverOld = dbauj.Driver1.KodeDriverOld;
            Driver = dbauj.Driver1.NamaDriver;
            TanggalMuat = dbitem.FirstOrDefault().MuatDate;
            JamMuat = dbSo.SalesOrderKontrak.JamMuat;
            JumlahRit = 0;
            ListIdSo = string.Join(".", dbitem.Select(d => d.Id.ToString()).ToList());
            ModifiedDate = dbSo.DateStatus;
            Rute = "";
        }

        public AdminUangJalanIndex(Context.SalesOrder dbitem, Context.AdminUangJalan dbauj)
        {
            Id = dbauj.Id;
            IdSalesOrder = dbitem.Id;
            Status = dbitem.Status == "dispatched" || dbitem.Status == "settlement" || dbitem.Status == "admin uang jalan" ? "Sudah" : "Belum";
            if (dbitem.AdminUangJalanId.HasValue)
            {
                if (dbitem.AdminUangJalan.Removal.Count > 0)
                    Status = "Removal";
            }

            ModifiedDate = dbitem.DateStatus;
            RuteRevised = dbitem.RuteRevised;
            KeteranganAdmin = dbauj == null ? "" : dbauj.KeteranganAdmin;
            if (dbauj != null)
            {
                IDDriver = dbauj.Driver1 == null ? "" : dbauj.Driver1.KodeDriver;
                Driver = dbauj.Driver1 == null ? "" : dbauj.Driver1.NamaDriver;
            }
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                JenisOrder = "Oncall";
                IdChild = dbitem.SalesOrderOncallId.Value;
                NoSo = dbitem.SalesOrderOncall.SONumber;
                KodeNama = dbitem.SalesOrderOncall.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrderOncall.Customer.CustomerNama;
                VehicleNo = dbauj != null && dbauj.DataTruck != null ? dbauj.DataTruck.VehicleNo : dbitem.SalesOrderOncall.DataTruck == null ? "" : dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                if (dbitem.SalesOrderOncall.DataTruck != null && dbitem.SalesOrderOncall.DataTruck.JenisTrucks != null)
                JenisTruk = dbitem.SalesOrderOncall.DataTruck == null ? "" : dbitem.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck;
                IDDriverOld = dbitem.SalesOrderOncall.Driver1 == null ? "" : dbitem.SalesOrderOncall.Driver1.KodeDriverOld;
                Rute = dbitem.SalesOrderOncall.StrDaftarHargaItem;
                TanggalMuat = dbitem.SalesOrderOncall.TanggalMuat;
                JamMuat = dbitem.SalesOrderOncall.JamMuat;
                JumlahRit = 0;
                KeteranganPenggatian = dbitem.SalesOrderOncall.KeteranganDriver1;
                Keterangan = dbitem.SalesOrderOncall.Keterangan;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                JenisOrder = "Pickup";
                IdChild = dbitem.SalesOrderPickupId.Value;
                NoSo = dbitem.SalesOrderPickup.SONumber;
                KodeNama = dbitem.SalesOrderPickup.Customer.CustomerCodeOld;
                Customer = dbitem.SalesOrderPickup.Customer.CustomerNama;
                VehicleNo = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
                JenisTruk = dbitem.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck;
                IDDriverOld = dbitem.SalesOrderPickup.Driver1.KodeDriverOld;
                Rute = dbitem.SalesOrderPickup.Rute.Nama;
                TanggalMuat = dbitem.SalesOrderPickup.TanggalPickup;
                JamMuat = dbitem.SalesOrderPickup.JamPickup;
                Keterangan = dbitem.SalesOrderPickup.Keterangan;
                JumlahRit = 0;
                KeteranganPenggatian = dbitem.SalesOrderPickup.KeteranganDriver1;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                JenisOrder = "Konsolidasi";
                IdChild = dbitem.SalesOrderProsesKonsolidasiId.Value;
                NoSo = dbitem.SalesOrderProsesKonsolidasi.SONumber;
                Customer = "";
                VehicleNo = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                JenisTruk = dbitem.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck;
                IDDriverOld = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriverOld;
                Rute = dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                TanggalMuat = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat;
                JamMuat = dbitem.SalesOrderProsesKonsolidasi.JamMuat;
                Keterangan = dbitem.SalesOrderProsesKonsolidasi.Keterangan;
                JumlahRit = 0;
                KeteranganPenggatian = dbitem.SalesOrderProsesKonsolidasi.KeteranganDriver1;
            }
        }

    }
}