using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.Utils
{
    public class DateUtils
    {
        public static string GetCurrentDateTime() {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss");
        }
    }
}
