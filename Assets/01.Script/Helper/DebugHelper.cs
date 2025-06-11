using UnityEditor.Rendering.LookDev;
using UnityEngine;

public static class DebugHelper
{
    public static void LogError(string message, Object _context)
    {
#if UNITY_EDITOR
        Debug.LogError($"{message} {_context}");
#endif
    }

    public static void LogWarrning(string message, Object _context)
    {
#if UNITY_EDITOR
        Debug.LogWarning($"{message} {_context}");
#endif
    }

    public static void Log(string message, Object _context)
    {
#if UNITY_EDITOR
        Debug.Log($"{message} {_context}");
#endif
    }
}
