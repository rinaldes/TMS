using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DaftarHargaKontrak
    {
        public int Id { get; set; }
        [Display(Name = "Kode Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdCust { get; set; }
        public string KodeCustomer { get; set; }
        public string KodeNama { get; set; }
        public string NamaCustomer { get; set; }
        public string AlamatCustomer { get; set; }
        public string TelpCustomer { get; set; }
        public string FaxCustomer { get; set; }
        public string ContactCustomer { get; set; }
        public string HpCustomer { get; set; }
        public string PeriodeHarga { get; set; }

        [Display(Name = "Periode")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? PeriodStart { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? PeriodEnd { get; set; }
        [Display(Name = "Kontrak")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdTypeKontrak { get; set; }
        public string StrItem { get; set; }
        public string StrAttachment { get; set; }
        public string StrKondisi { get; set; }
        public List<DaftarHargaKontrakItem> listItem { get; set; }
        public List<DaftarHargaKontrakAttachment> listAtt { get; set; }
        public List<DaftarHargaKondisi> listKondisi { get; set; }        
        public DaftarHargaKontrak()
        {
            listItem = new List<DaftarHargaKontrakItem>();
            listAtt = new List<DaftarHargaKontrakAttachment>();
            listKondisi = new List<DaftarHargaKondisi>();
            DaftarHargaKondisi.GenerateKondisi(listKondisi);
        }
        public DaftarHargaKontrak(Context.DaftarHargaKontrak dbitem)
        {
            Context.CustomerPic custPIC = dbitem.Customer.CustomerPic.FirstOrDefault();
            Context.CustomerAddress custAddr = dbitem.Customer.CustomerAddress.Where(
                a => a.LookUpCodesOffice != null && (a.LookUpCodesOffice.Nama.ToLower() == "head office" || a.LookUpCodesOffice.Nama.ToLower() == "kantor pusat")).FirstOrDefault();

            listKondisi = new List<DaftarHargaKondisi>();
            DaftarHargaKondisi.GenerateKondisi(listKondisi);

            Id = dbitem.Id;
            IdCust = dbitem.IdCust;
            KodeCustomer = dbitem.Customer.CustomerCode;
            KodeNama = dbitem.Customer.CustomerCodeOld;
            NamaCustomer = dbitem.Customer.CustomerNama;
            PeriodStart = dbitem.PeriodStart;
            PeriodEnd = dbitem.PeriodEnd;
            IdTypeKontrak = dbitem.IdTypeKontrak;

            if (custAddr != null)
            {
                AlamatCustomer = custAddr.Alamat;
                TelpCustomer = custAddr.Telp;
                FaxCustomer = custAddr.Fax;
            }

            if (custPIC != null)
            {
                ContactCustomer = custPIC.Name;
                HpCustomer = custPIC.Mobile;
            }
            
            //item
            listItem = new List<DaftarHargaKontrakItem>();
            foreach (Context.DaftarHargaKontrakItem item in dbitem.DaftarHargaKontrakItem.ToList())
            {
                listItem.Add(new DaftarHargaKontrakItem(item));
            }
            //kondisi
            listKondisi = new List<DaftarHargaKondisi>();
            foreach (Context.DaftarHargaKontrakKondisi item in dbitem.DaftarHargaKontrakKondisi.ToList())
            {
                listKondisi.Add(new DaftarHargaKondisi(item));
            }
            //attachment
            listAtt = new List<DaftarHargaKontrakAttachment>();
            foreach (Context.DaftarHargaKontrakAttachment item in dbitem.DaftarHargaKontrakAttachment.ToList())
            {
                listAtt.Add(new DaftarHargaKontrakAttachment(item));
            }
        }
        public void setDb(Context.DaftarHargaKontrak dbitem)
        {
            dbitem.IdCust = IdCust;
            dbitem.PeriodStart = PeriodStart.Value;
            dbitem.PeriodEnd = PeriodEnd.Value;
            dbitem.IdTypeKontrak = IdTypeKontrak;

            // items
            //dbitem.DaftarHargaKontrakItem.Clear();
            DaftarHargaKontrakItem[] result = JsonConvert.DeserializeObject<DaftarHargaKontrakItem[]>(StrItem);
            List<Context.DaftarHargaKontrakItem> DummyItems = dbitem.DaftarHargaKontrakItem.ToList();
            List<int> ListAnuTeuDiHapus = new List<int>();
            foreach (DaftarHargaKontrakItem item in result)
            {
                Context.DaftarHargaKontrakItem dhkItem = dbitem.DaftarHargaKontrakItem.Where(i => i.IdDaftarHargaKontrak == dbitem.Id && i.Id == item.Id).FirstOrDefault();
                if (item.Id != 0)
                {
                    dhkItem.Id = item.Id;
                    dhkItem.NamaRuteDaftarHarga = item.NamaRuteDaftarHarga;
                    dhkItem.ListIdRute = item.ListIdRute;
                    dhkItem.ListNamaRute = item.ListNamaRute;                    
                    dhkItem.IdJenisTruck = item.IdJenisTruck;
                    dhkItem.BeratMinimum = item.BeratMinimum;
                    dhkItem.Harga = item.Harga;
                    dhkItem.IdSatuanHarga = item.IdSatuanHarga;
                    dhkItem.HargaRit2 = item.HargaRit2;
                    dhkItem.Overtime = item.Overtime;
                    dhkItem.RitaseBulan = item.RitaseBulan;
                    dhkItem.IsAsuransi = item.IsAsuransi;
                    dhkItem.PihakPenanggung = item.PihakPenanggung;
                    dhkItem.TipeNilaiTanggungan = item.TipeNilaiTanggungan;
                    dhkItem.NilaiTanggungan = item.NilaiTanggungan;
                    dhkItem.Premi = item.Premi;
                    dhkItem.Keterangan = item.Keterangan;
                    ListAnuTeuDiHapus.Add(item.Id);
                }
                else
                {
                    dbitem.DaftarHargaKontrakItem.Add(new Context.DaftarHargaKontrakItem(){
                        //Id = item.Id,
                        NamaRuteDaftarHarga = item.NamaRuteDaftarHarga,
                        ListIdRute = item.ListIdRute,
                        ListNamaRute = item.ListNamaRute,                    
                        IdJenisTruck = item.IdJenisTruck,
                        BeratMinimum = item.BeratMinimum,
                        Harga = item.Harga,
                        IdSatuanHarga = item.IdSatuanHarga,
                        HargaRit2 = item.HargaRit2,
                        Overtime = item.Overtime,
                        RitaseBulan = item.RitaseBulan,
                        IsAsuransi = item.IsAsuransi,
                        PihakPenanggung = item.PihakPenanggung,
                        TipeNilaiTanggungan = item.TipeNilaiTanggungan,
                        NilaiTanggungan = item.NilaiTanggungan,
                        Premi = item.Premi,            
                        Keterangan = item.Keterangan
                    });
                }
                
            }

            foreach (Context.DaftarHargaKontrakItem dbhapus in DummyItems)
            {
                if (!ListAnuTeuDiHapus.Any(d => d == dbhapus.Id))
                {
                    dbitem.DaftarHargaKontrakItem.Remove(dbhapus);
                }
            }

            //kondisi
            dbitem.DaftarHargaKontrakKondisi.Clear();
            foreach (DaftarHargaKondisi item in listKondisi.Where(d => d.IsDelete == false))
            {
                dbitem.DaftarHargaKontrakKondisi.Add(new Context.DaftarHargaKontrakKondisi()
                {
                    kondisi = item.kondisi,
                    IsInclude = item.IsInclude,
                    IsBill = item.IsBill,
                    value = item.value,
                    IsDefault = item.IsDefault,
                    IsKota = item.IsKota,
                    IsTitik = item.IsTitik,
                    ValKota = item.ValKota,
                    ValTitik = item.ValTitik,
                    IsDelete = item.IsDelete,
                });
            }
            //Attachment
            dbitem.DaftarHargaKontrakAttachment.Clear();
            DaftarHargaKontrakAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaKontrakAttachment[]>(StrAttachment);
            foreach (DaftarHargaKontrakAttachment item in resultAtt)
            {
                dbitem.DaftarHargaKontrakAttachment.Add(new Context.DaftarHargaKontrakAttachment()
                {
                    FileName = item.FileName,
                    RFileName = item.RFileName,
                });
            }
        }
        public void setItem(Context.DaftarHargaKontrakItem dbitem)
        {
            
        }
    }

    public class DaftarHargaKontrakItem
    {
        public int Id { get; set; }
        public string NamaRuteDaftarHarga { get; set; }
        public string ListIdRute { get; set; }
        public string ListNamaRute { get; set; }
        public int IdJenisTruck { get; set; }
        public string NamaJenisTruck { get; set; }
        public int BeratMinimum { get; set; }
        public Decimal Harga { get; set; }
        public int IdSatuanHarga { get; set; }
        public string SatuanHarga { get; set; }
        public Decimal? HargaRit2 { get; set; }
        public Decimal? Overtime { get; set; }
        public Decimal? RitaseBulan { get; set; }
        public bool IsAsuransi { get; set; }
        public string PihakPenanggung { get; set; }
        public string TipeNilaiTanggungan { get; set; }        
        public Decimal? NilaiTanggungan { get; set; }
        public Decimal? Premi { get; set; }
        public string Keterangan { get; set; }

        public DaftarHargaKontrakItem()
        {

        }

        public DaftarHargaKontrakItem(Context.DaftarHargaKontrakItem dbitem)
        {
            Id = dbitem.Id;
            NamaRuteDaftarHarga = dbitem.NamaRuteDaftarHarga;
            ListIdRute = dbitem.ListIdRute;
            ListNamaRute = dbitem.ListNamaRute;
            NamaJenisTruck = dbitem.JenisTrucks.StrJenisTruck;
            IdJenisTruck = dbitem.IdJenisTruck;
            BeratMinimum = dbitem.BeratMinimum;
            Harga = dbitem.Harga;
            IdSatuanHarga = dbitem.IdSatuanHarga;
            SatuanHarga = dbitem.LookupCodeSatuan.Nama;
            HargaRit2 = (dbitem.HargaRit2.HasValue)?dbitem.HargaRit2.Value:0;
            Overtime = (dbitem.Overtime.HasValue)?dbitem.Overtime.Value:0;
            RitaseBulan = (dbitem.RitaseBulan.HasValue)?dbitem.RitaseBulan.Value:0;
            IsAsuransi = dbitem.IsAsuransi;
            PihakPenanggung = dbitem.PihakPenanggung;
            TipeNilaiTanggungan = dbitem.TipeNilaiTanggungan;
            NilaiTanggungan = (dbitem.NilaiTanggungan.HasValue)?dbitem.NilaiTanggungan.Value:0;
            Premi = (dbitem.Premi.HasValue)?dbitem.Premi.Value:0;
            Keterangan = dbitem.Keterangan;
        }
    }

    public class DaftarHargaKontrakAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string RFileName { get; set; }

        public DaftarHargaKontrakAttachment()
        {

        }

        public DaftarHargaKontrakAttachment(Context.DaftarHargaKontrakAttachment dbitem)
        {
            Id = dbitem.Id;
            FileName = dbitem.FileName;
            RFileName = dbitem.RFileName;
        }
    }
}