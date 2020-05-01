using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace tms_mka_v2.Security
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public string Menu { get; set; }
        public string Action { get; set; }

        protected virtual MyPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as MyPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (Menu != null && Menu != "")
                {
                    if (!CurrentUser.HasActionAccess(Menu,Action))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }                
                }
                else
                {
                    base.OnAuthorization(filterContext);
                }
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}