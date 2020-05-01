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
    public class DaftarHargaOnCallRepo : IDaftarHargaOnCallRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DaftarHargaOnCall dbitem, int id, string dhki=null)
        {
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.DaftarHargaOnCall.Add(dbitem);
                query += "INSERT INTO dbo.\"DaftarHargaOnCall\" (\"IdCust\", \"PeriodStart\", \"PeriodEnd\") VALUES (" + dbitem.IdCust + ", " + dbitem.PeriodStart + ", " + dbitem.PeriodEnd + ");";
            }
            else //edit
            {
                context.DaftarHargaOnCall.Attach(dbitem);
                query += "UPDATE dbo.\"DaftarHargaOnCall\" SET \"IdCust\" = " + dbitem.IdCust + ", \"PeriodStart\" = " + dbitem.PeriodStart + ", \"PeriodEnd\" = " + dbitem.PeriodEnd +
                    " WHERE \"Id\" = " + dbitem.Id + ";";
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            query += "DELETE FROM dbo.\"DaftarHargaOnCallAttachment\" WHERE \"IdDaftarHargaOnCall\" = " + dbitem.Id + ";";
            foreach (Context.DaftarHargaOnCallAttachment dhka in dbitem.DaftarHargaOnCallAttachment)
            {
                query += "INSERT INTO dbo.\"DaftarHargaOnCallAttachment\" (\"IdDaftarHargaOnCall\", \"FileName\", \"RFileName\") VALUES (" + dbitem.Id + ", " + dhka.FileName + ", " + dhka.RFileName + ");";
            }
            query += "DELETE FROM dbo.\"DaftarHargaOnCallKondisi\" WHERE \"IdDaftarHargaOnCall\" = " + dbitem.Id + ";";
            foreach (Context.DaftarHargaOnCallKondisi dhkk in dbitem.DaftarHargaOnCallKondisi)
            {
                query += "INSERT INTO dbo.\"DaftarHargaOnCallKondisi\" (\"IdDaftarHargaOnCall\", kondisi, \"IsInclude\", \"IsBill\", value, \"IsDefault\", \"IsKota\", \"IsTitik\", " +
                    "\"ValKota\", \"ValTitik\", \"IsDelete\") VALUES (" + dhkk.Id + ", " + dhkk.kondisi + ", " + dhkk.IsInclude + ", " + dhkk.IsBill + ", " + dhkk.value + ", " + dhkk.IsDefault + ", " +
                    dhkk.IsKota + ", " + dhkk.IsTitik + ", " + dhkk.ValKota + ", " + dhkk.ValTitik + ", " + dhkk.IsDelete + ");";
            }
            if (dbitem.Id == 0) //create
            {
                context.DaftarHargaOnCall.Add(dbitem);
                foreach (Context.DaftarHargaOnCallItem item in dbitem.DaftarHargaOnCallItem)
                {
                    query += "INSERT INTO dbo.\"DaftarHargaOnCallItem\"(\"IdDaftarHargaOnCall\", \"ListIdRute\", \"ListNamaRute\", \"MinKg\", \"Harga\", \"IsAsuransi\", " +
                        "\"Premi\", \"Keterangan\", \"NamaRuteDaftarHarga\", \"IdJenisTruck\", \"IsAdHoc\", \"PihakPenanggung\", \"TipeNilaiTanggungan\", \"NilaiTanggungan\"," +
                        "\"IdSatuanHarga\") VALUES (" + dbitem.Id + ", " + item.ListIdRute + ", " + item.ListNamaRute + ", " + ", " + item.MinKg + ", " + item.Harga + ", " +
                        item.IsAsuransi + ", " + item.Premi + ", " + item.Keterangan + ", " + item.NamaRuteDaftarHarga + ", " + item.IdJenisTruck + ", " + item.IsAdHoc + ", " +
                        item.PihakPenanggung + ", " + item.TipeNilaiTanggungan + ", " + item.NilaiTanggungan + ", " + item.IdSatuanHarga + ");";
                }
            }
            else
                query += dhki;
            var auditrail = new Auditrail{
                Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "Daftar Harga On Call", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public List<DaftarHargaOnCall> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DaftarHargaOnCall> list = context.DaftarHargaOnCall;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DaftarHargaOnCall>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<DaftarHargaOnCall>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DaftarHargaOnCall>("Id"); //default, wajib ada atau EF error
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
            List<DaftarHargaOnCall> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DaftarHargaOnCall> items = context.DaftarHargaOnCall;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DaftarHargaOnCall>(filters, ref items);
            }

            return items.Count();
        }
        public DaftarHargaOnCall FindByPK(int id)
        {
            return context.DaftarHargaOnCall.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(DaftarHargaOnCall dbitem)
        {
            context.DaftarHargaOnCall.Remove(dbitem);
            var auditrail = new Auditrail { Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Daftar Harga Oncall", QueryDetail = "Delete " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsPeriodeStartExist(DateTime startPeriode, int custId, int id = 0)
        {
            if (id == 0)
                return context.DaftarHargaOnCall.Any(d => d.PeriodStart == startPeriode && d.IdCust == custId);
            else
                return context.DaftarHargaOnCall.Any(d => d.PeriodStart == startPeriode && d.IdCust == custId && d.Id != id);
        }
        public DaftarHargaOnCall FindByPeriodAndCust(DateTime startPeriode, DateTime endPeriode, int custId)
        {
            return context.DaftarHargaOnCall.Where(d => startPeriode >= d.PeriodStart && endPeriode <= d.PeriodEnd && d.IdCust == custId).FirstOrDefault();
        }
        public bool IsPeriodValid(DateTime StrPeriode, DateTime EndPeriode, int custId, int id = 0)
        {
            if (id == 0)
                return context.DaftarHargaOnCall.Any(d => d.IdCust == custId && (StrPeriode <= d.PeriodEnd && EndPeriode >= d.PeriodStart));
            else
                return context.DaftarHargaOnCall.Any(d => d.IdCust == custId && (StrPeriode <= d.PeriodEnd && EndPeriode >= d.PeriodStart) && d.Id != id);
        }
        public void FindRuteTruk(int id, out string IdRute, out int idJenisTruk)
        {
            try
            {
                DaftarHargaOnCall db = context.DaftarHargaOnCall.Where(d => d.DaftarHargaOnCallItem.Any(i => i.Id == id)).FirstOrDefault();
                DaftarHargaOnCallItem dbitem = db.DaftarHargaOnCallItem.Where(i => i.Id == id).FirstOrDefault();
                IdRute = dbitem.ListIdRute;
                idJenisTruk = dbitem.IdJenisTruck;
            }
            catch (Exception)
            {
                IdRute = "";
                idJenisTruk = -1;
            }

        }
    }
}