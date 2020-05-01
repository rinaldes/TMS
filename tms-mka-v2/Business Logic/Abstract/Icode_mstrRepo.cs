using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tms_mka_v2.Context;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Business_Logic.Abstract
{
    public interface Icode_mstrRepo
    {
        Models.code_mstr FindByCodeName(string vehicleNo);
        void saveVehicle(DataTruck dbitem);
        Context.code_mstr FindByCodeCode(string vehicleNo);
        void updateVehicle(code_mstr dbitem);
    }
}