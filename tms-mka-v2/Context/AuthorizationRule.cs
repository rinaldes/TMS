using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace tms_mka_v2.Context
{
    public class AuthorizationRule
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string idProses { get; set; }
        [Required]
        public string keteranganBagian { get; set; }
        public bool statusAktif { get; set; }
        [Required]
        public int frekuensi { get; set; }
        [Required]
        public string frekuensiSatuan { get; set; }
        [Required]
        public string idUserOtoritas1 { get; set; }
        public string idUserOtoritas2 { get; set; }
        public bool AlertPopup { get; set; }
        public bool AlertFingerPrint { get; set; }
        public bool AlertEmail { get; set; }
        public bool AlertPassword { get; set; }

    }
}