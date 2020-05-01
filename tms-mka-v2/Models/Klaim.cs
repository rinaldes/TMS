using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Klaim
    {
        public int Id { get; set; }
        [Display(Name = "SalesOrderId")]
        public int? SOKlaimId { get; set; }
        public int? SOKlaimKontrakId { get; set; }
        public string NoSo { get; set; }
        public string JnsOrder { get; set; }
        public string NamaCustomer { get; set; }
        [Display(Name = "No Klaim")]
        public string NoKlaim { get; set; }
        [Display(Name = "Tanggal Pengajuan")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public System.DateTime? TanggalPengajuan { get; set; }
        [Display(Name = "Status Klaim")]
        public int? StatusClaim { get; set; }
        public string StrStatusClaim { get; set; }
        [Display(Name = "Sumber Informasi")]
        public int? SumberInformasi { get; set; }
        [Display(Name = "Kategori Klaim")]
        public string ClaimType { get; set; }
        [Display(Name = "Laporan Kejadian")]
        public string LaporanKejadian { get; set; }
        [Display(Name = "Hasil Pemeriksaan")]
        public string HasilPemeriksaan { get; set; }
        [Display(Name = "Penyelesaian")]
        public string Penyelesaian { get; set; }
        [Display(Name = "No BAP")]
        public string NoBAP { get; set; }
        public string IdBap { get; set; }
        [Display(Name = "Kesalahan")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Kesalahan { get; set; }
        public string KesalahanLain { get; set; }
        [Display(Name = "Hasil Keputusan")]
        public bool IsClaim { get; set; }
        public string strProduk { get; set; }
        public List<KlaimProduct> ListProduct { get; set; }
        [Display(Name = "Total Pengajuan Claim")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Double? TotalPengajuanClaim { get; set; }
        [Display(Name = "Nilai Disetujui")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Double? NilaiDisetujui { get; set; }
        [Display(Name = "Asuransi")]
        public bool AsuransiFlag { get; set; }
        [Display(Name = "Asuransi")]
        public Double? Asuransi { get; set; }
        [Display(Name = "Beban Claim")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public Double? BebanClaim { get; set; }
        [Display(Name = "Beban Claim Driver")]
        public int? BebanClaimDriverPercentage { get; set; }
        public Double? BebanClaimDriver { get; set; }
        [Display(Name = "Beban Claim Kantor")]
        public int? BebanClaimKantorPercentage { get; set; }
        public Double? BebanClaimKantor { get; set; }
        [Display(Name = "Keterangan")]
        public string Keterangan { get; set; }
        public string LastUpdate { get; set; }
        public List<KlaimAttachment> ListAtt { get; set; }
        public string StrAtt { get; set; }
        public System.DateTime? CreatedAt { get; set; }
        public string NoPol { get; set; }
        public string JnsTruck { get; set; }
        public string JenisPendingin { get; set; }
        public string Rute { get; set; }
        public string JenisBarang { get; set; }
        public decimal TargetSuhu { get; set; }
        public int? Driver1Id { get; set; }
        public int? IdDataTruck { get; set; }
        public string KodeDriver1 { get; set; }
        public string KodeDriver1Old { get; set; }
        public string NamaDriver1 { get; set; }
        public string KeteranganDriver1 { get; set; }

        public Klaim()
        {
            ListProduct = new List<KlaimProduct>();
            ListAtt = new List<KlaimAttachment>();
        }
        public Klaim(Context.Klaim dbitem)
        {
            Id = dbitem.Id;
            SOKlaimId = dbitem.SalesOrderId;
            SOKlaimKontrakId = dbitem.SalesOrderKontrakId;
            if (dbitem.SalesOrderId.HasValue)
            {
                if (dbitem.SalesOrder.SalesOrderOncallId.HasValue)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderOncall.SONumber;
                    NamaCustomer = dbitem.SalesOrder.SalesOrderOncall.Customer.CustomerNama;
                    JnsOrder = "OnCall";
                    NoPol = dbitem.SalesOrder.SalesOrderOncall.DataTruck.VehicleNo;
                    JnsTruck = dbitem.SalesOrder.SalesOrderOncall.DataTruck.JenisTrucks.StrJenisTruck;
                    Rute = dbitem.SalesOrder.SalesOrderOncall.StrDaftarHargaItem;
                    JenisBarang = dbitem.SalesOrder.SalesOrderOncall.MasterProduct.NamaProduk;
                    TargetSuhu = dbitem.SalesOrder.SalesOrderOncall.MasterProduct.TargetSuhu;
                }
                else if (dbitem.SalesOrder.SalesOrderProsesKonsolidasiId.HasValue)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.SONumber;
                    //NamaCustomer = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.CustomerNama;
                    JnsOrder = "Konsolidasi";
                    NoPol = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
                    JnsTruck = dbitem.SalesOrder.SalesOrderProsesKonsolidasi.DataTruck.JenisTrucks.StrJenisTruck;
                }
                else if (dbitem.SalesOrder.SalesOrderPickupId.HasValue)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderPickup.SONumber;
                    NamaCustomer = dbitem.SalesOrder.SalesOrderPickup.Customer.CustomerNama;
                    JnsOrder = "Pickup";
                    NoPol = dbitem.SalesOrder.SalesOrderPickup.DataTruck.VehicleNo;
                    JnsTruck = dbitem.SalesOrder.SalesOrderPickup.DataTruck.JenisTrucks.StrJenisTruck;
                }
                else if (dbitem.SalesOrderKontrakId != 0)
                {
                    NoSo = dbitem.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == dbitem.SalesOrderKontrakId).FirstOrDefault().NoSo;
                    NamaCustomer = dbitem.SalesOrder.SalesOrderKontrak.Customer.CustomerNama;
                    JnsOrder = "Kontrak";
                    NoPol = dbitem.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == dbitem.SalesOrderKontrakId).FirstOrDefault().DataTruck.VehicleNo;
                    JnsTruck = dbitem.SalesOrder.SalesOrderKontrak.SalesOrderKontrakListSo.Where(d => d.Id == dbitem.SalesOrderKontrakId).FirstOrDefault().DataTruck.JenisTrucks.StrJenisTruck;
                }
                if (dbitem.IdDataTruck != null)
                {
                    NoPol = dbitem.DataTruck.VehicleNo;
                    JnsTruck = dbitem.DataTruck.JenisTrucks.StrJenisTruck;
                    JenisPendingin = dbitem.DataTruck.DataPendingin.FirstOrDefault().Merk;
                }
            }
            IdDataTruck = dbitem.IdDataTruck;

            NoKlaim = dbitem.NoKlaim;
            TanggalPengajuan = dbitem.TanggalPengajuan;
            StatusClaim = dbitem.StatusKlaim;
            StrStatusClaim = dbitem.LookUpStatusKlaim == null ? "" : dbitem.LookUpStatusKlaim.Nama;
            SumberInformasi = dbitem.SumberInformasiId;
            LaporanKejadian = dbitem.LaporanKejadian;
            HasilPemeriksaan = dbitem.HasilPemeriksaan;
            Penyelesaian = dbitem.Penyelesaian;
            NoBAP = dbitem.NoBap;
            IdBap = dbitem.IdBap;
            Kesalahan = dbitem.Kesalahan;
            KesalahanLain = dbitem.KesalahanLain;
            IsClaim = dbitem.IsClaim;
            ListProduct = new List<KlaimProduct>();
            foreach (var item in dbitem.ListProduk)
	        {
		        ListProduct.Add(new KlaimProduct(item));
	        }
            TotalPengajuanClaim = dbitem.TotalPengajuanClaim;
            NilaiDisetujui = dbitem.NilaiDisetujui;
            AsuransiFlag = dbitem.AsuransiFlag;
            Asuransi = dbitem.Asuransi;
            BebanClaim = dbitem.BebanClaim;
            BebanClaimDriverPercentage = dbitem.BebanClaimDriverPercentage;
            BebanClaimDriver = dbitem.BebanClaimDriver;
            BebanClaimKantorPercentage = dbitem.BebanClaimKantorPercentage;
            BebanClaimKantor = dbitem.BebanClaimKantor;
            Keterangan = dbitem.Keterangan;
            CreatedAt = dbitem.CreatedAt;
            ListAtt = new List<KlaimAttachment>();
            foreach (var item in dbitem.ListAtts)
            {
                ListAtt.Add(new KlaimAttachment(item));
            }
        }
        public void setDb(Context.Klaim dbitem)
        {
            dbitem.CreatedAt = DateTime.Now;
            dbitem.SalesOrderId = SOKlaimId;
            dbitem.SalesOrderKontrakId = SOKlaimKontrakId;
            string dateTime = TanggalPengajuan.Value.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss.00");
            DateTime dt = Convert.ToDateTime(dateTime);
            dbitem.TanggalPengajuan = dt;
            dbitem.StatusKlaim = StatusClaim;
            dbitem.SumberInformasiId = SumberInformasi;
            dbitem.LaporanKejadian = LaporanKejadian;
            dbitem.HasilPemeriksaan = HasilPemeriksaan;
            dbitem.Penyelesaian = Penyelesaian;
            dbitem.IdBap = IdBap;
            dbitem.NoBap = NoBAP;
            dbitem.Kesalahan = Kesalahan;
            dbitem.KesalahanLain = KesalahanLain;
            dbitem.IsClaim = IsClaim;
            dbitem.ClaimType = ClaimType;
            dbitem.ListProduk.Clear();
            foreach (var item in ListProduct)
            {
                dbitem.ListProduk.Add(new Context.KlaimProduct()
                {
                    IdProduct = item.IdProduct,
                    Jumlah = item.Jumlah.Value,
                    NilaiKlaim = item.NilaiKlaim.Value,
                    Keterangan = item.Keterangan
                });
            }
            dbitem.TotalPengajuanClaim = TotalPengajuanClaim;
            dbitem.NilaiDisetujui = NilaiDisetujui;
            dbitem.AsuransiFlag = AsuransiFlag;
            dbitem.Asuransi = Asuransi;
            dbitem.BebanClaim = BebanClaim;
            dbitem.BebanClaimDriverPercentage = BebanClaimDriverPercentage;
            dbitem.BebanClaimDriver = BebanClaimDriver;
            dbitem.IdDataTruck = IdDataTruck;
            dbitem.BebanClaimKantorPercentage = BebanClaimKantorPercentage;
            dbitem.BebanClaimKantor = BebanClaimKantor;
            dbitem.Keterangan = Keterangan;
            dbitem.ListAtts.Clear();
            foreach (var item in ListAtt)
            {
                dbitem.ListAtts.Add(new Context.KlaimAttachments()
                {
                    Filename = item.filename,
                    Realfname = item.realfname,
                    Url = item.url
                });
            }
        }
    }

    public class KlaimProduct
    {
        public int? Id { get; set; }
        public int? IdProduct { get; set; }
        public string NamaBarang { get; set; }
        public string JenisBarang { get; set; }
        public int? Jumlah { get; set; }
        public int? NilaiKlaim { get; set; }
        public string Keterangan { get; set; }

        public KlaimProduct()
        {

        }
        public KlaimProduct(Context.KlaimProduct dbitem)
        {
            Id = dbitem.Id;
            IdProduct = dbitem.IdProduct;
            NamaBarang = dbitem.MasterProduct.NamaProduk;
            JenisBarang = dbitem.MasterProduct.LookupCode.Nama;
            Jumlah = dbitem.Jumlah;
            NilaiKlaim = dbitem.NilaiKlaim;
            Keterangan = dbitem.Keterangan;
        }
    }
    public class KlaimAttachment
    {
        public int Id { get; set; }
        public int IdKlaim { get; set; }
        public string url { get; set; }
        public string filename { get; set; }
        public string realfname { get; set; }

        public KlaimAttachment()
        {

        }
        public KlaimAttachment(Context.KlaimAttachments dbitem)
        {
            Id = dbitem.Id;
            IdKlaim = dbitem.IdKlaim;
            url = dbitem.Url;
            filename = dbitem.Filename;
            realfname = dbitem.Realfname;
        }
    }
}