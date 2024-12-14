using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PixelPerfectCamera))]
public class PixelPerfectCameraEnabler: MonoBehaviour
{
    private Camera _camera;
    private float _unscaledCameraSize = 4.5f;
    private PixelPerfectCamera _pixelPerfectCamera;


    private void Start()
    {
        _camera = GetComponent<Camera>();
        _unscaledCameraSize = _camera.orthographicSize;
        
        _pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        Settings.AddAndInvokeModificationCallback(ApplyPixelPerfectCameraSetting);
    }
    private void OnDestroy() => Settings.Modified -= ApplyPixelPerfectCameraSetting;

    private void ApplyPixelPerfectCameraSetting()
    {
        _pixelPerfectCamera.enabled = Settings.PixelPerfectCameraEnabled;
        _camera.orthographicSize = _unscaledCameraSize;
    }
}