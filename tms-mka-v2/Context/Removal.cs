using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Removal
    {
        public Removal()
        {
            this.RemovalTambahanLain = new HashSet<RemovalTambahanLain>();
            this.RemovalPotonganDriver = new HashSet<RemovalPotonganDriver>();
            this.RemovalTambahanRute = new HashSet<RemovalTambahanRute>();
            this.RemovalVoucherSpbu = new HashSet<RemovalVoucherSpbu>();
            this.RemovalVoucherKapal = new HashSet<RemovalVoucherKapal>();
            this.RemovalUangTf = new HashSet<RemovalUangTf>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("AdminUangJalan")]
        public int? IdAdminUangJalan { get; set; }
        [ForeignKey("SalesOrder")]
        public int? IdSO { get; set; }
        public DateTime? TanggalRemoval { get; set; }
        public TimeSpan? JamRemoval { get; set; }
        public string StatusTagihan { get; set; }
        public string KeteranganRemoval { get; set; }
        public string IdDataBorongan { get; set; }
        public Decimal? NilaiBorongan { get; set; }
        public Decimal? Kawalan { get; set; }
        public Decimal? Timbangan { get; set; }
        public Decimal? Karantina { get; set; }
        public Decimal? SPSI { get; set; }
        public Decimal? Multidrop { get; set; }
        public Decimal? TotalBorongan { get; set; }
        public string KeteranganAdmin { get; set; }
        [ForeignKey("Driver1")]
        public int? IdDriver1 { get; set; }
        [ForeignKey("Driver2")]
        public int? IdDriver2 { get; set; }
        public Decimal? TotalKasbon { get; set; }
        public Decimal? KasbonDriver1 { get; set; }
        public Decimal? KasbonDriver2 { get; set; }
        public Decimal? TotalKlaim { get; set; }
        public Decimal? KlaimDriver1 { get; set; }
        public Decimal? KlaimDriver2 { get; set; }
        public Decimal? TotalPotonganDriver { get; set; }
        public Decimal? TotalAlokasi { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; }


        public virtual AdminUangJalan AdminUangJalan { get; set; }
        public virtual Driver Driver1 { get; set; }
        public virtual Driver Driver2 { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }

        public virtual ICollection<RemovalTambahanLain> RemovalTambahanLain { get; set; }
        public virtual ICollection<RemovalPotonganDriver> RemovalPotonganDriver { get; set; }
        public virtual ICollection<RemovalTambahanRute> RemovalTambahanRute { get; set; }
        public virtual ICollection<RemovalVoucherSpbu> RemovalVoucherSpbu { get; set; }
        public virtual ICollection<RemovalVoucherKapal> RemovalVoucherKapal { get; set; }
        public virtual ICollection<RemovalUangTf> RemovalUangTf { get; set; }
    }
}