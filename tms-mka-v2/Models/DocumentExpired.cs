using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DocumentExpired
    {
        #region initial setting

        public string Waktu { get; set; }
        public int STNK { get; set; }
        public int KIR { get; set; }
        public int KIU { get; set; }
        public int IBM { get; set; }

        #endregion initial setting

        #region initial value

        public DocumentExpired(string waktuProses, int stnk, int kir, int kiu, int ibm)
        {
            Waktu = waktuProses;
            STNK = stnk;
            KIR = kir;
            KIU = kiu;
            IBM = ibm;
        }

        #endregion initial value
    }
}