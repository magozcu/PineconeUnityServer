using PineconeGames.Network.Core.Data;
using PineconeGames.Network.Core.Messages.Base;
using System.Linq;

namespace PineconeGames.Server.Core.Servers
{
    public class MainServer : ServerBase
    {
        #region Constructors

        public MainServer(int port, int maxClients) : base (port, maxClients)
        {

        }

        public MainServer(int port, int maxClients, string serverName) : base(port, maxClients, serverName)
        {

        }

        #endregion
    }
}