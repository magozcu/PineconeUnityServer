using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.Base;

namespace PineconeGames.Network.Core.Messages.ClientSide
{
    #region Event Handlers

    public delegate void WelcomeReceivedMessageEventHandler(string id, string username);

    #endregion

    public class WelcomeReceivedMessage : MessageBase
    {
        #region Variables

        public int ID
        {
            get
            {
                return _id;
            }
        }
        public string Message
        {
            get
            {
                return _message;
            }
        }

        protected int _id;
        protected string _message;

        #endregion

        #region Constructors

        public WelcomeReceivedMessage(string id, string username) : base(PacketType.WelcomeReceived)
        {
            _packet.Write(id);
            _packet.Write(username);
        }

        #endregion
    }
}