using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.Base;

namespace PineconeGames.Network.Core.Messages.ClientSide
{
    #region Event Handlers

    public delegate void WelcomeReceivedMessageEventHandler(string id, string username);

    #endregion

    public class WelcomeReceivedMessage : ClientMessageBase
    {
        #region Constructors

        public WelcomeReceivedMessage(string id, string username) : base(id, PacketType.WelcomeReceived)
        {
            _packet.Write(username);
        }

        #endregion
    }
}