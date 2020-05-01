using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace tms_mka_v2.Models
{
    public class ERPDynamicConfig
    {
        public string CodeBiayaInap { get; set; }
        public string NamaBiayaInap { get; set; }
        public int? IdBiayaPerjalanan { get; set; }
        public string CodeBiayaPerjalanan { get; set; }
        public string NamaBiayaPerjalanan { get; set; }
        public int? IdBiayaKlaim { get; set; }
        public string CodeBiayaKlaim { get; set; }
        public string NamaBiayaKlaim { get; set; }
        public int? IdKreditKlaim { get; set; }
        public string CodeKreditKlaim { get; set; }
        public string NamaKreditKlaim { get; set; }
        public int? IdOprInapCash { get; set; }
        public string CodeOprInapCash { get; set; }
        public string NamaOprInapCash { get; set; }
        public int? IdOprInapTf { get; set; }
        public string CodeOprInapTf { get; set; }
        public string NamaOprInapTf { get; set; }
        public int? IdKasbonDriver { get; set; }
        public string CodeKasbonDriver { get; set; }
        public string NamaKasbonDriver { get; set; }
        public int? IdPiutangCustomer { get; set; }
        public string CodePiutangCustomer { get; set; }
        public string NamaPiutangCustomer { get; set; }
        public int? IdPiutangBlmInv { get; set; }
        public string CodePiutangBlmInv { get; set; }
        public string NamaPiutangBlmInv { get; set; }
        public int? IdPenjualanBlmInv { get; set; }
        public string CodePenjualanBlmInv { get; set; }
        public string NamaPenjualanBlmInv { get; set; }
        public string StrERPDynamicConfig { get; set; }
    }
}