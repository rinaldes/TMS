using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RemovalUangTf
    {
        public RemovalUangTf()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Removal")]
        public int? IdRemoval { get; set; }
        public int? IdCreditTf { get; set; }
        public string Code { get; set; }
        public string Keterangan { get; set; }
        public int? Value { get; set; }
        public DateTime? Tanggal { get; set; }
        public decimal? JumlahTransfer { get; set; }
        [ForeignKey("Atm")]
        public int? idRekenings { get; set; }
        public DateTime? TanggalAktual { get; set; }
        public TimeSpan? JamAktual { get; set; }
        public string KeteranganTf { get; set; }
        [ForeignKey("Driver")]
        public int? IdDriverPenerima { get; set; }
        public bool isTf { get; set; }

        public virtual Removal Removal { get; set; }
        public virtual Atm Atm { get; set; }
        public virtual Driver Driver { get; set; }
    }
}