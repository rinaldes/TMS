using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Linq;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class DatatruckRepo : IDataTruckRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DataTruck dbitem, int id){
            if (dbitem.Id == 0) //create
            {
                context.DataTruck.Add(dbitem);
                var query = "INSERT INTO dbo.\"DataTruck\" (\"NoTruck\", \"VehicleNo\", \"IdMerk\", \"IdJenisTruck\", \"TahunBuat\", \"TahunBeli\", \"IdPool\", \"IdUnit\", \"Keterangan\", \"Kondisi\", \"SpecModel\", " +
                    "\"KmLimit\", \"NoMesin\", \"NoRangka\", \"GaransiStr\", \"GaransiEnd\", \"SpecKeterangan\", \"AtasNama\", \"BPKB\", \"urlBPKB\", \"fnameBPKB\", \"keteranganBPKB\", \"STNK\", \"urlSTNK\", \"fnameSTNK\"," +
                    "\"keteranganSTNK\", \"KIR\", \"urlKIR\", \"fnameKIR\", \"keteranganKIR\", \"KIU\", \"urlKIU\", \"fnameKIU\", \"keteranganKIU\", \"IBM\", \"urlIBM\", \"fnameIBM\", \"keteranganIBM\", \"Asuransi\", " +
                    "\"urlAsuransi\", \"fnameAsuransi\", \"keteranganAsuransi\", \"Reklame\", \"urlReklame\", \"fnameReklame\", \"keteranganReklame\", \"NoPolis\", \"urlNoPolis\", \"fnameNoPolis\", \"keteranganNoPolis\", " +
                    "\"Peminjam\", \"urlPeminjam\", \"fnamePeminjam\", \"keteranganPeminjam\", \"Leasing\", \"urlLeasing\", \"fnameLeasing\", \"keteranganLeasing\", urutan) VALUES (" + dbitem.NoTruck + ", " + 
                    dbitem.VehicleNo + ", " + dbitem.IdMerk + ", " + dbitem.IdJenisTruck + ", " + dbitem.TahunBuat + ", " + dbitem.TahunBeli + ", " + dbitem.IdPool + ", " + dbitem.IdUnit + ", " + dbitem.Keterangan + ", " + 
                    dbitem.Kondisi + ", " + dbitem.SpecModel + ", " + dbitem.KmLimit + ", " + dbitem.NoMesin + ", " + dbitem.NoRangka + ", " + dbitem.GaransiStr + ", " + dbitem.GaransiEnd + ", " + dbitem.SpecKeterangan +
                    ", " + dbitem.AtasNama + ", " + dbitem.BPKB + ", " + dbitem.urlBPKB + ", " + dbitem.fnameBPKB + ", " + dbitem.keteranganBPKB + ", " + dbitem.STNK + ", " + dbitem.urlSTNK + ", " + dbitem.fnameSTNK + ", " + 
                    dbitem.keteranganSTNK + ", " + dbitem.KIR + ", " + dbitem.urlKIR + ", " + dbitem.fnameKIR + ", " + dbitem.keteranganKIR + ", " + dbitem.KIU + ", " + dbitem.urlKIU + ", " + dbitem.fnameKIU + ", " + 
                    dbitem.keteranganKIU + ", " + dbitem.IBM + ", " + dbitem.urlIBM + ", " + dbitem.fnameIBM + ", " + dbitem.keteranganIBM + ", " + dbitem.Asuransi + ", " + dbitem.urlAsuransi + ", " + dbitem.fnameAsuransi +
                    ", " + dbitem.keteranganAsuransi + ", " + dbitem.Reklame + ", " + dbitem.urlReklame + ", " + dbitem.fnameReklame + ", " + dbitem.keteranganReklame + ", " + dbitem.NoPolis + ", " + dbitem.urlNoPolis +
                    ", " + dbitem.fnameNoPolis + ", " + dbitem.keteranganNoPolis + ", " + dbitem.Peminjam + ", " + dbitem.urlPeminjam + ", " + dbitem.fnamePeminjam + ", " + dbitem.keteranganPeminjam + ", " + dbitem.Leasing +
                    ", " + dbitem.urlLeasing + ", " + dbitem.fnameLeasing + ", " + dbitem.keteranganLeasing + ", " + dbitem.urutan + ");";
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Data Truk", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.DataTruck.Attach(dbitem);
                var query = "UPDATE dbo.\"DataTruck\" SET \"NoTruck\" = " + dbitem.NoTruck + ", \"VehicleNo\" = " + dbitem.VehicleNo + ", \"IdMerk\" = " + dbitem.IdMerk + ", \"IdJenisTruck\" = \" = " + dbitem.IdJenisTruck +
                    ", \"TahunBuat\" = \" = " + dbitem.TahunBuat + ", \"TahunBeli\" = \" = " + dbitem.TahunBeli + ", \"IdPool\" = \" = " + dbitem.IdPool + ", \"IdUnit\" = \" = " + dbitem.IdUnit + ", \"Keterangan\" = " +
                    dbitem.Keterangan + ", \"Kondisi\" = \" = " + dbitem.Kondisi + ", \"SpecModel\" = \" = " + dbitem.SpecModel + ", \"KmLimit\" = \" = " + dbitem.KmLimit + ", \"NoMesin\" = \" = " + dbitem.NoMesin + 
                    ", \"NoRangka\" = " + dbitem.NoRangka + ", \"GaransiStr\" = " + dbitem.GaransiStr + ", \"GaransiEnd\" = " + dbitem.GaransiEnd + ", \"SpecKeterangan\" = \" = " + dbitem.SpecKeterangan + ", \"AtasNama\" = "+
                    dbitem.AtasNama + ", \"BPKB\" = " + dbitem.BPKB + ", \"urlBPKB\" = " + dbitem.urlBPKB + ", \"fnameBPKB\" = \" = " + dbitem.fnameBPKB + ", \"keteranganBPKB\" = " + dbitem.keteranganBPKB + ", \"STNK\" = " +
                    dbitem.STNK + ", \"urlSTNK\" = " + dbitem.urlSTNK + ", \"fnameSTNK\" = " + dbitem.fnameSTNK + ", \"keteranganSTNK\" = " + dbitem.keteranganSTNK + ", \"KIR\" = " + dbitem.KIR + ", \"urlKIR\" = " +
                    dbitem.urlKIR + ", \"fnameKIR\" = " + dbitem.fnameKIR + ", \"keteranganKIR\" = " + dbitem.keteranganKIR + ", \"KIU\" = " + dbitem.KIU + ", \"urlKIU\" = " + dbitem.urlKIU + ", \"fnameKIU\" = " +
                    dbitem.fnameKIU + ", \"keteranganKIU\" = " + dbitem.keteranganKIU + ", \"IBM\" = " + dbitem.IBM + ", \"urlIBM\" = " + dbitem.urlIBM + ", \"fnameIBM\" = " + dbitem.fnameIBM + ", \"keteranganIBM\" = " +
                    dbitem.keteranganIBM + ", \"Asuransi\" = " + dbitem.Asuransi + ", \"urlAsuransi\" = " + dbitem.urlAsuransi + ", \"fnameAsuransi\" = " + dbitem.fnameAsuransi + ", \"keteranganAsuransi\" = " +
                    dbitem.keteranganAsuransi + ", \"Reklame\" = " + dbitem.Reklame + ", \"urlReklame\" = " + dbitem.urlReklame + ", \"fnameReklame\" = " + dbitem.fnameReklame + ", \"keteranganReklame\" = " +
                    dbitem.keteranganReklame + ", \"NoPolis\" = " + dbitem.NoPolis + ", \"urlNoPolis\" = " + dbitem.urlNoPolis + ", \"fnameNoPolis\" = " + dbitem.fnameNoPolis + ", \"keteranganNoPolis\" = " +
                    dbitem.keteranganNoPolis + ", \"Peminjam\" = " + dbitem.Peminjam + ", \"urlPeminjam\" = " + dbitem.urlPeminjam + ", \"fnamePeminjam\" = " + dbitem.fnamePeminjam + ", \"keteranganPeminjam\" = " +
                    dbitem.keteranganPeminjam + ", \"Leasing\" = " + dbitem.Leasing + ", \"urlLeasing\" = " + dbitem.urlLeasing + ", \"fnameLeasing\" = " + dbitem.fnameLeasing + ", \"keteranganLeasing\" = " +
                    dbitem.keteranganLeasing + ", urutan = " + dbitem.urutan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Data Truk", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public List<DataTruck> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DataTruck> list = context.DataTruck;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataTruck>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<DataTruck>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DataTruck>("id"); //default, wajib ada atau EF error
            }

            //take & skip
            var takeList = list;
            if (skip != null && skip != 0)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null && take != 0)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            List<DataTruck> result = takeList.ToList().ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DataTruck> items = context.DataTruck;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataTruck>(filters, ref items);
            }

            return items.Count();
        }
        public DataTruck FindByPK(int? id)
        {
            return context.DataTruck.Where(d => d.Id == id).FirstOrDefault();
        }
        public DataTruck FindByName(string name)
        {
            return context.DataTruck.Where(d => d.VehicleNo == name).FirstOrDefault();
        }
        public void delete(DataTruck dbitem, int id)
        {
            context.DataTruck.Remove(dbitem);
            var query = "DELETE FROM dbo.\"DataTruck\" WHERE \"Id\" = " +dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Data Truk", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsValidDelete(int id)
        {
            DataTruck db = context.DataTruck.Where(d=>d.Id == id).FirstOrDefault();
            if (db.DataBox.Count() > 0 || db.DataGPS.Count() > 0 || db.DataPendingin.Count() > 0)
                return false;
 
            return true;
        }
        public string generateCode(int urutan)
        {
            return "TR-" + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutan()
        {
            return context.DataTruck.Count() == 0 ? 0 : context.DataTruck.Max(d => d.urutan);
        }
        public bool IsExist(string vehicleNo, int id = 0)
        {
            if (id == 0)
                return context.DataTruck.Any(d => d.VehicleNo == vehicleNo);
            else
                return context.DataTruck.Any(d => d.VehicleNo == vehicleNo && d.Id != id);
        }
    }
}