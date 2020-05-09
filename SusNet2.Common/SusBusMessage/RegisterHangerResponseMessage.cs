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
    /// 注册衣架【响应】
    /// </summary>
    public class RegisterHangerResponseMessage : MessageBody
    {
        public RegisterHangerResponseMessage(byte[] resBytes) : base(resBytes)
        {
            this.CMD = "";
        }
    }
}
