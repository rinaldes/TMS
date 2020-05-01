using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDataTruckRepo
    {
        void save(DataTruck dbitem, int id);
        List<DataTruck> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        DataTruck FindByPK(int? id);
        DataTruck FindByName(string name);
        void delete(DataTruck dbitem, int id);
        bool IsValidDelete(int id);
        string generateCode(int urutan);
        int getUrutan();
        bool IsExist(string vehicleNo, int id = 0);
    }
}