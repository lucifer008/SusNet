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
    /// 上班【响应】
    /// </summary>
    public class GotoWorkResponseMessage : MessageBody
    {

        public GotoWorkResponseMessage(byte[] resBytes) : base(resBytes) { }
    }
}
