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
    public class JnsTolRepo : IJnsTolRepo
    {
        private ContextModel context = new ContextModel();
        public void save(JnsTols dbitem, int id, HistoryJnsTols hjt){
            var h_query = "INSERT INTO dbo.\"HistoryJnsTols\" (\"IdTol\", \"Tanggal\", \"NamaTol\", \"GolonganTol1\", \"GolonganTol2\", \"GolonganTol3\", \"GolonganTol4\", \"Keterangan\", \"IdUser\") VALUES (" + hjt.IdTol + 
                ", " + hjt.Tanggal + ", " + hjt.NamaTol + ", " + hjt.GolonganTol1 + ", " + hjt.GolonganTol2 + ", " + hjt.GolonganTol3 + ", " + hjt.GolonganTol4 + ", " + hjt.Keterangan + ", " + hjt.IdUser + ");";
            if (dbitem.Id == 0) //create
            {
                context.JnsTols.Add(dbitem);
                var query = "INSERT INTO dbo.\"JnsTols\" (\"NamaTol\", \"GolonganTol1\", \"GolonganTol2\", \"GolonganTol3\", \"GolonganTol4\", \"Keterangan\") VALUES (" + dbitem.NamaTol + ", " + dbitem.GolonganTol1 + ", " +
                    dbitem.GolonganTol2 + ", " + dbitem.GolonganTol3 + ", " + dbitem.GolonganTol4 + ", " + dbitem.Keterangan + ");";
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Jenis Tol", QueryDetail = query + h_query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.JnsTols.Attach(dbitem);
                var query = "UPDATE dbo.\"JnsTols\" SET \"NamaTol\" = " + dbitem.NamaTol + ", \"GolonganTol1\" = " + dbitem.GolonganTol1 + ", \"GolonganTol2\" = " + dbitem.GolonganTol2 + ", \"GolonganTol3\" = " +
                    dbitem.GolonganTol3 + ", \"GolonganTol4\" = " + dbitem.GolonganTol4 + ", \"Keterangan\" = " + dbitem.Keterangan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Jenis Tol", QueryDetail = query + h_query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<JnsTols> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<JnsTols> list = context.JnsTols;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<JnsTols>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<JnsTols>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<JnsTols>("Id"); //default, wajib ada atau EF error
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
            List<JnsTols> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<JnsTols> items = context.JnsTols;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<JnsTols>(filters, ref items);
            }

            return items.Count();
        }
        public JnsTols FindByPK(int id)
        {
            return context.JnsTols.Where(d => d.Id == id).FirstOrDefault();
        }
        public JnsTols FindByNamaTol(string namaTol)
        {
            return context.JnsTols.Where(d => d.NamaTol == namaTol).FirstOrDefault();
        }
        public void delete(JnsTols dbitem, int id)
        {
            context.JnsTols.Remove(dbitem);
            var query = "DELETE FROM dbo.\"JnsTols\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Jenis Tol", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();

        }
        public bool IsExist(string nama, int id = 0)
        {
            if (id == 0)
            { return context.JnsTols.Any(p => p.NamaTol.Contains(nama)); }
            else
            { return context.JnsTols.Any(p => p.NamaTol.Contains(nama) && p.Id != id); }
        }
    }
}