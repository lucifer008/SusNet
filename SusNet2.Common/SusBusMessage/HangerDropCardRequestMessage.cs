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
    /// 衣架落入读卡器发送的请求，硬件发pc端
    /// 【衣架在04站内进行前后衣架对比的命令】
    /// 01 04 06 XX 00 54 00 AA BB CC DD EE
    /// </summary>
    public class HangerDropCardRequestMessage : MessageBody
    {
        public HangerDropCardRequestMessage(byte[] resBytes) : base(resBytes)
        {
            
        }
        public static HangerDropCardRequestMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
           
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("06".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "00".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[4] })) && "54".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                IList<byte> bList = new List<byte>();
                for (var index = 7; index < 12; index++)
                {
                    bList.Add(resBytes[index]);
                }
                return new HangerDropCardRequestMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
      
    }
}
