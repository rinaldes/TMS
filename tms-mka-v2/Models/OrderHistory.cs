using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public string StatusFlow { get; set; }
        public DateTime FlowDate { get; set; }
        public DateTime SavedAt { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string PIC { get; set; }
        
        public OrderHistory()
        {

        }
        public OrderHistory(Context.OrderHistory dbitem, string pic)
        {
            StatusFlow = dbitem.StatusFlow == 1 ? "MARKETING" : dbitem.StatusFlow == 2 ? "PLANNING" : dbitem.StatusFlow == 3 ? "KONFIRMASI" : dbitem.StatusFlow == 4 ? "ADMIN UANG JALAN" : dbitem.StatusFlow == 5 ? "TRANSFER" : dbitem.StatusFlow == 6 ? "KAS" : dbitem.StatusFlow == 7 ? "SETTLEMENT REGULER" : dbitem.StatusFlow == 9 ? "BATAL TRUK" : dbitem.StatusFlow == 10 ? "MAJU 1 FLOW" : dbitem.StatusFlow == 11 ? "REVISI TANGGAL" : dbitem.StatusFlow == 12 ? "REVISI JENIS TRUK" : dbitem.StatusFlow == 13 ? "REVISI RUTE" : dbitem.StatusFlow == 14 ? "REVISI TANGGAL BP" : dbitem.StatusFlow == 15 ? "SETTLEMENT BATAL" : dbitem.StatusFlow == 16 ? "SURAT JALAN" : dbitem.StatusFlow == 17 ? "BILLING" : dbitem.StatusFlow == 18 ? "SOLAR INAP" : "BATAL ORDER";
            FlowDate = dbitem.FlowDate;
            SavedAt = dbitem.SavedAt;
            ProcessedAt = dbitem.ProcessedAt;
            PIC = pic;
        }
    }
}