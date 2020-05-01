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
using tms_mka_v2.Models;

namespace tms_mka_v2.Business_Logic.Concrete
{
    public class so_mstrRepo : Iso_mstrRepo
    {
      private ContextModelERP context = new ContextModelERP();
      private ContextModel contextBiasa = new ContextModel();
      public void saveSoMstr(SalesOrder soitem, string username, string guid, int customerId, decimal harga, Context.SalesOrderKontrakListSo sokls = null, Context.AdminUangJalan auj = null, string codeSoMstr=null)
      {
          Context.so_mstr model = new Context.so_mstr();
            model.so_oid = guid;
            model.so_ref_po_code = soitem.NoShipment;
            model.so_dom_id = 1;
            model.so_en_id = 1;
            model.so_add_by = username;
            model.so_add_date = DateTime.Now;
            model.so_upd_by = username;
            model.so_upd_date = DateTime.Now;
            model.so_total = harga;//, Total SO
            if (soitem.SalesOrderKonsolidasiId.HasValue)//konsolidasi
            {
                model.so_code = soitem.SONumber;
                model.so_date = soitem.OrderTanggalMuat.Value;
                model.so_departure_date = model.so_date;
                Context.SalesOrderProsesKonsolidasi sopk = contextBiasa.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == soitem.SalesOrderKonsolidasiId).FirstOrDefault().SalesOrderProsesKonsolidasi;
                model.so_vehicle_type = soitem.SalesOrderKonsolidasi.PerhitunganDasar == "Tonase" ? "Kg" : soitem.SalesOrderKonsolidasi.PerhitunganDasar;
                model.so_vehicle = context.code_mstr.Where(d => d.code_code == sopk.DataTruck.VehicleNo).FirstOrDefault().code_id;
                model.so_sales_person = sopk.Driver1Id.Value + 7000000;
            }
            else if (soitem.SalesOrderKontrakId.HasValue && codeSoMstr != null) //kontrak tipe 2
            {
                model.so_code = codeSoMstr;
                model.so_date = soitem.SalesOrderKontrak.SalesOrderKontrakListSo.Select(d => d.MuatDate).Min();
                model.so_departure_date = soitem.SalesOrderKontrak.SalesOrderKontrakListSo.FirstOrDefault().MuatDate;
                model.so_vehicle_type = soitem.SalesOrderKontrak.JenisTrucks.StrJenisTruck;
                model.so_vehicle = context.code_mstr.Where(d => d.code_code == sokls.DataTruck.VehicleNo).FirstOrDefault().code_id;
                model.so_sales_person = sokls.Driver1Id.Value + 7000000;
            }
            else if (sokls == null){//oncall
                model.so_code = soitem.SONumber;
                model.so_date = soitem.OrderTanggalMuat.Value;
                model.so_departure_date = model.so_date;
                model.so_vehicle_type = soitem.SalesOrderOncall.JenisTrucks.StrJenisTruck;
                model.so_vehicle = context.code_mstr.Where(d => d.code_code == soitem.SalesOrderOncall.DataTruck.VehicleNo).FirstOrDefault().code_id;
                model.so_sales_person = soitem.DriverId.Value + 7000000;
            }
            else//kontrak tipe 1 & 3
            {
                string noKontrakInduk = contextBiasa.SalesOrderKontrak.Where(e => e.SalesOrderKontrakId == sokls.SalesKontrakId).FirstOrDefault().SONumber;
                Context.sod_det sod_det = context.sod_det.Where(d => d.sod_price > 0 && d.sod_nopol == sokls.DataTruck.VehicleNo && d.so_mstr.so_code.Contains(noKontrakInduk)).FirstOrDefault();
                if (sod_det != null)
                    model.so_total = 0;//, Total SO
                model.so_code = sokls.NoSo;
                model.so_date = sokls.MuatDate;
                model.so_departure_date = sokls.MuatDate;
                model.so_vehicle_type = sokls.DataTruck.JenisTrucks.StrJenisTruck;
                model.so_vehicle = context.code_mstr.Where(d => d.code_name == sokls.DataTruck.VehicleNo).FirstOrDefault().code_id;
                model.so_sales_person = sokls.Driver1Id.Value + 7000000;
            }
            model.so_ptnr_id_sold = customerId;
            model.so_ptnr_id_bill = customerId;
            model.so_credit_term = 991025;
            Context.CustomerPPN customerPPN = contextBiasa.CustomerPPN.Where(d => d.CustomerId == customerId && !d.PPN).FirstOrDefault();
            model.so_taxable = customerPPN != null ? "N" : "Y";
            model.so_tax_class = customerPPN != null ? 1034 : 992390;
            model.so_si_id = 1;
            model.so_type = "R";
            model.so_pi_id = 101;//, from pi_mstr
            model.so_pay_type = "9942";//,  from code_mstr where code_field ~~* 'payment_type'
            model.so_pay_method = 99290;//"0";// ,   from code_mstr where code_field ~~* 'payment_methode'
            model.so_ar_ac_id = 0; //, From pla_mstr.pla_ac_id partnumber --> pt_mstr.pt_pl_id =pla_mstr.pl_id)
            model.so_dp = 0;//,    nominal yg didapat dr form InputDP
            model.so_disc_header = 0;
            model.so_tran_id = 0;
            model.so_trans_id = "D";
            model.so_dt = DateTime.Now;
            model.so_cu_id = 1;//, from cu_mstr.cu_id currency)
            model.so_total_ppn = 0;//, Total PPN From Detail SO
            model.so_total_pph = 0;//, Total PPH From Detail SO
            model.so_payment = 0;
            model.so_exc_rate = 1;//,  kurs mata uang From exr_rate current Date and Current Currency
            model.so_tax_inc = "N";//,   Y or N termasuk pajak atau belum, checkbox)
            model.so_cons = "N";//,  Y or N konsinyasi atau bukan, checkbox)
            model.so_terbilang = "N";//, Terbilang dalam bahasa indonesia
            model.so_bk_id = 0;//, from bk_mstr.bk_id Nama Bank)
            model.so_interval = 1;//,  from code_mstr where code_field ~~* 'payment_type'  kolom code_user_1
            model.so_ppn_type = customerPPN != null ? "N" : "A";//,  E=PPN BEBAS, A = PPN BAYAR, N = NON
            model.so_branch_id = 10001;//, SO Branch from branch_mstr.branch_id)
            model.so_group_id = 991414;//,  from code_mstr where code_field ~~* 'so_group'
            model.so_exc_rate_pi = 0;
            model.so_prct_limit_return = 0;
            model.so_disc_type = 0;
            model.so_start_route = "0";//,   Kota Awal Berangkat from code_mstr where code_field ~~* 'route_mstr'
            model.so_destination_route = "0";//  kota Tujuan from code_mstr where code_field ~~* 'route_mstr'*/
            if (soitem.SalesOrderOncallId.HasValue)//oncall
            {
                Context.DaftarHargaOnCallItem dho = contextBiasa.DaftarHargaOnCallItem.Where(d => d.Id == soitem.SalesOrderOncall.IdDaftarHargaItem).FirstOrDefault();
                model.so_start = dho.NamaRuteDaftarHarga.Split('-')[0];
                model.so_end = dho.NamaRuteDaftarHarga.Split(new string[] { dho.NamaRuteDaftarHarga.Split('-')[0] + "-" }, StringSplitOptions.None)[1];
            }
            if (soitem.SalesOrderKonsolidasiId.HasValue)//konsolidasi
            {
                Context.DaftarHargaKonsolidasiItem dho = contextBiasa.DaftarHargaKonsolidasiItem.Where(d => d.Id == soitem.SalesOrderKonsolidasi.IdDaftarHargaItem).FirstOrDefault();
                model.so_start = dho.NamaDaftarHargaRute.Split('-')[0];
                model.so_end = dho.NamaDaftarHargaRute.Split(new string[] {dho.NamaDaftarHargaRute.Split('-')[0] + "-"}, StringSplitOptions.None)[1];
            }
            else if (sokls != null && codeSoMstr == null)//kontrak tipe 1&3
            {
                Context.AdminUangJalan adminuj = sokls.AdminUangJalan == null ? contextBiasa.AdminUangJalan.Where(d => d.SONumber != null && d.SONumber.Contains(sokls.NoSo) && d.Status != "Batal").FirstOrDefault() : sokls.AdminUangJalan;
                Context.DaftarHargaKontrakItem dho = contextBiasa.DaftarHargaKontrakItem.Where(d => d.Id == adminuj.DaftarHargaKontrakId).FirstOrDefault();
                model.so_start = dho.NamaRuteDaftarHarga.Split('-')[0];
                model.so_end = dho.NamaRuteDaftarHarga.Split(new string[] {dho.NamaRuteDaftarHarga.Split('-')[0] + "-"}, StringSplitOptions.None)[1];
            }
            else if (soitem.SalesOrderKontrakId.HasValue)//kontrak tipe 2
            {
                Context.DaftarHargaKontrakItem dho = contextBiasa.DaftarHargaKontrakItem.Where(e => e.Id == auj.DaftarHargaKontrakId).FirstOrDefault();
                model.so_start = dho.NamaRuteDaftarHarga.Split('-')[0];
                model.so_end = dho.NamaRuteDaftarHarga.Split(new string[] { dho.NamaRuteDaftarHarga.Split('-')[0] + "-" }, StringSplitOptions.None)[1];
            }

