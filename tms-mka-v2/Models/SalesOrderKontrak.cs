using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class SalesOrderKontrak
    {
        public int? SalesOrderId { get; set; }
        public int SalesOrderKontrakId { get; set; }
        [Display(Name = "No Kontrak")]
        public string SONumber { get; set; }
        public string DN { get; set; }
        public int Urutan { get; set; }
        [Display(Name = "Kode Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? CustomerId { get; set; }
        [Display(Name = "Kode Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string KodeCustomer { get; set; }
        public string KodeNama { get; set; }
        [Display(Name = "Nama Customer")]
        public string NamaCustomer { get; set; }
        [Display(Name = "Status Kredit")]
        public string StatusKredit { get; set; }
        [Display(Name = "Jenis Truk")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? JenisTruckId { get; set; }
        public string StrJenisTruck { get; set; }
        [Display(Name = "Jenis Barang")]
        public int? ProductId { get; set; }
        public string StrProduct { get; set; }
        [Display(Name = "Suhu")]
        public int? Suhu { get; set; }
        [Display(Name = "Periode")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? PeriodStr { get; set; }
        [Display(Name = "To")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? PeriodEnd { get; set; }
        [Display(Name = "Jam Muat")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan? JamMuat { get; set; }
        [Display(Name = "Jumlah Truk")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? JumlahTruck { get; set; }
        [Display(Name = "Rit")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? Rit { get; set; }
        [Display(Name = "Keterangan")]
        public string Keterangan { get; set; }
        [Display(Name = "Jumlah Hari")]
        public int JumHari { get; set; }
        [Display(Name = "Hari Kerja")]
        public int Kerja { get; set; }
        [Display(Name = "Hari Libur")]
        public int libur { get; set; }
        public string TanggalMuat { get; set; }
        public string JsonDateMuat { get; set; }
        public string existingMuatDate { get; set; }
        public string invalidMuatDate { get; set; }
        public string Status { get; set; }
        public DateTime? DateStatus { get; set; }
        public bool IsReturn { get; set; }
        public bool IsBatalTruk { get; set; }
        public string TanggalMuatBaru { get; set; }
        public string JsonDateMuatBaru { get; set; }
        public List<SalesOrderKontrakListSo> ListModelSOKontrak { get; set; }
        public List<SalesOrderKontrakListSo> ListValueModelSOKontrak { get; set; }
        [Display(Name = "Keterangan Revisi")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string KeteranganRevisi { get; set; }

        public string strListTruck { get; set; }

        public SalesOrderKontrak()
        {
            ListModelSOKontrak = new List<SalesOrderKontrakListSo>();
            ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
        }
        public SalesOrderKontrak(Context.SalesOrder dbitem)
        {
            IsBatalTruk = false;
            SalesOrderId = dbitem.Id;
            SalesOrderKontrakId = dbitem.SalesOrderKontrak.SalesOrderKontrakId;
            SONumber = dbitem.SalesOrderKontrak.SONumber;
            DN = dbitem.SalesOrderKontrak.DN;
            Urutan = dbitem.SalesOrderKontrak.Urutan;
            CustomerId = dbitem.SalesOrderKontrak.CustomerId;
            KodeCustomer = dbitem.SalesOrderKontrak.Customer.CustomerCode;
            KodeNama = dbitem.SalesOrderKontrak.Customer.CustomerCodeOld;
            NamaCustomer = dbitem.SalesOrderKontrak.Customer.CustomerNama;
            JenisTruckId = dbitem.SalesOrderKontrak.JenisTruckId;
            StrJenisTruck = dbitem.SalesOrderKontrak.JenisTruckId == null ? "" : dbitem.SalesOrderKontrak.JenisTrucks.StrJenisTruck;
            ProductId = dbitem.SalesOrderKontrak.ProductId;
            StrProduct = dbitem.SalesOrderKontrak.ProductId == null ? "" : dbitem.SalesOrderKontrak.MasterProduct.NamaProduk;
            Suhu = dbitem.SalesOrderKontrak.ProductId == null ? 0 : int.Parse(dbitem.SalesOrderKontrak.MasterProduct.TargetSuhu.ToString());
            PeriodStr = dbitem.SalesOrderKontrak.PeriodStr;
            PeriodEnd = dbitem.SalesOrderKontrak.PeriodEnd;
            JamMuat = dbitem.SalesOrderKontrak.JamMuat;
            JumlahTruck = dbitem.SalesOrderKontrak.JumlahTruck;
            Rit = dbitem.SalesOrderKontrak.Rit;
            Keterangan = dbitem.SalesOrderKontrak.Keterangan;
            JumHari = dbitem.SalesOrderKontrak.JumlahHari;
            Kerja = dbitem.SalesOrderKontrak.JumlahHariKerja;
            libur = dbitem.SalesOrderKontrak.JumlahHariLibur;
            Status = dbitem.Status;
            DateStatus = dbitem.DateStatus;

            List<string> strDate = new List<string>();
            foreach (Context.SalesOrderKontrakDetail item in dbitem.SalesOrderKontrak.SalesOrderKontrakDetail)
            {
                strDate.Add(item.MuatDate.ToString());
            }
            TanggalMuat = string.Join("|", strDate);
            JsonDateMuat = string.Join("|", strDate);

            ListModelSOKontrak = new List<SalesOrderKontrakListSo>();
            ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
            foreach (Context.SalesOrderKontrakListSo item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo)
            {
                ListModelSOKontrak.Add(new SalesOrderKontrakListSo(item));                
            }
            IsReturn = dbitem.isReturn;
        }

        public SalesOrderKontrak(Context.SalesOrderKontrak dbitem)
        {
            IsBatalTruk = false;
            SalesOrderKontrakId = dbitem.SalesOrderKontrakId;
            SONumber = dbitem.SONumber;
            DN = dbitem.DN;
            Urutan = dbitem.Urutan;
            CustomerId = dbitem.CustomerId;
            KodeCustomer = dbitem.Customer.CustomerCode;
            KodeNama = dbitem.Customer.CustomerCodeOld;
            NamaCustomer = dbitem.Customer.CustomerNama;
            JenisTruckId = dbitem.JenisTruckId;
            StrJenisTruck = dbitem.JenisTruckId == null ? "" : dbitem.JenisTrucks.StrJenisTruck;
            ProductId = dbitem.ProductId;
            StrProduct = dbitem.ProductId == null ? "" : dbitem.MasterProduct.NamaProduk;
            Suhu = dbitem.ProductId == null ? 0 : int.Parse(dbitem.MasterProduct.TargetSuhu.ToString());
            PeriodStr = dbitem.PeriodStr;
            PeriodEnd = dbitem.PeriodEnd;
            JamMuat = dbitem.JamMuat;
            JumlahTruck = dbitem.JumlahTruck;
            Rit = dbitem.Rit;
            Keterangan = dbitem.Keterangan;
            JumHari = dbitem.JumlahHari;
            Kerja = dbitem.JumlahHariKerja;
            libur = dbitem.JumlahHariLibur;

            List<string> strDate = new List<string>();
            foreach (Context.SalesOrderKontrakDetail item in dbitem.SalesOrderKontrakDetail)
            {
                strDate.Add(item.MuatDate.ToString());
            }
            TanggalMuat = string.Join("|", strDate);
            JsonDateMuat = string.Join("|", strDate);

            ListModelSOKontrak = new List<SalesOrderKontrakListSo>();
            ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
            foreach (Context.SalesOrderKontrakListSo item in dbitem.SalesOrderKontrakListSo)
            {
                ListModelSOKontrak.Add(new SalesOrderKontrakListSo(item));
            }
        }

        public SalesOrderKontrak(Context.SalesOrder dbitem, Context.SalesOrderKontrakListSo salesOrderKontrakListSo)
        {
            IsBatalTruk = true;
            SalesOrderId = dbitem.Id;
            SalesOrderKontrakId = dbitem.SalesOrderKontrak.SalesOrderKontrakId;
            SONumber = salesOrderKontrakListSo.NoSo;
            DN = dbitem.SalesOrderKontrak.DN;
            Urutan = dbitem.SalesOrderKontrak.Urutan;
            CustomerId = dbitem.SalesOrderKontrak.CustomerId;
            KodeCustomer = dbitem.SalesOrderKontrak.Customer.CustomerCode;
            KodeNama = dbitem.SalesOrderKontrak.Customer.CustomerCodeOld;
            NamaCustomer = dbitem.SalesOrderKontrak.Customer.CustomerNama;
            JenisTruckId = dbitem.SalesOrderKontrak.JenisTruckId;
            StrJenisTruck = dbitem.SalesOrderKontrak.JenisTruckId == null ? "" : dbitem.SalesOrderKontrak.JenisTrucks.StrJenisTruck;
            ProductId = dbitem.SalesOrderKontrak.ProductId;
            StrProduct = dbitem.SalesOrderKontrak.ProductId == null ? "" : dbitem.SalesOrderKontrak.MasterProduct.NamaProduk;
            Suhu = dbitem.SalesOrderKontrak.ProductId == null ? 0 : int.Parse(dbitem.SalesOrderKontrak.MasterProduct.TargetSuhu.ToString());
            PeriodStr = dbitem.SalesOrderKontrak.PeriodStr;
            PeriodEnd = dbitem.SalesOrderKontrak.PeriodEnd;
            JamMuat = dbitem.SalesOrderKontrak.JamMuat;
            JumlahTruck = 1;
            Rit = dbitem.SalesOrderKontrak.Rit;
            Keterangan = dbitem.SalesOrderKontrak.Keterangan;
            JumHari = dbitem.SalesOrderKontrak.JumlahHari;
            Kerja = dbitem.SalesOrderKontrak.JumlahHariKerja;
            libur = dbitem.SalesOrderKontrak.JumlahHariLibur;
            Status = dbitem.Status;
            DateStatus = dbitem.DateStatus;
            JsonDateMuat = salesOrderKontrakListSo.MuatDate.ToString();

            List<string> strDate = new List<string>();
            foreach (Context.SalesOrderKontrakDetail item in dbitem.SalesOrderKontrak.SalesOrderKontrakDetail)
            {
                strDate.Add(item.MuatDate.ToString());
            }
            TanggalMuat = string.Join("|", strDate);

            ListModelSOKontrak = new List<SalesOrderKontrakListSo>();
            ListValueModelSOKontrak = new List<SalesOrderKontrakListSo>();
            foreach (Context.SalesOrderKontrakListSo item in dbitem.SalesOrderKontrak.SalesOrderKontrakListSo)
            {
                ListModelSOKontrak.Add(new SalesOrderKontrakListSo(item));                
            }
            IsReturn = dbitem.isReturn;
        }
        
        public void setDb(Context.SalesOrderKontrak dbitem)
        {
            dbitem.SalesOrderKontrakId = SalesOrderKontrakId;
            
            dbitem.CustomerId = CustomerId;
            dbitem.JenisTruckId = JenisTruckId;
            dbitem.ProductId = ProductId;
            dbitem.PeriodStr = PeriodStr.Value;
            dbitem.PeriodEnd = PeriodEnd.Value;
            dbitem.JamMuat = JamMuat.Value;
            dbitem.JumlahTruck = JumlahTruck.Value;
            dbitem.Rit = Rit.Value;
            dbitem.Keterangan = Keterangan;
            dbitem.JumlahHari = JumHari;
            dbitem.JumlahHariKerja = Kerja;
            dbitem.JumlahHariLibur = libur;

            dbitem.SalesOrderKontrakDetail.Clear();
            if (JsonDateMuat != null && JsonDateMuat != "")
            {
                foreach (string item in JsonDateMuat.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbitem.SalesOrderKontrakDetail.Add(new Context.SalesOrderKontrakDetail()
                    {
                        MuatDate = DateTime.Parse(item)
                    });
                }
            }
        }

        public void setDbOpertional(Context.SalesOrderKontrak dbitem)
        {
            SalesOrderKontrakItemTruck[] resultLoad = JsonConvert.DeserializeObject<SalesOrderKontrakItemTruck[]>(strListTruck);
            dbitem.SalesOrderKontrakTruck.Clear();
            foreach (SalesOrderKontrakItemTruck item in resultLoad)
            {
                dbitem.SalesOrderKontrakTruck.Add(new SalesOrderKontrakTruck() {
                    DataTruckId = item.IdTruk,
                    IdDriver1 = item.IdDriver1,
                    IdDriver2 = item.IdDriver2 == 0 ? null : item.IdDriver2,
                    StatusTruk = item.Status,
                });
            }
        }
    }
    
    public class SalesOrderKontrakItemTruck
    {
        public int Id { get; set; }
        public int? IdTruk { get; set; }
        public string Status { get; set; }
        public string StatusDriver1 { get; set; }
        public string StatusDriver2 { get; set; }
        public string VehicleNumber { get; set; }
        public string JenisTruck { get; set; }
        public int? IdDriver1 { get; set; }
        public string KodeDriver1 { get; set; }
        public string NamaDriver1 { get; set; }        
        public int? IdDriver2 { get; set; }
        public string KodeDriver2 { get; set; }
        public string NamaDriver2 { get; set; }

        public SalesOrderKontrakItemTruck(){ }
        public SalesOrderKontrakItemTruck(Context.SalesOrderKontrakTruck dbitem) {
            Id = dbitem.Id;
            IdTruk = dbitem.DataTruckId;
            Status = dbitem.StatusTruk;
            VehicleNumber = dbitem.DataTruck.VehicleNo;
            JenisTruck = dbitem.DataTruck.JenisTrucks == null ? "" : dbitem.DataTruck.JenisTrucks.StrJenisTruck;
            IdDriver1 = dbitem.IdDriver1;
            KodeDriver1 = dbitem.Driver1.KodeDriver;
            NamaDriver1 = dbitem.Driver1.NamaDriver;
            StatusDriver1 = dbitem.Driver1.LookupCodeStatus.Nama.ToUpper();
            if (dbitem.IdDriver2 != null)
            {
                IdDriver2 = dbitem.IdDriver2;
                KodeDriver2 = dbitem.Driver2.KodeDriver;
                NamaDriver2 = dbitem.Driver2.NamaDriver;
                StatusDriver2 = dbitem.Driver2.LookupCodeStatus.Nama.ToUpper();
            }
        }

        public SalesOrderKontrakItemTruck(Context.DataTruck dbitem, List<Context.SalesOrder> dbso)
        {
            Id = 1;
            IdTruk = dbitem.Id;
            VehicleNumber = dbitem.VehicleNo;
            JenisTruck = dbitem.JenisTrucks == null ? "" : dbitem.JenisTrucks.StrJenisTruck;
            Status = "Available";
            if (dbso != null)
            {
                if (dbso.Any(d => (d.Status == "save" || d.Status == "draft planning") &&
                    ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderKontrakId.HasValue ? d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.DataTruckId == dbitem.Id) : false))))
                { 
                    Status = "On Duty";
                }
                else if (dbso.Any(d => (d.Status == "save planning" || d.Status == "draft konfirmasi") &&
                    ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderKontrakId.HasValue ? d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.DataTruckId == dbitem.Id) : false))))
                {
                    Status = "On Duty";
                }
                else if (dbso.Any(d => (d.Status == "save konfirmasi" || d.Status == "dispatched") &&
                    ((d.SalesOrderOncallId.HasValue ? d.SalesOrderOncall.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderPickupId.HasValue ? d.SalesOrderPickup.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderProsesKonsolidasiId.HasValue ? d.SalesOrderProsesKonsolidasi.IdDataTruck == dbitem.Id : false) ||
                    (d.SalesOrderKontrakId.HasValue ? d.SalesOrderKontrak.SalesOrderKontrakTruck.Any(k => k.DataTruckId == dbitem.Id) : false))))
                {
                    Status = "On Duty";
                }
            }
        }
    }
    public class SalesOrderKontrakListSo {
        public int Id { get; set; }
        public int? SalesKontrakId { get; set; }
        public string NoSo { get; set; }
        public DateTime MuatDate { get; set; }
        public bool IsProses { get; set; }
        public int? IdDataTruck { get; set; }
        public string Nopol { get; set; }
        public int? IdJenisTruck { get; set; }
        public string NamaJenisTruck { get; set; }
        public int? Driver1Id { get; set; }
        public int? Driver2Id { get; set; }
        public int Urutan { get; set; }
        public string Status { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public SalesOrderKontrakListSo()
        {

        }
        public SalesOrderKontrakListSo(Context.SalesOrderKontrakListSo dbitem)
        {
            Id = dbitem.Id;
            SalesKontrakId = dbitem.SalesKontrakId;
            NoSo = dbitem.NoSo;
            MuatDate = dbitem.MuatDate;
            IsProses = dbitem.IsProses;
            IdDataTruck = dbitem.IdDataTruck;
            if (dbitem.IdDataTruck.HasValue)
            {
                Nopol = dbitem.DataTruck.VehicleNo;
                IdJenisTruck = dbitem.DataTruck.IdJenisTruck;
                NamaJenisTruck = dbitem.DataTruck.JenisTrucks.StrJenisTruck;
            }
            Driver1Id = dbitem.Driver1Id;
            Driver2Id = dbitem.Driver2Id;
            Urutan = dbitem.Urutan;
            Status = dbitem.Status;
            IdAdminUangJalan = dbitem.IdAdminUangJalan;
        }
    }
}