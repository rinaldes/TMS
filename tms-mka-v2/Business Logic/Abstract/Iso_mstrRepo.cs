using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Models;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface Iso_mstrRepo
    {
        void saveSoMstr(SalesOrder soitem, string username, string guid, int customerId, decimal harga, Context.SalesOrderKontrakListSo sokls = null, Context.AdminUangJalan auj = null, string codeSoMstr = null);
        void saveSoDet(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null, decimal priceKonsolidasi = 0, Context.AdminUangJalan auj = null);
        void saveSoShipMstr(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null, string soNumber = null);
        void saveSoShipDet(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null);
		so_mstr FindByPK(string code);
        soship_mstr FindSoShipBySo(string soship_so_oid);
        sod_det FindSodDetBySo(string sod_so_oid);
		so_mstr FindSoDet(string code);
		void UpdateSoMstrVehicle(so_mstr dbitem);
        void saveSoShipMstrSolarInap(SalesOrder soitem, string username, string guid, string ship_guid, Context.SalesOrderKontrakListSo sokls = null, Context.SolarInap solarInap = null);
        string saveSoMstrSolarInap(SalesOrder soitem, string username, string guid, int customerId, decimal harga, Context.SalesOrderKontrakListSo sokls = null, Context.SolarInap solarInap = null);
        void saveSoDetSolarInap(SalesOrder soitem, string username, string guid, string ship_guid, decimal price);
    }
}