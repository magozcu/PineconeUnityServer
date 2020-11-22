using PineconeGames.Core.Logs;
using PineconeGames.Core.Threads;
using PineconeGames.CoreUnity.Patterns;
using PineconeGames.CoreUnity.Threads;
using System;
using UnityEngine;

namespace PineconeGames.CoreUnity
{
    public class PineconeMasterManager : SingletonMonoBehaviour<PineconeMasterManager>
    {
        #region Variables

        protected bool _isInitialized = false;

        #endregion

        #region Unity Functions

        protected virtual void Awake()
        {
            Initialize();
        }

        #endregion

        #region Public Functions

        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                PineconeLogManager.Instance.BindOnLogEvents(InfoLog, WarningLog, ErrorLog);
                PineconeThreadManager.Instance.BindOnEvents(ThreadManager.Instance.ExecuteOnMainThread);

                _isInitialized = true;
            }
        }

        #endregion

        #region Event Binded Functions

        #region Log Binding Functions

        private void InfoLog(string message)
        {
            Debug.Log("Info Log: " + message);
        }

        private void WarningLog(string message)
        {
            Debug.LogWarning("Warning Log: " + message);
        }

        private void ErrorLog(string message, Exception ex)
        {
            Debug.LogError("Error Log: " + message + ". Exception: " + ex.Message);
        }

        #endregion

        #endregion
    }
}