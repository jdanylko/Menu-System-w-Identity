using System;
using System.Collections.Generic;
using System.Linq;
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
            var menuManager = new MenuManager(new ApplicationDbContext());

            var username = GetUserName();

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var model = new MenuViewModel();

            var user = userManager.FindByName(username);
            if (user != null)
            {
                model.MenuItems = menuManager.GetAllByUser(user).ToList();
            }

            return View(model);
        }

        [NonAction]
        private string GetUserName()
        {
            return this.ControllerContext.HttpContext.User.Identity.Name;
        }
    }
}