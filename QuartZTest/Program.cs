using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace QuartZTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            new TestJobScheduler().RunHelloJob(scheduler).GetAwaiter().GetResult();
            new PingJob().RunPingJob(scheduler).GetAwaiter().GetResult();

            // and start it off
            scheduler.Start().GetAwaiter().GetResult();

            // some sleep to show what's happening
            Task.Delay(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
            LogRunningJobs(scheduler);
            Task.Delay(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
            LogRunningJobs(scheduler);
            Task.Delay(TimeSpan.FromSeconds(18)).GetAwaiter().GetResult();
            LogRunningJobs(scheduler);
            // and last shut down the scheduler when you are ready to close your program
            scheduler.Shutdown().GetAwaiter().GetResult();
            LogRunningJobs(scheduler);
            Console.WriteLine("Press any key to close the application");

            while (true)
            {
                Console.ReadLine();
                // Quartz.SchedulerException: 'The Scheduler cannot be restarted after Shutdown() has been called.'

                //scheduler.Start().GetAwaiter().GetResult();
                LogRunningJobs(scheduler);
            }
        }

        /// <summary>
        /// Only Job in running state will be logged
        /// </summary>
        /// <param name="scheduler"></param>
        private static void LogRunningJobs(IScheduler scheduler)
        {
            StringBuilder sb = new StringBuilder($"{Environment.NewLine}Running Jobs:{Environment.NewLine}");
            foreach (var job in scheduler.GetCurrentlyExecutingJobs().Result)
            {
                sb.Append($"JobKey:[{job.JobDetail.Key.ToString()}], ");
                sb.Append($"JobType:[{job.JobDetail.JobType}], ");
                sb.Append($"TriggerKey:[{job.Trigger.Key}], ");
                sb.AppendLine($"FireTime:[{job.FireTimeUtc.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff}] ");
            }
            Console.WriteLine(sb);
        }
    }
}
