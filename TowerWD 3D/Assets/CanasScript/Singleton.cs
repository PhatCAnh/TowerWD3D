using UnityEngine;

namespace CanasSource
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region How to use

        //Create
        /*void Awake()
        {
            Singleton<"Class name">.Instance = this;
        }*/

        //Call
        //Singleton<"Class name">.Instance.

        #endregion

        #region Source

        static T s_instance;
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    Debug.LogError(typeof(T).Name + " is not a Singleton!!!");
                }
                return s_instance;
            }
            set
            {
                if (s_instance == null)
                {
                    s_instance = value;
                }
            }
        }

        public static bool HasInstance()
        {
            return s_instance != null;
        }

        public static void Clear()
        {
            s_instance = null;
        }

        void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
            }
        }
        #endregion
    }
}
