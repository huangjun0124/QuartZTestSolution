using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace WebMvcSite.Controllers
{
    public class HomeController : Controller
    {
        private ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            Logger.Info("Home:Index visited...");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            Logger.Info("Home:About visited...");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            Logger.Info("Home:Contact visited...");
            return View();
        }
    }
}