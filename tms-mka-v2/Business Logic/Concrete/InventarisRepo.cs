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
    public class InventarisRepo : IInventarisRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Inventaris dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                context.Inventaris.Add(dbitem);
                var query = "INSERT INTO dbo.\"Inventaris\" (\"TanggalPemberian\", \"TanggalPengembalian\", \"IdNamaBarang\", \"Keterangan\", \"DriverId\") VALUES (" + dbitem.TanggalPemberian + ", " + 
                    dbitem.TanggalPengembalian + ", " + dbitem.IdNamaBarang + ", " + dbitem.Keterangan + ", " + dbitem.DriverId + ");";
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Inventaris", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.Inventaris.Attach(dbitem);
                var query = "UPDATE dbo.\"Inventaris\" SET \"TanggalPemberian\" = " + dbitem.TanggalPemberian + ", \"TanggalPengembalian\" = " + dbitem.TanggalPengembalian + ", \"IdNamaBarang\" = " + dbitem.IdNamaBarang + 
                ", \"Keterangan\" = " + dbitem.Keterangan + ", \"DriverId\" = " + dbitem.DriverId + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Inventaris", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public List<Inventaris> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Inventaris> list = context.Inventaris;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Inventaris>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Inventaris>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Inventaris>("Id"); //default, wajib ada atau EF error
            }

            //take & skip
            var takeList = list;
            if (skip != null && skip != 0)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null && skip != 0)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            //var sql = takeList.ToString();
            List<Inventaris> result = takeList.ToList();
            return result;
        }

        public Inventaris FindByPK(int id)
        {
            return context.Inventaris.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(Inventaris dbitem, int id)
        {
            context.Inventaris.Remove(dbitem);
            var query = "DELETE FROM dbo.\"Inventaris\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Inventaris", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}