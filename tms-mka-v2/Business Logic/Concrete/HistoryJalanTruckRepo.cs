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
    public class HistoryJalanTruckRepo : IHistoryJalanTruckRepo
    {
        private ContextModel context = new ContextModel();
        public void save(HistoryJalanTruck dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.HistoryJalanTruck.Add(dbitem);            
            }
            else //edit
            {
                context.HistoryJalanTruck.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        public List<HistoryJalanTruck> FindByDriver(int idDriver) 
        {
            return context.HistoryJalanTruck.Where(d => d.IdDriver1 == idDriver || d.IdDriver2 == idDriver).ToList();
        }
        public List<HistoryJalanTruck> FindByTruck(int idTruck)
        {
            return context.HistoryJalanTruck.Where(d => d.IdTruck == idTruck).ToList();
        }
        public HistoryJalanTruck FindByAdm(int idAdm)
        {
            return context.HistoryJalanTruck.Where(d => d.IdAdminUangJalan == idAdm).FirstOrDefault();
        }
    }
}