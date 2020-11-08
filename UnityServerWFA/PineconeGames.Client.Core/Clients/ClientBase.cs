using PineconeGames.Client.Core.ConnectionTypes;

namespace PineconeGames.Client.Core.Clients
{
    public abstract class ClientBase
    {
        #region Variables

        public string ID
        {
            get
            {
                return _tcp?.ID;
            }
        }

        protected TCP _tcp;

        #endregion

        #region Constructors

        public ClientBase(string id)
        {
            _tcp = new TCP(id);
        }

        #endregion

        #region Public Functions

        public virtual void Connect(string ip, int port, ConnectionResultEventHandler onConnectionResultReceived)
        {
            _tcp.Connect(ip, port, onConnectionResultReceived);
        }

        #endregion
    }
}