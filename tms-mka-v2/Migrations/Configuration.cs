using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Collections.Generic;
using tms_mka_v2.Context;

namespace tms_mka_v2.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ContextModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ContextModel context)
        {
            //role
            List<Role> ListRole = new List<Role>();
            ListRole.Add(new Role { RoleName = "Superadmin" });
            ListRole.Add(new Role { RoleName = "DM" });
            ListRole.Add(new Role { RoleName = "Marketing" });
            ListRole.Add(new Role { RoleName = "Operasional" });
            context.Role.AddOrUpdate(p => p.RoleName, ListRole.ToArray());

            //menu
            List<Menu> ListMenu = new List<Menu>();
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Basic Data" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer PIC" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Address" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Product Type" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Loading Address" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Unloading Address" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Supplier" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Billing" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer PPN" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Credit Status" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Notification" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Attachment" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Customer Truck Type" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Produk" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Jenis Truk" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Vendor Gps" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Data Truk" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Data Pendingin" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Data Box" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Data Gps" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Unit List" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Rekening" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Lookup" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Solar" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Faktor Borongan" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Data Borongan" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Area" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Tol" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Rute Tol" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Rute" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Pool" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "ATM" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Lokasi" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Multidrop" });
            ListMenu.Add(new Menu { Modul = "Master", MenuName = "Mekanik" });

            ListMenu.Add(new Menu { Modul = "DM", MenuName = "Driver Batangan" });

            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Daftar Harga Oncall" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Daftar Harga Kontrak" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Daftar Harga Konsolidasi" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Sales Order Oncall" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Sales Order Kontrak" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Sales Order Konsolidasi Pickup" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Sales Order Konsolidasi Daftar Barang" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Sales Order Proses Konsolidasi" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "List Order" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Input DP" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Batal Truck" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Batal Order" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Revisi Rute" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Revisi Tanggal" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Revisi Jenis Truck" });
            ListMenu.Add(new Menu { Modul = "Marketing", MenuName = "Klaim" });

            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Calendar View" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Monitoring" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Planning Oncall" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Planning Kontrak" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Konfirmasi Planning Oncall" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Konfirmasi Planning kontrak" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Admin Uang Jalan" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Settlement Reguler" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Settlement Batal" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Dokumen Surat Jalan" });
            ListMenu.Add(new Menu { Modul = "Operasional", MenuName = "Dokumen Billing" });

            ListMenu.Add(new Menu { Modul = "HR", MenuName = "Data Driver" });
            ListMenu.Add(new Menu { Modul = "HR", MenuName = "Training Setting" });
            ListMenu.Add(new Menu { Modul = "HR", MenuName = "Pelaksanaan Training" });

            ListMenu.Add(new Menu { Modul = "GA", MenuName = "BAP" });

            ListMenu.Add(new Menu { Modul = "Finance", MenuName = "Kasir Transfer" });
            ListMenu.Add(new Menu { Modul = "Finance", MenuName = "Kasir Cash" });

            ListMenu.Add(new Menu { Modul = "Workshop", MenuName = "Perbaikan Kendaraan" });
            ListMenu.Add(new Menu { Modul = "Workshop", MenuName = "Mekanik" });

            ListMenu.Add(new Menu { Modul = "Customer Service", MenuName = "Klaim" });
            ListMenu.Add(new Menu { Modul = "Customer Service", MenuName = "Tiket" });

            ListMenu.Add(new Menu { Modul = "Solar Inap", MenuName = "DM" });
            ListMenu.Add(new Menu { Modul = "Solar Inap", MenuName = "Marketing" });
            ListMenu.Add(new Menu { Modul = "Solar Inap", MenuName = "Admin" });
            ListMenu.Add(new Menu { Modul = "Solar Inap", MenuName = "Kasir" });

            ListMenu.Add(new Menu { Modul = "Setting", MenuName = "General Setting" });
            ListMenu.Add(new Menu { Modul = "Setting", MenuName = "Audit Trail" });
            ListMenu.Add(new Menu { Modul = "Setting", MenuName = "Time Alert" });
            ListMenu.Add(new Menu { Modul = "Setting", MenuName = "ERP Config" });

            ListMenu.Add(new Menu { Modul = "User Management", MenuName = "User" });
            ListMenu.Add(new Menu { Modul = "User Management", MenuName = "Role" });

//            context.Menu.AddOrUpdate(p => p.MenuName, ListMenu.ToArray());


            try
            {
  //              context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}