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
    public class SpkRepo : ISpkRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Spk spk_dbitem, int id)
        {
            if (spk_dbitem.Id == 0) //create
            {
                spk_dbitem.Status = "Open";
                context.Spk.Add(spk_dbitem);
                var query = "INSERT INTO dbo.\"Spk\" (\"Jenis\", \"Permintaan\", \"Keterangan\", \"Mekanik1\", \"Mekanik2\", \"Workshop_id\", \"Status\", \"ServiceIn\", \"Estimasi\", \"ServiceOut\", " +
                    "\"RevEstimasi\", \"KeteranganSPK\") VALUES ( " + spk_dbitem.Jenis + ", " + spk_dbitem.Permintaan + ", " + spk_dbitem.Keterangan + ", " + spk_dbitem.Mekanik1 + ", " + 
                    spk_dbitem.Mekanik2 + ", " + spk_dbitem.Workshop_id + ", " + spk_dbitem.Status + ", " + spk_dbitem.ServiceIn + ", " + spk_dbitem.Estimasi + ", " + spk_dbitem.ServiceOut + ", " +
                    spk_dbitem.RevEstimasi + ", " + spk_dbitem.KeteranganSPK + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Spk", EventDate = DateTime.Now, Modulenya = "Workshop", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.Spk.Attach(spk_dbitem);
                var spk_history = new SpkHistory {
                    Tanggal = DateTime.Now, Jenis = spk_dbitem.Jenis, Estimasi = spk_dbitem.Estimasi ?? DateTime.Now,
                    JamEstimasi = spk_dbitem.JamEstimasi, Status = spk_dbitem.Status, RevEstimasi = spk_dbitem.RevEstimasi ?? 0,
                    WorkshopId = spk_dbitem.Workshop_id
                };
                context.SpkHistory.Add(spk_history);
                var query = "UPDATE dbo.\"Spk\" SET \"Jenis\" = " + spk_dbitem.Jenis + ", \"Permintaan\" = " + spk_dbitem.Permintaan + ", \"Keterangan\" = " + spk_dbitem.Keterangan +
                    ", \"Mekanik1\" \" = " + spk_dbitem.Mekanik1 + ", \"Mekanik2\" = \" = " + spk_dbitem.Mekanik2 + ", \"Workshop_id\" = \" = " + spk_dbitem.Workshop_id + ", \"Status\" = \" = " +
                    spk_dbitem.Status + ", \"ServiceIn\" = \" = " + spk_dbitem.ServiceIn + ", \"Estimasi\" = \" = " + spk_dbitem.Estimasi + ", \"ServiceOut\" = \" = " + spk_dbitem.ServiceOut +
                    ", \"RevEstimasi\" = \" = " + spk_dbitem.RevEstimasi + ", \"KeteranganSPK\" = \" = " + spk_dbitem.KeteranganSPK + " WHERE \"Id\" = " + spk_dbitem.Id + ";" +
                    "INSERT INTO dbo.\"SpkHistory\" (\"Jenis\", \"Status\", \"Estimasi\", \"Tanggal\", \"RevEstimasi\", \"WorkshopId\") VALUES (" + spk_history.Jenis + ", " + spk_history.Status + ", " +
                    spk_history.Estimasi + ", " + spk_history.Tanggal + ", " + spk_history.RevEstimasi + ", " + spk_history.WorkshopId + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Spk", EventDate = DateTime.Now, Modulenya = "Workshop", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(spk_dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<Spk> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Spk> list = context.Spk;
            
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Spk>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Spk>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Spk>("Id"); //default, wajib ada atau EF error
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
            List<Spk> result = takeList.ToList();
            return result;
        }
        public List<Spk> FindAllOnProgress(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Spk> list = context.Spk.Where(d => d.Status == "On-Progress" && d.Mekanik1 != null);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Spk>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Spk>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Spk>("Id"); //default, wajib ada atau EF error
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
            List<Spk> result = takeList.ToList();
            return result;
        }
        public List<Spk> FindByWorkshop(int id)
        {
            IQueryable<Spk> list = context.Spk.Where(d => d.Workshop_id == id);
            list = list.OrderBy<Spk>("Id"); //default, wajib ada atau EF error
            //take & skip
            var takeList = list;
            //return result
            List<Spk> result = takeList.ToList();
            return result;
        }
        public Spk FindByWorkshopAndType(int id, string type)
        {
            IQueryable<Spk> list = context.Spk.Where(d => d.Workshop_id == id && d.Jenis == type);
            list = list.OrderBy<Spk>("Id"); //default, wajib ada atau EF error
            //take & skip
            var takeList = list;
            //return result
            List<Spk> result = takeList.ToList();
            return result.FirstOrDefault();
        }
        public List<SpkHistory> FindByWorkshopForHistory(int id)
        {
            IQueryable<SpkHistory> list = context.SpkHistory.Where(d => d.WorkshopId == id);
            list = list.OrderBy<SpkHistory>("Id"); //default, wajib ada atau EF error
            //take & skip
            var takeList = list;
            //return result
            List<SpkHistory> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Spk> items = context.Spk;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Spk>(filters, ref items);
            }

            return items.Count();
        }
        public int CountOnProgress(FilterInfo filters = null)
        {
            IQueryable<Spk> items = context.Spk.Where(d => d.Status == "On-Progress" && d.Mekanik1 != null);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Spk>(filters, ref items);
            }

            return items.Count();
        }
        public Spk FindByPK(int id)
        {
            return context.Spk.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(Spk dbitem)
        {
            context.Spk.Remove(dbitem);
            context.SaveChanges();
        }
        public string generateCode(DateTime tglGabung, int urutan)
        {
            return "SPK-" + tglGabung.ToString("yyMMdd") + "-" + (urutan).ToString().PadLeft(3, '0');
        }
        public int? getUrutan(DateTime tglGbg)
        {
            DateTime tglGbgA = tglGbg.AddDays(1);
            DateTime tglGbgB = tglGbg.AddDays(-1);
            return context.Spk.Where(d => d.TglSPK.Value < tglGbgA && d.TglSPK.Value > tglGbgB).Count() == 0 ? 1 : context.Spk.Where(d => d.TglSPK.Value < tglGbgA && d.TglSPK.Value > tglGbgB).Max(d => d.Urutan) + 1;
        }
    }
}