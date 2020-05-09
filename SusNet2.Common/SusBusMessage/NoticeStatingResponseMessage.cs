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
    /// 通知站点消息【终端响应】
    /// 01 46 04 XX 00 51 00 AA BB CC DD EE 将衣架分配到下一个46工位成功
    /// </summary>
    public class NoticeStatingResponseMessage : MessageBody
    {
        public NoticeStatingResponseMessage(byte[] resBytes) : base(resBytes)
        {

        }
        public NoticeStatingResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 6; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("04".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "51".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                return new NoticeStatingResponseMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
    }
}
