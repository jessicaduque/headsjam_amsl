using UnityEngine;
namespace Utils.Singleton
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T Instance;
        public static T I
        {
            get
            {
                if (Instance == null)
                {
                    Instance = FindAnyObjectByType<T>();
                    if (Instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        Instance = obj.AddComponent<T>();
                    }
                }
                return Instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}