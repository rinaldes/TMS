using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IFaktorBoronganRepo
    {
        void save(FaktorBorongan dbitem, int id, FaktorBoronganHistory fbh);
        List<FaktorBorongan> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        FaktorBorongan FindByPK(int id);
        void delete(FaktorBorongan dbitem, int id);
        bool isExist(int idPool, int idJnsTruck, int id = 0);
    }
}