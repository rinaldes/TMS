using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Removal
    {
        #region variable
        public int Id { get; set; }
        public int? IdSo { get; set; }
        public int? IdAdminUangJalan { get; set; }
        public int? IdRuteLama { get; set; }
        [Display(Name = "Rute Lama")]
        public string StrRuteLama { get; set; }
        public string StatusTagihan { get; set; }
        public int? IdRute { get; set; }
        [Display(Name = "Rute Baru")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string StrRute { get; set; }
        [Display(Name = "Tanggal Removal")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? TanggalRemoval { get; set; }
        [Display(Name = "Jam Removal")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public TimeSpan? JamRemoval { get; set; }
        public string Keterangan { get; set; }
        public SalesOrderOncall ModelOncall { get; set; }
        public SalesOrderPickup ModelPickup { get; set; }
        public SalesOrderProsesKonsolidasi ModelKonsolidasi { get; set; }
        public string StrUnload { get; set; }
        public List<SalesOrderLoadUnload> ListUnload { get; set; }
        #endregion
        
        public Removal() { }
        public Removal(Context.SalesOrder dbitem)
        {
            if (dbitem.SalesOrderOncallId.HasValue)
            {
                ModelOncall = new SalesOrderOncall(dbitem);
                IdSo = dbitem.SalesOrderOncallId;
                StrRuteLama = ModelOncall.Rute;
                ListUnload = ModelOncall.ListUnload;
            }
            else if (dbitem.SalesOrderPickupId.HasValue)
            {
                ModelPickup = new SalesOrderPickup(dbitem);
                IdSo = dbitem.SalesOrderPickupId;
                StrRuteLama = ModelPickup.Rute;
                ListUnload = ModelPickup.ListUnload;
            }
            else if (dbitem.SalesOrderProsesKonsolidasiId.HasValue)
            {
                ModelKonsolidasi = new SalesOrderProsesKonsolidasi(dbitem);
                IdSo = dbitem.SalesOrderProsesKonsolidasiId;
                StrRuteLama = ModelKonsolidasi.Rute;
            }
        }
    }
}