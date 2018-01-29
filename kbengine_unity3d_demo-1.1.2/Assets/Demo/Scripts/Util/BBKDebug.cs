
using UnityEngine;
public class BBKDebug
{
    public static void Log(object message)
    {
    if (Const.OPEN_LOG)
        Debug.Log(message);

    }

    public static void LogWarning(object message)
    {
        if (Const.OPEN_LOG)
            Debug.LogWarning(message);


    }

    public static void LogWarning(object message,Object context)
    {
        if (Const.OPEN_LOG)
            Debug.LogWarning(message,context);
    }

    public static void LogError(object message)
    {
        if (Const.OPEN_LOG)
            Debug.LogError(message);

    }

    public static void LogError(object message, Object context)
    {
        if (Const.OPEN_LOG)
            Debug.LogError(message, context);

       
	}




    public static void BeginSample(string name)
    {
        if (Const.OPEN_LOG)
            UnityEngine.Profiling.Profiler.BeginSample(name);
    }

    public static void EndSample()
    {
        if (Const.OPEN_LOG)
            UnityEngine.Profiling.Profiler.EndSample();

    }
}