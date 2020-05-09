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
    /// 01 00 03 XX 00 06 00 00 00 00 00 11
    /// 停止主轨【请求】
    /// </summary>
    public class StopMainTrackRequestMessage : MessageBody
    {
        public StopMainTrackRequestMessage(string mainTrackNo, string xor = null)
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
            DATA6 = "11";
        }
        public static StopMainTrackRequestMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 4; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if (hexStr.Replace(" ", "").Equals("0037000000000011") && "03".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })))
            {
                return new StopMainTrackRequestMessage(resBytes);
            }
            return null;
        }
        public StopMainTrackRequestMessage(byte[] resBytes):base(resBytes)
        { }
    }
}
