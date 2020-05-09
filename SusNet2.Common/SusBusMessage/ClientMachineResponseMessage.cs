﻿using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    /// <summary>
    /// 制品界面，直接选择产品上线 硬件响应
    /// 01 01 03 XX 00 35 00 00 00 00 00 05
    //给01产线01挂片站点指定05产品上线
    /// </summary>
    public class ClientMachineResponseMessage : SusNet2.Common.Message.MessageBody
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainTrackNo">主轨号</param>
        /// <param name="statingNo">挂片站</param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="productNumber">排产号</param>
        /// <param name="xor"></param>
        public ClientMachineResponseMessage(string mainTrackNo, string statingNo, string addh, string addl, string productNumber, string xor = null)
        {
            XID = mainTrackNo;
            ID = statingNo;
            CMD = "04";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            ADDH = addh;
            ADDL = addl;
            DATA1 = "00";
            DATA2 = "00";
            DATA3 = "00";
            DATA4 = "00";
            DATA5 = "00";
            DATA6 = productNumber;
        }
        public ClientMachineResponseMessage(byte[] resBytes) : base(resBytes)
        {}
        public static ClientMachineResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;

            //ProductNumber = HexHelper.byteToHexStr(bList.ToArray());

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("04".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "00".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[4] })) && "35".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                IList<byte> bList = new List<byte>();
                bList.Add(resBytes[11]);
                var clientMachineResponseMessage = new ClientMachineResponseMessage(resBytes)
                {
                    ProductNumber = HexHelper.HexToTen(HexHelper.byteToHexStr(bList.ToArray()))
                };
                clientMachineResponseMessage.MainTrackNo =HexHelper.HexToTen(clientMachineResponseMessage.XID);
                clientMachineResponseMessage.StatingNo=HexHelper.HexToTen(clientMachineResponseMessage.ID);
                return clientMachineResponseMessage;
            }
            return null;
        }
        public int ProductNumber { set; get; }
        public int StatingNo { set; get; }
        public int MainTrackNo { set; get; }
    }
}