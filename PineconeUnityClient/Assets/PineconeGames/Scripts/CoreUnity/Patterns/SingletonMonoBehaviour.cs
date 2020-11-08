using UnityEngine;

namespace PineconeGames.CoreUnity.Patterns
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Variables

        public static T Instance
        {
            get
            {
                lock(_padlock)
                {
                    if (_instance == null)
                    {
                        Debug.LogWarning(string.Format("Instance is null for {0}. Creating new instance...", typeof(T).ToString()));

                        _instance = GameObject.FindObjectOfType<T>();
                        
                        if (_instance == null)
                        {
                            GameObject instanceGO = new GameObject(typeof(T).ToString());
                            _instance = instanceGO.AddComponent<T>();

                            DontDestroyOnLoad(instanceGO);
                        }
                    }

                    return _instance;
                }
            }
        }

        private static T _instance = null;

        private static object _padlock = new object();

        #endregion
    }
}