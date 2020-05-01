using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class JurnalSetting
    {
        public JurnalSetting()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public int? IdBiayaBorongan { get; set; }
        public int? IdBiayaKawalan { get; set; }
        public int? IdBiayaTimbangan { get; set; }
        public int? IdBiayaKarantina { get; set; }
        public int? IdBiayaKuliBongkar { get; set; }
        public int? IdBiayaMultidrop { get; set; }
        public int? IdBiayaTambahanSolar { get; set; }
        public int? IdHutangUangJalanDriver { get; set; }
    }
}