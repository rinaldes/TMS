using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IHistoryJalanTruckRepo
    {
        void save(HistoryJalanTruck dbitem);
        List<HistoryJalanTruck> FindByDriver(int idDriver);
        List<HistoryJalanTruck> FindByTruck(int idTruck);
        HistoryJalanTruck FindByAdm(int idAdm);
    }
}