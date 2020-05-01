using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tms_mka_v2.Models;
using tms_mka_v2.Context;
using tms_mka_v2.Security;
using tms_mka_v2.Business_Logic.Abstract;
using System.Web.Script.Serialization;
using Npgsql;
using System.Data;

namespace tms_mka_v2.Controllers
{
    public class BaseController : Controller
    {
        protected IUserReferenceRepo RepoUserReference;
        protected ILookupCodeRepo RepoLookup;
        public BaseController(IUserReferenceRepo repoUserReference, ILookupCodeRepo repoLookup)
        {
            RepoLookup = repoLookup;
            RepoUserReference = repoUserReference;
        }

        public void queryManual(string query)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsDefault"].ConnectionString);
            con.Open();
            using (DataTable dt = new DataTable())
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Dispose();
                con.Close();
            }
        }

        public void queryManualERP(string query)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tmsERP"].ConnectionString);
            con.Open();
            using (DataTable dt = new DataTable())
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Dispose();
                con.Close();
            }
        }

        protected virtual MyPrincipal UserPrincipal
        {
            get { return HttpContext.User as MyPrincipal; }
        }
        protected virtual List<Context.UserReference> ListKolom
        {
            get { return RepoUserReference.FindByUser(UserPrincipal.id); }
        }
        public ResponeModel saveReference(string act, string contr,string kolom, string HideShow)
        {
            ResponeModel model = new ResponeModel();

            Context.UserReference dbitem = RepoUserReference.find(UserPrincipal.id, act, contr, kolom) != null ? RepoUserReference.find(UserPrincipal.id, act, contr, kolom) : new Context.UserReference();
            dbitem.Action = act;
            dbitem.Controller = contr;
            dbitem.Coloumn = kolom;
            dbitem.HideShow = HideShow;
            dbitem.IdUser = UserPrincipal.id;

            RepoUserReference.save(dbitem, UserPrincipal.id);
            return model;
        }
        public string GetCustomerKonsolidasi()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_customer_konsolidasi").ToList());
        }
        public string GetKeteranganBatal()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_keterangan").ToList());
        }
        public string GetBank()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_bank").ToList());
        }
        public string GetNamaBiaya()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_biaya_tambahan_settlement_reguler").ToList());
        }
        public string GetNamaBiayaLain()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_biaya_tambahan_settlement_reguler" && d.DipakaidiAUJ).ToList());
        }
        public string GetProductKategori()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "produk_kategori").ToList());
        }
        public string GetNamaBarang()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_inventaris_nama_barang").ToList());
        }
        public string GetPrioritas()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_prioritas").ToList());
        }
        public string GetSPKPrioritas()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_spk_prioritas").ToList());
        }
        public string GetDepartment()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_departemen").ToList());
        }
        public string GetJabatan()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_jabatan").ToList());
        }
        public string GetOfficeType()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_officetype").ToList());
        }
        public string GetZone()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_zona").ToList());
        }
        public string GetBagian()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_bagian").ToList());
        }
        public string GetGrade()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_grade").ToList());
        }
        public string GetMerk()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_merk").ToList());
        }
        public string GetTypeKontrak()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_tipe_kontrak").ToList());
        }
        public string GetUnit()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_unit").ToList());
        }
        public string GetBoxType()
        {
	    return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_box_type").ToList());        
        }
        public string GetJenisPendingin()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_jenis_pendingin").ToList());
        }
        public string GetBoxKategori()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_box_kategori").ToList());
        }
        public string GetJenisSim()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_jenisSim").ToList());
        }
        public string GetLevelDriver()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_level_driver").ToList());
        }
        public string GetSPBU()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_SPBU").ToList());
        }
        public string GetKapal()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_penyebrangan").ToList());
        }
        public string GetRefCode()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_referensi_driver").ToList());
        }
        public string GetStatusDriver()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_status_driver").ToList());
        }
        public string GetSoPrioritas()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_so_prioritas").ToList());
        }
        public string GetKonsolidasiType()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_konsolidasi_type").ToList());
        }
        public string GetBeratMinimumTonase()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_minimum_tonase").ToList());
        }
        public string GetBeratMinimumKarton()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_minimum_karton").ToList());
        }
        public string GetBeratMinimumPallet()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_minimum_pallet").ToList());
        }
        public string GetBeratMinimumContainer()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_minimum_container").ToList());
        }
        public string GetBeratMinimumM3()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_minimum_m3").ToList());
        }
        public string GetSatuan()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_satuan_daftar_harga").ToList());
        }
        public string GetStatusKlaim()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_status_klaim").ToList());
        }
        public string GetSumberKlaim()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_sumber_klaim").ToList());
        }
        public string GetClaimType()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_kategori_klaim").ToList());
        }
        public string GetComboStatusBAP()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_status_bap").ToList());
        }
        public string GetComboKategoriBAP()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_category_bap").ToList());
        }
        public string GetComboDepartemenBAP()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_departemen").ToList());
        }
        public List<Context.LookupCode> GetListProductTreatment()
        {
            return RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "produk_treatments").ToList();
        }
        public List<Context.LookupCode> GeListtWarna()
        {
            return RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_warna").ToList();
        }
        public List<Context.LookupCode> GetListLantaiBox()
        {
            return RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_lantai_box").ToList();
        }
        public List<Context.LookupCode> GetListDindingBox()
        {
            return RepoLookup.FindAll().Where(d => d.LookupCodeCategories.Category == "tms_dinding_box").ToList();
        }
        //public List<Context.LookupCode> GetSatuanHarga()
        public string GetSatuanHarga()
        {
            return new JavaScriptSerializer().Serialize(RepoLookup.FindAll().Where(
                d => d.LookupCodeCategories.Category == "tms_satuan_daftar_harga").ToList().Select(i => new { IdSatuanHarga = i.Id, SatuanHarga = i.Nama } )
            );
        }
    }
}