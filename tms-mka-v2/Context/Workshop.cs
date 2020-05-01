using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Workshop
    {
        public Workshop()
        {
            this.Spk = new HashSet<Spk>();
        }
        [Key]
        public int Id { get; set; }
        public string NoPPK { get; set; }
        public string NoPrePPK { get; set; }
        public DateTime TglPre { get; set; }
        public DateTime TglPPK { get; set; }
        [ForeignKey("Truk")]
        public int IdVehicle { get; set; }
        public int Urutan { get; set; }
        public int UrutanPPK { get; set; }
        [ForeignKey("LookUp")]
        public int? IdPrioritas { get; set; }
        public DateTime? PrioritasFrom { get; set; }
        public DateTime? PrioritasTo { get; set; }
        public string KetPrioritas { get; set; }
        public Boolean IsPool { get; set; }
        public Boolean IsTruck { get; set; }
        public Boolean IsAc { get; set; }
        public Boolean IsGps { get; set; }
        public Boolean IsBan { get; set; }
        public Boolean IsBox { get; set; }
        public string KetTruck { get; set; }
        public string KetAc { get; set; }
        public string KetGps { get; set; }
        public string KetBan { get; set; }
        public string KetBox { get; set; }
        public string KetKerjaTruck { get; set; }
        public string KetKerjaAc { get; set; }
        public string KetKerjaGps { get; set; }
        public string KetKerjaBan { get; set; }
        public string KetKerjaBox { get; set; }
        public string Status { get; set; }
        public int? KmActual { get; set; }
        public int? HmActual { get; set; }
        public virtual DataTruck Truk { get; set; }
        public virtual LookupCode LookUp { get; set; }

        public virtual ICollection<Spk> Spk { get; set; }
    }
}