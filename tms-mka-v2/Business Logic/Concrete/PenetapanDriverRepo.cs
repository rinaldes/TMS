﻿using System;
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
    public class PenetapanDriverRepo : IPenetapanaDriverRepo
    {
        private ContextModel context = new ContextModel();
        public void save(PenetapanDriver dbitem){
            if (dbitem.Id == 0) //create
            {
                context.PenetapanDriver.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Penetapan Driver", QueryDetail = "Add " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                context.PenetapanDriver.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Penetapan Driver", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
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

        public List<PenetapanDriver> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<PenetapanDriver> list = context.PenetapanDriver;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<PenetapanDriver>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {

                    list = list.OrderBy<PenetapanDriver>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<PenetapanDriver>("Id"); //default, wajib ada atau EF error
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
            List<PenetapanDriver> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<PenetapanDriver> items = context.PenetapanDriver;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<PenetapanDriver>(filters, ref items);
            }

            return items.Count();
        }
        public PenetapanDriver FindByPK(int id)
        {
            return context.PenetapanDriver.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(PenetapanDriver dbitem)
        {
            context.PenetapanDriver.Remove(dbitem);
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Penetapan Driver", QueryDetail = "Delete " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = 1
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(int IdTruck)
        {
            return context.PenetapanDriver.Any(d => d.IdDataTruck == IdTruck);
        }

        public bool isExistDriver(int IdDriver,  int id)
        {
            if(id == 0)
                return context.PenetapanDriver.Any(d => d.IdDriver1 == IdDriver || d.IdDriver2 == IdDriver);
            else
                return context.PenetapanDriver.Any(d => d.IdDriver1 == IdDriver || d.IdDriver2 == IdDriver && d.Id != id);
        }

        public Context.DriverTruckHistory FindLastDriverHistory(string type) {
            return context.DriverTruckHistory.Where(d => d.Type == type).FirstOrDefault();
        }
    }
}