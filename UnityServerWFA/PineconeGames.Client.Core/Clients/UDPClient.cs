using PineconeGames.Client.Core.ConnectionTypes;
using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.ClientSide;

namespace PineconeGames.Client.Core.Clients
{
    public class UDPClient
    {
        #region Variables

        protected UDP _udp;

        #endregion

        #region Constructors

        public UDPClient(string id)
        {
            _udp = new UDP(id);
        }

        #endregion

        #region Public Functions

        #region Message Functions

        public void SendWelcomeReceivedMessage(string id, string username)
        {
            Packet welcomeReceivedMessage = new WelcomeReceivedMessage(id, username).GenerateTCPData();
            _udp.SendData(welcomeReceivedMessage);
        }

        #endregion

        #region Util Functions

        public void Connect(string ip, int port, int localPort)
        {
            if (_udp != null)
            {
                _udp.Connect(ip, port, localPort);
            }
        }

        #endregion

        #endregion
    }
}