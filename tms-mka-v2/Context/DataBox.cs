using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataBox
    {
        public DataBox()
        {
            this.DataBoxHistory = new HashSet<DataBoxHistory>();
            this.DataBoxLantai = new HashSet<DataBoxLantai>();
            this.DataBoxDinding = new HashSet<DataBoxDinding>();
        }

        [Key]
        public int Id { get; set; }
        public string NoBox { get; set; }
        [ForeignKey("DataTruck")]
        public int? IdDataTruck { get; set; }
        public string Karoseri { get; set; }
        public int? Tahun { get; set; }
        [ForeignKey("LookupCodeType")]
        public int? IdType { get; set; }
        [ForeignKey("LookupCodeKategori")]
        public int? IdKategori { get; set; }
        public string Lantai { get; set; }
        public string Dinding { get; set; }
        public bool? PintuSamping { get; set; }
        public bool? Sekat { get; set; }
        public DateTime? garansiStr { get; set; }
        public DateTime? garansiEnd { get; set; }
        public DateTime? asuransiStr { get; set; }
        public DateTime? asuransiEnd { get; set; }
        public DateTime? tglPasang { get; set; }
        //public bool IsDelete { get; set; }
        public int Urutan { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        public virtual LookupCode LookupCodeType { get; set; }
        public virtual LookupCode LookupCodeKategori { get; set; }
        public virtual ICollection<DataBoxLantai> DataBoxLantai { get; set; }
        public virtual ICollection<DataBoxDinding> DataBoxDinding { get; set; }
        public virtual ICollection<DataBoxHistory> DataBoxHistory { get; set; }
    }
}