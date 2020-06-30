using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Utilities;

namespace WebMvcSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ThreadPool.QueueUserWorkItem(x => PingMySelf());
        }

        private void PingMySelf()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                try
                {
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadStringTaskAsync("http://localhost:1807").GetAwaiter().GetResult();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.GetMessage());
                }
            }
        }
    }
}
