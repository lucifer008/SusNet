using System;
using System.Collections.Generic;
using System.Text;

namespace Sus.Net.Common.Message
{
    [Serializable]
    public enum MessageType : byte
    {
        Common = 0x01,//1

        //登录
        Login = 0xF1,//241
        //应答
        ACK = 0xF2,//242
        //心跳
        Heartbeat = 0xF3,//243
    }
}
