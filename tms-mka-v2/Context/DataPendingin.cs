using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataPendingin
    {
        public DataPendingin()
        {
            this.ListHistoryPendingin = new HashSet<DataPendinginHistory>();
        }

        [Key]
        public int Id { get; set; }
        public string NoPendingin { get; set; }
        [ForeignKey("DataTruk")]
        public int? IdDataTruk { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public string HmLimit { get; set; }
        public int? Tahun { get; set; }
        [ForeignKey("LookupCodeJenis")]
        public int? IdJenisPendingin { get; set; }
        public string NoMesin { get; set; }
        public string NoKompresor { get; set; }
        public DateTime? tglPasang { get; set; }
        public int Urutan { get; set; }
        //public bool IsDelete { get; set; }

        public virtual LookupCode LookupCodeJenis { get; set; }
        public virtual DataTruck DataTruk { get; set; }
        public virtual ICollection<DataPendinginHistory> ListHistoryPendingin { get; set; }
    }
}