using PineconeGames.Client.Core.Clients;
using PineconeGames.Core.Logs;
using PineconeGames.Core.Threads;
using PineconeGames.CoreUnity.Network.Client;
using PineconeGames.CoreUnity.Threads;
using PineconeGames.Network.Core.Messages;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PineconeGames.UnityClient.Tests
{
    public class TestClient : MonoBehaviour
    {
        #region Variables

        public Button BtnConnect;

        #endregion

        #region Unity Functions

        private void Start()
        {
            BtnConnect.onClick.AddListener(Connect);
        }

        #endregion

        #region Private Functions

        private void Connect()
        {
            PineconeClient.Instance.Connect("192.168.1.22", 5056, null);
        }

        #endregion
    }
}