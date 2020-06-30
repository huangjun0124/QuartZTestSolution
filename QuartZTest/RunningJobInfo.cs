using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartZTest
{
    public class RunningJobInfo
    {
        public string JobKey { get; set; }
        public string JobType { get; set; }
        public string TriggerKey { get; set; }
        public string FireTime { get; set; }
        public string PreviousFireTime { get; set; }
        public string RefireCount { get; set; }
    }
}
