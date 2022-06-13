using UnityEngine;

/// <summary>
///     This creates a basic singleton. This will destroy any new instance
///     versions created, leaving the original instance intact
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }
    protected virtual void Awake() {
        if (Instance != null) Destroy(gameObject);
       
        Instance = this as T;
    }

    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
///     This creates a singleton which will persist through scenes, in other words,
///     a persistent singleton.
/// </summary>
public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }
    protected virtual void Awake() {
        if (Instance != null) Destroy(gameObject);
       
        Instance = this as T;
        
        DontDestroyOnLoad(gameObject);
    }
}
