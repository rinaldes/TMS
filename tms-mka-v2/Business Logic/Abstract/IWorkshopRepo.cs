using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IWorkshopRepo
    {
        void save(Workshop dbitem, int id);
        List<Workshop> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Workshop FindByPK(int id);
        string generateCodePPK(DateTime tglGabung, int urutan);
        Workshop FindByNoPPK(string noPPK);
        string generateCodePrePPK(int urutan);
        int getUrutan();
        int getUrutanPPK(DateTime tglGabung);
        void delete(Workshop dbitem, int id);
        int CountTruck(string status);
        int CountPendingin(string status);
        int CountBox(string status);
        int CountBan(string status);
        int CountGPS(string status);
        int CountTruckSPKO();
        int CountPendinginSPKO();
        int CountBanSPKO();
        int CountBoxSPKO();
        int CountGPSSPKO();
        int CountTruckSPKP();
        int CountPendinginSPKP();
        int CountBanSPKP();
        int CountBoxSPKP();
        int CountGPSSPKP();
        int CountTruckSPKC();
        int CountPendinginSPKC();
        int CountBanSPKC();
        int CountBoxSPKC();
        int CountGPSSPKC();
    }
}