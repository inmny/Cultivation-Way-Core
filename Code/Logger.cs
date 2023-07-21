using UnityEngine;

namespace Cultivation_Way;

public class Logger
{
    public static void Log(string msg)
    {
        Debug.Log("[INFO] " + msg);
    }

    public static void Warn(string msg)
    {
        Debug.LogWarning("[WARN] " + msg);
    }

    public static void Error(string msg)
    {
        Debug.LogError("[ERROR] " + msg);
    }
}