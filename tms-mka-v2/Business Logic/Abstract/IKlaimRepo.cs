using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IKlaimRepo
    {
        void save(Klaim dbitem, int id);
        List<Klaim> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Klaim FindByPK(int id);
        Klaim FindByNoKlaim(string noKlaim);
        void delete(Klaim dbitem, int id);
        bool IsExist(string nama, int id = 0);
        string GenerateCode(DateTime valdate, int urutan);
        int getUrutanOnCAll(DateTime valdate);
        Klaim FindBySoId(string soNumber);
    }
}