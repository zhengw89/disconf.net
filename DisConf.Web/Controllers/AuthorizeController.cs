using System.Web.Mvc;
using System.Web.Security;
using DisConf.Web.Models.Auth;
using DisConf.Web.Service.Interfaces;

namespace DisConf.Web.Controllers
{
    public class AuthorizeController : BaseController
    {
        #region View

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        #endregion

        #region Get

        [HttpGet]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToRoute("Login");
        }

        #endregion

        #region Post

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            var service = base.ResolveService<IUserService>();
            var user = service.GetByUserName(model.UserName);

            if (user.HasError)
            {
                ViewBag.Error = user.Error.Message;
                return View("Login", model);
            }
            else if (user.Data == null)
            {
                ViewBag.Error = "用户不存在";
                return View("Login", model);
            }
            else if (!user.Data.Password.Equals(model.Password))
            {
                ViewBag.Error = "密码错误";
                return View("Login", model);
            }

            FormsAuthentication.SetAuthCookie(user.Data.UserName, true);

            return RedirectToRoute("Apps");
        }

        #endregion
    }
}