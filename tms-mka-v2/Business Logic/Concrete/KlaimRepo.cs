using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using tms_mka_v2.Context;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Linq;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class KlaimRepo : IKlaimRepo
    {
        private ContextModel context = new ContextModel();
        public void save(Klaim dbitem, int id){
            try{
                if (dbitem.Id == 0 && context.Klaim.Where(d => d.NoKlaim == dbitem.NoKlaim).Count() == 0) //create
                {
                    context.Klaim.Add(dbitem);
                    var query = "INSERT INTO dbo.\"Klaim\" (\"NoKlaim\", \"TanggalPengajuan\", \"StatusKlaim\", \"SumberInformasiId\", \"LaporanKejadian\", \"HasilPemeriksaan\", \"Penyelesaian\", \"Kesalahan\", " +
                        "\"KesalahanLain\", \"SalesOrderId\", \"TotalPengajuanClaim\", \"NilaiDisetujui\", \"AsuransiFlag\", \"Asuransi\", \"BebanClaim\", \"BebanClaimDriverPercentage\", \"BebanClaimDriver\", " +
                        "\"BebanClaimKantorPercentage\", \"BebanClaimKantor\", \"Keterangan\", \"LastUpdate\", \"SalesOrderKontrakId\", \"IdBap\", \"NoBap\", \"IsClaim\", \"Code\") VALUES (" + dbitem.NoKlaim + ", " +
                        dbitem.TanggalPengajuan + ", " + dbitem.StatusKlaim + ", " + dbitem.SumberInformasiId + ", " + dbitem.LaporanKejadian + ", " + dbitem.HasilPemeriksaan + ", " + dbitem.Penyelesaian + ", " +
                        dbitem.Kesalahan + ", " + dbitem.KesalahanLain + ", " + dbitem.SalesOrderId + ", " + dbitem.TotalPengajuanClaim + ", " + dbitem.NilaiDisetujui + ", " + dbitem.AsuransiFlag + ", " + dbitem.Asuransi +
                        ", " + dbitem.BebanClaim + ", " + dbitem.BebanClaimDriverPercentage + ", " + dbitem.BebanClaimDriver + ", " + dbitem.BebanClaimKantorPercentage + ", " + dbitem.BebanClaimKantor + ", " + dbitem.Keterangan +
                        ", " + dbitem.LastUpdate + ", " + dbitem.SalesOrderKontrakId + ", " + dbitem.IdBap + ", " + dbitem.NoBap + ", " + dbitem.IsClaim + ", " + dbitem.Code + ");";
                    var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Klaim", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                    context.Auditrail.Add(auditrail);
                }
                else //edit
                {
                    context.Klaim.Attach(dbitem);
                    var query = "UPDATE dbo.\"Klaim\" SET \"NoKlaim\" = " + dbitem.NoKlaim + ", \"TanggalPengajuan\" = " + dbitem.TanggalPengajuan + ", \"StatusKlaim\" = " + dbitem.StatusKlaim + ", \"SumberInformasiId\" = " +
                        dbitem.SumberInformasiId + ", \"LaporanKejadian\" = " + dbitem.LaporanKejadian + ", \"HasilPemeriksaan\" = " + dbitem.HasilPemeriksaan + ", \"Penyelesaian\" = " + dbitem.Penyelesaian +
                        ", \"Kesalahan\" = " + dbitem.Kesalahan + ", \"KesalahanLain\" = " + dbitem.KesalahanLain + ", \"SalesOrderId\" = " + dbitem.SalesOrderId + ", \"TotalPengajuanClaim\" = " + dbitem.TotalPengajuanClaim +
                        ", \"NilaiDisetujui\" = " + dbitem.NilaiDisetujui + ", \"AsuransiFlag\" = " + dbitem.AsuransiFlag + ", \"Asuransi\" = " + dbitem.Asuransi + ", \"BebanClaim\" = " + dbitem.BebanClaim +
                        ", \"BebanClaimDriverPercentage\" = " + dbitem.BebanClaimDriverPercentage + ", \"BebanClaimDriver\" = " + dbitem.BebanClaimDriver + ", \"BebanClaimKantorPercentage\" = " +
                        dbitem.BebanClaimKantorPercentage + ", \"BebanClaimKantor\" = " + dbitem.BebanClaimKantor + ", \"Keterangan\" = " + dbitem.Keterangan + ", \"LastUpdate\" = " + dbitem.LastUpdate +
                        ", \"SalesOrderKontrakId\" = " + dbitem.SalesOrderKontrakId + ", \"IdBap\" = " + dbitem.IdBap + ", \"NoBap\" = " + dbitem.NoBap + ", \"IsClaim\" = " + dbitem.IsClaim + ", \"Code\" = " + dbitem.Code +
                        " WHERE \"Id\" = " + dbitem.Id + ";";
                    var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Klaim", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id };
                    context.Auditrail.Add(auditrail);
                    var entry = context.Entry(dbitem);
                    entry.State = EntityState.Modified;
                }
                context.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
        public List<Klaim> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            IQueryable<Klaim> list = context.Klaim;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Klaim>(filters, ref list);
            }

            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    
                    list = list.OrderBy<Klaim>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
            {
                list = list.OrderByDescending(d => d.Id); //default, wajib ada atau EF error
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
            List<Klaim> result = takeList.ToList();
            return result;
        }
        public int Count(FilterInfo filters = null)
        {
            IQueryable<Klaim> items = context.Klaim;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<Klaim>(filters, ref items);
            }

            return items.Count();
        }
        public Klaim FindByPK(int id)
        {
            return context.Klaim.Where(d => d.Id == id).FirstOrDefault();
        }
        public Klaim FindByNoKlaim(string noKlaim)
        {
            return context.Klaim.Where(d => d.NoKlaim == noKlaim).FirstOrDefault();
        }
        public Klaim FindBySoId(string soNumber)
        {
            return context.Klaim.FirstOrDefault();
        }
        public void delete(Klaim dbitem, int id)
        {
            context.Klaim.Remove(dbitem);

            context.SaveChanges();

        }
        public bool IsExist(string nama, int id = 0)
        {
            if (id == 0)
            { return context.Klaim.Any(p => p.NoKlaim.Contains(nama)); }
            else
            { return context.Klaim.Any(p => p.NoKlaim.Contains(nama) && p.Id != id); }
        }

        public string GenerateCode(DateTime valdate, int urutan)
        {
            return "KL-" + valdate.ToString("yyMMdd") + '-' + (urutan).ToString().PadLeft(2, '0');
        }

        public int getUrutanOnCAll(DateTime valdate)
        {
            try
            {
            DateTime firstDayOfMonth = valdate.AddDays(-1);
            DateTime lastDayOfMonth = valdate.AddDays(1);
            List<Context.Klaim> dbKlaim = context.Klaim.Where(d => d.TanggalPengajuan > firstDayOfMonth && d.TanggalPengajuan < lastDayOfMonth).ToList();
                return int.Parse(dbKlaim.Select(e => e.NoKlaim).Max().Split('-').Last());
            }
            catch (Exception) { }
            return 0;
        }
    }
}