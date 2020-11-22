using PineconeGames.Client.Core.Clients;
using PineconeGames.Client.Core.ConnectionTypes;
using PineconeGames.Core.Logs;
using PineconeGames.CoreUnity.Patterns;
using PineconeGames.Network.Core.Messages;
using PineconeGames.Network.Core.Messages.ServerSide;
using UnityEngine;

namespace PineconeGames.CoreUnity.Network.Client
{
    public class PineconeClient : SingletonMonoBehaviour<PineconeClient>
    {
        #region Events

        protected ConnectionResultEventHandler _onConnectionResultReceived;
        protected DisconnectedEventHandler _onDisconnected;

        #endregion

        #region Variables

        protected TCPClient _tcpClient;

        #endregion

        #region Unity Functions

        protected virtual void Awake()
        {
            BindOnIncomingMessageEvents();

            PineconeMasterManager.Instance.Initialize();
        }

        protected virtual void OnApplicationQuit()
        {
            Disconnect();
        }

        #endregion

        #region Public Functions

        public void Connect(string ip, int port, string id, ConnectionResultEventHandler onConnectionResultReceived, DisconnectedEventHandler onDisconnected)
        {
            _onConnectionResultReceived = onConnectionResultReceived;
            _onDisconnected = onDisconnected;

            InitializeTCPClient(id);
            _tcpClient.Connect(ip, port, onConnectionResultReceived);
        }

        public void Disconnect()
        {
            _tcpClient?.Disconnect(); 
        }

        #endregion

        #region Protected Functions

        #region Initialize Functions

        protected virtual void BindOnIncomingMessageEvents()
        {
            IncomingMessageManager.Instance.OnWelcomeMessage += WelcomeMessage;
        }

        protected virtual void InitializeTCPClient(string id)
        {
            if (_tcpClient != null && _tcpClient.IsConnected)
            {
                _onDisconnected = null;
                _tcpClient.Disconnect();
            }

            _tcpClient?.DestroyClient(false);
            _tcpClient = new TCPClient(id, Disconnected);
        }

        #endregion

        #region Connect Functions

        protected virtual void ConnectionResultReceived(bool result)
        {
            _onConnectionResultReceived?.Invoke(result);
        }

        protected virtual void Disconnected(string id)
        {
            PineconeLogManager.Instance.EnterInfoLog(string.Format("PineconeClient.Disconnected({0})", id));

            _onDisconnected?.Invoke(id);
        }

        #endregion

        #endregion

        #region Event Binded Functions

        #region Incoming Message Binding Functions

        protected virtual void WelcomeMessage(string id, string message)
        {
            Debug.Log(string.Format("PineconeClient.WelcomeMessageReceived({0}, {1})", id, message));
        }

        #endregion

        #endregion
    }
}