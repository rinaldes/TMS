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
    public class SalesOrderPickup
    {
        public int? SalesOrderId { get; set; }
        public int SalesOrderPickupId { get; set; }
        [Display(Name = "Delivery No")]
        public string SONumber { get; set; }
        public int Urutan { get; set; }
        [Display(Name = "Tanggal Order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime? TanggalOrder { get; set; }
        public TimeSpan JamOrder { get; set; }
        public int? CustomerId { get; set; }
        [Display(Name = "Kode Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string KodeCustomer {get;set;}
        public string KodeNama {get;set;}
        [Display(Name = "Nama Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaCustomer {get;set;}
        [Display(Name = "Status Kredit")]
        public string StatusKredit { get; set; }
        [Display(Name = "Jenis Truk")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? JenisTruckId { get; set; }
        public string StrJenisTruck { get; set; }
        public int? JenisTruckBaruId { get; set; }
        public string StrJenisTruckBaru { get; set; }

        [Display(Name = "Jenis Barang")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? ProductId { get; set; }
        public string StrProduct { get; set; }
        [Display(Name = "Penanganan Khusus")]
        public string PenanganKhusus {get;set;}
        [Display(Name = "Target Suhu")]
        public int? Suhu {get;set;}
        [Display(Name = "Tanggal Pickup")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime? TanggalPickup { get; set; }
        [Display(Name = "Jam Pickup")]
        public TimeSpan JamPickup { get; set; }
        [Display(Name = "Rute")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? RuteId { get; set; }
        public string Rute { get; set; }
        [Display(Name = "Multi Drop")]
        public string StrMultidrop { get; set; }
        [Display(Name = "Keterangan")]
        public string Keterangan { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganLoading { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganUnloading { get; set; }
        public string StrLoad { get; set; }
        public string StrUnload { get; set; }
        public string Status { get; set; }
        public DateTime? DateStatus { get; set; }
        
        public List<SalesOrderLoadUnload> ListLoad { get; set; }
        public List<SalesOrderLoadUnload> ListUnload { get; set; }

        public int? IdDataTruck { get; set; }
        public string VehicleNo { get; set; }
        public int? JenisTruckItemId { get; set; }
        public string JenisTruckItem { get; set; }
        public string KeteranganDataTruck { get; set; }
        public int? Driver1Id { get; set; }
        public string KodeDriver1 { get; set; }
        public string KodeDriver1Old { get; set; }
        public string NamaDriver1 { get; set; }
        public string KeteranganDriver1 { get; set; }
        public int? Driver2Id { get; set; }
        public string KodeDriver2 { get; set; }
        public string NamaDriver2 { get; set; }
        public string KeteranganDriver2 { get; set; }

        public bool IsCash { get; set; }
        public int? IdDriverTitip { get; set; }
        public int? AtmId { get; set; }
        public string StrRekening { get; set; }
        public string AtasNamaRek { get; set; }
        public string Bank { get; set; }
        public string KeteranganRek { get; set; }

        [Display(Name = "Comment")]
        public string CommentUser { get; set; }

        public string StrLoadBaru { get; set; }
        public string StrUnloadBaru { get; set; }
        public bool IsReturn { get; set; }
        public List<SalesOrderLoadUnload> ListLoadBaru { get; set; }
        public List<SalesOrderLoadUnload> ListUnloadBaru { get; set; }

        public SalesOrderPickup()
        {

        }

        public SalesOrderPickup(Context.SalesOrder dbitem)
        {
            SalesOrderId = dbitem.Id;
            SalesOrderPickupId = dbitem.SalesOrderPickup.SalesOrderPickupId;
            SONumber = dbitem.SalesOrderPickup.SONumber;
            Urutan = dbitem.SalesOrderPickup.Urutan;
            TanggalOrder = dbitem.SalesOrderPickup.TanggalOrder;
            JamOrder = dbitem.SalesOrderPickup.JamOrder;
            CustomerId = dbitem.SalesOrderPickup.CustomerId;
            KodeCustomer = dbitem.SalesOrderPickup.Customer.CustomerCode;
            KodeNama = dbitem.SalesOrderPickup.Customer.CustomerCodeOld;
            NamaCustomer = dbitem.SalesOrderPickup.Customer.CustomerNama;
            StatusKredit = "";
            JenisTruckId = dbitem.SalesOrderPickup.JenisTruckId;
            StrJenisTruck = dbitem.SalesOrderPickup.JenisTrucks.StrJenisTruck;
            ProductId = dbitem.SalesOrderPickup.ProductId;
            StrProduct = dbitem.SalesOrderPickup.MasterProduct.NamaProduk;
            PenanganKhusus = dbitem.SalesOrderPickup.Customer.SpecialTreatment + ", " +
                    string.Join(", ", dbitem.SalesOrderPickup.Customer.CustomerProductType.Where(d => d.idProduk == ProductId).Select(d => d.PenangananKhusus).ToList());
            Suhu = int.Parse(dbitem.SalesOrderPickup.MasterProduct.TargetSuhu.ToString());
            TanggalPickup = dbitem.SalesOrderPickup.TanggalPickup;
            JamPickup = dbitem.SalesOrderPickup.JamPickup;
            RuteId = dbitem.SalesOrderPickup.RuteId;
            Rute = dbitem.SalesOrderPickup.Rute.Nama;
            StrMultidrop = dbitem.SalesOrderPickup.StrMultidrop;
            Keterangan = dbitem.SalesOrderPickup.Keterangan;
            KeteranganLoading = dbitem.SalesOrderPickup.KeteranganLoading;
            KeteranganUnloading = dbitem.SalesOrderPickup.KeteranganUnloading;
            Status = dbitem.Status;
            DateStatus = dbitem.DateStatus;

            ListLoad = new List<SalesOrderLoadUnload>();
            foreach (Context.SalesOrderPickupLoadingAdd item in dbitem.SalesOrderPickup.SalesOrderPickupLoadingAdd)
            {
                ListLoad.Add(new SalesOrderLoadUnload(item));
            }

            ListUnload = new List<SalesOrderLoadUnload>();
            foreach (Context.SalesOrderPickupUnLoadingAdd item in dbitem.SalesOrderPickup.SalesOrderPickupUnLoadingAdd)
            {
                ListUnload.Add(new SalesOrderLoadUnload(item));
            }

            if (dbitem.SalesOrderPickup.IdDataTruck.HasValue)
            {
                IdDataTruck = dbitem.SalesOrderPickup.IdDataTruck;
                VehicleNo = dbitem.SalesOrderPickup.DataTruck.VehicleNo;
                JenisTruckItemId = dbitem.SalesOrderPickup.DataTruck.IdJenisTruck;
                JenisTruckItem = dbitem.SalesOrderPickup.DataTruck.IdJenisTruck.HasValue ? dbitem.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck : "";
            }
            KeteranganDataTruck = dbitem.SalesOrderPickup.KeteranganDataTruck;

            if (dbitem.SalesOrderPickup.Driver1Id.HasValue)
            {
                Driver1Id = dbitem.SalesOrderPickup.Driver1Id;
                KodeDriver1 = dbitem.SalesOrderPickup.Driver1.KodeDriver;
                NamaDriver1 = dbitem.SalesOrderPickup.Driver1.NamaDriver;
            }
            if (dbitem.SalesOrderPickup.Driver2Id.HasValue)
            {
                Driver2Id = dbitem.SalesOrderPickup.Driver2Id;
                KodeDriver2 = dbitem.SalesOrderPickup.Driver2.KodeDriver;
                NamaDriver2 = dbitem.SalesOrderPickup.Driver2.NamaDriver;
            }

            KeteranganDriver1 = dbitem.SalesOrderPickup.KeteranganDriver1;
            KeteranganDriver2 = dbitem.SalesOrderPickup.KeteranganDriver2;

            IsCash = dbitem.SalesOrderPickup.IsCash;
            if (dbitem.SalesOrderPickup.AtmId.HasValue)
            {
                AtmId = dbitem.SalesOrderPickup.AtmId;
                StrRekening = dbitem.SalesOrderPickup.Atm.NoRekening;
                AtasNamaRek = dbitem.SalesOrderPickup.Atm.AtasNama;
                Bank = dbitem.SalesOrderPickup.Atm.LookupCodeBank.Nama;
            }

            KeteranganRek = dbitem.SalesOrderPickup.KeteranganRek;

            IdDriverTitip = dbitem.SalesOrderPickup.IdDriverTitip;

            IsReturn = dbitem.isReturn;
        }

        public void setDb(Context.SalesOrderPickup dbitem)
        {
            dbitem.SalesOrderPickupId = SalesOrderPickupId;
            
            dbitem.TanggalOrder = TanggalOrder.Value;
            dbitem.JamOrder = JamOrder;
            dbitem.CustomerId = CustomerId;
            dbitem.JenisTruckId = JenisTruckId;
            dbitem.ProductId = ProductId;
            dbitem.TanggalPickup = TanggalPickup.Value;
            dbitem.JamPickup = JamPickup;
            dbitem.StrMultidrop = StrMultidrop;
            dbitem.RuteId = RuteId;
            dbitem.Keterangan = Keterangan;
            dbitem.KeteranganLoading = KeteranganLoading;
            dbitem.KeteranganUnloading = KeteranganUnloading;

            //SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(StrLoad);
            //SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(StrUnload);

            dbitem.SalesOrderPickupLoadingAdd.Clear();
            foreach (SalesOrderLoadUnload item in ListLoad)
            {
                dbitem.SalesOrderPickupLoadingAdd.Add(new Context.SalesOrderPickupLoadingAdd()
                {
                    CustomerId = dbitem.CustomerId,
                    CustomerLoadingAddressId = item.Id,
                    urutan = item.urutan,
                    IsSelect = item.IsSelect
                });
            }

            dbitem.SalesOrderPickupUnLoadingAdd.Clear();
            foreach (SalesOrderLoadUnload item in ListUnload)
            {
                dbitem.SalesOrderPickupUnLoadingAdd.Add(new Context.SalesOrderPickupUnLoadingAdd()
                {
                    CustomerId = dbitem.CustomerId,
                    CustomerUnloadingAddressId = item.Id,
                    urutan = item.urutan,
                    IsSelect = item.IsSelect
                });
            }
        }

        public void setDbOperasional(Context.SalesOrderPickup dbitem, string action = "", string userdept = "")
        {
            dbitem.IdDataTruck = IdDataTruck;
            dbitem.KeteranganDataTruck = KeteranganDataTruck;
            dbitem.Driver1Id = Driver1Id;
            dbitem.Driver2Id = Driver2Id == 0 ? null : Driver2Id;
            dbitem.KeteranganDriver1 = KeteranganDriver1;
            dbitem.KeteranganDriver2 = KeteranganDriver2;
            dbitem.IsCash = IsCash;
            dbitem.IdDriverTitip = IdDriverTitip;
            dbitem.AtmId = AtmId;
            dbitem.KeteranganRek = KeteranganRek;

            if (CommentUser != null && CommentUser != "")
            {
                dbitem.SalesOrderPickupComment.Add(new SalesOrderPickupComment()
                {
                    Tanggal = DateTime.Now,
                    CommentUser = CommentUser,
                    Action = action,
                    Username = userdept
                });
            }
        }
    }

}