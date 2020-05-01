using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class PelaksanaanTrainingDetail
    {
        public PelaksanaanTrainingDetail()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(Order = 1)]
        [ForeignKey("PelaksanaanTraining")]
        public int? PelaksanaanTrainingId { get; set; }
        [ForeignKey("idDriver")]
        public int? IdDriver { get; set; }
        public int Nilai { get; set; }
        public string Keterangan { get; set; }
        public string Status { get; set; }

        public virtual PelaksanaanTraining PelaksanaanTraining { get; set; }
        public virtual Driver idDriver { get; set; }
    }
}