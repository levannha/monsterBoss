using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    private float dragSpeed = 5f; // Tốc độ kéo
    private Vector3 lastMousePosition;

    public float zoomSpeed = 10f; // Tốc độ zoom
    public float minZoom = 5f;    // Giới hạn nhỏ nhất
    public float maxZoom = 50f;   // Giới hạn lớn nhất
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (!UnitSelectManager.Instance.attackCursorVisible)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel"); // Nhận tín hiệu lăn chuột
            cam.fieldOfView -= scroll * zoomSpeed; // Điều chỉnh góc nhìn camera
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom); // Giới hạn zoom
            if (Input.GetMouseButtonDown(1)) // Khi bắt đầu chạm vào màn hình
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(1)) // Khi giữ và kéo
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                transform.position -= new Vector3(delta.x * -dragSpeed * Time.deltaTime, transform.position.y, delta.y * -dragSpeed * Time.deltaTime);
                lastMousePosition = Input.mousePosition;
            }
        }
    }
}
