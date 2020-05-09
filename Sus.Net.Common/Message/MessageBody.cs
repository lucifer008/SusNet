using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Sus.Net.Common.Message
{
    public class MessageBody : IMessage
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
        ////流水编号
        ////public int sno { get; set; }
        ////公司ID
        //public string gid { get; set; }
        ////用户ID
        //public string uid { get; set; }
        ////业务内容
        //public string content { get; set; }

        public static MessageBody Decode(byte[] data)
        {
            if ( data == null )
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
            return "XID:" + XID + " ID:" + ID + " CMD:" + CMD + " XOR:" + XOR + " ADDH:" + ADDH + " ADDL:" + ADDL+ " DATA"+ DATA;
        }
    }
}