            context.so_mstr.Add(model);
            try
            {
                context.SaveChanges();
            }
            catch (Exception) { }
      }

      public string saveSoMstrSolarInap(SalesOrder soitem, string username, string guid, int customerId, decimal harga, Context.SalesOrderKontrakListSo sokls = null, Context.SolarInap solarInap = null)
      {
          Context.so_mstr model = new Context.so_mstr();
            model.so_oid = guid;
            model.so_ref_po_code = soitem.NoShipment;
            model.so_dom_id = 1;
            model.so_en_id = 1;
            model.so_add_by = username;
            model.so_add_date = DateTime.Now;
            model.so_upd_by = username;
            model.so_date = solarInap.TanggalDari;
            model.so_upd_date = DateTime.Now;
            if (sokls == null){
                string so_code = soitem.SONumber + "-SI" + solarInap.TanggalDari.ToString();
                model.so_vehicle = context.code_mstr.Where(d => d.code_code == soitem.DataTruck.VehicleNo).FirstOrDefault().code_id;
                model.so_departure_date = soitem.OrderTanggalMuat.Value;
                model.so_vehicle_type = soitem.DataTruck.JenisTrucks.StrJenisTruck;
                model.so_sales_person = soitem.DriverId.Value + 7000000;
                try
                {
                    model.so_code = so_code + "-" + (context.so_mstr.Where(d => d.so_code.Contains(so_code)).Select(d => d.so_code).Max().Split('-').Last()+1);
                }
                catch (Exception) {
                    model.so_code = so_code + "-1";
                }
            }
            else
            {
                string so_code = sokls.NoSo + "-SI" + solarInap.TanggalDari.ToString();
                model.so_date = sokls.MuatDate;
                model.so_departure_date = sokls.MuatDate;
                model.so_vehicle_type = sokls.DataTruck.JenisTrucks.StrJenisTruck;
                model.so_vehicle = context.code_mstr.Where(d => d.code_name == sokls.DataTruck.VehicleNo).FirstOrDefault().code_id;
                model.so_sales_person = sokls.Driver1Id.Value + 7000000;
                try
                {
                    model.so_code = so_code + "-" + (context.so_mstr.Where(d => d.so_code.Contains(so_code)).Select(d => d.so_code).Max().Split('-').Last()+1);
                }
                catch (Exception)
                {
                    model.so_code = so_code + "-1";
                }
            }
            model.so_ptnr_id_sold = customerId;
            model.so_ptnr_id_bill = customerId;
            model.so_credit_term = 991025;
            Context.CustomerPPN customerPPN = contextBiasa.CustomerPPN.Where(d => d.CustomerId == soitem.CustomerId && !d.PPN).FirstOrDefault();
            model.so_taxable = customerPPN != null ? "N" : "Y";
            model.so_tax_class = customerPPN != null ? 1034 : 992390;
            model.so_si_id = 1;
            model.so_type = "R";
            model.so_pi_id = 101;//, from pi_mstr
            model.so_pay_type = "9942";//,  from code_mstr where code_field ~~* 'payment_type'
            model.so_pay_method = 99290;//"0";// ,   from code_mstr where code_field ~~* 'payment_methode'
            model.so_ar_ac_id = 0; //, From pla_mstr.pla_ac_id partnumber --> pt_mstr.pt_pl_id =pla_mstr.pl_id)
            model.so_dp = 0;//,    nominal yg didapat dr form InputDP
            model.so_disc_header = 0;
            model.so_total = harga;//, Total SO
            model.so_tran_id = 0;
            model.so_trans_id = "D";
            model.so_dt = DateTime.Now;
            model.so_cu_id = 1;//, from cu_mstr.cu_id currency)
            model.so_total_ppn = 0;//, Total PPN From Detail SO
            model.so_total_pph = 0;//, Total PPH From Detail SO
            model.so_payment = 0;
            model.so_exc_rate = 1;//,  kurs mata uang From exr_rate current Date and Current Currency
            model.so_tax_inc = "N";//,   Y or N termasuk pajak atau belum, checkbox)
            model.so_cons = "N";//,  Y or N konsinyasi atau bukan, checkbox)
            model.so_terbilang = "N";//, Terbilang dalam bahasa indonesia
            model.so_bk_id = 0;//, from bk_mstr.bk_id Nama Bank)
            model.so_interval = 1;//,  from code_mstr where code_field ~~* 'payment_type'  kolom code_user_1
            model.so_ppn_type = customerPPN != null ? "N" : "A";//,  E=PPN BEBAS, A = PPN BAYAR, N = NON
            model.so_branch_id = 10001;//, SO Branch from branch_mstr.branch_id)
            model.so_group_id = 991414;//,  from code_mstr where code_field ~~* 'so_group'
            model.so_exc_rate_pi = 0;
            model.so_prct_limit_return = 0;
            model.so_disc_type = 0;
            model.so_start_route = "0";//,   Kota Awal Berangkat from code_mstr where code_field ~~* 'route_mstr'
            model.so_destination_route = "0";//  kota Tujuan from code_mstr where code_field ~~* 'route_mstr'*/
            if (soitem.SalesOrderOncallId.HasValue)
            {
                Context.DaftarHargaOnCallItem dho = contextBiasa.DaftarHargaOnCallItem.Where(d => d.Id == soitem.SalesOrderOncall.IdDaftarHargaItem).FirstOrDefault();
                model.so_start = dho.NamaRuteDaftarHarga.Split('-')[0];
                model.so_end = dho.NamaRuteDaftarHarga.Split('-')[1];
            }
            else if (sokls != null)
            {
                Context.AdminUangJalan adminuj = sokls.AdminUangJalan == null ? contextBiasa.AdminUangJalan.Where(d => d.SONumber != null && d.SONumber.Contains(sokls.NoSo) && d.Status != "Batal").FirstOrDefault() : sokls.AdminUangJalan;
                Context.DaftarHargaKontrakItem dho = contextBiasa.DaftarHargaKontrakItem.Where(d => d.Id == adminuj.DaftarHargaKontrakId).FirstOrDefault();
                model.so_start = dho.NamaRuteDaftarHarga.Split('-')[0];
                model.so_end = dho.NamaRuteDaftarHarga.Split('-')[1];
            }

            context.so_mstr.Add(model);
            context.SaveChanges();
            return model.so_code;
      }

