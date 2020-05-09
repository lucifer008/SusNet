using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sus.Net.Common.Message
{
    [Serializable]
    public class SusMessage : IMessage
    {
        /// <summary>
        /// 消息长度12字节
        /// XID+ID+CMD+XOR+ADDH+ADDL+DATA....=12
        /// XID+ID+CMD+XOR+ADDH+ADDL 各占一个字节
        /// </summary>
        public static readonly int MsgLen = 12;
        private ushort length { get; set; }
        public string Describe()
        {
            return "length:" + length + " body:" + (body == null ? string.Empty : Encoding.UTF8.GetString(body));
        }
        /// <summary>
        /// 消息体
        /// </summary>
        public byte[] body { get; set; }
        public byte[] Encode()
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(body);
            return buffer.ToArray();
        }
        public SusMessage() { }
        public static SusMessage EncodeMessaage(byte[] bodyData)
        {
            List<byte> buffer = new List<byte>();
            SusMessage message = new SusMessage(bodyData);
            return message;
        }
        /// <summary>
        /// 初始化消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="bodyData">消息体数据</param>
        public SusMessage(byte[] bodyData)
        {
            this.length = (ushort)bodyData.Length; //length=12
            this.body = bodyData;
        }
        public static List<SusMessage> DecodeMessaage(List<byte> buffer, out int offset)
        {
            List<SusMessage> messageList = new List<SusMessage>();
            offset = buffer.Count - 1;
            for (int i = 0; i < buffer.Count; i++)
            {
                //查找消息头
                offset = i;
                //确认满足最小完整包
                if (buffer.Count >= i + Message.MinSizeOfMessage)
                {
                    //获取消息长度
                    ushort length = System.BitConverter.ToUInt16(buffer.GetRange(i + 1, 2).ToArray(), 0);
                    int tailFlagIndex = i + sizeof(byte) + sizeof(ushort) + length; //消息头(1 byte) + 消息Length字段(2 byte) + 消息长度
                    int bodyLength = length - 2 * sizeof(byte)/*Message.type and Message.crc size*/;
                    //if (buffer.Count > tailFlagIndex && buffer[tailFlagIndex] == TAIL_FLAG)
                    //{
                    //    SusMessage message = new SusMessage();
                    //    message.length = length;
                    //    message.body = buffer.GetRange(i, bodyLength).ToArray();
                    //    messageList.Add(message);

                    //    //设置偏移
                    //    i = offset = tailFlagIndex;
                    //}
                }
            }
            return messageList;
        }
    }
    
    /// <summary>
    /// 命令类型
    /// </summary>
    public enum CMD_TYPE : UInt16 {
        /// <summary>
        /// PC查询
        /// </summary>
        PC_QUERY=01,
        /// <summary>
        /// 站点应答
        /// </summary>
        SITE_REPLY=02,
        /// <summary>
        /// PC修改
        /// </summary>
        PC_UPDATE=03,

        /// <summary>
        /// 站点应答
        /// </summary>
        SITE_REPLY2=04,
        /// <summary>
        /// PC应答
        /// </summary>
        PC_REPLY=05,

        /// <summary>
        /// 主动上传
        /// </summary>
        PC__DO_UPLOAD=06
    }
}
