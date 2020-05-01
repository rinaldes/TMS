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
    public class TimeAlert
    {
        public int Id { get; set; }
        [Required]
        public string idProses { get; set; }
        [Required]
        public string keteranganBagian { get; set; }
        [Required]
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

        #region add model custom
        public string statusKet { get; set; }
        public string overKet { get; set; }
        public string ProsesKet { get; set; }
        public string namaUserGridAlert { get; set; }
        public string namaUserAlert { get; set; }
        public string methodAlert { get; set; }
        public string rowColorKet { get; set; }
        #endregion add model custom

        public TimeAlert()
        { }

        public TimeAlert(Context.TimeAlert dbitem)
        {
            Id = dbitem.Id;
            idProses = dbitem.idProses;
            keteranganBagian = dbitem.keteranganBagian;
            status = dbitem.status;
            idUserAlert = dbitem.idUserAlert;
            over = dbitem.over;
            overSatuan = dbitem.overSatuan;
            AlertPopup = dbitem.AlertPopup;
            AlertSound = dbitem.AlertSound;
            AlertEmail = dbitem.AlertEmail;
            rowColor = dbitem.rowColor;

            #region model custom
            ProsesKet = new alertProsesHelper()[dbitem.idProses];
            statusKet = dbitem.status == true ? "Aktif" : "Tidak Aktif";
            namaUserGridAlert = new userHelper()[dbitem.idUserAlert];
            namaUserAlert = new usernameHelper()[dbitem.idUserAlert];
            overKet = ">= " + dbitem.over + " " + dbitem.overSatuan;
            methodAlert = new alertMthodHelper()[dbitem.AlertPopup, dbitem.AlertSound, dbitem.AlertEmail];
            rowColorKet = new colorHelper()[dbitem.rowColor];
            #endregion model custom
        }
        public void setDb(Context.TimeAlert dbitem)
        {
            dbitem.Id = Id;
            dbitem.idProses = idProses;
            dbitem.keteranganBagian = keteranganBagian;
            dbitem.status = status;
            dbitem.idUserAlert = idUserAlert;
            dbitem.over = over;
            dbitem.overSatuan = overSatuan;
            dbitem.AlertPopup = AlertPopup;
            dbitem.AlertSound = AlertSound;
            dbitem.AlertEmail = AlertEmail;
            dbitem.rowColor = rowColor;

        }

    }
}