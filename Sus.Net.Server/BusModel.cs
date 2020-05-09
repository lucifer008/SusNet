using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sus.Net.Server
{
    public class BusModel
    {

    }
    public class StatingInfo
    {
        public int Index { set; get; }
        public string MainTrackNo { set; get; }
        public string StatingNo { set; get; }
        /// <summary>
        /// 0:正常;1:不进站
        /// </summary>
        public int Tag { set; get; }
    }
}
