using PineconeGames.Core.Logs;
using PineconeGames.Core.Patterns;
using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.ClientSide;
using PineconeGames.Network.Core.Messages.ServerSide;
using System;
using System.Collections.Generic;

namespace PineconeGames.Network.Core.Messages
{
    public class IncomingMessageManager : Singleton<IncomingMessageManager>
    {
        #region Events

        public WelcomeMessageEventHandler OnWelcomeMessage;
        public WelcomeReceivedMessageEventHandler OnWelcomeReceivedMessage;

        #endregion

        #region Variables

        protected Dictionary<PacketType, PacketHandler> _packetHandleDict;

        #endregion

        #region Constructors

        public IncomingMessageManager()
        {
            InitializePacketHandleDictionary();
        }

        #endregion

        #region Public Functions

        public virtual bool HandleIncomingMessage(int packetType, Packet packet)
        {
            bool result = false;

            try
            {
                if (packetType > 0)
                {
                    PacketType packetContentType = (PacketType)(packetType);

                    if (_packetHandleDict.ContainsKey(packetContentType))
                    {
                        _packetHandleDict[packetContentType]?.Invoke(packet);

                        result = true;
                    }
                }
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("IncomingMessageManager.HandleIncomingMessage({0}) failed. Reason: {1}", packetType, ex.Message), ex);
            }

            return result;
        }

        #endregion

        #region Protected Functions

        #region Initialize Functions

        protected virtual void InitializePacketHandleDictionary()
        {
            _packetHandleDict = new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.Welcome, WelcomeMessage },
                { PacketType.WelcomeReceived, WelcomeReceivedMessage }
            };
        }

        #endregion

        #region Message Handle Functions

        protected void WelcomeMessage(Packet packet)
        {
            string id = packet.ReadString();
            string message = packet.ReadString(); 

            OnWelcomeMessage?.Invoke(id, message);
        }

        protected void WelcomeReceivedMessage(Packet packet)
        {
            string id = packet.ReadString();
            string username = packet.ReadString();

            OnWelcomeReceivedMessage?.Invoke(id, username);
        }

        #endregion

        #endregion
    }
}