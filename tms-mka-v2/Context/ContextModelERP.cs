using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace tms_mka_v2.Context
{
    public class ContextModelERP : DbContext
    {
        public ContextModelERP()
            : base("tmsERP")
        {

        }

        //public DbSet<ERPConfig> ERPConfig { get; set; }
        public DbSet<glt_det> glt_det { get; set; }
        public DbSet<ac_mstr> ac_mstr { get; set; }
        public DbSet<gr_mstr> gr_mstr { get; set; }
        public DbSet<code_mstr> code_mstr { get; set; }
        public DbSet<bk_mstr> bk_mstr { get; set; }
        public DbSet<ptnr_mstr> ptnr_mstr { get; set; }
        public DbSet<ptnra_addr> ptnra_addr { get; set; }
        public DbSet<pbyd_det> pbyd_det { get; set; }
        public DbSet<pby_mstr> pby_mstr { get; set; }
        public DbSet<cashd_det> cashd_det { get; set; }
        public DbSet<so_mstr> so_mstr { get; set; }
        public DbSet<sod_det> sod_det { get; set; }
        public DbSet<soship_mstr> soship_mstr { get; set; }
        public DbSet<soshipd_det> soshipd_det { get; set; }
        public DbSet<Auditrail> Auditrail { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("public");
        }
    }
}