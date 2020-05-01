using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class DataTruck
    {
        public DataTruck()
        {
            this.DataBox = new HashSet<DataBox>();
            this.DataPendingin = new HashSet<DataPendingin>();
            this.DataGPS = new HashSet<DataGPS>();
            this.DataTruckBoxHistory = new HashSet<DataTruckBoxHistory>();
            this.DataTruckPendinginHistory = new HashSet<DataTruckPendinginHistory>();
            this.DataTruckGPSHistory = new HashSet<DataTruckGPSHistory>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NoTruck { get; set; }
        public string VehicleNo { get; set; }
        [ForeignKey("LookupCodeMerk")]
        public int? IdMerk { get; set; }
        [ForeignKey("JenisTrucks")]
        public int? IdJenisTruck { get; set; }
        public int? TahunBuat { get; set; }
        public int? TahunBeli { get; set; }
        [ForeignKey("MasterPool")]
        public int? IdPool { get; set; }
        [ForeignKey("LookupCodeUnit")]
        public int? IdUnit { get; set; }
        public string Keterangan { get; set; }
        public string Kondisi { get; set; }
        public string SpecModel { get; set; }
        public int? KmLimit { get; set; }
        public string NoMesin { get; set; }
        public string NoRangka { get; set; }
        public DateTime? GaransiStr { get; set; }
        public DateTime? GaransiEnd { get; set; }
        public string SpecKeterangan { get; set; }
        public string AtasNama { get; set; }
        public string BPKB { get; set; }
        public string urlBPKB { get; set; }
        public string fnameBPKB { get; set; }
        public string keteranganBPKB { get; set; }
        public DateTime? STNK { get; set; }
        public string urlSTNK { get; set; }
        public string fnameSTNK { get; set; }
        public string keteranganSTNK { get; set; }
        public DateTime? KIR { get; set; }
        public string urlKIR { get; set; }
        public string fnameKIR { get; set; }
        public string keteranganKIR { get; set; }
        public DateTime? KIU { get; set; }
        public string urlKIU { get; set; }
        public string fnameKIU { get; set; }
        public string keteranganKIU { get; set; }
        public DateTime? IBM { get; set; }
        public string urlIBM { get; set; }
        public string fnameIBM { get; set; }
        public string keteranganIBM { get; set; }
        public DateTime? Asuransi { get; set; }
        public string urlAsuransi { get; set; }
        public string fnameAsuransi { get; set; }
        public string keteranganAsuransi { get; set; }
        public DateTime? Reklame { get; set; }
        public string urlReklame { get; set; }
        public string fnameReklame { get; set; }
        public string keteranganReklame { get; set; }
        public string NoPolis { get; set; }
        public string urlNoPolis { get; set; }
        public string fnameNoPolis { get; set; }
        public string keteranganNoPolis { get; set; }
        public string Peminjam { get; set; }
        public string urlPeminjam { get; set; }
        public string fnamePeminjam { get; set; }
        public string keteranganPeminjam { get; set; }
        public string Leasing { get; set; }
        public string urlLeasing { get; set; }
        public string fnameLeasing { get; set; }
        public string keteranganLeasing { get; set; }
        public int urutan { get; set; }
        public float MaxSpeed { get; set; }
        //public bool IsDelete { get; set; }

        public virtual LookupCode LookupCodeMerk { get; set; }
        public virtual LookupCode LookupCodeUnit { get; set; }
        public virtual JenisTrucks JenisTrucks { get; set; }
        public virtual MasterPool MasterPool { get; set; }
        public virtual ICollection<DataBox> DataBox { get; set; }
        public virtual ICollection<DataGPS> DataGPS { get; set; }
        public virtual ICollection<DataPendingin> DataPendingin { get; set; }
        public virtual ICollection<DataTruckBoxHistory> DataTruckBoxHistory { get; set; }
        public virtual ICollection<DataTruckPendinginHistory> DataTruckPendinginHistory { get; set; }
        public virtual ICollection<DataTruckGPSHistory> DataTruckGPSHistory { get; set; }
    }
}