      public void UpdateSoMstrVehicle(so_mstr dbitem)
      {
            context.so_mstr.Attach(dbitem);
            var entry = context.Entry(dbitem);
            entry.State = EntityState.Modified;
            context.SaveChanges();
      }

      public void saveSoShipMstrSolarInap(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null, Context.SolarInap solarInap = null)
        {
//            try
  //          {
                Context.soship_mstr model = new Context.soship_mstr();
                model.soship_oid = ship_guid; // uuid NOT NULL, Random uid
                model.soship_dom_id = 1;
                model.soship_en_id = 1;
                model.soship_add_by = username;
                model.soship_add_date = DateTime.Now;
                model.soship_upd_by = username;
                model.soship_upd_date = DateTime.Now;
                model.soship_date = solarInap.TanggalDari;// date,    tgl settlement
                model.soship_code = context.so_mstr.Where(d => d.so_oid == guid).FirstOrDefault().so_code;
                model.soship_so_oid = guid;
                model.soship_si_id = 1;
                model.soship_is_shipment = "Y";
                model.soship_dt = DateTime.Now;
                model.soship_exc_rate = 1;
                model.soship_cu_id = 1;
                model.soship_branch_id = 10001;
                model.soship_expeditur = 0;
                context.soship_mstr.Add(model);
                context.SaveChanges();
    /*        }
            catch (Exception e)
            {
            }*/
      }

