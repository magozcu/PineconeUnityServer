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
        #region Unity Functions

        protected virtual void Awake()
        {
            PineconeLogManager.Instance.BindOnLogEvents(InfoLog, WarningLog, ErrorLog);
            PineconeThreadManager.Instance.BindOnEvents(ThreadManager.Instance.ExecuteOnMainThread);
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