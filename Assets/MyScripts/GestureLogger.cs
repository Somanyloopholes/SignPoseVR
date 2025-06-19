using UnityEngine;

public class GestureLogger : MonoBehaviour
{
    // Must be public, void, no parameters
    public void LogPerformed()
    {
        Debug.Log($"[GestureLogger] Pose performed on '{gameObject.name}'");
    }

    public void LogEnded()
    {
        Debug.Log($"[GestureLogger] Pose ended on '{gameObject.name}'");
    }
}
