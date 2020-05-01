using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface ICustomerRepo
    {
        void save(Customer dbitem);
        List<Customer> FindAll(int? skip = null, int? take = null, List<SortingInfo> sortings = null, FilterInfo filters = null);
        int Count(FilterInfo filters = null);
        Customer FindByPK(int id);
        Customer FindByCode(string Code);
        void delete(Customer dbitem, int id);
        bool IsExist(string code, int id = 0);
        string generateCode(int urutan);
        int getUrutan();
        Customer FindByName(string Code);
    }
}