using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class BAP
    {
       
        [Key]
        public int Id { get; set; }
        [ForeignKey("SalesOrder")]
        public int? SalesOrderId { get; set; }
        public int? SalesOrderKontrakId { get; set; }
        public string Status { get; set; }
        public System.DateTime? CreatedAt { get; set; }
        public string NoBAP { get; set; }
        public System.DateTime? TanggalKejadian { get; set; }
        public TimeSpan JamKejadian { get; set; }
        [ForeignKey("LookUpKategori")]
        public int? KategoriId { get; set; }
        public string LaporanKejadian { get; set; }
        public string DilaporkanOleh { get; set; }
        [ForeignKey("Departemen1")]
        public int? Departemen1Id { get; set; }
        public string HasilPemeriksaan { get; set; }
        public string Penyelesaian { get; set; }
        public string DiperiksaOleh { get; set; }
        [ForeignKey("Departemen2")]
        public int? Departemen2Id { get; set; }
        [ForeignKey("Driver1")]
        public int? Driver1Id { get; set; }
        [ForeignKey("Driver2")]
        public int? Driver2Id { get; set; }
        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        public string File { get; set; }

        public virtual SalesOrder SalesOrder { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual LookupCode Departemen1 { get; set; }
        public virtual LookupCode Departemen2 { get; set; }
        public virtual LookupCode LookUpKategori { get; set; }
        public virtual ICollection<BAPAttachments> ListAtts { get; set; }
    }
}