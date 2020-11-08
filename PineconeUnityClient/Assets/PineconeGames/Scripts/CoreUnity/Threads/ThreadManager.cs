using PineconeGames.CoreUnity.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PineconeGames.CoreUnity.Threads
{
    public class ThreadManager : SingletonMonoBehaviour<ThreadManager>
    {
        private static readonly List<Action> _executeOnMainThread = new List<Action>();
        private static readonly List<Action> _executeCopiedOnMainThread = new List<Action>();
        private static bool _actionToExecuteOnMainThread = false;

        private void Update()
        {
            UpdateMain();
        }

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="_action">The action to be executed on the main thread.</param>
        public void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Debug.Log("No action to execute on main thread!");
                return;
            }

            lock (_executeOnMainThread)
            {
                _executeOnMainThread.Add(_action);
                _actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        public void UpdateMain()
        {
            if (_actionToExecuteOnMainThread)
            {
                _executeCopiedOnMainThread.Clear();
                lock (_executeOnMainThread)
                {
                    _executeCopiedOnMainThread.AddRange(_executeOnMainThread);
                    _executeOnMainThread.Clear();
                    _actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < _executeCopiedOnMainThread.Count; i++)
                {
                    _executeCopiedOnMainThread[i]();
                }
            }
        }
    }
}