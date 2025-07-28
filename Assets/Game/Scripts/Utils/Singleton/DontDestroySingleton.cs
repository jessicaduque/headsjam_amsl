using UnityEngine;
namespace Utils.Singleton
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class DontDestroySingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            transform.SetParent(null, true);
            if (Instance == this as T)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}