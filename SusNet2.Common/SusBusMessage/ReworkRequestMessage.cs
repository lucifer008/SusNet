using Sus.Net.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    /// <summary>
    /// 【XID+ID+CMD+XOR+ADDH+ADDL+DATA1+DATA2+DATA3+DATA4+DATA5+DATA6】
    ///01 44 06 XX 00 54 00 AA BB CC DD EE 
    /// 返工【硬件请求-->pc】
    /// </summary>
    public class ReworkRequestMessage : MessageBody
    {

    }
}
