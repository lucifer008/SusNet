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
    /// 急停主轨道【请求】01 00 03 XX 00 37 00 00 00 00 00 12
    /// </summary>
    public class EmergencyStopMainTrackRequestMessage : MessageBody
    {
        public EmergencyStopMainTrackRequestMessage(string mainTrackNo, string xor = null)
        {
            XID = mainTrackNo;
            ID = "00";
            CMD = "03";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            ADDH = "00";
            ADDL = "37";
            DATA1 = "00";
            DATA2 = "00";
            DATA3 = "00";
            DATA4 = "00";
            DATA5 = "00";
            DATA6 = "12";
        }
        public static EmergencyStopMainTrackRequestMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 4; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if (hexStr.Replace(" ", "").Equals("0037000000000012") && "03".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })))
            {
                return new EmergencyStopMainTrackRequestMessage(resBytes);
            }
            return null;
        }

        public EmergencyStopMainTrackRequestMessage(byte[] resBytes):base(resBytes)
        { }
    }
}
