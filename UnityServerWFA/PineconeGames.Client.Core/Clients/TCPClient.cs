using PineconeGames.Client.Core.ConnectionTypes;
using PineconeGames.Core.Logs;
using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages;
using PineconeGames.Network.Core.Messages.ClientSide;

namespace PineconeGames.Client.Core.Clients
{
    public class TCPClient
    {
        #region Events

        protected DisconnectedEventHandler _onDisconnected;

        #endregion

        #region Variables

        public string ID
        {
            get
            {
                return _tcp?.ID;
            }
        }

        public bool IsConnected
        {
            get
            {
                bool result = false;

                if (_tcp != null)
                {
                    result = _tcp.IsConnected;
                }

                return result;
            }
        }

        protected TCP _tcp;

        #endregion

        #region Constructors

        public TCPClient(string id)
        {
            _tcp = new TCP(id);

            IncomingMessageManager.Instance.OnWelcomeMessage += WelcomeReceived;
        }

        public TCPClient(string id, DisconnectedEventHandler onDisconnected)
        {
            _onDisconnected = onDisconnected;
            _tcp = new TCP(id, Disconnected);

            IncomingMessageManager.Instance.OnWelcomeMessage += WelcomeReceived;
        }

        #endregion

        #region Public Functions

        #region Message Functions

        public void SendWelcomeReceivedMessage(string id, string username)
        {
            Packet welcomeReceivedMessage = new WelcomeReceivedMessage(id, username).GenerateTCPData();
            _tcp.SendData(welcomeReceivedMessage);
        }

        #endregion

        #region Util Functions

        public void Connect(string ip, int port, ConnectionResultEventHandler onConnectionResult)
        {
            if (_tcp != null)
            {
                _tcp.Connect(ip, port, onConnectionResult);
            }
        }

        public void Disconnect()
        {
            _tcp?.Disconnect();

            DestroyClient(false);
        }

        public void DestroyClient(bool disconnect)
        {
            if (disconnect)
            {
                _tcp?.Disconnect();
            }

            IncomingMessageManager.Instance.OnWelcomeMessage -= WelcomeReceived;
        }

        public int GetLocalEndPoint()
        {
            int result = (-1);

            if (_tcp != null)
            {
                result = _tcp.GetLocalEndPoint();
            }

            return result;
        }

        #endregion

        #endregion

        #region Protected Functions

        protected virtual void Disconnected(string id)
        {
            PineconeLogManager.Instance.EnterInfoLog("TCPClient.Disconnected()");

            _onDisconnected?.Invoke(id);
        }

        #endregion

        #region Event Binded Functions

        protected virtual void WelcomeReceived(string id, string message)
        {
            //Set this value from options
            //_tcp?.SetId(id);

            //SendWelcomeReceivedMessage(id, id);
            SendWelcomeReceivedMessage(id, ID);
        }

        #endregion
    }
}