      public void saveSoShipMstr(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null, string soNumber=null)
      {
            Context.soship_mstr model = new Context.soship_mstr();
            model.soship_oid = ship_guid; // uuid NOT NULL, Random uid
            model.soship_dom_id = 1;
            model.soship_en_id = 1;
            model.soship_add_by = username;
            model.soship_add_date = DateTime.Now;
            model.soship_upd_by = username;
            model.soship_upd_date = DateTime.Now;
            if (soNumber != null)
            {
                model.soship_code = soNumber;
                model.soship_date = soitem.SalesOrderKontrak.SalesOrderKontrakListSo.FirstOrDefault().MuatDate;// date,    tgl settlement
            }
            else if (sokls == null)
            {
                model.soship_code = soitem.SONumber;
                model.soship_date = soitem.OrderTanggalMuat;// date,    tgl settlement
            }
            else
            {
                model.soship_code = sokls.NoSo;
                model.soship_date = sokls.MuatDate;
            }
            model.soship_so_oid = guid;
            model.soship_si_id = 1;
            model.soship_is_shipment = "Y";
            model.soship_dt = DateTime.Now;
            model.soship_exc_rate = 1;
            model.soship_cu_id = 1;
            model.soship_branch_id = 10001;
            model.soship_expeditur = 0;
            context.soship_mstr.Add(model);
            try
            {
                context.SaveChanges();
            }
            catch (Exception) { }
      }

