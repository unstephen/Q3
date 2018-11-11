using System.Reflection;
public class Singleton<T> where T : class, new()
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new T();

                MethodInfo method = typeof(T).GetMethod("Init");
                if (method != null)
                {
                    method.Invoke(s_instance, null);
                }
            }
            return s_instance;
        }
    }
}