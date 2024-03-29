﻿using SusNet2.Common.Message;
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
    /// 01 44 04 XX 00 06 00 00 00 00 00 11
    /// 停止主轨【响应】
    /// </summary>
    public class StopMainTrackResponseMessage : MessageBody
    {
        public static StopMainTrackResponseMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;
            IList<byte> bList = new List<byte>();
            for (var index = 4; index < 12; index++)
            {
                bList.Add(resBytes[index]);
            }
            var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if (hexStr.Replace(" ", "").Equals("0006000000000011") && "04".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })))
            {
                return new StopMainTrackResponseMessage(resBytes);
            }
            return null;
        }
       
        public StopMainTrackResponseMessage(byte[] resBytes):base(resBytes)
        { }
    }
}
