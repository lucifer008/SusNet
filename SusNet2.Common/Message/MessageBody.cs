using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SusNet2.Common.Utils;
using log4net;

namespace SusNet2.Common.Message
{
    public class MessageBody : IMessage
    {
        public static readonly ushort MinSizeOfMessage = 12;
        public MessageBody() { }

        public MessageBody(byte[] resBytes)
        {
            if (null == resBytes)
            {
                var e = new ApplicationException("错误，相应字节为空!");
                log.Error("MessageBody", e);
                throw e;
            }
            if (resBytes.Length != 12)
            {
                var e = new ApplicationException(string.Format("错误，消息长度错误，长度为:{0}!", resBytes.Length));
                log.Error("MessageBody", e);
                throw e;
            }

            XID = HexHelper.byteToHexStr(new byte[] { resBytes[0] });
            ID = HexHelper.byteToHexStr(new byte[] { resBytes[1] });
            CMD = HexHelper.byteToHexStr(new byte[] { resBytes[2] });
            XOR = HexHelper.byteToHexStr(new byte[] { resBytes[3] });
            ADDH = HexHelper.byteToHexStr(new byte[] { resBytes[4] });
            ADDL = HexHelper.byteToHexStr(new byte[] { resBytes[5] });
            DATA1 = HexHelper.byteToHexStr(new byte[] { resBytes[6] });
            DATA2 = HexHelper.byteToHexStr(new byte[] { resBytes[7] });
            DATA3 = HexHelper.byteToHexStr(new byte[] { resBytes[8] });
            DATA4 = HexHelper.byteToHexStr(new byte[] { resBytes[9] });
            DATA5 = HexHelper.byteToHexStr(new byte[] { resBytes[10] });
            DATA6 = HexHelper.byteToHexStr(new byte[] { resBytes[11] });
        }

        protected static readonly ILog log = log4net.LogManager.GetLogger(typeof(MessageBody));

        /// <summary>
        /// 主轨
        /// </summary>
        public string XID { set; get; }
        /// <summary>
        /// 站Id:每个站点都分配一个ID，同一线上ID不能重复；00和FF表示群发。站点ID只能由液晶屏进行修改
        /// </summary>
        public string ID { set; get; }

        /// <summary>
        /// 命令
        /// 01表示PC查询；----- 02表示站点应答
        /// 03表示PC修改；-----04表示站点应答
        /// 06表示站点主动上传；-----05表示PC应答
        /// </summary>
        public string CMD { set; get; }
        /// <summary>
        /// 用FF时表示忽略校验。只在测试中使用FF，正常使用时，是除自己外，所有11个字节的XOR校验
        /// </summary>
        public string XOR { set; get; }
        public string ADDH { set; get; }
        public string ADDL { set; get; }
        public string DATA1 { set; get; }
        public string DATA2 { set; get; }
        public string DATA3 { set; get; }
        public string DATA4 { set; get; }
        public string DATA5 { set; get; }
        public string DATA6 { set; get; }

        ////消息ID
        public int id { get; set; } 
        public string gid
        {
            get; set;
        } = "1";

        public string uid { get; set; } = "2";

