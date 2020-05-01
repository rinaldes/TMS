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
    public class UserRepo : IUserRepo
    {
        private ContextModel context = new ContextModel();
        public void save(User dbitem, int id, string quer=null){
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.User.Add(dbitem);
                query += "INSERT INTO dbo.\"User\" (\"Nik\", \"Username\", \"Password\", \"Fristname\", \"Lastname\", \"Email\", \"Phone\", path_foto) VALUES ( '" + dbitem.Nik + "', '" + dbitem.Username +
                    "', '" + dbitem.Password + "', '" + dbitem.Fristname + "', '" + dbitem.Lastname + "', '" + dbitem.Email + "', '" + dbitem.Phone + "', '" + dbitem.path_foto + "');";
            }
            else //edit
            {
                context.User.Attach(dbitem);
                query += "UPDATE dbo.\"User\" SET \"Nik\" = " + dbitem.Nik + ", \"Username\" = " + dbitem.Username + ", \"Password\" = " + dbitem.Password + ", \"Fristname\" = " + dbitem.Fristname + 
                    ", \"Lastname\" = " + dbitem.Lastname + ", \"Email\" = " + dbitem.Email + ", \"Phone\" = " + dbitem.Phone + ", path_foto = " + dbitem.path_foto + " WHERE \"Id\" = " + dbitem.Id + ";";
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
            if (dbitem.Id == 0) //create
            {
                foreach (Context.UserMenus tp in dbitem.UserMenus){
                    query += "INSERT INTO dbo.\"UserMenus\" (\"IdMenu\", \"IdUser\") VALUES (" + tp.IdMenu + ", " + tp.IdUser + ");";
                }
            }
            else
                query += quer;
            foreach (Context.UserRole tp in dbitem.UserRole){
                query += "INSERT INTO dbo.\"UserRole\" (\"IdUser\", \"IdRole\") VALUES (" + tp.IdUser + ", " + tp.IdRole + ");";
            }
            var auditrail = new Auditrail {
                Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "User", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public List<User> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<User> list = context.User;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<User>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<User>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<User>("id"); //default, wajib ada atau EF error
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
            List<User> result = takeList.ToList();
            return result;
        }
        public List<User> FindAllName(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<User> list = context.User;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<User>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {

                    list = list.OrderBy<User>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<User>("id"); //default, wajib ada atau EF error
            }

            //take & skip
            var takeList = list;
            if (skip != null && skip != 0)
            {
                takeList = takeList.Skip(skip.Value);
            }
            if (take != null && skip != 0)
            {
                takeList = takeList.Take(take.Value);
            }

            //return result
            //var sql = takeList.ToString();
            List<User> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<User> items = context.User;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<User>(filters, ref items);
            }

            return items.Count();
        }
        public User FindByPK(int id)
        {
            return context.User.Where(d => d.Id == id).FirstOrDefault();
        }
        public User FindByUsername(string username)
        {
            return context.User.Where(d => d.Username == username).FirstOrDefault();
        }
        public void delete(User dbitem, int id)
        {
            context.User.Remove(dbitem);
            var query = "DELETE FROM dbo.\"UserMenus\" WHERE \"IdUser\" = " + dbitem.Id + ";DELETE FROM dbo.\"UserMenus\" WHERE \"IdUser\" = " + dbitem.Id + ";DELETE FROM dbo.\"User\" WHERE \"Id\" = " +
                dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "User", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsExist(string nama, int id = 0)
        {
            if (id == 0)
            { return context.User.Any(p => p.Username == nama); }
            else
            { return context.User.Any(p => p.Username == nama && p.Id != id); }
        }
    }
}