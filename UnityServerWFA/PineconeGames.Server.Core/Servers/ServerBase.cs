using PineconeGames.Client.Core.ConnectionTypes;
using PineconeGames.Core.Logs;
using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages;
using PineconeGames.Network.Core.Messages.Base;
using PineconeGames.Server.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PineconeGames.Server.Core.Servers
{
    public abstract class ServerBase
    {
        #region Events

        public ConnectedEventHandler OnConnected;
        public DisconnectedEventHandler OnDisconnected;

        #endregion

        #region Variables

        public int Port
        {
            get
            {
                return _port;
            }
        }
        public int MaxClients
        {
            get
            {
                return _maxClients;
            }
        }


        protected int _port;
        protected int _maxClients;
        protected TcpListener _tcpListener;
        protected List<TCP> _clientList;

        protected int _idCounter;
        protected string _serverName;
        protected string _welcomeMessage
        {
            get
            {
                return "Welcome to " + _serverName;
            }
        }

        #endregion

        #region Constructors   

        public ServerBase(int port, int maxClients)
        {
            _port = port;
            _maxClients = maxClients;

            _tcpListener = new TcpListener(IPAddress.Any, port);
            _clientList = new List<TCP>();

            _serverName = IPAddress.Any.ToString() + ":" + port.ToString();

            BindOnIncomingMessageEvents();
        }

        public ServerBase(int port, int maxClient, string serverName) : this(port, maxClient)
        {
            _serverName = serverName;
        }

        #endregion

        #region Public Functions

        public virtual bool StartServer()
        {
            bool result = false;

            PineconeLogManager.Instance.EnterInfoLog(string.Format("Starting server on port {0}...", Port));
            try
            {
                if (_tcpListener == null)
                {
                    PineconeLogManager.Instance.EnterErrorLog(string.Format("ServerBase.StartServer() failed. Port: {0}. Reason: TCP Listener has failed to initalize. Server is not working...", Port), null);
                    return false;
                }

                _tcpListener.Start();
                ListenForConnections();

                result = true;
                PineconeLogManager.Instance.EnterInfoLog(string.Format("Server started successfully on port {0}", Port));
            }
            catch (Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("ServerBase.StartServer() failed. Port: {0}. Reason: {1}", Port, ex.Message), ex);
            }

            return result;
        }

        public virtual bool StopServer()
        {
            bool result = false;

            try
            {
                DisconnectAllUsers();
                _tcpListener.Stop();

                result = true;
            }
            catch (Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("ServerBase.StopServer() failed. Reason: {0}", ex.Message), ex);
            }

            return result;
        }

        public virtual void SendMessageToClient(string clientId, MessageBase message)
        {
            if (_clientList != null && _clientList.Any())
            {
                TCP client = _clientList.FirstOrDefault(a => a.ID == clientId);

                if (client != null)
                {
                    client.SendData(message.GenerateTCPData());
                }
            }
        }

        public virtual void SendMessageToClient(int clientIndex, MessageBase message)
        {
            if (_clientList != null && _clientList.Any())
            {
                try
                {
                    TCP client = _clientList.ElementAt(clientIndex);

                    if (client != null)
                    {
                        client.SendData(message.GenerateTCPData());
                    }
                }
                catch (Exception ex)
                {
                    PineconeLogManager.Instance.EnterErrorLog(string.Format("ServerBase.SendMessageToClient({0}, {1}) failed. Reason: {2}", clientIndex, clientIndex, ex.Message), ex);
                }
            }
        }

        #endregion

        #region Protected Functions

        #region Initialize Functions

        protected virtual void BindOnIncomingMessageEvents()
        {
            IncomingMessageManager.Instance.OnWelcomeReceivedMessage += WelcomeReceivedMessage;
        }

        #endregion

        #region Network Functions

        protected virtual void ListenForConnections()
        {
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        }

        protected virtual void TCPConnectCallback(IAsyncResult result)
        {
            if (_tcpListener.Server != null && _tcpListener.Server.IsBound)
            {
                TcpClient client = _tcpListener.EndAcceptTcpClient(result);
                ListenForConnections();

                string userId = GetNewUserId();
                TCP tcp = new TCP(userId, ClientDisconnected);
                if (tcp.Connect(client))
                {
                    _clientList.Add(tcp);
                    ServerMessageManager.Instance.SendWelcomeMessageToClient(userId, _welcomeMessage, tcp);
                }  

                PineconeLogManager.Instance.EnterInfoLog(string.Format("Incoming connection from {0}...", client?.Client.RemoteEndPoint));
            }
        }

        protected virtual bool SendMessage(TCP target, MessageBase message)
        {
            bool result = false;

            Packet packet = message.GenerateTCPData();
            result = target.SendData(packet);

            return result;
        }

        #endregion

        #region Message Functions

        protected virtual void WelcomeReceivedMessage(string id, string username)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(username))
            {
                TCP tcp = _clientList.FirstOrDefault(a => a.ID == id);
                if (tcp != null)
                {
                    tcp.SetId(username);
                    PineconeLogManager.Instance.EnterInfoLog(string.Format("{0}: {1} has been changed as {2}", _serverName, id, username));

                    OnConnected?.Invoke(tcp.ID);
                }
            }
        }

        #endregion

        #region Util Functions

        protected virtual string GetNewUserId()
        {
            return "Player_" + _idCounter++;
        }

        protected virtual void DisconnectAllUsers()
        {
            if (_clientList != null && _clientList.Any())
            {
                List<TCP> allClients = _clientList.ToList();

                for (int i = 0; i < allClients.Count; i++)
                {
                    allClients[i].Disconnect();
                }
            }
        }

        #endregion

        #endregion

        #region Event Binded Functions

        protected virtual void ClientDisconnected(string id)
        {
            if (!string.IsNullOrEmpty(id) && _clientList != null && _clientList.Any())
            {
                TCP disconnectedClient = _clientList.FirstOrDefault(a => a.ID == id);
                if (disconnectedClient != null)
                {
                    _clientList.Remove(disconnectedClient);
                    PineconeLogManager.Instance.EnterInfoLog(string.Format("Client Disconnected() ID: {0}", disconnectedClient.ID));

                    OnDisconnected?.Invoke(disconnectedClient.ID);
                }
            }
        }

        #endregion
    }
}