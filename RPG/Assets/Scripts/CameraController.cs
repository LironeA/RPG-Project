using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class CameraController : MonoBehaviour
{
    [Foldout("Camera", true)] [AutoProperty]
    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;
    public bool lockCursor = true;


    [Foldout("Zoom", true)] public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    private bool isZoomed = false;
    private float yaw = 0.0f;
    private float pitch = 0.0f;


    // Start is called before the first frame update
    private void Start()
    {
        if (lockCursor) Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        // Control camera movement
        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            else
                // Inverted Y
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }


        if (enableZoom)
        {
            // Changes isZoomed when key is pressed
            // Behavior for toogle zoom
            if (Input.GetKeyDown(zoomKey) && !holdToZoom)
            {
                if (!isZoomed)
                    isZoomed = true;
                else
                    isZoomed = false;
            }

            // Changes isZoomed when key is pressed
            // Behavior for hold to zoom
            if (holdToZoom)
            {
                if (Input.GetKeyDown(zoomKey))
                    isZoomed = true;
                else if (Input.GetKeyUp(zoomKey)) isZoomed = false;
            }

            // Lerps camera.fieldOfView to allow for a smooth transistion
            if (isZoomed)
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            else if (!isZoomed)
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
        }
    }
}