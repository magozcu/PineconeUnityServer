using PineconeGames.Core.Logs;
using PineconeGames.Network.Core.Data;
using System;
using System.Net;
using System.Net.Sockets;

namespace PineconeGames.Client.Core.ConnectionTypes
{
    public class UDP : ConnectionBase
    {
        #region Variables

        protected UdpClient _socket;
        protected IPEndPoint _ipEndPoint;

        #endregion

        #region Constructors

        public UDP(string id) : base(id)
        {
           
        }

        #endregion

        #region Public Functions

        public override bool SendData(Packet packet)
        {
            bool result = false;

            if (packet != null)
            {
                try
                {
                    if (_socket != null)
                    {
                        _socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                    }
                }
                catch(Exception ex)
                {
                    PineconeLogManager.Instance.EnterErrorLog(string.Format("UDP.SendData() failed. Reason: {0}", ex.Message), ex);
                }
            }

            return result;
        }

        public virtual bool Connect(string ip, int port, int localPort)
        {
            bool result = false;

            try
            {
                _socket = new UdpClient(localPort);
                _ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                _socket.Connect(_ipEndPoint);
                _socket.BeginReceive(ReceiveCallback, null);

                result = true;
            }
            catch (Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.Connect() failed. Reason: {0}", ex.Message), ex);
            }

            return result;
        }

        public virtual bool Connect(int serverPort, int localPort)
        {
            bool result = false;

            try
            { 
                _socket = new UdpClient(localPort);
                _ipEndPoint = new IPEndPoint(IPAddress.Any, serverPort);

                _socket.Connect(_ipEndPoint);
                _socket.BeginReceive(ReceiveCallback, null);

                result = true;
            }
            catch(Exception ex)
            {
                PineconeLogManager.Instance.EnterErrorLog(string.Format("UDP.Connect({0}, {1}) failed. Reason: {2}", serverPort, localPort, ex.Message), ex);
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
                catch (Exception ex)
                {
                    PineconeLogManager.Instance.EnterErrorLog(string.Format("TCP.GetLocalEndPoint() failed. Reason: {0}", ex.Message), ex);
                }
            }

            return result;
        }

        #endregion
    }
}