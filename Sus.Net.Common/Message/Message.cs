using System;
using System.Collections.Generic;
using System.Text;

namespace Sus.Net.Common.Message
{
    [Serializable]
    public class Message : IMessage
    {
        //消息结构(最小长度 6）:
        //标识位(1 byte) 消息长度(2 byte) 消息类型(1 byte) 消息体(x byte) 校验和(1 byte) 标识位(1 byte)
        public static readonly ushort MinSizeOfMessage = 6;

        /// <summary>
        /// 标识位
        /// </summary>
        public static readonly byte HEAD_FLAG = 0x02;

        public static readonly byte TAIL_FLAG = 0x03;

        /// <summary>
        /// 转义字符
        /// </summary>
        public static readonly byte ESCAPE_FLAG = 0x7d;

        public static readonly byte HEAD_FLAG_ESCAPE = 0x04; //0x02 => 0x7d0x04
        public static readonly byte TAIL_FLAG_ESCAPE = 0x05; //0x03 => 0x7d0x05
        public static readonly byte ESCAPE_FLAG_ESCAPE = 0x01; //0x7d => 0x7d0x01

        /// <summary>
        /// 消息体长度
        /// </summary>
        private ushort length { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType type { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public byte[] body { get; set; }

        /// <summary>
        /// 校验和
        /// </summary>
        public byte crc { get; set; }

        public Message() { }

        /// <summary>
        /// 初始化消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="bodyData">消息体数据</param>
        public Message(MessageType type,byte[] bodyData)
        {
            this.length = (ushort)( sizeof(byte) + bodyData.Length + sizeof(byte) ); //length=消息类型长度(1 byte) + 消息体长度(?) + CRC长度(1 byte)
            this.type = type;
            this.body = bodyData;
            this.crc = 0x00;
        }

        public byte[] Encode()
        {
            List<byte> buffer = new List<byte>();
            //标识头
            buffer.Add((byte)HEAD_FLAG);

            buffer.AddRange(System.BitConverter.GetBytes(length));
            buffer.Add((byte)type);
            buffer.AddRange(body);
            crc = GetCRC(buffer.ToArray(),0,buffer.Count);
            buffer.Add(crc);

            //标识尾
            buffer.Add((byte)TAIL_FLAG);
            return buffer.ToArray();
        }

        public Message Decode(List<byte> buffer)
        {
            return this;
        }

        public string Describe()
        {
            return "length:" + length + " type:" + type + " crc:" + crc + " body:" + ( body == null ? string.Empty : Encoding.UTF8.GetString(body) );
        }

        public static Message EncodeMessaage(MessageType type,byte[] bodyData)
        {
            List<byte> buffer = new List<byte>();
            Message message = new Message(type,bodyData);
            return message;
        }
        public static List<Message> DecodeMessaage2(List<byte> buffer)
        {
            List<Message> messageList = new List<Message>();
            Message message = new Message();
            message.length = (ushort)buffer.Count;
            message.type = MessageType.Common;
            message.body = buffer.ToArray();
            message.crc = 0;
            messageList.Add(message);
            return messageList;
        }
        public static List<Message> DecodeMessaage(List<byte> buffer,out int offset)
        {
            List<Message> messageList = new List<Message>();

            offset = buffer.Count - 1;
            for (int i = 0; i < buffer.Count; i++)
            {
                //查找消息头
                if (buffer[i] == HEAD_FLAG)
                {
                    offset = i;
                    //确认满足最小完整包
                    if (buffer.Count >= i + Message.MinSizeOfMessage)
                    {
                        //获取消息长度
                        ushort length = System.BitConverter.ToUInt16(buffer.GetRange(i + 1, 2).ToArray(), 0);
                        int tailFlagIndex = i + sizeof(byte) + sizeof(ushort) + length; //消息头(1 byte) + 消息Length字段(2 byte) + 消息长度
                        int bodyLength = length - 2 * sizeof(byte)/*Message.type and Message.crc size*/;
                        if (buffer.Count > tailFlagIndex && buffer[tailFlagIndex] == TAIL_FLAG)
                        {
                            Message message = new Message();
                            message.length = length;
                            message.type = (MessageType)(buffer[i + 3]);
                            message.body = buffer.GetRange(i + 4, bodyLength).ToArray();
                            message.crc = buffer[i + 4 + bodyLength];
                            messageList.Add(message);

                            //设置偏移
                            i = offset = tailFlagIndex;
                        }
                    }
                }
            }
            return messageList;
        }


        public static byte GetCRC(byte[] array,int offset,int count)
        {
            if ( offset >= 0 && count <= array.Length )
            {
                byte crc = 0;
                for ( int i = offset; i < count; i++ )
                {
                    crc ^= array[i];
                }
                return crc;
            }
            else
            {
                throw new ArgumentOutOfRangeException("start||end","数组越界");
            }
        }
    }
}
