using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace QuartZTest
{
    public class TestJobScheduler
    {
        private static IScheduler scheduler;
        public static void StartAllJobs()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            RunHelloJob().GetAwaiter().GetResult();
            new PingJob().RunPingJob(scheduler).GetAwaiter().GetResult();

            // and start it off
            scheduler.Start().GetAwaiter().GetResult();
        }

        public static bool ShutDownAllJobs()
        {
            if(scheduler == null) return false;
            scheduler.Shutdown(true).GetAwaiter().GetResult();
            return true;
        }

        private static async Task RunHelloJob()
        {
            try
            {
                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("helloJob", "HelloGroup")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("helloTrigger", "HelloGroup")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
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

        /// <summary>
        /// Only Job in running state will be logged
        /// </summary>
        /// <param name="scheduler"></param>
        public static IList<RunningJobInfo> GetCurrentlyRunningJobs()
        {
            IList<RunningJobInfo> jobs = new List<RunningJobInfo>();
            if (scheduler != null)
            {
                foreach (var job in scheduler.GetCurrentlyExecutingJobs().Result)
                {
                    string triggeredCount = job.RefireCount.ToString();
                    jobs.Add(new RunningJobInfo()
                    {
                        JobKey = job.JobDetail.Key.ToString(),
                        JobType = job.JobDetail.JobType.ToString(),
                        TriggerKey = job.Trigger.Key.ToString(),
                        FireTime = job.FireTimeUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        PreviousFireTime = job.PreviousFireTimeUtc.HasValue ? job.PreviousFireTimeUtc.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff") : "",
                        RefireCount = GetJobTriggeredCount(job)
                    });
                    
                }
            }
            return jobs;
        }

        private static string GetJobTriggeredCount(IJobExecutionContext job)
        {
            var simpleTrigger = job.Trigger as SimpleTriggerImpl;
            if (simpleTrigger != null)
            {
                return simpleTrigger.TimesTriggered.ToString();
            }
            return job.RefireCount.ToString();
        }
    }
}
