using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DaftarHargaOnCallAttachment
    {
        public DaftarHargaOnCallAttachment()
        {

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DaftarHargaOnCall")]
        public int IdDaftarHargaOnCall { get; set; }
        public string FileName { get; set; }
        public string RFileName { get; set; }

        public virtual DaftarHargaOnCall DaftarHargaOnCall { get; set; }
    }
}