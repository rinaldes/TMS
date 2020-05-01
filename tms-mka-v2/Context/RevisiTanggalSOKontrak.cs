using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class RevisiTanggalSOKontrak
    {
        public RevisiTanggalSOKontrak()
        {
            
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("RevisiTanggal")]
        public int? RevisiTanggalID { get; set; }
        public System.DateTime? TanggalMuatLama { get; set; }

        public virtual RevisiTanggal RevisiTanggal { get; set; }

    }
}