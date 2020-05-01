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
    public class DataGPSRepo : IDataGPSRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DataGPS dbitem, int id, DataGPSHistory dgh){
            var hstq = "INSERT INTO dbo.\"DataGPSHistory\" (\"IdDataGPS\", \"NoGPS\", \"Vehicle\", \"strVendor\", \"ModelGps\", \"NoDevice\", \"SensorSuhu\", \"SensorPintu\", \"Tahun\", \"TanggalPasang\", " +
                "\"TanggalGaransi\", \"Tanggal\", \"Username\") VALUES (" + dgh.IdDataGPS + ", " + dgh.NoGPS + ", " + dgh.Vehicle + ", " + dgh.strVendor + ", " + dgh.ModelGps + ", " + dgh.NoDevice + ", " + dgh.SensorSuhu +
                ", " + dgh.SensorPintu + ", " + dgh.Tahun + ", " + dgh.TanggalPasang + ", " + dgh.TanggalGaransi + ", " + dgh.Tanggal + ", " + dgh.Username + ");";
            if (dbitem.Id == 0) //create
            {
                context.DataGPS.Add(dbitem);
                var query = "INSERT INTO dbo.\"DataGPS\" (\"NoGPS\", \"IdDataTruck\", \"IdVendor\", \"ModelGps\", \"NoDevice\", \"SensorSuhu\", \"SensorPintu\", \"Tahun\", \"TanggalPasang\", \"TanggalGaransi\", urutan) " +
                    "VALUES (" + dbitem.NoGPS + "," + dbitem.IdDataTruck + "," + dbitem.IdVendor + "," + dbitem.ModelGps + "," + dbitem.NoDevice + "," + dbitem.SensorSuhu + "," + dbitem.SensorPintu + "," + dbitem.Tahun +
                    "," + dbitem.TanggalPasang + "," +  dbitem.TanggalGaransi + "," +  dbitem.urutan + ");";
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Data GPS", QueryDetail = query + hstq, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.DataGPS.Attach(dbitem);
                var query = "UPDATE dbo.\"DataGPS\" SET \"NoGPS\" = " + dbitem.NoGPS + ", \"IdDataTruck\" = " + dbitem.IdDataTruck + ", \"IdVendor\" = " + dbitem.IdVendor + ", \"ModelGps\" = " + dbitem.ModelGps +
                    ", \"NoDevice\" = " + dbitem.NoDevice + ", \"SensorSuhu\" = " + dbitem.SensorSuhu + ", \"SensorPintu\" = " + dbitem.SensorPintu + ", \"Tahun\" = " + dbitem.Tahun + ", \"TanggalPasang\" = " + 
                    dbitem.TanggalPasang + ", \"TanggalGaransi\" = " + dbitem.TanggalGaransi + ", urutan = " + dbitem.urutan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Data GPS", QueryDetail = query + hstq, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
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

        public List<DataGPS> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DataGPS> list = context.DataGPS;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataGPS>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<DataGPS>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DataGPS>("id"); //default, wajib ada atau EF error
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
            List<DataGPS> result = takeList.ToList().ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DataGPS> items = context.DataGPS;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataGPS>(filters, ref items);
            }

            return items.Count();
        }
        public DataGPS FindByPK(int id)
        {
            return context.DataGPS.Where(d => d.Id == id).FirstOrDefault();
        }
        public DataGPS FindByTruck(int id)
        {
            return context.DataGPS.Where(d => d.IdDataTruck == id).FirstOrDefault();
        }
        public void delete(DataGPS dbitem, int id)
        {
            context.DataGPS.Remove(dbitem);
            var query = "DELETE FROM dbo.\"DataGPS\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Data GPS", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsBoxExist(int idtruck, int id = 0)
        {
            if (id == 0)
                return context.DataGPS.Any(d => d.IdDataTruck == idtruck);
            else
                return context.DataGPS.Any(d => d.IdDataTruck == idtruck && d.Id != id);
        }
        public string generateCode(int urutan)
        {
            return "GPS-" + (urutan).ToString().PadLeft(4, '0');
        }
        public int getUrutan()
        {
            return context.DataGPS.Count() == 0 ? 0 : context.DataGPS.Max(d => d.urutan);
        }
    }
}