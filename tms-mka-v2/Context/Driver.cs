using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class Driver
    {
        public Driver()
        {
            this.DriverStatusHistory = new HashSet<DriverStatusHistory>();
            this.DriverTruckHistory = new HashSet<DriverTruckHistory>();
            this.BebanKlaimDriver = new HashSet<BebanKlaimDriver>();
            this.Inventaris = new HashSet<Inventaris>();
            this.DriverJasa = new HashSet<DriverJasa>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("LookupCodeStatus")]
        public int? IdStatus { get; set; }
        public string KodeDriver { get; set; }
        public string KodeDriverOld { get; set; }
        public DateTime? TglGabung { get; set; }
        public string NoKtp { get; set; }
        public string NamaDriver { get; set; }
        public string NamaPangilan { get; set; }
        public string TempatLahir { get; set; }
        public DateTime? TglLahir { get; set; }
        [ForeignKey("LookupCodeJenisSim")]
        public int? IdJenisSim { get; set; }
        public string NoSim { get; set; }
        public DateTime? TglBerlakuSim { get; set; }
        public string NoTlp { get; set; }
        public string NoHp1 { get; set; }
        public string NoHp2 { get; set; }
        public string Alamat { get; set; }
        public string Rt { get; set; }
        public string Rw { get; set; }
        [ForeignKey("LocProvinsi")]
        public int? IdProvinsi { get; set; }
        [ForeignKey("LocKabKota")]
        public int? IdKabKota { get; set; }
        [ForeignKey("LocKecamatan")]
        public int? IdKec { get; set; }
        [ForeignKey("LocKelurahan")]
        public int? IdKel { get; set; }
        public bool IsSameKtp { get; set; }
        public string AlamatDomisili { get; set; }
        public string RtDomisili { get; set; }
        public string RwDomisili { get; set; }
        [ForeignKey("LocProvinsiDomisili")]
        public int? IdProvinsiDomisili { get; set; }
        [ForeignKey("LocKabKotaDomisili")]
        public int? IdKabKotaDomisili { get; set; }
        [ForeignKey("LocKecamatanDomisili")]        
        public int? IdKecDomisili { get; set; }
        [ForeignKey("LocKelurahanDomisili")]
        public int? IdKelDomisili { get; set; }
        public string Keterangan { get; set; }
        public bool IsSms { get; set; }
        public string Pathfoto { get; set; }        
        [ForeignKey("LookupCodeReferensiDriver")]
        public int? IdReferensiDriver { get; set; }
        [ForeignKey("DriverRef")]
        public int? IdRef { get; set; }
        public string HubunganRef { get; set; }
        public string KeteranganRef { get; set; }
        
        public bool IsKemitraan { get; set; }
        public bool IsKemitraanAsli { get; set; }
        public string UrlKemitraan { get; set; }
        public bool IsJaminanKel { get; set; }
        public bool IsJaminanKelAsli { get; set; }
        public string UrlJaminanKel { get; set; }
        public bool IsIjazah { get; set; }
        public bool IsIjazahAsli { get; set; }
        public string UrlIjazah { get; set; }
        public bool IsBukuNikah { get; set; }
        public bool IsBukuNikahAsli { get; set; }
        public string UrlBukuNikah { get; set; }
        public bool IsSKCK { get; set; }
        public bool IsSKCKAsli { get; set; }
        public string UrlSKCK { get; set; }
        public bool IsDomisili { get; set; }
        public bool IsDomisiliAsli { get; set; }
        public string UrlDomisili { get; set; }
        public bool IsKK { get; set; }
        public bool IsKKAsli { get; set; }
        public string UrlKK { get; set; }
        public bool IsKTP { get; set; }
        public bool IsKTPAsli { get; set; }
        public string UrlKTP { get; set; }
        public bool IsSIM { get; set; }
        public bool IsSIMAsli { get; set; }
        public string UrlSIM { get; set; }
        public decimal SaldoPiutang { get; set; }
        public decimal SaldoKlaim { get; set; }
        public string LastPbyOid { get; set; }
        public DateTime? LastSync { get; set; }


        public int Urutan { get; set; }
        //public bool IsDelete { get; set; }
        public virtual Driver DriverRef { get; set; }
        public virtual LookupCode LookupCodeStatus { get; set; }
        public virtual LookupCode LookupCodeReferensiDriver { get; set; }
        public virtual LookupCode LookupCodeJenisSim { get; set; }
        public virtual Location LocProvinsi { get; set; }
        public virtual Location LocKabKota { get; set; }
        public virtual Location LocKecamatan { get; set; }
        public virtual Location LocKelurahan { get; set; }
        public virtual Location LocProvinsiDomisili { get; set; }
        public virtual Location LocKabKotaDomisili { get; set; }
        public virtual Location LocKecamatanDomisili { get; set; }
        public virtual Location LocKelurahanDomisili { get; set; }
        public virtual ICollection<DriverStatusHistory> DriverStatusHistory { get; set; }
        public virtual ICollection<DriverTruckHistory> DriverTruckHistory { get; set; }
        public virtual ICollection<BebanKlaimDriver> BebanKlaimDriver { get; set; }
        public virtual ICollection<Inventaris> Inventaris { get; set; }
        public virtual ICollection<DriverJasa> DriverJasa { get; set; }
    }
}