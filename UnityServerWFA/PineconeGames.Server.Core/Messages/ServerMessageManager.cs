using PineconeGames.Client.Core.ConnectionTypes;
using PineconeGames.Core.Patterns;
using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.ServerSide;

namespace PineconeGames.Server.Core.Messages
{
    public class ServerMessageManager : Singleton<ServerMessageManager>
    {
        #region Public Functions

        public void SendWelcomeMessageToClient(string id, string message, TCP client)
        {
            Packet welcomePacket = new WelcomeMessage(id, message).GenerateTCPData();
            client.SendData(welcomePacket);
        }

        #endregion
    }
}