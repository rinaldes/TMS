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
    public class AuditrailRepo : IAuditrailRepo
    {
        private ContextModel context = new ContextModel();
        public void saveOrderHistory(Context.SalesOrder so){
            if (so != null)
            {
                var order_history = new OrderHistory();
                if (so.Status == "draft") //hanya simpan
                    order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = DateTime.Now, PIC = so.UpdatedBy };
                else
                {//proses ke planning
                    var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 1).FirstOrDefault();
                    if (last_oh == null)
                        order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy };
                    else
                        order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy };
                }
                context.OrderHistory.Add(order_history);
                Context.SalesOrderOncall dboncall = so.SalesOrderOncall;
                if (dboncall != null)
                {
                    var query = "INSERT INTO dbo.\"SalesOrderOncall\" (\"SONumber\", \"Urutan\", \"TanggalOrder\", \"JamOrder\", \"CustomerId\", \"PrioritasId\", \"JenisTruckId\", \"ProductId\", " +
                        "\"TanggalMuat\", \"JamMuat\", \"Keterangan\", \"KeteranganLoading\", \"KeteranganUnloading\", \"IdDaftarHargaItem\", \"StrDaftarHargaItem\", \"StrMultidrop\", \"IdDataTruck\", \"Driver1Id\", " +
                        "\"KeteranganDriver1\", \"Driver2Id\", \"KeteranganDriver2\", \"IsCash\", \"KeteranganRek\", \"IdDriverTitip\", \"DN\", \"KeteranganDataTruck\", \"AtmId\") VALUES (" + dboncall.SONumber + ", "
                        + dboncall.Urutan + ", " + dboncall.TanggalOrder + ", " + dboncall.JamOrder + ", " + dboncall.CustomerId + ", " + dboncall.PrioritasId + ", " + dboncall.JenisTruckId + ", " + dboncall.ProductId +
                        ", " + dboncall.TanggalMuat + ", " + dboncall.JamMuat + ", " + dboncall.Keterangan + ", " + dboncall.KeteranganLoading + ", " + dboncall.KeteranganUnloading + ", " + dboncall.IdDaftarHargaItem +
                        "," + dboncall.StrDaftarHargaItem + ", " + dboncall.StrMultidrop + ", " + dboncall.IdDataTruck + ", " + dboncall.Driver1Id + ", " + dboncall.KeteranganDriver1 + ", " + dboncall.Driver2Id + ", " +
                        dboncall.KeteranganDriver2 + ", " + dboncall.IsCash + ", " + dboncall.KeteranganRek + ", " + dboncall.IdDriverTitip + ", " + dboncall.DN + ", " + dboncall.KeteranganDataTruck + ", " +
                        dboncall.AtmId + ");";
                    foreach (Context.SalesOrderOnCallUnLoadingAdd soocula in dboncall.SalesOrderOnCallUnLoadingAdd)
                    {
                        query += "INSERT INTO dbo.\"SalesOrderOnCallUnLoadingAdd\" (\"SalesOrderOnCallId\", \"CustomerUnloadingAddressId\", \"CustomerId\", urutan, \"IsSelect\") VALUES (" + soocula.SalesOrderOnCallId + ", " +
                            soocula.CustomerUnloadingAddressId + ", " + soocula.CustomerId + ", " + soocula.urutan + ", " + soocula.IsSelect + ");";
                    }
                    var auditrail = new Auditrail
                    {
                        Actionnya = "Add",
                        EventDate = DateTime.Now,
                        Modulenya = "Sales Order On Call",
                        QueryDetail = query,
                        RemoteAddress = AppHelper.GetIPAddress(),
                        IdUser = so.UpdatedBy
                    };
                    context.Auditrail.Add(auditrail);
                }
                context.SaveChanges();
            }
        }
        public void saveInapHistory(Context.SolarInap si, int userId)
        {
            if (si != null)
            {
                var order_history = new SolarInapHistory();
                //proses ke marketing
                var last_oh = context.SolarInapHistory.Where(d => d.SolarInapId == si.Id && d.StatusFlow == 1).FirstOrDefault();
                if (last_oh == null)
                    order_history = new SolarInapHistory { SolarInapId = si.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = DateTime.Now, ProcessedAt = DateTime.Now, PIC = userId };
                else
                    order_history = new SolarInapHistory { SolarInapId = si.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = userId };
                var so_history = new OrderHistory { SalesOrderId = si.IdSO, StatusFlow = 18, FlowDate = DateTime.Now, SavedAt = DateTime.Now, ProcessedAt = DateTime.Now, PIC = userId };
                context.OrderHistory.Add(so_history);
                context.SolarInapHistory.Add(order_history);
                context.SaveChanges();
            }
        }

        public OrderHistory getKasirHistoryUnder1Minutes(Context.SalesOrder so, DateTime ProcessedAt)
        {
            return context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.ProcessedAt > ProcessedAt).FirstOrDefault();
        }

        public OrderHistory getAUJKontrakHistoryUnder1Minutes(Context.SalesOrder so, DateTime ProcessedAt, int userId)
        {
            return context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.ProcessedAt > ProcessedAt && d.PIC == userId && d.StatusFlow == 4).FirstOrDefault();
        }

        public OrderHistory getHistoryUnderSpecificUser(Context.SalesOrder so, DateTime ProcessedAt, int userId)
        {
            return context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.ProcessedAt > ProcessedAt && d.PIC == userId).FirstOrDefault();
        }

        public void savePlanningHistory(Context.SalesOrder so, string listSo = null)
        {
            var order_history = new OrderHistory();
            if (so.Status == "draft planning") //hanya simpan
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 2, FlowDate = DateTime.Now, SavedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            else{//proses ke konfirmasi
                var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 2 ).FirstOrDefault();
                if (last_oh == null)
                    order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 2, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
                else
                    order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 2, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            } 
            context.OrderHistory.Add(order_history);
            Context.SalesOrderOncall dboncall = so.SalesOrderOncall;
            if (dboncall != null)
            {
                var query = "UPDATE dbo.\"SalesOrderOncall\" SET \"IdDataTruck\" = " + so.SalesOrderOncall.IdDataTruck + ", \"KeteranganDataTruck\" = " + so.SalesOrderOncall.KeteranganDataTruck + ", \"TanggalOrder\" = \" = " +
                    so.SalesOrderOncall.TanggalOrder + ", \"Driver1Id\" = " + so.SalesOrderOncall.Driver1Id + ", \"Driver2Id\" = " + so.SalesOrderOncall.Driver2Id + ", \"KeteranganDriver1\" = " +
                    so.SalesOrderOncall.KeteranganDriver1 + ", \"KeteranganDriver2\" = " + so.SalesOrderOncall.KeteranganDriver2 + ", \"IsCash\" = " + so.SalesOrderOncall.IsCash + ", \"IdDriverTitip\" = "
                    + so.SalesOrderOncall.IdDriverTitip + ", \"AtmId\" = " + so.SalesOrderOncall.AtmId + ", \"KeteranganRek\" = " + so.SalesOrderOncall.KeteranganRek + " WHERE \"SalesOrderOnCallId\" = " + so.SalesOrderOncallId +
                    ";";
                var auditrail = new Auditrail
                {
                    Actionnya = "Submit",
                    EventDate = DateTime.Now,
                    Modulenya = "Planning",
                    QueryDetail = query,
                    RemoteAddress = AppHelper.GetIPAddress(),
                    IdUser = so.UpdatedBy
                };
                context.Auditrail.Add(auditrail);
            }
            context.SaveChanges();
        }

        public void saveKonfirmasiHistory(Context.SalesOrder so, string ListSo=null){
            var order_history = new OrderHistory();
            if (so.Status == "draft konfirmasi" && ListSo==null) //hanya simpan
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 3, FlowDate = DateTime.Now, SavedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = ListSo };
            else{//proses ke konfirmasi
                var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 3 ).FirstOrDefault();
                if (last_oh == null || ListSo != null)
                    order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 3, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = ListSo };
                else
                    order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 3, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = ListSo };
            }
            context.OrderHistory.Add(order_history);
            var query = "";
            if (so.SalesOrderOncallId.HasValue)
            {
                query = "UPDATE dbo.\"SalesOrderOncall\" SET \"IdDataTruck\" = " + so.SalesOrderOncall.IdDataTruck + ", \"KeteranganDataTruck\" = " + so.SalesOrderOncall.KeteranganDataTruck + ", \"TanggalOrder\" = \" = " +
                    so.SalesOrderOncall.TanggalOrder + ", \"Driver1Id\" = " + so.SalesOrderOncall.Driver1Id + ", \"Driver2Id\" = " + so.SalesOrderOncall.Driver2Id + ", \"KeteranganDriver1\" = " +
                    so.SalesOrderOncall.KeteranganDriver1 + ", \"KeteranganDriver2\" = " + so.SalesOrderOncall.KeteranganDriver2 + ", \"IsCash\" = " + so.SalesOrderOncall.IsCash + ", \"IdDriverTitip\" = "
                    + so.SalesOrderOncall.IdDriverTitip + ", \"AtmId\" = " + so.SalesOrderOncall.AtmId + ", \"KeteranganRek\" = " + so.SalesOrderOncall.KeteranganRek + " WHERE \"SalesOrderOnCallId\" = " + so.SalesOrderOncallId +
                    ";";
            }
            else if (so.SalesOrderPickupId.HasValue)
            {
                query = "UPDATE dbo.\"SalesOrderPickup\" SET \"IdDataTruck\" = " + so.SalesOrderPickup.IdDataTruck + ", \"KeteranganDataTruck\" = " + so.SalesOrderPickup.KeteranganDataTruck + ", \"TanggalOrder\" = \" = " +
                    so.SalesOrderPickup.TanggalOrder + ", \"Driver1Id\" = " + so.SalesOrderPickup.Driver1Id + ", \"Driver2Id\" = " + so.SalesOrderPickup.Driver2Id + ", \"KeteranganDriver1\" = " +
                    so.SalesOrderPickup.KeteranganDriver1 + ", \"KeteranganDriver2\" = " + so.SalesOrderPickup.KeteranganDriver2 + ", \"IsCash\" = " + so.SalesOrderPickup.IsCash + ", \"IdDriverTitip\" = "
                    + so.SalesOrderPickup.IdDriverTitip + ", \"AtmId\" = " + so.SalesOrderPickup.AtmId + ", \"KeteranganRek\" = " + so.SalesOrderPickup.KeteranganRek + " WHERE \"SalesOrderOnPickupId\" = " + so.SalesOrderPickupId +
                    ";";
            }
            var auditrail = new Auditrail {
                Actionnya="Submit", EventDate=DateTime.Now, Modulenya = "Konfirmasi", QueryDetail=query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = so.UpdatedBy
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveAUJHistory(Context.SalesOrder so, Context.HistoryJalanTruck hjt, Context.AdminUangJalan auj = null, String listSo=null){
            var order_history = new OrderHistory();
            var dbauj = so.AdminUangJalan;
            if (dbauj == null)
                dbauj = auj;
            var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 4).FirstOrDefault();
            if (last_oh == null)
                order_history = new OrderHistory {SalesOrderId = so.Id, StatusFlow = 4, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo=listSo};
            else
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 4, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            context.OrderHistory.Add(order_history);

            //AuditTrail Beneran
            var query = "INSERT INTO dbo.\"AdminUangJalan\" (\"Id\", \"KeteranganAdmin\", \"IdDriverOld1\", \"IdDriverOld2\", \"IdDriver1\", \"KeteranganGanti1\", \"IdDriver2\", \"KeteranganGanti2\",";
            query += "\"TotalKasbon\", \"TotalKlaim\", \"CreatedDate\", \"CreatedBy\", \"NilaiBorongan\", \"Kawalan\", \"Timbangan\", \"Karantina\", \"SPSI\", \"TotalBorongan\", \"KasbonDriver1\",";
            query += "\"KasbonDriver2\", \"KlaimDriver1\", \"KlaimDriver2\", \"TotalPotonganDriver\", \"Status\", \"Multidrop\", \"IdDataBorongan\", \"TotalAlokasi\", \"Code\") VALUES (" +
                dbauj.Id.ToString() + ", " + (dbauj.KeteranganAdmin == null ? "NULL" : dbauj.KeteranganAdmin) + ", " + (dbauj.IdDriverOld1 == null ? "NULL" : dbauj.IdDriverOld1.ToString()) + ", " + 
                (dbauj.IdDriverOld2 == null ? "NULL" : dbauj.IdDriverOld2.ToString()) + ", " + (dbauj.IdDriver1 == null ? "NULL" : dbauj.IdDriver1.ToString()) + ", " + 
                (dbauj.KeteranganGanti1 == null ? "NULL" : dbauj.KeteranganGanti1) + ", " + (dbauj.IdDriver2 == null ? "NULL" : dbauj.IdDriver2.ToString()) + ", " +
                (dbauj.KeteranganGanti2 == null ? "NULL" : dbauj.KeteranganGanti2) + ", " + (dbauj.TotalKasbon == null ? "NULL" : dbauj.TotalKasbon.ToString()) + ", " + 
                (dbauj.TotalKlaim == null ? "NULL" : dbauj.TotalKlaim.ToString()) + ", " + (dbauj.CreatedDate == null ? "NULL" : dbauj.CreatedDate.ToString()) + ", " + 
                (dbauj.CreatedBy == null ? "NULL" : dbauj.CreatedBy.ToString()) + ", " + (dbauj.NilaiBorongan == null ? "NULL" : dbauj.NilaiBorongan.ToString()) + ", " + 
                (dbauj.Kawalan == null ? "NULL" : dbauj.Kawalan.ToString()) + ", " + dbauj.Timbangan + ", " + dbauj.Karantina + ", " + dbauj.SPSI + ", " + dbauj.TotalBorongan + ", " +
                (dbauj.KasbonDriver1 == null ? "NULL" : dbauj.KasbonDriver1.ToString()) + ", " + (dbauj.KasbonDriver2 == null ? "NULL" : dbauj.KasbonDriver2.ToString()) + ", " +
                (dbauj.KlaimDriver1 == null ? "NULL" : dbauj.KlaimDriver1.ToString()) + ", " + (dbauj.KlaimDriver2 == null ? "NULL" : dbauj.KlaimDriver2.ToString()) + ", " + dbauj.TotalPotonganDriver +
                ", " + (dbauj.Status == null ? "NULL" : dbauj.Status) + ", " + (dbauj.Multidrop == null ? "NULL" : dbauj.Multidrop.ToString()) + ", " + 
                (dbauj.IdDataBorongan == null ? "NULL" : dbauj.IdDataBorongan.ToString()) + ", " + (dbauj.TotalAlokasi == null ? "NULL" : dbauj.TotalAlokasi.ToString()) + ", '" + dbauj.Code + "');";

            foreach (AdminUangJalanPotonganDriver pd in dbauj.AdminUangJalanPotonganDriver){
                query += "INSERT INTO dbo.\"AdminUangJalanPotonganDriver\" (\"Id\", \"IdAdminUangJalan\", \"Keterangan\", \"TypeDriver\", \"Value\") VALUES (" + pd.Id + ", " + dbauj.Id + ", '" + 
                pd.Keterangan + "', " + pd.TypeDriver + ", " + pd.Value + ");";
            };
            foreach (AdminUangJalanTambahanLain pd in dbauj.AdminUangJalanTambahanLain){
                query += "INSERT INTO dbo.\"AdminUangJalanTambahanLain\" (\"Id\", \"IdAdminUangJalan\", \"Keterangan\", \"Values\") VALUES (" + pd.Id + ", " + dbauj.Id + ", '" + pd.Keterangan + "', " + 
                pd.Values + ");";
            };
            foreach (AdminUangJalanTambahanRute pd in dbauj.AdminUangJalanTambahanRute){
                query += "INSERT INTO dbo.\"AdminUangJalanTambahanRute\" (\"Id\", \"IdAdminUangJalan\", \"IdDataBorongan\", \"values\") VALUES (" + pd.Id + ", " + dbauj.Id + ", " + pd.IdDataBorongan +
                ", " + pd.values + ");";
            };
            foreach (AdminUangJalanUangTf pd in dbauj.AdminUangJalanUangTf){
                query += "INSERT INTO dbo.\"AdminUangJalanUangTf\" (\"Id\", \"IdAdminUangJalan\", \"Keterangan\", \"Value\", \"Tanggal\", \"JumlahTransfer\", \"idRekenings\", \"TanggalAktual\",";
                query += " \"JamAktual\", \"KeteranganTf\", \"IdDriverPenerima\", \"isTf\", \"Code\", \"IdCreditTf\", \"Urutan\") VALUES (" + pd.Id + ", " + dbauj.Id + ", '" + pd.Keterangan + "', " +
                pd.Value + ", '" + pd.Tanggal + "', " + (pd.JumlahTransfer == null ? "NULL" : pd.JumlahTransfer.ToString()) + ", " +
                (pd.idRekenings == null ? "NULL" : pd.idRekenings.ToString()) + ", '" + pd.TanggalAktual + "', '" + pd.JamAktual + "', '" + pd.KeteranganTf + "', " +
                (pd.IdDriverPenerima == null ? "NULL" : pd.IdDriverPenerima.ToString()) + ", " +  (pd.isTf == false ? "NULL" : pd.isTf.ToString()) + ", " + (pd.Code == null ? "NULL" : pd.Code.ToString()) +
                ", " + (pd.IdCreditTf == null ? "NULL" : pd.IdCreditTf.ToString()) + ", " + (pd.Urutan == null ? "NULL" : pd.Urutan.ToString()) + ");";
            };

            foreach (AdminUangJalanVoucherKapal pd in dbauj.AdminUangJalanVoucherKapal){
                query += "INSERT INTO dbo.\"AdminUangJalanVoucherKapal\" (\"Id\", \"IdAdminUangJalan\", \"Keterangan\", \"Value\" ("+pd.Id+", " + dbauj.Id + ", '" + pd.Keterangan + "', " + pd.Value + ");";
            };

            foreach (AdminUangJalanVoucherSpbu pd in dbauj.AdminUangJalanVoucherSpbu){
                query += "INSERT INTO dbo.\"AdminUangJalanVoucherSpbu\" (\"Id\", \"IdAdminUangJalan\", \"Keterangan\", \"Value\" ("+pd.Id + ", "+ dbauj.Id + ", '" + pd.Keterangan + "', " + pd.Value + ");";
            };
            if (hjt != null){
                query += "INSERT INTO dbo.\"HistoryJalanTruck\" (\"IdAdminUangJalan\", \"IdDriver1\", \"IdDriver2\", \"IdTruck\", \"IdCustomer\", \"ShipmentId\", \"NoSo\", \"TanggalMuat\", \"JenisOrder\", \"Rute\") VALUES (" +
                    hjt.IdAdminUangJalan + ", " + hjt.IdDriver1 + ", " + hjt.IdDriver2 + ", " + hjt.IdTruck + ", " + hjt.IdCustomer + ", " + hjt.ShipmentId + ", " + hjt.NoSo + ", " + hjt.TanggalMuat + ", " + hjt.JenisOrder +
                    ", " + hjt.Rute + ");";
            }

            var auditrail = new Auditrail {
                Actionnya="Submit", EventDate=DateTime.Now, Modulenya = "Admin Uang Jalan", QueryDetail=query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = so.UpdatedBy
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveKasirTfHistory(Context.SalesOrder so, string strQuery, String listSo = null)
        {
            var order_history = new OrderHistory();
            var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 5).FirstOrDefault();
            if (last_oh == null)
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 5, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            else
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 5, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            context.OrderHistory.Add(order_history);
            var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Kasir Transfer", QueryDetail = strQuery, RemoteAddress = AppHelper.GetIPAddress(), IdUser = so.UpdatedBy };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveMajuHistory(Context.SalesOrder so, string strQuery)
        {
            var order_history = new OrderHistory();
            var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 10).FirstOrDefault();
            if (last_oh == null)
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 10, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy };
            else
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 10, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy };
            context.OrderHistory.Add(order_history);
            var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Maju 1 Flow", QueryDetail = strQuery, RemoteAddress = AppHelper.GetIPAddress(), IdUser = so.UpdatedBy };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveKasirKasHistory(Context.SalesOrder so, string strQuery, String listSo = null)
        {
            var order_history = new OrderHistory();
            var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 6).FirstOrDefault();
            if (last_oh == null)
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 6, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            else
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 6, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            context.OrderHistory.Add(order_history);
            var auditrail = new Auditrail { Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Kasir Kas", QueryDetail = strQuery, RemoteAddress = AppHelper.GetIPAddress(), IdUser = so.UpdatedBy };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveSettRegHistory(Context.SalesOrder so, String listSo = null)
        {
            var order_history = new OrderHistory();
            var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 7).FirstOrDefault();
            if (last_oh == null)
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 7, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            else
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 7, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            context.OrderHistory.Add(order_history);
            context.SaveChanges();
        }

        public void saveBOHistory(Context.SalesOrder so){
            var order_history = new OrderHistory();
            var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 8).FirstOrDefault();
            if (last_oh == null)
                order_history = new OrderHistory {SalesOrderId = so.Id, StatusFlow = 8, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy};
            else
                order_history = new OrderHistory {SalesOrderId = so.Id, StatusFlow = 8, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy};
            context.OrderHistory.Add(order_history);
            context.SaveChanges();
        }

        public void saveBTHistory(Context.SalesOrder so, String listSo = null)
        {
            var order_history = new OrderHistory();
            var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 2).FirstOrDefault();
            if (last_oh == null)
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 2, FlowDate = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            else
                order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 2, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy, ListSo = listSo };
            context.OrderHistory.Add(order_history);
            context.SaveChanges();
        }

        public void save(Auditrail dbitem)
        {
            if (dbitem.Id == 0) //create
            {
                context.Auditrail.Add(dbitem);
            }
            else //edit
            {
                context.Auditrail.Attach(dbitem);

                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public List<Auditrail> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<Auditrail> list = context.Auditrail.Where(d => d.EventDate > date2018);
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
                GridHelper.ProcessFilters<Auditrail>(filters, ref list);
            int count = list.Count();
            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    list = list.OrderBy<Auditrail>(s.SortOn + " " + s.SortOrder);
                }
            }
            else
                list = list.OrderBy<Auditrail>("\"EventDate\" DESC"); //default, wajib ada atau EF error
            var takeList = list;
            if (skip != null)
                takeList = takeList.Skip(skip.Value);
            if (take != null)
                takeList = takeList.Take(take.Value);
            return takeList.ToList();
        }

        public String FindAllNativeQuery(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null)
        {
            String query = "SELECT * FROM dbo.\"Auditrail\"";

            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
            {
                List<String> conditions = new List<String>();
                foreach (var filter in filters.Filters)
                {
                    //list = list.OrderBy<Auditrail>(s.SortOn + " " + s.SortOrder);
                    conditions.Add("\"" + filter.Field + "\" LIKE '%" + filter.Value + "%'");
                }
                query += " WHERE " + String.Join(" AND ", conditions);
            }
            if (sortings != null && sortings.Count > 0)
            {
                foreach (var s in sortings)
                {
                    query += " ORDER BY \"" + s.SortOn + "\" " + s.SortOrder;
                }
            }
            else
                query += " ORDER BY \"EventDate\" DESC";
            return query + " LIMIT 10";
        }

        public int Count(FilterInfo filters = null)
        {
            DateTime date2018 = DateTime.Parse("2017-12-31");
            IQueryable<Auditrail> items = context.Auditrail.Where(d => d.EventDate > date2018);
            if (filters != null && (filters.Filters != null && filters.Filters.Count > 0))
                GridHelper.ProcessFilters<Auditrail>(filters, ref items);
            return items.Count();
        }
        public Auditrail FindByPK(int id)
        {
            return context.Auditrail.Where(d => d.Id == id).FirstOrDefault();
        }
        public void delete(Auditrail dbitem)
        {
            context.Auditrail.Remove(dbitem);

            context.SaveChanges();
        }

        public void saveCustomerQuery(Customer dbitem, int id, string add=null){
            if (add != null) //create
            {
                var query = "INSERT INTO dbo.\"Customer\" (\"CustomerCode\", \"CustomerCodeOld\", \"CustomerNama\", \"PrioritasId\", \"WajibPO\", \"WajibGPS\", \"CustomerPicId\", " +
                    "\"SpecialTreatment\", \"Keterangan\", urutan, sent_to_erp) VALUES ( " + dbitem.Id + ", " + dbitem.CustomerCode + ", " + dbitem.CustomerCodeOld + ", " + dbitem.CustomerNama + ", " + 
                    dbitem.PrioritasId + ", " + dbitem.WajibPO + ", " + dbitem.WajibGPS + ", " + dbitem.CustomerPicId + ", " + dbitem.SpecialTreatment + ", " + dbitem.Keterangan + ", " + dbitem.urutan +
                    ", " + dbitem.sent_to_erp + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                var query = "UPDATE dbo.\"Customer\" SET \"CustomerCode\" = " + dbitem.CustomerCode + ", \"CustomerCodeOld\" = " + dbitem.CustomerCodeOld + ", \"CustomerNama\" = " + dbitem.CustomerNama +
                    "\"PrioritasId\" = " + dbitem.PrioritasId + ", \"WajibPO\" = " + dbitem.WajibPO + ", \"WajibGPS\" = " + dbitem.WajibGPS + ", \"CustomerPicId\" = "  + dbitem.CustomerPicId + 
                    ", \"SpecialTreatment\" = " + dbitem.SpecialTreatment + ", \"Keterangan\" = " + dbitem.Keterangan + ", urutan = " + dbitem.urutan + ", sent_to_erp = " + dbitem.sent_to_erp + 
                    " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            context.SaveChanges();
        }

        public void saveCustomerAddressQuery(CustomerAddress dbitem, int id, string add=null){
            if (add != null) //create
            {
                var query = "INSERT INTO dbo.\"CustomerAddress\" (\"CustomerId\", \"Code\", \"Alamat\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\", \"Longitude\", \"Latitude\", \"Radius\"," +
                    "\"Zona\", \"OfficeTypeId\", \"Telp\", \"Fax\", urutan) VALUES (" + dbitem.Id + ", " + dbitem.CustomerId + ", " + dbitem.Code + ", " + dbitem.Alamat + ", " + dbitem.IdProvinsi + ", " +
                    dbitem.IdKabKota + ", " + dbitem.IdKec + ", " + dbitem.IdKel + ", " + dbitem.Longitude + ", " + dbitem.Latitude + ", " + dbitem.Radius + ", " + dbitem.Zona + ", " + 
                    dbitem.OfficeTypeId + ", " + dbitem.Telp + ", " + dbitem.Fax + ", " + dbitem.urutan + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                var query = "UPDATE dbo.\"CustomerAddress\" SET \"Id\" = " + dbitem.Id + ", \"CustomerId\" = " + dbitem.CustomerId + ", \"Code\" = " + dbitem.Code + ", \"Alamat\" = " + dbitem.Alamat +
                    ", \"IdProvinsi\" = " + dbitem.IdProvinsi + "\"IdKabKota\" = " + dbitem.IdKabKota + ", \"IdKec\" = " + dbitem.IdKec + "\"IdKel\" =  " + dbitem.IdKel + ", \"Longitude\" = " +
                    dbitem.Longitude + ", \"Latitude\" = " + dbitem.Latitude + ", \"Radius\" = " + dbitem.Radius + ", \"Zona\" = " + dbitem.Zona + ", \"OfficeTypeId\" = " + dbitem.OfficeTypeId + 
                    ", \"Telp\" = " + dbitem.Telp + "\"Fax\" = " + dbitem.Fax + ", urutan = " + dbitem.urutan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            context.SaveChanges();
        }

        public void saveCustomerLoadingAddressQuery(CustomerLoadingAddress dbitem, int id, string add=null){
            if (add != null) //create
            {
                var query = "INSERT INTO dbo.\"CustomerLoadingAddress\" (\"CustomerId\", \"Code\", \"Alamat\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\", \"Longitude\", \"Latitude\"," + 
                    " \"Radius\", \"Zona\", \"Telp\", \"Fax\", urutan) VALUES (" + dbitem.Id + ", " + dbitem.CustomerId + ", " + dbitem.Code + ", " + dbitem.Alamat + ", " + dbitem.IdProvinsi + ", " + 
                    dbitem.IdKabKota + ", " + dbitem.IdKec + ", " + dbitem.IdKel + ", " + dbitem.Longitude + ", " + dbitem.Latitude + ", " + dbitem.Radius + ", " + dbitem.Zona + ", " + dbitem.Telp + ", " +
                    dbitem.Fax + ", " + dbitem.urutan + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                var query = "UPDATE dbo.\"CustomerLoadingAddress\" SET \"Alamat\" = " + dbitem.Alamat + ", \"IdProvinsi\" = " + dbitem.IdProvinsi + "\"IdKabKota\" = " + dbitem.IdKabKota +
                    ", \"IdKec\" = " + dbitem.IdKec + "\"IdKel\" =  " + dbitem.IdKel + ", \"Longitude\" = " + dbitem.Longitude + ", \"Latitude\" = " + dbitem.Latitude + ", \"Radius\" = " + dbitem.Radius +
                    ", \"Zona\" = " + dbitem.Zona + ", \"Telp\" = " + dbitem.Telp + "\"Fax\" = " + dbitem.Fax + ", urutan = " + dbitem.urutan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            context.SaveChanges();
        }

        public void saveDelCustomerAddressQuery(CustomerAddress dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerAddress\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerLoadingAddressQuery(CustomerLoadingAddress dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerLoadingAddress\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllCustomerLoadingAddressQuery(Customer dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerLoadingAddress\" WHERE \"CustomerId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerProductQuery(CustomerProductType dbitem, int id){
            if (dbitem.Id == 0) //create
            {
/*                var query = "INSERT INTO dbo.\"CustomerAddress\" (\"CustomerId\", \"Code\", \"Alamat\", \"IdProvinsi\", \"IdKabKota\", \"IdKec\", \"IdKel\", \"Longitude\", \"Latitude\", \"Radius\"," +
                    "\"Zona\", \"OfficeTypeId\", \"Telp\", \"Fax\", urutan) VALUES (" + dbitem.Id + ", " + dbitem.CustomerId + ", " + dbitem.Code + ", " + dbitem.Alamat + ", " + dbitem.IdProvinsi + ", " +
                    dbitem.IdKabKota + ", " + dbitem.IdKec + ", " + dbitem.IdKel + ", " + dbitem.Longitude + ", " + dbitem.Latitude + ", " + dbitem.Radius + ", " + dbitem.Zona + ", " + 
                    dbitem.OfficeTypeId + ", " + dbitem.Telp + ", " + dbitem.Fax + ", " + dbitem.urutan + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = 1 
                };
                context.Auditrail.Add(auditrail);*/
            }
            else //edit
            {
/*                var query = "UPDATE dbo.\"CustomerAddress\" SET \"Id\" = " + dbitem.Id + ", \"CustomerId\" = " + dbitem.CustomerId + ", \"Code\" = " + dbitem.Code + ", \"Alamat\" = " + dbitem.Alamat +
                    ", \"IdProvinsi\" = " + dbitem.IdProvinsi + "\"IdKabKota\" = " + dbitem.IdKabKota + ", \"IdKec\" = " + dbitem.IdKec + "\"IdKel\" =  " + dbitem.IdKel + ", \"Longitude\" = " +
                    dbitem.Longitude + ", \"Latitude\" = " + dbitem.Latitude + ", \"Radius\" = " + dbitem.Radius + ", \"Zona\" = " + dbitem.Zona + ", \"OfficeTypeId\" = " + dbitem.OfficeTypeId + 
                    ", \"Telp\" = " + dbitem.Telp + "\"Fax\" = " + dbitem.Fax + ", urutan = " + dbitem.urutan + " WHERE \"Id\" = " + dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
                var entry = context.Entry(dbitem);
                entry.State = EntityState.Modified;*/
            }
            context.SaveChanges();
        }

        public void saveCustomerAttachmentQuery(CustomerAttachment dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerAttachment\" (\"CustomerId\", url, filename, realfname) VALUES (" + dbitem.Id + ", " + dbitem.CustomerId + ", " + dbitem.url + ", " + 
                dbitem.filename + ", " + dbitem.realfname + ");";
            var auditrail = new Auditrail {
                Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerAttachmentQuery(CustomerAttachment dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerAttachment\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerBillingQuery(CustomerBilling dbitem, int id){
            if (dbitem.Id == 0) //create
            {
                var query = "INSERT INTO dbo.\"CustomerBilling\" (\"CustomerId\", \"DocumentName\", \"Lembar\", \"Warna\", \"Stempel\", \"IsFax\", \"Fax\", \"IsEmail\", \"Email\", \"IsTukarFaktur\", " +
                    "\"IsJasaPengiriman\", \"UrlAtt\", \"FileName\") VALUES (" + dbitem.CustomerId + ", " + dbitem.DocumentName + ", " + dbitem.Lembar + ", " + dbitem.Warna + ", " + dbitem.Stempel + ", " +
                    dbitem.IsFax + ", " + dbitem.Fax + ", " + dbitem.IsEmail + ", " + dbitem.Email + ", " + dbitem.IsTukarFaktur + ", " + dbitem.IsJasaPengiriman + ", " + dbitem.UrlAtt + ", " +
                    dbitem.FileName + ");";
                var auditrail = new Auditrail {
                    Actionnya = "Add", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            else //edit
            {
                var query = "UPDATE dbo.\"CustomerBilling\" SET \"CustomerId\" = " + dbitem.CustomerId + ", \"DocumentName\" = " + dbitem.DocumentName + ", \"Lembar\" = " + dbitem.Lembar +
                    ", \"Warna\" = " + dbitem.Warna + "\"Stempel\" = " + dbitem.Stempel + ", \"IsFax\" = " + dbitem.IsFax + "\"Fax\" =  " + dbitem.Fax + ", \"IsEmail\" = " +
                    dbitem.IsEmail + ", \"Email\" = " + dbitem.Email + ", \"IsTukarFaktur\" = " + dbitem.IsTukarFaktur + ", \"IsJasaPengiriman\" = " + dbitem.IsJasaPengiriman + ", \"UrlAtt\" = " + 
                    dbitem.UrlAtt + ", \"FileName\" = " + dbitem.FileName + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            context.SaveChanges();
        }

        public void saveDelCustomerBillingQuery(CustomerBilling dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerBilling\" WHERE \"CustomerBillingId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerJadwalBillingQuery(CustomerBilling dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerJadwalBilling\" WHERE \"CustomerBillingId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllCustomerBillingQuery(Customer dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerBilling\" WHERE \"CustomerId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya = "Delete All", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerCreditStatusQuery(CustomerCreditStatus dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerCreditStatus\" (\"CustomerId\", \"StatusSystem\", \"StatusOveride\", \"Keterangan\", \"Condition\", \"MinTOPOverdue1\", \"MaxTOPOverdue1\", " +
                "\"ValueOverdue2\", \"TOPOverdue2\", \"ShipmentDay1\", \"ShipmentDay2\") VALUES (" + dbitem.CustomerId + ", " + dbitem.StatusSystem + ", " + dbitem.StatusOveride + ", " +
                dbitem.Keterangan + ", " + dbitem.Condition + ", " + dbitem.MinTOPOverdue1 + ", " + dbitem.MaxTOPOverdue1 + ", " + dbitem.ValueOverdue2 + ", " + dbitem.TOPOverdue2 + ", " + 
                dbitem.ShipmentDay1 + ", " + dbitem.ShipmentDay2 + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Attachment", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveUpdCustomerCreditStatusQuery(CustomerCreditStatus dbitem, int id){
            var query = "UPDATE dbo.\"CustomerCreditStatus\" SET \"StatusSystem\" = " + dbitem.StatusSystem + ", \"StatusOveride\" = " + dbitem.StatusOveride + ", \"Keterangan\" = " + dbitem.Keterangan +
                "\"Condition\" = " + dbitem.Condition + ", \"MinTOPOverdue1\" = " + dbitem.MinTOPOverdue1 + ", \"MaxTOPOverdue1\" = " + dbitem.MaxTOPOverdue1 + ", \"ValueOverdue2\" = " +
                dbitem.ValueOverdue2 + ", \"TOPOverdue2\" = " + dbitem.TOPOverdue2 + ", \"ShipmentDay1\" = " + dbitem.ShipmentDay1 + ", \"ShipmentDay2\" = " + dbitem.ShipmentDay2 + " WHERE \"Id\" = " +
                dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Attachment", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerCreditStatusHistoryQuery(CustomerCreditStatusHistory dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerCreditStatus\" (\"CustomerCreditStatusId\", \"CustomerId\", \"StatusSystem\", \"StatusOveride\", \"Keterangan\", \"Condition\", \"MinTOPOverdue1\", " + 
                "\"MaxTOPOverdue1\", \"ValueOverdue2\", \"TOPOverdue2\", \"ShipmentDay1\", \"ShipmentDay2\", \"StatAwal\", \"StatAkhir\", \"Tanggal\", \"Username\") VALUES (" + 
                dbitem.CustomerCreditStatusId + ", " + dbitem.CustomerId + ", " + dbitem.StatusSystem + ", " + dbitem.StatusOveride + ", " + dbitem.Keterangan + ", " + dbitem.Condition + ", " + 
                dbitem.MinTOPOverdue1 + ", " + dbitem.MaxTOPOverdue1 + ", " + dbitem.ValueOverdue2 + ", " + dbitem.TOPOverdue2 + ", " +  dbitem.ShipmentDay1 + ", " + dbitem.ShipmentDay2 + ", " +
                dbitem.StatAwal + ", " + dbitem.StatAkhir + ", " + dbitem.Tanggal + ", " + dbitem.Username + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Attachment", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerJadwalBillingQuery(CustomerJadwalBilling dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerJadwalBilling\" (\"CustomerBillingId\", \"CustomerId\", \"Hari\", \"Jam\", \"Catatan\", \"PIC\") VALUES (" + dbitem.CustomerBillingId + ", " + 
            dbitem.CustomerId + ", " + dbitem.Hari + ", " + dbitem.Jam + ", " + dbitem.Catatan + ", " + dbitem.PIC + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Jadwal Billing", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerNotifRuteQuery(CustomerNotifRute dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerNotifRute\" (\"CustomerNotifId\", \"CustomerId\", \"IdRute\") VALUES (" + dbitem.CustomerNotifId + ", " + dbitem.CustomerId + ", " + dbitem.IdRute + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Notif Rute", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerNotifRuteQuery(CustomerNotifRute dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerNotifRute\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Notif Rute", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllCustomerNotifRuteQuery(CustomerNotification dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerNotifRute\" WHERE \"CustomerNotificationId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Notif Rute", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerNotifTruckQuery(CustomerNotifTruck dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerNotifRute\" (\"CustomerNotifId\", \"CustomerId\", \"IdTruck\") VALUES ("+dbitem.CustomerNotifId + ", " + dbitem.CustomerId + ", " + dbitem.IdTruck + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Notif Truck", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerNotifTruckQuery(CustomerNotifTruck dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerNotifTruck\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Notif Truck", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllCustomerNotifTruckQuery(CustomerNotification dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerNotifTruck\" WHERE \"CustomerNotificationId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Notif Truck", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerNotificationQuery(CustomerNotification dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerNotification\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Notification", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllCustomerNotificationQuery(Customer dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerNotifTruck\" WHERE \"CustomerId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Notification", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerNotificationQuery(CustomerNotification dbitem, int id){
            if (dbitem.Id == 0) //create
            {
            var query = "INSERT INTO dbo.\"CustomerNotification\" (\"CustomerId\", \"IsActive\", \"IdPic\", \"NotifType\") VALUES (" + dbitem.Id + ", " + dbitem.CustomerId + ", " + dbitem.IsActive + ", " +
                dbitem.IdPic + ", " + dbitem.NotifType + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer Notification", EventDate=DateTime.Now, Modulenya="Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            }
            else{
                var query = "UPDATE dbo.\"CustomerNotification\" SET \"IsActive\" = " + dbitem.IsActive + ", \"IdPic\" = " + dbitem.IdPic + ", \"NotifType\" = " + dbitem.NotifType + "WHERE \"Id\" = " + 
                    dbitem.Id + ";";
                var auditrail = new Auditrail {
                    Actionnya = "Edit", EventDate = DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
                };
                context.Auditrail.Add(auditrail);
            }
            context.SaveChanges();
        }

        public void saveCustomerPPNQuery(CustomerPPN dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerPPN\" (\"CustomerId\", \"PPN\", \"IdRekening\", \"NomorNPWP\", \"NamaNPWP\", \"AddressNPWP\") VALUES (" + dbitem.Id + ", " + dbitem.CustomerId + ", " +
                dbitem.PPN + ", " + dbitem.IdRekening + ", " + dbitem.NomorNPWP + ", " + dbitem.NamaNPWP + ", " + dbitem.AddressNPWP + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer PPN", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerPPNQuery(CustomerPPN dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerPPN\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer PPN", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveUpdCustomerPPNQuery(CustomerPPN dbitem, int id){
            var query = "UPDATE dbo.\"CustomerPPN\" SET \"PPN\" = " + dbitem.PPN + ", \"IdRekening\" = " + dbitem.IdRekening + ", \"NomorNPWP\" = " + dbitem.NomorNPWP + ", \"NamaNPWP\" = " + 
                dbitem.NamaNPWP + ", \"AddressNPWP\" = " + dbitem.AddressNPWP + "WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Update Customer PPN", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveUpdCustomerPICQuery(CustomerPic dbitem, int id){
            var query = "UPDATE dbo.\"CustomerPic\" SET \"Code\" = " + dbitem.Code + ", \"Name\" = " + dbitem.Name + ", \"DepartemenId\" = " + dbitem.DepartemenId + ", \"JabatanId\" = " + 
                dbitem.JabatanId + ", \"EmailAdd\" = " + dbitem.EmailAdd + ", \"Mobile\" = " + dbitem.Mobile + ", \"Urutan\" = " + dbitem.Urutan + " WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Update Customer PIC", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerPICQuery(CustomerPic dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerPic\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Pic", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllCustomerPICQuery(Customer dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerPic\" WHERE \"CustomerId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Pic", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerPICQuery(CustomerPic dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerPic\" (\"CustomerId\", \"Code\", \"Name\", \"DepartemenId\", \"JabatanId\", \"EmailAdd\", \"Mobile\", \"Urutan\") VALUES ( " + dbitem.CustomerId + ", " +
                dbitem.Code + ", " + dbitem.Name + ", " + dbitem.DepartemenId + ", " + dbitem.JabatanId + ", " + dbitem.EmailAdd + ", " + dbitem.Mobile + ", " + dbitem.Urutan + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer PIC", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveUpdCustomerProductTypeQuery(CustomerProductType dbitem, int id){
            var query = "UPDATE dbo.\"CustomerProductType\" SET \"idProduk\" = " + dbitem.idProduk + ", \"PenangananKhusus\" = " + dbitem.PenangananKhusus + ", \"Keterangan\" = " + dbitem.Keterangan +
                " WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Update Customer PIC", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelCustomerProductTypeQuery(CustomerProductType dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerProductType\" WHERE \"Id\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Product Type", EventDate=DateTime.Now, Modulenya="Customer", QueryDetail=query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllCustomerProductTypeQuery(Customer dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerProductType\" WHERE \"CustomerId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete Customer Product Type", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(),
                IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveCustomerProductTypeQuery(CustomerProductType dbitem, int id){
            var query = "INSERT INTO dbo.\"CustomerProductType\" (\"CustomerId\", \"idProduk\", \"PenangananKhusus\", \"Keterangan\") VALUES (" + dbitem.CustomerId + ", " + dbitem.idProduk + ", " +
                dbitem.PenangananKhusus + ", " + dbitem.Keterangan + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Customer PIC", EventDate=DateTime.Now, Modulenya = "Customer", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllSalesOrderPickupUnLoadingAddQuery(SalesOrderPickup dbitem, int id){
            var query = "DELETE FROM dbo.\"SalesOrderPickupLoadingAdd\" WHERE \"SalesOrderPickupId\" = " + dbitem.SalesOrderPickupId + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete SO Pickup", EventDate=DateTime.Now, Modulenya = "Sales Order", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveSalesOrderPickupUnLoadingAddQuery(SalesOrderPickupUnLoadingAdd dbitem, int id){
            var query = "INSERT INTO dbo.\"SalesOrderPickupUnLoadingAdd\" (\"SalesOrderPickupId\", \"CustomerUnloadingAddressId\", \"CustomerId\", urutan, \"IsSelect\") VALUES ( " +
                dbitem.SalesOrderPickupId + ", " + dbitem.CustomerUnloadingAddressId + ", " + dbitem.CustomerId + ", " + dbitem.urutan + ", " + dbitem.IsSelect + ");";
            var auditrail = new Auditrail {
                Actionnya="Add Sales Order Pickup", EventDate=DateTime.Now, Modulenya="Sales Order", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveUpdSalesOrderProsesKonsolidasiQuery(SalesOrderProsesKonsolidasi dbitem, int id){

        }

        public void saveSalesOrderProsesKonsolidasiQuery(SalesOrderProsesKonsolidasi dbitem, int id){
            Context.SalesOrder so = context.SalesOrder.Where(d => d.SalesOrderProsesKonsolidasiId == dbitem.SalesOrderProsesKonsolidasiId).FirstOrDefault();
            if (so != null)
            {
                var order_history = new OrderHistory();
                if (so.Status == "draft") //hanya simpan
                    order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = DateTime.Now, PIC = so.UpdatedBy };
                else
                {//proses ke planning
                    var last_oh = context.OrderHistory.Where(d => d.SalesOrderId == so.Id && d.StatusFlow == 1).FirstOrDefault();
                    if (last_oh == null)
                        order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = DateTime.Now, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy };
                    else
                        order_history = new OrderHistory { SalesOrderId = so.Id, StatusFlow = 1, FlowDate = DateTime.Now, SavedAt = last_oh.SavedAt, ProcessedAt = DateTime.Now, PIC = so.UpdatedBy };
                }
                context.OrderHistory.Add(order_history);
                context.SaveChanges();
            }
        }

        public void saveSalesOrderProsesKonsolidasiCommentQuery(SalesOrderProsesKonsolidasi dbitem, int id){
            
        }

        public void saveSalesOrderProsesKonsolidasiItemQuery(SalesOrderProsesKonsolidasiItem dbitem, int id){
            
        }

        public void saveSalesOrderProsesKonsolidasiLoadingAddQuery(SalesOrderProsesKonsolidasiLoadingAdd dbitem, int id){
            
        }

        public void saveSalesOrderProsesKonsolidasiUnLoadingAddQuery(SalesOrderProsesKonsolidasiUnLoadingAdd dbitem, int id){
        }

        public void saveSettlementRegulerBiayaTambahanQuery(SettlementRegulerTambahanBiaya dbitem, int id){
        }

        public void saveDelAllSettlementRegulerBiayaTambahanQuery(SettlementReguler dbitem, int id){
            var query = "DELETE FROM dbo.\"SettlementRegulerTambahanBiaya\" WHERE \"IdSettlementReguler\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete", EventDate=DateTime.Now, Modulenya = "Settlement Reguler", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveDelAllSpkQuery(Workshop dbitem, int id){
            var query = "DELETE FROM dbo.\"Spk\" WHERE \"IdWorkshop\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete", EventDate=DateTime.Now, Modulenya = "Workshop", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void saveUpdCustomerSupplierQuery(CustomerSupplier dbitem, int id){

        }

        public void saveCustomerSupplierQuery(CustomerSupplier dbitem, int id){

        }

        public void saveDelCustomerSupplierQuery(CustomerSupplier dbitem, int id){
            var query = "DELETE FROM dbo.\"CustomerSupplier\" WHERE \"CustomerId\" = " + dbitem.Id + ";";
            var auditrail = new Auditrail {
                Actionnya="Delete", EventDate=DateTime.Now, Modulenya = "Customer Supplier", QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }

        public void SetAuditTrail(string query, string modul, string act, int user_id){
            var auditrail = new Auditrail {
                Actionnya = act, EventDate = DateTime.Now, Modulenya = modul, QueryDetail = query, RemoteAddress = AppHelper.GetIPAddress(), IdUser = user_id
            };
            context.Auditrail.Add(auditrail);
            context.SaveChanges();
        }
    }
}