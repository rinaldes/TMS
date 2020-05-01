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
    public class BatalOrderRepo : IBatalOrderRepo
    {
        private ContextModel context = new ContextModel();
        public void save(BatalOrder dbitem, int id)
        {
            if (dbitem.Id == 0) //create
            {
                context.BatalOrder.Add(dbitem);
                var query = "INSERT INTO dbo.\"BatalOrder\" (\"Id\", \"IdSalesOrder\", \"Keterangan\", \"ModifiedDate\", \"IsBatalTruk\", \"Code\", \"IdSoKontrak\") VALUES (" + dbitem.Id + ", " + 
                    dbitem.IdSalesOrder + ", " + dbitem.Keterangan + ", " + dbitem.ModifiedDate + ", " + dbitem.IsBatalTruk + ", " + dbitem.Code + ", " + dbitem.IdSoKontrak + ");";
                var auditrail = new Auditrail { 
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Batal Order", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.BatalOrder.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<BatalOrder> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<BatalOrder> list = context.BatalOrder;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<BatalOrder>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<BatalOrder>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<BatalOrder>("Id"); //default, wajib ada atau EF error
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
            List<BatalOrder> result = takeList.ToList();
            return result;
        }        
        public int Count(FilterInfo filters = null)
        {
            IQueryable<BatalOrder> items = context.BatalOrder;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<BatalOrder>(filters, ref items);
            }

            return items.Count();
        }
        public BatalOrder FindByPK(int id)
        {
            return context.BatalOrder.Where(d => d.Id == id).FirstOrDefault();
        }
        public BatalOrder FindBySO(int IdSalesOrder)
        {
            return context.BatalOrder.Where(d => d.IdSalesOrder == IdSalesOrder).FirstOrDefault();
        }
        public void delete(BatalOrder dbitem)
        {
            context.BatalOrder.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Batal Order", QueryDetail = "Delete Batal Order " + dbitem.SalesOrder, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public bool IsExist(int SalesOrderId, int VehicleId, int DriverId)
        {
            return context.SettlementBatal.Any(d => d.IdDriver == DriverId && d.IdSalesOrder == SalesOrderId && d.IdDataTruck == VehicleId);
        }
    }
}