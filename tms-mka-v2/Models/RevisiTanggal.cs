using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace tms_mka_v2.Models
{
    public class RevisiTanggal
    {
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        [Display(Name = "Tanggal Lama")]
        public DateTime? TanggalLama { get; set; }
        [Display(Name = "Jam Lama")]
        public TimeSpan? JamLama { get; set; }
        [Display(Name = "Tanggal Baru")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TanggalBaru { get; set; }
        [Display(Name = "Jam Baru")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan? JamBaru { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganRevisi { get; set; }
        public RevisiTanggal()
        {

        }
        public RevisiTanggal(Context.SalesOrder dbitem)
        {
            IdSalesOrder = dbitem.Id;

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem);
                TanggalLama = ModelOncall.TanggalMuat;
                JamLama = ModelOncall.JamMuat;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem);
                TanggalLama = ModelPickup.TanggalPickup;
                JamLama = ModelPickup.JamPickup;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                TanggalLama = ModelKonsolidasi.TanggalMuat;
                JamLama = ModelKonsolidasi.JamMuat;
            }
        }
        public RevisiTanggal(Context.RevisiTanggal dbitem)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSalesOrder;

            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem.SalesOrder);
                TanggalLama = ModelOncall.TanggalMuat;
                JamLama = ModelOncall.JamMuat;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem.SalesOrder);
                TanggalLama = ModelPickup.TanggalPickup;
                JamLama = ModelPickup.JamPickup;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem.SalesOrder);
                TanggalLama = ModelKonsolidasi.TanggalMuat;
                JamLama = ModelKonsolidasi.JamMuat;
            }
            TanggalBaru = dbitem.TanggalMuat;
            JamBaru = dbitem.JamMuat;
            KeteranganRevisi = KeteranganRevisi;
        }
    }
}