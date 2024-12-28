using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    public float zoomSpeed = 10f;
    public float rotateSpeed = 250f;
    public float minZoom = -50f;
    public float maxZoom = 50f;
    public float offsetX = 0;
    public float offsetY = 0;
    public float offsetZ = 0;

    private Vector3 dragOrigin;

    private float currentRotationAngle = 0f;

    void Update()
    {
        HandlePan();
        HandleZoom();
        HandleRotation();
        FollowCurrentBeing();
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

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRotationAngle -= 90f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentRotationAngle += 90f;
        }
    }

    void FollowCurrentBeing()
    {
        Vector3 currentBeingPos;
        if (ActionManager.instance?.currentBeing?.transform == null)
        {
            Debug.LogWarning("CameraController.FollowCurrentBeing could not find currentBeingPos");
            return;
        }
        currentBeingPos = ActionManager.instance.currentBeing.transform.position;

        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);
        Vector3 offset = rotation * new Vector3(offsetX, offsetY, offsetZ);
        Vector3 targetPosition = currentBeingPos + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * panSpeed);
        transform.LookAt(currentBeingPos);
    }
}
