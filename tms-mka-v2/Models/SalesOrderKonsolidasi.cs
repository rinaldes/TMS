using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class SalesOrderKonsolidasi
    {
        private ContextModel context = new ContextModel();
        public int? SalesOrderId { get; set; }
        public int SalesOrderKonsolidasiId { get; set; }
        [Display(Name = "NO SO")]
        public string SONumber { get; set; }
        public string IKSNo { get; set; }
        public string DN { get; set; }
        public int Urutan { get; set; }
        [Display(Name = "Tanggal Order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime? TanggalOrder { get; set; }
        [Display(Name = "Jam Order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamOrder { get; set; }
        [Display(Name = "Tanggal Masuk")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime? TanggalMasuk { get; set; }
        public System.DateTime? TanggalProses { get; set; }
        [Display(Name = "Jam Masuk")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamMasuk { get; set; }
        [Display(Name = "Order Number")]
        public string SONumberCust { get; set; }
        [Display(Name = "Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? CustomerId { get; set; }
        public string KodeCustomer { get; set; }
        public string KodeNama { get; set; }
        public string NamaCustomer { get; set; }
        public bool IsOld { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        [Display(Name = "Tagihan Atas Nama")]
        public int? CustomerTagihanId { get; set; }
        public string KodeCustomerTagihan { get; set; }
        public string NamaCustomerTagihan { get; set; }
        [Display(Name = "Keterangan Konsolidasi")]
        public string Keterangan { get; set; }
        [Display(Name = "Jenis Barang")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? ProductId { get; set; }
        public string StrProduct { get; set; }
        [Display(Name = "Suhu")]
        public decimal? Suhu { get; set; }
        [Display(Name = "Rute")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? RuteId { get; set; }
        public string Rute { get; set; }
        [Display(Name = "Type")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string TypeKonsolidasi { get; set; }
        [Display(Name = "Tonase")]
        public decimal? Tonase { get; set; }
        [Display(Name = "Karton")]
        public decimal? karton { get; set; }
        [Display(Name = "Pallet")]
        public decimal? Pallet { get; set; }
        [Display(Name = "Container")]
        public decimal? Container { get; set; }
        [Display(Name = "m3")]
        public decimal? m3 { get; set; }
        [Display(Name = "Minimum Berat")]
        public bool isMinimumBerat { get; set; }
        public int? MinimumId { get; set; }
        [Display(Name = "Dasar Perhitungan")]
        public string PerhitunganDasar { get; set; }
        [Display(Name = "Harga")]
        public int? Harga { get; set; }
        [Display(Name = "Total Harga")]
        public int? TotalHarga { get; set; }
        [Display(Name = "Cara Bayar")]
        public string CaraBayar { get; set; }
        public string Status { get; set; }
        public bool IsSelect { get; set; }
        public DateTime? DateStatus { get; set; }
        
        public SalesOrderKonsolidasi()
        {

        }

        public SalesOrderKonsolidasi(Context.SalesOrder dbitem)
        {
            SalesOrderId = dbitem.Id;
            SalesOrderKonsolidasiId = dbitem.SalesOrderKonsolidasi.SalesOrderKonsolidasiId;
            SONumber = dbitem.SalesOrderKonsolidasi.SONumber;
            IKSNo = dbitem.SalesOrderKonsolidasi.IKSNo;
            DN = dbitem.SalesOrderKonsolidasi.DN;
            Urutan = dbitem.SalesOrderKonsolidasi.Urutan;
            TanggalOrder = dbitem.SalesOrderKonsolidasi.TanggalOrder;
            TanggalMasuk = dbitem.SalesOrderKonsolidasi.TanggalMasuk;
            TanggalProses = dbitem.SalesOrderKonsolidasi.TanggalProses;
            SONumberCust = dbitem.SalesOrderKonsolidasi.SONumberCust;
            CustomerId = dbitem.SalesOrderKonsolidasi.CustomerId;
            SupplierId = dbitem.SalesOrderKonsolidasi.SupplierId;
            Context.Customer supp = context.Customer.Where(d => d.Id == SupplierId).FirstOrDefault();
            SupplierName = (supp != null) ? supp.CustomerNama : "";

            if (dbitem.SalesOrderKonsolidasi.NamaTagihanId.HasValue)
            {
                CustomerTagihanId = dbitem.SalesOrderKonsolidasi.NamaTagihanId;
                KodeCustomerTagihan = dbitem.SalesOrderKonsolidasi.CustomerTagihan.CustomerCode;
                NamaCustomerTagihan = dbitem.SalesOrderKonsolidasi.CustomerTagihan.CustomerNama;            
            }
            
            Keterangan = dbitem.SalesOrderKonsolidasi.Keterangan;
            ProductId = dbitem.SalesOrderKonsolidasi.ProductId;
            if (dbitem.SalesOrderKonsolidasi.MasterProduct != null)
            {
                StrProduct = dbitem.SalesOrderKonsolidasi.MasterProduct == null ? "" : dbitem.SalesOrderKonsolidasi.MasterProduct.NamaProduk;
                Suhu = int.Parse(dbitem.SalesOrderKonsolidasi.MasterProduct.TargetSuhu.ToString());
            }
            RuteId = dbitem.SalesOrderKonsolidasi.IdDaftarHargaItem;
            Rute = dbitem.SalesOrderKonsolidasi.StrDaftarHargaItem;
            TypeKonsolidasi = dbitem.SalesOrderKonsolidasi.TypeKonsolidasi;
            Tonase = dbitem.SalesOrderKonsolidasi.Tonase;
            karton = dbitem.SalesOrderKonsolidasi.karton;
            Container = dbitem.SalesOrderKonsolidasi.Container;
            m3 = dbitem.SalesOrderKonsolidasi.m3;
            Pallet = dbitem.SalesOrderKonsolidasi.Pallet;
            isMinimumBerat = dbitem.SalesOrderKonsolidasi.isMinimumBerat;
            MinimumId = dbitem.SalesOrderKonsolidasi.MinimumId;
            PerhitunganDasar = dbitem.SalesOrderKonsolidasi.PerhitunganDasar;
            Harga = dbitem.SalesOrderKonsolidasi.Harga;
            TotalHarga = dbitem.SalesOrderKonsolidasi.TotalHarga;
            CaraBayar = dbitem.SalesOrderKonsolidasi.CaraBayar;
            Status = dbitem.Status;
            DateStatus = dbitem.DateStatus;
            IsSelect = dbitem.SalesOrderKonsolidasi.IsSelect;
        }

        public SalesOrderKonsolidasi(Context.SalesOrderKonsolidasi dbitem)
        {
            SalesOrderKonsolidasiId = dbitem.SalesOrderKonsolidasiId;
            SONumber = dbitem.SONumber;
            IKSNo = dbitem.IKSNo;
            DN = dbitem.DN;
            Urutan = dbitem.Urutan;
            TanggalOrder = dbitem.TanggalOrder;
            TanggalMasuk = dbitem.TanggalMasuk;
            SONumberCust = dbitem.SONumberCust;
            CustomerId = dbitem.CustomerId;
//            KodeCustomer = dbitem.Customer.CustomerCode;
            NamaCustomer = context.LookupCode.Where(d => d.Id == dbitem.CustomerId).FirstOrDefault().Nama;

            SupplierId = dbitem.SupplierId;
            Context.Customer supp = context.Customer.Where(d => d.Id == SupplierId).FirstOrDefault();
            if (supp != null)
            {
                SupplierName = (supp != null) ? supp.CustomerNama : "";
                KodeNama = supp.CustomerCodeOld;
            }

            if (dbitem.NamaTagihanId.HasValue)
            {
                CustomerTagihanId = dbitem.NamaTagihanId;
                KodeCustomerTagihan = dbitem.CustomerTagihan.CustomerCode;
                NamaCustomerTagihan = dbitem.CustomerTagihan.CustomerNama;
            }

            Keterangan = dbitem.Keterangan;
            ProductId = dbitem.ProductId;
            if (dbitem.MasterProduct != null)
            {
                StrProduct = dbitem.MasterProduct.NamaProduk;
                Suhu = int.Parse(dbitem.MasterProduct.TargetSuhu.ToString());
            }
            RuteId = dbitem.IdDaftarHargaItem;
            Rute = dbitem.StrDaftarHargaItem;
            TypeKonsolidasi = dbitem.TypeKonsolidasi;
            Tonase = dbitem.Tonase;
            karton = dbitem.karton;
            Container = dbitem.Container;
            m3 = dbitem.m3;
            Pallet = dbitem.Pallet;
            isMinimumBerat = dbitem.isMinimumBerat;
            MinimumId = dbitem.MinimumId;
            PerhitunganDasar = dbitem.PerhitunganDasar;
            Harga = dbitem.Harga;
            TotalHarga = dbitem.TotalHarga;
            CaraBayar = dbitem.CaraBayar;
            IsSelect = dbitem.IsSelect;
        }

        public SalesOrderKonsolidasi(Context.SalesOrder dbitem, Context.Customer supp=null)
        {
            SalesOrderId = dbitem.Id;
            SalesOrderKonsolidasiId = dbitem.SalesOrderKonsolidasi.SalesOrderKonsolidasiId;
            SONumber = dbitem.SalesOrderKonsolidasi.SONumber;
            IKSNo = dbitem.SalesOrderKonsolidasi.IKSNo;
            DN = dbitem.SalesOrderKonsolidasi.DN;
            Urutan = dbitem.SalesOrderKonsolidasi.Urutan;
            TanggalOrder = dbitem.SalesOrderKonsolidasi.TanggalOrder;
            TanggalMasuk = dbitem.SalesOrderKonsolidasi.TanggalMasuk;
            SONumberCust = dbitem.SalesOrderKonsolidasi.SONumberCust;
            CustomerId = dbitem.SalesOrderKonsolidasi.CustomerId;

            SupplierId = dbitem.SalesOrderKonsolidasi.SupplierId;
            SupplierName = (supp != null) ? supp.CustomerNama : "";

            if (dbitem.SalesOrderKonsolidasi.NamaTagihanId.HasValue)
            {
                CustomerTagihanId = dbitem.SalesOrderKonsolidasi.NamaTagihanId;
                KodeCustomerTagihan = dbitem.SalesOrderKonsolidasi.CustomerTagihan.CustomerCode;
                NamaCustomerTagihan = dbitem.SalesOrderKonsolidasi.CustomerTagihan.CustomerNama;            
            }
            
            Keterangan = dbitem.SalesOrderKonsolidasi.Keterangan;
            ProductId = dbitem.SalesOrderKonsolidasi.ProductId;
            if (dbitem.SalesOrderKonsolidasi.MasterProduct != null)
            {
                StrProduct = dbitem.SalesOrderKonsolidasi.MasterProduct.NamaProduk;
                Suhu = int.Parse(dbitem.SalesOrderKonsolidasi.MasterProduct.TargetSuhu.ToString());
            }
            RuteId = dbitem.SalesOrderKonsolidasi.IdDaftarHargaItem;
            Rute = dbitem.SalesOrderKonsolidasi.StrDaftarHargaItem;
            TypeKonsolidasi = dbitem.SalesOrderKonsolidasi.TypeKonsolidasi;
            Tonase = dbitem.SalesOrderKonsolidasi.Tonase;
            karton = dbitem.SalesOrderKonsolidasi.karton;
            Container = dbitem.SalesOrderKonsolidasi.Container;
            m3 = dbitem.SalesOrderKonsolidasi.m3;
            Pallet = dbitem.SalesOrderKonsolidasi.Pallet;
            isMinimumBerat = dbitem.SalesOrderKonsolidasi.isMinimumBerat;
            MinimumId = dbitem.SalesOrderKonsolidasi.MinimumId;
            PerhitunganDasar = dbitem.SalesOrderKonsolidasi.PerhitunganDasar;
            Harga = dbitem.SalesOrderKonsolidasi.Harga;
            TotalHarga = dbitem.SalesOrderKonsolidasi.TotalHarga;
            CaraBayar = dbitem.SalesOrderKonsolidasi.CaraBayar;
            Status = dbitem.Status;
            DateStatus = dbitem.DateStatus;
            IsSelect = dbitem.SalesOrderKonsolidasi.IsSelect;
        }

        public void setDb(Context.SalesOrderKonsolidasi dbitem)
        {
            dbitem.SalesOrderKonsolidasiId = SalesOrderKonsolidasiId;

            dbitem.TanggalOrder = TanggalOrder.Value;
            dbitem.TanggalMasuk = TanggalMasuk.Value;
            dbitem.JamOrder = JamOrder;
            dbitem.JamMasuk = JamMasuk;
            dbitem.SONumber = SONumber;
            dbitem.SONumberCust = SONumberCust;
            dbitem.CustomerId = CustomerId;
            dbitem.SupplierId = SupplierId;
            dbitem.NamaTagihanId = CustomerTagihanId;
            dbitem.Keterangan = Keterangan;
            dbitem.ProductId = ProductId;
            dbitem.IdDaftarHargaItem = RuteId;
            dbitem.StrDaftarHargaItem = Rute;
            dbitem.Keterangan = Keterangan;
            dbitem.TypeKonsolidasi = TypeKonsolidasi;
            dbitem.Tonase = Tonase;
            dbitem.karton = karton;
            dbitem.Pallet = Pallet;
            dbitem.Container = Container;
            dbitem.m3 = m3;
            dbitem.PerhitunganDasar = PerhitunganDasar;
            dbitem.isMinimumBerat = isMinimumBerat;
            dbitem.MinimumId = MinimumId;
            dbitem.Harga = Harga;
            dbitem.TotalHarga = TotalHarga;
            dbitem.CaraBayar = CaraBayar;
        }
    }
}