using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DokumenItem
    {
        public DokumenItem()
        {}

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Dokumen")]
        public int IdDok { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("CustomerBilling")]
        public int IdBilling { get; set; }
        [Key]
        [Column(Order = 3)]
        [ForeignKey("CustomerBilling")]
        public int CustomerId { get; set; }
        public string KeteranganAdmin{ get; set; }
        public string KeteranganBilling { get; set; }
        public bool IsLengkap { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Dokumen Dokumen { get; set; }
        public virtual CustomerBilling CustomerBilling { get; set; }
    }
}