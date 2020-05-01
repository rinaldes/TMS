using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace tms_mka_v2.Context
{
    public class SettingGeneral
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string idProses { get; set; }
        [Required]
        public string keteranganBagian { get; set; }
        public bool status { get; set; }
        [Required]
        public int over { get; set; }
        [Required]
        public string overSatuan { get; set; }
        [Required]
        public string idUserAlert { get; set; }
        public bool AlertPopup { get; set; }
        public bool AlertSound { get; set; }
        public bool AlertEmail { get; set; }
        [Required]
        public string rowColor { get; set; }



        //#region start hanya untuk catatan saja

        //[NotMapped]
        //public string statusKet { get; set; }
        //[NotMapped]
        //public string overKet { get; set; }

        //#endregion end hanya untuk catatan saja

    }
}