using PineconeGames.Core.Logs;
using PineconeGames.Network.Core.Data;
using System;

namespace PineconeGames.Network.Core.Messages.Base
{
    public abstract class MessageBase
    {
        #region Variables

        public PacketType PacketContentType
        {
            get
            {
                return _packetContentType;
            }
        }

        protected Packet _packet;
        protected PacketType _packetContentType;

        #endregion

        #region Constructors

        public MessageBase(PacketType packetContentType)
        {
            try
            {
                _packet = new Packet((int)(packetContentType));
                _packetContentType = packetContentType;
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("MessageBase({0}) failed. Reason: {1}", packetContentType.ToString(), ex.Message), ex);
            }
        }

        #endregion

        #region Public Functions

        public virtual Packet GenerateTCPData()
        {
            _packet.WriteLength();
            return _packet;
        }

        #endregion
    }
}