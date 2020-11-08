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

            IncomingMessageManager.Instance.OnWelcomeReceivedMessage += WelcomeReceivedMessage;
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
                _tcpListener.Stop();

                result = true;
            }
            catch (Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("ServerBase.StopServer() failed. Reason: {0}", ex.Message), ex);
            }

            return result;
        }

        #endregion

        #region Protected Functions

        #region Network Functions

        protected virtual void ListenForConnections()
        {
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        }

        protected virtual void TCPConnectCallback(IAsyncResult result)
        {
            if (_tcpListener.Server.IsBound)
            {
                TcpClient client = _tcpListener.EndAcceptTcpClient(result);
                ListenForConnections();

                //int userId = GetNewUserId();
                string userId = GetNewUserId();
                TCP tcp = new TCP(userId);
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
                }
            }
        }

        #endregion

        #region Util Functions

        protected virtual string GetNewUserId()
        {
            return "Player_" + _idCounter++;
        }

        #endregion

        #endregion
    }
}