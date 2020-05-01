using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DriverPiutangHistory
    {
        #region field
        public string Keterangan { get; set; }
        public decimal? Jumlah { get; set; }
        public System.DateTime? Tanggal { get; set; }
        public decimal? Saldo { get; set; }
        public int? Id { get; set; }
        #endregion

        private ContextModel contextBiasa = new ContextModel();
        public DriverPiutangHistory()
        {

        }
        public DriverPiutangHistory(Context.pbyd_det dbitem, decimal saldo)
        {
            //dv
            Tanggal = dbitem.pby_mstr.pby_add_date;
            Jumlah = dbitem.pbyd_amount_pay;
            Keterangan = "Pencairan " + dbitem.pby_mstr.pby_code +" ("+ dbitem.pby_mstr.pby_remarks + ")";
            Saldo = Jumlah + saldo;
        }

        public DriverPiutangHistory(string penghibur, Context.cashd_det dbitem, decimal saldo)
        {
            //dv
            Tanggal = dbitem.pbyd_det.pby_mstr.pby_add_date;
            Jumlah = dbitem.cashd_refund_amount > 0 ? (dbitem.cashd_amount + dbitem.cashd_refund_amount) * -1 : dbitem.cashd_amount * -1;
            Keterangan = "Pengembalian " + dbitem.pbyd_det.pby_mstr.pby_code;
            Saldo = Jumlah + saldo;
        }

        public DriverPiutangHistory(string penghibur, string fromKlaim, Context.Klaim dbitem, decimal saldo)
        {
            //klaim
            Tanggal = dbitem.TanggalPengajuan;
            if (dbitem.BebanClaimDriver != null)
            Jumlah = decimal.Parse(dbitem.BebanClaimDriver.ToString());
            else
                Jumlah = 0;
            try
            {
                int claimType = int.Parse(dbitem.ClaimType);
                Keterangan = "Klaim " + dbitem.NoKlaim + " (" + contextBiasa.LookupCode.Where(d => d.Id == claimType).FirstOrDefault().Nama + " - " + dbitem.Keterangan + ")";
            }
            catch (Exception) {
                Keterangan = "Klaim " + dbitem.NoKlaim + " (" + dbitem.Keterangan + ")";
            }
            Saldo = Jumlah + saldo;
            Id = dbitem.Id;
        }

        public DriverPiutangHistory(Context.AdminUangJalan dbitem, decimal saldo)
        {
            //Potongan Klaim AUJ
            Tanggal = dbitem.KlaimSubmittedAt;
            Jumlah = decimal.Parse(dbitem.PotonganK.ToString())*-1;
            Keterangan = "Potongan Klaim AUJ " + dbitem.SONumber;
            Saldo = Jumlah + saldo;
            Id = dbitem.Id;
        }

        public void setDb(Context.pbyd_det dbitem)
        {
//            dbitem.pbyd_dt = pbyd_dt;
        }
    }
}