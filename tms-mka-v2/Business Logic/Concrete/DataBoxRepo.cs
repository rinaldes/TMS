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
    public class DataBoxRepo : IDataBoxRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DataBox dbitem, int id, DataBoxHistory dbh, DataTruckBoxHistory dtbh=null, DataTruckGPSHistory dtgh=null){
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.DataBox.Add(dbitem);
                query += "INSERT INTO dbo.\"DataBox\" (\"NoBox\", \"IdDataTruck\", \"Karoseri\", \"Tahun\", \"IdType\", \"IdKategori\", \"Lantai\", \"Dinding\", \"PintuSamping\", \"Sekat\", \"garansiStr\", " +
                    "\"garansiEnd\", \"asuransiStr(\"garansiEnd\", asuransiEnd\"garansiEnd\", tglPasang\"garansiEnd\", \"Urutan\") VALUES (" + dbitem.NoBox + ", " + dbitem.IdDataTruck + ", " + dbitem.Karoseri + ", " + 
                    dbitem.Tahun + ", " + dbitem.IdType + ", " + dbitem.IdKategori + ", " + dbitem.Lantai + ", " + dbitem.Dinding + ", " + dbitem.PintuSamping + ", " + dbitem.Sekat + ", " + dbitem.garansiStr + ", " + 
                    dbitem.garansiEnd + ", " + dbitem.asuransiStr + ", " + dbitem.asuransiEnd + ", " + dbitem.tglPasang + ", " + dbitem.Urutan + ");";
            }
            else //edit
            {
                context.DataBox.Attach(dbitem);
                query += "UPDATE dbo.\"DataBox\" SET \"NoBox\" = " + dbitem.NoBox + ",\"IdDataTruck\" = " + dbitem.IdDataTruck + " ,\"Karoseri\" = " + dbitem.Karoseri + ",\"Tahun\" = " + dbitem.Tahun + ", \"IdType\" = " +
                    dbitem.IdType + ", \"IdKategori\" = " + dbitem.IdKategori + ", \"Lantai\" = " + dbitem.Lantai + ", \"Dinding\" = " + dbitem.Dinding + ", \"PintuSamping\" = " + dbitem.PintuSamping + ", \"Sekat\" = " + 
                    dbitem.Sekat + ", \"garansiStr\" = " + dbitem.garansiStr + ", \"garansiEnd\" = " + dbitem.garansiEnd + ", \"asuransiStr\" = \" = " + dbitem.asuransiStr + ", \"asuransiEnd\" = \" = " + dbitem.asuransiEnd +
                    ", \"tglPasang\" = " + dbitem.tglPasang + ", \"Urutan\" = " + dbitem.Urutan + "WHERE \"Id\" = " + dbitem.Id + ";";
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            query += "DELETE FROM dbo.\"DataBoxDinding\" WHERE \"IdDataBox\" = " + dbitem.Id + ";";
            foreach(Context.DataBoxDinding dbd in dbitem.DataBoxDinding){
                query += "INSERT INTO dbo.\"DataBoxDinding\" (\"IdDataBox\", \"IdDindingCode\") VALUES (" + dbitem.Id + ", " + dbd.IdDindingCode + ");";
            }
            query += "DELETE FROM dbo.\"DataBoxLantai\" WHERE \"IdDataBox\" = " + dbitem.Id + ";";
            foreach(Context.DataBoxLantai dbd in dbitem.DataBoxLantai){
                query += "INSERT INTO dbo.\"DataBoxLantai\" (\"IdDataBox\", \"IdLantaiCode\") VALUES (" + dbitem.Id + ", " + dbd.IdLantaiCode + ");";
            }
            query += "INSERT INTO dbo.\"DataBoxHistory\" (\"IdDataBox\", \"Vehicle\", \"NoBox\", \"Karoseri\", \"Tahun\", \"strType\", \"strKategori\", \"Lantai\", \"Dinding\", \"PintuSamping\", \"Sekat\", \"garansiStr\", " +
                "\"garansiEnd\", \"asuransiStr\", \"asuransiEnd\", \"tglPasang\", \"Tanggal, username) VALUES (" + dbitem.Id + ", " + dbitem.IdDataTruck + ", " + dbitem.NoBox + ", " + dbitem.Karoseri + ", " + dbitem.Tahun +
                ", " + dbh.strType + ", " + dbh.strKategori + ", " + dbitem.Lantai + ", " + dbitem.Dinding + ", " + dbitem.PintuSamping + ", " + dbitem.Sekat + ", " + dbitem.garansiStr + ", " + dbitem.garansiEnd +
                ", " + dbitem.asuransiStr + ", " + dbitem.asuransiEnd + ", " + dbitem.tglPasang + ", " + dbh.Tanggal + ", " + dbh.username + ");";
            if (dtbh != null){
                query += "INSERT INTO dbo.\"DataTruckBoxHistory\" (\"IdDataTruck\", \"Vehicle\", \"NoBox\", \"Karoseri\", \"Tahun\", \"strType\", \"strKategori\", \"Lantai\", \"Dinding\", \"PintuSamping\", \"Sekat\", " +
                    "\"garansiStr\", \"garansiEnd\", \"asuransiStr\", \"asuransiEnd\", \"tglPasang\", \"Tanggal\", username) VALUES (" + dtbh.IdDataTruck + ", " + dtbh.Vehicle + ", " + dtbh.NoBox + ", " + dtbh.Karoseri +
                    ", " + dtbh.Tahun + ", " + dtbh.strType + ", " + dtbh.strKategori + ", " + dtbh.Lantai + ", " + dtbh.Dinding + ", " + dtbh.PintuSamping + ", " + dtbh.Sekat + ", " + dtbh.garansiStr + ", " + 
                    dtbh.garansiEnd + ", " + dtbh.asuransiStr + ", " + dtbh.asuransiEnd + ", " + dtbh.tglPasang + ", " + dtbh.Tanggal + ", " + dtbh.username + ");";
            }
            if (dtgh != null){
                query += "INSERT INTO dbo.\"DataTruckGPSHistory\" (\"IdDataTruck\", \"NoGPS\", \"Vehicle\", \"strVendor\", \"ModelGps\", \"NoDevice\", \"SensorSuhu\", \"SensorPintu\", \"Tahun\", \"TanggalPasang\", " +
                "\"TanggalGaransi\", \"Tanggal\", \"Username\") VALUES (" + dtgh.IdDataTruck + ", " + dtgh.NoGPS + ", " + dtgh.Vehicle + ", " + dtgh.strVendor + ", " + dtgh.ModelGps + ", " + dtgh.NoDevice + ", " +
                dtgh.SensorSuhu + ", " + dtgh.SensorPintu + ", " + dtgh.Tahun + ", " + dtgh.TanggalPasang + ", " + dtgh.TanggalGaransi + ", " + dtgh.Tanggal + ", " + dtgh.Username + ");";
            }
            var auditrail = new Auditrail { Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "Data Box", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id};
            context.Auditrail.Add(auditrail);
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

        public List<DataBox> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DataBox> list = context.DataBox;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataBox>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<DataBox>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DataBox>("id"); //default, wajib ada atau EF error
            }

            //take & skip
            var takeList = list;
            if (skip != null)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            List<DataBox> result = takeList.ToList().ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DataBox> items = context.DataBox;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataBox>(filters, ref items);
            }

            return items.Count();
        }
        public DataBox FindByPK(int id)
        {
            return context.DataBox.Where(d => d.Id == id).FirstOrDefault();
        }
        public DataBox FindByTruck(int id)
        {
            return context.DataBox.Where(d => d.IdDataTruck == id).FirstOrDefault();
        }
        public void delete(DataBox dbitem, int id)
        {
            context.DataBox.Remove(dbitem);
            var query = "DELETE FROM dbo.\"DataBox\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Data Box", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsBoxExist(int idtruck, int id = 0)
        {
            if(id == 0)
                return context.DataBox.Any(d => d.IdDataTruck == idtruck);
            else
                return context.DataBox.Any(d => d.IdDataTruck == idtruck && d.Id != id);
        }
        public string generateCode(int urutan)
        {
            return "BOX-" + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutan()
        {
            return context.DataBox.Count() == 0 ? 0 : context.DataBox.Max(d => d.Urutan);
        }
    }
}