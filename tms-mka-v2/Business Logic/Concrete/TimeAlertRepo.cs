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
    public class TimeAlertRepo : ITimeAlertRepo
    {
        private ContextModel context = new ContextModel();
        public void save(TimeAlert dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                context.TimeAlert.Add(dbitem);
                var query = "INSERT INTO dbo.\"TimeAlert\" (\"idProses\", \"keteranganBagian\", status, \"over\", \"overSatuan\", \"idUserAlert\", \"AlertPopup\", \"AlertSound\", \"AlertEmail\", " +
                    "\"rowColor\") VALUES ( " + dbitem.idProses + ", " + dbitem.keteranganBagian + ", " + dbitem.status + ", " + dbitem.over + ", " + dbitem.overSatuan + ", " + dbitem.idUserAlert + ", " +
                    dbitem.AlertPopup + ", " + dbitem.AlertSound + ", " + dbitem.AlertEmail + ", " + dbitem.rowColor + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Time Alert", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.TimeAlert.Attach(dbitem);
                var query = "UPDATE dbo.\"TimeAlert\" SET \"idProses\" = " + dbitem.idProses + ", \"keteranganBagian\" = " + dbitem.keteranganBagian + ", status = " + dbitem.status +
                    ", \"over\" = " + dbitem.over + ", \"overSatuan\" = " + dbitem.overSatuan + ", \"idUserAlert\" = " + dbitem.idUserAlert + ", \"AlertPopup\" = " + dbitem.AlertPopup +
                    ", \"AlertSound\" = " + dbitem.AlertSound + ", \"AlertEmail\" = " + dbitem.AlertEmail + ", \"rowColor\" = " + dbitem.rowColor + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Time Alert", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id 
                };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public List<TimeAlert> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<TimeAlert> list = context.TimeAlert;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<TimeAlert>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<TimeAlert>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<TimeAlert>("Id"); //default, wajib ada atau EF error
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
            List<TimeAlert> result = takeList.ToList();
            return result;
        }

        public TimeAlert FindByPK(int id)
        {
            return context.TimeAlert.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(TimeAlert dbitem, int id)
        {
            context.TimeAlert.Remove(dbitem);
            var query = "DELETE FROM dbo.\"TimeAlert\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Time Alert", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}