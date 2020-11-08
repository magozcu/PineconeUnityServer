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

    #endregion

    public class TCP
    {
        #region Events

        protected ConnectionResultEventHandler _onConnectionResultReceived; 

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

        protected TcpClient _socket;
        protected string _id;
        protected NetworkStream _stream;
        protected byte[] _receiveBuffer;
        protected Packet _receivedData;

        private static int _dataBufferSize = 4096;

        #endregion

        #region Constructors

        public TCP (string id)
        {
            _id = id;

            _receivedData = new Packet();
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

        public virtual bool Connect(TcpClient socket)
        {
            bool result = false;

            try
            {
                _socket = socket;
                _socket.ReceiveBufferSize = DataBufferSize;
                _socket.SendBufferSize = DataBufferSize;

                _stream = _socket.GetStream();
                _receiveBuffer = new byte[DataBufferSize];

                _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

                result = true;
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.Connect() failed. Reason: {0}", ex.Message), ex);
            }

            return result;
        }

        public virtual void Connect(string ip, int port, ConnectionResultEventHandler onConnectionResultReceived)
        {
            try
            {
                _socket = new TcpClient()
                {
                    ReceiveBufferSize = DataBufferSize,
                    SendBufferSize = DataBufferSize
                };

                _receiveBuffer = new byte[DataBufferSize];
                _socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), _socket);

                _onConnectionResultReceived = onConnectionResultReceived;
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.Connect({0}, {1}) failed. Reason: {2}", ip, port.ToString(), ex.Message), ex);
                _onConnectionResultReceived?.Invoke(false);
            }
        }

        public virtual bool SendData(Packet packet)
        {
            bool result = false;

            try
            {
                if (_socket != null)
                {
                    _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.SendData({0}) failed. Reason: {1}", packet, ex.Message), ex);
            }

            return result;
        }

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
                int byteLength = _stream.EndRead(result);
                if (byteLength <= 0)
                {
                    //TODO Disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(_receiveBuffer, data, byteLength);

                _receivedData.Reset(HandleData(data));
                ListenForReceivedData();
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.ReceiveCallback() failed. Reason: {0}", ex.Message), ex);
                //TODO Disconnect
            }
        }

        protected virtual void ConnectCallback(IAsyncResult result)
        {
            bool isConnected = false;

            try
            {
                _socket.EndConnect(result);

                if (!_socket.Connected)
                {
                    return;
                }

                _stream = _socket.GetStream();

                ListenForReceivedData();
                isConnected = true;
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.ConnectCallback() failed. Reason: {0}", ex.Message), ex);
            }

            _onConnectionResultReceived?.Invoke(isConnected);
        } 

        #endregion
    }
}