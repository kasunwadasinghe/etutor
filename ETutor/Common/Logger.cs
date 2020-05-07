using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.Common
{
    public class Logger
    {
        private static readonly ILog l = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void log(string info,string debug)
        {
            l.Info(info);
            l.Debug(debug);
        }
    }
}