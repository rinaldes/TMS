using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace tms_mka_v2.Models
{
    public class RevisiJenisTruk
    {
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public int? JenisTruckId { get; set; }
        public string StrJenisTruck { get; set; }
        [Display(Name = "Jenis Truk Baru")]
        public int? JenisTruckIdBaru { get; set; }
        [Display(Name = "Jenis Truk Lama")]
        public int? JenisTruckIdLama { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganRevisi { get; set; }
        public RevisiJenisTruk()
        {

        }
        public RevisiJenisTruk(Context.SalesOrder dbitem)
        {
            IdSalesOrder = dbitem.Id;

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem);
                JenisTruckIdLama = ModelOncall.JenisTruckId;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                JenisTruckIdLama = ModelKonsolidasi.IdJnsTruck;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem);
                JenisTruckIdLama = ModelPickup.JenisTruckId;
            }
        }
        public RevisiJenisTruk(Context.RevisiJenisTruk dbitem)
        {
            Id = dbitem.Id;
            IdSalesOrder = dbitem.IdSalesOrder;

            if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem.SalesOrder);
                JenisTruckIdLama = ModelOncall.JenisTruckId;
            }
            else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem.SalesOrder);
                JenisTruckIdLama = ModelKonsolidasi.IdJnsTruck;
            }
            else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem.SalesOrder);
                JenisTruckIdLama = ModelPickup.JenisTruckId;
            }
            JenisTruckIdBaru = dbitem.JenisTruckIdBaru;
            KeteranganRevisi = KeteranganRevisi;
        }
    }
}