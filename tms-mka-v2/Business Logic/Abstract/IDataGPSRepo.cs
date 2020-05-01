using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDataGPSRepo
    {
        void save(DataGPS dbitem, int id, DataGPSHistory dgh);
        List<DataGPS> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        DataGPS FindByPK(int id);
        DataGPS FindByTruck(int id);
        void delete(DataGPS dbitem, int id);
        bool IsBoxExist(int idtruck, int id = 0);
        string generateCode(int urutan);
        int getUrutan();
    }
}