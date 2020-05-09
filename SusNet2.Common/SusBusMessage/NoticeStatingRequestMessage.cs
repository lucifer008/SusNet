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
    /// 通知站点消息【pc请求硬件】
    /// 01 46 03 XX 00 51 00 AA BB CC DD EE 将衣架分配到下一个46工位
    /// </summary>
    public class NoticeStatingRequestMessage: MessageBody
    {
        public NoticeStatingRequestMessage(byte[] resBytes) : base(resBytes)
        {

        }
        public NoticeStatingRequestMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 6; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("03".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "51".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                return new NoticeStatingRequestMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
    }
}
