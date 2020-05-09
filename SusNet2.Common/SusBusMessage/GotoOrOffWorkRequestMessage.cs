using SusNet2.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    /// <summary>
    /// 【XID+ID+CMD+XOR+ADDH+ADDL+DATA1+DATA2+DATA3+DATA4+DATA5+DATA6】
    /// 上下班【请求】
    /// </summary>
    public class GotoOrOffWorkRequestMessage : MessageBody
    {
        public GotoOrOffWorkRequestMessage(byte []resBytes) : base(resBytes) { }
    }
}
