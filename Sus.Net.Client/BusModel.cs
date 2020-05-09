using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sus.Net.Client
{
    public class BusModel
    {

    }
    public class StatingInfo
    {
        public int Index { get; set; }
        public string MainTrackNo { get; set; }
        public int MainTrackNumber { set; get; }
        public string StatingNo { set; get; }
        /// <summary>
        /// 0:正常;1:不进站
        /// </summary>
        public int Tag { set; get; }
    }
}
