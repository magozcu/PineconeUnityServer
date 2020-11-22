using PineconeGames.Core.Logs;
using PineconeGames.Network.Core.Data;
using System;
using System.Net;
using System.Net.Sockets;

namespace PineconeGames.Client.Core.ConnectionTypes
{
    public class TCP : ConnectionBase
    {
        #region Variables

        public override bool IsConnected => base.IsConnected && _isConnected;

        protected TcpClient _socket;
        protected bool _isConnected;

        #endregion

        #region Constructors

        public TCP (string id) : base (id)
        {
            _isConnected = false;
        }

        public TCP (string id, DisconnectedEventHandler onDisconnected) : base(id, onDisconnected)
        {
            _isConnected = false;
        }

        #endregion

        #region Public Functions   

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
                _isConnected = result;
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
                _isConnected = true;
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.Connect({0}, {1}) failed. Reason: {2}", ip, port.ToString(), ex.Message), ex);
                _onConnectionResultReceived?.Invoke(false);
            }
        }

        public override void Disconnect()
        {
            base.Disconnect();

            _socket?.Close();

            _stream = null;
            _receivedData = null;
            _receiveBuffer = null;
            _socket = null;

            _isConnected = false;
        }

        public override bool SendData(Packet packet)
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

        public override int GetLocalEndPoint()
        {
            int result = (-1);

            if (_socket != null)
            {
                try
                {
                    result = ((IPEndPoint)(_socket.Client.LocalEndPoint)).Port;
                }
                catch(Exception ex)
                {
                    PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.GetLocalEndPoint() failed. Reason: {0}", ex.Message), ex);
                }
            }

            return result;
        }

        #endregion

        #region Protected Functions  

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

        #region Event Binded Functions

        protected virtual void Disconnected(string id)
        {
            _isConnected = false;

            _onDisconnected?.Invoke(id);    
        }

        #endregion
    }
}