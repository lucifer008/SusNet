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
    /// 衣架落入读卡器,pc对请求的响应
    /// 1---->显示不同工序的响应
    ///     01 44 05 XX 00 54 01 AA BB CC DD EE 不同工序
    /// 2---->向终端通知显示 制单号、工序信息等信息
    ///     01 44 05 XX 01 00 A0 45 B0 44 C0 23 显示内容的UCICODE码
    ///01 44 05 XX 01 01 A0 45 B0 44 C0 23 显示内容的UCICODE码
    ///01 44 05 XX 01 02 A0 45 B0 44 C0 23 显示内容的UCICODE码
    ///.......
    ///01 44 05 XX 01 20 00 00 00 00 00 00
    /// </summary>
    public class HangerDropCardResponseMessage : MessageBody
    {
        /// <summary>
        /// 衣架落入读卡器,pc对请求的响应
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="hangerNo"></param>
        /// <param name="Tag">
        /// 0:相同工序
        /// 1:工序不同
        /// 2:返工衣架
        /// </param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="xor"></param>
        public HangerDropCardResponseMessage(string mainTrackNo, string statingNo, string hangerNo, int Tag, string addh, string addl, string xor = null)
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
                log.Error("HangerDropCardResponseMessage", new ApplicationException("衣架号长度有误!"));
            }
            switch (Tag)
            {
                case 0://相同工序
                    DATA1 = "00";
                    break;
                case 1://工序不同
                    DATA1 = "01";
                    break;
                case 2://返工衣架
                    DATA1 = "02";
                    break;
            }
           
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
        public HangerDropCardResponseMessage(byte[] resBytes) : base(resBytes)
        {
            //不同工序
            this.DATA1 = "01";
            //pc响应
            this.CMD = "05";
        }
        public static HangerDropCardResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 6; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("05".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "00".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[4] })) && "54".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                return new HangerDropCardResponseMessage(resBytes)
                {
                    HangerNo = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string HangerNo { set; get; }
        /// <summary>
        /// 工序是否相同
        /// </summary>
        public bool IsEqualFlow { set; get; }

        /// <summary>
        /// 【衣架落入读卡器,衣架携带制品信息推送】pc向硬件制品信息
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="productNumber"></param>
        /// <param name="index"></param>
        /// <param name="xor"></param>
        /// <returns></returns>
        public static byte[] GetHeaderBytes(string mainTrackNo, string statingNo, string addh, string addl, int index, string xor = null)
        {
            if (string.IsNullOrEmpty(xor))
                xor = "00";
            string hexStr = string.Format("{0} {1} {2} {3} {4} {5}", mainTrackNo, statingNo, "05", xor, addh, addl);
            return HexHelper.strToToHexByte(hexStr);
        }

        /// <summary>
        /// 是否是【衣架落入读卡器,衣架携带制品信息推送 pc回应 】请求
        /// </summary>
        /// <param name="resBytes"></param>
        /// <returns></returns>
        public static bool isEqual(Byte[] resBytes, out List<byte> productsInfo)
        {
            // Array ar = null;

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            var cmd = HexHelper.byteToHexStr(new byte[] { resBytes[0], resBytes[1], resBytes[2] });
            if (cmd.Equals("887905"))
            {
                productsInfo = new List<byte>();

                for (int b = 6; b < resBytes.Length; b++)
                {
                    productsInfo.Add(resBytes[b]);
                }
                //productNumber = null;
                return true;
            }
            productsInfo = null;
            return false;
        }
        /// <summary>
        /// 是否是【衣架落入读卡器,衣架携带制品信息推送】请求结束
        /// </summary>
        /// <param name="resBytes"></param>
        /// <returns></returns>
        public static bool isEnd(Byte[] resBytes)
        {
            // Array ar = null;

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            var cmd = HexHelper.byteToHexStr(new byte[] { resBytes[0], resBytes[1], resBytes[2] });
            //var endFlag = new byte[7];
            //resBytes.CopyTo(endFlag, 5);
            var list = new List<byte>();
            for (var i = 6; i < resBytes.Length; i++)
            {
                list.Add(resBytes[i]);
            }
            var endCmdHex = HexHelper.byteToHexStr(list.ToArray());
            IList<byte> bList = new List<byte>();
            if (cmd.Equals("88 79 05".Replace(" ", "")) && "00 00 00 00 00 00".Replace(" ", "").Equals(endCmdHex))
            {
                return true;
            }
            return false;
        }

    }
}
