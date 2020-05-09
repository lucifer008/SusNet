using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Utils;
using SuspeSys.Utils;

namespace Sus.Net.Common.Message
{
    public class MessageFactory: SusLog
    {
        private MessageFactory() { }
        public static MessageFactory Instance { get { return Nested._instance; } }
        private ClientUserInfo userInfo { get; set; }

        /// <summary>
        /// 消息的 ID, 递增
        /// </summary>
        private int MessageID = 0;
        private object MessageIDlock = new object();

        public void Init(ClientUserInfo userInfo)
        {
            this.userInfo = userInfo;
        }

        public Message CreateMessage(string strMessage,MessageType type = MessageType.Common,int id = 0)
        {
            if ( userInfo == null )
            {
                throw new ArgumentNullException("userInfo","userInfo 不能为null");
            }

            MessageBody cMessage = new MessageBody();
            lock ( MessageIDlock )
            {
                cMessage.id = ( 0 == id ? ++MessageID : id );
            }
            //cMessage.gid = userInfo.gid;
            //cMessage.uid = userInfo.uid;
            //cMessage.content = strMessage;
            cMessage.XID = userInfo.gid;
            cMessage.ID = userInfo.uid;
            cMessage.DATA = strMessage;
            Message message = CreateMessage(type,cMessage.Encode());
            return message;
        }


        public Message CreateMessage(MessageType type,byte[] bodyData)
        {
            try
            {
                return Message.EncodeMessaage(type,bodyData);
            }
            catch ( Exception ex )
            {
                tcpLogInfo.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 单类懒加载
        /// </summary>
        private static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly MessageFactory _instance = new MessageFactory();
        }
    }
}
