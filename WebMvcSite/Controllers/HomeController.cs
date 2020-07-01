using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using QuartZTest;

namespace WebMvcSite.Controllers
{
    public class HomeController : Controller
    {
        private ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            Logger.Info("Home:Index visited...");
            var jobs = TestJobScheduler.GetCurrentlyRunningJobs();
            for (int i = 0; i < 6; i++)
            {
                jobs.Add(new RunningJobInfo()
                {
                    JobKey = $"FakeJob_[{i}]",
                    TriggerKey = $"FakeTrigger_[{i}]",
                    JobType = "Fake Job, Fill for show UI",
                    FireTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                });
            }
            string[] htmlClass = new[] {"success", "danger", "info", "warning", "active"};
            int roundIndex = 0;
            foreach (var runningJobInfo in jobs)
            {
                runningJobInfo.HtmlClass = htmlClass[roundIndex];
                roundIndex = (++roundIndex) % htmlClass.Length;
            }
            return View(jobs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            Logger.Info("Home:About visited...");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me at keithhuang0124@gmail.com";
            Logger.Info("Home:Contact visited...");
            return View();
        }

        [HttpPost]
        public ActionResult ShutDownAllJobs(FormCollection collection)
        {
            string token = collection.Get("token");
            Logger.Info("ShutDownAllJobs method invoked");
            return Json("ShutDown All Jobs successfully, please go back home page to see recently status");
        }
    }
}