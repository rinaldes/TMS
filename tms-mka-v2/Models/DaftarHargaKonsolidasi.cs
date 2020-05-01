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
    public class DaftarHargaKonsolidasi
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
        public string StrItem { get; set; }
        public string StrAttachment { get; set; }
        public string StrKondisi { get; set; }
        public List<DaftarHargaKonsolidasiItem> listItem { get; set; }
        public List<DaftarHargaKonsolidasiAttachment> listAtt { get; set; }
        public List<DaftarHargaKondisi> listKondisi { get; set; }
        public DaftarHargaKonsolidasi()
        {
            listItem = new List<DaftarHargaKonsolidasiItem>();
            listAtt = new List<DaftarHargaKonsolidasiAttachment>();
            listKondisi = new List<DaftarHargaKondisi>();
            DaftarHargaKondisi.GenerateKondisi(listKondisi);
        }
        public DaftarHargaKonsolidasi(Context.DaftarHargaKonsolidasi dbitem)
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
            listItem = new List<DaftarHargaKonsolidasiItem>();
            foreach (Context.DaftarHargaKonsolidasiItem item in dbitem.DaftarHargaKonsolidasiItem.ToList())
            {
                listItem.Add(new DaftarHargaKonsolidasiItem(item));
            }
            //kondisi
            listKondisi = new List<DaftarHargaKondisi>();
            foreach (Context.DaftarHargaKonsolidasiKondisi item in dbitem.DaftarHargaKonsolidasiKondisi.ToList())
            {
                listKondisi.Add(new DaftarHargaKondisi(item));
            }
            //attachment
            listAtt = new List<DaftarHargaKonsolidasiAttachment>();
            foreach (Context.DaftarHargaKonsolidasiAttachment item in dbitem.DaftarHargaKonsolidasiAttachment.ToList())
            {
                listAtt.Add(new DaftarHargaKonsolidasiAttachment(item));
            }
        }
        public void setDb(Context.DaftarHargaKonsolidasi dbitem)
        {
            //dbitem.Id = Id;
            dbitem.IdCust = IdCust;
            dbitem.PeriodStart = PeriodStart.Value;
            dbitem.PeriodEnd = PeriodEnd.Value;
            
            // items
            //dbitem.DaftarHargaKonsolidasiItem.Clear();
            DaftarHargaKonsolidasiItem[] result = JsonConvert.DeserializeObject<DaftarHargaKonsolidasiItem[]>(StrItem);
            List<Context.DaftarHargaKonsolidasiItem> DummyItems = dbitem.DaftarHargaKonsolidasiItem.ToList();
            List<int> ListAnuTeuDiHapus = new List<int>();
            foreach (DaftarHargaKonsolidasiItem item in result)
            {
                // edit row
                if (item.Id != 0)
                {
                    Context.DaftarHargaKonsolidasiItem dhkItem = dbitem.DaftarHargaKonsolidasiItem.Where(i => i.IdDaftarHargaKonsolidasi == dbitem.Id && i.Id == item.Id).FirstOrDefault();

                    dhkItem.Id = item.Id;
                    dhkItem.NamaDaftarHargaRute = item.NamaRuteDaftarHarga;
                    dhkItem.ListIdRute = item.ListIdRute;
                    dhkItem.ListNamaRute = item.ListNamaRute;
                    dhkItem.IdJenisKendaraan = item.IdJenisKendaraan;
                    dhkItem.MinKg = item.MinKg;
                    dhkItem.MaxKg = item.MaxKg;
                    dhkItem.Harga = item.Harga;
                    dhkItem.IdSatuanHarga = item.IdSatuanHarga;
                    dhkItem.IsAsuransi = item.IsAsuransi;
                    dhkItem.Premi = item.Premi;
                    dhkItem.PihakPenanggung = item.PihakPenanggung;
                    dhkItem.TipeNilaiTanggungan = item.TipeNilaiTanggungan;
                    dhkItem.NilaiTanggungan = item.NilaiTanggungan;
                    dhkItem.Keterangan = item.Keterangan;
                    ListAnuTeuDiHapus.Add(item.Id);
                }
                else //add row
                {
                    dbitem.DaftarHargaKonsolidasiItem.Add(new Context.DaftarHargaKonsolidasiItem()
                    {
                        NamaDaftarHargaRute = item.NamaRuteDaftarHarga,
                        ListIdRute = item.ListIdRute,
                        ListNamaRute = item.ListNamaRute,
                        IdJenisKendaraan = item.IdJenisKendaraan,
                        MinKg = item.MinKg,
                        MaxKg = item.MaxKg,
                        Harga = item.Harga,
                        IdSatuanHarga = item.IdSatuanHarga,
                        IsAsuransi = item.IsAsuransi,
                        Premi = item.Premi,
                        PihakPenanggung = item.PihakPenanggung,
                        TipeNilaiTanggungan = item.TipeNilaiTanggungan,
                        NilaiTanggungan = item.NilaiTanggungan,
                        Keterangan = item.Keterangan
                    });
                }
            }
            foreach (Context.DaftarHargaKonsolidasiItem dbhapus in DummyItems)
            {
                if (!ListAnuTeuDiHapus.Any(d => d == dbhapus.Id))
                {
                    dbitem.DaftarHargaKonsolidasiItem.Remove(dbhapus);
                }
            }
            //kondisi
            dbitem.DaftarHargaKonsolidasiKondisi.Clear();
            foreach (DaftarHargaKondisi item in listKondisi.Where(d => d.IsDelete == false))
            {
                dbitem.DaftarHargaKonsolidasiKondisi.Add(new Context.DaftarHargaKonsolidasiKondisi()
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
            dbitem.DaftarHargaKonsolidasiAttachment.Clear();
            DaftarHargaKonsolidasiAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaKonsolidasiAttachment[]>(StrAttachment);
            foreach (DaftarHargaKonsolidasiAttachment item in resultAtt)
            {
                dbitem.DaftarHargaKonsolidasiAttachment.Add(new Context.DaftarHargaKonsolidasiAttachment()
                {
                    FileName = item.FileName,
                    RFileName = item.RFileName,
                });
            }
        }
        public void setItem(Context.DaftarHargaKonsolidasiItem dbitem)
        {
            
        }
    }

    public class DaftarHargaKonsolidasiItem
    {
        public int Id { get; set; }
        public string NamaRuteDaftarHarga { get; set; }
        public string ListIdRute { get; set; }
        public string ListNamaRute { get; set; }
        public int? IdJenisKendaraan { get; set; }
        public string NamaJenisKendaraan { get; set; }
        public int MinKg { get; set; }
        public int MaxKg { get; set; }
        public Decimal Harga { get; set; }
        public int IdSatuanHarga { get; set; }
        public string SatuanHarga { get; set; }
        public Boolean IsAsuransi { get; set; }
        public Decimal? Premi { get; set; }
        public string TipeNilaiTanggungan { get; set; }
        public Decimal? NilaiTanggungan { get; set; }
        public string PihakPenanggung { get; set; }
        public string Keterangan { get; set; }



        public DaftarHargaKonsolidasiItem()
        {

        }

        public DaftarHargaKonsolidasiItem(Context.DaftarHargaKonsolidasiItem dbitem)
        {
            Id = dbitem.Id;
            NamaRuteDaftarHarga = dbitem.NamaDaftarHargaRute;
            ListIdRute = dbitem.ListIdRute;
            ListNamaRute = dbitem.ListNamaRute;
            IdJenisKendaraan = dbitem.IdJenisKendaraan;
            if (dbitem.IdJenisKendaraan.HasValue)
                NamaJenisKendaraan = dbitem.JenisTrucks.StrJenisTruck;
            MinKg = dbitem.MinKg;
            MaxKg = dbitem.MaxKg;
            Harga = dbitem.Harga;
            IdSatuanHarga = dbitem.IdSatuanHarga;
            SatuanHarga = dbitem.LookupCodeSatuan.Nama;
            IsAsuransi = dbitem.IsAsuransi;
            Premi = dbitem.Premi;
            PihakPenanggung = dbitem.PihakPenanggung;
            TipeNilaiTanggungan = dbitem.TipeNilaiTanggungan;
            NilaiTanggungan = dbitem.NilaiTanggungan;
            Keterangan = dbitem.Keterangan;
        }
    }

    public class DaftarHargaKonsolidasiAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string RFileName { get; set; }

        public DaftarHargaKonsolidasiAttachment()
        {

        }

        public DaftarHargaKonsolidasiAttachment(Context.DaftarHargaKonsolidasiAttachment dbitem)
        {
            Id = dbitem.Id;
            FileName = dbitem.FileName;
            RFileName = dbitem.RFileName;
        }
    }
}