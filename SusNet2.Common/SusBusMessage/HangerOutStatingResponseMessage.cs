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
    /// 衣架出战
    /// 01 44 05 XX 00 55 00 AA BB CC DD EE 允许出站
    /// </summary>
    public class HangerOutStatingResponseMessage : MessageBody
    {
        public HangerOutStatingResponseMessage(string mainTrackNo,string statingNo,bool isAuto,string hangerNo, string addh, string addl, string xor = null)
        {
            XID = mainTrackNo;
            ID = statingNo;
            CMD = "05";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            ADDH =addh;
            ADDL = addl;
            var bHanger=HexHelper.strToToHexByte(hangerNo);
            if (bHanger.Length!=5) {
                log.Error("HangerOutStatingResponseMessage", new ApplicationException("衣架号长度有误!"));
            }
            if (isAuto)
                DATA1 = "00";
            else
                DATA1 = "01";
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
        public HangerOutStatingResponseMessage(byte[] resBytes) : base(resBytes)
        {

        }
        public static HangerOutStatingResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 6; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("05".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "00".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[4] })) && "55".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                return new HangerOutStatingResponseMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
    }
}
