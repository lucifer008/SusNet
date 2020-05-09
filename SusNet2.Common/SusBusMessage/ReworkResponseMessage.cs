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
    ///01 44 05 XX 00 54 02 AA BB CC DD EE 返工工序
    /// 返工【pc响应到--->硬件】
    /// </summary>
    public class ReworkResponseMessage : MessageBody
    {
        /// <summary>
        ///  返工【pc响应到--->硬件】 格式：01 44 05 XX 00 54 02 AA BB CC DD EE 返工工序
        /// </summary>
        /// <param name="xid">主轨号</param>
        public ReworkResponseMessage(byte[] resBytes) : base(resBytes)
        { }

        /// <summary>
        /// 返工【pc响应到--->硬件】 格式：01 44 05 XX 00 54 02 AA BB CC DD EE 返工工序
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo">请求返工的站点</param>
        /// <param name="returnStatingNo">返工到的站点</param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="hangerNo"></param>
        /// <param name="xor"></param>
        public ReworkResponseMessage(string mainTrackNo, string statingNo,string returnStatingNo, string addh, string addl, string hangerNo, string xor = null)
        {
            XID = mainTrackNo;
            ID = statingNo;
            CMD = "05";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            ADDH = addh;
            ADDL = addl;
            var bHanger = HexHelper.strToToHexByte(hangerNo);
            if (bHanger.Length != 5)
            {
                log.Error("ReworkResponseMessage", new ApplicationException("衣架号长度有误!"));
            }
            DATA1 = returnStatingNo;//返工到的站点
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
        public ReworkResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 4; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if (hexStr.Replace(" ", "").Equals("0006000000000010") && "03".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })))
            {
                return new ReworkResponseMessage(resBytes);
            }
            return null;
        }
    }
}
