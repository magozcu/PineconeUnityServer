using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.ClientSide;

namespace PineconeGames.Client.Core.Clients
{
    public class TCPClient : ClientBase
    {
        #region Constructors

        public TCPClient(string id) : base(id)
        {

        }

        #endregion

        #region Public Functions

        public void SendWelcomeReceivedMessage(string id, string username)
        {
            Packet welcomeReceivedMessage = new WelcomeReceivedMessage(id, username).GenerateTCPData();
            _tcp.SendData(welcomeReceivedMessage);
        }

        #endregion
    }
}