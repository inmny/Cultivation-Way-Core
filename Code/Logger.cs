using UnityEngine;

namespace Cultivation_Way;

public class Logger
{
    public static void Log(string msg)
    {
        Debug.Log(msg);
    }

    public static void Warn(string msg)
    {
        Debug.LogWarning(msg);
    }

    public static void Error(string msg)
    {
        Debug.LogError(msg);
    }
}