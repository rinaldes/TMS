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
    public class TiketRepo : ITiketRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Tiket dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                context.Tiket.Add(dbitem);
                var query = "INSERT INTO dbo.\"Tiket\" (\"NoTiket\", \"IdCustomer\", \"TanggalLapor\", \"DitujukanKe\", \"Kategori\", \"Prioritas\", \"Status\", \"Subject\", \"Keluhan\", \"IdCS\"," +
                    " \"Urutan\", \"Respon\", \"NamaPelapor\", \"Attactment\", \"IdSo\", \"IdSoKontrak\") VALUES ( " + dbitem.NoTiket + ", " + dbitem.IdCustomer + ", " + dbitem.TanggalLapor + ", " + 
                    dbitem.DitujukanKe + ", " + dbitem.Kategori + ", " + dbitem.Prioritas + ", " + dbitem.Status + ", " + dbitem.Subject + ", " + dbitem.Keluhan + ", " + dbitem.IdCS + ", " + 
                    dbitem.Urutan + ", " + dbitem.Respon + ", " + dbitem.NamaPelapor + ", " + dbitem.Attactment + ", " + dbitem.IdSo + ", " + dbitem.IdSoKontrak + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Tiket", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                dbitem.LastUpdate = DateTime.Now;
                context.Tiket.Attach(dbitem);
                var query = "UPDATE dbo.\"Tiket\" SET \"IdCustomer\" = " + dbitem.IdCustomer + ", \"TanggalLapor\" = " + dbitem.TanggalLapor + ", \"DitujukanKe\" = " + dbitem.DitujukanKe +
                    ", \"Kategori\" = " + dbitem.Kategori + ", \"Prioritas\" = " + dbitem.Prioritas + ", \"Status\" = " + dbitem.Status + ", \"Subject\" = " + dbitem.Subject + ", \"Keluhan\" = " +
                    dbitem.Keluhan + ", \"IdCS\" = " + dbitem.IdCS + ", \"Urutan\" = " + dbitem.Urutan + ", \"Respon\" = " + dbitem.Respon + ", \"NamaPelapor\" = " + dbitem.NamaPelapor +
                    ", \"Attactment\" = " + dbitem.Attactment + ", \"IdSo\" = " + dbitem.IdSo + ", \"IdSoKontrak\" = " + dbitem.IdSoKontrak + " WHERE \"Id\" = " + dbitem.Id + ";";
                if (dbitem.Respon != null){
                    context.TiketResponse.Add(new TiketResponse {Respon = dbitem.Respon, IdResponder = id, IdTiket = dbitem.Id, ResponseAttachment = dbitem.ResponseAttachment, CreatedAt = DateTime.Now});
                    query += "INSERT INTO dbo.\"TiketResponse\" (\"IdTiket\", \"Respon\", \"IdResponder\", \"ResponseAttachment\") VALUES (" + dbitem.Id + ", " + dbitem.Respon + ", " + id + ", " +
                        dbitem.ResponseAttachment + ");";
                }
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Tiket List", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<Tiket> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Tiket> list = context.Tiket;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Tiket>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Tiket>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Tiket>("Id"); //default, wajib ada atau EF error
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
            List<Tiket> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Tiket> items = context.Tiket;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Tiket>(filters, ref items);
            }

            return items.Count();
        }
        public Tiket FindByPK(int id)
        {
            return context.Tiket.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(Tiket dbitem, int id)
        {
            context.Tiket.Remove(dbitem);
            var query = "DELETE FROM dbo.\"Tiket\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Tiket List", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public int getUrutan()
        {
            List<Tiket> dboncall = context.Tiket.ToList();
            return dboncall.Count() == 0 ? 0 : dboncall.Max(d => d.Urutan);
        }
        public string generateCodePPK(int urutan)
        {
            string sMonth = DateTime.Now.ToString("MM");
            return "CS-" + sMonth + DateTime.Now.Day.ToString("00") + "-" + (urutan).ToString().PadLeft(4, '0');
        } 
    }
}