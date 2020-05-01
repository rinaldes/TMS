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
    public class glt_detRepo : Iglt_detRepo
    {
        private ContextModelERP context = new ContextModelERP();
        private ContextModel contextBiasa = new ContextModel();
        public void save(int seq, Context.glt_det model, Context.SalesOrder soitem, string code, decimal? nominalDb, decimal? nominalCr, Models.ac_mstr ac_mstr)
        {
            model.glt_oid = Guid.NewGuid().ToString();
            model.glt_dom_id = 1;
            model.glt_en_id = 1;
            model.glt_add_by = "";
            model.glt_add_date = DateTime.Now;
            model.glt_upd_by = "";
            model.glt_upd_date = DateTime.Now;
            model.glt_code = code;
            model.glt_date = DateTime.Now;//soitem.SalesOrderOncallId.HasValue ? soitem.SalesOrderOncall.TanggalMuat : soitem.SalesOrderProsesKonsolidasiId.HasValue ? soitem.SalesOrderProsesKonsolidasi.TanggalMuat : soitem.SalesOrderPickupId.HasValue ? soitem.SalesOrderPickup.TanggalPickup : DateTime.Now;
            model.glt_type = "SO"; //?
            model.glt_cu_id = 1;
            model.glt_exc_rate = 1;
            model.glt_seq = seq;
            model.glt_ac_id = ac_mstr.id;
            model.glt_cc_id = 0;
            model.glt_sb_id = 1;
            model.glt_desc = ac_mstr.ac_name;
            model.glt_debit = nominalDb;
            model.glt_credit = nominalCr;
            model.glt_posted = "N";
            model.glt_dt = DateTime.Now;
            model.glt_branch_id = 10001;
            if (soitem.SalesOrderKontrakId.HasValue)
            {
                try
                {
                    int idx = code.Split(',').Count() - 1;
                    string cd = code.Split(',')[idx];
                    Context.SalesOrderKontrakListSo sokls = contextBiasa.SalesOrderKontrakListSo.Where(d => d.NoSo == cd).FirstOrDefault();
                    if (sokls == null)
                    {
                        //sokls = sok;
                    }
                    if (sokls.Driver1Id == null)
                    {
                        model.glt_driver_id = sokls.AdminUangJalan.IdDriver1.Value + 7000000;
                        model.glt_no_pol = sokls.AdminUangJalan.DataTruck.VehicleNo;
                    }
                    else
                    {
                        model.glt_driver_id = sokls.Driver1Id.Value + 7000000;
                        model.glt_no_pol = sokls.DataTruck.VehicleNo;
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                model.glt_driver_id = soitem.SalesOrderOncallId.HasValue ? (soitem.SalesOrderOncall.Driver1Id.HasValue ? soitem.SalesOrderOncall.Driver1Id.Value + 7000000 : 0) : soitem.SalesOrderProsesKonsolidasiId.HasValue ? (soitem.SalesOrderProsesKonsolidasi.Driver1Id.HasValue ? soitem.SalesOrderProsesKonsolidasi.Driver1Id.Value + 7000000 : 0) : soitem.SalesOrderPickupId.HasValue ? (soitem.SalesOrderPickup.Driver1Id.HasValue ? soitem.SalesOrderPickup.Driver1Id.Value + 7000000 : 0) : 0;
                model.glt_no_pol = soitem.SalesOrderOncallId.HasValue ? (soitem.SalesOrderOncall.DataTruck == null ? null : soitem.SalesOrderOncall.DataTruck.VehicleNo) : soitem.SalesOrderProsesKonsolidasiId.HasValue ? (soitem.SalesOrderProsesKonsolidasi.DataTruck == null ? null : soitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo) : soitem.SalesOrderPickupId.HasValue ? (soitem.SalesOrderPickup.DataTruck == null ? null : soitem.SalesOrderPickup.DataTruck.VehicleNo) : null;
            }
            context.glt_det.Add(model);
            context.SaveChanges();
        }

        public void saveFromAc(int seq, string code, decimal? nominalDb, decimal? nominalCr, Models.ac_mstr ac_mstr, Context.SalesOrder soitem, string desc = null, Context.SalesOrderKontrakListSo sok = null, System.DateTime? TanggalAktual = null, int? supplier_id = null, string glt_ref_detail_no = null, int? driver_id=null)
        {
            if (nominalDb > 0 || nominalCr > 0 || nominalDb < 0 || nominalCr < 0)
            {
                Context.glt_det model = new Context.glt_det();
                model.glt_oid = Guid.NewGuid().ToString();
                if (soitem != null)
                {
                    if (soitem.CustomerId != null)
                    {
                        model.glt_ptnr_id = supplier_id == null ? soitem.CustomerId.Value : supplier_id.Value;
                        model.glt_det_ptnr_id = supplier_id == null ? soitem.CustomerId.Value : supplier_id.Value;
                    }
                    model.glt_ref_trans_code = soitem.SONumber;
                    if (context.so_mstr.Any(d => d.so_code.Contains(soitem.SONumber)))
                        model.glt_ref_oid = context.so_mstr.Where(d => d.so_code.Contains(soitem.SONumber)).FirstOrDefault().so_oid;
                }
                model.glt_dom_id = 1;
                model.glt_en_id = 1;
                model.glt_add_by = "";
                model.glt_add_date = TanggalAktual == null ? DateTime.Now : TanggalAktual.Value;
                model.glt_upd_by = "";
                model.glt_upd_date = TanggalAktual == null ? DateTime.Now : TanggalAktual.Value;
                model.glt_code = code;
                model.glt_date = TanggalAktual == null ? DateTime.Now : TanggalAktual.Value;
                model.glt_type = "SO"; //?
                model.glt_cu_id = 1;
                model.glt_exc_rate = 1;
                model.glt_seq = seq;
                model.glt_ac_id = ac_mstr.id;
                model.glt_cc_id = 0;
                model.glt_sb_id = 1;
                model.glt_desc = desc == null ? ac_mstr.ac_name : desc;
                model.glt_debit = nominalDb;
                model.glt_credit = nominalCr;
                model.glt_posted = "N";
                model.glt_dt = TanggalAktual == null ? DateTime.Now : TanggalAktual.Value;
                model.glt_branch_id = 10001;
                model.glt_ref_detail_no = glt_ref_detail_no;
                if (soitem == null)
                {
                    model.glt_driver_id = driver_id.Value + 7000000;
                }
                else
                {
                    if (sok != null && sok.AdminUangJalan != null)
                    {
                        model.glt_driver_id = sok.AdminUangJalan.IdDriver1.Value + 7000000;
                        model.glt_no_pol = sok.AdminUangJalan.DataTruck.VehicleNo;
                    }
                    else if (soitem.SalesOrderKontrakId.HasValue)
                    {
                        try
                        {
                            int idx = code.Split(',').Count() - 1;
                            string cd = code.Split(',')[idx];
                            Context.SalesOrderKontrakListSo sokls = contextBiasa.SalesOrderKontrakListSo.Where(d => d.NoSo == cd).FirstOrDefault();
                            if (sokls == null)
                            {
                                sokls = sok;
                            }
                            if (sokls.Driver1Id == null)
                            {
                                model.glt_driver_id = sokls.AdminUangJalan.IdDriver1.Value + 7000000;
                                model.glt_no_pol = sokls.AdminUangJalan.DataTruck.VehicleNo;
                            }
                            else
                            {
                                model.glt_driver_id = sokls.Driver1Id.Value + 7000000;
                                model.glt_no_pol = sokls.DataTruck.VehicleNo;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else if (driver_id != null)
                    {
                        model.glt_driver_id = driver_id.Value + 7000000;
                        model.glt_no_pol = soitem.SalesOrderOncallId.HasValue ? (soitem.SalesOrderOncall.DataTruck == null ? null : soitem.SalesOrderOncall.DataTruck.VehicleNo) : soitem.SalesOrderProsesKonsolidasiId.HasValue ? (soitem.SalesOrderProsesKonsolidasi.DataTruck == null ? null : soitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo) : soitem.SalesOrderPickupId.HasValue ? (soitem.SalesOrderPickup.DataTruck == null ? null : soitem.SalesOrderPickup.DataTruck.VehicleNo) : null;
                    }
                    else
                    {
                        model.glt_driver_id = soitem.SalesOrderOncallId.HasValue ? (soitem.SalesOrderOncall.Driver1Id.HasValue ? soitem.SalesOrderOncall.Driver1Id.Value + 7000000 : 0) : soitem.SalesOrderProsesKonsolidasiId.HasValue ? (soitem.SalesOrderProsesKonsolidasi.Driver1Id.HasValue ? soitem.SalesOrderProsesKonsolidasi.Driver1Id.Value + 7000000 : 0) : soitem.SalesOrderPickupId.HasValue ? (soitem.SalesOrderPickup.Driver1Id.HasValue ? soitem.SalesOrderPickup.Driver1Id.Value + 7000000 : 0) : 0;
                        model.glt_no_pol = soitem.SalesOrderOncallId.HasValue ? (soitem.SalesOrderOncall.DataTruck == null ? null : soitem.SalesOrderOncall.DataTruck.VehicleNo) : soitem.SalesOrderProsesKonsolidasiId.HasValue ? (soitem.SalesOrderProsesKonsolidasi.DataTruck == null ? null : soitem.SalesOrderProsesKonsolidasi.DataTruck.VehicleNo) : soitem.SalesOrderPickupId.HasValue ? (soitem.SalesOrderPickup.DataTruck == null ? null : soitem.SalesOrderPickup.DataTruck.VehicleNo) : null;
                    }
                }
                try
                {
                    model.glt_daybook = "TMS-" + (model.glt_code.Contains("PD") ? "SO" : model.glt_code.Contains("KL") ? "CL" : "AUJ");
                }
                catch (Exception)
                {
                }
                context.glt_det.Add(model);
                context.SaveChanges();
            }
        }

        public void updateFromAc(Context.glt_det model, decimal? nominalDb, decimal? nominalCr)
        {
            model.glt_debit = nominalDb;
            model.glt_credit = nominalCr;
            context.glt_det.Attach(model);
            var entry = context.Entry(model);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }

        public void saveFromBk(int seq, string code, decimal? nominalDb, decimal? nominalCr, Models.bk_mstr bk_mstr, Context.SalesOrder soitem)
        {
            Context.glt_det model = new Context.glt_det();
            model.glt_oid = Guid.NewGuid().ToString();
            model.glt_dom_id = 1;
            model.glt_en_id = 1;
            model.glt_add_by = "";
            model.glt_add_date = DateTime.Now;
            model.glt_upd_by = "";
            model.glt_upd_date = DateTime.Now;
            model.glt_code = code;
            model.glt_date = DateTime.Now;
            model.glt_type = "SO";
            model.glt_type = model.glt_code.Contains("KL") ? "CL" : "SO";
            model.glt_cu_id = 1;
            model.glt_exc_rate = 1;
            model.glt_seq = seq;
            model.glt_ac_id = bk_mstr.id;
            model.glt_cc_id = 0;
            model.glt_sb_id = 1;
            model.glt_desc = bk_mstr.bk_name;
            model.glt_debit = nominalDb;
            model.glt_credit = nominalCr;
            model.glt_posted = "N";
            model.glt_dt = DateTime.Now;
            model.glt_branch_id = 10001;
            model.glt_driver_id = soitem.SalesOrderOncallId.HasValue ? (soitem.SalesOrderOncall.Driver1Id.HasValue ? soitem.SalesOrderOncall.Driver1Id.Value + 7000000 : 0) : soitem.SalesOrderProsesKonsolidasiId.HasValue ? (soitem.SalesOrderProsesKonsolidasi.Driver1Id.HasValue ? soitem.SalesOrderProsesKonsolidasi.Driver1Id.Value + 7000000 : 0) : soitem.SalesOrderPickupId.HasValue ? (soitem.SalesOrderPickup.Driver1Id.HasValue ? soitem.SalesOrderPickup.Driver1Id.Value + 7000000 : 0) : 0;
            context.glt_det.Add(model);
            context.SaveChanges();
        }
        public void delete(string glt_ref_detail_no)
        {
            foreach (Context.glt_det dbitem in context.glt_det.Where(d => d.glt_ref_detail_no == glt_ref_detail_no))
            {
                context.glt_det.Remove(dbitem);
                context.SaveChanges();
            }
        }

        public List<Context.glt_det> FindAllByglt_codeAndglt_ref_detail_no(string glt_code, string glt_ref_detail_no)
        {
            return context.glt_det.Where(d => d.glt_code == glt_code && d.glt_ref_detail_no == glt_ref_detail_no).ToList();
        }

        public List<Context.glt_det> FindAllByglt_desc(string glt_desc)
        {
            return context.glt_det.Where(d => d.glt_desc == glt_desc).ToList();
        }
    }
}