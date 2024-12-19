using JetBrains.Annotations;
using System;
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

    public void FocusOn(Transform target, float focusSpeed = 2f)
    {
        Vector3 targetPosition = new Vector3(
            target.position.x + offsetX,
            target.position.y + offsetY,
            target.position.z + offsetZ
        );
        StartCoroutine(SmoothFocus(targetPosition, focusSpeed));
    }

    private IEnumerator SmoothFocus(Vector3 targetPosition, float focusSpeed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, focusSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
