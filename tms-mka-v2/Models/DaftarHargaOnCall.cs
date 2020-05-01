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
    public class DaftarHargaOnCall
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        [Display(Name = "Kode Customer")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdCust { get; set; }
        public string KodeCustomer { get; set; }
        public string KodeNama { get; set; }
        public string NamaCustomer { get; set; }
        [Display(Name = "Periode")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? PeriodStart { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public DateTime? PeriodEnd { get; set; }
        public string AlamatCustomer { get; set; }
        public string TelpCustomer { get; set; }
        public string FaxCustomer { get; set; }
        public string ContactCustomer{ get; set; }
        public string HpCustomer { get; set; }
        public string StrItem { get; set; }
        public List<DaftarHargaOnCallItem> listItem { get; set; }
        public string StrAttachment { get; set; }
        public List<DaftarHargaOnCallAttachment> listAtt { get; set; }
        public List<DaftarHargaKondisi> listKondisi { get; set; }

        public DaftarHargaOnCall()
        {
            listItem = new List<DaftarHargaOnCallItem>();
            listAtt = new List<DaftarHargaOnCallAttachment>();
            listKondisi = new List<DaftarHargaKondisi>();
            DaftarHargaKondisi.GenerateKondisi(listKondisi);
        }

        public DaftarHargaOnCall(Context.DaftarHargaOnCall dbitem)
        {
            Context.CustomerPic custPIC = dbitem.Customer.CustomerPic.FirstOrDefault();
            Context.CustomerAddress custAddr = dbitem.Customer.CustomerAddress.Where(
                a => a.LookUpCodesOffice != null && (a.LookUpCodesOffice.Nama.ToLower() == "head office" || a.LookUpCodesOffice.Nama.ToLower() == "kantor pusat")).FirstOrDefault();

            //header
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
            listItem = new List<DaftarHargaOnCallItem>();
            foreach (Context.DaftarHargaOnCallItem item in dbitem.DaftarHargaOnCallItem.ToList())
            {
                listItem.Add(new DaftarHargaOnCallItem(item));
            }
            //kondisi
            listKondisi = new List<DaftarHargaKondisi>();
            foreach (Context.DaftarHargaOnCallKondisi item in dbitem.DaftarHargaOnCallKondisi.ToList())
            {
                listKondisi.Add(new DaftarHargaKondisi(item));
            }
            //attachment
            listAtt = new List<DaftarHargaOnCallAttachment>();
            foreach (Context.DaftarHargaOnCallAttachment item in dbitem.DaftarHargaOnCallAttachment.ToList())
            {
                listAtt.Add(new DaftarHargaOnCallAttachment(item));
            }
        }

        public void setDb(Context.DaftarHargaOnCall dbitem)
        {
            //header
            dbitem.Id = Id;
            dbitem.IdCust = IdCust;
            dbitem.PeriodStart = PeriodStart.Value;
            dbitem.PeriodEnd = PeriodEnd.Value;
            //item
            DaftarHargaOnCallItem[] result = JsonConvert.DeserializeObject<DaftarHargaOnCallItem[]>(StrItem);
            List<Context.DaftarHargaOnCallItem> DummyItems = dbitem.DaftarHargaOnCallItem.ToList();
            // edit row

            List<int> ListAnuTeuDiHapus = new List<int>();
            foreach (DaftarHargaOnCallItem item in result)
            {
                if (item.Id != 0) {
                    Context.DaftarHargaOnCallItem dhkItem = dbitem.DaftarHargaOnCallItem.Where(i => i.IdDaftarHargaOnCall == dbitem.Id && i.Id == item.Id).FirstOrDefault();
                    dhkItem.Id = item.Id;
                    dhkItem.NamaRuteDaftarHarga = item.NamaRuteDaftarHarga;
                    dhkItem.ListIdRute = item.ListIdRute;
                    dhkItem.ListNamaRute = item.ListNamaRute;
                    dhkItem.IdJenisTruck = item.IdJenisTruck;
                    dhkItem.MinKg = item.MinKg;
                    dhkItem.Harga = item.Harga;
                    dhkItem.IdSatuanHarga = item.IdSatuanHarga;
                    dhkItem.IsAsuransi = item.IsAsuransi;
                    dhkItem.IsAdHoc = item.IsAdHoc;
                    dhkItem.PihakPenanggung = item.PihakPenanggung;
                    dhkItem.TipeNilaiTanggungan = item.TipeNilaiTanggungan;
                    dhkItem.NilaiTanggungan = item.NilaiTanggungan;
                    dhkItem.Premi = item.Premi;
                    dhkItem.Keterangan = item.Keterangan;
                    ListAnuTeuDiHapus.Add(item.Id);
                }
                else {
                    dbitem.DaftarHargaOnCallItem.Add(new Context.DaftarHargaOnCallItem()
                    {
                        NamaRuteDaftarHarga = item.NamaRuteDaftarHarga,
                        ListIdRute = item.ListIdRute,
                        ListNamaRute = item.ListNamaRute,
                        IdJenisTruck = item.IdJenisTruck,
                        MinKg = item.MinKg,
                        Harga = item.Harga,
                        IdSatuanHarga = item.IdSatuanHarga,
                        IsAsuransi = item.IsAsuransi,
                        IsAdHoc = item.IsAdHoc,
                        PihakPenanggung = item.PihakPenanggung,
                        TipeNilaiTanggungan = item.TipeNilaiTanggungan,
                        NilaiTanggungan = item.NilaiTanggungan,
                        Premi = item.Premi,
                        Keterangan = item.Keterangan
                    });
                }
            }
            //hapus anu teu dipilih
            foreach (Context.DaftarHargaOnCallItem dbhapus in DummyItems)
	        {
                if (!ListAnuTeuDiHapus.Any(d => d == dbhapus.Id))
                {
                    dbitem.DaftarHargaOnCallItem.Remove(dbhapus);
                }
	        }

            //kondisi
            dbitem.DaftarHargaOnCallKondisi.Clear();
            foreach (DaftarHargaKondisi item in listKondisi.Where(d=>d.IsDelete == false))
            {
                dbitem.DaftarHargaOnCallKondisi.Add(new Context.DaftarHargaOnCallKondisi()
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
            dbitem.DaftarHargaOnCallAttachment.Clear();
            DaftarHargaOnCallAttachment[] resultAtt = JsonConvert.DeserializeObject<DaftarHargaOnCallAttachment[]>(StrAttachment);
            foreach (DaftarHargaOnCallAttachment item in resultAtt)
            {
                dbitem.DaftarHargaOnCallAttachment.Add(new Context.DaftarHargaOnCallAttachment()
                {
                    FileName = item.FileName,
                    RFileName = item.RFileName,
                });
            }
        }
    }

    public class DaftarHargaOnCallItem
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public string NamaRuteDaftarHarga { get; set; }
        public string ListIdRute { get; set; }
        public string ListNamaRute { get; set; }
        public int IdJenisTruck { get; set; }
        public string NamaJenisTruck { get; set; }
        public int MinKg { get; set; }
        public Decimal Harga { get; set; }
        public int IdSatuanHarga { get; set; }
        public string SatuanHarga { get; set; }
        public bool IsAdHoc { get; set; }
        public bool IsAsuransi { get; set; }
        public String PihakPenanggung { get; set; }
        public String TipeNilaiTanggungan { get; set; }
        public Decimal? Premi { get; set; }
        public Decimal? NilaiTanggungan { get; set; }
        public string Keterangan { get; set; }

        public DaftarHargaOnCallItem()
        {
            
        }
        public DaftarHargaOnCallItem(Context.DaftarHargaOnCallItem dbitem)
        {
            for (int i = 0; i < dbitem.ListIdRute.Split(',').Count(); i++)
            {
                string idRute = dbitem.ListIdRute.Split(',')[i];
                if (context.Rute.Where(d => d.Id.ToString() == idRute).FirstOrDefault() != null)
                ListNamaRute += context.Rute.Where(d => d.Id.ToString() == idRute).FirstOrDefault().Nama;
                if (i < dbitem.ListIdRute.Split(',').Count() - 1)
                    ListNamaRute += ", ";
            }
            Id = dbitem.Id;
            NamaRuteDaftarHarga = dbitem.NamaRuteDaftarHarga;
            ListIdRute = dbitem.ListIdRute;
            NamaJenisTruck = dbitem.JenisTrucks.StrJenisTruck;
            IdJenisTruck = dbitem.IdJenisTruck;
            MinKg = dbitem.MinKg;
            Harga = dbitem.Harga;
            IdSatuanHarga = dbitem.IdSatuanHarga;
            SatuanHarga = dbitem.LookupCodeSatuan.Nama;
            IsAdHoc = dbitem.IsAdHoc;
            IsAsuransi = dbitem.IsAsuransi;
            PihakPenanggung = dbitem.PihakPenanggung;
            TipeNilaiTanggungan = dbitem.TipeNilaiTanggungan;            
            NilaiTanggungan = (dbitem.NilaiTanggungan.HasValue) ? dbitem.NilaiTanggungan.Value : 0;
            Premi = (dbitem.Premi.HasValue) ? dbitem.Premi.Value : 0;
            Keterangan = dbitem.Keterangan;
        }
    }

    public class DaftarHargaOnCallAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string RFileName { get; set; }

        public DaftarHargaOnCallAttachment()
        {
            
        }
        public DaftarHargaOnCallAttachment(Context.DaftarHargaOnCallAttachment dbitem)
        {
            Id = dbitem.Id;
            FileName = dbitem.FileName;
            RFileName = dbitem.RFileName;
        }
    }
}