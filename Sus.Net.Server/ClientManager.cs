using log4net;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;

namespace Sus.Net.Server
{
    class ClientManager
    {
        private ClientManager() { }
        public static ClientManager Instance { get { return Nested._instance; } }
        private readonly ConcurrentDictionary<string, ClientUserInfo> clients = new ConcurrentDictionary<string, ClientUserInfo>();
        private readonly static ILog log = LogManager.GetLogger(typeof(ClientManager));
        public void Clear()
        {
            clients.Clear();
        }

        public List<string> ClientKeyWithUserInfos(List<ClientUserInfo> clientUserInfos)
        {
            List<string> clientKeys = new List<string>();
            if (clientUserInfos == null || clientUserInfos.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (ClientUserInfo info in clientUserInfos)
                {
                    string gid = info.gid;
                    string uid = info.uid;
                    if (!string.IsNullOrWhiteSpace(uid))
                    {
                        var allClientKeys = clients.Keys;
                        foreach (var key in allClientKeys)
                        {
                            ClientUserInfo userInfo = null;
                            clients.TryGetValue(key, out userInfo);
                            clientKeys.Add(key);
                            if (userInfo != null)
                            {
                                //if (userInfo.gid == gid)
                                //{
                                //    if (string.IsNullOrWhiteSpace(uid) || userInfo.uid == uid)
                                //    {
                                //        clientKeys.Add(key);
                                //    }
                                //}
                                if (userInfo.MaintrackNumberList.Contains(uid))
                                {
                                    if (!clientKeys.Contains(key))
                                        clientKeys.Add(key);
                                }
                            }
                        }
                    }
                }
            }
            return clientKeys;
        }
        public string ClientKeyWithUserInfo(ClientUserInfo clientUserInfo)
        {
            // List<string> clientKeys = new List<string>();
            if (clientUserInfo == null)
            {
                log.Error(new ApplicationException("clientUserInfo为空!"));
                return null;
            }
            else
            {
                string gid = clientUserInfo.gid;
                string uid = clientUserInfo.uid;
                if (!string.IsNullOrWhiteSpace(uid))
                {
                    var allClientKeys = clients.Keys;
                    foreach (var key in allClientKeys)
                    {
                        ClientUserInfo userInfo = null;
                        clients.TryGetValue(key, out userInfo);
                        //clientKeys.Add(key);
                        if (userInfo != null)
                        {
                            if (userInfo.MaintrackNumberList.Contains(uid))
                            {
                                //if (string.IsNullOrWhiteSpace(uid) || userInfo.uid == uid)
                                //{
                                //    //  clientKeys.Add(key);
                                //    return key;
                                //}
                                return key;
                            }
                        }
                        // return key;
                    }
                }
            }
            log.Error(new ApplicationException("clientUserInfo为空!"));
            return null;
        }

        public List<ClientUserInfo> ClientInfoWithCorpIds(List<string> corpIds)
        {
            if (corpIds != null)
            {
                List<ClientUserInfo> clientInfos = new List<ClientUserInfo>();
                foreach (var gid in corpIds)
                {
                    ClientUserInfo clientInfo = new ClientUserInfo(gid, null);
                    clientInfos.Add(clientInfo);
                }
                return clientInfos;
            }
            else
            {
                log.Error(new ApplicationException("clientUserInfo为空!"));
                return null;
            }
        }

        public string ClientUserInfoDesc(string clientKey)
        {
            ClientUserInfo clientUserInfo = null;
            clients.TryGetValue(clientKey, out clientUserInfo);
            if (null != clientUserInfo)
            {
                return clientUserInfo.ToString() + " host: " + clientKey;
            }
            return string.Empty;
        }


        public void AddOrUpdateTcpClient(string gid, string uid, TcpClient tcpClient)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(gid) && !string.IsNullOrWhiteSpace(uid) && null != tcpClient)
                {
                    ClientUserInfo clientUserInfo = new ClientUserInfo(gid, uid);

                    clients.AddOrUpdate(tcpClient.Client.RemoteEndPoint.ToString(), (o) => clientUserInfo, (k, vv)=> {
                        if (clients.ContainsKey(k))
                        {
                            var cResult = clients[k];
                            var mnList = vv.MaintrackNumberList;
                            if (!mnList.Contains(clientUserInfo.uid))
                            {
                                mnList.Add(clientUserInfo.uid);
                                return vv;
                            }
                        }
                        return vv;
                    });//updateValueFactory);
                    //if (clients.ContainsKey(tcpClient.Client.RemoteEndPoint.ToString()))
                    //{
                    //    var mnList = clients[tcpClient.Client.RemoteEndPoint.ToString()].MaintrackNumberList;
                    //    if (!mnList.Contains(clientUserInfo.uid))
                    //    {
                    //        mnList.Add(clientUserInfo.uid);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

       

        private ClientUserInfo updateValueFactory(string clientKey,  ClientUserInfo clientUserInfo)
        {
            if (clients.ContainsKey(clientKey))
            {
                var cResult = clients[clientKey];
                var mnList = cResult.MaintrackNumberList;
                if (!mnList.Contains(clientUserInfo.uid))
                {
                    mnList.Add(clientUserInfo.uid);
                    return cResult;
                }
            }
            return clientUserInfo;
        }

        //private ClientUserInfo addValueFactory(string clientKey,)
        //{
        //    ClientUserInfo clientUserInfo = null;
        //    if (clients.ContainsKey(clientKey))
        //    {
        //        var mnList = clients[clientKey].MaintrackNumberList;
        //        if (!mnList.Contains(clientUserInfo.uid))
        //        {
        //            mnList.Add(clientUserInfo.uid);
        //        }
        //    }
        //    return clientUserInfo;
        //}

        void addValueFactory(string key,ClientUserInfo clientUserInfo) {

        }
        void updateValueFactory(string key, ClientUserInfo clientUserInfo, ClientUserInfo clientUserInfo2)
        {

        }
        public bool RemoveTcpClient(TcpClient tcpClient)
        {
            if (tcpClient != null)
            {
                ClientUserInfo clientUserInfo;
                return clients.TryRemove(tcpClient.Client.RemoteEndPoint.ToString(), out clientUserInfo);
            }
            return true;
        }

        //public void RemoveTcpClient(string gid,string uid = null)
        //{
        //    if ( !string.IsNullOrWhiteSpace(gid) )
        //    {
        //    }
        //}


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

            internal static readonly ClientManager _instance = new ClientManager();
        }
    }
}
