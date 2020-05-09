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
    /// 【工序分配衣架回应】 【硬件分配衣架到下一站点工序 回应】
    /// </summary>
    public class AllocationHangerResponseMessage : MessageBody
    {
        public AllocationHangerResponseMessage(string mainTrackNo, string statingNo, string addh, string addl, string hangerNo, string xor = null)
        {
            XID = mainTrackNo;
            ID = statingNo;
            CMD = "04";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            ADDH = addh;
            ADDL = addl;
            var bHanger = HexHelper.strToToHexByte(hangerNo);
            if (bHanger.Length != 5)
            {
                log.Error("HangerOutStatingResponseMessage", new ApplicationException("衣架号长度有误!"));
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
        public AllocationHangerResponseMessage(byte[] resBytes) : base(resBytes)
        {
        }
        public static AllocationHangerResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 7; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("04".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "00".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[4] })) && "51".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                return new AllocationHangerResponseMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
    }
}
