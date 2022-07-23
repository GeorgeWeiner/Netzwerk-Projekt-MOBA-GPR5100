using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        mainCameraTransform = GameObject.FindGameObjectWithTag("MiniMapCamera").transform;
    }

    private void LateUpdate()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
       UiHelper.Tweens.RotateWorldUiTowardsCamera(transform,mainCameraTransform);
    }
}
