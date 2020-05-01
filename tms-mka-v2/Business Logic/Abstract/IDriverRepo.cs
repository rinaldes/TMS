using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IDriverRepo
    {
        void save(Driver dbitem);
        List<Driver> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Driver FindByPK(int id);
        Driver FindByCode(string code);
        void delete(Driver dbitem);
        string generateCode(DateTime tglGabung, int urutan);
        int getUrutan(DateTime tglGabung);
        bool IsKtpExist(string ktp, int id = 0);
        bool IsSimExist(string sim, int id = 0);
    }
}