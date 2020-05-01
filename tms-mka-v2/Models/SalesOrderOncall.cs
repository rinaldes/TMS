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
    public class SalesOrderOncall
    {
        public int? SalesOrderId { get; set; }
        public int SalesOrderOnCallId { get; set; }
        [Display(Name = "NO SO")]
        public string SONumber { get; set; }
        public string DN { get; set; }
        public int Urutan { get; set; }
        [Display(Name = "Tanggal Order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime? TanggalOrder { get; set; }
        [Display(Name = "Jam Order")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamOrder { get; set; }
        public int? CustomerId { get; set; }
        [Display(Name = "Kode Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string KodeCustomer { get; set; }
        public string KodeNama { get; set; }
        [Display(Name = "Nama Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaCustomer { get; set; }
        [Display(Name = "Status Kredit")]
        public string StatusKredit { get; set; }
        [Display(Name = "Prioritas")]
        public int? PrioritasId { get; set; }
        public string StrPrioritas { get; set; }
        [Display(Name = "Jenis Truk")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? JenisTruckId { get; set; }
        public string StrJenisTruck { get; set; }
        [Display(Name = "Jenis Truk")]
        public int? JenisTruckBaruId { get; set; }
        public string StrJenisTruckBaru { get; set; }
        [Display(Name = "Jenis Barang")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? ProductId { get; set; }
        public string StrProduct { get; set; }
        [Display(Name = "Penanganan Khusus")]
        public string PenanganKhusus { get; set; }
        [Display(Name = "Target Suhu")]
        public int? Suhu { get; set; }
        [Display(Name = "Tanggal Muat")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime? TanggalMuat { get; set; }
        [Display(Name = "Jam Muat")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamMuat { get; set; }
        [Display(Name = "Rute")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? RuteId { get; set; }
        public string Rute { get; set; }
        [Display(Name = "Multi Drop")]
        public string StrMultidrop { get; set; }
        public string IsAreaPulang { get; set; }
        [Display(Name = "Keterangan")]
        public string Keterangan { get; set; }
        [Display(Name = "No Shipment")]
        public string NoShipment { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganLoading { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganUnloading { get; set; }
        public string StrLoad { get; set; }
        public string StrUnload { get; set; }
        public string Status { get; set; }
        public DateTime? DateStatus { get; set; }
        public bool IsReturn { get; set; }
        public bool DateRevised { get; set; }
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
        public bool RuteRevised { get; set; }
        public int? IdDriverTitip { get; set; }
        public int? AtmId { get; set; }
        public string StrRekening {get; set; }
        public string AtasNamaRek { get; set; }
        public string Bank { get; set; }
        public string KeteranganRek { get; set; }

        [Display(Name = "Keterangan Batal")]
        public String KeteranganBatal { get; set; }

        [Display(Name = "Comment")]
        public string CommentUser { get; set; }

        public SalesOrderOncall()
        {

        }

        public SalesOrderOncall(Context.SalesOrder dbitem)
        {
            SalesOrderId = dbitem.Id;
            SalesOrderOnCallId = dbitem.SalesOrderOncall.SalesOrderOnCallId;
            SONumber = dbitem.SalesOrderOncall.SONumber;
            DN = dbitem.SalesOrderOncall.DN;
            Urutan = dbitem.SalesOrderOncall.Urutan;
            RuteRevised  = dbitem.RuteRevised;
            TanggalOrder = dbitem.SalesOrderOncall.TanggalOrder;
            JamOrder = dbitem.SalesOrderOncall.JamOrder;
            CustomerId = dbitem.SalesOrderOncall.CustomerId;
            KodeCustomer = dbitem.SalesOrderOncall.Customer.CustomerCode;
            KodeNama = dbitem.SalesOrderOncall.Customer.CustomerCodeOld;
            NamaCustomer = dbitem.SalesOrderOncall.Customer.CustomerNama;
            StatusKredit = "";
            PrioritasId = dbitem.SalesOrderOncall.PrioritasId;
            StrPrioritas = dbitem.SalesOrderOncall.PrioritasId == null ? "" : dbitem.SalesOrderOncall.LookUpPrioritas.Nama;
            JenisTruckId = dbitem.SalesOrderOncall.JenisTruckId;
            StrJenisTruck = dbitem.SalesOrderOncall.JenisTrucks.StrJenisTruck;
            ProductId = dbitem.SalesOrderOncall.ProductId;
            StrProduct = dbitem.SalesOrderOncall.MasterProduct.NamaProduk;
            PenanganKhusus = dbitem.SalesOrderOncall.Customer.SpecialTreatment + ", " +
                   string.Join(", ", dbitem.SalesOrderOncall.Customer.CustomerProductType.Where(d => d.idProduk == ProductId).Select(d => d.PenangananKhusus).ToList());
            Suhu = int.Parse(dbitem.SalesOrderOncall.MasterProduct.TargetSuhu.ToString());
            TanggalMuat = dbitem.SalesOrderOncall.TanggalMuat;
            JamMuat = dbitem.SalesOrderOncall.JamMuat;
            RuteId = dbitem.SalesOrderOncall.IdDaftarHargaItem;
            Rute = dbitem.SalesOrderOncall.StrDaftarHargaItem;
            StrMultidrop = dbitem.SalesOrderOncall.StrMultidrop;
            Keterangan = dbitem.SalesOrderOncall.Keterangan;
            KeteranganLoading = dbitem.SalesOrderOncall.KeteranganLoading;
            KeteranganUnloading = dbitem.SalesOrderOncall.KeteranganUnloading;
            Status = dbitem.Status;
            DateStatus = dbitem.DateStatus;
            ListLoad = new List<SalesOrderLoadUnload>();

            foreach (Context.SalesOrderOnCallLoadingAdd item in dbitem.SalesOrderOncall.SalesOrderOnCallLoadingAdd)
            {
                ListLoad.Add(new SalesOrderLoadUnload(item));
            }

            ListUnload = new List<SalesOrderLoadUnload>();

            foreach (Context.SalesOrderOnCallUnLoadingAdd item in dbitem.SalesOrderOncall.SalesOrderOnCallUnLoadingAdd)
            {
                ListUnload.Add(new SalesOrderLoadUnload(item));
            }


            if (dbitem.SalesOrderOncall.IdDataTruck.HasValue)
            {
                IdDataTruck = dbitem.SalesOrderOncall.IdDataTruck;
                VehicleNo = dbitem.SalesOrderOncall.DataTruck.VehicleNo;
                JenisTruckItemId = dbitem.SalesOrderOncall.DataTruck.IdJenisTruck;
                JenisTruckItem = dbitem.SalesOrderOncall.DataTruck.IdJenisTruck.HasValue ? dbitem.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck : "";
            }
            KeteranganDataTruck = dbitem.SalesOrderOncall.KeteranganDataTruck;

            if (dbitem.SalesOrderOncall.Driver1Id.HasValue)
            {
                Driver1Id = dbitem.SalesOrderOncall.Driver1Id;
                KodeDriver1 = dbitem.SalesOrderOncall.Driver1.KodeDriver;
                NamaDriver1 = dbitem.SalesOrderOncall.Driver1.NamaDriver;
                KodeDriver1Old = dbitem.SalesOrderOncall.Driver1.KodeDriverOld;
            }
            if (dbitem.SalesOrderOncall.Driver2Id.HasValue)
            {
                Driver2Id = dbitem.SalesOrderOncall.Driver2Id;
                KodeDriver2 = dbitem.SalesOrderOncall.Driver2.KodeDriver;
                NamaDriver2 = dbitem.SalesOrderOncall.Driver2.NamaDriver;
            }

            KeteranganDriver1 = dbitem.SalesOrderOncall.KeteranganDriver1;
            KeteranganDriver2 = dbitem.SalesOrderOncall.KeteranganDriver2;

            IsCash = dbitem.SalesOrderOncall.IsCash;
            if (dbitem.SalesOrderOncall.AtmId.HasValue)
            {
                AtmId = dbitem.SalesOrderOncall.AtmId;
                StrRekening = dbitem.SalesOrderOncall.Atm.NoRekening;
                AtasNamaRek = dbitem.SalesOrderOncall.Atm.AtasNama;
                Bank = dbitem.SalesOrderOncall.Atm.LookupCodeBank.Nama;
            }

            KeteranganRek = dbitem.SalesOrderOncall.KeteranganRek;

            IdDriverTitip = dbitem.SalesOrderOncall.IdDriverTitip;

            IsReturn = dbitem.isReturn;
            DateRevised = dbitem.DateRevised;
        }

        public void setDb(Context.SalesOrderOncall dbitem, string action = "", string userdept = "")
        {
            //var JSONSetting = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            
            dbitem.SalesOrderOnCallId = SalesOrderOnCallId;
            
            dbitem.TanggalOrder = TanggalOrder.Value;
            dbitem.JamOrder = JamOrder;
            dbitem.CustomerId = CustomerId;
            dbitem.PrioritasId = PrioritasId;
            dbitem.JenisTruckId = JenisTruckId;
            dbitem.ProductId = ProductId;
            dbitem.TanggalMuat = TanggalMuat.Value;
            dbitem.JamMuat = JamMuat;
            dbitem.StrMultidrop = StrMultidrop;
            dbitem.IdDaftarHargaItem = RuteId;
            dbitem.StrDaftarHargaItem = Rute;
            dbitem.Keterangan = Keterangan;
            dbitem.KeteranganLoading = KeteranganLoading;
            dbitem.KeteranganUnloading = KeteranganUnloading;
            //SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(StrLoad);
            //SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(StrUnload);
        
            dbitem.SalesOrderOnCallLoadingAdd.Clear();
            foreach (SalesOrderLoadUnload item in ListLoad)
            {
                dbitem.SalesOrderOnCallLoadingAdd.Add(new Context.SalesOrderOnCallLoadingAdd()
                {
                    CustomerId = dbitem.CustomerId,
                    CustomerLoadingAddressId = item.Id,
                    urutan = item.urutan,
                    IsSelect = item.IsSelect
                });
            }

            dbitem.SalesOrderOnCallUnLoadingAdd.Clear();
            foreach (SalesOrderLoadUnload item in ListUnload)
            {
                dbitem.SalesOrderOnCallUnLoadingAdd.Add(new Context.SalesOrderOnCallUnLoadingAdd()
                {
                    CustomerId = dbitem.CustomerId,
                    CustomerUnloadingAddressId = item.Id,
                    urutan = item.urutan,
                    IsSelect = item.IsSelect
                });
            }

            if (CommentUser != null && CommentUser != "")
            {
                dbitem.SalesOrderOnCallComment.Add(new SalesOrderOnCallComment()
                {
                    Tanggal = DateTime.Now,
                    CommentUser = CommentUser,
                    Action = action,
                    Username = userdept
                });
            }
        }

        public void setDbOperasional(Context.SalesOrderOncall dbitem, string action = "", string userdept = "")
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
                dbitem.SalesOrderOnCallComment.Add(new SalesOrderOnCallComment()
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
