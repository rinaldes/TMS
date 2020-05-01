using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDataPendinginRepo
    {
        void save(DataPendingin dbitem, int id, DataPendinginHistory dph, DataTruckPendinginHistory dtph=null);
        List<DataPendingin> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        DataPendingin FindByPK(int id);
        DataPendingin FindByTruck(int id);
        void delete(DataPendingin dbitem, int id);
        bool IsBoxExist(int idtruck, int id = 0);
        string generateCode(int urutan);
        int getUrutan();
    }
}