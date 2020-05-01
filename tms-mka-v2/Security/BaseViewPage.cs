using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tms_mka_v2.Security
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new MyPrincipal User
        {
            get { return base.User as MyPrincipal; }
        }
    }
    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new MyPrincipal User
        {
            get { return base.User as MyPrincipal; }
        }
    }
}