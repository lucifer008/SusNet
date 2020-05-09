using log4net;
using Sus.Net.Common.Utils;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Sus.Net.Client.Sockets
{
    /// <summary>
    /// 异步TCP客户端
    /// </summary>
    public class AsyncTcpClient : IDisposable
    {
        private ILog log = LogManager.GetLogger(typeof(AsyncTcpClient));
        protected readonly static ILog logTcpInfo = LogManager.GetLogger("TcpLogInfo");
        protected readonly static ILog logTcpError= LogManager.GetLogger("TcpErrorInfo");
        #region Fields

        private TcpClient _tcpClient;
        private bool _disposed = false;
        private int _retries = 0;
        private Timer reconnecTimer;

        #endregion

        #region Ctors

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteEP">远端服务器终结点</param>
        public AsyncTcpClient(IPEndPoint remoteEP)
          : this(new[] { remoteEP.Address },remoteEP.Port)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteEP">远端服务器终结点</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPEndPoint remoteEP,IPEndPoint localEP)
          : this(new[] { remoteEP.Address },remoteEP.Port,localEP)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddress">远端服务器IP地址</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(IPAddress remoteIPAddress,int remotePort)
          : this(new[] { remoteIPAddress },remotePort)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddress">远端服务器IP地址</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress remoteIPAddress,int remotePort,IPEndPoint localEP)
          : this(new[] { remoteIPAddress },remotePort,localEP)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteHostName">远端服务器主机名</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(string remoteHostName,int remotePort)
          : this(Dns.GetHostAddresses(remoteHostName),remotePort)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteHostName">远端服务器主机名</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(string remoteHostName,int remotePort,IPEndPoint localEP)
          : this(Dns.GetHostAddresses(remoteHostName),remotePort,localEP)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddresses">远端服务器IP地址列表</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(IPAddress[] remoteIPAddresses,int remotePort)
          : this(remoteIPAddresses,remotePort,null)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddresses">远端服务器IP地址列表</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress[] remoteIPAddresses,int remotePort,IPEndPoint localEP)
        {
            this.Addresses = remoteIPAddresses;
            this.Port = remotePort;
            this.LocalIPEndPoint = localEP;
            this.Encoding = Encoding.Default;
            Retries = 3;
            RetryInterval = 5;
            reconnecTimer = new System.Timers.Timer { AutoReset = false };
            reconnecTimer.Elapsed += ReConnectHandler;
        }

        public void CreateTCPClient()
        {
            _tcpClient?.Close();
            if ( this.LocalIPEndPoint != null )
            {
                this._tcpClient = new TcpClient(this.LocalIPEndPoint);
            }
            else
            {
                this._tcpClient = new TcpClient();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 是否已与服务器建立连接
        /// </summary>
        public bool Connected
        {
            get
            {
                if ( _tcpClient == null )
                {
                    log.InfoFormat("Connected: 当前Socket为null");
                    return false;
                }
                //  Log.Info("Connected: 当前Socket{0}状态:{1}",_tcpClient.ToString(),_tcpClient.Client.Connected ? "已连接" : "未连接");
                log.Info(string.Format("Connected: 当前Socket{0}状态:{1}", _tcpClient.ToString(), _tcpClient.Client.Connected ? "已连接" : "未连接"));
                return _tcpClient.Client.Connected;
            }
        }
        /// <summary>
        /// 远端服务器的IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; private set; }
        /// <summary>
        /// 远端服务器的端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 连接重试次数,如果值为 UnlimitedRetry，则无限重连
        /// </summary>
        public int Retries { get; set; }
        /// <summary>
        /// 无限制重连
        /// </summary>
        public static int UnlimitedRetry = -1;
        /// <summary>
        /// 连接重试间隔
        /// </summary>
        public int RetryInterval { get; set; }
        /// <summary>
        /// 远端服务器终结点
        /// </summary>
        public IPEndPoint RemoteIPEndPoint { get { return new IPEndPoint(Addresses[0],Port); } }
        /// <summary>
        /// 本地客户端终结点
        /// </summary>
        protected IPEndPoint LocalIPEndPoint { get; private set; }
        /// <summary>
        /// 通信所使用的编码
        /// </summary>
        public Encoding Encoding { get; set; }

        #endregion

        #region Connect

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public void Connect()
        {
            try
            {
                
                CreateTCPClient();
                if ( !Connected )
                {
                    // start the async connect operation
                    _tcpClient?.BeginConnect(Addresses,Port,HandleTcpServerConnected,_tcpClient);
                }
            }
            catch ( ObjectDisposedException ex )
            {
                // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                CreateTCPClient();
                _tcpClient?.BeginConnect(Addresses,Port,HandleTcpServerConnected,_tcpClient);
            }
            catch ( Exception ex )
            {
                
                //Log.Fatal("_tcpClient:{0}",_tcpClient?.ToString());
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("_tcpClient:{0}", _tcpClient?.ToString()));
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                Console.WriteLine(string.Format("异常:{0}" ,ex.Message));
                ReConnect(RetryInterval);
            }
        }

   

        /// <summary>
        /// 关闭与服务器的连接
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public void Close(bool isManual = false)
        {
            try
            {
                if ( isManual )
                {
                    reconnecTimer?.Stop();
                }
                //if ( Connected )
                //{
                _retries = 0;
                _tcpClient?.Close();
                _tcpClient = null;
                RaiseServerDisconnected(Addresses,Port);
                //}
                log.InfoFormat("当前Socket状态:{0}",_tcpClient);
            }
            catch ( Exception ex )
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        private void ReConnectHandler(object o,EventArgs e)
        {
            Connect();
        }

        private void ReConnectImmediately()
        {
            ReConnect(1);
        }

        private void ReConnect(int delayInterval)
        {
            try
            {
                _retries++;
                if ( Retries != UnlimitedRetry && _retries > Retries )
                {
                    //Log.Info("已经超过最大重连次数，连接已断开({0}/{1}:{2}s)",_retries,Retries,RetryInterval);
                    log.Info(string.Format("已经超过最大重连次数，连接已断开({0}/{1}:{2}s)", _retries, Retries, RetryInterval));
                }
                else
                {
                    //等待 delayInterval 秒后重新连接
                    if ( reconnecTimer.Enabled )
                    {
                        reconnecTimer.Stop();
                    }
                    reconnecTimer.Interval = delayInterval * 1000;
                    reconnecTimer.Start();
                    //Log.Info("正在重连服务器....({0}/{1}:{2}s)",_retries,Retries,delayInterval);
                    log.Info(string.Format("正在重连服务器....({0}/{1}:{2}s)", _retries, Retries, delayInterval));
                }
            }
            catch ( Exception ex )
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        #endregion

        #region Receive

        private void HandleTcpServerConnected(IAsyncResult ar)
        {
            try
            {
                _tcpClient?.EndConnect(ar);
                RaiseServerConnected(Addresses,Port);
                _retries = 0;
            }
            catch ( Exception ex )
            {
                // we have failed to connect to all the IP Addresses, connection has failed overall.
                RaiseServerExceptionOccurred(Addresses,Port,ex);
                ReConnect(RetryInterval);
                return;
            }

            try
            {
                // we are connected successfully and start asyn read operation.
                byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];
                _tcpClient?.GetStream().BeginRead(buffer,0,buffer.Length,HandleDatagramReceived,buffer);
            }
            catch ( Exception ex )
            {
                // we have failed to connect to all the IP Addresses, connection has failed overall.
                RaiseServerExceptionOccurred(Addresses,Port,ex);
                ReConnect(RetryInterval);
            }
        }

        private void HandleDatagramReceived(IAsyncResult ar)
        {
            try
            {
                NetworkStream stream = _tcpClient.GetStream();
                int numberOfReadBytes = 0;
                try
                {
                    numberOfReadBytes = stream.EndRead(ar);
                }
                catch ( System.Exception ex )
                {
                    numberOfReadBytes = 0;
                    //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                    log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                }

                if ( numberOfReadBytes == 0 )
                {
                    // connection has been closed
                    Close();
                    ReConnect(RetryInterval);
                    return;
                }

                // received byte and trigger event notification
                byte[] buffer = (byte[])ar.AsyncState;
                byte[] receivedBytes = new byte[numberOfReadBytes];
                Array.Copy(buffer,0,receivedBytes,0,numberOfReadBytes);

                // 当你看到这里时，这里的代码已经年久失修了，我并没有再进一步完善这里的 Socket 相关代码。
                // 比如，没有分包组包粘包，甚至连消息头都没有定义。
                // 那怎么办呢？可以参考这里： https://github.com/gaochundong/Cowboy

                // If you reach here for some reasons, I'm sorry the source code here hasn't been maintained for a little while.
                // I didn't keep update the code about socket and other related such as packet splitting or packet concatting.
                // So much even that I didn't define a packet header or something like the packet end delimiters.
                // Then, what can I do now? please take a look my new socket project:
                // https://github.com/gaochundong/Cowboy
                // Good Luck!

                try
                {
                    RaiseDatagramReceived(_tcpClient,receivedBytes);
                    RaisePlaintextReceived(_tcpClient,receivedBytes);
                }
                catch ( Exception ex )
                {
                    //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                    log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                }

                // then start reading from the network again
                stream.BeginRead(buffer,0,buffer.Length,HandleDatagramReceived,buffer);
            }
            catch ( ObjectDisposedException ex )
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch ( Exception ex )
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 接收到数据报文事件
        /// </summary>
        public event EventHandler<TcpDatagramReceivedEventArgs<byte[]>> DatagramReceived;
        /// <summary>
        /// 接收到数据报文明文事件
        /// </summary>
        public event EventHandler<TcpDatagramReceivedEventArgs<string>> PlaintextReceived;

        private void RaiseDatagramReceived(TcpClient sender,byte[] datagram)
        {
            DatagramReceived?.Invoke(this,new TcpDatagramReceivedEventArgs<byte[]>(sender,datagram));
        }

        private void RaisePlaintextReceived(TcpClient sender,byte[] datagram)
        {
            PlaintextReceived?.Invoke(this,new TcpDatagramReceivedEventArgs<string>(sender,this.Encoding.GetString(datagram,0,datagram.Length)));
        }

        /// <summary>
        /// 与服务器的连接已建立事件
        /// </summary>
        public event EventHandler<TcpServerConnectedEventArgs> ServerConnected;
        /// <summary>
        /// 与服务器的连接已断开事件
        /// </summary>
        public event EventHandler<TcpServerDisconnectedEventArgs> ServerDisconnected;
        /// <summary>
        /// 与服务器的连接发生异常事件
        /// </summary>
        public event EventHandler<TcpServerExceptionOccurredEventArgs> ServerExceptionOccurred;

        private void RaiseServerConnected(IPAddress[] ipAddresses,int port)
        {
            if ( ServerConnected != null )
            {
                ServerConnected(this,new TcpServerConnectedEventArgs(ipAddresses,port));
            }
        }

        private void RaiseServerDisconnected(IPAddress[] ipAddresses,int port)
        {
            ServerDisconnected?.Invoke(this,new TcpServerDisconnectedEventArgs(ipAddresses,port));
        }

        private void RaiseServerExceptionOccurred(IPAddress[] ipAddresses,int port,Exception innerException)
        {
            ServerExceptionOccurred?.Invoke(this,new TcpServerExceptionOccurredEventArgs(ipAddresses,port,innerException));
        }

        #endregion

        #region Send

        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(byte[] datagram)
        {
            try
            {
                if ( datagram == null )
                    throw new ArgumentNullException("datagram");
                //生成check number
                datagram = SetTcpCheckNumber(datagram);
                if ( !Connected )
                {
                    Thread th_back = new Thread(ReConnectImmediately) { Name = "Thread_back",IsBackground = true };
                    th_back.Start();
                    RaiseServerDisconnected(Addresses,Port);
                    throw new InvalidProgramException("This client has not connected to server.");
                }

                _tcpClient.GetStream().BeginWrite(datagram,0,datagram.Length,HandleDatagramWritten,_tcpClient);
            }
            catch ( Exception ex )
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        private void HandleDatagramWritten(IAsyncResult ar)
        {
            try
            {
                ( (TcpClient)ar.AsyncState ).GetStream().EndWrite(ar);
            }
            catch ( Exception ex )
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(string datagram)
        {
            Send(this.Encoding.GetBytes(datagram));
        }

        #endregion

        #region check
        /// <summary>
        /// 计算并赋值校验位
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] SetTcpCheckNumber(byte[] bytes)
        {
            bytes[3] = HexHelper.GetBCCNumer(bytes);

            return bytes;
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if ( !this._disposed )
                {
                    if ( disposing )
                    {
                        try
                        {
                            Close();
                        }
                        catch ( SocketException ex )
                        {
                            //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                            log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                        }
                        catch ( Exception ex )
                        {
                            //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                            log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                        }
                    }
                    _disposed = true;
                }
            }
            catch ( Exception ex )
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        public override String ToString()
        {
            return LocalIPEndPoint.ToString();
        }
        #endregion
    }
}
