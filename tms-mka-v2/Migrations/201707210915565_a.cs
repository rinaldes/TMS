namespace tms_mka_v2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using tms_mka_v2.Context;
   
    public partial class a : DbMigration
    {
        public override void Up()
        {
            List<Menu> ListMenu = new List<Menu>();
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Input DP" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Batal Truck" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Batal Order" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Revisi Rute" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Revisi Tanggal" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Revisi Jenis Truck" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Klaim" });
        }
        
        public override void Down()
        {
        }
    }
}
