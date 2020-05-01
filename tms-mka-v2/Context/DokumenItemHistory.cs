using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DokumenItemHistory
    {
        public DokumenItemHistory()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Dokumen")]
        public int IdDok { get; set; }
        public int UserId { get; set; }
        public string Nama { get; set; }
        public int Jml { get; set; }
        public string Warna { get; set; }
        public bool Stempel { get; set; }
        public bool Lengkap { get; set; }
        public string KeteranganAdmin{ get; set; }
        public string KeteranganBilling { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Dokumen Dokumen { get; set; }
    }
}