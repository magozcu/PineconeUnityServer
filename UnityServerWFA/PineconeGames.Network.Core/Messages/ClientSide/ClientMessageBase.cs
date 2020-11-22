using PineconeGames.Network.Core.Data;

namespace PineconeGames.Network.Core.Messages.Base
{
    public class ClientMessageBase : MessageBase
    {
        #region Variables

        public string ID
        {
            get
            {
                return _id;
            }
        }

        protected string _id;

        #endregion

        #region Constructors

        public ClientMessageBase(string id, PacketType packetType) : base (packetType)
        {
            _id = id;

            _packet.Write(id);
        }

        #endregion
    }
}