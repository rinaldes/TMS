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
    public class DataPendinginRepo : IDataPendinginRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DataPendingin dbitem, int id, DataPendinginHistory dph, DataTruckPendinginHistory dtph=null)
        {
            var dphq = "INSERT INTO dbo.\"DataPendinginHistory\" (\"IdPendingin\", \"NoPendingin\", \"Tanggal\", \"strDataTruk\", \"Merk\", \"Model\", \"HmLimit\", \"Tahun\", \"strJenisPendingin\", " +
                "\"NoMesin\", \"NoKompresor\", \"tglPasang, user) VALUES (" + dph.IdPendingin + ", " + dph.NoPendingin + ", " + dph.Tanggal + ", " + dph.strDataTruk + ", " + dph.Merk + ", " + dph.Model +
                ", " + dph.HmLimit + ", " + dph.Tahun + ", " + dph.strJenisPendingin + ", " + dph.NoMesin + ", " + dph.NoKompresor + ", " + dph.tglPasang + ", " + dph.user + ");";
            if (dtph != null){
                dphq += "INSERT INTO dbo.\"DataTruckPendinginHistory\" (\"IdDataTruck\", \"NoPendingin\", \"Tanggal\", \"strDataTruk\", \"Merk\", \"Model\", \"HmLimit\", \"Tahun\", \"strJenisPendingin\", \"NoMesin\", " +
                "\"NoKompresor\", \"tglPasang\", user) VALUES (" + dtph.IdDataTruck + ", " + dtph.NoPendingin + ", " + dtph.Tanggal + ", " + dtph.strDataTruk + ", " + dtph.Merk + ", " + dtph.Model + ", " + dtph.HmLimit + ", " +
                dtph.Tahun + ", " + dtph.strJenisPendingin + ", " + dtph.NoMesin + ", " + dtph.NoKompresor + ", " + dtph.tglPasang + ", " + dtph.user + ");";
            }
            if (dbitem.Id == 0) //create
            {
                context.DataPendingin.Add(dbitem);
                var query = "INSERT INTO dbo.\"DataPendingin\" (\"NoPendingin\", \"IdDataTruk\", \"Merk\", \"Model\", \"HmLimit\", \"Tahun\", \"IdJenisPendingin\", \"NoMesin\", \"NoKompresor\", \"tglPasang\", \"Urutan\")" +
                    "VALUES (" + dbitem.NoPendingin + ", " + dbitem.IdDataTruk + ", " + dbitem.Merk + ", " + dbitem.Model + ", " + dbitem.HmLimit + ", " + dbitem.Tahun + ", " + dbitem.IdJenisPendingin + ", " +
                    dbitem.NoMesin + ", " + dbitem.NoKompresor + ", " + dbitem.tglPasang + ", " + dbitem.Urutan + ");";
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Data Pendingin", QueryDetail = query + dphq, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.DataPendingin.Attach(dbitem);
                var query = "UPDATE dbo.\"DataPendingin\" SET \"NoPendingin\" = " + dbitem.NoPendingin + ", \"IdDataTruk\" = " + dbitem.IdDataTruk + ", \"Merk\" = " + dbitem.Merk + ", \"Model\" = \" = " + dbitem.Model +
                    ", \"HmLimit\" = " + dbitem.HmLimit + ", \"Tahun\" = " + dbitem.Tahun + ", \"IdJenisPendingin\" = " + dbitem.IdJenisPendingin + ", \"NoMesin\" = " + dbitem.NoMesin + ", \"NoKompresor\" = " +
                    dbitem.NoKompresor + ", \"tglPasang\" = " + dbitem.tglPasang + ", \"Urutan\" = " + dbitem.Urutan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Data Pendingin", QueryDetail = query + dphq, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<DataPendingin> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DataPendingin> list = context.DataPendingin;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataPendingin>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<DataPendingin>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DataPendingin>("id"); //default, wajib ada atau EF error
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
            //var sql = takeList.ToString();
            List<DataPendingin> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DataPendingin> items = context.DataPendingin;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataPendingin>(filters, ref items);
            }

            return items.Count();
        }
        public DataPendingin FindByPK(int id)
        {
            return context.DataPendingin.Where(d => d.Id == id).FirstOrDefault();
        }
        public DataPendingin FindByTruck(int id)
        {
            return context.DataPendingin.Where(d => d.IdDataTruk == id).FirstOrDefault();
        }
        public void delete(DataPendingin dbitem, int id)
        {
            context.DataPendingin.Remove(dbitem);
            var query = "DELETE FROM dbo.\"DataPendingin\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Data Pendingin", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsBoxExist(int idtruck, int id = 0)
        {
            if (id == 0)
                return context.DataPendingin.Any(d => d.IdDataTruk == idtruck);
            else
                return context.DataPendingin.Any(d => d.IdDataTruk == idtruck && d.Id != id);
        }
        public string generateCode(int urutan)
        {
            return "AC-" + (urutan + 1).ToString().PadLeft(4, '0');
        }
        public int getUrutan()
        {
            return context.DataPendingin.Count() == 0 ? 0 : context.DataPendingin.Max(d => d.Urutan);
        }
    }
}