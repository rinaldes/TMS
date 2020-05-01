using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataBoxHistory
    {
        public DataBoxHistory()
        {
            
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DataBox")]
        public int IdDataBox { get; set; }
        public string Vehicle { get; set; }
        public string NoBox { get; set; }
        public string Karoseri { get; set; }
        public int? Tahun { get; set; }
        public string strType { get; set; }
        public string strKategori { get; set; }
        public string Lantai { get; set; }
        public string Dinding { get; set; }
        public bool? PintuSamping { get; set; }
        public bool? Sekat { get; set; }
        public DateTime? garansiStr { get; set; }
        public DateTime? garansiEnd { get; set; }
        public DateTime? asuransiStr { get; set; }
        public DateTime? asuransiEnd { get; set; }
        public DateTime? tglPasang { get; set; }
        public DateTime? Tanggal { get; set; }
        public string username { get; set; }        
        public virtual DataBox DataBox { get; set; }
    }
}