using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class SolarInapHistory
    {
        public int Id { get; set; }
        public string StatusFlow { get; set; }
        public DateTime FlowDate { get; set; }
        public DateTime SavedAt { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string PIC { get; set; }
        
        public SolarInapHistory()
        {

        }
        public SolarInapHistory(Context.SolarInapHistory dbitem, string pic)
        {
            StatusFlow = dbitem.StatusFlow == 1 ? "DM" : dbitem.StatusFlow == 2 ? "MARKETING" : dbitem.StatusFlow == 3 ? "ADMIN UANG JALAN" : dbitem.StatusFlow == 4 ? "KASIR TRANSFER" : dbitem.StatusFlow == 5 ? "KASIR KAS" : "BATAL";
            FlowDate = dbitem.FlowDate;
            SavedAt = dbitem.SavedAt;
            ProcessedAt = dbitem.ProcessedAt;
            PIC = pic;
        }
    }
}