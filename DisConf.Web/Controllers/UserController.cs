using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisConf.Web.Controllers.Base;
using DisConf.Web.Models.User;
using DisConf.Web.Service.Interfaces;

namespace DisConf.Web.Controllers
{
    public class UserController : PrivateController
    {
        #region View

        [HttpGet]
        public ActionResult Users(int pageIndex = 1, int pageSize = 10)
        {
            var service = base.ResolveService<IUserService>();
            var result = service.GetByCondition(pageIndex, pageSize);

            return View(result);
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        #endregion

        #region Post

        [HttpPost]
        public ActionResult CreateUser(CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUser", model);
            }

            var service = base.ResolveService<IUserService>();
            var result = service.CreateUser(model.UserName, model.Password);

            if (result.HasError)
            {
                ViewBag.Error = result.Error.Message;
                return View("CreateUser", model); 
            }

            return RedirectToRoute("Users");
        }

        #endregion
    }
}