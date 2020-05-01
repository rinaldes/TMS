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
    public class SalesOrderKontrakListSoRepo : ISalesOrderKontrakListSoRepo
    {
        private ContextModel context = new ContextModel();
        public void save(SalesOrderKontrakListSo dbitem)
        {
            if (dbitem.Status == "draft")
            {
                dbitem.StatusFlow = "MARKETING";
            }
            else if (dbitem.Status.ToLower().Contains("save konfirmasi")){
                dbitem.StatusFlow = "ADMIN UANG JALAN";
            }
            else if (dbitem.Status.ToLower().Contains("admin uang jalan")){
                dbitem.StatusFlow = "KASIR";
            }
            else if (dbitem.Status.ToLower().Contains("konfirmasi") || dbitem.Status == "save planning")
            {
                dbitem.StatusFlow = "KONFIRMASI";
            }
            else if (dbitem.Status.ToLower().Contains("planning") || dbitem.Status == "save")
            {
                dbitem.StatusFlow = "PLANNING";
            }
            else if (dbitem.Status.ToLower().Contains("dispatched")) 
            {
                if (dbitem.AdminUangJalan != null)
                {
                    foreach (Context.AdminUangJalanUangTf aujtf in dbitem.AdminUangJalan.AdminUangJalanUangTf.Where(n => n.isTf == true).OrderByDescending(x => x.TanggalAktual))
                    {
                        dbitem.StatusFlow = aujtf.Keterangan == "Tunai" ? "CASH" : "TRANSFER";
                        break;
                    }
                    if (dbitem.StatusFlow == null)
                        dbitem.StatusFlow = "DISPATCHED";
                }
                else
                {
                    dbitem.StatusFlow = "CASH";
                }
            }
            else{
                dbitem.StatusFlow = dbitem.Status.ToUpper();
            }
            if (dbitem.Id == 0) //create
            {
                context.SalesOrderKontrakListSo.Add(dbitem);
                var auditrail = new Auditrail { Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Sales Order Kontrak List", QueryDetail = "Add " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                if (dbitem.Status == "dispatched" && dbitem.DokumenId == null)
                {
                    string listIdSo = dbitem.Id.ToString();
                    Context.Dokumen dokumen = context.Dokumen.Where(d => d.ListIdSo == listIdSo).FirstOrDefault();
                    if (dokumen != null)
                        dbitem.DokumenId = dokumen.Id;
                }
                context.SalesOrderKontrakListSo.Attach(dbitem);
                var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Sales Order Kontrak List", QueryDetail = "Edit " + dbitem.Id, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void saveParent(SalesOrderKontrakTruck dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.SalesOrderKontrakTruck.Add(dbitem);
            }
            else //edit
            {
                context.SalesOrderKontrakTruck.Attach(dbitem);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        
        public SalesOrderKontrakListSo FindByPK(int id)
        {
            return context.SalesOrderKontrakListSo.Where(d => d.Id == id).FirstOrDefault();
        }
        public SalesOrderKontrakListSo FindByNoSo(string noso)
        {
            return context.SalesOrderKontrakListSo.Where(d => d.NoSo == noso).FirstOrDefault();
        }

        public List<SalesOrderKontrakListSo> AUJReport(int aujId)
        {
            return context.SalesOrderKontrakListSo.Where(p => p.IdAdminUangJalan == aujId && p.IsProses && (p.Status == "dispatched" || p.Status == "admin uang jalan")).ToList();
        }

        public void OnlyAdd(SalesOrderKontrakListSo dbitem)
        {
            context.SalesOrderKontrakListSo.Add(dbitem);
            context.SaveChanges();
        }
        public List<Context.SalesOrderKontrakListSo> FindAllByAUJ(int aujId)
        {
            return context.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan == aujId).ToList();
        }
        public void OnlyUpdate(SalesOrderKontrakListSo dbitem)
        {
            context.SalesOrderKontrakListSo.Attach(dbitem);
            var entry = context.Entry(dbitem);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }
        public List<Context.SalesOrderKontrakListSo> FindAllByUrutan(int Urutan)
        {
            return context.SalesOrderKontrakListSo.Where(d => d.Urutan == Urutan).ToList();
        }

        public void deleteAdd(int? id)
        {
            Context.SalesOrderKontrak dbitem = context.SalesOrderKontrak.Where(d => d.SalesOrderKontrakId == id).FirstOrDefault();
            
            if (dbitem.SalesOrderKontrakListSo != null)
            {
               context.SalesOrderKontrakListSo.RemoveRange(dbitem.SalesOrderKontrakListSo);
            }
            context.SaveChanges();
        }

        public void returnListSo(int? id)
        {
            List<SalesOrderKontrakListSo> dbitem = context.SalesOrderKontrakListSo.Where(d => d.SalesKontrakId == id).ToList();

            foreach (var ListSo in dbitem)
            {
                ListSo.IdDataTruck = null;
                ListSo.Driver1Id = null;
                ListSo.Driver2Id = null;
                ListSo.Urutan = 0;
            }
            context.SaveChanges();
        }

        public List<SalesOrderKontrakListSo> returnListSoBatalTruckOnly()
        {
            return context.SalesOrderKontrakListSo.Where(d => d.IsBatalTruck == true && d.Status == "draft planning").ToList();
        }

        public List<SalesOrderKontrakListSo> FindAll()
        {
            return context.SalesOrderKontrakListSo.Where(d => d.Status == "dispatched").ToList();
        }

        public List<SalesOrderKontrakListSo> FindAllKontrak()
        {
            return context.SalesOrderKontrakListSo.ToList();
        }

        public List<SalesOrderKontrakListSo> FindAllDispatched()
        {
            return context.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan != null && d.IsProses && (d.Status == "dispatched" || d.Status == "admin uang jalan") && d.AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan != "Tunai" && s.Value > 0 && !s.isTf && s.AdminUangJalan.Status != "Batal").Count() > 0).ToList();
        }

        public List<SalesOrderKontrakListSo> FindAllKasDispatched()
        {
            return context.SalesOrderKontrakListSo.Where(d => d.IdAdminUangJalan != null && d.IsProses && (d.Status == "dispatched" || d.Status == "admin uang jalan") && d.AdminUangJalan.AdminUangJalanUangTf.Where(s => s.Keterangan == "Tunai" && s.Value > 0 && !s.isTf && s.AdminUangJalan.Status != "Batal").Count() > 0).ToList();
        }

        public List<SalesOrderKontrakListSo> FindAllPerKontrak(List<int> ListIdDumy)
        {
            return context.SalesOrderKontrakListSo.Where(d => ListIdDumy.Contains(d.Id)).OrderBy<SalesOrderKontrakListSo>("NoSo").ToList();
        }

        public string generateCodeListSo(string NoKontrak, DateTime valdate, int rit, int urutan, int urutanInduk)
        {
            return "K-" + valdate.ToString("yyMM") + '-' + (urutanInduk).ToString().PadLeft(4, '0') + '-' + valdate.ToString("dd") + '.' + urutan.ToString();
        }
        public int getUrutanProses(int? id)
        {
            List<SalesOrderKontrakListSo> dbkontrakListSo = context.SalesOrderKontrakListSo.Where(d => d.SalesKontrakId == id).ToList();
            return dbkontrakListSo.Max(d => d.Urutan);
        }
        public int CountKontrakListSo(FilterInfo filters = null)
        {
            IQueryable<SalesOrderKontrakListSo> items = context.SalesOrderKontrakListSo;

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                GridHelper.ProcessFilters<SalesOrderKontrakListSo>(filters, ref items);
            }

            return items.Count();
        }

        public Context.SalesOrderKontrakListSo FindSONextMonth(Context.SalesOrderKontrakListSo  sokontrak)
        {
            return context.SalesOrderKontrakListSo.Where(d => d.Driver1Id == sokontrak.Driver1Id && d.MuatDate.Month == sokontrak.MuatDate.Month+1 && d.MuatDate.Day == 1).FirstOrDefault();
        }

        public void delete(SalesOrderKontrakListSo dbitem, int id)
        {
            context.SalesOrderKontrakListSo.Remove(dbitem);
            var query = "DELETE FROM dbo.\"SalesOrderKontrakListSo\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail
            {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Planning Kontrak", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}