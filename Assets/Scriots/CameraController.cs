using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    public float zoomSpeed = 10f;
    public float rotateSpeed = 250f;
    public float minZoom = -50f;
    public float maxZoom = 50f;

    private Vector3 dragOrigin;
    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(2) || (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0)))
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(2) || (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0)))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-difference.x * panSpeed, -difference.y * panSpeed, 0);
            transform.Translate(move, Space.Self);
            dragOrigin = Input.mousePosition;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera camera = Camera.main;

        if (camera.orthographic)
        {
            camera.orthographicSize -= scroll * zoomSpeed;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
        }
    }
}
