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
    /// 【pc向 硬件分配衣架到下一站点工序 请求】
    /// </summary>
    public class AllocationHangerRequestMessage : MessageBody
    {
        public AllocationHangerRequestMessage(string mainTrackNo, string statingNo, string addh, string addl, string hangerNo, string xor = null)
        {
            XID = HexHelper.tenToHexString(mainTrackNo);
            ID = HexHelper.tenToHexString(statingNo);
            CMD = "03";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            if (!string.IsNullOrEmpty(addh))
                ADDH = addh;
            else
                ADDH = "00";
            if (!string.IsNullOrEmpty(addl))
                ADDL = addl;
            else
                ADDL = "51";
            var bHanger = HexHelper.strToToHexByte(hangerNo);
            if (bHanger.Length != 5)
            {
                log.Error("AllocationHangerRequestMessage", new ApplicationException("衣架号长度有误!"));
            }
            DATA1 = "00";
            DATA2 = HexHelper.byteToHexStr(new byte[] { bHanger[0] });
            DATA3 = HexHelper.byteToHexStr(new byte[] { bHanger[1] });
            DATA4 = HexHelper.byteToHexStr(new byte[] { bHanger[2] });
            DATA5 = HexHelper.byteToHexStr(new byte[] { bHanger[3] });
            DATA6 = HexHelper.byteToHexStr(new byte[] { bHanger[4] });
            //DATA3 = "00";
            //DATA4 = "00";
            //DATA5 = "00";
            //DATA6 = "12";
        }
        public AllocationHangerRequestMessage(byte[] resBytes) : base(resBytes)
        {
        }
        public static AllocationHangerRequestMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 7; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("03".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "51".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                return new AllocationHangerRequestMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
    }
}
