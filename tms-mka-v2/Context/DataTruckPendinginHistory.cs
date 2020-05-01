using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataTruckPendinginHistory
    {
        public DataTruckPendinginHistory()
        {

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DataTruck")]
        public int IdDataTruck { get; set; }
        public string NoPendingin { get; set; }
        public DateTime? Tanggal { get; set; }
        public string strDataTruk { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public string HmLimit { get; set; }
        public int? Tahun { get; set; }
        public string strJenisPendingin { get; set; }
        public string NoMesin { get; set; }
        public string NoKompresor { get; set; }
        public DateTime? tglPasang { get; set; }
        public string user { get; set; }
        public virtual DataTruck DataTruck { get; set; }
        
    }
}