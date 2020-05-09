using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sus.Net.Client
{
    public class UDPClient
    {
        public static UDPClient Instance = new UDPClient();
        private UDPClient() {
           // Init();
        }
        public Socket Socket {
            get { return client; }
        }
        private static Socket client;

        public void Init(string ip= "192.168.10.139", int port = 1902)//"255.255.255.255", int port=1901)
        {
            if (client == null)
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                client.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            }
        }


    }
}
