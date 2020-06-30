using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;

namespace QuartZTest
{
    /// <summary>
    /// tells Quartz not to execute multiple instances of a given job definition (that refers to the given job class) concurrently. Notice the wording there, as it was chosen very carefully. In the example from the previous section, if “SalesReportJob” has this attribute, than only one instance of “SalesReportForJoe” can execute at a given time, but it can execute concurrently with an instance of “SalesReportForMike”. The constraint is based upon an instance definition (JobDetail), not on instances of the job class.
    /// </summary>
    [DisallowConcurrentExecution]
    public class HelloJob : IJob
    {
        private ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            Logger.Info("Greetings from HelloJob!");
        }
    }
}
