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
    public class AdminUangJalanRepo : IAdminUangJalanRepo
    {
        private ContextModel context = new ContextModel();
        public void save(AdminUangJalan dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.AdminUangJalan.Add(dbitem);
            }
            else //edit
            {
                //int id = dbitem.Id
                context.AdminUangJalan.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        protected string GetIPAddress()
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

        public List<AdminUangJalan> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> list = context.AdminUangJalan;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<AdminUangJalan>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<AdminUangJalan>("Id"); //default, wajib ada atau EF error
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
            List<AdminUangJalan> result = takeList.ToList();
            return result;
        }
        public List<AdminUangJalanUangTf> FindAllByDriverTranfer(int idDriver)
        {
            List<AdminUangJalan> list = context.AdminUangJalan.Where(d => d.AdminUangJalanUangTf.Any( t => t.IdDriverPenerima == idDriver && t.isTf)).ToList();

            List<AdminUangJalanUangTf> listResult = new List<AdminUangJalanUangTf>();
            foreach (var item in list)
            {
                listResult.AddRange(item.AdminUangJalanUangTf);
            }

            return listResult;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<AdminUangJalan> items = context.AdminUangJalan;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<AdminUangJalan>(filters, ref items);
            }

            return items.Count();
        }
        public AdminUangJalan FindByPK(int id)
        {
            return context.AdminUangJalan.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(AdminUangJalan dbitem)
        {
            context.AdminUangJalan.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Admin Uang Jalan", QueryDetail = "Delete Admin Uang Jalan no" + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public AdminUangJalan FindBySalesOrderId(int id)
        {
            return context.AdminUangJalan.Where(d => d.SalesOrderId == id).FirstOrDefault();
        }

        public AdminUangJalan FindBySalesOrderIdForKasirReport(int id)
        {
            return context.AdminUangJalan.Where(d => d.SalesOrderId == id && d.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai" && s.isTf == true).Count() > 0).FirstOrDefault();
        }

        public int? getUrutanUang(DateTime valdate)
        {
            DateTime firstDayOfMonth = new DateTime(valdate.Year, valdate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return context.AdminUangJalanUangTf.Where(d => d.TanggalAktual >= firstDayOfMonth && d.TanggalAktual <= lastDayOfMonth).Count() == 0 ? 0 : context.AdminUangJalanUangTf.Where(d => d.TanggalAktual >= firstDayOfMonth && d.TanggalAktual <= lastDayOfMonth).Max(d => d.Urutan);
        }
        public string terbilang(decimal number){
            /*
                250000 => dua ratus lima puluh ribu
            */
            if (number < 1000000){
                int int_number = int.Parse(number.ToString());
                int first_number = int_number / 100000;
                string first_unit = "RATUS";
                int second_number = int_number % 100000;
                string second_unit = "RIBU";
                return satuanTerbilang(first_number) + " " + first_unit + " " + satuanTerbilang(second_number) + " " + second_unit + " " + "RP";
            }
                return "";
        }

        public string satuanTerbilang(int number){
            if (number == 1)
                return "SATU";
            else if (number == 2)
                return "DUA";
            else if (number == 3)
                return "TIGA";
            else if (number == 4)
                return "EMPAT";
            else if (number == 5)
                return "LIMA";
            else if (number == 6)
                return "ENAM";
            else if (number == 7)
                return "TUJUH";
            else if (number == 8)
                return "DELAPAN";
            else if (number == 9)
                return "SEMBILAN";
            else
                return "SEPULUH";
        }
    }
}