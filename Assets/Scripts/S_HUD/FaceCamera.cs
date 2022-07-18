using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform mainCameraTransform;

    void Start()
    {
        mainCameraTransform = GameObject.FindGameObjectWithTag("MiniMapCamera").transform;
    }

    void LateUpdate()
    {
        LookAtCamera();
    }

    void LookAtCamera()
    {
       UiHelper.Tweens.RotateWorldUiTowardsCamera(transform,mainCameraTransform);
    }
}
