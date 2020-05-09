using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspeSys.Utils
{
    public class SusLog
    {
        protected static readonly ILog tcpLogInfo = LogManager.GetLogger("TcpLogInfo");
        protected static readonly ILog tcpLogError = LogManager.GetLogger("TcpErrorInfo");
        protected static readonly ILog tcpLogHardware = LogManager.GetLogger("TcpHardwareInfo");
    }
}
