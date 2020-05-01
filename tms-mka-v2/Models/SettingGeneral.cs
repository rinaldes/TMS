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
    public class SettingGeneral
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

        public SettingGeneral()
        { }

        public SettingGeneral(Context.SettingGeneral dbitem)
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
            ProsesKet = new prosesHelper()[dbitem.idProses];
            statusKet = dbitem.status == true ? "Aktif" : "Tidak Aktif";
            namaUserGridAlert = new userHelper()[dbitem.idUserAlert];
            namaUserAlert = new usernameHelper()[dbitem.idUserAlert];
            overKet = ">= " + dbitem.over + " " + dbitem.overSatuan;
            methodAlert = new alertMthodHelper()[dbitem.AlertPopup, dbitem.AlertSound, dbitem.AlertEmail];
            rowColorKet = new colorHelper()[dbitem.rowColor];
            #endregion model custom
        }

    }

    public class prosesHelper
    {
        private static Dictionary<string, string> proses = new Dictionary<string, string>
        {
            { "Speed Alert", "Speed Alert" },
            { "Parking Alert", "Parking Alert" },
            { "Revisi Rute Alert", "Revisi Rute Alert" }
        };

        public string this[string idProses]
        {
            get
            {
                return proses.FirstOrDefault(p => p.Key.Contains(idProses)).Value ?? "";
            }
        }
    }

    public class alertProsesHelper
    {
        private static Dictionary<string, string> proses = new Dictionary<string, string>
        {
            { "S", "Sales Order" },
            { "P", "Planning Order" },
            { "KP", "Konfirmasi Planning" },
            { "A", "Admin Uang Jalan" },
            { "K", "Kasir" }
        };

        public string this[string idProses]
        {
            get
            {
                return proses.FirstOrDefault(p => p.Key.Contains(idProses)).Value ?? "";
            }
        }
    }

    public class userHelper
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

    public class usernameHelper
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

    public class alertMthodHelper
    {
        string mthodKet;
        public bool AlertPopup { get; set; }
        public bool AlertSound { get; set; }
        public bool AlertEmail { get; set; }

        public string this[bool AlertPopup, bool AlertSound, bool AlertEmail]
        {
            get
            {
                mthodKet = AlertPopup == true ? "Popup" : "";
                mthodKet = mthodKet != "" ? ( AlertSound == true ? mthodKet + ", Sound" : mthodKet) : (AlertSound == true ? "Sound" : "");
                mthodKet = mthodKet != "" ? ( AlertEmail == true ? mthodKet + ", Email" : mthodKet) : (AlertEmail == true ? "Email" : "");

                return mthodKet;
            }
        }
    }

    public class colorHelper
    {
        private static Dictionary<string, string> proses = new Dictionary<string, string>
        {
            { "K", "Kuning" },
            { "M", "Merah" },
            { "H", "Hijau" }
        };

        public string this[string idProses]
        {
            get
            {
                return proses.FirstOrDefault(p => p.Key.Contains(idProses)).Value ?? "";
            }
        }
    }

}