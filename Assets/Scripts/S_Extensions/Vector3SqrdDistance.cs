using UnityEngine;

public static class Vector3SqrdDistance
{
    // Vector3.magnitude ?
    
    public static float GetSqredDistance(this Vector3 shaft, Vector3 tip)
    {
        return Mathf.Sqrt((tip - shaft).sqrMagnitude);
    }

    public static float Distance(this Vector3 shaft, Vector3 tip)
    {
        return Vector3.Distance(shaft, tip);
    }
}
