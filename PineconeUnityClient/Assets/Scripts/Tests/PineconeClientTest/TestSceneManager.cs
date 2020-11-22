using PineconeGames.CoreUnity.Network.Client;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.PineconeClientTest
{
    public class TestSceneManager : MonoBehaviour
    {
        #region UI Elements

        public Button BtnConnect;
        public Button BtnDisconnect;

        #endregion

        #region Variables

        private PineconeClient _client
        {
            get
            {
                return PineconeClient.Instance;
            }
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            BtnConnect?.onClick.AddListener(Connect);
            BtnDisconnect.onClick.AddListener(Disconnect);
        }

        #endregion

        #region Event Binded Functions

        private void Connect()
        {
            _client.Connect("192.168.1.22", 5056, "Test User", null, null);
        }

        private void Disconnect()
        {
            _client.Disconnect();
        }

        #endregion
    }
}