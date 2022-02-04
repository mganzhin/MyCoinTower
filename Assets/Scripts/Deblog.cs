using UnityEngine;

public class Deblog 
{
    public static void Log(bool flag, string message)
    {
        if (flag)
        {
            Debug.Log(message);
        }
    }
}
