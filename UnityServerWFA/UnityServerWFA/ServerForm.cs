using PineconeGames.Client.Core.Clients;
using PineconeGames.Core.Logs;
using PineconeGames.Core.Threads;
using PineconeGames.Network.Core.Messages;
using PineconeGames.Server.Core.Servers;
using System;
using System.Threading;
using System.Windows.Forms;

namespace UnityServerWFA
{
    public partial class ServerForm : Form
    {
        #region Temp Variables

        private TCPClient _testClient;

        #endregion

        #region Variables

        protected MainServer _mainServer;

        #endregion

        #region Constructors

        public ServerForm()
        {
            InitializeComponent();
            InitializeLogging();
            InitializeClient();
        }

        #endregion

        #region Protected Functions

        #region Log Binding Functions

        protected virtual void InitializeLogging()
        {
            PineconeLogManager.Instance.BindOnLogEvents(InfoLog, WarningLog, ErrorLog);
        }

        protected virtual void InfoLog(string message)
        {
            Console.WriteLine(string.Format("Info: {0}", message));
            lblInfo.BeginInvoke((MethodInvoker)delegate () { lblInfo.Text = message; });
        }

        protected virtual void WarningLog(string message)
        {
            Console.WriteLine(string.Format("Warning: {0}", message));
            lblWarning.BeginInvoke((MethodInvoker)delegate () { lblWarning.Text = message; });
        }

        protected virtual void ErrorLog(string message, Exception ex)
        {
            Console.WriteLine(string.Format("Error: {0}. Exception: {1}", message, ex.Message));
            lblError.BeginInvoke((MethodInvoker)delegate () { lblError.Text = message; });
        }

        #endregion

        #region Client Binding Functions

        protected void InitializeClient()
        {
            PineconeThreadManager.Instance.BindOnEvents(ExecuteAction);
            IncomingMessageManager.Instance.OnWelcomeMessage += WelcomeMessageReceived;
        }

        protected void ClientTest()
        {
            _testClient = new TCPClient("Mert Ali Gozcu");
            _testClient.Connect("127.0.0.1", 5056, ConnectionResultReceived);
        }

        protected void ConnectionResultReceived(bool result)
        {
            InfoLog(result.ToString());
        }

        protected void ExecuteAction(Action action)
        {
            action?.Invoke();
        }

        protected void WelcomeMessageReceived(string id, string message)
        {
            string userId = id;
            string userMessage = message;

            _testClient.SendWelcomeReceivedMessage(id, "Mert Ali GÖZCÜ");
        }

        #endregion

        #endregion

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            _mainServer = new MainServer(5056, 2, "Main Server");

            bool serverStarted = _mainServer.StartServer();

            InfoLog(string.Format("Server Started Result: {0}", serverStarted.ToString()));
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            bool serverStopped = _mainServer.StopServer();

            InfoLog(string.Format("Server Stopped Result: {0}", serverStopped.ToString()));
        }

        private void btnClientConnect_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ClientTest)).Start();
        }

        private void WelcomeReceivedMessage(string id, string username)
        {
            string _id = id;
            string _username = username;
        }
    }
}
