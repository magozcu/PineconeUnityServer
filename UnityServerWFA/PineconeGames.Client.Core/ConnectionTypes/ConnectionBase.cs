using PineconeGames.Core.Logs;
using PineconeGames.Core.Threads;
using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages;
using System;
using System.Net.Sockets;

namespace PineconeGames.Client.Core.ConnectionTypes
{
    #region Event Handler

    public delegate void ConnectionResultEventHandler(bool connected);
    public delegate void ConnectedEventHandler(string id);
    public delegate void DisconnectedEventHandler(string id);

    #endregion

    public abstract class ConnectionBase
    {
        #region Events

        protected ConnectionResultEventHandler _onConnectionResultReceived;
        protected DisconnectedEventHandler _onDisconnected;

        #endregion

        #region Variables

        public static int DataBufferSize
        {
            get
            {
                return _dataBufferSize;
            }
        }

        public string ID
        {
            get
            {
                return _id;
            }
        }

        public virtual bool IsConnected
        {
            get
            {
                return !_isDisposed;
            }
        }

        protected static int _dataBufferSize = 4096;

        protected string _id;
        protected NetworkStream _stream;
        protected byte[] _receiveBuffer;
        protected Packet _receivedData;
        protected bool _isDisposed = false;

        #endregion

        #region Constructors

        public ConnectionBase(string id)
        {
            _id = id;

            _receivedData = new Packet();
        }

        public ConnectionBase(string id, DisconnectedEventHandler onDisconnected) : this(id)
        {
            _onDisconnected = onDisconnected;
        }

        #endregion

        #region Public Functions

        #region Static Functions

        public static void SetDataBufferSize(int dataBufferSize = 4096)
        {
            if (dataBufferSize > 0)
            {
                _dataBufferSize = dataBufferSize;
            }
        }

        #endregion

        #region Instance Functions

        public virtual void SetId(string id)
        {
            _id = id;
        }

        public virtual void Disconnect()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                PineconeLogManager.Instance.EnterInfoLog("ConnectionBase Disconnected from server!");

                _onDisconnected?.Invoke(ID);
            }
        }

        public abstract bool SendData(Packet packet); 

        public abstract int GetLocalEndPoint();

        #endregion

        #endregion

        #region Protected Functions

        protected virtual void ListenForReceivedData()
        {
            _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        protected virtual bool HandleData(byte[] data)
        {
            if (data != null)
            {
                try
                {
                    int packetLength = 0;
                    _receivedData.SetBytes(data);

                    if (_receivedData.UnreadLength() >= 4)
                    {
                        packetLength = _receivedData.ReadInt();
                        if (packetLength <= 0)
                        {
                            return true;
                        }
                    }

                    while (packetLength > 0 && packetLength <= _receivedData.UnreadLength())
                    {
                        byte[] packetBytes = _receivedData.ReadBytes(packetLength);
                        PineconeThreadManager.Instance.ExecuteOnMainThread(() =>
                        {
                            using (Packet packet = new Packet(packetBytes))
                            {
                                int packetType = packet.ReadInt();
                                IncomingMessageManager.Instance.HandleIncomingMessage(packetType, packet);
                            }
                        });

                        packetLength = 0;
                        if (_receivedData.UnreadLength() >= 4)
                        {
                            packetLength = _receivedData.ReadInt();
                            if (packetLength <= 0)
                            {
                                return true;
                            }
                        }
                    }

                    if (packetLength <= 1)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.HandleData({0}) failed. Reason: {1}", data.Length, ex.Message), ex);
                }
            }

            return false;
        }

        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = 0;
                if (_stream != null && !_isDisposed)
                {
                    byteLength = _stream.EndRead(result);
                }

                if (byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(_receiveBuffer, data, byteLength);

                _receivedData.Reset(HandleData(data));
                ListenForReceivedData();
            }
            catch (Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("ConnectionBase.ReceiveCallback() failed. Reason: {0}", ex.Message), ex);
                Disconnect();
            }
        }

        #endregion
    }
}