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
    public class TrainingSettingRepo : ITrainingSettingRepo
    {
        private ContextModel context = new ContextModel();
        private System.Web.HttpContext context2 = System.Web.HttpContext.Current; 
        public void save(TrainingSetting dbitem, int id)
        {
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.TrainingSetting.Add(dbitem);
                query += "INSERT INTO dbo.\"TrainingSetting\" (\"Nama\", \"Interval\") VALUES (" + dbitem.Nama + ", " + dbitem.Interval + ");";
            }
            else //edit
            {
                context.TrainingSetting.Attach(dbitem);
                query += "UPDATE dbo.\"TrainingSetting\" SET \"Nama\" = " + dbitem.Nama + ", \"Interval\" = " + dbitem.Interval + " WHERE \"Id\" = " + dbitem.Id + ";";
                query += "DELETE FROM dbo.\"TrainingSettingDetail\" WHERE \"TrainingSettingId\" = " + dbitem.Id + ";";
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
            foreach (Context.TrainingSettingDetail tsd in dbitem.TrainingSettingDetail){
                query += "INSERT INTO dbo.\"TrainingSettingDetail\" (\"TrainingSettingId\", \"Materi\", \"NilaiMinimum\") VALUES (" + tsd.TrainingSettingId + ", " + tsd.Materi + ", "+tsd.NilaiMinimum+");";
            }
            var auditrail = new Auditrail {
                Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "Training", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public List<TrainingSetting> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<TrainingSetting> list = context.TrainingSetting;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<TrainingSetting>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<TrainingSetting>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<TrainingSetting>("Id"); //default, wajib ada atau EF error
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
            List<TrainingSetting> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<TrainingSetting> items = context.TrainingSetting;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<TrainingSetting>(filters, ref items);
            }

            return items.Count();
        }
        public TrainingSetting FindByPK(int id)
        {
            return context.TrainingSetting.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(TrainingSetting dbitem, int id)
        {
            context.TrainingSetting.Remove(dbitem);
            var query = "DELETE FROM dbo.\"TrainingSetting\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Training Setting", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}