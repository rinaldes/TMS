using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDataBoxRepo
    {
        void save(DataBox dbitem, int id, DataBoxHistory dbh, DataTruckBoxHistory dtbh=null, DataTruckGPSHistory dtgh=null);
        List<DataBox> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        DataBox FindByPK(int id);
        DataBox FindByTruck(int id);
        void delete(DataBox dbitem, int id);
        bool IsBoxExist(int idtruck, int id = 0);
        string generateCode(int urutan);
        int getUrutan();
    }
}