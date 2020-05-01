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
    public class VendorGps
    {
        public int Id { get; set; }
        public string strVendor { get; set; }
        //public bool IsDelete { get; set; }
        [Display(Name = "Merk")]
        [RegularExpression("[a-zA-Z0-9 ]+", ErrorMessage = "Format Nama tidak valid")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Merk { get; set; }
        [Display(Name = "Nama Vendor")]
        [RegularExpression("[a-zA-Z0-9 ]+", ErrorMessage = "Format Nama tidak valid")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Nama { get; set; }
        [Display(Name = "Alamat")]
        [RegularExpression("[a-zA-Z0-9.,-/ ]+", ErrorMessage = "Format Alamat tidak valid")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Alamat { get; set; }
        [Display(Name = "E-Mail")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [EmailAddress(ErrorMessage = "E-Mail Tidak Valid.")]
        public string Email { get; set; }
        [Display(Name = "Telepon")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string Telp { get; set; }
        [Display(Name = "Website")]
        [RegularExpression(@"(http://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessage = "Website tidak valid.")]
        public string Web { get; set; }
        public List<Kontak> ListKontak { get; set; }
        public VendorGps()
        {
            ListKontak = new List<Kontak>();
        }
        public VendorGps(Context.VendorGps dbitem)
        {
            Id = dbitem.Id;
            Merk = dbitem.Merk;
            Nama = dbitem.Nama;
            Alamat = dbitem.Alamat;
            Email = dbitem.Email;
            Telp = dbitem.Telp;
            Web = dbitem.Web;
            ListKontak = new List<Kontak>();
            foreach (Context.Kontak item in dbitem.ListKontak.ToList())
            {
                ListKontak.Add(new Kontak(item));
            }
        }
        public void setDb(Context.VendorGps dbitem)
        {
            dbitem.Id = Id;
            dbitem.Merk = Merk;
            dbitem.Nama = Nama;
            dbitem.Alamat = Alamat;
            dbitem.Email = Email;
            dbitem.Telp = Telp;
            dbitem.Web = Web;

            dbitem.ListKontak.Clear();

            Kontak[] result = JsonConvert.DeserializeObject<Kontak[]>(strVendor);

            foreach (Kontak item in result)
            {
                dbitem.ListKontak.Add(new Context.Kontak()
                {
                    Nama = item.Nama,
                    IdJabatan = item.IdJabatan,
                    Hp = item.Hp,
                    Email = item.Email
                });
            }
        }
    }
    public class Kontak
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public int? IdJabatan { get; set; }
        public string NamaJabatan { get; set; }
        public string Hp { get; set; }
        public string Email { get; set; }
        public Kontak()
        {

        }

        public Kontak(Context.Kontak dbitem)
        {
            Id = dbitem.Id;
            Nama = dbitem.Nama;
            IdJabatan = dbitem.IdJabatan;
            NamaJabatan = dbitem.LookUpCodeJabatan.Nama;
            Hp = dbitem.Hp;
            Email = dbitem.Email;
        }
    }
}