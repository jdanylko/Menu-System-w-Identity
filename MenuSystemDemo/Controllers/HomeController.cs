using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using MenuDemo.Repository;
using MenuSystemDemo.GeneratedClasses;
using MenuSystemDemo.Identity;
using MenuSystemDemo.Repository;
using MenuSystemDemo.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MenuSystemDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = GetUserName();

            var model = new MenuViewModel();

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var appUser = userManager.FindByName(user.Identity.Name);
            if (appUser != null)
            {
                var menuManager = new MenuManager(new ApplicationDbContext());
                model.MenuItems = menuManager.GetAllByUser(appUser).ToList();
            }
            
            return View(model);
        }

        [NonAction]
        private IPrincipal GetUserName()
        {
            return this.ControllerContext.HttpContext.User;
        }
    }
}