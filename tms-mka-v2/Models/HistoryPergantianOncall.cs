using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class HistoryPergantianOncall
    {
        public int Id { get; set; }
        public int? SalesOrderOncallId { get; set; }
        public int? IdDriver { get; set; }
        public int? IdDriverOld { get; set; }
        public string KeteranganGanti { get; set; }
        public DateTime Tanggal { get; set; }
        public HistoryPergantianOncall()
        {

        }
        public HistoryPergantianOncall(Context.HistoryPergantianOncall dbitem)
        {
            Id = dbitem.Id;
            SalesOrderOncallId = dbitem.SalesOrderOncallId;
            IdDriver = dbitem.IdDriver;
            IdDriverOld = dbitem.IdDriverOld;
            KeteranganGanti = dbitem.KeteranganGanti;
            Tanggal = dbitem.Tanggal;
        }

        public void setDb(Context.HistoryPergantianOncall dbitem)
        { 

        }
    }
}