
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
    /// 制单绑定到挂片站完成时响应【 硬件--->pc 】
    /// 
    /// </summary>
    public class BindProcessOrderHangingPieceResponseMessage : MessageBody
    {

    }
}
