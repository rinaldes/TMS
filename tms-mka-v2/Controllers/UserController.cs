using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using Newtonsoft.Json;
using tms_mka_v2.Business_Logic.Abstract;
using tms_mka_v2.Models;
using tms_mka_v2.Security;
using tms_mka_v2.Infrastructure;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace tms_mka_v2.Controllers
{
    public class UserController : BaseController 
    {
        private IUserRepo RepoUser;
        private IRoleRepo RepoRole;
        private IMenuRepo RepoMenu;
        public UserController(IUserReferenceRepo repoBase,  ILookupCodeRepo repoLookup,
            IUserRepo repoUser, IRoleRepo repoRole, IMenuRepo repoMenu)
            : base(repoBase, repoLookup)
        {   
            RepoUser = repoUser;
            RepoRole = repoRole;
            RepoMenu = repoMenu;
        }
        [MyAuthorize(Menu = "User", Action="read")]
        public ActionResult Index()
        {
            ViewBag.listKolom = ListKolom.Where(d=>d.Action == "Index" && d.Controller == "User").ToList();
            return View();
        }
        public string Binding()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.User> items = RepoUser.FindAll(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<User> ListModel = new List<User>();
            foreach (Context.User item in items)
            {
                ListModel.Add(new User(item));
            }

            int total = RepoUser.Count(param.Filters);

            return new JavaScriptSerializer().Serialize(new { total = total, data = ListModel });
        }
        public string BindingName()
        {
            GridRequestParameters param = GridRequestParameters.Current;

            List<Context.User> items = RepoUser.FindAllName(param.Skip, param.Take, (param.Sortings != null ? param.Sortings.ToList() : null), param.Filters);

            List<User> ListModel = new List<User>();
            foreach (Context.User item in items)
            {
                ListModel.Add(new User(item));
            }

            int total = RepoUser.Count(param.Filters);

            var query = from allName in ListModel
                        select new UserShow() { Id = allName.Id, Username = allName.Username };

            return new JavaScriptSerializer().Serialize(new { total = total, data = query });
        }
        [MyAuthorize(Menu = "User", Action="create")]
        public ActionResult Add()
        {
            User model = new User();
            initRole(model);
            return View("Form",model);
        }
        [HttpPost]
        public async Task<ActionResult> Add(User model)
        {
            if (ModelState.IsValid)
            {
                //more validation
                bool IsExist = RepoUser.IsExist(model.Username);

                if (IsExist)
                {
                    ModelState.AddModelError("Nik", "Nik & Username sudah terdaftar.");
                    ModelState.AddModelError("Username", "Nik & Username sudah terdaftar");
                    initRole(model);
                    return View("Form", model);
                }

                if (!model.ListRole.Any(d => d.isselect.Equals(true)))
                {
                    ModelState.AddModelError("roles", "Pilih salah satu atau lebih.");
                    initRole(model);
                    return View("Form", model);
                }

                Context.User dbitem = new Context.User();
                model.setDb(dbitem);
                dbitem.Password = Encrypt(model.Password);
                //tambahan default menu user
                foreach (var item in RepoMenu.FindAll())
                {
                    dbitem.UserMenus.Add(new Context.UserMenus() { IdMenu = item.Id});
                }
                RepoUser.save(dbitem, UserPrincipal.id);
                string BodyEmail = string.Format("<BR/><BR/>Thank you for your registration," +
                    "<p>You already have an account with TMS. You can access  TMS any time by visiting:</p>" +
                    "<a href='"+Url.Action("Index", "User", new object { }, Request.Url.Scheme)+"'> TMS MKA</a>" +
                    "<p> Username : " + model.Username + "</p>" +
                    "<p> Password : " + Decrypt(dbitem.Password) + "</p>" +
                    "<BR/><BR/><BR/> Regards,<BR/><BR/> TMS MKA Team");
                if (model.Email != null)
                EmailHelper.SendEmail(model.Email, "Email Registration TMS MKA", BodyEmail);

                return RedirectToAction("EditAkses","User", new {@id=dbitem.Id});
            }
            initRole(model);
            return View("Form", model);
        }
        [MyAuthorize(Menu = "User", Action="update")]
        public ActionResult Edit(int id)
        {
            Context.User dbitem = RepoUser.FindByPK(id);
            User model = new User(dbitem);
            ViewBag.name = model.Username;
            initRole(model);
            setRole(model, dbitem);
            model.Password = Decrypt(model.Password);
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                Context.User dbitem = RepoUser.FindByPK(model.Id);
                dbitem.UserRole.Clear();
                model.setDb(dbitem);
                List<int?> idMenus = dbitem.UserMenus.Select(i => i.IdMenu).ToList();
                string query = "DELETE FROM dbo.\"UserRole\" WHERE \"IdUser\" = " + model.Id + ";";
                foreach (var item in RepoMenu.FindAll().Where(d => !idMenus.Contains(d.Id)))
                {
                    dbitem.UserMenus.Add(new Context.UserMenus() { IdMenu = item.Id });
                    query += "INSERT INTO dbo.\"UserMenus\" (\"IdUser\", \"IdMenu\") VALUES ( " + model.Id + ", " +  item.Id + ");";
                }
                RepoUser.save(dbitem, UserPrincipal.id, query);

                //send email
                //System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                //    new System.Net.Mail.MailAddress("tms.kamanggala.net", "TMS MKA Registration"),
                //    new System.Net.Mail.MailAddress("yudhasatria0808@gmail.com")
                //);
                //System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage("tms@kamanggala.net", "yudhasatria0808@gmail.com");
                //m.Subject = "Email Registration TMS MKA " + dbitem.Password;
                //m.Body = "";
                //m.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient("kamanggala.com", 110);
                //var credential = new NetworkCredential
                //{
                //    UserName = "tms.kamanggala.net",
                //    Domain = "kamanggala.com"
                //};
                //smtp.EnableSsl = false;
                //smtp.Credentials = credential;
                ////smtp.UseDefaultCredentials = false;
                ////smtp.Credentials = new NetworkCredential("tms.kamanggala.net", "", "kamanggala.com");
                ////smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                ////ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                ////await smtp.SendMailAsync(m);
                //smtp.Send(m);

                return RedirectToAction("Index");
            }
            initRole(model);
            return View("Form", model);
        }

        [MyAuthorize(Menu = "User", Action="update")]
        public ActionResult EditAkses(int id)
        {
            Context.User dbitem = RepoUser.FindByPK(id);
            User model = new User(dbitem);
            ViewBag.name = model.Username;
            return View("FormAkses", model);
        }

        [HttpPost]
        public ActionResult EditAkses(User model)
        {
            UserMenu[] result = JsonConvert.DeserializeObject<UserMenu[]>(model.StrMenu);
            model.ListMenu = result.ToList();
            Context.User dbitem = RepoUser.FindByPK(model.Id);
            dbitem.UserMenus.Clear();
            var query = "DELETE FROM dbo.\"UserMenus\" WHERE \"IdUser\" = " + dbitem.Id + ";";
            foreach (var item in model.ListMenu)
            {
                dbitem.UserMenus.Add(item.setDb(new Context.UserMenus()));
                query += "INSERT INTO dbo.\"UserMenus\" (\"IdUser\", \"IdMenu\", \"IsCreate\", \"IsRead\", \"IsUpdate\", \"IsDelete\", \"IsPrint\", \"IsProses\") VALUES (" + model.Id + ", " + item.IdMenu +
                    ", " + item.IsCreate + ", " + item.IsRead + ", " + item.IsUpdate + ", " + item.IsDelete + ", " + item.IsPrint + ", " + item.IsProses + ");";
            }
            RepoUser.save(dbitem, UserPrincipal.id, query);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            ResponeModel response = new ResponeModel(true);
            Context.User dbItem = RepoUser.FindByPK(id);
            dbItem.UserRole.Clear();
            dbItem.UserMenus.Clear();
            RepoUser.delete(dbItem, UserPrincipal.id);

            return Json(response);
        }

        public ActionResult ChangePassword(int id)
        {
            Context.User dbitem = RepoUser.FindByPK(id);
            User model = new User(dbitem);
            ViewBag.name = model.Username;
            initRole(model);
            setRole(model, dbitem);
            model.Password = Decrypt(model.Password);
            return View("ChangePassword", model);
        }

        [HttpPost]
        public ActionResult ChangePassword(User model)
        {
            Context.User dbitem = RepoUser.FindByPK(model.Id);
            dbitem.Password = Encrypt(model.Password);
            RepoUser.save(dbitem, UserPrincipal.id);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            User model = new User();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User model, string returnUrl = "")
        {
            Context.User _user = RepoUser.FindByUsername(model.Username);

            if (_user != null)
            {
                if (Decrypt(_user.Password) != model.Password)
                {
                    ModelState.AddModelError("Password", "Password tidak cocok.");
                    return View(model);
                }

                //login succes
                //Models.User serializeModel = new Models.User(_user);
                MyPrincipalSerializeModel serializeModel = new MyPrincipalSerializeModel(_user);

                string userData = JsonConvert.SerializeObject(serializeModel);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                    serializeModel.username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(120),
                    true,
                    userData,
                    FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);

                return RedirectToAction("Index", "Home");
                //RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Username", "Username tidak terdaftar.");
                return View(model);
            }
        }
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User", null);
        }
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public void initRole(User model)
        {
            model.ListRole = new List<Role>();
            foreach (Context.Role item in RepoRole.FindAll())
            {
                model.ListRole.Add(new Role() { id = item.Id, name = item.RoleName, isselect = false});    
            }
        }
        private void setRole(User model, Context.User dbitem)
        {
            foreach (Context.UserRole d in dbitem.UserRole)
            {
                Role UserRole = model.ListRole.Find(ur => ur.id == d.IdRole);
                if (UserRole != null)
                { UserRole.isselect = true; }
            }
        }

        public string getUser()
        {
            List<User> listModel = new List<User>();
            foreach (Context.User item in RepoUser.FindAll())
            {
                listModel.Add(new User(item));
            }
            return new JavaScriptSerializer().Serialize(listModel);
        }
    }
}