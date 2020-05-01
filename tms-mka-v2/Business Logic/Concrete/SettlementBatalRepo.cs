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
    public class SettlementBatalRepo : ISettlementBatalRepo
    {
        private ContextModel context = new ContextModel();
        public void save(SettlementBatal dbitem, int id, string modul)
        {
            dbitem.ModifiedDate = DateTime.Now;

            if (dbitem.Id == 0) //create
            {
                var query = "INSERT INTO dbo.\"SettlementBatal\" (\"IdSalesOrder\", \"KasDiterima\", \"TransferDiterima\", \"SolarDiterima\", \"KapalDiterima\", \"KeteranganDiterima\", \"KasDiakui\"," +
                    "\"TransferDiakui\", \"SolarDiakui\", \"KapalDiakui\", \"KeteranganDiakui\", \"KasKembali\", \"TransferKembali\", \"SolarKembali\", \"KapalKembali\", \"KeteranganKembali\"," +
                    "\"KasAktual\", \"TransferAktual\", \"SolarAktual\", \"KapalAktual\", \"KeteranganAktual\", \"KasSelisih\", \"TransferSelisih\", \"SolarSelisih\", \"KapalSelisih\"," +
                    "\"KeteranganSelisih\", \"Keterangan\", \"IsProses\", \"ModifiedDate\", \"ModifiedBy\", \"JenisBatal\", \"IdDriver\", \"IdAdminUangJalan\", \"IdSoKontrak\", \"Code\", \"IdDataTruck\"" +
                    ") VALUES (" + dbitem.IdSalesOrder + ", " + dbitem.KasDiterima + ", " + dbitem.TransferDiterima + ", " + dbitem.SolarDiterima + ", " + dbitem.KapalDiterima + ", " +
                    dbitem.KeteranganDiterima + ", " + dbitem.KasDiakui + ", " + dbitem.TransferDiakui + ", " + dbitem.SolarDiakui + ", " + dbitem.KapalDiakui + ", " + dbitem.KeteranganDiakui + ", " + 
                    dbitem.KasKembali + ", " + dbitem.TransferKembali + ", " + dbitem.SolarKembali + ", " + dbitem.KapalKembali + ", " + dbitem.KeteranganKembali + ", " + dbitem.KasAktual + ", " + 
                    dbitem.TransferAktual + ", " + dbitem.SolarAktual + ", " + dbitem.KapalAktual + ", " + dbitem.KeteranganAktual + ", " + dbitem.KasSelisih + ", " + dbitem.TransferSelisih + ", " + 
                    dbitem.SolarSelisih + ", " + dbitem.KapalSelisih + ", " + dbitem.KeteranganSelisih + ", " + dbitem.Keterangan + ", " + dbitem.IsProses + ", " + dbitem.ModifiedDate + ", " + 
                    dbitem.ModifiedBy+", " + dbitem.JenisBatal + ", " + dbitem.IdDriver + ", " + dbitem.IdAdminUangJalan + ", " + dbitem.IdSoKontrak + ", " + dbitem.Code + ", " + dbitem.IdDataTruck + ");";
                context.SettlementBatal.Add(dbitem);
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = modul, QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.SettlementBatal.Attach(dbitem);
                var query = "UPDATE dbo.\"SettlementBatal\" SET \"KasDiterima\" = " +dbitem.KasDiterima + ", \"TransferDiterima\" = " + 
                    dbitem.TransferDiterima + ", \"SolarDiterima\" = " + dbitem.SolarDiterima + ", \"KapalDiterima\" = " + dbitem.KapalDiterima + ", \"KeteranganDiterima\" = " + dbitem.KeteranganDiterima +
                    ", \"KasDiakui\" = " + dbitem.KasDiakui + ", \"TransferDiakui\" = " + dbitem.TransferDiakui + ", \"SolarDiakui\" = " + dbitem.SolarDiakui + ", \"KapalDiakui\" = " + dbitem.KapalDiakui +
                    ", \"KeteranganDiakui\" = " + dbitem.KeteranganDiakui + ", \"KasKembali\" = " + dbitem.KasKembali + ", \"TransferKembali\" = " + dbitem.TransferKembali + ", \"SolarKembali\" = " +
                    dbitem.SolarKembali + ", \"KapalKembali\" = " + dbitem.KapalKembali + ", \"KeteranganKembali\" = " + dbitem.KeteranganKembali + ", \"KasAktual\" = " + dbitem.KasAktual +
                    ", \"TransferAktual\" = " + dbitem.TransferAktual + ", \"SolarAktual\" = " + dbitem.SolarAktual + ", \"KapalAktual\" = " + dbitem.KapalAktual + ", \"KeteranganAktual\" = " +
                    dbitem.KeteranganAktual + ", \"KasSelisih\" = " + dbitem.KasSelisih + ", \"TransferSelisih\" = " + dbitem.TransferSelisih + ", \"SolarSelisih\" = " + dbitem.SolarSelisih +
                    ", \"KapalSelisih\" = " + dbitem.KapalSelisih + ", \"KeteranganSelisih\" = " + dbitem.KeteranganSelisih + ", \"Keterangan\" = " + dbitem.Keterangan + ", \"IsProses\" = " + 
                    dbitem.IsProses + ", \"ModifiedDate\" = " + dbitem.ModifiedDate + ", \"ModifiedBy\" = " + dbitem.ModifiedBy + ", \"JenisBatal\" = " + dbitem.JenisBatal + ", \"IdDriver\" = " + 
                    dbitem.IdDriver+ ", \"IdAdminUangJalan\" = " + dbitem.IdAdminUangJalan + ", \"Code\" = " + dbitem.Code + ", \"IdDataTruck\"" + dbitem.IdDataTruck + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = modul, QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<SettlementBatal> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SettlementBatal> list = context.SettlementBatal.Where(d => d.IdSoKontrak == null && !d.IsProses && (d.KasDiterima > 0 || d.TransferDiterima > 0 || d.KapalDiterima > 0 || d.SolarDiterima > 0 || context.AdminUangJalan.Any(e => e.Id == d.IdAdminUangJalan && e.KasbonDriver1 > 0)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SettlementBatal>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SettlementBatal>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SettlementBatal>("\"Id\" DESC"); //default, wajib ada atau EF error
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
            List<SettlementBatal> result = takeList.ToList();
            return result;
        }
        public List<SettlementBatal> FindAllReport(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SettlementBatal> list = context.SettlementBatal.Where(d => d.IdSoKontrak != null || d.IsProses && (d.KasDiterima > 0 || d.TransferDiterima > 0 || d.KapalDiterima > 0 || d.SolarDiterima > 0 || context.AdminUangJalan.Any(e => e.Id == d.IdAdminUangJalan && e.KasbonDriver1 > 0)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SettlementBatal>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SettlementBatal>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SettlementBatal>("\"Id\" DESC"); //default, wajib ada atau EF error
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
            List<SettlementBatal> result = takeList.ToList();
            return result;
        }
        public List<SettlementBatal> FindAllReportKontrak(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SettlementBatal> list = context.SettlementBatal.Where(d => d.IdSoKontrak != null);

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SettlementBatal>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SettlementBatal>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SettlementBatal>("\"Id\" DESC"); //default, wajib ada atau EF error
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
            List<SettlementBatal> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<SettlementBatal> items = context.SettlementBatal.Where(d => d.IdSoKontrak == null && !d.IsProses && (d.KasDiterima > 0 || d.TransferDiterima > 0 || d.KapalDiterima > 0 || d.SolarDiterima > 0 || context.AdminUangJalan.Any(e => e.Id == d.IdAdminUangJalan && e.KasbonDriver1 > 0)));

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SettlementBatal>(filters, ref items);
            }

            return items.Count();
        }
        public SettlementBatal FindByPK(int id)
        {
            return context.SettlementBatal.Where(d => d.Id == id).FirstOrDefault();
        }
        public SettlementBatal FindBySo(int IdSalesOrder)
        {
            return context.SettlementBatal.Where(d => d.IdSalesOrder == IdSalesOrder).FirstOrDefault();
        }
        public SettlementBatal FindByAUJ(int IdAUJ)
        {
            return context.SettlementBatal.Where(d => d.IdAdminUangJalan == IdAUJ).FirstOrDefault();
        }
        public void delete(SettlementBatal dbitem)
        {
            context.SettlementBatal.Remove(dbitem);

            context.SaveChanges();
        }
    }
}