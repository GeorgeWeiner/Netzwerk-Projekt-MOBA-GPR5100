using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Vector3SqrdDistance
{
    // Vector3.magnitude ?
    
    static public float GetSqredDistance(this Vector3 shaft, Vector3 tip)
    {
        return Mathf.Sqrt((tip - shaft).sqrMagnitude);
    }

    public static float Distance(this Vector3 shaft, Vector3 tip)
    {
        return Vector3.Distance(shaft, tip);
    }
}