      public void saveSoShipDet(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null)
        {
            Context.soshipd_det model = new Context.soshipd_det();
            model.soshipd_oid = Guid.NewGuid().ToString(); // uuid NOT NULL, Random uid
            model.soshipd_soship_oid = guid;//  uuid,     shipmstr
            model.soshipd_sod_oid = ship_guid;//  uuid,  so_det
            model.soshipd_seq = 0;
            model.soshipd_qty = -1;
            model.soshipd_um = 991403;
            model.soshipd_um_conv = 1;
            model.soshipd_qty_real = -1;
            model.soshipd_si_id = 1;
            model.soshipd_loc_id = 10001;;
            model.soshipd_dt = DateTime.Now;
            model.soshipd_qty_inv = 0;
            model.soshipd_close_line = "N";
            model.soshipd_qty_allocated = 0;
            model.soshipd_pieces = 0;
            if (soitem.SalesOrderKontrakId == null)
                model.soshipd_remarks = soitem.SalesOrderOncallId.HasValue ? soitem.SalesOrderOncall.Keterangan : ""; //  character varying100),   remak biasa we
            model.soshipd_packaging_id = 0;
            model.soshipd_qty_packaging = 0;
            model.soshipd_qty_plus = 0;
            model.soshipd_qty_minus = 0;
            model.soshipd_shipcust = 0;
            context.soshipd_det.Add(model);
            try
            {
                context.SaveChanges();
            }
            catch (Exception) { }
        }

      public so_mstr FindByPK(string code)
      {
            return context.so_mstr.Where(d => d.so_code == code).FirstOrDefault();
      }

      public soship_mstr FindSoShipBySo(string soship_so_oid)
      {
          return context.soship_mstr.Where(d => d.soship_so_oid == soship_so_oid).FirstOrDefault();
      }

      public sod_det FindSodDetBySo(string sod_so_oid)
      {
          return context.sod_det.Where(d => d.sod_so_oid == sod_so_oid).FirstOrDefault();
      }

      public so_mstr FindSoDet(string code)
      {
            return context.so_mstr.Where(d => d.so_code == code).FirstOrDefault();
      }

