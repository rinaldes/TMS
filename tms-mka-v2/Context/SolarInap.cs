using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SolarInap
    {
        public SolarInap()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("SO")]
        public int? IdSO { get; set; }
        public int? SalesOrderKontrakListSOId { get; set; }
        public DateTime TanggalDari { get; set; }
        public DateTime TanggalHingga { get; set; }
        public System.DateTime? TanggalTiba { get; set; }
        public TimeSpan JamTiba { get; set; }
        public int NilaiYgDiajukan { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string KeteranganOperation { get; set; }
        public string so_oid { get; set; }
        public int Nominal { get; set; }
        public int AktualCash { get; set; }
        public int AktualTransfer { get; set; }
        public string KeteranganMarketing { get; set; }
        public string KeteranganAdmin { get; set; }
        public string KeteranganKasirCash { get; set; }
        public string KeteranganBatal { get; set; }
        public string KeteranganKasirTransfer { get; set; }
        public string StatusTagihan { get; set; }
        [ForeignKey("Driver")]
        public int? IdDriver { get; set; }
        public int Cash { get; set; }
        public int CashBack { get; set; }
        public int Transfer { get; set; }
        public DateTime TglTransfer { get; set; }
        public System.DateTime TanggalAktualCash { get; set; }
        public TimeSpan JamAktualCash { get; set; }
        public System.DateTime TanggalAktualTransfer { get; set; }
        public TimeSpan JamAktualTransfer { get; set; }
        public DateTime TglCash { get; set; }
        public string DititipKe { get; set; }
        public string AktualDititipkanKepada { get; set; }
        public System.DateTime TanggalBatal { get; set; }
        public int? IdCreditTf { get; set; }
        public int? UrutanBatal { get; set; }

        [ForeignKey("Atm")]
        public int? IdAtm { get; set; }

        [ForeignKey("AktualAtm")]
        public int? AktualIdAtm { get; set; }

        public int StepKe { get; set; }
        //public bool IsDelete { get; set; }
        public virtual SalesOrder SO { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Atm Atm { get; set; }
        public virtual Atm AktualAtm { get; set; }
    }
}