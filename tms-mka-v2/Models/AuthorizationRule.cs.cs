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
    public class AuthorizationRule
    {
        public int Id { get; set; }
        [Required]
        public string idProses { get; set; }
        [Required]
        public string keteranganBagian { get; set; }
        [Required]
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

        #region add model custom
        public string statusKet { get; set; }
        public string frekuensiKet { get; set; }
        public string ProsesKet { get; set; }
        public string namaUserGridOtoritas1 { get; set; }
        public string namaUserGridOtoritas2 { get; set; }
        public string namaUserOtoritas1 { get; set; }
        public string namaUserOtoritas2 { get; set; }
        public string methodAlert { get; set; }
        #endregion add model custom

        public AuthorizationRule()
        { }

        public AuthorizationRule(Context.AuthorizationRule dbitem)
        {
            Id = dbitem.Id;
            idProses = dbitem.idProses;
            keteranganBagian = dbitem.keteranganBagian;
            statusAktif = dbitem.statusAktif;
            idUserOtoritas1 = dbitem.idUserOtoritas1;
            idUserOtoritas2 = dbitem.idUserOtoritas2;
            frekuensi = dbitem.frekuensi;
            frekuensiSatuan = dbitem.frekuensiSatuan;
            AlertPopup = dbitem.AlertPopup;
            AlertPassword = dbitem.AlertPassword;
            AlertEmail = dbitem.AlertEmail;
            AlertFingerPrint = dbitem.AlertFingerPrint;

            #region model custom
            ProsesKet = new prosesAuthorizationRuleHelper()[dbitem.idProses];
            statusKet = dbitem.statusAktif == true ? "Aktif" : "Tidak Aktif";
            namaUserGridOtoritas1 = new userAuthorizationRuleHelper()[dbitem.idUserOtoritas1];
            namaUserOtoritas1 = new usernameAuthorizationRuleHelper()[dbitem.idUserOtoritas1];
            namaUserGridOtoritas2 = new userAuthorizationRuleHelper()[dbitem.idUserOtoritas2];
            namaUserOtoritas2 = new usernameAuthorizationRuleHelper()[dbitem.idUserOtoritas2];
            frekuensiKet = ">= " + dbitem.frekuensi + " " + dbitem.frekuensiSatuan;
            methodAlert = new alertMthodAuthorizationRuleHelper()[dbitem.AlertPopup, dbitem.AlertFingerPrint, dbitem.AlertPassword, dbitem.AlertEmail];
            #endregion model custom
        }

    }

    public class prosesAuthorizationRuleHelper
    {
        private static Dictionary<string, string> proses = new Dictionary<string, string>
        {
            { "S", "Speed Alert" },
            { "P", "Parking Alert" },
            { "R", "Revisi Rute Alert" }
        };

        public string this[string idProses]
        {
            get
            {
                return proses.FirstOrDefault(p => p.Key.Contains(idProses)).Value ?? "";
            }
        }
    }

    public class userAuthorizationRuleHelper
    {
        private ContextModel context = new ContextModel();
        string userAlert;
        int id = 1;

        public string this[string idUserAlert]
        {

            get
            {
                string[] idUsers = Regex.Split(idUserAlert, ",");

                foreach (string idUser in idUsers)
                {
                    id = Convert.ToInt32(idUser);

                    Context.User dbitem = context.User.Where(d => d.Id == id).FirstOrDefault();
                    User model = new User(dbitem);
                    userAlert += id + "|" + model.Username + ",";
                }

                userAlert = userAlert.Trim(',');

                return userAlert;
            }
        }
    }

    public class usernameAuthorizationRuleHelper
    {
        private ContextModel context = new ContextModel();
        string userAlert;
        int id = 1;

        public string this[string idUserAlert]
        {

            get
            {
                string[] idUsers = Regex.Split(idUserAlert, ",");

                foreach (string idUser in idUsers)
                {
                    id = Convert.ToInt32(idUser);

                    Context.User dbitem = context.User.Where(d => d.Id == id).FirstOrDefault();
                    User model = new User(dbitem);
                    userAlert += model.Username + ",";
                }

                userAlert = userAlert.Trim(',');

                return userAlert;
            }
        }
    }

    public class alertMthodAuthorizationRuleHelper
    {
        string mthodKet;
        public bool AlertPopup { get; set; }
        public bool AlertFingerPrint { get; set; }
        public bool AlertPassword { get; set; }
        public bool AlertEmail { get; set; }

        public string this[bool AlertPopup, bool AlertFingerPrint, bool AlertPassword, bool AlertEmail]
        {
            get
            {
                mthodKet = AlertPopup == true ? "Popup" : "";
                mthodKet = mthodKet != "" ? (AlertFingerPrint == true ? mthodKet + ", Finger Print" : mthodKet) : (AlertFingerPrint == true ? "Finger Print" : "");
                mthodKet = mthodKet != "" ? (AlertPassword == true ? mthodKet + ", Password" : mthodKet) : (AlertPassword == true ? "Password" : "");
                mthodKet = mthodKet != "" ? (AlertEmail == true ? mthodKet + ", Email" : mthodKet) : (AlertEmail == true ? "Email" : "");

                return mthodKet;
            }
        }
    }
}