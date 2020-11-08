using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.Base;

namespace PineconeGames.Network.Core.Messages.ServerSide
{
    #region Event Handlers

    public delegate void WelcomeMessageEventHandler(string id, string message);

    #endregion

    public class WelcomeMessage : MessageBase
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

        public WelcomeMessage(string id, string message) : base(PacketType.Welcome)
        {
            _packet.Write(id);
            _packet.Write(message); 
        }

        #endregion
    }
}