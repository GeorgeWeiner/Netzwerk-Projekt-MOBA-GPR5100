using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Vector3SqrdDistance
{
    static public float GetSqredDistance(this Vector3 shaft, Vector3 tip)
    {
        return Mathf.Sqrt((tip - shaft).sqrMagnitude);
    }
}
