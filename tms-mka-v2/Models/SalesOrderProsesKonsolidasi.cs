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
    public class SalesOrderProsesKonsolidasi
    {
        public int? SalesOrderId { get; set; }
        public int SalesOrderProsesKonsolidasiId { get; set; }
        [Display(Name = "Tanggal Proses")]
        public DateTime? TanggalProses { get; set; }
        [Display(Name = "NO SO")]
        public string SONumber { get; set; }
        public string DN { get; set; }
        [Display(Name = "Tanggal Muat")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TanggalMuat { get; set; }
        [Display(Name = "Jam Muat")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan JamMuat { get; set; }
        [Display(Name = "Jenis Truck")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdJnsTruck { get; set; }
        public string StrJnsTruck { get; set; }
        public int? JenisTruckBaruId { get; set; }
        public string StrJenisTruckBaru { get; set; }
        [Display(Name = "Rute")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? RuteId { get; set; }
        public string Rute { get; set; }
        [Display(Name = "Multidrop")]
        public string Multidrop { get; set; }
        public string Status { get; set; }
        public DateTime? DateStatus { get; set; }

        public string StrListSo { get; set; }
        public string StrLoad { get; set; }
        public string StrUnload { get; set; }
        public List<SalesOrderLoadUnload> ListLoad { get; set; }
        public List<SalesOrderLoadUnload> ListUnload { get; set; }
        public List<SalesOrderKonsolidasi> ListModelSo { get; set; }

        public int? IdDataTruck { get; set; }
        public string VehicleNo { get; set; }
        public int? JenisTruckItemId { get; set; }
        public string JenisTruckItem { get; set; }
        public string KeteranganDataTruck { get; set; }
        public int? Driver1Id { get; set; }
        public string KodeDriver1Old { get; set; }
        public string KodeDriver1 { get; set; }
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

        public SalesOrderProsesKonsolidasi()
        {
                    
        }

        public SalesOrderProsesKonsolidasi(Context.SalesOrder dbitem)
        {
            SalesOrderId = dbitem.Id;
            SalesOrderProsesKonsolidasiId = dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiId;
            SONumber = dbitem.SalesOrderProsesKonsolidasi.SONumber;
            DN = dbitem.SalesOrderProsesKonsolidasi.DN;
            TanggalProses = dbitem.SalesOrderProsesKonsolidasi.TanggalProses;
            TanggalMuat = dbitem.SalesOrderProsesKonsolidasi.TanggalMuat;
            JamMuat = dbitem.SalesOrderProsesKonsolidasi.JamMuat;
            IdJnsTruck = dbitem.SalesOrderProsesKonsolidasi.JenisTruckId;
            StrJnsTruck = dbitem.SalesOrderProsesKonsolidasi.JenisTrucks.StrJenisTruck;
            RuteId = dbitem.SalesOrderProsesKonsolidasi.IdDaftarHargaItem;
            Rute = dbitem.SalesOrderProsesKonsolidasi.StrDaftarHargaItem;
            Multidrop = dbitem.SalesOrderProsesKonsolidasi.Multidrop;
            Status = dbitem.Status;
            DateStatus = dbitem.DateStatus;

            ListModelSo = new List<SalesOrderKonsolidasi>();
            List<string> listSo = new List<string>();
            foreach (Context.SalesOrderProsesKonsolidasiItem item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiItem)
            {
                listSo.Add(item.SalesOrderKonsolidasiId.ToString());
                ListModelSo.Add(new SalesOrderKonsolidasi(item.SalesOrderKonsolidasi));
            }
            StrListSo = string.Join(",", listSo);

            ListLoad = new List<SalesOrderLoadUnload>();

            foreach (Context.SalesOrderProsesKonsolidasiLoadingAdd item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiLoadingAdd)
            {
                ListLoad.Add(new SalesOrderLoadUnload(item));
            }

            ListUnload = new List<SalesOrderLoadUnload>();

            foreach (Context.SalesOrderProsesKonsolidasiUnLoadingAdd item in dbitem.SalesOrderProsesKonsolidasi.SalesOrderProsesKonsolidasiUnLoadingAdd)
            {
                ListUnload.Add(new SalesOrderLoadUnload(item));
            }

            if (dbitem.SalesOrderProsesKonsolidasi.IdDataTruck.HasValue)
            {
                IdDataTruck = dbitem.SalesOrderProsesKonsolidasi.IdDataTruck;
                VehicleNo = dbitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                JenisTruckItemId = dbitem.SalesOrderProsesKonsolidasi.DataTruck.IdJenisTruck;
                JenisTruckItem = dbitem.SalesOrderProsesKonsolidasi.DataTruck.IdJenisTruck.HasValue ? dbitem.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck : "";
            }
            if (dbitem.SalesOrderProsesKonsolidasi.Driver1Id.HasValue)
            {
                Driver1Id = dbitem.SalesOrderProsesKonsolidasi.Driver1Id;
                KodeDriver1 = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriver;
                KodeDriver1Old = dbitem.SalesOrderProsesKonsolidasi.Driver1.KodeDriverOld;
                NamaDriver1 = dbitem.SalesOrderProsesKonsolidasi.Driver1.NamaDriver;
            }
            if (dbitem.SalesOrderProsesKonsolidasi.Driver2Id.HasValue)
            {
                Driver2Id = dbitem.SalesOrderProsesKonsolidasi.Driver2Id;
                KodeDriver2 = dbitem.SalesOrderProsesKonsolidasi.Driver2.KodeDriver;
                NamaDriver2 = dbitem.SalesOrderProsesKonsolidasi.Driver2.NamaDriver;
            }
            
            KeteranganDataTruck = dbitem.SalesOrderProsesKonsolidasi.KeteranganDataTruck;
            KeteranganDriver1 = dbitem.SalesOrderProsesKonsolidasi.KeteranganDriver1;
            KeteranganDriver2 = dbitem.SalesOrderProsesKonsolidasi.KeteranganDriver2;

            IsCash = dbitem.SalesOrderProsesKonsolidasi.IsCash;
            if (dbitem.SalesOrderProsesKonsolidasi.AtmId.HasValue)
            {
                AtmId = dbitem.SalesOrderProsesKonsolidasi.AtmId;
                StrRekening = dbitem.SalesOrderProsesKonsolidasi.Atm.NoRekening;
                AtasNamaRek = dbitem.SalesOrderProsesKonsolidasi.Atm.AtasNama;
                Bank = dbitem.SalesOrderProsesKonsolidasi.Atm.LookupCodeBank.Nama;
            }

            KeteranganRek = dbitem.SalesOrderProsesKonsolidasi.KeteranganRek;

            IdDriverTitip = dbitem.SalesOrderProsesKonsolidasi.IdDriverTitip;
        }

        public void setDb(Context.SalesOrderProsesKonsolidasi dbitem)
        {
            dbitem.SalesOrderProsesKonsolidasiId = SalesOrderProsesKonsolidasiId;
            
            dbitem.TanggalProses = DateTime.Now.Date;
            dbitem.TanggalMuat = TanggalMuat;
            dbitem.JamMuat = JamMuat;
            dbitem.JenisTruckId = IdJnsTruck;
            dbitem.IdDaftarHargaItem = RuteId;
            dbitem.StrDaftarHargaItem = Rute;
            dbitem.Multidrop = Multidrop;

            dbitem.SalesOrderProsesKonsolidasiItem.Clear();
            foreach (string item in StrListSo.Split(','))
            {
                dbitem.SalesOrderProsesKonsolidasiItem.Add(new SalesOrderProsesKonsolidasiItem() { 
                    SalesOrderKonsolidasiId = int.Parse(item)
                });
            }

            //SalesOrderLoadUnload[] resultLoad = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(StrLoad);
            //SalesOrderLoadUnload[] resultUnload = JsonConvert.DeserializeObject<SalesOrderLoadUnload[]>(StrUnload);

            dbitem.SalesOrderProsesKonsolidasiLoadingAdd.Clear();
            foreach (SalesOrderLoadUnload item in ListLoad)
            {
                dbitem.SalesOrderProsesKonsolidasiLoadingAdd.Add(new Context.SalesOrderProsesKonsolidasiLoadingAdd()
                {
                    CustomerId = item.CustId,
                    SalesOrderKonsolidasiId = item.SalesOrderKonsolidasiId,
                    CustomerLoadingAddressId = item.Id,
                    urutan = item.urutan,
                    IsSelect = item.IsSelect
                });
            }

            dbitem.SalesOrderProsesKonsolidasiUnLoadingAdd.Clear();
            foreach (SalesOrderLoadUnload item in ListUnload)
            {
                dbitem.SalesOrderProsesKonsolidasiUnLoadingAdd.Add(new Context.SalesOrderProsesKonsolidasiUnLoadingAdd()
                {
                    CustomerId = item.CustId,
                    SalesOrderKonsolidasiId = item.SalesOrderKonsolidasiId,
                    CustomerUnloadingAddressId = item.Id,
                    urutan = item.urutan,
                    IsSelect = item.IsSelect
                });
            }
        }

        public void setDbOperasional(Context.SalesOrderProsesKonsolidasi dbitem, string action = "", string userdept = "")
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
                dbitem.SalesOrderProsesKonsolidasiComment.Add(new SalesOrderProsesKonsolidasiComment()
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