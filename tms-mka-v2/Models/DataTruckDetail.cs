using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class DataTruckDetail
    {
        private ContextModel context = new ContextModel();
        public int Id { get; set; }
        public string StatusOrder { get; set; }
        public string StatusTruk { get; set; }
        public string VehicleNo { get; set; }
        public string Merk { get; set; }
        public string JenisTruk { get; set; }
        public string Pendingin { get; set; }
        public string Lantai { get; set; }
        public string Dinding { get; set; }
        public string AlokasiPool { get; set; }
        public string AlokasiUnit { get; set; }
        public string KondisiKhusus { get; set; }
        public string DokumenExp { get; set; }
        public int IdDriver1 { get; set; }
        public string KodeDriver1 { get; set; }
        public string NamaDriver1 { get; set; }
        public string StatusDriver1 { get; set; }
        public int IdDriver2 { get; set; }
        public string KodeDriver2 { get; set; }
        public string NamaDriver2 { get; set; }
        public string StatusDriver2 { get; set; }

        public DataTruckDetail()
        {

        }
        public DataTruckDetail(Context.DataTruck dbitem)
        {
            Id = dbitem.Id;
            //cek status order
            StatusOrder = "";
            StatusTruk = "Available";
            if (context.SalesOrder.Where(d => (d.Status == "save" || d.Status == "draft planning") && d.DataTruckId == dbitem.Id).Count()>1)
            { 
                StatusOrder = "Planning";
                StatusTruk = "On Duty";
            }
            else if (context.SalesOrder.Where(d => (d.Status == "save planning" || d.Status == "draft konfirmasi") && d.DataTruckId == dbitem.Id).Count()>1)
            {
                StatusOrder = "Konfirmasi";
                StatusTruk = "On Duty";
            }
            else if (context.SalesOrder.Where(d => (d.Status == "save konfirmasi" || d.Status == "dispatched") && d.DataTruckId == dbitem.Id).Count()>1)
            {
                StatusOrder = "Dispatched";
                StatusTruk = "On Duty";
            }
            VehicleNo = dbitem.VehicleNo;
            Merk = dbitem.IdMerk != null ? dbitem.LookupCodeMerk.Nama : "";
            JenisTruk = !dbitem.IdJenisTruck.HasValue ? "" : dbitem.JenisTrucks.StrJenisTruck;
            if (dbitem.DataPendingin.Count > 0)
            {
                Context.DataPendingin dbpendingin = dbitem.DataPendingin.FirstOrDefault();
                Pendingin = dbpendingin.Merk + " - " + dbpendingin.Model;
            }
            if (dbitem.DataBox.Count > 0)
            {
                Context.DataBox dbbox = dbitem.DataBox.FirstOrDefault();
                List<string> _lantai = new List<string>();
                List<string> _dinding = new List<string>();

                if (dbbox.Lantai != null && dbbox.Lantai != "")
                    _lantai.Add(dbbox.Lantai);
                if (dbbox.Dinding != null && dbbox.Dinding != "")
                    _dinding.Add(dbbox.Dinding);
                foreach (DataBoxLantai item in dbbox.DataBoxLantai)
                {
                    _lantai.Add(item.LookupCodeLantaiCode.Nama);
                }
                foreach (DataBoxDinding item in dbbox.DataBoxDinding)
                {
                    _dinding.Add(item.LookupCodeDinding.Nama);
                }
                if (_lantai.Count() > 0)
                    Lantai = string.Join(", ", _lantai);
                if (_dinding.Count() > 0)
                    Dinding = string.Join(", ", _dinding);
            }
            KondisiKhusus = dbitem.Kondisi;
            List<string> _dok = new List<string>();
            if (dbitem.STNK != null && dbitem.STNK.Value >= DateTime.Now)
                _dok.Add("STNK");
            if (dbitem.KIR != null && dbitem.KIR.Value >= DateTime.Now)
                _dok.Add("KIR");
            if (dbitem.KIU != null && dbitem.KIU.Value >= DateTime.Now)
                _dok.Add("KIU/SIPA");
            if (dbitem.IBM != null && dbitem.IBM.Value >= DateTime.Now)
                _dok.Add("IBM");
            if (dbitem.Asuransi != null && dbitem.Asuransi.Value >= DateTime.Now)
                _dok.Add("Asuransi");
            if (dbitem.Reklame != null && dbitem.Reklame.Value >= DateTime.Now)
                _dok.Add("Pajak Reklame");
            if (_dok.Count > 0)
                DokumenExp = string.Join(", ", _dok);

            Context.PenetapanDriver dbPenetapan = context.PenetapanDriver.Where(d => d.IdDataTruck == dbitem.Id).FirstOrDefault();
            if (dbPenetapan != null)
            {
                IdDriver1 = dbPenetapan.IdDriver1.Value;
                KodeDriver1 = dbPenetapan.Driver1.KodeDriver;
                NamaDriver1 = dbPenetapan.Driver1.NamaDriver;
                StatusDriver1 = dbPenetapan.Driver1.LookupCodeStatus.Nama;
                if (dbPenetapan.IdDriver2.HasValue)
                {
                    IdDriver2 = dbPenetapan.IdDriver2.Value;
                    KodeDriver2 = dbPenetapan.Driver2.KodeDriver;
                    NamaDriver2 = dbPenetapan.Driver2.NamaDriver;
                    StatusDriver2 = dbPenetapan.Driver2.LookupCodeStatus.Nama;
                }
            }
        }
    }
}