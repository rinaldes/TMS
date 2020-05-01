using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace tms_mka_v2.Models
{
    public class RevisiKeterangan
    {
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        [Display(Name = "Keterangan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string KeteranganRevisi { get; set; }
        public RevisiKeterangan()
        {

        }
        public RevisiKeterangan(Context.SalesOrder dbitem)
        {
            IdSalesOrder = dbitem.Id;

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem);
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem);
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
            }
        }
    }
}