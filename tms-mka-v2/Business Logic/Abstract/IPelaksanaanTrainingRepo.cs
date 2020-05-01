using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IPelaksanaanTrainingRepo
    {
        void save(PelaksanaanTraining dbitem);
        List<PelaksanaanTraining> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        List<PelaksanaanTrainingDetail> FindAllDetail(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        PelaksanaanTraining FindByPK(int id);
        void delete(PelaksanaanTraining dbitem);
    }
}