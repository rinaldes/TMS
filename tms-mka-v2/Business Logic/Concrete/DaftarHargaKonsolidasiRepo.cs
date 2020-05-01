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
    public class DaftarHargaKonsolidasiRepo : IDaftarHargaKonsolidasiRepo
    {
        private ContextModel context = new ContextModel();
        public void save(DaftarHargaKonsolidasi dbitem, int id, string dhki=null)
        {
            var query = "";
            if (dbitem.Id == 0) //create
            {
                context.DaftarHargaKonsolidasi.Add(dbitem);
                query += "INSERT INTO dbo.\"DaftarHargaKonsolidasi\" (\"IdCust\", \"PeriodStart\", \"PeriodEnd\") VALUES (" + dbitem.IdCust + ", " + dbitem.PeriodStart + ", " + dbitem.PeriodEnd + ");";
            }
            else //edit
            {
                context.DaftarHargaKonsolidasi.Attach(dbitem);
                query += "UPDATE dbo.\"DaftarHargaKonsolidasi\" SET \"IdCust\" = " + dbitem.IdCust + ", \"PeriodStart\" = " + dbitem.PeriodStart + ", \"PeriodEnd\" = " + dbitem.PeriodEnd +
                    " WHERE \"Id\" = " + dbitem.Id + ";";
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
            query += "DELETE FROM dbo.\"DaftarHargaKonsolidasiAttachment\" WHERE \"IdDaftarHargaKonsolidasi\" = " + dbitem.Id + ";";
            foreach(Context.DaftarHargaKonsolidasiAttachment dhka in dbitem.DaftarHargaKonsolidasiAttachment){
                query += "INSERT INTO dbo.\"DaftarHargaKonsolidasiAttachment\" (\"IdDaftarHargaKonsolidasi\", \"FileName\", \"RFileName\") VALUES ("+dbitem.Id+", "+dhka.FileName+", "+dhka.RFileName + ");";
            }
            query += "DELETE FROM dbo.\"DaftarHargaKonsolidasiKondisi\" WHERE \"IdDaftarHargaKonsolidasi\" = " + dbitem.Id + ";";
            foreach(Context.DaftarHargaKonsolidasiKondisi dhkk in dbitem.DaftarHargaKonsolidasiKondisi){
                query += "INSERT INTO dbo.\"DaftarHargaKonsolidasiKondisi\" (\"IdDaftarHargaKonsolidasi\", kondisi, \"IsInclude\", \"IsBill\", value, \"IsDefault\", \"IsKota\", \"IsTitik\", " +
                    "\"ValKota\", \"ValTitik\", \"IsDelete\") VALUES (" + dhkk.Id + ", " + dhkk.kondisi + ", " + dhkk.IsInclude + ", " + dhkk.IsBill + ", " + dhkk.value + ", " + dhkk.IsDefault + ", " +
                    dhkk.IsKota + ", " + dhkk.IsTitik + ", " + dhkk.ValKota + ", " + dhkk.ValTitik + ", " + dhkk.IsDelete + ");";
            }
            if (dbitem.Id == 0) //create
            {
                context.DaftarHargaKonsolidasi.Add(dbitem);
                foreach (Context.DaftarHargaKonsolidasiItem item in dbitem.DaftarHargaKonsolidasiItem){
                    query += "INSERT INTO dbo.\"DaftarHargaKonsolidasiItem\" (\"IdDaftarHargaKonsolidasi\", \"NamaDaftarHargaRute\", \"ListIdRute\", \"ListNamaRute\", \"IdJenisKendaraan\", " +
                        "\"MinKg\", \"MaxKg\", \"Harga\", \"IsAsuransi\", \"Premi\", \"NilaiTanggungan\", \"Keterangan\", \"PihakPenanggung\", \"TipeNilaiTanggungan\", \"IdSatuanHarga\") VALUES ( " +
                        dbitem.Id + ", " + item.NamaDaftarHargaRute + ", " + item.ListIdRute + ", " + item.ListNamaRute + ", " + item.IdJenisKendaraan + ", " + item.MinKg + ", " + item.MaxKg + ", " +
                        item.Harga + ", " + item.IsAsuransi + ", " + item.Premi + ", " + item.NilaiTanggungan + ", " + item.Keterangan + ", " + item.PihakPenanggung + ", " + item.TipeNilaiTanggungan +
                        ", " + item.IdSatuanHarga + ");";
                }
            }
            else
                query += dhki;
            var auditrail = new Auditrail {
                Actionnya = dbitem.Id == 0 ? "Add" : "Edit", EventDate = DateTime.Now, Modulenya = "Daftar Harga Konsolidasi", QueryDetail = query,
                RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public List<DaftarHargaKonsolidasi> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<DaftarHargaKonsolidasi> list = context.DaftarHargaKonsolidasi;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DaftarHargaKonsolidasi>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<DaftarHargaKonsolidasi>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderBy<DaftarHargaKonsolidasi>("Id"); //default, wajib ada atau EF error
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
            List<DaftarHargaKonsolidasi> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<DaftarHargaKonsolidasi> items = context.DaftarHargaKonsolidasi;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<DaftarHargaKonsolidasi>(filters, ref items);
            }

            return items.Count();
        }
        public DaftarHargaKonsolidasi FindByPK(int id)
        {
            return context.DaftarHargaKonsolidasi.Where(d => d.Id == id).FirstOrDefault();
        }
        public Context.DaftarHargaKonsolidasiItem FindItemByPK(int id)
        {
            return context.DaftarHargaKonsolidasiItem.Where(d => d.Id == id).FirstOrDefault();
        }
        public DaftarHargaKonsolidasi FindByItemId(int id)
        {
            return context.DaftarHargaKonsolidasi.Where(d => d.DaftarHargaKonsolidasiItem.Any(i => i.Id == id)).FirstOrDefault();
        }
        public void delete(DaftarHargaKonsolidasi dbitem, int id)
        {
            context.DaftarHargaKonsolidasi.Remove(dbitem);
            var query = "DELETE FROM dbo.\"DaftarHargaKonsolidasi\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Harga Konsolidasi", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
        public bool IsPeriodeStartExist(DateTime startPeriode, int custId, int id = 0)
        {
            if (id == 0)
                return context.DaftarHargaKonsolidasi.Any(d => d.PeriodStart == startPeriode && d.IdCust == custId);
            else
                return context.DaftarHargaKonsolidasi.Any(d => d.PeriodStart == startPeriode && d.IdCust == custId && d.Id != id);
        }
        public bool IsPeriodValid(DateTime StrPeriode, DateTime EndPeriode, int custId, int id = 0)
        {
            if (id == 0)
                return context.DaftarHargaKonsolidasi.Any(d => d.IdCust == custId && (StrPeriode <= d.PeriodEnd && EndPeriode >= d.PeriodStart));
            else
                return context.DaftarHargaKonsolidasi.Any(d => d.IdCust == custId && (StrPeriode <= d.PeriodEnd && EndPeriode >= d.PeriodStart) && d.Id != id);
        }
        public void FindRuteTruk(int id, out string IdRute, out int idJenisTruk)
        {
            try
            {
                DaftarHargaKonsolidasi db = context.DaftarHargaKonsolidasi.Where(d => d.DaftarHargaKonsolidasiItem.Any(i => i.Id == id)).FirstOrDefault();
                DaftarHargaKonsolidasiItem dbitem = db.DaftarHargaKonsolidasiItem.Where(i => i.Id == id).FirstOrDefault();
                IdRute = dbitem.ListIdRute;
                idJenisTruk = dbitem.IdJenisKendaraan.Value;
            }
            catch (Exception)
            {
                IdRute = "";
                idJenisTruk = -1;                
            }

        }
    }
}