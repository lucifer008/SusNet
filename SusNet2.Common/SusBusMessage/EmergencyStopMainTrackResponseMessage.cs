using SusNet2.Common.Message;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    /// <summary>
    /// 【XID+ID+CMD+XOR+ADDH+ADDL+DATA1+DATA2+DATA3+DATA4+DATA5+DATA6】
    /// 急停主轨 【回复】:01 44 04 XX 00 06 00 00 00 00 00 12
    /// </summary>
    public class EmergencyStopMainTrackResponseMessage : MessageBody
    {
        public static EmergencyStopMainTrackResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 4; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if (hexStr.Replace(" ", "").Equals("0006000000000012") && "04".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })))
            {
                return new EmergencyStopMainTrackResponseMessage(resBytes);
            }
            return null;
        }

        public EmergencyStopMainTrackResponseMessage(byte[] resBytes):base(resBytes)
        { }
    }
}