      public void saveSoDet(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null, decimal priceKonsolidasi = 0, Context.AdminUangJalan auj = null)
      {
            Context.sod_det model = new Context.sod_det();
            model.sod_oid = ship_guid; // uuid NOT NULL, Random uid
            model.sod_dom_id = 1; //  smallint,  1
            model.sod_qty_shipment = 1;
            model.sod_en_id = 1; //  integer,    1
            model.sod_add_by = username; //  character varying25),  current_user.name
            model.sod_add_date = DateTime.Now; //  timestamp without time zone,     DateTime.Now
            model.sod_upd_by = username;//  character varying25),  current_user.name
            model.sod_upd_date = DateTime.Now; //  timestamp without time zone,     DateTime.Now
            model.sod_so_oid = guid; //  uuid,      so_mstr.so_uid
            model.sod_seq = 1; //  smallint,     urutan sod_det dlm 1 so_mstr
            model.sod_is_additional_charge = "N"; //  character1),   0
            model.sod_si_id = 1; //  integer DEFAULT 1,
            model.sod_pt_id = 103690; // ,      9999999
            model.sod_rmks = soitem.SalesOrderOncallId.HasValue ? soitem.SalesOrderOncall.Keterangan : soitem.SalesOrderProsesKonsolidasiId.HasValue ? "" : soitem.SalesOrderPickupId.HasValue ? soitem.SalesOrderPickup.Keterangan : ""; //  character varying100),   remak biasa we
            model.sod_qty = 1; //  numeric18,8), banyaknya truk yg disewa
            model.sod_qty_allocated = 0; //  numeric18,8) DEFAULT 0,  0
            model.sod_qty_picked = 0; //  numeric18,8),   0
            model.sod_qty_pending_inv = 0; //  numeric18,8),    0
            model.sod_um = 0; //  integer,    0
            model.sod_cost = 0; //  numeric26,8),   0
            if (soitem.SalesOrderOncallId.HasValue)
            {
                Context.DaftarHargaOnCallItem dho = contextBiasa.DaftarHargaOnCallItem.Where(d => d.Id == soitem.SalesOrderOncall.IdDaftarHargaItem).FirstOrDefault();
                decimal price = dho.Harga;
                model.sod_price = price; //  numeric26,8),  kl dh ada harganya harga masuk k sini
                model.sod_ppn = price / 10; //  numeric26,8) DEFAULT 0,      0
                model.sod_nopol = soitem.DataTruck.VehicleNo;
            }
            else if (soitem.SalesOrderKontrakId.HasValue)
            {
                Context.DaftarHargaKontrakItem dho;
                if (sokls == null || auj != null)
                    dho = contextBiasa.DaftarHargaKontrakItem.Where(d => d.Id == auj.DaftarHargaKontrakId).FirstOrDefault();
                else
                {
                    Context.AdminUangJalan adminuj = sokls.AdminUangJalan == null ? contextBiasa.AdminUangJalan.Where(d => d.SONumber != null && d.SONumber.Contains(sokls.NoSo) && d.Status != "Batal").FirstOrDefault() : sokls.AdminUangJalan;
                    dho = contextBiasa.DaftarHargaKontrakItem.Where(d => d.Id == adminuj.DaftarHargaKontrakId).FirstOrDefault();
                }
                if (dho.DaftarHargaKontrak.LookUpTypeKontrak.Nama == "TYPE 1")
                {
                    model.sod_price = int.Parse(dho.Harga.ToString()) / soitem.SalesOrderKontrak.JumlahHariKerja; //  numeric26,8),  kl dh ada harganya harga masuk k sini
                    model.sod_ppn = model.sod_price / 10; //  numeric26,8) DEFAULT 0,      0
                }
                else if (dho.DaftarHargaKontrak.LookUpTypeKontrak.Nama == "TYPE 2")
                {
                    model.sod_price = dho.Harga; //  numeric26,8),  kl dh ada harganya harga masuk k sini
                    model.sod_ppn = model.sod_price / 10; //  numeric26,8) DEFAULT 0,      0
                }
                else
                {
                    string noKontrakInduk = contextBiasa.SalesOrderKontrak.Where(e => e.SalesOrderKontrakId == sokls.SalesKontrakId).FirstOrDefault().SONumber;
                    Context.sod_det sod_det = context.sod_det.Where(d => d.sod_price > 0 && d.sod_nopol == sokls.DataTruck.VehicleNo && d.so_mstr.so_code.Contains(noKontrakInduk)).FirstOrDefault();
                    decimal price = sod_det == null ? dho.Harga : 0;
                    model.sod_price = price; //  numeric26,8),  kl dh ada harganya harga masuk k sini
                    model.sod_ppn = price / 10; //  numeric26,8) DEFAULT 0,      0
                }
                model.sod_nopol = sokls == null ? soitem.SalesOrderKontrak.SalesOrderKontrakListSo.FirstOrDefault().DataTruck.VehicleNo : sokls.DataTruck.VehicleNo;
            }
            else if (soitem.SalesOrderKonsolidasiId.HasValue)
            {
                model.sod_price = priceKonsolidasi; //  numeric26,8),  kl dh ada harganya harga masuk k sini
                model.sod_ppn = priceKonsolidasi / 10; //  numeric26,8) DEFAULT 0,      0
                model.sod_nopol = contextBiasa.SalesOrderProsesKonsolidasiItem.Where(d => d.SalesOrderKonsolidasiId == soitem.SalesOrderKonsolidasiId).FirstOrDefault().SalesOrderProsesKonsolidasi.DataTruck.VehicleNo;
            }
            model.sod_disc = 0;
            model.sod_sales_ac_id = 0;
            model.sod_sales_sb_id = 0;
            model.sod_sales_cc_id = 0;
            model.sod_disc_ac_id = 0;
            model.sod_um_conv = 1; //  numeric18,8),      1
            Context.CustomerPPN customerPPN = contextBiasa.CustomerPPN.Where(d => d.CustomerId == soitem.CustomerId && !d.PPN).FirstOrDefault();
            if (soitem.SalesOrderKonsolidasiId.HasValue)
                customerPPN = contextBiasa.CustomerPPN.Where(d => d.CustomerId == soitem.SalesOrderKonsolidasi.NamaTagihanId && !d.PPN).FirstOrDefault();
            model.sod_taxable = customerPPN != null ? "N" : "Y";
            model.sod_tax_inc = "N"; //  character1), N
            model.sod_tax_class = customerPPN != null ? 1034 : 992390;
            model.sod_dt = DateTime.Now; //  without time zone,      biasa we
            model.sod_payment = 0; //  numeric26,8), -- ini untuk angsuran/bulan  0
            model.sod_dp = 0; //  numeric26,8),     0
            model.sod_sales_unit = 0; //  numeric26,8),   0
            model.sod_loc_id = 10001; //  integer,      0
            model.sod_serial = "0"; //  character varying100),    0
            model.sod_qty_return = 0; //  numeric26,8),   0
            model.sod_ppn_type = customerPPN != null ? "N" : "A";//,  E=PPN BEBAS, A = PPN BAYAR, N = NON
            model.sod_pph = 0; //  numeric26,8) DEFAULT 0,      0
            model.sod_price_line = model.sod_price + model.sod_ppn; //  numeric26,8),     sod_qty numeric18,8),
            model.sod_disc1 = 0; //  numeric11,8) DEFAULT 0,    0
            model.sod_disc2 = 0; //  numeric11,8) DEFAULT 0,    0
            model.sod_packaging_id = 0; //  integer NOT NULL DEFAULT 0,     0
            model.sod_qty_packaging = 0; //  numeric26,8) DEFAULT 0,  0
            model.sod_qty_add = 0; //  numeric26,8) DEFAULT 0,  0
            context.sod_det.Add(model);
            try
            {
                context.SaveChanges();
            }
            catch (Exception) { }
        }

