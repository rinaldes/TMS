using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class ListOrder
    {
        #region initial setting

        public int Id { get; set; }
        public int IdChild { get; set; }
        public int IdSoKontrak { get; set; }
        public string DN { get; set; }
        public string JenisOrder { get; set; }
        public string SONumber { get; set; }
        public string NoShipment { get; set; }
        public DateTime TanggalOrder { get; set; }
        public TimeSpan JamOrder { get; set; }
        public string TanggalMuat { get; set; }
        public TimeSpan JamMuat { get; set; }
        public int? CustomerId { get; set; }
        public string KodeNama { get; set; }
        public string NamaCustomer { get; set; }
        public int? ProductId { get; set; }
        public string StrProduct { get; set; }
        public decimal? Suhu { get; set; }
        public int? RuteId { get; set; }
        public string Rute { get; set; }
        public string StrMultidrop { get; set; }
        public int? JenisTruckId { get; set; }
        public string StrJenisTruck { get; set; }
        public string MktJenisTruck { get; set; }
        public string JenisPendingin { get; set; }
        public string StatusFlow { get; set; }
        public DateTime? FlowDate { get; set; }
        public string StatusBatal { get; set; }
        public string StatusDokumen { get; set; }
        public string PenanganKhusus { get; set; }
        public string Status { get; set; }
        public string TypeSo { get; set; }
        public string VehicleNo { get; set; }
        public string Driver1 { get; set; }
        public string Keterangan { get; set; }
        public string KeteranganBatal { get; set; }
        public bool IsReturn { get; set; }
        public bool IsBatalTruk { get; set; }
        public bool DateRevised { get; set; }
        public bool RuteRevised { get; set; }
        public string name { get; set; }
        public double y { get; set; }

        #endregion initial setting

        #region initial value

        public ListOrder(Context.SalesOrder dbitem)
        {
            Id = dbitem.Id;
            Status = dbitem.Status;
            StatusFlow = dbitem.StatusFlow;
            FlowDate = dbitem.DateStatus;
            IsReturn = dbitem.isReturn;
            IsBatalTruk = dbitem.IsBatalTruk;
            DateRevised = dbitem.DateRevised;
            RuteRevised = dbitem.RuteRevised;
            StatusBatal = null;
            StatusDokumen = null;
            NoShipment = dbitem.NoShipment;
            try
            {
                JenisPendingin = dbitem.DataTruck.DataPendingin.FirstOrDefault().Merk;
            }
            catch (Exception) { }
            if (dbitem.SalesOrderOncallId != null)
            {
                IdChild = dbitem.SalesOrderOncallId.Value;
                JenisOrder = dbitem.SalesOrderOncall.PrioritasId.HasValue && dbitem.SalesOrderOncall.LookUpPrioritas.Nama != null ? dbitem.SalesOrderOncall.LookUpPrioritas.Nama : "On Call";
                TypeSo = dbitem.SalesOrderOncall.PrioritasId.HasValue && dbitem.SalesOrderOncall.LookUpPrioritas.Nama != null ? dbitem.SalesOrderOncall.LookUpPrioritas.Nama : "On Call";
                DN = dbitem.SalesOrderOncall.DN;
                SONumber = dbitem.SalesOrderOncall.SONumber;
                TanggalOrder = dbitem.SalesOrderOncall.TanggalOrder;
                JamOrder = dbitem.SalesOrderOncall.JamOrder;
                TanggalMuat = dbitem.SalesOrderOncall.TanggalMuat.Value.ToString();
                JamMuat = dbitem.SalesOrderOncall.JamMuat;
                CustomerId = dbitem.SalesOrderOncall.CustomerId;
                KodeNama = dbitem.SalesOrderOncall.Customer.CustomerCodeOld;
                NamaCustomer = dbitem.SalesOrderOncall.Customer.CustomerNama;
                ProductId = dbitem.SalesOrderOncall.ProductId;
                StrProduct = dbitem.SalesOrderOncall.MasterProduct.NamaProduk;
                Suhu = dbitem.SalesOrderOncall.MasterProduct.TargetSuhu;
                RuteId = dbitem.SalesOrderOncall.IdDaftarHargaItem;
                Rute = dbitem.SalesOrderOncall.StrDaftarHargaItem;
                StrMultidrop = dbitem.SalesOrderOncall.StrMultidrop;
                JenisTruckId = dbitem.DataTruckId.HasValue ? dbitem.DataTruck.IdJenisTruck : dbitem.SalesOrderOncall.JenisTruckId;
                VehicleNo = dbitem.DataTruckId.HasValue ? dbitem.DataTruck.VehicleNo : "";
                Driver1 = dbitem.DriverId.HasValue ? dbitem.Driver.KodeDriver + " - " + dbitem.Driver.NamaDriver : "";
                Keterangan = dbitem.SalesOrderOncall.Keterangan;
                PenanganKhusus = dbitem.SalesOrderOncall.Customer.SpecialTreatment + ", " +
                   string.Join(", ", dbitem.SalesOrderOncall.Customer.CustomerProductType.Where(d => d.idProduk == ProductId).Select(d => d.PenangananKhusus).ToList());
                MktJenisTruck = dbitem.SalesOrderOncall.JenisTrucks.StrJenisTruck;
            }
            else if (dbitem.SalesOrderKontrakId != null)
            {
                IdChild = dbitem.SalesOrderKontrakId.Value;
                JenisOrder = "Kontrak";
                DN = null;
                SONumber = dbitem.SalesOrderKontrak.SONumber;
                JamMuat = dbitem.SalesOrderKontrak.JamMuat;
                CustomerId = dbitem.SalesOrderKontrak.CustomerId;
                NamaCustomer = dbitem.SalesOrderKontrak.Customer.CustomerNama;
                if (dbitem.SalesOrderKontrak.ProductId.HasValue)
                {
                    ProductId = dbitem.SalesOrderKontrak.ProductId;
                    StrProduct = dbitem.SalesOrderKontrak.MasterProduct.NamaProduk;
                    Suhu = dbitem.SalesOrderKontrak.MasterProduct.TargetSuhu;
                }
                RuteId = null;
                Rute = "";
                StrMultidrop = null;
                JenisTruckId = dbitem.SalesOrderKontrak.JenisTruckId;
                PenanganKhusus = null;

                List<string> strDate = new List<string>();
                foreach (Context.SalesOrderKontrakDetail item in dbitem.SalesOrderKontrak.SalesOrderKontrakDetail)
                {
                    strDate.Add(item.MuatDate.ToString());
                }

                TanggalMuat = string.Join("|", strDate);
            }
            else if (dbitem.SalesOrderProsesKonsolidasi != null)
            {
                IdChild = dbitem.SalesOrderProsesKonsolidasiId.Value;
                JenisOrder = "Konsolidassi";
                TypeSo = "Konsolidasi";
                DN = dbitem.SalesOrderProsesKonsolidasi.DN;
                SONumber = dbitem.SalesOrderProsesKonsolidasi.SONumber;
                TanggalMuat = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat.Value.ToString();
                JamMuat = dbitem.SalesOrderProsesKonsolidasi.JamMuat;
                CustomerId = null;
                NamaCustomer = null;
                ProductId = null;
                StrProduct = null;
                Suhu = null;
                RuteId = dbitem.SalesOrderProsesKonsolidasi.IdDaftarHargaItem;
                Rute = dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
                StrMultidrop = dbitem.SalesOrderProsesKonsolidasi.Multidrop;
                JenisTruckId = dbitem.SalesOrderProsesKonsolidasi.JenisTruckId;
                VehicleNo = dbitem.SalesOrderProsesKonsolidasi.IdDataTruck.HasValue ? dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo : "";
                Driver1 = dbitem.SalesOrderProsesKonsolidasi.Driver1Id.HasValue ? dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver + " - " + dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver : "";
                PenanganKhusus = null;
                MktJenisTruck = dbitem.SalesOrderProsesKonsolidasi.JenisTrucks.StrJenisTruck;
                Keterangan = dbitem.SalesOrderProsesKonsolidasi.Keterangan;
            }
            else if (dbitem.SalesOrderPickupId != null)
            {
                IdChild = dbitem.SalesOrderPickupId.Value;
                JenisOrder = "Pickup";
                TypeSo = "Pickup";
                TanggalOrder = dbitem.SalesOrderPickup.TanggalOrder;
                DN = dbitem.SalesOrderPickup.SONumber;
                SONumber = dbitem.SalesOrderPickup.SONumber;
                TanggalMuat = dbitem.SalesOrderPickup.TanggalPickup.ToString();
                JamMuat = dbitem.SalesOrderPickup.JamPickup;
                CustomerId = dbitem.SalesOrderPickup.CustomerId;
                NamaCustomer = dbitem.SalesOrderPickup.Customer.CustomerNama;
                ProductId = dbitem.SalesOrderPickup.ProductId;
                StrProduct = dbitem.SalesOrderPickup.MasterProduct.NamaProduk;
                Suhu = dbitem.SalesOrderPickup.MasterProduct.TargetSuhu;
                RuteId = dbitem.SalesOrderPickup.RuteId;
                Rute = dbitem.SalesOrderPickup.Rute.Nama;
                KodeNama = dbitem.SalesOrderPickup.Customer.CustomerCodeOld;
                StrMultidrop = dbitem.SalesOrderPickup.StrMultidrop;
                JenisTruckId = dbitem.SalesOrderPickup.IdDataTruck.HasValue ? dbitem.SalesOrderPickup.DataTruck.IdJenisTruck : dbitem.SalesOrderPickup.JenisTruckId;
                VehicleNo = dbitem.SalesOrderPickup.IdDataTruck.HasValue ? dbitem.SalesOrderPickup.DataTruck.VehicleNo : "";
                Driver1 = dbitem.SalesOrderPickup.Driver1Id.HasValue ? dbitem.SalesOrderPickup.Driver1.KodeDriver + " - " + dbitem.SalesOrderPickup.Driver1.NamaDriver : "";
                PenanganKhusus = dbitem.SalesOrderPickup.Customer.SpecialTreatment + ", " +
                   string.Join(", ", dbitem.SalesOrderPickup.Customer.CustomerProductType.Where(d => d.idProduk == ProductId).Select(d => d.PenangananKhusus).ToList());
                Keterangan = dbitem.SalesOrderPickup.Keterangan;
                MktJenisTruck = dbitem.SalesOrderPickup.JenisTrucks.StrJenisTruck;
            }
            else
                JenisOrder = "duka";
            try
            {
                StrJenisTruck = dbitem.DataTruck.JenisTrucks.StrJenisTruck;
            }
            catch (Exception) {
                StrJenisTruck = "";
            }
            try
            {
                MktJenisTruck = dbitem.JenisTruck.StrJenisTruck;
            }
            catch (Exception) { }
            KeteranganBatal = dbitem.KeteranganBatal;
        }

        public ListOrder(string penghibur1, double penghibur2, string penghibur3)
        {
            name = penghibur1;
            y = penghibur2;
        }

        public ListOrder(Context.SalesOrder dbso, Context.SalesOrderKontrakListSo dbitem)
        {
            Id = dbso.Id;
            Status = dbitem.Status;
            if (dbitem.Status != null){
                if (dbitem.Status.ToLower().Contains("konfirmasi"))
                {
                    StatusFlow = "KONFIRMASI";
                }
                else {
                    StatusFlow = dbitem.Status.ToLower().ToUpper();
                }
            }
            if (dbitem.Status == "draft")
            {
                StatusFlow = "MARKETING";
            }
            else if (dbitem.Status.ToLower().Contains("save konfirmasi"))
            {
                StatusFlow = "ADMIN UANG JALAN";
            }
            else if (dbitem.Status.ToLower().Contains("admin uang jalan"))
            {
                StatusFlow = "KASIR";
            }
            else if (dbitem.Status.ToLower().Contains("konfirmasi") || dbitem.Status == "save planning")
            {
                StatusFlow = "KONFIRMASI";
            }
            else if (dbitem.Status.ToLower().Contains("planning") || dbitem.Status == "save")
            {
                StatusFlow = "PLANNING";
            }
            else if (dbitem.Status.ToLower().Contains("dispatched"))
            {
                if (dbitem.AdminUangJalan != null)
                {
                    foreach (Context.AdminUangJalanUangTf aujtf in dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(n => n.isTf == true).OrderByDescending(x => x.TanggalAktual))
                    {
                        StatusFlow = aujtf.Keterangan == "Tunai" ? "CASH" : "TRANSFER";
                        break;
                    }
                }
                else
                {
                    StatusFlow = "CASH";
                }
            }
            else
            {
                StatusFlow = dbitem.Status.ToUpper();
            }
            FlowDate = dbso.DateStatus;
            IsBatalTruk = dbitem.IsBatalTruck;
            IdChild = dbso.SalesOrderKontrakId.Value;
            IdSoKontrak = dbitem.Id; 
            JenisOrder = "Kontrak";
            TypeSo = "Kontrak";
            DN = null;
            SONumber = dbitem.NoSo;
            JamMuat = dbso.SalesOrderKontrak.JamMuat;
            CustomerId = dbso.SalesOrderKontrak.CustomerId;
            NamaCustomer = dbso.SalesOrderKontrak.Customer.CustomerNama;
            Driver1 = dbitem.Driver1 == null ? "" : dbitem.Driver1.KodeDriver + " - " + dbitem.Driver1.NamaDriver;
            VehicleNo = dbitem.DataTruck == null ? "" : dbitem.DataTruck.VehicleNo;
            if (dbso.SalesOrderKontrak.ProductId.HasValue)
            {
                ProductId = dbso.SalesOrderKontrak.ProductId;
                StrProduct = dbso.SalesOrderKontrak.MasterProduct.NamaProduk;
                Suhu = dbso.SalesOrderKontrak.MasterProduct.TargetSuhu;
            }
            RuteId = null;
            Rute = "";
            StrMultidrop = null;
            if (dbso.SalesOrderKontrakId.HasValue)
            {
                JenisTruckId = dbso.SalesOrderKontrak.JenisTrucks.Id;
                StrJenisTruck = dbso.SalesOrderKontrak.JenisTrucks.StrJenisTruck;
            }
            else{
                JenisTruckId = dbso.SalesOrderOncall.JenisTruckId;
                StrJenisTruck = dbso.SalesOrderOncall.JenisTrucks.StrJenisTruck;
            }
            PenanganKhusus = null;
            List<string> strDate = new List<string>();
            strDate.Add(dbitem.MuatDate.ToString());

            TanggalMuat = string.Join("|", strDate);
        }
        #endregion initial value

        public ListOrder(Context.SalesOrderKontrakListSo dbitem)
        {
            Id = dbitem.Id;
            if (dbitem.Status == null)
                StatusFlow = "PLANNING";
            else {
                if (dbitem.Status.ToLower().Contains("konfirmasi"))
                    StatusFlow = "KONFIRMASI";
                else
                    StatusFlow = dbitem.Status.ToLower().ToUpper();
            }
            StatusFlow = dbitem.StatusFlow;
            IsBatalTruk = dbitem.IsBatalTruck;
            IdSoKontrak = dbitem.SalesOrderKontrak.SalesOrderKontrakId;
            JenisOrder = "Kontrak";
            TypeSo = "Kontrak";
            DN = "DN-" + dbitem.NoSo;
            SONumber = dbitem.NoSo;
            JamMuat = dbitem.SalesOrderKontrak.JamMuat;
            CustomerId = dbitem.SalesOrderKontrak.CustomerId;
            KodeNama = dbitem.SalesOrderKontrak.Customer.CustomerCodeOld;
            NamaCustomer = dbitem.SalesOrderKontrak.Customer.CustomerNama;
            Driver1 = dbitem.Driver1 == null ? "" : dbitem.Driver1.KodeDriver + " - " + dbitem.Driver1.NamaDriver;
            VehicleNo = dbitem.DataTruck == null ? "" : dbitem.DataTruck.VehicleNo;
            ProductId = dbitem.SalesOrderKontrak.ProductId;
            if (dbitem.SalesOrderKontrak.MasterProduct != null)
            {
                StrProduct = dbitem.SalesOrderKontrak.MasterProduct.NamaProduk;
                Suhu = dbitem.SalesOrderKontrak.MasterProduct.TargetSuhu;
            }
            RuteId = null;
            Rute = "";
            StrMultidrop = null;
            JenisTruckId = dbitem.SalesOrderKontrak.JenisTrucks.Id;
            StrJenisTruck = dbitem.SalesOrderKontrak.JenisTrucks.StrJenisTruck;
            PenanganKhusus = null;
            List<string> strDate = new List<string>();
            strDate.Add(dbitem.MuatDate.ToString());
            TanggalMuat = string.Join("|", strDate);
        }
    }
}