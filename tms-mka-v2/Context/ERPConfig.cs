using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class ERPConfig
    {
        public ERPConfig()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public int? IdKawalan { get; set; }
        public int? IdTimbangan { get; set; }
        public int? IdKarantina { get; set; }
        public int? IdSPSI { get; set; }
        public int? IdMultidrop { get; set; }
        public int? IdSolar { get; set; }
        public int? IdKapal { get; set; }
        public int? IdTambahanRuteMuat { get; set; }
        public int? IdTambahanRuteLain { get; set; }
        public int? IdAUJCredit { get; set; }
        public int? IdKasbonAuj { get; set; }
        public int? IdKlaimAuj { get; set; }
        public int? IdPotonganLainAuj { get; set; }
        public int? IdKasirCash { get; set; }
        public int? IdCashCredit { get; set; }
        public int? IdKasirTf { get; set; }
        public int? IdTfCredit { get; set; }
        public int? IdBiayaInap { get; set; }
        public int? IdOprInapCash { get; set; }
        public int? IdOprInapTf { get; set; }
        public int? IdKasbonDriver { get; set; }
        public int? IdBoronganDasar { get; set; }
        public int? IdVoucherSolar { get; set; }
        public int? IdVoucherKapal { get; set; }
        public int? IdBiayaPerjalanan { get; set; }
        public int? IdPiutangCustomer { get; set; }
        public int? IdDP { get; set; }
        public int? IdBiayaKlaim { get; set; }
        public int? IdKreditKlaim { get; set; }
        public int? IdBiayaKlaim2 { get; set; }
        public int? IdKreditKlaim2 { get; set; }
        public int? IdPiutangDagang { get; set; }
        public int? IdPendapatanUsahaBlmInv { get; set; }
        public int? IdPiutangDailySupir { get; set; }
        public int? IdKlaimSupir { get; set; }
        public int? IdPiutangDriverBatalJalan { get; set; }
        public int? IdPiutangDriverPribadi { get; set; }
        public int? IdPendapatanPengembalianPiutangDriver { get; set; }
        public int? IdTabunganDriver { get; set; }
        public int? IdBiayaBatalJalan { get; set; }
        public int? IdPiutangDriverBatalJalanSementaraSolar { get; set; }
        public int? IdPendapatanPosSementara { get; set; }
        public int? IdUangDM { get; set; }
    }
}