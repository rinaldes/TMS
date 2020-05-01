using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class VendorGps
    {
        public VendorGps()
        {
            this.ListKontak = new HashSet<Kontak>();
        }
        [Key]
        public int Id { get; set; }
        //public bool IsDelete { get; set; }
        public string Nama { get; set; }
        public string Merk { get; set; }
        public string Alamat{ get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public string Web { get; set; }
        
        public virtual ICollection<Kontak> ListKontak { get; set; }
    }
}