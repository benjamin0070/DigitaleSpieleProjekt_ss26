using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private Camera currentCamera;

    void Awake() => Instance = this;

    public void SwitchCamera(Camera newCam)
    {
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);

        newCam.gameObject.SetActive(true);
        currentCamera = newCam;
    }
}