        public byte[] GetContent()
        {
            this.XID = "00";
            this.ID = "01";
            this.CMD = "02";
            this.XOR = "00";
            this.ADDH = "0001";
            this.ADDL = "0001";
            this.DATA1 = "aaa";

            var xid = HexHelper.strToToHexByte(XID);
            var id = HexHelper.strToToHexByte(ID);
            var addh = HexHelper.strToToHexByte(ADDH);
            var addl = HexHelper.strToToHexByte(ADDL);
            var byteList = new List<byte>();
            byteList.AddRange(xid);
            byteList.AddRange(id);
            byteList.AddRange(addh);
            byteList.AddRange(addl);
            return byteList.ToArray();
        }

        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] GetMessage()
        {
            if (string.IsNullOrEmpty(this.XID))
                this.XID = "00";
            if (string.IsNullOrEmpty(this.ID))
                this.ID = "01";
            if (string.IsNullOrEmpty(this.CMD))
                this.CMD = "02";
            if (string.IsNullOrEmpty(this.XOR))
                this.XOR = "00";
            if (string.IsNullOrEmpty(this.ADDH))
                this.ADDH = "0001";
            if (string.IsNullOrEmpty(this.ADDL))
                this.ADDL = "0001";
            //if(string.IsNullOrEmpty(this.DATA1))
            //    this.DATA1 = "DATA1";
            //if (string.IsNullOrEmpty(this.DATA2))
            //    this.DATA2 = "DATA2";
            //if (string.IsNullOrEmpty(this.DATA3))
            //    this.DATA3 = "DATA3";
            //if (string.IsNullOrEmpty(this.DATA4))
            //    this.DATA4 = "DATA4";
            //if (string.IsNullOrEmpty(this.DATA5))
            //    this.DATA5 = "DATA5";
            //if (string.IsNullOrEmpty(this.DATA6))
            //    this.DATA6 = "DATA6";
            var xid = HexHelper.strToToHexByte(XID);
            var id = HexHelper.strToToHexByte(ID);
            var addh = HexHelper.strToToHexByte(ADDH);
            var addl = HexHelper.strToToHexByte(ADDL);
            //if (string.IsNullOrEmpty(this.DATA1))
            //{
            //    var data1 = HexHelper.strToToHexByte(this.DATA1);
            //}
            //var data2 = HexHelper.strToToHexByte(this.DATA2);
            //var data3 = HexHelper.strToToHexByte(this.DATA3);
            //var data4 = HexHelper.strToToHexByte(this.DATA4);
            //var data5 = HexHelper.strToToHexByte(this.DATA5);
            //var data6 = HexHelper.strToToHexByte(this.DATA6);

            var byteList = new List<byte>();
            byteList.AddRange(xid);
            byteList.AddRange(id);
            byteList.AddRange(addh);
            byteList.AddRange(addl);
            if (!string.IsNullOrEmpty(this.DATA1))
            {
                var data1 = HexHelper.strToToHexByte(this.DATA1);
                byteList.AddRange(data1);
            }
            if (!string.IsNullOrEmpty(this.DATA2))
            {
                var data1 = HexHelper.strToToHexByte(this.DATA2);
                byteList.AddRange(data1);
            }
            if (!string.IsNullOrEmpty(this.DATA3))
            {
                var data1 = HexHelper.strToToHexByte(this.DATA3);
                byteList.AddRange(data1);
            }
            if (!string.IsNullOrEmpty(this.DATA4))
            {
                var data1 = HexHelper.strToToHexByte(this.DATA4);
                byteList.AddRange(data1);
            }
            if (!string.IsNullOrEmpty(this.DATA5))
            {
                var data1 = HexHelper.strToToHexByte(this.DATA5);
                byteList.AddRange(data1);
            }
            if (!string.IsNullOrEmpty(this.DATA6))
            {
                var data1 = HexHelper.strToToHexByte(this.DATA6);
                byteList.AddRange(data1);
            }
            //byteList.AddRange(data2);
            //byteList.AddRange(data3);
            //byteList.AddRange(data4);
            //byteList.AddRange(data5);
            //byteList.AddRange(data6);

            return byteList.ToArray();
        }
        public static MessageBody Decode(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(data));
        }

        public virtual byte[] Encode()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        public virtual string Describe()
        {
            //return " id:" + id + " gid:" + gid + " uid:" + uid + " content:" + content;
            return "XID:" + XID + " ID:" + ID + " CMD:" + CMD + " XOR:" + XOR + " ADDH:" + ADDH + " ADDL:" + ADDL + " DATA" + DATA1;
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", this.XID, this.ID, this.CMD, this.XOR, this.ADDH, this.ADDL, this.DATA1, this.DATA2, this.DATA3, this.DATA4, this.DATA5, this.DATA6);
        }
        public byte[] GetBytes()
        {
            string hexStr = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", this.XID, this.ID, this.CMD, this.XOR, this.ADDH, this.ADDL, this.DATA1, this.DATA2, this.DATA3, this.DATA4, this.DATA5, this.DATA6);
            return HexHelper.strToToHexByte(hexStr);
        }
        public string GetHexStr()
        {
            string hexStr = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", this.XID, this.ID, this.CMD, this.XOR, this.ADDH, this.ADDL, this.DATA1, this.DATA2, this.DATA3, this.DATA4, this.DATA5, this.DATA6);
            return hexStr;
        }
    }
}
