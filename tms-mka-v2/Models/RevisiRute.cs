using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace tms_mka_v2.Models
{
    public class RevisiRute
    {
        public int Id { get; set; }
        public int? IdSalesOrder { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        [Display(Name = "Rute Lama")]
        public string RuteLama { get; set; }
        [Display(Name = "Multidrop Lama")]
        public string MultidropLama { get; set; }
        [Display(Name = "Rute")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string RuteBaru { get; set; }
        public int IdRute { get; set; }
        [Display(Name = "Multidrop")]
        public string MultidropBaru { get; set; }
        [Display(Name = "Keterangan")]
        public string KeteranganRevisi { get; set; }
        public string StrLoad { get; set; }
        public string StrUnload { get; set; }
        public List<SalesOrderLoadUnload> ListLoad { get; set; }
        public List<SalesOrderLoadUnload> ListUnload { get; set; }

        public RevisiRute()
        {

        }
        public RevisiRute(Context.SalesOrder dbitem)
        {
            IdSalesOrder = dbitem.Id;

            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem);
                RuteLama = ModelOncall.Rute;
                MultidropLama = ModelOncall.StrMultidrop;
                ListLoad = ModelOncall.ListLoad;
                ListUnload = ModelOncall.ListUnload;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem);
                RuteLama = ModelPickup.Rute;
                ListLoad = ModelPickup.ListLoad;
                ListUnload = ModelPickup.ListUnload;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                RuteLama = ModelKonsolidasi.Rute;
            }
        }
    }
}