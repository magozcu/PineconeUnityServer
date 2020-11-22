using PineconeGames.Client.Core.ConnectionTypes;
using PineconeGames.Core.Patterns;
using PineconeGames.Network.Core.Data;

namespace PineconeGames.Client.Core.Messages
{
    public class ClientMessageManager : Singleton<ClientMessageManager>
    {
        #region Variables

        protected TCP _connection;

        #endregion

        #region Public Functions

        #region Initialize Functions

        public void SetConnection(TCP connection)
        {
            _connection = connection;
        }

        #endregion

        #region Message Functions

        public void SendWelcomeReceivedMessageToServer(string id, string username)
        {
            using (Packet packet = new Packet())
            {
                packet.Write(id);
                packet.Write(username);

                _connection.SendData(packet);
            }
        }

        public void SendReadyStatusMessageToServer(string id, bool readyStatus)
        {
            using (Packet packet = new Packet())
            {
                
            }
        }

        #endregion

        #endregion
    }
}