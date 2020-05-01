using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;
using System.Text.RegularExpressions;
using tms_mka_v2.Business_Logic.Abstract;

namespace tms_mka_v2.Models
{
    public class Inventaris
    {
        public int Id { get; set; }
        public int IdNamaBarang { get; set; }
        public string NamaBarang { get; set; }
        public string Keterangan { get; set; }
        public int DriverId { get; set; }
        public DateTime TanggalPemberian { get; set; }
        public DateTime TanggalPengembalian { get; set; }
        public string StrTanggalPemberian { get; set; }
        public string StrTanggalPengembalian { get; set; }

        public Inventaris()
        { }

        public Inventaris(Context.Inventaris dbitem)
        {
            Id = dbitem.Id;
            NamaBarang = dbitem.LookupCode.Nama;
            Keterangan = dbitem.Keterangan;
            TanggalPemberian = dbitem.TanggalPemberian;
            TanggalPengembalian = dbitem.TanggalPengembalian;
            StrTanggalPemberian = dbitem.TanggalPemberian.ToString();
            StrTanggalPengembalian = dbitem.TanggalPengembalian.ToString();
            IdNamaBarang = dbitem.IdNamaBarang;
        }
        public void setDb(Context.Inventaris dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdNamaBarang = IdNamaBarang;
            dbitem.Keterangan = Keterangan;
            dbitem.DriverId = DriverId;
            dbitem.TanggalPemberian = TanggalPemberian;
            dbitem.TanggalPengembalian = TanggalPengembalian;
        }
    }
}