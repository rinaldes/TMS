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
    public class DaftarHargaKondisi
    {
        public int id { get; set; }
        public string kondisi { get; set; }
        public bool IsInclude { get; set; }
        public bool IsBill { get; set; }
        public decimal? value { get; set; }
        public bool IsDefault { get; set; }
        public bool IsKota { get; set; }
        public bool IsTitik { get; set; }
        public decimal? ValKota { get; set; }
        public decimal? ValTitik { get; set; }
        public bool IsDelete { get; set; }

        public DaftarHargaKondisi()
        {

        }

        public DaftarHargaKondisi(Context.DaftarHargaOnCallKondisi dbitem)
        {
            kondisi = dbitem.kondisi;
            IsInclude = dbitem.IsInclude;
            IsBill = dbitem.IsBill;
            value = dbitem.value;
            IsDefault = dbitem.IsDefault;
            IsKota = dbitem.IsKota;
            IsTitik = dbitem.IsTitik;
            ValKota = dbitem.ValKota;
            ValTitik = dbitem.ValTitik;
            IsDelete = dbitem.IsDelete;
        }
        public DaftarHargaKondisi(Context.DaftarHargaKontrakKondisi dbitem)
        {
            kondisi = dbitem.kondisi;
            IsInclude = dbitem.IsInclude;
            IsBill = dbitem.IsBill;
            value = dbitem.value;
            IsDefault = dbitem.IsDefault;
            IsKota = dbitem.IsKota;
            IsTitik = dbitem.IsTitik;
            ValKota = dbitem.ValKota;
            ValTitik = dbitem.ValTitik;
            IsDelete = dbitem.IsDelete;
        }
        public DaftarHargaKondisi(Context.DaftarHargaKonsolidasiKondisi dbitem)
        {
            kondisi = dbitem.kondisi;
            IsInclude = dbitem.IsInclude;
            IsBill = dbitem.IsBill;
            value = dbitem.value;
            IsDefault = dbitem.IsDefault;
            IsKota = dbitem.IsKota;
            IsTitik = dbitem.IsTitik;
            ValKota = dbitem.ValKota;
            ValTitik = dbitem.ValTitik;
            IsDelete = dbitem.IsDelete;
        }
        
        public void SetDb(Context.DaftarHargaOnCallKondisi dbitem)
        {
            dbitem.kondisi = kondisi;
            dbitem.IsInclude = IsInclude;
            dbitem.IsBill = IsBill;
            dbitem.value = value;
            dbitem.IsDefault = IsDefault;
            dbitem.IsKota = IsKota;
            dbitem.IsTitik = IsTitik;
            dbitem.ValKota = ValKota;
            dbitem.ValTitik = ValTitik;
            dbitem.IsDelete = IsDelete;
        }

        public static void GenerateKondisi(List<DaftarHargaKondisi> item)
        {
            item.Add(new DaftarHargaKondisi() { 
                kondisi = "Driver",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Operasional ( Solar / Tol / Kapal )",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Ongkos Bongkar Muat",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Karantina",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Inap",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Helper",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Multidrop",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Sticker",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Handling",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Kawalan",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Timbangan",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "Biaya Pluging",
                IsDefault = true,
            });
            item.Add(new DaftarHargaKondisi()
            {
                kondisi = "PPFTZ",
                IsDefault = true,
            });
        }
    }
}