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
    /// 衣架进站硬件终端请求--->PC
    /// 01 44 06 XX 00 50 00 AA BB CC DD EE 衣架入站
    /// </summary>
    public class HangerArrivalStatingRequestMessage : MessageBody
    {
        public HangerArrivalStatingRequestMessage(byte[] resBytes) : base(resBytes)
        {

        }
        public static HangerArrivalStatingRequestMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 7; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("06".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "50".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                return new HangerArrivalStatingRequestMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
    }
}
