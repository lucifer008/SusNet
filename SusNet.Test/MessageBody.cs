using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SusNet2.Common.Utils;

namespace SusNet2.Common.Message
{
    public class MessageBody1: IMessage1
    {
        public string XID { set; get; }
        public string ID { set; get; }
        public string CMD { set; get; }
        public string XOR { set; get; }
        public string ADDH { set; get; }
        public string ADDL { set; get; }
        public string DATA { set; get; }

        ////消息ID
        public int id { get; set; }
        public byte[] GetContent()
        {
            this.XID = "00";
            this.ID = "01";
            this.CMD = "02";
            this.XOR = "00";
            this.ADDH = "0001";
            this.ADDL = "0001";
            this.DATA = "aaa";

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
        public static MessageBody1 Decode(byte[] data)
        {
            if ( data == null )
            {
                return null;
            }
            return JsonConvert.DeserializeObject<MessageBody1>(Encoding.UTF8.GetString(data));
        }

        public virtual byte[] Encode()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        public virtual string Describe()
        {
            //return " id:" + id + " gid:" + gid + " uid:" + uid + " content:" + content;
            return "XID:" + XID + " ID:" + ID + " CMD:" + CMD + " XOR:" + XOR + " ADDH:" + ADDH + " ADDL:" + ADDL+ " DATA"+ DATA;
        }
    }
}
