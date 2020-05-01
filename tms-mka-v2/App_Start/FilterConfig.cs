using System.Web;
using System.Web.Mvc;
using tms_mka_v2.Security;

namespace tms_mka_v2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MyAuthorizeAttribute());
        }
    }
}