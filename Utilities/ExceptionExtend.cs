using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ExceptionExtend
    {
        public static string GetMessage(this Exception e)
        {
            StringBuilder sb = new StringBuilder();
            GetMessage(sb, e);
            return sb.ToString();
        }

        private static void GetMessage(StringBuilder sb, Exception e)
        {
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);
            if (e.InnerException != null)
            {
                GetMessage(sb, e.InnerException);
            }
        }
    }
}
