using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SettlementRegulerTambahanBiaya
    {
        public SettlementRegulerTambahanBiaya()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SettlementReguler")]
        public int? IdSettlementReguler { get; set; }
        public string Keterangan { get; set; }
        public string NamaBiaya { get; set; }
        public Decimal Value { get; set; }

        public virtual SettlementReguler SettlementReguler { get; set; }
    }
}