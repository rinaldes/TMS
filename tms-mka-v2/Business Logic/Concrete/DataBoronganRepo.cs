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
    public class DataBoronganRepo : IDataBoronganRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DataBorongan dbitem, int id){
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.DataBorongan.Add(dbitem);
                query += "INSERT INTO dbo.\"DataBorongan\" (\"IsTambahan\", \"IdMasterPool\", \"IdJenisTruck\", \"NamaBorongan\", \"Jarak\", \"Rasio\", \"LiterSolar\", \"HargaSolar\", " +
                    "\"WaktuHariKerja\", \"JumlahMakan\", \"AreaUangMakan\", \"UangMakan\", \"BiayaTol\", \"BobotTipsParkir\", \"TipsParkir\", \"BobotGaji1\", gaji1, \"BobotGaji2\", gaji2, " +
                    "\"TotalGaji\", \"Kapal\", \"BiayaKapal\", \"BoronganDasar\", \"Kawalan\", \"Timbangan\", \"Karantina\", \"SPSI\", \"MultiDrop\", \"TotalBorongan\", \"Pembulatan\", \"CustomerId\", " +
                    "\"AlokasiCash\", \"TotalAlokasiPembulatan\") VALUES (" + dbitem.IsTambahan + ", " + dbitem.IdMasterPool + ", " + dbitem.IdJenisTruck + ", " + dbitem.NamaBorongan + ", " +
                    dbitem.Jarak + ", " + dbitem.Rasio + ", " + dbitem.LiterSolar + ", " + dbitem.HargaSolar + ", " + dbitem.WaktuHariKerja + ", " + dbitem.JumlahMakan + ", " +
                    dbitem.AreaUangMakan + ", " + dbitem.UangMakan + ", " + dbitem.BiayaTol + ", " + dbitem.BobotTipsParkir + ", " + dbitem.TipsParkir + ", " + dbitem.BobotGaji1 + ", " + dbitem.gaji1 +
                    ", " + dbitem.BobotGaji2 + ", " + dbitem.gaji2 + ", " + dbitem.TotalGaji + ", " + dbitem.Kapal + ", " + dbitem.BiayaKapal + ", " + dbitem.BoronganDasar + ", " + dbitem.Kawalan + ", " +
                    dbitem.Timbangan + ", " + dbitem.Karantina + ", " + dbitem.SPSI + ", " + dbitem.MultiDrop + ", " + dbitem.TotalBorongan + ", " + dbitem.Pembulatan + ", " + dbitem.CustomerId + ", " +
                    dbitem.AlokasiCash + ", " + dbitem.TotalAlokasiPembulatan + ");";
            }
            else //edit
            {
                context.DataBorongan.Attach(dbitem);
                query += "UPDATE dbo.\"DataBorongan\" SET \"IsTambahan\" = " + dbitem.IsTambahan + ", \"IdMasterPool\" = " + dbitem.IdMasterPool + ", \"IdJenisTruck\" = " + dbitem.IdJenisTruck +
                ", \"NamaBorongan\" = " + dbitem.NamaBorongan + ", \"Jarak\" = " + dbitem.Jarak + ", \"Rasio\" = " + dbitem.Rasio + ", \"LiterSolar\" = " + dbitem.LiterSolar + ", \"HargaSolar\" = " +
                dbitem.HargaSolar + ", \"WaktuHariKerja\" = " + dbitem.WaktuHariKerja + ", \"JumlahMakan\" = " + dbitem.JumlahMakan + ", \"AreaUangMakan\" = " + dbitem.AreaUangMakan +
                ", \"UangMakan\" = " + dbitem.UangMakan + ", \"BiayaTol\" = " + dbitem.BiayaTol + ", \"BobotTipsParkir\" = " + dbitem.BobotTipsParkir + ", \"TipsParkir\" = " + dbitem.TipsParkir +
                ", \"BobotGaji1\" = " + dbitem.BobotGaji1 + ", gaji1 = " + dbitem.gaji1 + ", \"BobotGaji2\" = " + dbitem.BobotGaji2 + ", gaji2 = " + dbitem.gaji2 + ", \"TotalGaji\" = " + dbitem.TotalGaji +
                ", \"Kapal\" = " + dbitem.Kapal + ", \"BiayaKapal\" = " + dbitem.BiayaKapal + ", \"BoronganDasar\" = " + dbitem.BoronganDasar + ", \"Kawalan\" = " + dbitem.Kawalan + ", \"Timbangan\" = " +
                dbitem.Timbangan + ", \"Karantina\" = " + dbitem.Karantina + ", \"SPSI\" = " + dbitem.SPSI + ", \"MultiDrop\" = " + dbitem.MultiDrop + ", \"TotalBorongan\" = " + dbitem.TotalBorongan +
                ", \"Pembulatan\" = " + dbitem.Pembulatan + ", \"CustomerId\" = " + dbitem.CustomerId + ", \"AlokasiCash\" = " + dbitem.AlokasiCash + ", \"TotalAlokasiPembulatan\" = " +
                dbitem.TotalAlokasiPembulatan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            query += "INSERT INTO \"dbo\".\"DataBoronganHistory\" (\"IdDataBorongan\", \"Tanggal\", \"IsTambahan\", \"NamaBorongan\", \"Customer\", \"Jarak\", \"Rasio\", \"LiterSolar\", \"HargaSolar\", " +
                     "\"WaktuHariKerja\", \"JumlahMakan\", \"AreaUangMakan\", \"UangMakan\", \"BiayaTol\", \"BobotTipsParkir\", \"TipsParkir\", \"BobotGaji1\", gaji1, \"BobotGaji2\", gaji2, " +
                     "\"TotalGaji\", \"Kapal\", \"BiayaKapal\", \"BoronganDasar\", \"Kawalan\", \"Timbangan\", \"Karantina\", \"SPSI\", \"MultiDrop\", \"TotalBorongan\", \"Pembulatan\") VALUES ( " +
                    dbitem.Id + ", " + DateTime.Now + ", " + dbitem.IsTambahan + ", " + dbitem.NamaBorongan + ", " + dbitem.Customer + ", " + dbitem.Jarak + ", " + dbitem.Rasio + ", " + dbitem.LiterSolar +
                    ", " + dbitem.HargaSolar + ", " + dbitem.WaktuHariKerja + ", " + dbitem.JumlahMakan + ", " + dbitem.AreaUangMakan + ", " + dbitem.UangMakan + ", " + dbitem.BiayaTol + ", " +
                    dbitem.BobotTipsParkir + ", " + dbitem.TipsParkir + ", " + dbitem.BobotGaji1 + ", " + dbitem.gaji1 + ", " + dbitem.BobotGaji2 + ", " + dbitem.gaji2 + ", " + dbitem.TotalGaji + ", " +
                    dbitem.Kapal + ", " + dbitem.BiayaKapal + ", " + dbitem.BoronganDasar + ", " + dbitem.Kawalan + ", " + dbitem.Timbangan + ", " + dbitem.Karantina + ", " + dbitem.SPSI + ", " +
                    dbitem.MultiDrop + ", " + dbitem.TotalBorongan + ", " + dbitem.Pembulatan + ");";
            query += "DELETE FROM \"dbo\".\"DataBoronganKapal\" WHERE \"IdBorongan\"=" + dbitem.Id + ";";
            foreach (Context.DataBoronganKapal dbk in dbitem.DataBoronganKapal){
                query += "INSERT INTO \"dbo\".\"DataBoronganKapal\" (\"IdBorongan\", \"IdLookupCodeKapal\", value) VALUES (" + dbk.IdBorongan + ", " + dbk.IdLookupCodeKapal + ", " + dbk.value + ");";
            }
            query += "DELETE FROM \"dbo\".\"DataBoronganRute\" WHERE \"IdBorongan\"=" + dbitem.Id + ";";
            foreach (Context.DataBoronganRute dbr in dbitem.DataBoronganRute){
                query += "INSERT INTO \"dbo\".\"DataBoronganRute\" (\"IdBorongan\", \"IdRute\", remarks) VALUES (" + dbr.IdBorongan + ", " + dbr.IdRute + ", " + dbr.remarks + ");";
            }
            query += "DELETE FROM \"dbo\".\"DataBoronganSPBU\" WHERE \"IdBorongan\"=" + dbitem.Id + ";";
            foreach (Context.DataBoronganSPBU dbr in dbitem.DataBoronganSPBU){
                query += "INSERT INTO \"dbo\".\"DataBoronganTf\" (\"IdBorongan\", value) VALUES (" + dbr.IdBorongan + ", " + dbr.IdLookupCodeSpbu + ", " + dbr.value + ");";
            }
            query += "DELETE FROM \"dbo\".\"DataBoronganTf\" WHERE \"IdBorongan\"=" + dbitem.Id + ";";
            foreach (Context.DataBoronganTf dbr in dbitem.DataBoronganTf){
                query += "INSERT INTO \"dbo\".\"DataBoronganSPBU\" (\"IdBorongan\", value, \"LeadTime\") VALUES (" + dbr.IdBorongan + ", " + dbr.value + "," + dbr.LeadTime + ")";
            }

            var auditrail = new Auditrail {
                Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "Data Borongan", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public List<DataBorongan> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DataBorongan> list = context.DataBorongan;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataBorongan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<DataBorongan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DataBorongan>("id"); //default, wajib ada atau EF error
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
            List<DataBorongan> result = takeList.ToList().ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DataBorongan> items = context.DataBorongan;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DataBorongan>(filters, ref items);
            }

            return items.Count();
        }
        public DataBorongan FindByPK(int id)
        {
            return context.DataBorongan.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(DataBorongan dbitem, int id)
        {
            context.DataBorongan.Remove(dbitem);
            var query = "DELETE FROM dbo.\"DataBorongan\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Data Borongan", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(int IdPool, int IdTruck, string nama, int id = 0, int CustomerId = 0)
        {
            if(id == 0)
                return context.DataBorongan.Any(d => d.IdMasterPool == IdPool && d.IdJenisTruck == IdTruck && d.NamaBorongan == nama && d.CustomerId == CustomerId);
            else
                return context.DataBorongan.Any(d => d.IdMasterPool == IdPool && d.IdJenisTruck == IdTruck && d.NamaBorongan == nama && d.Id != id && d.CustomerId == CustomerId);
        }
    }
}