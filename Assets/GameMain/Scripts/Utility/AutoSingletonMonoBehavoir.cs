using UnityEngine;

public class AutoSingletonMonoBehaviour<T> where T : MonoBehaviour
{
    private static MonoBehaviour s_instance;

    public static T Instance
    {
        get
        {
			if (s_instance == null && !isApplicationQuit)
				Initialize ();
            return s_instance as T;
        }
    }

	private static bool isApplicationQuit = false;
	void OnApplicationQuit()
	{
		isApplicationQuit = true;
	}

	static void Initialize()
	{
		var g = new GameObject(typeof(T).Name);
		Object.DontDestroyOnLoad( g );
		s_instance = g.AddComponent<T>();
	}

 

    private void ClearInstance()
    {
        s_instance = null;
    }
}
