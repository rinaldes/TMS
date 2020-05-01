using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class KekuranganDriver
    {
        #region initial setting

        public string JenisTruck { get; set; }
        public int JumlahKurang { get; set; }
        public decimal Percentage { get; set; }
        public string LamaKurang { get; set; }

        #endregion initial setting

        #region initial value

        public KekuranganDriver(string jenisTruk, int jumlahKurang, decimal persentase, string lamaKurang)
        {
            JenisTruck = jenisTruk;
            JumlahKurang = jumlahKurang;
            Percentage = persentase;
            LamaKurang = lamaKurang;
        }
        
        #endregion initial value
    }
}