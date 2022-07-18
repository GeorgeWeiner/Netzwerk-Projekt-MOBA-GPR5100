using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class UiHelper 
{
     public class Tweens : MonoBehaviour
    {
        static public void RotateWorldUiTowardsCamera(Transform uiTransform,Transform mainCam)
        {
            uiTransform.LookAt(uiTransform.position + mainCam.rotation * Vector3.forward,
                mainCam.rotation * Vector3.up);
        }
    }
}
