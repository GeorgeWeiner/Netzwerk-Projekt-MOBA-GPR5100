using UnityEngine;

public static class UiHelper 
{
     public class Tweens : MonoBehaviour
    {
        public static void RotateWorldUiTowardsCamera(Transform uiTransform,Transform mainCam)
        {
            uiTransform.LookAt(uiTransform.position + mainCam.rotation * Vector3.forward,
                mainCam.rotation * Vector3.up);
        }
    }
}
