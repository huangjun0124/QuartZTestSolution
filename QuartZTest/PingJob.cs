using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;

namespace QuartZTest
{
    public class PingJob : IJob
    {
        private ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task Execute(IJobExecutionContext context)
        {
            using (var webClient = new WebClient())
            {
               string reply = webClient.DownloadString(new Uri("http://worldclockapi.com/api/json/est/now"));
               Logger.Info($"Get Api Response:[{reply}]");
            }
        }

        public async Task RunPingJob(IScheduler scheduler)
        {
            try
            {
                IJobDetail job = JobBuilder.Create<PingJob>()
                    .WithIdentity("pingJob", "PingGroup")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("pingTrigger", "PingGroup")
                    .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Second))
                    .WithSimpleSchedule(x => x
                        
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}
