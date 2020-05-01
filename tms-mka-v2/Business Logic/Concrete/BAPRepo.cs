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
    public class BAPRepo : IBAPRepo
    {
        private ContextModel context = new ContextModel();
        public void save(BAP dbitem, int id){
            string query = "";
            if (dbitem.Id == 0) //create
            {
                context.BAP.Add(dbitem);
                query = "INSERT INTO dbo.\"BAP\" (\"NoBAP\", \"TanggalKejadian\", \"JamKejadian\", \"KategoriId\", \"LaporanKejadian\", \"DilaporkanOleh\", \"Departemen1Id\", \"HasilPemeriksaan\"," +
                    "\"Penyelesaian\", \"DiperiksaOleh\", \"Departemen2Id\", \"SalesOrderId\", \"Driver1Id\", \"Driver2Id\", \"IdDataTruck\", \"Status\", \"File\", \"SalesOrderKontrakId\") VALUES (" +
                    dbitem.NoBAP + ", " + dbitem.TanggalKejadian + ", " + dbitem.JamKejadian + ", " + dbitem.KategoriId + ", " + dbitem.LaporanKejadian + ", " + dbitem.DilaporkanOleh + ", " +
                    dbitem.Departemen1Id + ", " + dbitem.HasilPemeriksaan + ", " + dbitem.Penyelesaian + ", " + dbitem.DiperiksaOleh + ", " + dbitem.Departemen2Id + ", " + dbitem.SalesOrderId + ", " +
                    dbitem.Driver1Id + ", " + dbitem.Driver2Id + ", " + dbitem.IdDataTruck + ", " + dbitem.Status + ", " + dbitem.File + ", " + dbitem.SalesOrderKontrakId + ");";
            }
            else //edit
            {
                context.BAP.Attach(dbitem);
                query = "UPDATE dbo.\"BAP\" SET \"NoBAP\" = " + dbitem.NoBAP + ",\"TanggalKejadian\" = " + dbitem.TanggalKejadian + ",\"JamKejadian\" = " + dbitem.JamKejadian + ",\"KategoriId\" = " +
                    dbitem.KategoriId + ",\"LaporanKejadian\" = " + dbitem.LaporanKejadian + ",\"DilaporkanOleh\" = " + dbitem.DilaporkanOleh + ",\"Departemen1Id\" = " + dbitem.Departemen1Id +
                    ",\"HasilPemeriksaan\" = " + dbitem.HasilPemeriksaan + ",\"Penyelesaian\" = " + dbitem.Penyelesaian + ",\"DiperiksaOleh\"; = " + dbitem.DiperiksaOleh + ",\"Departemen2Id\" = " +
                    dbitem.Departemen2Id + ",\"SalesOrderId\" = " + dbitem.SalesOrderId + ",\"Driver1Id\" = " + dbitem.Driver1Id + ",\"Driver2Id\" = " + dbitem.Driver2Id + ",\"IdDataTruck\" = " + 
                    dbitem.IdDataTruck + ",\"Status\" = " + dbitem.Status + ",\"File\" = " + dbitem.File + ",\"SalesOrderKontrakId\" = " + dbitem.SalesOrderKontrakId + "WHERE \"Id\" = " + dbitem.Id + ";";

                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            var auditrail = new Auditrail {
                Actionnya = "Update", EventDate = DateTime.Now, Modulenya = "BAP", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public List<BAP> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<BAP> list = context.BAP;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<BAP>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<BAP>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<BAP>("\"Id\" DESC"); //default, wajib ada atau EF error
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
            List<BAP> result = takeList.ToList();
            return result;
        }
        public List<BAP> FindAllAll()
        {
            IQueryable<BAP> list = context.BAP;
            var takeList = list;
            List<BAP> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<BAP> items = context.BAP;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<BAP>(filters, ref items);
            }

            return items.Count();
        }
        public BAP FindByPK(int id)
        {
            return context.BAP.Where(d => d.Id == id).FirstOrDefault();
        }
        public BAP FindByNoBAP(string noBAP)
        {
            return context.BAP.Where(d => d.NoBAP == noBAP).FirstOrDefault();
        }
        public void delete(BAP dbitem, int id)
        {
            context.BAP.Remove(dbitem);
            var query = "DELETE FROM dbo.\"BAP\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "BAP", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.SaveChanges();

        }
        public bool IsExist(int? IdSo, int? SalesOrderKontrakId, int? idDriver, int? idTruck, int? IdKategori, int id = 0)
        {
            return context.BAP.Any(d => (d.SalesOrderId == IdSo && d.SalesOrderKontrakId == SalesOrderKontrakId) && d.Driver1Id == idDriver && d.IdDataTruck == idTruck && d.KategoriId == IdKategori && d.Id != id);
        }

        public string GenerateCode(DateTime valdate, int urutan)
        {
            return "BAP-" + valdate.ToString("yyMMdd") + '-' + (urutan).ToString().PadLeft(2, '0');
        }

        public int getUrutanOnCAll(DateTime valdate)
        {
            try
            {
                return int.Parse(context.BAP.Where(d => d.TanggalKejadian == valdate).Select(d => d.NoBAP).Max().Split('-').Last());
            }
            catch (Exception) { }
            return 0;
        }
    }
}