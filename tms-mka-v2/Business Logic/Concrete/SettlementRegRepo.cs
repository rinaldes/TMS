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
    public class SettlementRegRepo : ISettlementRegRepo
    {
        private ContextModel context = new ContextModel();
        public void save(SettlementReguler dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                context.SettlementReguler.Add(dbitem);
                var query = "INSERT INTO dbo.\"SettlementReguler\" (\"IdSalesOrder\", \"KasDiakui\", \"TransferDiakui\", \"SolarDiakui\", \"KapalDiakui\", \"KeteranganDiterima\", \"KeteranganDiakui\", " +
                    "\"KasAktual\", \"TransferAktual\", \"SolarAktual\", \"KapalAktual\", \"KeteranganKembali\", \"KeteranganAktual\", \"KasSelisih\", \"TransferSelisih\", \"SolarSelisih\", " +
                    "\"KapalSelisih\", \"KeteranganSelisih\", \"ModifiedDate\", \"ModifiedBy\", \"KasDiterima\", \"TransferDiterima\", \"SolarDiterima\", \"KapalDiterima\", \"KasKembali\", " +
                    "\"TransferKembali\", \"SolarKembali\", \"KapalKembali\", \"TotalCash\", \"TanggalCash\", \"IdDriverTujuan\", \"IdDriverTitip\", \"TotalTf\", \"TanggalTf\", \"IdAtm\", " +
                    "\"KeteranganPembayaran\", \"TotalBayar\", \"LisSoKontrak\", \"Code\") VALUES (" + dbitem.IdSalesOrder + ", " + dbitem.KasDiakui + ", " + dbitem.TransferDiakui + ", " + 
                    dbitem.SolarDiakui + ", " + dbitem.KapalDiakui + ", " + dbitem.KeteranganDiterima + ", " + dbitem.KeteranganDiakui + ", " + dbitem.KasAktual + ", " + dbitem.TransferAktual + ", " +
                    dbitem.SolarAktual + ", " + dbitem.KapalAktual + ", " + dbitem.KeteranganKembali + ", " + dbitem.KeteranganAktual + ", " + dbitem.KasSelisih + ", " + dbitem.TransferSelisih + ", " + 
                    dbitem.SolarSelisih + ", " + dbitem.KapalSelisih + ", " + dbitem.KeteranganSelisih + ", " + dbitem.ModifiedDate + ", " + dbitem.ModifiedBy + ", " + dbitem.KasDiterima + ", " + 
                    dbitem.TransferDiterima + ", " + dbitem.SolarDiterima + ", " + dbitem.KapalDiterima + ", " + dbitem.KasKembali + ", " + dbitem.TransferKembali + ", " + dbitem.SolarKembali + ", " + 
                    dbitem.KapalKembali + ", " + dbitem.TotalCash + ", " + dbitem.TanggalCash + ", " + dbitem.IdDriverTujuan + ", " + dbitem.IdDriverTitip + ", " + dbitem.TotalTf + ", " + 
                    dbitem.TanggalTf + ", " + dbitem.IdAtm + ", " + dbitem.KeteranganPembayaran + ", " + dbitem.TotalBayar + ", " + dbitem.LisSoKontrak + ", " + dbitem.Code + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Settlement Reguler", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.SettlementReguler.Attach(dbitem);
                var query = "UPDATE dbo.\"SettlementReguler\" SET \"KasDiakui\" = " + dbitem.KasDiakui + ", \"TransferDiakui\" = " + dbitem.TransferDiakui + ",\"SolarDiakui\" = " + dbitem.SolarDiakui +
                    ", \"KapalDiakui\" = " + dbitem.KapalDiakui + ", \"KeteranganDiterima\" = " + dbitem.KeteranganDiterima + ", \"KeteranganDiakui\" = " + dbitem.KeteranganDiakui + ", \"KasAktual\" = " +
                    dbitem.KasAktual + ", \"TransferAktual\" = " + dbitem.TransferAktual + ", \"SolarAktual\" = " + dbitem.SolarAktual + ", \"KapalAktual\" = " + dbitem.KapalAktual +
                    ", \"KeteranganKembali\" = " + dbitem.KeteranganKembali + ", \"KeteranganAktual\" = " + dbitem.KeteranganAktual + ", \"KasSelisih\" = " + dbitem.KasSelisih +
                    ", \"TransferSelisih\" = " + dbitem.TransferSelisih + ", \"SolarSelisih\" = " + dbitem.SolarSelisih + ", \"KapalSelisih\" = " + dbitem.KapalSelisih + ", \"KeteranganSelisih\" = " +
                    dbitem.KeteranganSelisih + ", \"ModifiedDate\" = " + dbitem.ModifiedDate + ", \"ModifiedBy\" = " + dbitem.ModifiedBy + ", \"KasDiterima\" = " + dbitem.KasDiterima + 
                    ", \"TransferDiterima\" = " + dbitem.TransferDiterima + ", \"SolarDiterima\" = " + dbitem.SolarDiterima + ", \"KapalDiterima\" = " + dbitem.KapalDiterima + ", \"KasKembali\" = " +
                    dbitem.KasKembali + ", \"TransferKembali\" = " + dbitem.TransferKembali + ", \"SolarKembali\" = " + dbitem.SolarKembali + ", \"KapalKembali\" = " + dbitem.KapalKembali +
                    ", \"TotalCash\" = " + dbitem.TotalCash + ", \"TanggalCash\" = " + dbitem.TanggalCash + ", \"IdDriverTujuan\" = " + dbitem.IdDriverTujuan + ", \"IdDriverTitip\" = " + 
                    dbitem.IdDriverTitip + ", \"TotalTf\" = " + dbitem.TotalTf + ", \"TanggalTf\" = " + dbitem.TanggalTf + ", \"IdAtm\" = " + dbitem.IdAtm + ", \"KeteranganPembayaran\" = " + 
                    dbitem.KeteranganPembayaran+", \"TotalBayar\" = "+dbitem.TotalBayar + ", \"LisSoKontrak\" = " + dbitem.LisSoKontrak + ", \"Code\" = " + dbitem.Code + "WHERE \"Id\" = "+ dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Settlement List", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 
            };

                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<SettlementReguler> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<SettlementReguler> list = context.SettlementReguler;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SettlementReguler>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<SettlementReguler>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<SettlementReguler>("Id"); //default, wajib ada atau EF error
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
            List<SettlementReguler> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<SettlementReguler> items = context.SettlementReguler;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SettlementReguler>(filters, ref items);
            }

            return items.Count();
        }
        public SettlementReguler FindByPK(int id)
        {
            return context.SettlementReguler.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(SettlementReguler dbitem, int id)
        {
            context.SettlementReguler.Remove(dbitem);
            var query = "DELETE FROM dbo.\"SettlementReguler\" WHERE \"Id\" = " + dbitem.Id +";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Settlement Reguler", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}