using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IJnsTolRepo
    {
        void save(JnsTols dbitem, int id, HistoryJnsTols hjt);
        List<JnsTols> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        JnsTols FindByPK(int id);
        JnsTols FindByNamaTol(string namaTol);
        void delete(JnsTols dbitem, int id);
        bool IsExist(string nama, int id = 0);
    }
}