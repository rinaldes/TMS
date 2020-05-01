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
    public class RuteTolRepo : IRuteTolRepo
    {
        private ContextModel context = new ContextModel();
        public void save(RuteTol dbitem, int id)
        {
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.RuteTol.Add(dbitem);
            }
            else //edit
            {
                context.RuteTol.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
                query += "DELETE FROM dbo.\"TolBerangkat\" WHERE \"IdRuteTol\" = " + dbitem.Id + ";";
                query += "DELETE FROM dbo.\"TolPulang\" WHERE \"IdRuteTol\" = " + dbitem.Id + ";";
            }
            context.SaveChanges();
            foreach (Context.TolBerangkat tp in dbitem.ListTolBerangkat){
                query += "INSERT INTO dbo.\"TolBerangkat\" (\"IdRuteTol\", \"IdTol\") VALUES (" + tp.IdRuteTol + ", " + tp.IdTol + ");";
            }
            foreach (Context.TolPulang tp in dbitem.ListTolPulang){
                query += "INSERT INTO dbo.\"TolPulang\" (\"IdRuteTol\", \"IdTol\") VALUES (" + tp.IdRuteTol + ", " + tp.IdTol + ");";
            }
            var auditrail = new Auditrail {
                Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "Rute Tol", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public List<RuteTol> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<RuteTol> list = context.RuteTol;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<RuteTol>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<RuteTol>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<RuteTol>("Id"); //default, wajib ada atau EF error
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
            //var sql = takeList.ToString();
            List<RuteTol> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<RuteTol> items = context.RuteTol;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<RuteTol>(filters, ref items);
            }

            return items.Count();
        }
        public RuteTol FindByPK(int id)
        {
            return context.RuteTol.Where(d => d.Id == id).FirstOrDefault();
        }
        public RuteTol FindByNamaRuteTol(string namaRuteTol)
        {
            return context.RuteTol.Where(d => d.NamaRuteTol == namaRuteTol).FirstOrDefault();
        }

        public RuteTol FindByUniqeTol(string kodeRute, string namaRuteTol)
        {
            return context.RuteTol.Where(d => d.NamaRuteTol == namaRuteTol && d.Rute.Kode == kodeRute).FirstOrDefault();
        }

        public void delete(RuteTol dbitem)
        {
            context.RuteTol.Remove(dbitem);

            context.SaveChanges();
        }
    }
}