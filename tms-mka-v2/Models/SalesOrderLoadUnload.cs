using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class SalesOrderLoadUnload
    {
        public int Id { get; set; }
        public int CustId { get; set; }
        public string Alamat { get; set; }
        public string Provinsi { get; set; }
        public string Kota { get; set; }
        public string Zona { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public int urutan { get; set; }
        public bool IsSelect { get; set; }
        public string SalesOrderKonsolidasiId { get; set; }

        public SalesOrderLoadUnload()
        {

        }

        public SalesOrderLoadUnload(Context.SalesOrderOnCallLoadingAdd dbitem)
        {
            Id = dbitem.CustomerLoadingAddress.Id;
            Alamat = dbitem.CustomerLoadingAddress.Alamat;
            Provinsi = dbitem.CustomerLoadingAddress.LocProvinsi == null ? "" : dbitem.CustomerLoadingAddress.LocProvinsi.Nama;
            Kota = dbitem.CustomerLoadingAddress.LocKabKota == null ? "" : dbitem.CustomerLoadingAddress.LocKabKota.Nama;
            Zona = dbitem.CustomerLoadingAddress.Zona;
            Telp = dbitem.CustomerLoadingAddress.Telp;
            Fax = dbitem.CustomerLoadingAddress.Fax;
            urutan = dbitem.urutan;
            IsSelect = dbitem.IsSelect;
        }

        public SalesOrderLoadUnload(Context.SalesOrderOnCallUnLoadingAdd dbitem)
        {
            Id = dbitem.CustomerUnloadingAddress.Id;
            Alamat = dbitem.CustomerUnloadingAddress.Alamat;
            Provinsi = dbitem.CustomerUnloadingAddress.LocProvinsi == null ? "" : dbitem.CustomerUnloadingAddress.LocProvinsi.Nama;
            Kota = dbitem.CustomerUnloadingAddress.LocKabKota == null ? "" : dbitem.CustomerUnloadingAddress.LocKabKota.Nama;
            Zona = dbitem.CustomerUnloadingAddress.Zona;
            Telp = dbitem.CustomerUnloadingAddress.Telp;
            Fax = dbitem.CustomerUnloadingAddress.Fax;
            urutan = dbitem.urutan;
            IsSelect = dbitem.IsSelect;
        }

        public SalesOrderLoadUnload(Context.SalesOrderPickupLoadingAdd dbitem)
        {
            Id = dbitem.CustomerLoadingAddress.Id;
            Alamat = dbitem.CustomerLoadingAddress.Alamat;
            Provinsi = dbitem.CustomerLoadingAddress.LocProvinsi == null ? "" : dbitem.CustomerLoadingAddress.LocProvinsi.Nama;
            Kota = dbitem.CustomerLoadingAddress.LocKabKota == null ? "" : dbitem.CustomerLoadingAddress.LocKabKota.Nama;
            Zona = dbitem.CustomerLoadingAddress.Zona;
            Telp = dbitem.CustomerLoadingAddress.Telp;
            Fax = dbitem.CustomerLoadingAddress.Fax;
            urutan = dbitem.urutan;
            IsSelect = dbitem.IsSelect;
        }

        public SalesOrderLoadUnload(Context.SalesOrderPickupUnLoadingAdd dbitem)
        {
            Id = dbitem.CustomerUnloadingAddress.Id;
            Alamat = dbitem.CustomerUnloadingAddress.Alamat;
            Provinsi = dbitem.CustomerUnloadingAddress.LocProvinsi == null ? "" : dbitem.CustomerUnloadingAddress.LocProvinsi.Nama;
            Kota = dbitem.CustomerUnloadingAddress.LocKabKota == null ? "" : dbitem.CustomerUnloadingAddress.LocKabKota.Nama;
            Zona = dbitem.CustomerUnloadingAddress.Zona;
            Telp = dbitem.CustomerUnloadingAddress.Telp;
            Fax = dbitem.CustomerUnloadingAddress.Fax;
            urutan = dbitem.urutan;
            IsSelect = dbitem.IsSelect;
        }

        public SalesOrderLoadUnload(Context.SalesOrderProsesKonsolidasiLoadingAdd dbitem)
        {
            Id = dbitem.CustomerLoadingAddress.Id;
            CustId = dbitem.CustomerLoadingAddress.CustomerId;
            SalesOrderKonsolidasiId = dbitem.SalesOrderKonsolidasiId;
            Alamat = dbitem.CustomerLoadingAddress.Alamat;
            Provinsi = dbitem.CustomerLoadingAddress.LocProvinsi == null ? "" : dbitem.CustomerLoadingAddress.LocProvinsi.Nama;
            Kota = dbitem.CustomerLoadingAddress.LocKabKota == null ? "" : dbitem.CustomerLoadingAddress.LocKabKota.Nama;
            Zona = dbitem.CustomerLoadingAddress.Zona;
            Telp = dbitem.CustomerLoadingAddress.Telp;
            Fax = dbitem.CustomerLoadingAddress.Fax;
            urutan = dbitem.urutan;
            IsSelect = dbitem.IsSelect;
        }

        public SalesOrderLoadUnload(Context.SalesOrderProsesKonsolidasiUnLoadingAdd dbitem)
        {
            Id = dbitem.CustomerUnloadingAddress.Id;
            CustId = dbitem.CustomerUnloadingAddress.CustomerId;
            SalesOrderKonsolidasiId = dbitem.SalesOrderKonsolidasiId;
            Alamat = dbitem.CustomerUnloadingAddress.Alamat;
            Provinsi = dbitem.CustomerUnloadingAddress.LocProvinsi == null ? "" : dbitem.CustomerUnloadingAddress.LocProvinsi.Nama;
            Kota = dbitem.CustomerUnloadingAddress.LocKabKota == null ? "" : dbitem.CustomerUnloadingAddress.LocKabKota.Nama;
            Zona = dbitem.CustomerUnloadingAddress.Zona;
            Telp = dbitem.CustomerUnloadingAddress.Telp;
            Fax = dbitem.CustomerUnloadingAddress.Fax;
            urutan = dbitem.urutan;
            IsSelect = dbitem.IsSelect;
        }
    }
}