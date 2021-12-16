using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAndPan : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 10f;
    [SerializeField]
    private float zoomSpeed = 0.1f;

    [SerializeField]
    private float[] xBounds = new float[] { -10, 10 };
    [SerializeField]
    private float[] zBounds = new float[] { -10, 10 };
    [SerializeField]
    private float[] zoomBounds = new float[] { 10, 85 };


    private Camera cam;
    private Vector3 lastPanPosition;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMouse();
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastPanPosition = Input.mousePosition;
            Debug.Log("Button Down");
        } else if (Input.GetMouseButton(1))
        {
            Pan(Input.mousePosition);
            Debug.Log("Panning!");
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scroll, zoomSpeed);
    }

    void Pan(Vector3 panPosition) {
        // Determine how far to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - panPosition);
        Vector3 move = new Vector3(offset.x * panSpeed, 0, offset.y * panSpeed);

        // Perform the movement
        cam.transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds
        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(pos.x, xBounds[0], xBounds[1]);
        pos.z = Mathf.Clamp(pos.z, zBounds[0], zBounds[1]);
        cam.transform.position = pos;

        // Cache the position
        lastPanPosition = panPosition;
    }

    void Zoom (float offset, float speed) {
        if (offset == 0) {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - offset * speed, zoomBounds[0], zoomBounds[1]);
    }
}
