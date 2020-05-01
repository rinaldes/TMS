using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using tms_mka_v2.Security;
using Newtonsoft.Json;
using System.Web.Optimization;
using tms_mka_v2.Infrastructure;
using tms_mka_v2.Services.Config;
namespace tms_mka_v2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependencyResolver.SetResolver(new tms_mka_v2.Infrastructure.NinjectDependencyResolver());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SchedulerConfig.Start();

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "MyGlobalErrors";
            DefaultModelBinder.ResourceClassKey = "MyGlobalErrors";

            ModelBinders.Binders.Add(typeof(DateTime?), new DateCustomBinder());
            ModelBinders.Binders.Add(typeof(Decimal?), new DecimalCustomBinder());
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    MyPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<MyPrincipalSerializeModel>(authTicket.UserData);
                    MyPrincipal newUser = new MyPrincipal(authTicket.Name);

                    newUser.id = serializeModel.id;
                    newUser.username = serializeModel.username;
                    newUser.password = serializeModel.password;
                    newUser.firstname = serializeModel.firstname;
                    newUser.lastname = serializeModel.lastname;
                    newUser.path_foto = serializeModel.path_foto;

                    newUser.menus = new List<PrincipalMenu>();
                    newUser.RoleUser = new List<string>();
                    tms_mka_v2.Context.ContextModel dbcontext = new tms_mka_v2.Context.ContextModel();
                    tms_mka_v2.Context.User dbuser = dbcontext.User.Where(u => u.Id == newUser.id).FirstOrDefault();

                    foreach (var _menu in dbuser.UserMenus)
                    {
                        PrincipalMenu _menuUser = new PrincipalMenu();
                        _menuUser.MenuName = _menu.Menu.MenuName;
                        _menuUser.Action = new List<string>();
                        if (_menu.IsCreate)
                            _menuUser.Action.Add("create");
                        if (_menu.IsRead)
                            _menuUser.Action.Add("read");
                        if (_menu.IsUpdate)
                            _menuUser.Action.Add("update");
                        if (_menu.IsDelete)
                            _menuUser.Action.Add("delete");
                        if (_menu.IsPrint)
                            _menuUser.Action.Add("print");
                        if (_menu.IsProses)
                            _menuUser.Action.Add("proses");
                        newUser.menus.Add(_menuUser);
                    }

                    foreach (var _role in dbuser.UserRole)
                    {
                        newUser.RoleUser.Add(_role.Role.RoleName);
                    }
                    
                    HttpContext.Current.User = newUser;
                }
                catch (Exception)
                {
                    //SignOut();
                    //return;
                }
            }
        }
        private void SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect(FormsAuthentication.LoginUrl, true);
        }
    }
}
