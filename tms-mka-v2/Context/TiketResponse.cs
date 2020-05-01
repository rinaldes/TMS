using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class TiketResponse
    {
        public TiketResponse()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Tiket")]
        public int IdTiket { get; set; }
        public string Respon { get; set; }
        [ForeignKey("User")]
        public int IdResponder { get; set; }
        public string ResponseAttachment { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Tiket Tiket { get; set; }
        public virtual User User { get; set; }
    }
}