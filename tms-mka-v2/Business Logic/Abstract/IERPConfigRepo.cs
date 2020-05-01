using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface IERPConfigRepo
    {
        void save(ERPConfig dbitem);
        ERPConfig FindByFrist();
        ERPDynamicConfig FindByIdAndType(int id, string type);
        void saveDynamic(int id, int ac_id, string type);
    }
}