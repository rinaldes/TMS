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
    public class FaktorBoronganRepo : IFaktorBoronganRepo
    {
        private ContextModel context = new ContextModel();
        public void save(FaktorBorongan dbitem, int id, FaktorBoronganHistory fbh){
            string hstq = "INSERT INTO dbo.\"FaktorBoronganHistory\" (\"IdFaktorBorongan\", \"IdMasterPool\", \"IdJenisTruck\", \"RasioDlmKota\", \"RasioDlmKota2\", \"RasioJawaBali\", \"RasioSumatra\", \"RasioKosong\", " +
                "\"RasioSolar\", \"UangMakanJawaBali\", \"UangMakanSumatra\", \"FaktorPengaliGaji\", \"FaktorPengaliTips\", \"PotonganDriver1\", \"PotonganDriver2\", \"BiayaKapalBali\", \"BiayaKapalBaliNTB\", " +
                "\"BiayaKapalSumatra\", \"BiayaKapalKalimantan\", \"BiayaKapalSulawesi\", \"Tanggal\", username) VALUES (" + fbh.IdFaktorBorongan + ", " + fbh.IdMasterPool + ", " + fbh.IdJenisTruck + ", " + fbh.RasioDlmKota +
                ", " + fbh.RasioDlmKota2 + ", " + fbh.RasioJawaBali + ", " + fbh.RasioSumatra + ", " + fbh.RasioKosong + ", " + fbh.RasioSolar + ", " + fbh.UangMakanJawaBali + ", " + fbh.UangMakanSumatra + ", " +
                fbh.FaktorPengaliGaji + ", " + fbh.FaktorPengaliTips + ", " + fbh.PotonganDriver1 + ", " + fbh.PotonganDriver2 + ", " + fbh.BiayaKapalBali + ", " + fbh.BiayaKapalBaliNTB + ", " + fbh.BiayaKapalSumatra + ", " +
                fbh.BiayaKapalKalimantan + ", " + fbh.BiayaKapalSulawesi + ", " + fbh.Tanggal + ", " + fbh.username + ");";
            if (dbitem.Id == 0) //create
            {
                context.FaktorBorongan.Add(dbitem);
                var query = "INSERT INTO dbo.\"FaktorBorongan\" (\"IdMasterPool\", \"IdJenisTruck\", \"RasioDlmKota\", \"RasioDlmKota2\", \"RasioJawaBali\", \"RasioSumatra\", \"RasioKosong\", \"UangMakanJawaBali\", " +
                    "\"UangMakanSumatra\", \"FaktorPengaliGaji\", \"FaktorPengaliTips\", \"PotonganDriver1\", \"PotonganDriver2\", \"BiayaKapalBali\", \"BiayaKapalBaliNTB\", \"BiayaKapalSumatra\", " +
                    "\"BiayaKapalKalimantan\", \"BiayaKapalSulawesi\") VALUES (" + dbitem.IdMasterPool + ", " + dbitem.IdJenisTruck + ", " + dbitem.RasioDlmKota + ", " + dbitem.RasioDlmKota2 + ", " + dbitem.RasioJawaBali +
                    ", " + dbitem.RasioSumatra + ", " + dbitem.RasioKosong + ", " + dbitem.UangMakanJawaBali + ", " + dbitem.UangMakanSumatra + ", " + dbitem.FaktorPengaliGaji + ", " + dbitem.FaktorPengaliTips + ", " + 
                    dbitem.PotonganDriver1 + ", " + dbitem.PotonganDriver2 + ", " + dbitem.BiayaKapalBali + ", " + dbitem.BiayaKapalBaliNTB + ", " + dbitem.BiayaKapalSumatra + ", " + dbitem.BiayaKapalKalimantan + ", " + 
                    dbitem.BiayaKapalSulawesi + ");";
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Faktor Borongan", QueryDetail = query + hstq, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.FaktorBorongan.Attach(dbitem);
                var query = "UPDATE dbo.\"FaktorBorongan\" SET \"IdMasterPool\" = " + dbitem.IdMasterPool + ", \"IdJenisTruck\" = " + dbitem.IdJenisTruck + ", \"RasioDlmKota\" = " + dbitem.RasioDlmKota +
                ", \"RasioDlmKota2\" = " + dbitem.RasioDlmKota2 + ", \"RasioJawaBali\" = " + dbitem.RasioJawaBali + ", \"RasioSumatra\" = " + dbitem.RasioSumatra + ", \"RasioKosong\" = " + dbitem.RasioKosong +
                ", \"UangMakanJawaBali\" = " + dbitem.UangMakanJawaBali + ", \"UangMakanSumatra\" = " + dbitem.UangMakanSumatra + ", \"FaktorPengaliGaji\" = " + dbitem.FaktorPengaliGaji + ", \"FaktorPengaliTips\" = " +
                dbitem.FaktorPengaliTips + ", \"PotonganDriver1\" = " + dbitem.PotonganDriver1 + ", \"PotonganDriver2\" = " + dbitem.PotonganDriver2 + ", \"BiayaKapalBali\" = " + dbitem.BiayaKapalBali + 
                ", \"BiayaKapalBaliNTB\" = " + dbitem.BiayaKapalBaliNTB + ", \"BiayaKapalSumatra\" = " + dbitem.BiayaKapalSumatra + ", \"BiayaKapalKalimantan\" = " + dbitem.BiayaKapalKalimantan +
                ", \"BiayaKapalSulawesi\" = " + dbitem.BiayaKapalSulawesi + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Faktor Borongan", QueryDetail = query + hstq, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
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

        public List<FaktorBorongan> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<FaktorBorongan> list = context.FaktorBorongan;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<FaktorBorongan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<FaktorBorongan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<FaktorBorongan>("id"); //default, wajib ada atau EF error
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
            List<FaktorBorongan> result = takeList.ToList().ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<FaktorBorongan> items = context.FaktorBorongan;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<FaktorBorongan>(filters, ref items);
            }

            return items.Count();
        }
        public FaktorBorongan FindByPK(int id)
        {
            return context.FaktorBorongan.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(FaktorBorongan dbitem, int id)
        {
            context.FaktorBorongan.Remove(dbitem);
            var query = "DELETE FROM dbo.\"FaktorBorongan\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Faktor Borongan", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool isExist(int idPool, int idJnsTruck, int id = 0)
        {
            if (id == 0)
                return context.FaktorBorongan.Any(d => d.IdMasterPool == idPool && d.IdJenisTruck == idJnsTruck);
            else
                return context.FaktorBorongan.Any(d => d.IdMasterPool == idPool && d.IdJenisTruck == idJnsTruck && d.Id != id);
        }
    }
}