      public void saveSoDetSolarInap(SalesOrder soitem, string username, string guid, string ship_guid, decimal price)
      {
            Context.sod_det model = new Context.sod_det();
            model.sod_oid = ship_guid; // uuid NOT NULL, Random uid
            model.sod_dom_id = 1; //  smallint,  1
            model.sod_qty_shipment = 1;
            model.sod_en_id = 1; //  integer,    1
            model.sod_add_by = username; //  character varying25),  current_user.name
            model.sod_add_date = DateTime.Now; //  timestamp without time zone,     DateTime.Now
            model.sod_upd_by = username;//  character varying25),  current_user.name
            model.sod_upd_date = DateTime.Now; //  timestamp without time zone,     DateTime.Now
            model.sod_so_oid = guid; //  uuid,      so_mstr.so_uid
            model.sod_seq = 1; //  smallint,     urutan sod_det dlm 1 so_mstr
            model.sod_is_additional_charge = "N"; //  character1),   0
            model.sod_si_id = 1; //  integer DEFAULT 1,
            model.sod_pt_id = 103690; // ,      9999999
            model.sod_rmks = soitem.SalesOrderOncallId.HasValue ? soitem.SalesOrderOncall.Keterangan : soitem.SalesOrderProsesKonsolidasiId.HasValue ? "" : soitem.SalesOrderPickupId.HasValue ? soitem.SalesOrderPickup.Keterangan : ""; //  character varying100),   remak biasa we
            model.sod_qty = 1; //  numeric18,8), banyaknya truk yg disewa
            model.sod_qty_allocated = 0; //  numeric18,8) DEFAULT 0,  0
            model.sod_qty_picked = 0; //  numeric18,8),   0
            model.sod_qty_pending_inv = 0; //  numeric18,8),    0
            model.sod_um = 0; //  integer,    0
            model.sod_cost = 0; //  numeric26,8),   0
            model.sod_price = price; //  numeric26,8),  kl dh ada harganya harga masuk k sini
            model.sod_ppn = price / 10; //  numeric26,8) DEFAULT 0,      0
            model.sod_disc = 0;
            model.sod_sales_ac_id = 0;
            model.sod_sales_sb_id = 0;
            model.sod_sales_cc_id = 0;
            model.sod_disc_ac_id = 0;
            model.sod_um_conv = 1; //  numeric18,8),      1
            Context.CustomerPPN customerPPN = contextBiasa.CustomerPPN.Where(d => d.CustomerId == soitem.CustomerId && !d.PPN).FirstOrDefault();
            model.sod_taxable = customerPPN != null ? "N" : "Y";
            model.sod_tax_inc = "N"; //  character1), N
            model.sod_tax_class = customerPPN != null ? 1034 : 992390;
            model.sod_dt = DateTime.Now; //  without time zone,      biasa we
            model.sod_payment = 0; //  numeric26,8), -- ini untuk angsuran/bulan  0
            model.sod_dp = 0; //  numeric26,8),     0
            model.sod_sales_unit = 0; //  numeric26,8),   0
            model.sod_loc_id = 10001; //  integer,      0
            model.sod_serial = "0"; //  character varying100),    0
            model.sod_qty_return = 0; //  numeric26,8),   0
            model.sod_ppn_type = customerPPN != null ? "N" : "A";//,  E=PPN BEBAS, A = PPN BAYAR, N = NON
            model.sod_pph = 0; //  numeric26,8) DEFAULT 0,      0
            model.sod_price_line = model.sod_price + model.sod_ppn; //  numeric26,8),     sod_qty numeric18,8),
            model.sod_disc1 = 0; //  numeric11,8) DEFAULT 0,    0
            model.sod_disc2 = 0; //  numeric11,8) DEFAULT 0,    0
            model.sod_packaging_id = 0; //  integer NOT NULL DEFAULT 0,     0
            model.sod_qty_packaging = 0; //  numeric26,8) DEFAULT 0,  0
            model.sod_qty_add = 0; //  numeric26,8) DEFAULT 0,  0
            model.sod_nopol = soitem.SalesOrderOncallId.HasValue ? soitem.SalesOrderOncall.DataTruck.VehicleNo : soitem.SalesOrderProsesKonsolidasiId.HasValue ? soitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo : soitem.SalesOrderPickupId.HasValue ? soitem.SalesOrderPickup.DataTruck.VehicleNo : ""; //  character varying100),   remak biasa we
            context.sod_det.Add(model);
            context.SaveChanges();
        }
    }
}