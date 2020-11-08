using PineconeGames.Client.Core.Clients;
using PineconeGames.Client.Core.ConnectionTypes;
using PineconeGames.CoreUnity.Patterns;
using PineconeGames.Network.Core.Messages;
using UnityEngine;

namespace PineconeGames.CoreUnity.Network.Client
{
    public class PineconeClient : SingletonMonoBehaviour<PineconeClient>
    {
        #region Events

        protected ConnectionResultEventHandler _onConnectionResultReceived;

        #endregion

        #region Variables

        protected TCPClient _tcpClient;

        #endregion

        #region Unity Functions

        protected virtual void Awake()
        {
            _tcpClient = new TCPClient("Dummy User");

            BindOnIncomingMessageEvents();
        }

        #endregion

        #region Public Functions

        public void Connect(string ip, int port, ConnectionResultEventHandler onConnectionResultReceived)
        {
            _onConnectionResultReceived = onConnectionResultReceived;

            _tcpClient.Connect(ip, port, onConnectionResultReceived);
        }

        public void SendWelcomeReceivedMessage(string id, string username)
        {
            _tcpClient.SendWelcomeReceivedMessage(id, username);
        }

        #endregion

        #region Protected Functions

        #region Initialize Functions

        protected virtual void BindOnIncomingMessageEvents()
        {
            IncomingMessageManager.Instance.OnWelcomeMessage += WelcomeMessage;
        }

        #endregion

        #region Connect Functions

        protected virtual void ConnectionResultReceived(bool result)
        {
            _onConnectionResultReceived?.Invoke(result);
        }

        #endregion

        #endregion

        #region Event Binded Functions

        #region Incoming Message Binding Functions

        protected virtual void WelcomeMessage(string id, string message)
        {
            Debug.Log(string.Format("PineconeClient.WelcomeMessageReceived({0}, {1})", id, message));

            SendWelcomeReceivedMessage(id, "Mert Ali Gözcü");
        }

        #endregion

        #endregion
    }
}