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
    public class WorkshopRepo : IWorkshopRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Workshop dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                context.Workshop.Add(dbitem);
                var query = "INSERT INTO dbo.\"Workshop\" (\"NoPPK\", \"TglPre\", \"TglPPK\", \"IdVehicle\", \"Urutan\", \"PrioritasFrom\", \"PrioritasTo\", \"KetPrioritas\", \"IsPool\", \"IsTruck\", " +
                    "\"IsAc\",  \"IsGps\", \"IsBan\", \"IsBox\", \"KetTruck\", \"KetAc\", \"KetGps\", \"KetBan\", \"KetBox\", \"KetKerjaTruck\", \"KetKerjaAc\", \"KetKerjaGps\", \"KetKerjaBan\", " +
                    "\"KetKerjaBox\", \"Status\", \"KmActual\", \"HmActual\", \"IdPrioritas\") VALUES (" + dbitem.NoPPK + ", " + dbitem.TglPre + ", " + dbitem.TglPPK + ", " + dbitem.IdVehicle + ", " +
                    dbitem.Urutan + ", " + dbitem.PrioritasFrom + ", " + dbitem.PrioritasTo + ", " + dbitem.KetPrioritas + ", " + dbitem.IsPool + ", " + dbitem.IsTruck + ", " + dbitem.IsAc + ", " +
                    dbitem.IsGps + ", " + dbitem.IsBan + ", " + dbitem.IsBox + ", " + dbitem.KetTruck + ", " + dbitem.KetAc + ", " + dbitem.KetGps + ", " + dbitem.KetBan + ", " + dbitem.KetBox + ", " +
                    dbitem.KetKerjaTruck + ", " + dbitem.KetKerjaAc + ", " + dbitem.KetKerjaGps + ", " + dbitem.KetKerjaBan + ", " + dbitem.KetKerjaBox + ", " + dbitem.Status + ", " + dbitem.KmActual +
                    ", " + dbitem.HmActual + ", " + dbitem.IdPrioritas + ");";
                var auditrail = new Auditrail
                {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Workshop", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                if (dbitem.Status.Contains("SPK"))
                {
                    //Status = dbitem.Status == "SPK" ? "SPK-O" : dbitem.Status;
                    if (dbitem.Spk.Count() == dbitem.Spk.Where(d => d.Status == "Closed").Count())
                    {
                        dbitem.Status = "SPK-C";
                    }
                    else if (dbitem.Spk.Where(d => (d.Status == null || d.Status == "Open")).Count() == dbitem.Spk.Count())
                    {
                        dbitem.Status = "SPK-O";
                    }
                    else
                    {
                        dbitem.Status = "SPK-W";
                    }
                }
                context.Workshop.Attach(dbitem);
                var query = "UPDATE dbo.\"Workshop\" SET \"NoPPK\" = " + dbitem.NoPPK + ", \"TglPre\" = " + dbitem.TglPre + ", \"TglPPK\" = " + dbitem.TglPPK + ", \"IdVehicle\" = " + dbitem.IdVehicle +
                    ", \"Urutan\" = " + dbitem.Urutan + ", \"PrioritasFrom\" = " + dbitem.PrioritasFrom + ", \"PrioritasTo\" = " + dbitem.PrioritasTo + ", \"KetPrioritas\" = " + dbitem.KetPrioritas +
                    ", \"IsPool\" = " + dbitem.IsPool + ", \"IsTruck\" = " + dbitem.IsTruck + ", \"IsAc\" = " + dbitem.IsAc + ", \"IsGps\" = " + dbitem.IsGps + ", \"IsBan\" = " + dbitem.IsBan +
                    ", \"IsBox\" = " + dbitem.IsBox + ", \"KetTruck\" = " + dbitem.KetTruck + ", \"KetAc\" = " + dbitem.KetAc + ", \"KetGps\" = " + dbitem.KetGps + ", \"KetBan\" = " + dbitem.KetBan +
                    ", \"KetBox\" = " + dbitem.KetBox + ", \"KetKerjaTruck\" = " + dbitem.KetKerjaTruck + ", \"KetKerjaAc\" = " + dbitem.KetKerjaAc + ", \"KetKerjaGps\" = " + dbitem.KetKerjaGps +
                    ", \"KetKerjaBan\" = " + dbitem.KetKerjaBan + ", \"KetKerjaBox\" = " + dbitem.KetKerjaBox + ", \"Status\" = " + dbitem.Status + ", \"KmActual\" = " + dbitem.KmActual +
                    ", \"HmActual\" = " + dbitem.HmActual + ", \"IdPrioritas\" = " + dbitem.IdPrioritas + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail
                {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Workshop", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<Workshop> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Workshop> list = context.Workshop;
            
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Workshop>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Workshop>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<Workshop>("Id"); //default, wajib ada atau EF error
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
            List<Workshop> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Workshop> items = context.Workshop;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Workshop>(filters, ref items);
            }

            return items.Count();
        }
        public Workshop FindByPK(int id)
        {
            return context.Workshop.Where(d => d.Id == id).FirstOrDefault();
        }
        public Workshop FindByNoPPK(string noPPK)
        {
            return context.Workshop.Where(d => d.NoPPK == noPPK).FirstOrDefault();
        }
        public void delete(Workshop dbitem, int id)
        {
            context.Workshop.Remove(dbitem);
            var query = "DELETE FROM dbo.\"Workshop\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Workshop", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public int getUrutan()
        {
            List<Workshop> dboncall = context.Workshop.Where(d => d.NoPrePPK != null).ToList();
            return dboncall.Count() == 0 ? 1 : dboncall.Max(d => d.Urutan)+1;
        }
        public int getUrutanPPK(DateTime tglGabung)
        {
            List<Workshop> dboncall = context.Workshop.Where(d => d.NoPPK != null && d.TglPre == tglGabung || d.TglPPK == tglGabung).ToList();
            return dboncall.Count() == 0 ? 1 : dboncall.Max(d => d.UrutanPPK)+1;
        }
        public string generateCodePPK(DateTime tglGabung, int urutan)
        {
            return "PPK-" + tglGabung.ToString("yyMMdd") + "-" + (urutan).ToString().PadLeft(3, '0');
        }

        public string generateCodePrePPK(int urutan)
        {
            return "PRE" + (urutan).ToString().PadLeft(4, '0');
        }

        public int CountTruck(string status){
            return context.Workshop.Where(d => d.Status == status && d.IsTruck == true).Count();
        }
        public int CountPendingin(string status){
            return context.Workshop.Where(d => d.Status == status && d.IsAc == true).Count();
        }
        public int CountBox(string status){
            return context.Workshop.Where(d => d.Status == status && d.IsBox == true).Count();
        }
        public int CountBan(string status){
            return context.Workshop.Where(d => d.Status == status && d.IsBan == true).Count();
        }
        public int CountGPS(string status){
            return context.Workshop.Where(d => d.Status == status && d.IsGps == true).Count();
        }
        public int CountTruckSPKO(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsTruck == true && d.Spk.Count == d.Spk.Where(e => e.Status == null).Count()).Count();
        }
        public int CountPendinginSPKO(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsAc == true && d.Spk.Count == d.Spk.Where(e => e.Status == null).Count()).Count();
        }
        public int CountBanSPKO(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsBan == true && d.Spk.Count == d.Spk.Where(e => e.Status == null).Count()).Count();
        }
        public int CountBoxSPKO(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsBox == true && d.Spk.Count == d.Spk.Where(e => e.Status == null).Count()).Count();
        }
        public int CountGPSSPKO(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsGps == true && d.Spk.Count == d.Spk.Where(e => e.Status == null).Count()).Count();
        }
        public int CountTruckSPKP(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsTruck == true && d.Spk.Where(e => e.Status == "On-Progress").Count() > 0).Count();
        }
        public int CountPendinginSPKP(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsAc == true && d.Spk.Where(e => e.Status == "On-Progress").Count() > 0).Count();
        }
        public int CountBanSPKP(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsBan == true && d.Spk.Where(e => e.Status == "On-Progress").Count() > 0).Count();
        }
        public int CountBoxSPKP(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsBox == true && d.Spk.Where(e => e.Status == "On-Progress").Count() > 0).Count();
        }
        public int CountGPSSPKP(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsGps == true && d.Spk.Where(e => e.Status == "On-Progress").Count() > 0).Count();
        }
        public int CountTruckSPKC(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsTruck == true && d.Spk.Count == d.Spk.Where(e => e.Status == "Closed").Count()).Count();
        }
        public int CountPendinginSPKC(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsAc == true && d.Spk.Count == d.Spk.Where(e => e.Status == "Closed").Count()).Count();
        }
        public int CountBanSPKC(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsBan == true && d.Spk.Count == d.Spk.Where(e => e.Status == "Closed").Count()).Count();
        }
        public int CountBoxSPKC(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsBox == true && d.Spk.Count == d.Spk.Where(e => e.Status == "Closed").Count()).Count();
        }
        public int CountGPSSPKC(){
            return context.Workshop.Where(d => d.Status == "SPK" && d.IsGps == true && d.Spk.Count == d.Spk.Where(e => e.Status == "Closed").Count()).Count();
        }
    }
}