using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        /// <summary>
        /// Website home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        /// <summary>
        /// About page
        /// </summary>
        /// <param name="id">
        /// 
        /// Some Identifier
        /// 
        /// </param>
        /// <returns></returns>
        public ActionResult About(string id)
        {
            return View();
        }
    }
}
